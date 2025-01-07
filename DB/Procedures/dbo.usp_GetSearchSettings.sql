SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 24-12-2024
-- Description:	Получение датасетов настроек поиска.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[usp_GetSearchSettings]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @PriorityColumns TABLE (ColNum int, Num1 int, Num2 int)

	DECLARE @MaxFieldTypes INT
	SELECT @MaxFieldTypes = MAX(AvailableCount - CurrentCount) FROM [dbo].[T_FieldTypes]

	IF (@MaxFieldTypes > 0)
	BEGIN
		DECLARE @PriorityFieldTypes TABLE (FC int, SC int)

		-- Формируем приоритетные типы сочетаний одинарных колонок
		INSERT INTO @PriorityFieldTypes (FC, SC)
		SELECT NumberFirstColumnWithOneNumber, NumberSecondColumnWithOneNumber
		FROM [dbo].[T_FieldTypes]
		WHERE RemainCount = @MaxFieldTypes

		-- Если сформирован всего приоритеный тип одинарных колонок для увеличения производительности добавим еще один
		-- Чтобы не искать "золотые" билеты где вид билета в обоих полях одинаковый (если такие возможны вообще)
		IF (@@ROWCOUNT < 2)
			INSERT INTO @PriorityFieldTypes (FC, SC)
			SELECT TOP(1) NumberFirstColumnWithOneNumber, NumberSecondColumnWithOneNumber
			FROM [dbo].[T_FieldTypes]
			WHERE RemainCount < @MaxFieldTypes
			ORDER BY RemainCount DESC

		-- Отбираем приоритетные значения для 1 и 9 колонки
		INSERT INTO @PriorityColumns (ColNum, Num1, Num2)
		SELECT ColumnNumber, Number_1, Number_2 FROM [dbo].[T_Columns] as MAIN
		CROSS APPLY
		(
			SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
			WHERE ColumnNumber = MAIN.ColumnNumber
		) AS MaxRank
		WHERE ColumnNumber IN (1,9) AND [dbo].[udf_GetRankColumn](ColumnID) = MaxRank.Value

		-- Добавляем дополнительные вторые варианты колонок
		;WITH CheckCount AS
		(
			SELECT ColNum, COUNT(1) AS ColCount FROM @PriorityColumns
			GROUP BY ColNum
		)
		,PretendentColumns AS
		(
			-- Считаем оконкой номер строки, чтобы отбирать не больше чем по одной записи для каждой требуемой колонки
			SELECT ColumnNumber, Number_1, Number_2, ROW_NUMBER() OVER (PARTITION BY ColumnNumber ORDER BY SecondMaxRank.Value DESC) AS RN FROM [dbo].[T_Columns] AS MAIN
			-- Определяем для колонки максимальный ранг, чтобы найти второй максимум для каждой
			CROSS APPLY
			(
				SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
				WHERE ColumnNumber = MAIN.ColumnNumber
			) AS MaxRank
			-- Второй максимум каждой колонки
			CROSS APPLY
			(
				SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
				WHERE ColumnNumber = MAIN.ColumnNumber AND [dbo].[udf_GetRankColumn](ColumnID) < MaxRank.Value
			) AS SecondMaxRank
			-- Отбираем только тех, кому не хватает второго варианта, чтобы не перенасыщать выборку и гарантировано уменьшать высокоранговые колонки
			WHERE ColumnNumber IN (SELECT ColNum FROM CheckCount WHERE ColCount = 1) AND [dbo].[udf_GetRankColumn](ColumnID) = SecondMaxRank.Value
		)
		INSERT INTO @PriorityColumns (ColNum, Num1, Num2)
		SELECT ColumnNumber, Number_1, Number_2 
		FROM PretendentColumns
		WHERE RN = 1

		-- Отбираем приоритетные одинарные значения для 2-7 колонок
		-- Заводим курсорчик и по каждому приоритетному типу сформируем пул допустимых одинарных и двойных колонок
		DECLARE CUR CURSOR FORWARD_ONLY STATIC READ_ONLY FOR
			SELECT FC, SC 
			FROM @PriorityFieldTypes
			
		DECLARE @FC int, @SC int

		OPEN CUR
		FETCH NEXT FROM CUR INTO @FC, @SC

		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Собираем объединение сетов
			INSERT INTO @PriorityColumns (ColNum, Num1, Num2)
			-- Одинарных колонок
			SELECT ColumnNumber, Number_1, Number_2 FROM [dbo].[T_Columns] AS MAIN
			CROSS APPLY
			(
				SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
				WHERE ColumnNumber = MAIN.ColumnNumber AND Number_2 IS NULL
			) AS MaxRank
			WHERE ColumnNumber IN (@FC, @SC) AND Number_2 IS NULL AND [dbo].[udf_GetRankColumn](ColumnID) = MaxRank.Value
			UNION
			-- Двойных колонок
			SELECT ColumnNumber, Number_1, Number_2 FROM [dbo].[T_Columns] AS MAIN
			CROSS APPLY
			(
				SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
				WHERE ColumnNumber = MAIN.ColumnNumber AND Number_2 IS NOT NULL
			) AS MaxRank
			WHERE ColumnNumber NOT IN (@FC, @SC, 1, 9) AND Number_2 IS NOT NULL AND [dbo].[udf_GetRankColumn](ColumnID) = MaxRank.Value
			EXCEPT
			SELECT ColNum, Num1, Num2 FROM @PriorityColumns

			FETCH NEXT FROM CUR INTO @FC, @SC
		END
		CLOSE CUR
		
		-- Так как существует два набора типов полей и отбор одинарных обеспечивает как минимум наличие одного одинарного столбца для каждого требуемого,
		-- то потребность двух полей в поиске "золотого" на одинарные столбцы однозначно закрыта,
		-- но выборка двойных столбцов по максимуму может не закрывать потребность двух полей одного билета.
		-- Провалидируем наличие достаточности каждого двойного столбца, исключением всех взаимных пар одинарных.
		--/*
		DECLARE @SecFC int, @SecSC int
		
		OPEN CUR
		FETCH NEXT FROM CUR INTO @FC, @SC

		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Определяем внутренний курсор
			DECLARE INNERCUR CURSOR FORWARD_ONLY STATIC READ_ONLY FOR
			SELECT FC, SC 
			FROM @PriorityFieldTypes WHERE FC != @FC AND SC != @SC

			OPEN INNERCUR
			FETCH NEXT FROM INNERCUR INTO @SecFC, @SecSC
			
			WHILE @@FETCH_STATUS = 0
			BEGIN
					-- Считаем количество двойных колонок в взаимной паре одинарных столбцов,.
					-- Получаем указание количества доступных вариантов столбцов, в тех что обязательно будут двойными дважды при этом сочетании одинарных 
					;WITH CheckTwiceColumn AS
					(
						SELECT ColNum, COUNT(1) OVER (PARTITION BY ColNum) AS ColCount FROM @PriorityColumns
						WHERE ColNum NOT IN (1,@FC,@SecFC,@SC,@SecFC,9) AND Num2 IS NOT NULL
					)
					-- Ищем претендентов на добавление
					,PretendentColumns AS
					(
						-- Считаем оконкой номер строки, чтобы отбирать не больше чем по одной записи для каждой требуемой колонки
						SELECT ColumnNumber, Number_1, Number_2, ROW_NUMBER() OVER (PARTITION BY ColumnNumber ORDER BY SecondMaxRank.Value DESC) AS RN FROM [dbo].[T_Columns] AS MAIN
						-- Определяем для колонки максимальный ранг, чтобы найти второй максимум для каждой
						CROSS APPLY
						(
							SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
							WHERE ColumnNumber = MAIN.ColumnNumber AND Number_2 IS NOT NULL
						) AS MaxRank
						-- Второй максимум каждой колонки
						CROSS APPLY
						(
							SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
							WHERE ColumnNumber = MAIN.ColumnNumber AND Number_2 IS NOT NULL AND [dbo].[udf_GetRankColumn](ColumnID) < MaxRank.Value
						) AS SecondMaxRank
						-- Отбираем только тех, кому не хватает второго варианта, чтобы не перенасыщать выборку и гарантировано уменьшать высокоранговые колонки
						WHERE ColumnNumber IN (SELECT ColNum FROM CheckTwiceColumn WHERE ColCount = 1) AND [dbo].[udf_GetRankColumn](ColumnID) = SecondMaxRank.Value
					)
					INSERT INTO @PriorityColumns (ColNum, Num1, Num2)
					SELECT ColumnNumber, Number_1, Number_2 
					FROM PretendentColumns
					WHERE RN = 1

				FETCH NEXT FROM INNERCUR INTO @SecFC, @SecSC
			END

			CLOSE INNERCUR
			DEALLOCATE INNERCUR
			
			FETCH NEXT FROM CUR INTO @FC, @SC
		END

		CLOSE CUR
		--*/
		DEALLOCATE CUR
	END

	-- Исправление ситуации, когда для одинарных колонок одного номера колонки отобраны только исключающие варианты для двойных колонок
	;WITH SuspiciousColumns AS
	(
		SELECT ColNum FROM @PriorityColumns
		WHERE	(ColNum IN (SELECT FC FROM @PriorityFieldTypes) OR ColNum IN (SELECT FC FROM @PriorityFieldTypes))
				AND Num2 IS NOT NULL
	)
	,ConfirmedBadColumns AS
	(
		SELECT ColNum FROM SuspiciousColumns
		WHERE ColNum NOT IN
			(
				SELECT One.ColNum FROM @PriorityColumns AS One
				LEFT JOIN @PriorityColumns AS Two ON One.ColNum = Two.ColNum
				WHERE 
					One.ColNum IN (SELECT ColNum FROM SuspiciousColumns) AND One.Num2 IS NULL AND Two.Num2 IS NOT NULL
					AND
					One.Num1 != Two.Num1 AND One.Num1 != Two.Num2
			)
	)
	,DataFromConfirmed AS
	(
		SELECT ColNum, Num1 FROM @PriorityColumns
		WHERE ColNum IN (SELECT ColNum FROM ConfirmedBadColumns)
		GROUP BY ColNum, Num1
	)
	,PretendentColumns AS
	(
		SELECT ColumnNumber, Number_1, Number_2, ROW_NUMBER() OVER (PARTITION BY ColumnNumber ORDER BY MaxRank.Value DESC) AS RN FROM [dbo].[T_Columns] AS MAIN
		CROSS APPLY
		(
			SELECT MAX([dbo].[udf_GetRankColumn](ColumnID)) AS Value FROM [dbo].[T_Columns]
			WHERE
				ColumnNumber = MAIN.ColumnNumber AND Number_2 IS NOT NULL
				AND Number_1 NOT IN (SELECT Num1 FROM DataFromConfirmed)
				AND Number_2 NOT IN (SELECT Num1 FROM DataFromConfirmed)
		) AS MaxRank
		WHERE ColumnNumber IN (SELECT ColNum FROM DataFromConfirmed) AND Number_2 IS NOT NULL AND [dbo].[udf_GetRankColumn](ColumnID) = MaxRank.Value
	)
	INSERT INTO @PriorityColumns (ColNum, Num1, Num2)
	SELECT ColumnNumber, Number_1, Number_2 
	FROM PretendentColumns
	WHERE RN = 1
	

	-- Общее донасыщение вариантов двойных колонок, на случай если существует вариант для номера колонки, когда выбор его исключает выбор остальных вариантов колонок
	;WITH TroubleCols AS
	(
		SELECT ColNum FROM @PriorityColumns
		WHERE Num2 IS NOT NULL
		EXCEPT
		SELECT One.ColNum FROM @PriorityColumns AS One
		LEFT JOIN @PriorityColumns AS Two ON One.ColNum = Two.ColNum AND One.Num1 != Two.Num1 AND One.Num2 != Two.Num2
		WHERE One.Num2 IS NOT NULL AND Two.Num2 IS NOT NULL AND One.Num1 != Two.Num2 AND One.Num2 != Two.Num1
	)
	,ExceptingNumbers AS
	(
		SELECT ColNum, Num1 AS Num FROM @PriorityColumns
		WHERE ColNum IN (SELECT ColNum FROM TroubleCols)
		UNION
		SELECT ColNum, Num2 AS Num FROM @PriorityColumns
		WHERE ColNum IN (SELECT ColNum FROM TroubleCols)
	)
	,PretendentColumns AS
	(
		SELECT ColumnID, ColumnNumber, Number_1, Number_2, ROW_NUMBER() OVER (PARTITION BY ColumnNumber ORDER BY [dbo].[udf_GetRankColumn](ColumnID)) AS [RN] FROM [dbo].[T_Columns] AS Src
		INNER JOIN ExceptingNumbers AS EN ON Src.ColumnNumber = EN.ColNum AND Number_1 NOT IN (SELECT Num FROM ExceptingNumbers WHERE ColNum = Src.ColumnNumber) AND Number_2 NOT IN (SELECT Num FROM ExceptingNumbers WHERE ColNum = Src.ColumnNumber)
		GROUP BY ColumnID, ColumnNumber, Number_1, Number_2
	)
	INSERT INTO @PriorityColumns (ColNum, Num1, Num2)
	SELECT ColumnNumber, Number_1, Number_2 
	FROM PretendentColumns
	WHERE RN = 1

	SELECT FC, SC FROM @PriorityFieldTypes
	ORDER BY FC

	SELECT ColNum, Num1, Num2 FROM @PriorityColumns
	ORDER BY ColNum, Num2, Num1
END
GO
