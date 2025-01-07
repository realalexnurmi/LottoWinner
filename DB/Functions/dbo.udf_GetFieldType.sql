SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 25-12-2024
-- Description:	Получение идентификатора типа колонки.
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[udf_GetFieldType]
(
	@FirstColumnWithOneNumber int,
	@SecondColumnWithOneNumber int
)
RETURNS uniqueidentifier
AS
BEGIN
	DECLARE @FieldTypeID uniqueidentifier

	SELECT @FieldTypeID = FieldTypeID FROM [dbo].[T_FieldTypes]
	WHERE 
		(NumberFirstColumnWithOneNumber = @FirstColumnWithOneNumber AND NumberSecondColumnWithOneNumber = @SecondColumnWithOneNumber)
		OR
		(NumberFirstColumnWithOneNumber = @SecondColumnWithOneNumber AND NumberSecondColumnWithOneNumber = @FirstColumnWithOneNumber)

	RETURN @FieldTypeID
END
GO

