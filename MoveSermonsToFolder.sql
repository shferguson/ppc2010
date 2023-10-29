-- Procedure to reorder sermons folder
-- Change the date everywhere it is listed below
-- Create a SermonFolder with the date you wish to have
-- Run the first query to determine that you've correctly created the folder
-- Run the second query to make sure that you will select the correct sermons
-- Run the UPDATE to do the update


SELECT [id] FROM [providence].[dbo].[umbracoNode]  WHERE [text] = '2022' and [trashed] = 0

SELECT TOP (1000) [id]
      ,[trashed]
      ,[parentID]
      ,[nodeUser]
      ,[level]
      ,[path]
      ,[sortOrder]
      ,[uniqueID]
      ,[text]
      ,[nodeObjectType]
      ,[createDate]
FROM [providence].[dbo].[umbracoNode]
WHERE [text] LIKE '__/__/2022 - %' and [trashed] = 0

UPDATE n
SET n.[level] = (SELECT [level] FROM [providence].[dbo].[umbracoNode]  WHERE [text] = '2022' and [trashed] = 0)+1,
    n.parentId = (SELECT [id] FROM [providence].[dbo].[umbracoNode]  WHERE [text] = '2022' and [trashed] = 0),
 n.[path] = CAST( (SELECT [path] FROM [providence].[dbo].[umbracoNode]  WHERE [text] = '2022' and [trashed] = 0) AS nvarchar(50)) + ',' + CAST(n.id as nvarchar(50))
FROM [providence].[dbo].[umbracoNode] n
WHERE n.[text]  LIKE '__/__/2022 - %' AND [trashed] = 0