SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 25-12-2024
-- Description:	Вставка нового поля билета из сырых данных.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[usp_InsertNewField]
	@FirstColumnWithOneNumber int,
	@SecondColumnWithOneNumber int,
	@RawFieldData [dbo].[udtt_RawFieldData] READONLY,
	@GuidCreatedField uniqueidentifier output
AS
BEGIN
	SET NOCOUNT ON;

	IF (@FirstColumnWithOneNumber = @SecondColumnWithOneNumber)
		THROW 50012, 'Положения средних колонок с одним числом дублировано.', 1

    EXEC [dbo].[usp_ValidateRawFieldData] @RawFieldData = @RawFieldData

	DECLARE @FieldType UNIQUEIDENTIFIER
	
	SET @FieldType = [dbo].[udf_GetFieldType](@FirstColumnWithOneNumber, @SecondColumnWithOneNumber)

	IF (@FieldType IS NULL)
		THROW 50013, 'Не существует указаного типа поля по средним колонкам.', 1

	IF ((SELECT RemainCount FROM [dbo].[T_FieldTypes] WHERE FieldTypeID = @FieldType) < 1)
		THROW 50014, 'Найденный тип поля не доступен.', 1

	SET @GuidCreatedField = NEWID()
	
	;WITH Cols AS
	(
		SELECT [1], [2], [3], [4], [5], [6], [7], [8], [9]
		FROM
		(
			SELECT ColNumber, [dbo].[udf_GetColumn](Num1, Num2) AS Col FROM @RawFieldData
		) AS Src
		PIVOT
		(
			MAX(Col) FOR ColNumber IN ([1], [2], [3], [4], [5], [6], [7], [8], [9])
		) AS Cols
	),
	ColTypes AS
	(
		SELECT [1], [2], [3], [4], [5], [6], [7], [8], [9]
		FROM
		(
			SELECT ColNumber, [dbo].[udf_GetColumnType](PosNum1, PosNum2) as ColType FROM @RawFieldData
		) AS Src
		PIVOT
		(
			MAX(ColType) FOR ColNumber IN ([1], [2], [3], [4], [5], [6], [7], [8], [9])
		) AS ColTypes
	)
	INSERT INTO [dbo].[T_Fields]
		([FieldID]
		,[FieldType]
		,[Column_1]
		,[Column_1_Type]
		,[Column_2]
		,[Column_2_Type]
		,[Column_3]
		,[Column_3_Type]
		,[Column_4]
		,[Column_4_Type]
		,[Column_5]
		,[Column_5_Type]
		,[Column_6]
		,[Column_6_Type]
		,[Column_7]
		,[Column_7_Type]
		,[Column_8]
		,[Column_8_Type]
		,[Column_9]
		,[Column_9_Type])
	SELECT
		@GuidCreatedField
		,@FieldType
		,Cols.[1]
		,ColTypes.[1]
		,Cols.[2]
		,ColTypes.[2]
		,Cols.[3]
		,ColTypes.[3]
		,Cols.[4]
		,ColTypes.[4]
		,Cols.[5]
		,ColTypes.[5]
		,Cols.[6]
		,ColTypes.[6]
		,Cols.[7]
		,ColTypes.[7]
		,Cols.[8]
		,ColTypes.[8]
		,Cols.[9]
		,ColTypes.[9]
	FROM Cols, ColTypes
END
GO
