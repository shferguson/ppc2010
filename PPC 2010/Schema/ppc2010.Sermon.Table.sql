SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ppc2010].[Sermon](
	[Id] [int] NOT NULL,
	[UmbracoTitle] [nvarchar](255) NULL,
	[RecordingDate] [datetime] NULL,
	[Title] [nvarchar](255) NULL,
	[SpeakerName] [nvarchar](255) NULL,
	[RecordingSession] [nvarchar](255) NULL,
	[SermonSeries] [nvarchar](255) NULL,
	[Book] [nvarchar](255) NULL,
	[StartChapter] [int] NULL,
	[StartVerse] [int] NULL,
	[EndChapter] [int] NULL,
	[EndVerse] [int] NULL,
	[ScriptureReferenceText] [nvarchar](255) NULL,
	[AudioFile] [nvarchar](255) NULL,
 CONSTRAINT [PK_Sermon] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
