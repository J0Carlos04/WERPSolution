<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucSort.ascx.cs" Inherits="UserControls_Common_ucSort" %>

<f:PageManager ID="pm" runat="server" Language="EN" AjaxAspnetControls="gvData" />

<table>
  <tr><td>
    <div style=" margin-top:-5px; margin-left:-5px; margin-right:-5px; background-color:#dfeaf2; border: 1px solid #c2c2c2; border-top-width:0px; border-bottom-color:#ccc; padding: 6px 0px 6px 8px">
      <table>
        <tr>
          <td><f:Button runat="server" ID="btnSearch" Text="Sorting" Icon="Sorting" OnClick="btn_Click"  /></td>
          <td><f:Button runat="server" ID="btnAdd" Text="Add" Icon="Add" OnClick="btn_Click"  /></td>
          <td><f:Button runat="server" ID="btnReset" Text="Reset" Icon="ArrowRefresh" OnClick="btn_Click"  /></td>
        </tr>
      </table>
    </div>
  </td></tr>
  <tr><td>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvData_RowDataBound" HeaderStyle-HorizontalAlign="Center" CssClass="table table-striped table-bordered table-hover" >
        <Columns>
        <asp:TemplateField HeaderText="Sort Field">
            <ItemTemplate><asp:DropDownList runat="server" ID="ddl_SortField" CssClass="form-select form-select-sm" /></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Sort Direction">
            <ItemTemplate>
            <asp:DropDownList runat="server" ID="ddl_SortDirection" CssClass="form-select form-select-sm" >
                <asp:ListItem Value="asc">Ascending</asp:ListItem>
                <asp:ListItem Value="desc">Descending</asp:ListItem>
            </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate><asp:ImageButton runat="server" ID="imbDelete" ImageUrl="~/res/images/btnDelete.png" OnClick="imb_Click" /></ItemTemplate>
        </asp:TemplateField>
        </Columns>       
    </asp:GridView>
  </td></tr>
</table>