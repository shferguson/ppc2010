<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactForm.ascx.cs" Inherits="PPC_2010.ContactForm" %>
<form runat="server">
  <h1><asp:Label runat="server" id="title"/></h1>
  <p>
    Please send us your message below. 
  </p>
  <asp:ValidationSummary class="error" runat="server" ValidationGroup="email" />
  <table>
    <tr>
      <td><label for="email">Your Email Address:&nbsp;</label></td>
      <td>
        <asp:TextBox runat="server" ID="email" style="width:400px"></asp:TextBox>
        <asp:RequiredFieldValidator class="error" ID="emailValidator" runat="server" ControlToValidate="email" Text="*" ErrorMessage="Please enter your email address" ValidationGroup="email" Display="Dynamic" SetFocusOnError="true" EnableClientScript="true" />
        <asp:RegularExpressionValidator class="error" ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="email"  Text="*" Display="Dynamic" ErrorMessage="Please check your email address" ValidationGroup="email" SetFocusOnError="true" EnableClientScript="true"/>
      </td>
    </tr>
    <tr>
      <td><label for="name">Your Name:</label></td>
      <td><asp:TextBox runat="server" ID="name" style="width:400px"></asp:TextBox></td>
    </tr>
    <tr>
      <td><label for="subject">Subject:&nbsp;</label></td>
      <td><asp:TextBox runat="server" ID="subject" style="width:400px"></asp:TextBox></td>
    </tr>
    <tr>
      <td style="vertical-align:top;text-align:left;"><label for="message">Your Message:</label></td>
      <td>
        <asp:TextBox runat="server" ID="message" TextMode="MultiLine" style="width:400px;height:200px"></asp:TextBox>
        <asp:RequiredFieldValidator class="error" style="vertical-align:top" ID="messageValidator" runat="server" ControlToValidate="message" Text="*" ErrorMessage="Please enter a message" ValidationGroup="email" Display="Dynamic" SetFocusOnError="true" EnableClientScript="true" />
      </td>
    </tr>
    <tr>
      <td></td>
      <td style="vertical-align:top;text-align:right;"><button style="width:100px" runat="server" type="submit" causesvalidation="true" ValidationGroup="email">Send</button></td>
    </tr>
  </table>
</form>