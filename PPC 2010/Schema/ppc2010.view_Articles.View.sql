CREATE view [ppc2010].[view_Articles] as
select
c.nodeId as Id,
n.text as UmbracoTitle,
title.Title,
[date].[Date],
[text].[Text],
scriptureReference.ScriptureReference
from cmsContent c
inner join cmsContentType ct on ct.alias = 'Article' and c.contentType = ct.nodeId
inner join umbracoNode n on n.id = c.nodeId and n.trashed = 0
inner join cmsDocument d on d.nodeId = c.nodeId and d.published = 1
inner join cmsContentVersion v on ContentId = c.nodeId and VersionDate = (select MAX(VersionDate) from cmsContentVersion where ContentId = c.nodeId)
left outer join (select d.contentNodeId, d.versionId, d.dataNVarChar as Title from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'title') title on title.contentNodeId = c.nodeId and title.versionId = v.VersionId
left outer join (select d.contentNodeId, d.versionId, d.dataDate as [Date] from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'date') [date] on [date].contentNodeId = c.nodeId and [date].versionId = v.VersionId
left outer join (select d.contentNodeId, d.versionId, d.dataNtext as ScriptureReference from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'scriptureReference') scriptureReference on scriptureReference.contentNodeId = c.nodeId and scriptureReference.versionId = v.VersionId
left outer join (select d.contentNodeId, d.versionId, d.dataNtext as [Text] from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'text') [text] on [text].contentNodeId = c.nodeId and [text].versionId = v.VersionId
GO
