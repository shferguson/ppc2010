USE [Providence]
GO
/****** Object:  StoredProcedure [ppc2010].[sp_RefreshSermons]    Script Date: 05/20/2012 23:11:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [ppc2010].[sp_RefreshSermons]
as
truncate table ppc2010.Sermon
insert into ppc2010.Sermon
(Id, UmbracoTitle, RecordingDate, Title, SpeakerName, RecordingSession, SermonSeries, Book, StartChapter, EndChapter, EndVerse, ScriptureReferenceText, AudioFile)
(select Id, UmbracoTitle, RecordingDate, Title, SpeakerName, RecordingSession, SermonSeries, Book, StartChapter, EndChapter, EndVerse, ScriptureReferenceText, AudioFile from ppc2010.view_Sermons)
GO
