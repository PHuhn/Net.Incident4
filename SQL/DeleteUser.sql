USE [NetIncidentIdentity04]
GO
DECLARE @ID NVARCHAR(100) = 'b0';
--
DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId = @ID
DELETE FROM [dbo].[AspNetUserLogins] WHERE UserId = @ID
DELETE FROM [dbo].[ApplicationUserApplicationServer] WHERE Id = @ID
DELETE FROM [dbo].[AspNetUsers] WHERE Id = @ID
--
GO


