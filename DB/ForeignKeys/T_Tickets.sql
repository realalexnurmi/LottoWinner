IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_FirstField]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Tickets]'))
	ALTER TABLE [dbo].[T_Tickets] WITH NOCHECK
	ADD CONSTRAINT [FK_Tickets_FirstField] FOREIGN KEY ([FirstField])
	REFERENCES [dbo].[T_Fields] ([FieldID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_SecondField]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Tickets]'))
	ALTER TABLE [dbo].[T_Tickets] WITH NOCHECK
	ADD CONSTRAINT [FK_Tickets_SecondField] FOREIGN KEY ([SecondField])
	REFERENCES [dbo].[T_Fields] ([FieldID])
GO