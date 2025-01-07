SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 25-12-2024
-- Description:	Получение идентификатора колонки.
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[udf_GetColumnType]
(
	@PosNumber1 int,
	@PosNumber2 int
)
RETURNS uniqueidentifier
AS
BEGIN
	DECLARE @ColumnTypeID uniqueidentifier

	SELECT @ColumnTypeID = ColumnTypeID FROM [dbo].[T_ColumnTypes]
	WHERE 
		Number_1_Row = @PosNumber1 AND ((Number_2_Row IS NULL AND @PosNumber2 IS NULL) OR Number_2_Row = @PosNumber2)

	RETURN @ColumnTypeID
END
