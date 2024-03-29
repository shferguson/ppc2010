CREATE view [ppc2010].[view_Sermons] as
select
c.nodeId as Id,
n.text as UmbracoTitle,
recordingDate.RecordingDate,
title.Title,
speakerName.SpeakerName,
speakerName.SpeakerNameId,
recordingSession.RecordingSession,
recordingSession.RecordingSessionId,
sermonSeries.SermonSeries,
sermonSeries.SermonSeriesId,
book.Book,
startChapter.StartChapter,
startVerse.StartVerse,
endChapter.EndChapter,
endVerse.EndVerse,
isnull(scriptureReferenceText.ScriptureReferenceText, '') as ScriptureReferenceText,
audioFile.AudioFile
from cmsContent c
inner join cmsContentType ct on ct.alias = 'Sermon' and c.contentType = ct.nodeId
inner join umbracoNode n on n.id = c.nodeId and n.trashed = 0
left outer join (select d.contentNodeId, d.dataNVarChar as Title from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'title') title on title.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, d.dataDate as RecordingDate from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'recordingDate') recordingDate on recordingDate.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, pv.Value as SpeakerTitle, pv.id as SpeakerTitleId from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNVarChar as nvarchar(10)) as int) = pv.id and pt.alias = 'speakerTitle') speakerTitle on speakerTitle.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, pv.Value as SpeakerName, pv.id as SpeakerNameId from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNText as nvarchar(10)) as int) = pv.id and pt.alias = 'speakerName') speakerName on speakerName.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, pv.Value as RecordingSession, pv.id as RecordingSessionId from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNText as nvarchar(10)) as int) = pv.id and pt.alias = 'recordingSession') recordingSession on recordingSession.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, pv.Value as SermonSeries, pv.id as SermonSeriesId from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNText as nvarchar(10)) as int) = pv.id and pt.alias = 'sermonSeries') sermonSeries on sermonSeries.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, pv.Value as Book from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNVarChar as nvarchar(10)) as int) = pv.id and pt.alias = 'book') book on book.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, d.dataInt as StartChapter from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'startChapter') startChapter on startChapter.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, d.dataInt as StartVerse from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'startVerse') startVerse on startVerse.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, d.dataInt as EndChapter from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'endChapter') endChapter on endChapter.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, d.dataInt as EndVerse from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'endVerse') endVerse on endVerse.contentnodeid = c.nodeId
left outer join (select d.contentNodeId, d.dataNVarChar as ScriptureReferenceText from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'scriptureReferenceText') scriptureReferenceText on scriptureReferenceText.contentnodeid = c.nodeId
left outer  join (select d.contentNodeId, d.dataNVarChar as AudioFile from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'audioFile') audioFile on audioFile.contentnodeid = c.nodeId

GO