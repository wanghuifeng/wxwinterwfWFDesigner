
-- Add a single user
INSERT INTO [GeoPlaces].[dbo].[Users] ([Name],[Password])
     VALUES ('sacha','sacha')

-- Add Some user places

GO


DECLARE @UserId AS INTEGER
SET @UserId = (SELECT ID FROM dbo.Users WHERE Name='sacha' AND Password='sacha')



INSERT INTO [GeoPlaces].[dbo].[Places]
           ([UserID],[Name],[Description],[Longitude],[Latitude])
     VALUES
           (@UserId,'The Crysler Building NY','Best building ever',
			
			-73.97528,
			40.75139 )

INSERT INTO [GeoPlaces].[dbo].[Places]
           ([UserID],[Name],[Description],[Longitude],[Latitude])
     VALUES
           (@UserId,'The Sydney Harbour Opera House','The Sydney Harbour Opera House',
			
			151.2146435776104,
			-33.85732060593062 )


INSERT INTO [GeoPlaces].[dbo].[Places]
           ([UserID],[Name],[Description],[Longitude],[Latitude])
     VALUES
           (@UserId,'My House In The UK','Where I live now',
			-0.16388055555555556,
			50.826958333333337 )

GO