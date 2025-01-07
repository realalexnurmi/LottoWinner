IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_Columns]'))
BEGIN
	CREATE TABLE [dbo].[T_Columns]
	(
		[ColumnID] uniqueidentifier NOT NULL,
		[ColumnNumber] int NOT NULL,
		[Number_1] int NOT NULL,
		[Number_2] int NULL,
		[AvailableCount] int NOT NULL,
		[CurrentCount] int NOT NULL,
		[RemainCount] AS [AvailableCount] - [CurrentCount]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[T_Columns] WITH NOCHECK
	ADD CONSTRAINT [PK_Columns] PRIMARY KEY CLUSTERED
	(
		[ColumnID]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[T_Columns] WITH NOCHECK
	ADD CONSTRAINT [DF_Column_CurrentCount] DEFAULT 0 FOR [CurrentCount]
END
GO