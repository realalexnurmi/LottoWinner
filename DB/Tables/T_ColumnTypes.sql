IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_ColumnTypes]'))
BEGIN
	CREATE TABLE [dbo].[T_ColumnTypes]
	(
		[ColumnTypeID] uniqueidentifier NOT NULL,
		[Number_1_Row] int NOT NULL,
		[Number_2_Row] int NULL,
	) ON [PRIMARY]

	ALTER TABLE [dbo].[T_ColumnTypes] WITH NOCHECK
	ADD CONSTRAINT [PK_ColumnTypes] PRIMARY KEY CLUSTERED
	(
		[ColumnTypeID]
	) ON [PRIMARY]
END
GO