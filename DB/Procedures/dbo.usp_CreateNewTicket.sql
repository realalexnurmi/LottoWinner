SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 25-12-2024
-- Description:	Создание билета по данным полей.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[usp_CreateNewTicket]
	@TicketNumber varchar(12),
	@FCWO_F1 int,
	@SCWO_F1 int,
	@RFD_F1 [dbo].[udtt_RawFieldData] READONLY,
	@FCWO_F2 int,
	@SCWO_F2 int,
	@RFD_F2 [dbo].[udtt_RawFieldData] READONLY,
	@GuidCreatedTicket uniqueidentifier output
AS
BEGIN
	SET NOCOUNT ON;

	SET XACT_ABORT ON;
	BEGIN TRANSACTION;

	-- Создаем первое поле
	DECLARE @Field1 uniqueidentifier
	EXEC [dbo].[usp_InsertNewField] @FirstColumnWithOneNumber = @FCWO_F1, @SecondColumnWithOneNumber = @SCWO_F1, @RawFieldData = @RFD_F1, @GuidCreatedField = @Field1 output

	-- Создаем второе поле
	DECLARE @Field2 uniqueidentifier
	EXEC [dbo].[usp_InsertNewField] @FirstColumnWithOneNumber = @FCWO_F2, @SecondColumnWithOneNumber = @SCWO_F2, @RawFieldData = @RFD_F2, @GuidCreatedField = @Field2 output

	-- Проверка непротиворечивости двух добавленых полей в рамках одного билета.
	-- Отсутствие одинаковых чисел в тех же колонках двух разных полей.
	IF	(
			SELECT SUM(IIF([dbo].[udf_IsPairsColumnsHasSameNumbers]([F1].[Link], [F2].[Link]) = 1, 1, 0)) as Res
			FROM
			(
				SELECT FieldID, Column_1, Column_2, Column_3, Column_4, Column_5, Column_6, Column_7, Column_8, Column_9 FROM [dbo].[T_Fields] WHERE FieldID = @Field1
			) as pvt
			UNPIVOT
			(
				Link FOR [Columns] IN
					(Column_1, Column_2, Column_3, Column_4, Column_5, Column_6, Column_7, Column_8, Column_9)
			) as F1
			JOIN
			(
				SELECT [Columns], Link
				FROM
				(
					SELECT FieldID, Column_1, Column_2, Column_3, Column_4, Column_5, Column_6, Column_7, Column_8, Column_9 FROM [dbo].[T_Fields] WHERE FieldID = @Field2
				) as pvt
				UNPIVOT
				(
					Link FOR [Columns] IN
						(Column_1, Column_2, Column_3, Column_4, Column_5, Column_6, Column_7, Column_8, Column_9)
				) as unpvt
			) AS F2 ON F2.[Columns] = F1.[Columns]
		) > 0
		THROW 50015, 'Ошибка данных - некорректный билет, так как в столбцах двух полей не может существовать одинаковых чисел.', 1

	INSERT INTO [dbo].[T_Tickets] (TicketID, TicketNumber, FirstField, SecondField)
	VALUES
	(NEWID(), @TicketNumber, @Field1, @Field2)
	
	COMMIT TRANSACTION;
END
GO
