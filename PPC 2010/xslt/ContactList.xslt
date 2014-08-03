<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:msxml="urn:schemas-microsoft-com:xslt" 
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" 
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets ">

<xsl:output method="xml" omit-xml-declaration="yes" />

<xsl:param name="currentPage"/>

<!-- Input the documenttype you want here -->
<xsl:variable name="documentTypeAlias" select="string('Contact')"/>
    
<xsl:template match="/">

<!-- The fun starts here -->
<ul>
<xsl:for-each select="$currentPage/* [name() = $documentTypeAlias and string(umbracoNaviHide) != '1']">
  
  <xsl:value-of select="umbraco.library:GetMedia(1537, 0)/data [@alias = 'umbracoFile']"/>

  <div class="contact">
    <div class="contactImage">
        <xsl:variable name="mediaId" select="umbraco.library:Item(@id, 'image')" />
        
      
      <xsl:if test="$mediaId">
        <xsl:value-of select="$mediaId" />
        <xsl:value-of select="data[@alias='image']" />
        <!--<xsl:value-of select="umbraco.library:GetMedia(current()/node[@alias='image'], 0)/data [@alias = 'umbracoFile']"/>-->
        
      </xsl:if>
    </div>
    <div class="contactInfo">
      <h4><xsl:value-of select="umbraco.library:Item(@id, 'name')" /></h4>
      <p />
      <xsl:value-of select="umbraco.library:Item(@id, 'position')" /><br />
      Phone:&nbsp;<xsl:value-of select="umbraco.library:Item(@id, 'phone')" /><br />
      Email:&nbsp;
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="umbraco.library:Item(@id, 'email')" />
        </xsl:attribute>
        <xsl:value-of select="umbraco.library:Item(@id, 'email')" />
      </a>
    </div>
  </div>
  <li>
    
    
    <a href="{umbraco.library:NiceUrl(@id)}">
      <xsl:value-of select="@nodeName"/>
    </a>
  </li>
</xsl:for-each>
</ul>

</xsl:template>

</xsl:stylesheet>