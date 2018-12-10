USE [master]
GO
/****** Object:  Database [PlagiarismDB]    Script Date: 10 Dec 2018 5:03:35 PM ******/
CREATE DATABASE [PlagiarismDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PlagiarismDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQL16\MSSQL\DATA\PlagiarismDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PlagiarismDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQL16\MSSQL\DATA\PlagiarismDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [PlagiarismDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PlagiarismDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PlagiarismDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PlagiarismDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PlagiarismDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PlagiarismDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PlagiarismDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [PlagiarismDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PlagiarismDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PlagiarismDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PlagiarismDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PlagiarismDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PlagiarismDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PlagiarismDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PlagiarismDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PlagiarismDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PlagiarismDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PlagiarismDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PlagiarismDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PlagiarismDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PlagiarismDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PlagiarismDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PlagiarismDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [PlagiarismDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PlagiarismDB] SET RECOVERY FULL 
GO
ALTER DATABASE [PlagiarismDB] SET  MULTI_USER 
GO
ALTER DATABASE [PlagiarismDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PlagiarismDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PlagiarismDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PlagiarismDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PlagiarismDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PlagiarismDB', N'ON'
GO
ALTER DATABASE [PlagiarismDB] SET QUERY_STORE = OFF
GO
USE [PlagiarismDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [PlagiarismDB]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 10 Dec 2018 5:03:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 10 Dec 2018 5:03:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 10 Dec 2018 5:03:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 10 Dec 2018 5:03:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 10 Dec 2018 5:03:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 10 Dec 2018 5:03:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[FullName] [varchar](256) NOT NULL,
	[ClassID] [uniqueidentifier] NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Assignments]    Script Date: 10 Dec 2018 5:03:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assignments](
	[Id] [uniqueidentifier] NOT NULL,
	[AssignmentName] [varchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.Assignments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentClasses]    Script Date: 10 Dec 2018 5:03:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentClasses](
	[Id] [uniqueidentifier] NOT NULL,
	[ClassName] [varchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.StudentClasses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubmittedAssignmentCommonAppUsers]    Script Date: 10 Dec 2018 5:03:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubmittedAssignmentCommonAppUsers](
	[SubmittedAssignment_Id] [uniqueidentifier] NOT NULL,
	[CommonAppUser_Id] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.SubmittedAssignmentCommonAppUsers] PRIMARY KEY CLUSTERED 
(
	[SubmittedAssignment_Id] ASC,
	[CommonAppUser_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubmittedAssignments]    Script Date: 10 Dec 2018 5:03:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubmittedAssignments](
	[Id] [uniqueidentifier] NOT NULL,
	[Counter] [int] NOT NULL,
	[UploadedFilePath] [varchar](max) NULL,
	[PercentageInteger] [int] NOT NULL,
	[Percentage] [real] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsAccepted] [bit] NOT NULL,
	[IsChecked] [bit] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[Assignment_Id] [uniqueidentifier] NOT NULL,
	[SubmissionDate] [datetime] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Score] [smallint] NOT NULL,
	[Class_Id] [uniqueidentifier] NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SubmittedAssignments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAssignments]    Script Date: 10 Dec 2018 5:03:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAssignments](
	[UserId] [nvarchar](128) NOT NULL,
	[AssignmentId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.UserAssignments] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[AssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleId]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClassID]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_ClassID] ON [dbo].[AspNetUsers]
(
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CommonAppUser_Id]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_CommonAppUser_Id] ON [dbo].[SubmittedAssignmentCommonAppUsers]
(
	[CommonAppUser_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SubmittedAssignment_Id]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_SubmittedAssignment_Id] ON [dbo].[SubmittedAssignmentCommonAppUsers]
(
	[SubmittedAssignment_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Assignment_Id]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_Assignment_Id] ON [dbo].[SubmittedAssignments]
(
	[Assignment_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClassId]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_ClassId] ON [dbo].[SubmittedAssignments]
(
	[Class_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssignmentId]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_AssignmentId] ON [dbo].[UserAssignments]
(
	[AssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 10 Dec 2018 5:03:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[UserAssignments]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SubmittedAssignments] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [SubmissionDate]
GO
ALTER TABLE [dbo].[SubmittedAssignments] ADD  DEFAULT ((0)) FOR [Score]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUsers_dbo.StudentClasses_ClassID] FOREIGN KEY([ClassID])
REFERENCES [dbo].[StudentClasses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_dbo.AspNetUsers_dbo.StudentClasses_ClassID]
GO
ALTER TABLE [dbo].[SubmittedAssignmentCommonAppUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SubmittedAssignmentCommonAppUsers_dbo.AspNetUsers_CommonAppUser_Id] FOREIGN KEY([CommonAppUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubmittedAssignmentCommonAppUsers] CHECK CONSTRAINT [FK_dbo.SubmittedAssignmentCommonAppUsers_dbo.AspNetUsers_CommonAppUser_Id]
GO
ALTER TABLE [dbo].[SubmittedAssignmentCommonAppUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SubmittedAssignmentCommonAppUsers_dbo.SubmittedAssignments_SubmittedAssignment_Id] FOREIGN KEY([SubmittedAssignment_Id])
REFERENCES [dbo].[SubmittedAssignments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubmittedAssignmentCommonAppUsers] CHECK CONSTRAINT [FK_dbo.SubmittedAssignmentCommonAppUsers_dbo.SubmittedAssignments_SubmittedAssignment_Id]
GO
ALTER TABLE [dbo].[SubmittedAssignments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SubmittedAssignments_dbo.Assignments_Assignment_Id] FOREIGN KEY([Assignment_Id])
REFERENCES [dbo].[Assignments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubmittedAssignments] CHECK CONSTRAINT [FK_dbo.SubmittedAssignments_dbo.Assignments_Assignment_Id]
GO
ALTER TABLE [dbo].[SubmittedAssignments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SubmittedAssignments_dbo.StudentClasses_ClassId] FOREIGN KEY([Class_Id])
REFERENCES [dbo].[StudentClasses] ([Id])
GO
ALTER TABLE [dbo].[SubmittedAssignments] CHECK CONSTRAINT [FK_dbo.SubmittedAssignments_dbo.StudentClasses_ClassId]
GO
ALTER TABLE [dbo].[UserAssignments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserAssignments_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAssignments] CHECK CONSTRAINT [FK_dbo.UserAssignments_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[UserAssignments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserAssignments_dbo.Assignments_AssignmentId] FOREIGN KEY([AssignmentId])
REFERENCES [dbo].[Assignments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAssignments] CHECK CONSTRAINT [FK_dbo.UserAssignments_dbo.Assignments_AssignmentId]
GO
USE [master]
GO
ALTER DATABASE [PlagiarismDB] SET  READ_WRITE 
GO
