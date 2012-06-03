CREATE view [ppc2010].[view_Prevalues] as
select pv.id as Id, n.text as Name, dt.dbType as DbType, pv.value as Value, pv.sortorder as SortOrder
from umbracoNode n
inner join cmsDataType dt on dt.nodeId = n.id
inner join cmsDataTypePreValues pv on pv.datatypeNodeId = dt.nodeId
GO
