USE [NetIncidentIdentity04]
GO

-- ALTER TABLE [dbo].[Servers] DROP CONSTRAINT [FK_Servers_Companies_CompanyId]
-- GO

ALTER TABLE [dbo].[Servers]  WITH CHECK ADD  CONSTRAINT [FK_Servers_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyId])
GO

ALTER TABLE [dbo].[Servers] CHECK CONSTRAINT [FK_Servers_Companies_CompanyId]
GO


