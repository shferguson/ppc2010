USE [Providence]
GO
/****** Object:  StoredProcedure [ppc2010].[sp_RefreshArticles]    Script Date: 05/20/2012 23:11:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [ppc2010].[sp_RefreshArticles]
as
truncate table ppc2010.Article
insert into ppc2010.Article
(Id, UmbracoTitle, Title, Date, Text, ScriptureReference)
(select Id, UmbracoTitle, Title, Date, Text, ScriptureReference from ppc2010.view_Articles)
GO
