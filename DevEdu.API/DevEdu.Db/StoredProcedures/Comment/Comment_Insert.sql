﻿CREATE PROCEDURE dbo.Comment_Insert
	@UserId int,
	@Text nvarchar(max),
	@Date datetime
AS
BEGIN
	INSERT INTO dbo.Comment (UserId,[Text],[Date])
	VALUES (@UserId, @Text,GETDATE())
	SELECT @@IDENTITY
END