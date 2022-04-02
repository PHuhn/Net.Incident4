USE NetIncidentIdentity04

DROP TABLE NetworkLog
DROP TABLE ApplicationUserApplicationServer
DROP TABLE [Log]
DROP TABLE EmailTemplates
DROP TABLE IncidentIncidentNotes
DROP TABLE IncidentNote
DROP TABLE NoteType
DROP TABLE Incident
DROP TABLE IncidentType
DROP TABLE NIC
DROP TABLE Servers
DROP TABLE [dbo].[AspNetRoleClaims]
DROP TABLE [dbo].[AspNetUserTokens]
DROP TABLE [dbo].[AspNetUserLogins]
DROP TABLE [dbo].[AspNetUserClaims]
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
DROP TABLE [dbo].[AspNetUserRoles]
DROP TABLE [dbo].[AspNetRoles]
DROP TABLE [dbo].[AspNetUsers]
DROP TABLE Companies
DROP TABLE [dbo].[__EFMigrationsHistory]

