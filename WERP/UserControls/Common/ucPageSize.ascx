<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucPageSize.ascx.cs" Inherits="ucPageSize" %>

<table><tr>
  <td>Show&nbsp;</td>
  <td>
    <asp:DropDownList ID="ddlNumberItem" runat="server" AutoPostBack="True" onselectedindexchanged="ddl_SelectedIndexChanged" CssClass="form-select form-select-sm" Width="70">                            
      <asp:ListItem>10</asp:ListItem>
      <asp:ListItem>20</asp:ListItem>
      <asp:ListItem>30</asp:ListItem>
      <asp:ListItem>40</asp:ListItem>
      <asp:ListItem>50</asp:ListItem>
      <asp:ListItem>100</asp:ListItem>
      <asp:ListItem>200</asp:ListItem>
      <asp:ListItem>ALL</asp:ListItem>
    </asp:DropDownList>
  </td>
  <td>&nbsp;Item</td>
</tr></table>
  
