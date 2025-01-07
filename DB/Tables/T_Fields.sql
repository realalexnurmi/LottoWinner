IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_Fields]'))
BEGIN
	CREATE TABLE [dbo].[T_Fields]
	(
		[FieldID] uniqueidentifier NOT NULL,
		[FieldType] uniqueidentifier NOT NULL,
		[Column_1] uniqueidentifier NOT NULL,
		[Column_1_Type] uniqueidentifier NOT NULL,
		[Column_2] uniqueidentifier NOT NULL,
		[Column_2_Type] uniqueidentifier NOT NULL,
		[Column_3] uniqueidentifier NOT NULL,
		[Column_3_Type] uniqueidentifier NOT NULL,
		[Column_4] uniqueidentifier NOT NULL,
		[Column_4_Type] uniqueidentifier NOT NULL,
		[Column_5] uniqueidentifier NOT NULL,
		[Column_5_Type] uniqueidentifier NOT NULL,
		[Column_6] uniqueidentifier NOT NULL,
		[Column_6_Type] uniqueidentifier NOT NULL,
		[Column_7] uniqueidentifier NOT NULL,
		[Column_7_Type] uniqueidentifier NOT NULL,
		[Column_8] uniqueidentifier NOT NULL,
		[Column_8_Type] uniqueidentifier NOT NULL,
		[Column_9] uniqueidentifier NOT NULL,
		[Column_9_Type] uniqueidentifier NOT NULL,
	) ON [PRIMARY]

	ALTER TABLE [dbo].[T_Fields] WITH NOCHECK
	ADD CONSTRAINT [PK_Fields] PRIMARY KEY CLUSTERED
	(
		[FieldID]
	) ON [PRIMARY]
END
GO