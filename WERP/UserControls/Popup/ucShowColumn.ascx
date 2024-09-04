<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucShowColumn.ascx.cs" Inherits="UserControls_Popup_ucShowColumn" %>

<f:PageManager ID="pm" runat="server" Language="EN" AjaxAspnetControls="gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast" />    

<table>
  <tr><td>
    <div style="background-color:#dfeaf2; border: 1px solid #c2c2c2; border-top-width:0px; border-bottom-color:#ccc; padding: 6px 0px 6px 8px">
      <table><tr>
        <td><f:TextBox runat="server" ID="tbColumnName" AutoPostBack="true" OnTextChanged="tbColumnName_TextChanged" /></td>
        <td><f:Button runat="server" ID="btnSave" Text="Save" Icon="DatabaseSave" OnClick="btn_Click" /></td>
        <td><f:Button runat="server" ID="btnClose" Text="Close" Icon="None" OnClick="btn_Click" /></td>
      </tr></table>      
    </div>
  </td></tr>   
  <tr><td>
    <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" HeaderStyle-HorizontalAlign="Center" Width="100%" OnRowDataBound="gvData_RowDataBound">
      <Columns>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
          <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
          <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' Enabled='<%# Eval("Mode", "{0}") == "" ? true : false %>' /></ItemTemplate>              
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Column" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"><ItemTemplate>
          <asp:Label runat="server" ID="lbl_Id" Text='<%# Eval("Id") %>' Visible="false" />
          <asp:Label runat="server" ID="lbl_Seq" Text='<%# Eval("Seq") %>' Visible="false" />                      
          <asp:Label runat="server" ID="lbl_ColumnName" Text='<%# Eval("ColumnName") %>' />      
        </ItemTemplate></asp:TemplateField>
      </Columns>
    </asp:GridView>
  </td></tr> 
</table>