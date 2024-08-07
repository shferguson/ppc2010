CREATE TABLE [ppc2010].[Article](
	[Id] [int] NOT NULL,
	[UmbracoTitle] [nvarchar](4000) NULL,
	[Title] [nvarchar](4000) NULL,
	[Date] [datetime] NULL,
	[Text] [ntext] NULL,
	[ScriptureReference] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
