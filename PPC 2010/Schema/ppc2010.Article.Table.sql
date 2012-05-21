USE [Providence]
GO
/****** Object:  Table [ppc2010].[Article]    Script Date: 05/20/2012 23:11:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ppc2010].[Article](
	[Id] [int] NOT NULL,
	[UmbracoTitle] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Date] [datetime] NULL,
	[Text] [ntext] NULL,
	[ScriptureReference] [nvarchar](255) NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
