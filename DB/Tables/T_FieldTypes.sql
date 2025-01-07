IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_FieldTypes]'))
BEGIN
	CREATE TABLE [dbo].[T_FieldTypes]
	(
		[FieldTypeID] uniqueidentifier NOT NULL,
		[NumberFirstColumnWithOneNumber] int NOT NULL,
		[NumberSecondColumnWithOneNumber] int NOT NULL,
		[AvailableCount] int NOT NULL,
		[CurrentCount] int NOT NULL,
		[RemainCount] AS [AvailableCount] - [CurrentCount]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[T_FieldTypes] WITH NOCHECK
	ADD CONSTRAINT [PK_FieldTypes] PRIMARY KEY CLUSTERED
	(
		[FieldTypeID]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[T_FieldTypes] WITH NOCHECK
	ADD CONSTRAINT [DF_FieldType_CurrentCount] DEFAULT 0 FOR [CurrentCount]
END
GO