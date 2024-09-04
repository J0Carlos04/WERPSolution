<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucPaging.ascx.cs" Inherits="UserControls_Common_ucPaging" %>

<div><table><tr valign="top">
  <td valign="middle" style="border-width : 0px;">
    <asp:LinkButton runat="server" ID="lnkFirst" Text="First" OnClick="linkPaging_Click" CausesValidation="false" Enabled="false" />&nbsp;
  </td>
  <td valign="middle" style="border-width : 0px;">
    <asp:LinkButton runat="server" ID="lnkPrevious" Text="Previous" OnClick="linkPaging_Click" CausesValidation="false" Enabled="false" />&nbsp;
  </td>
  <td valign="middle" style="border-width : 0px;"><asp:DropDownList runat="server" ID="ddlPage" OnSelectedIndexChanged="ddl_paging_ItemCommand" AutoPostBack="True" CssClass="form-select form-select-sm" Width="70" /></td>
  <td valign="middle" style="border-width : 0px;">&nbsp;<asp:LinkButton runat="server" ID="lnkNext" Text="Next" OnClick="linkPaging_Click" CausesValidation="false" Enabled="true" />&nbsp;</td>
  <td valign="middle" style="border-width : 0px;"><asp:LinkButton runat="server" ID="lnkLast" Text="Last" OnClick="linkPaging_Click" CausesValidation="false" Enabled="true" /></td>
</tr></table></div>


