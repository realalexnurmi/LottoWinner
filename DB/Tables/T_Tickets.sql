IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_Tickets]'))
BEGIN
	CREATE TABLE [dbo].[T_Tickets]
	(
		[TicketID] uniqueidentifier NOT NULL,
		[TicketNumber] varchar(12) NOT NULL,
		[FirstField] uniqueidentifier NOT NULL,
		[SecondField] uniqueidentifier NOT NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[T_Tickets] WITH NOCHECK
	ADD CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED
	(
		[TicketID]
	) ON [PRIMARY]
END
GO