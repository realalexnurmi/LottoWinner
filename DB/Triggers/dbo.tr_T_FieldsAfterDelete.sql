SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 26-12-2024
-- Description:	Триггер обновления текущего количества используемых колонок и типа поля после удаления существующего поля.
-- =============================================
CREATE OR ALTER TRIGGER [dbo].[tr_T_FieldsAfterDelete]
   ON  [dbo].[T_Fields]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[T_FieldTypes]
	SET CurrentCount = CurrentCount - 1
	WHERE FieldTypeID IN (SELECT FieldType FROM deleted)

	UPDATE [dbo].[T_Columns]
	SET CurrentCount = CurrentCount - 1
	WHERE
		ColumnID IN (SELECT Column_1 FROM deleted)
		OR ColumnID IN (SELECT Column_2 FROM deleted)
		OR ColumnID IN (SELECT Column_3 FROM deleted)
		OR ColumnID IN (SELECT Column_4 FROM deleted)
		OR ColumnID IN (SELECT Column_5 FROM deleted)
		OR ColumnID IN (SELECT Column_6 FROM deleted)
		OR ColumnID IN (SELECT Column_7 FROM deleted)
		OR ColumnID IN (SELECT Column_8 FROM deleted)
		OR ColumnID IN (SELECT Column_9 FROM deleted)
	
END
GO
