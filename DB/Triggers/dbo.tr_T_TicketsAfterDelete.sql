SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nurmi Alex
-- Create date: 26-12-2024
-- Description:	Триггер удаления связанных полей при удалении билета.
-- =============================================
CREATE OR ALTER TRIGGER [dbo].[tr_T_TicketsAfterDelete]
   ON  [dbo].[T_Tickets]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[T_Fields]
	WHERE FieldID IN (SELECT FirstField FROM deleted)

	DELETE FROM [dbo].[T_Fields]
	WHERE FieldID IN (SELECT SecondField FROM deleted)
	
END
GO