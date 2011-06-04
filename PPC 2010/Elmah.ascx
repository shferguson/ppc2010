<%@ Control Language="C#" %>
<%--
    Add this control to the config/dashboard.config to get it to display in the umbraco admin screens

   <section>
    <areas>
      <area>developer</area>
    </areas>
    <tab caption="Error Logging Modules and Handlers for ASP.NET">
      <control>/usercontrols/ELMAH.ascx</control>
    </tab>
  </section>
--%>

<script language="JavaScript">
<!--
window.onresize=autoResize

function autoResize() {
    jQuery("#elmahFrame").height(jQuery(window).height() - 100);
}
//-->
</script>

<iframe id="elmahFrame" width="100%" scrolling="auto" src="elmah.axd" style="margin-top:5px;" onLoad="autoResize()" ></iframe>



