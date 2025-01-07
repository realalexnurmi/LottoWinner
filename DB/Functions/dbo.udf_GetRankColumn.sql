SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 23-12-2024
-- Description:	Получение ранга колонки
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[udf_GetRankColumn]
(
	-- Идентификатор колонки
	@ColumnID uniqueidentifier
)
RETURNS int
AS
BEGIN
	DECLARE @Result int
	SELECT @Result = [dbo].[udf_GetRankNumber](Number_1) * [dbo].[udf_GetRankNumber](Number_2) * (RemainCount) FROM [dbo].[T_Columns] WHERE ColumnID = @ColumnID
	RETURN @Result
END
GO