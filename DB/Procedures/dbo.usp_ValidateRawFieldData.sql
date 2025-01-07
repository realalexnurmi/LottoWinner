SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 25-12-2024
-- Description:	��������� ����� ������ ����.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[usp_ValidateRawFieldData]
	@RawFieldData [dbo].[udtt_RawFieldData] READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ColCount int

	SELECT @ColCount = COUNT(1) FROM @RawFieldData
	
	-- �������� ���������� (����� 9)
	IF (@ColCount < 9)
		THROW 50001, '� ������ ������������ ����� 9 �������, ������������ ������� ����.', 1
	
	-- �������� ���������� (����� 9)
	IF (@ColCount > 9)
		THROW 50002, '� ������ ������������ ����� 9 �������, ������������ ������� ����.', 1

	-- �������� ���������� (�������� ������� ��������)
	IF (EXISTS(SELECT 1 FROM (SELECT COUNT(1) OVER (PARTITION BY ColNumber) AS ColCount FROM @RawFieldData) AS Cols WHERE ColCount > 1))
		THROW 50003, '� ������ ���� ������������ 2 ��� ����� ������ ������ � ���� �� ������ �������.', 1

	-- �������� �������. ����� ��� ��� ������� ����� ����� 3.
	IF ((SELECT COUNT(1) FROM @RawFieldData WHERE Num2 IS NULL) != 3)
		THROW 50004, '� ������ ���� ����� ������� � ����� ������ �� ���.', 1

	-- �������� ������������ ������� �������� ������� ������ �����
	IF (EXISTS(SELECT 1 FROM @RawFieldData WHERE (Num2 IS NOT NULL AND PosNum2 IS NULL) OR (Num2 IS NULL AND PosNum2 IS NOT NULL)))
		THROW 50005, '� ������ ���� ���������� ������ � ���������� ��������� ������� ������� �����: ������������ �������.', 1

	-- �������� ������������ �������� ������� (�������� 1-3)
	IF (EXISTS(SELECT 1 FROM @RawFieldData WHERE PosNum1 < 1 OR PosNum1 > 3 OR (PosNum2 IS NOT NULL AND (PosNum2 < 1 OR PosNum2 > 3))))
		THROW 50006, '� ������ ���� ���������� ������ � ���������� ��������� �������: ��������� ��������� 1-3.', 1

	-- �������� ������������ �������� ������� (������������ �������)
	IF (EXISTS(SELECT 1 FROM @RawFieldData WHERE PosNum2 IS NOT NULL AND PosNum1 = PosNum2))
		THROW 50007, '� ������ ���� ���������� ������ � ���������� ��������� �������: ������������ �������.', 1

	-- �������� ������������ ����� (������������ �����)
	IF (EXISTS(SELECT 1 FROM @RawFieldData WHERE Num2 IS NOT NULL AND Num1 = Num2))
		THROW 50008, '� ������ ���� ���������� ������ � ���������� �������: ������������ �����.', 1

	-- �������� ������������ ����� (������ ��������� ����� � ��������)
	IF (EXISTS(SELECT 1 FROM @RawFieldData AS Src
	LEFT JOIN
	(
		SELECT ColNumber, (ColNumber - 1) * 10 + IIF(ColNumber = 1, 1, 0) AS BeginRange, (ColNumber * 10) - IIF(ColNumber = 9, 0, 1) AS EndRange FROM @RawFieldData
	) AS [Range] ON Src.ColNumber = [Range].ColNumber
	WHERE Num1 < [Range].BeginRange OR Num1 > [Range].EndRange OR (Num2 IS NOT NULL AND (Num2 < [Range].BeginRange OR Num2 > [Range].EndRange))))
		THROW 50009, '� ������ ���� ���������� ������ � ���������� �������: ��������� ��������� �������.', 1

	-- �������� ������������ ������ �������, ����� 1 ������� �������, ��� ����� 2.
	IF (EXISTS(SELECT 1 FROM @RawFieldData WHERE Num2 IS NOT NULL AND Num1 > Num2))
		THROW 50010, '� ������ ���� ���������� ������ ����������������� �������.', 1

	-- �������� ����������� ���� ��������� �������
	IF ((SELECT COUNT(1) FROM [dbo].[T_Columns] AS Cols
	INNER JOIN @RawFieldData AS Pretendent ON 
			Cols.ColumnNumber = Pretendent.ColNumber 
		AND Cols.Number_1 = Pretendent.Num1 
		AND ((Cols.Number_2 IS NULL AND Pretendent.Num2 IS NULL) OR Cols.Number_2 = Pretendent.Num2)
	WHERE Cols.RemainCount > 0) != 9)
		THROW 50011, '� ������ ���� ���������� ������ � ������������ ���������.', 1
END
GO
