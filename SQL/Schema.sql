USE [master]
GO
/****** Object:  Database [NetIncidentIdentity04]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE DATABASE [NetIncidentIdentity04]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NetIncidentIdentity04', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.EXPRESS\MSSQL\DATA\NetIncidentIdentity04.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NetIncidentIdentity04_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.EXPRESS\MSSQL\DATA\NetIncidentIdentity04_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NetIncidentIdentity04] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NetIncidentIdentity04].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NetIncidentIdentity04] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET ARITHABORT OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NetIncidentIdentity04] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NetIncidentIdentity04] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NetIncidentIdentity04] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NetIncidentIdentity04] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [NetIncidentIdentity04] SET  MULTI_USER 
GO
ALTER DATABASE [NetIncidentIdentity04] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NetIncidentIdentity04] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NetIncidentIdentity04] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NetIncidentIdentity04] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NetIncidentIdentity04] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NetIncidentIdentity04] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [NetIncidentIdentity04] SET QUERY_STORE = OFF
GO
USE [NetIncidentIdentity04]
GO
/****** Object:  User [NT AUTHORITY\SYSTEM]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [NT AUTHORITY\NETWORK SERVICE]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE USER [NT AUTHORITY\NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [NT AUTHORITY\LOCAL SERVICE]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE USER [NT AUTHORITY\LOCAL SERVICE] FOR LOGIN [NT AUTHORITY\LOCAL SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [NT AUTHORITY\SYSTEM]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [NT AUTHORITY\SYSTEM]
GO
ALTER ROLE [db_datareader] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
GO
ALTER ROLE [db_datareader] ADD MEMBER [NT AUTHORITY\LOCAL SERVICE]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [NT AUTHORITY\LOCAL SERVICE]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationUserApplicationServer]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserApplicationServer](
	[Id] [nvarchar](450) NOT NULL,
	[ServerId] [int] NOT NULL,
 CONSTRAINT [PK_ApplicationUserApplicationServer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[UserNicName] [nvarchar](16) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Audit]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audit](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ChangeDate] [datetime2](7) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Program] [nvarchar](256) NOT NULL,
	[TableName] [nvarchar](256) NOT NULL,
	[UpdateType] [char](1) NOT NULL,
	[Keys] [nvarchar](512) NOT NULL,
	[Before] [nvarchar](4000) NOT NULL,
	[After] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyShortName] [nvarchar](12) NOT NULL,
	[CompanyName] [nvarchar](80) NOT NULL,
	[Address] [nvarchar](80) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](4) NULL,
	[PostalCode] [nvarchar](15) NULL,
	[Country] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplates](
	[CompanyId] [int] NOT NULL,
	[IncidentTypeId] [int] NOT NULL,
	[SubjectLine] [nvarchar](max) NOT NULL,
	[EmailBody] [nvarchar](max) NOT NULL,
	[TimeTemplate] [nvarchar](max) NOT NULL,
	[ThanksTemplate] [nvarchar](max) NOT NULL,
	[LogTemplate] [nvarchar](max) NOT NULL,
	[Template] [nvarchar](max) NOT NULL,
	[FromServer] [bit] NOT NULL,
 CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[IncidentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Incident]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incident](
	[IncidentId] [bigint] IDENTITY(1,1) NOT NULL,
	[ServerId] [int] NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[NIC_Id] [nvarchar](16) NOT NULL,
	[NetworkName] [nvarchar](255) NULL,
	[AbuseEmailAddress] [nvarchar](255) NULL,
	[ISPTicketNumber] [nvarchar](50) NULL,
	[Mailed] [bit] NOT NULL,
	[Closed] [bit] NOT NULL,
	[Special] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Incident] PRIMARY KEY CLUSTERED 
(
	[IncidentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentIncidentNotes]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentIncidentNotes](
	[IncidentId] [bigint] NOT NULL,
	[IncidentNoteId] [bigint] NOT NULL,
 CONSTRAINT [PK_IncidentIncidentNotes] PRIMARY KEY CLUSTERED 
(
	[IncidentId] ASC,
	[IncidentNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentNote]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentNote](
	[IncidentNoteId] [bigint] IDENTITY(1,1) NOT NULL,
	[NoteTypeId] [int] NOT NULL,
	[Note] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_IncidentNote] PRIMARY KEY CLUSTERED 
(
	[IncidentNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentType]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentType](
	[IncidentTypeId] [int] IDENTITY(1,1) NOT NULL,
	[IncidentTypeShortDesc] [nvarchar](8) NOT NULL,
	[IncidentTypeDesc] [nvarchar](50) NOT NULL,
	[IncidentTypeFromServer] [bit] NOT NULL,
	[IncidentTypeSubjectLine] [nvarchar](max) NOT NULL,
	[IncidentTypeEmailTemplate] [nvarchar](max) NOT NULL,
	[IncidentTypeTimeTemplate] [nvarchar](max) NOT NULL,
	[IncidentTypeThanksTemplate] [nvarchar](max) NOT NULL,
	[IncidentTypeLogTemplate] [nvarchar](max) NOT NULL,
	[IncidentTypeTemplate] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_IncidentType] PRIMARY KEY CLUSTERED 
(
	[IncidentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Application] [nvarchar](30) NOT NULL,
	[Method] [nvarchar](255) NOT NULL,
	[LogLevel] [tinyint] NOT NULL,
	[Level] [nvarchar](8) NOT NULL,
	[UserAccount] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](4000) NOT NULL,
	[Exception] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetworkLog]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetworkLog](
	[NetworkLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[ServerId] [int] NOT NULL,
	[IncidentId] [bigint] NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[NetworkLogDate] [datetime2](7) NOT NULL,
	[Log] [nvarchar](max) NOT NULL,
	[IncidentTypeId] [int] NOT NULL,
 CONSTRAINT [PK_NetworkLog] PRIMARY KEY CLUSTERED 
(
	[NetworkLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NIC]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NIC](
	[NIC_Id] [nvarchar](16) NOT NULL,
	[NICDescription] [nvarchar](255) NOT NULL,
	[NICAbuseEmailAddress] [nvarchar](50) NULL,
	[NICRestService] [nvarchar](255) NULL,
	[NICWebSite] [nvarchar](255) NULL,
 CONSTRAINT [PK_NIC] PRIMARY KEY CLUSTERED 
(
	[NIC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteType]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteType](
	[NoteTypeId] [int] IDENTITY(1,1) NOT NULL,
	[NoteTypeShortDesc] [nvarchar](8) NOT NULL,
	[NoteTypeDesc] [nvarchar](50) NOT NULL,
	[NoteTypeClientScript] [nvarchar](12) NULL,
 CONSTRAINT [PK_NoteType] PRIMARY KEY CLUSTERED 
(
	[NoteTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servers]    Script Date: 2/9/2024 1:24:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servers](
	[ServerId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ServerShortName] [nvarchar](12) NOT NULL,
	[ServerName] [nvarchar](80) NOT NULL,
	[ServerDescription] [nvarchar](255) NOT NULL,
	[WebSite] [nvarchar](255) NOT NULL,
	[ServerLocation] [nvarchar](255) NOT NULL,
	[FromName] [nvarchar](255) NOT NULL,
	[FromNicName] [nvarchar](16) NOT NULL,
	[FromEmailAddress] [nvarchar](255) NOT NULL,
	[TimeZone] [nvarchar](16) NOT NULL,
	[DST] [bit] NOT NULL,
	[TimeZone_DST] [nvarchar](16) NULL,
	[DST_Start] [datetime2](7) NULL,
	[DST_End] [datetime2](7) NULL,
 CONSTRAINT [PK_Servers] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApplicationUserApplicationServer_ServerId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_ApplicationUserApplicationServer_ServerId] ON [dbo].[ApplicationUserApplicationServer]
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Idx_AspNetUsers_FullName]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_AspNetUsers_FullName] ON [dbo].[AspNetUsers]
(
	[FullName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUsers_CompanyId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_CompanyId] ON [dbo].[AspNetUsers]
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Idx_Companies_ShortName]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_Companies_ShortName] ON [dbo].[Companies]
(
	[CompanyShortName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EmailTemplates_IncidentTypeId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_EmailTemplates_IncidentTypeId] ON [dbo].[EmailTemplates]
(
	[IncidentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Incident_NIC_Id]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_Incident_NIC_Id] ON [dbo].[Incident]
(
	[NIC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Incident_ServerId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_Incident_ServerId] ON [dbo].[Incident]
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_IncidentIncidentNotes_IncidentNoteId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_IncidentIncidentNotes_IncidentNoteId] ON [dbo].[IncidentIncidentNotes]
(
	[IncidentNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_IncidentNote_NoteTypeId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_IncidentNote_NoteTypeId] ON [dbo].[IncidentNote]
(
	[NoteTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Idx_IncidentType_ShortDesc]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_IncidentType_ShortDesc] ON [dbo].[IncidentType]
(
	[IncidentTypeShortDesc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_NetworkLog_IncidentId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_NetworkLog_IncidentId] ON [dbo].[NetworkLog]
(
	[IncidentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_NetworkLog_IncidentTypeId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_NetworkLog_IncidentTypeId] ON [dbo].[NetworkLog]
(
	[IncidentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_NetworkLog_ServerId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_NetworkLog_ServerId] ON [dbo].[NetworkLog]
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Idx_NoteType_ShortDesc]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_NoteType_ShortDesc] ON [dbo].[NoteType]
(
	[NoteTypeShortDesc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Idx_AspNetServers_ShortName]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_AspNetServers_ShortName] ON [dbo].[Servers]
(
	[ServerShortName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Servers_CompanyId]    Script Date: 2/9/2024 1:24:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_Servers_CompanyId] ON [dbo].[Servers]
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApplicationUserApplicationServer]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationUserApplicationServer_AspNetUsers_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationUserApplicationServer] CHECK CONSTRAINT [FK_ApplicationUserApplicationServer_AspNetUsers_Id]
GO
ALTER TABLE [dbo].[ApplicationUserApplicationServer]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationUserApplicationServer_Servers_ServerId] FOREIGN KEY([ServerId])
REFERENCES [dbo].[Servers] ([ServerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationUserApplicationServer] CHECK CONSTRAINT [FK_ApplicationUserApplicationServer_Servers_ServerId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyId])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_Companies_CompanyId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[EmailTemplates]  WITH CHECK ADD  CONSTRAINT [FK_EmailTemplates_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyId])
GO
ALTER TABLE [dbo].[EmailTemplates] CHECK CONSTRAINT [FK_EmailTemplates_Companies_CompanyId]
GO
ALTER TABLE [dbo].[EmailTemplates]  WITH CHECK ADD  CONSTRAINT [FK_EmailTemplates_IncidentType_IncidentTypeId] FOREIGN KEY([IncidentTypeId])
REFERENCES [dbo].[IncidentType] ([IncidentTypeId])
GO
ALTER TABLE [dbo].[EmailTemplates] CHECK CONSTRAINT [FK_EmailTemplates_IncidentType_IncidentTypeId]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK_Incident_NIC_NIC_Id] FOREIGN KEY([NIC_Id])
REFERENCES [dbo].[NIC] ([NIC_Id])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK_Incident_NIC_NIC_Id]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK_Incident_Servers_ServerId] FOREIGN KEY([ServerId])
REFERENCES [dbo].[Servers] ([ServerId])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK_Incident_Servers_ServerId]
GO
ALTER TABLE [dbo].[IncidentIncidentNotes]  WITH CHECK ADD  CONSTRAINT [FK_IncidentIncidentNotes_Incident_IncidentId] FOREIGN KEY([IncidentId])
REFERENCES [dbo].[Incident] ([IncidentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IncidentIncidentNotes] CHECK CONSTRAINT [FK_IncidentIncidentNotes_Incident_IncidentId]
GO
ALTER TABLE [dbo].[IncidentIncidentNotes]  WITH CHECK ADD  CONSTRAINT [FK_IncidentIncidentNotes_IncidentNote_IncidentNoteId] FOREIGN KEY([IncidentNoteId])
REFERENCES [dbo].[IncidentNote] ([IncidentNoteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IncidentIncidentNotes] CHECK CONSTRAINT [FK_IncidentIncidentNotes_IncidentNote_IncidentNoteId]
GO
ALTER TABLE [dbo].[IncidentNote]  WITH CHECK ADD  CONSTRAINT [FK_IncidentNote_NoteType_NoteTypeId] FOREIGN KEY([NoteTypeId])
REFERENCES [dbo].[NoteType] ([NoteTypeId])
GO
ALTER TABLE [dbo].[IncidentNote] CHECK CONSTRAINT [FK_IncidentNote_NoteType_NoteTypeId]
GO
ALTER TABLE [dbo].[NetworkLog]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLog_Incident_IncidentId] FOREIGN KEY([IncidentId])
REFERENCES [dbo].[Incident] ([IncidentId])
GO
ALTER TABLE [dbo].[NetworkLog] CHECK CONSTRAINT [FK_NetworkLog_Incident_IncidentId]
GO
ALTER TABLE [dbo].[NetworkLog]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLog_IncidentType_IncidentTypeId] FOREIGN KEY([IncidentTypeId])
REFERENCES [dbo].[IncidentType] ([IncidentTypeId])
GO
ALTER TABLE [dbo].[NetworkLog] CHECK CONSTRAINT [FK_NetworkLog_IncidentType_IncidentTypeId]
GO
ALTER TABLE [dbo].[NetworkLog]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLog_Servers_ServerId] FOREIGN KEY([ServerId])
REFERENCES [dbo].[Servers] ([ServerId])
GO
ALTER TABLE [dbo].[NetworkLog] CHECK CONSTRAINT [FK_NetworkLog_Servers_ServerId]
GO

-- ALTER TABLE [dbo].[Servers] DROP CONSTRAINT [FK_Servers_Companies_CompanyId]
-- GO

ALTER TABLE [dbo].[Servers]  WITH CHECK ADD  CONSTRAINT [FK_Servers_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyId])
GO

ALTER TABLE [dbo].[Servers] CHECK CONSTRAINT [FK_Servers_Companies_CompanyId]
GO



USE [master]
GO
ALTER DATABASE [NetIncidentIdentity04] SET  READ_WRITE 
GO
