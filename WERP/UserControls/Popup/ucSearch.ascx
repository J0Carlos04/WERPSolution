<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucSearch.ascx.cs" Inherits="UserControls_Common_ucSearch" %>

<f:PageManager ID="pm" runat="server" Language="EN" AjaxAspnetControls="gvFilter" />

<table>
  <tr><td>
    <div style=" margin-top:-5px; margin-left:-5px; margin-right:-5px; background-color:#dfeaf2; border: 1px solid #c2c2c2; border-top-width:0px; border-bottom-color:#ccc; padding: 6px 0px 6px 8px">
      <table>
        <tr>
          <td><f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" OnClick="btn_Click"  /></td>
          <td><f:Button runat="server" ID="btnAddFilter" Text="Add Filter" Icon="Add" OnClick="btn_Click"  /></td>
          <td><f:Button runat="server" ID="btnReset" Text="Reset" Icon="ArrowRefresh" OnClick="btn_Click"  /></td>
        </tr>
      </table>
    </div>
  </td></tr>
  <tr><td>
    <asp:GridView runat="server" ID="gvFilter" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvFilter_RowDataBound" HeaderStyle-HorizontalAlign="Center">
      <Columns>
        <asp:TemplateField HeaderText="Condition" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"><ItemTemplate>
          <asp:Label runat="server" ID="lbl_Seq" Text='<%# Eval("Seq") %>' Visible="false" />
          <asp:DropDownList runat="server" ID="ddl_Logicaloperator" CssClass="form-select form-select-sm" Visible="false" Width="74" >
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>AND</asp:ListItem>
            <asp:ListItem>OR</asp:ListItem>                                                  
          </asp:DropDownList>          
        </ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Field" HeaderStyle-Width="150px"><ItemTemplate>
          <asp:Label runat="server" ID="lbl_SearchName" Text='<%# Eval("Value") %>' Visible="false" />
          <asp:DropDownList runat="server" ID="ddl_Field" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" CssClass="form-select form-select-sm" />
        </ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Operator"><ItemTemplate><asp:DropDownList runat="server" ID="ddl_Operator" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" CssClass="form-select form-select-sm" Width="100" /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Value"><ItemTemplate>                          
          <div runat="server" id="dvValue" style="min-width:200px;"><asp:TextBox runat="server" ID="tb_Value" Text='<%# Eval("Value") %>' CssClass="form-control form-control-sm" /></div>
          <asp:DropDownList runat="server" ID="ddl_BoolValue" CssClass="form-select form-select-sm" Visible="false" >
            <asp:ListItem>True</asp:ListItem>
            <asp:ListItem>False</asp:ListItem>
            </asp:DropDownList>
          <div runat="server" id="dvStartValue" style="display:inline-block;"><asp:TextBox runat="server" ID="tb_StartValue" Text='<%# Eval("StartValue") %>' CssClass="form-control form-control-sm" /></div>
          <div runat="server" id="dvEndValue" style="display:inline-block"><asp:TextBox runat="server" ID="tb_EndValue" Text='<%# Eval("EndValue") %>' CssClass="form-control form-control-sm" /></div>
        </ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Priority"><ItemTemplate><asp:DropDownList runat="server" ID="ddl_Priority" CssClass="form-select form-select-sm" Width="53" /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Del"><ItemTemplate>                 
          <asp:ImageButton runat="server" ID="imbSearch" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" style="line-height:0px; display:none;" />
          <asp:ImageButton runat="server" ID="imbDelete" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" style="line-height:0px;" />          
        </ItemTemplate></asp:TemplateField>
    </Columns>
    </asp:GridView>
  </td></tr>
</table>

