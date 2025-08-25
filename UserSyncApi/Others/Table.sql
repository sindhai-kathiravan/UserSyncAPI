USE [MIS]
GO
 
/****** Object:  Table [dbo].[Users]    Script Date: 8/5/2025 11:53:00 AM ******/
SET ANSI_NULLS ON
GO
 
SET QUOTED_IDENTIFIER ON
GO
 
CREATE TABLE [dbo].[Users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [char](20) NOT NULL,
	[user_loginname] [nvarchar](100) NOT NULL,
	[user_loggedonat] [char](100) NOT NULL,
	[user_fullname] [char](30) NOT NULL,
	[user_email] [char](50) NOT NULL,
	[user_initials] [char](4) NOT NULL,
	[user_password] [nvarchar](50) NOT NULL,
	[user_department] [int] NOT NULL,
	[user_loggedin] [bit] NOT NULL,
	[user_inmodule] [int] NOT NULL,
	[g2version] [nchar](20) NULL,
	[lastlogin] [datetime] NULL,
	[os] [char](100) NOT NULL,
	[clr] [char](50) NOT NULL,
	[user_menus] [char](400) NOT NULL,
	[logincount] [int] NOT NULL,
	[libraries_readonly] [bit] NOT NULL,
	[group_company] [int] NOT NULL,
	[screen1_res] [nchar](30) NOT NULL,
	[screen2_res] [nchar](30) NOT NULL,
	[screen3_res] [nchar](30) NOT NULL,
	[screen4_res] [nchar](30) NOT NULL,
	[po_auth_id] [int] NOT NULL,
	[po_auth_all] [bit] NOT NULL,
	[order_alerts] [bit] NOT NULL,
	[po_auth_temp_user_id] [int] NOT NULL,
	[maxordervalue] [int] NOT NULL,
	[outofoffice] [bit] NOT NULL,
	[default_order_department] [int] NOT NULL,
	[porole_id] [int] NOT NULL,
	[deleted] [bit] NOT NULL,
	[alias_username_1] [nchar](10) NULL,
	[invoicebarcodeprinter] [nvarchar](50) NOT NULL,
	[smtpserver] [nvarchar](50) NOT NULL,
	[factory_id] [int] NOT NULL,
	[piecemonitoringaccesslevel] [int] NOT NULL,
	[exclassedit] [int] NOT NULL,
	[timesheetsaccesslevel] [int] NOT NULL,
	[initial_windows] [nvarchar](50) NOT NULL,
	[fabscheduleaccesslevel] [int] NOT NULL,
	[fablinescheduleaccesslevel] [int] NOT NULL,
	[paintlinescheduleaccesslevel] [int] NOT NULL,
	[contractsaccesslevel] [int] NOT NULL,
	[g2updaterversion] [nvarchar](20) NULL,
	[updatelocation_id] [int] NOT NULL,
	[allocationadmin] [bit] NOT NULL,
	[user_password_last_changed] [date] NULL,
	[date_created] [date] NULL,
	[createdbyuser_id] [int] NULL,
	[date_modified] [date] NULL,
	[modifiedbyuser_id] [int] NULL,
	[loggedinoncomputer] [nvarchar](100) NOT NULL,
	[yloc] [char](400) NULL,
	[ylocdsc] [nvarchar](400) NULL,
	[locked] [int] NULL,
	[fattempt] [int] NULL,
	[remarks] [varchar](50) NULL,
	[releasedt] [datetime] NULL,
	[releaseby] [varchar](50) NULL,
	[inactive] [int] NULL,
	[inactiveremarks] [varchar](50) NULL,
	[inactivereleasedt] [datetime] NULL,
	[inactivereleaseby] [varchar](50) NULL,
	[password_attempts] [nvarchar](50) NULL,
	[password_updated_flag] [nvarchar](50) NULL,
	[unlock_date] [datetime] NULL,
CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_loginname]  DEFAULT ('') FOR [user_loginname]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_location]  DEFAULT ('') FOR [user_loggedonat]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_fullname]  DEFAULT ('') FOR [user_fullname]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_email]  DEFAULT ('') FOR [user_email]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_initials]  DEFAULT ('') FOR [user_initials]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_password]  DEFAULT ('') FOR [user_password]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_department]  DEFAULT ((0)) FOR [user_department]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_loggedin]  DEFAULT ((0)) FOR [user_loggedin]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_inmodule]  DEFAULT ((0)) FOR [user_inmodule]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_os]  DEFAULT ('') FOR [os]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_clr]  DEFAULT ('') FOR [clr]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_menus]  DEFAULT ('') FOR [user_menus]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_logincount]  DEFAULT ((0)) FOR [logincount]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_libraries_readonly]  DEFAULT ((0)) FOR [libraries_readonly]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_group_company]  DEFAULT ((6)) FOR [group_company]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_screen1_res]  DEFAULT ('') FOR [screen1_res]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_screen1_res1]  DEFAULT ('') FOR [screen2_res]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_screen1_res2]  DEFAULT ('') FOR [screen3_res]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_screen1_res3]  DEFAULT ('') FOR [screen4_res]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_po_auth_id]  DEFAULT ((0)) FOR [po_auth_id]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_po_auth_all]  DEFAULT ((0)) FOR [po_auth_all]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_order_alerts]  DEFAULT ((0)) FOR [order_alerts]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_po_auth_temp_user_id]  DEFAULT ((0)) FOR [po_auth_temp_user_id]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_maxordervalue]  DEFAULT ((0)) FOR [maxordervalue]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_outofoffice]  DEFAULT ((0)) FOR [outofoffice]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_default_order_department]  DEFAULT ((0)) FOR [default_order_department]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_porole_id]  DEFAULT ((0)) FOR [porole_id]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_deleted]  DEFAULT ((0)) FOR [deleted]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_invoicebarcodeprinter]  DEFAULT ('') FOR [invoicebarcodeprinter]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_smtpserver]  DEFAULT ('') FOR [smtpserver]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_factory_id]  DEFAULT ((1)) FOR [factory_id]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_piecemonitoringaccesslevel]  DEFAULT ((0)) FOR [piecemonitoringaccesslevel]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_exclassedit]  DEFAULT ((0)) FOR [exclassedit]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_timesheetsaccesslevel]  DEFAULT ((0)) FOR [timesheetsaccesslevel]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_initial_windows]  DEFAULT ('') FOR [initial_windows]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_fabscheduleaccesslevel]  DEFAULT ((0)) FOR [fabscheduleaccesslevel]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_fablinescheduleaccesslevel]  DEFAULT ((0)) FOR [fablinescheduleaccesslevel]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_paintlinescheduleaccesslevel]  DEFAULT ((0)) FOR [paintlinescheduleaccesslevel]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_contractsaccesslevel]  DEFAULT ((0)) FOR [contractsaccesslevel]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_updatelocation_id]  DEFAULT ((1)) FOR [updatelocation_id]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_allocationadmin]  DEFAULT ((0)) FOR [allocationadmin]
GO
 
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_loggedinoncomputer]  DEFAULT ('') FOR [loggedinoncomputer]
GO
 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Initials' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'user_initials'
GO
 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SEV = 1, WARDS = 2, ROW = 3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'group_company'
GO
 