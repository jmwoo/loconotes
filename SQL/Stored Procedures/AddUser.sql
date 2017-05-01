
GO

DROP PROCEDURE [dbo].[AddUser]
GO

CREATE PROCEDURE [dbo].[AddUser]
	@username varchar(50)
	,@password varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	declare @hashedBytes BINARY(64) = HASHBYTES('SHA2_512', @password)
	declare @hashedString varchar(300) = cast('' as xml).value('xs:base64Binary(sql:variable("@hashedBytes"))', 'varchar(300)')

    insert into [dbo].[Users] (
		Uid
		,Username
		,PasswordHash
	)
	values (
		NEWID()
		,@username
		,@hashedString
	)
END

GO

