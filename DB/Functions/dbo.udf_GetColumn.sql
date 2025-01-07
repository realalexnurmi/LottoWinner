SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 25-12-2024
-- Description:	Получение идентификатора колонки.
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[udf_GetColumn]
(
	@Number1 int,
	@Number2 int
)
RETURNS uniqueidentifier
AS
BEGIN
	DECLARE @ColumnID uniqueidentifier

	SELECT @ColumnID = ColumnID FROM [dbo].[T_Columns]
	WHERE 
		Number_1 = @Number1 AND ((Number_2 IS NULL AND @Number2 IS NULL) OR Number_2 = @Number2)

	RETURN @ColumnID
END
