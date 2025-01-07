SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 26-12-2024
-- Description:	Проверка имееют ли два указанных столбца одинаковые числа.
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[udf_IsPairsColumnsHasSameNumbers]
(
	@Column1 uniqueidentifier,
	@Column2 uniqueidentifier
)
RETURNS bit
AS
BEGIN
	DECLARE @Result bit = 0

	DECLARE @Col1Num int, @Num1Col1 int, @Num2Col1 int
	DECLARE @Col2Num int, @Num1Col2 int, @Num2Col2 int

	SELECT @Col1Num = ColumnNumber, @Num1Col1 = Number_1, @Num2Col1 = Number_2 FROM [dbo].[T_Columns] WHERE ColumnID = @Column1
	SELECT @Col2Num = ColumnNumber, @Num1Col2 = Number_1, @Num2Col2 = Number_2 FROM [dbo].[T_Columns] WHERE ColumnID = @Column2

	IF (@Col1Num = @Col2Num)
	BEGIN
		SET @Result = IIF(
							@Num1Col1 = @Num1Col2 OR
							(@Num2Col1 IS NOT NULL AND @Num2Col1 = @Num1Col2) OR
							(@Num2Col2 IS NOT NULL AND @Num2Col2 = @Num1Col1) OR
							(@Num2Col1 IS NOT NULL AND @Num2Col2 IS NOT NULL AND @Num2Col1 = @Num2Col2), 1, 0)
	END

	RETURN @Result
END
GO

