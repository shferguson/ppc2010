<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactForm.ascx.cs" Inherits="PPC_2010.ContactForm" %>
<form runat="server">
  <div style="
  <h1><asp:Label runat="server" id="title"/></h1>
  <p>
    Please send us your message below. 
  </p>
  <table>
    <tr>
      <td><label for="email">Your Email Address:&nbsp;</label></td>
      <td><asp:TextBox runat="server" ID="email" style="width:400px"></asp:TextBox></td>
     <td><asp:RequiredFieldValidator ID="emailValidator" runat="server" ControlToValidate="email" ErrorMessage="Please enter your email"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
      <td><label for="subject">Subject:&nbsp;</label></td>
      <td><asp:TextBox runat="server" ID="subject" style="width:400px"></asp:TextBox></td>
    </tr>
    <tr>
      <td style="vertical-align:top;text-align:left;"><label for="message" \>Your Message:</label></td>
      <td><asp:TextBox runat="server" ID="message" TextMode="MultiLine" style="width:400px;height:200px"></asp:TextBox></td>
      <td style="vertical-align:top;text-align:left;"><asp:RequiredFieldValidator ID="messageValidator" runat="server" ControlToValidate="message" ErrorMessage="Please enter your message"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
      <td></td>
      <td style="vertical-align:top;text-align:right;"><button style="width:100px" runat="server" type="submit">Send</button></td>
    </tr>
  </table>
</form>