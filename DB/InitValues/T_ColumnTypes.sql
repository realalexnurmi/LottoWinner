INSERT INTO [dbo].[T_ColumnTypes]
           ([ColumnTypeID]
           ,[Number_1_Row]
           ,[Number_2_Row])
     VALUES
           (NEWID(), 1, NULL),
           (NEWID(), 2, NULL),
		   (NEWID(), 3, NULL),
		   (NEWID(), 1, 2),
		   (NEWID(), 1, 3),
		   (NEWID(), 2, 1),
		   (NEWID(), 2, 3),
		   (NEWID(), 3, 1),
		   (NEWID(), 3, 2)
GO
