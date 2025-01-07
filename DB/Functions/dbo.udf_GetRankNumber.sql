SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 23-12-2024
-- Description:	Получение ранга числа
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[udf_GetRankNumber]
(
	-- Число
	@Number int NULL
)
RETURNS int
AS
BEGIN
	DECLARE @Result int
	IF (@Number is NULL)
		SET @Result = 1
	ELSE
		SELECT @Result = SUM(RemainCount) + 1 FROM [dbo].[T_Columns] WHERE Number_1 = @Number OR Number_2 = @Number
	RETURN @Result
END
GO