IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_FieldType]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_FieldType] FOREIGN KEY ([FieldType])
	REFERENCES [dbo].[T_FieldTypes] ([FieldTypeID])
GO

--Column 1
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_1]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_1] FOREIGN KEY ([Column_1])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_1_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_1_Type] FOREIGN KEY ([Column_1_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 2
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_2]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_2] FOREIGN KEY ([Column_2])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_2_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_2_Type] FOREIGN KEY ([Column_2_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 3
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_3]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_3] FOREIGN KEY ([Column_3])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_3_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_3_Type] FOREIGN KEY ([Column_3_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 4
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_4]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_4] FOREIGN KEY ([Column_4])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_4_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_4_Type] FOREIGN KEY ([Column_4_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 5
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_5]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_5] FOREIGN KEY ([Column_5])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_5_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_5_Type] FOREIGN KEY ([Column_5_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 6
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_6]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_6] FOREIGN KEY ([Column_6])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_6_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_6_Type] FOREIGN KEY ([Column_6_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 7
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_7]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_7] FOREIGN KEY ([Column_7])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_7_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_7_Type] FOREIGN KEY ([Column_7_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 8
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_8]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_8] FOREIGN KEY ([Column_8])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_8_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_8_Type] FOREIGN KEY ([Column_8_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO

--Column 9
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_9]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_9] FOREIGN KEY ([Column_9])
	REFERENCES [dbo].[T_Columns] ([ColumnID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fields_Column_9_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [FK_Fields_Column_9_Type] FOREIGN KEY ([Column_9_Type])
	REFERENCES [dbo].[T_ColumnTypes] ([ColumnTypeID])
GO