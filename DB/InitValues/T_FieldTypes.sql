INSERT INTO [dbo].[T_FieldTypes]
           ([FieldTypeID]
           ,[NumberFirstColumnWithOneNumber]
           ,[NumberSecondColumnWithOneNumber]
           ,[AvailableCount])
     VALUES
           (NEWID(), 2, 3, 3),
		   (NEWID(), 2, 4, 3),
		   (NEWID(), 2, 5, 3),
		   (NEWID(), 2, 6, 3),
		   (NEWID(), 2, 7, 3),
		   (NEWID(), 2, 8, 4),
		   (NEWID(), 3, 4, 3),
           (NEWID(), 3, 5, 3),
		   (NEWID(), 3, 6, 3),
		   (NEWID(), 3, 7, 4),
		   (NEWID(), 3, 8, 3),
		   (NEWID(), 4, 5, 3),
		   (NEWID(), 4, 6, 4),
		   (NEWID(), 4, 7, 3),
           (NEWID(), 4, 8, 3),
		   (NEWID(), 5, 6, 3),
		   (NEWID(), 5, 7, 3),
		   (NEWID(), 5, 8, 3),
		   (NEWID(), 6, 7, 3),
		   (NEWID(), 6, 8, 3),
		   (NEWID(), 7, 8, 3)
GO