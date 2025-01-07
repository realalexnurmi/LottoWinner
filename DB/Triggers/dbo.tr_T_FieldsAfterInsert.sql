SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 26-12-2024
-- Description:	Триггер обновления текущего количества используемых колонок и типа поля после вставки нового поля.
-- =============================================
CREATE OR ALTER TRIGGER [dbo].[tr_T_FieldsAfterInsert]
   ON  [dbo].[T_Fields]
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[T_FieldTypes]
	SET CurrentCount = CurrentCount + 1
	WHERE FieldTypeID IN (SELECT FieldType FROM inserted)

	UPDATE [dbo].[T_Columns]
	SET CurrentCount = CurrentCount + 1
	WHERE
		ColumnID IN (SELECT Column_1 FROM inserted)
		OR ColumnID IN (SELECT Column_2 FROM inserted)
		OR ColumnID IN (SELECT Column_3 FROM inserted)
		OR ColumnID IN (SELECT Column_4 FROM inserted)
		OR ColumnID IN (SELECT Column_5 FROM inserted)
		OR ColumnID IN (SELECT Column_6 FROM inserted)
		OR ColumnID IN (SELECT Column_7 FROM inserted)
		OR ColumnID IN (SELECT Column_8 FROM inserted)
		OR ColumnID IN (SELECT Column_9 FROM inserted)
	
END
GO
