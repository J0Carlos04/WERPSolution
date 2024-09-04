<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectWOItem.aspx.cs" Inherits="Pages_WorkOrder_SelectWOItem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>WERP</title>
  <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
  <script lang="javascript" type="text/javascript" src="../../res/js/JScript.js"></script>
</head>
<body>
  <form id="form1" runat="server">
    <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvData" />
    <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region" EnableAjaxLoading="true" AjaxLoadingType="Default">
      <Toolbars>
        <f:Toolbar runat="server" ID="tlb">
          <Items>
            <f:Button runat="server" ID="btnSelect" Text="Select" Icon="DatabaseSave" OnClick="btnSelect_Click" />
          </Items>
        </f:Toolbar>
      </Toolbars>
      <Items>
        <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" Title="Bank Account" ShowBorder="true" ShowHeader="false" AutoScroll="true" EnableAjaxLoading="true" AjaxLoadingType="Default">
          <Content>
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvData_RowDataBound">
              <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                  <HeaderTemplate>
                    <asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                  <ItemTemplate>
                    <asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Detail">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_WorkDetail" Text='<%# Eval("WorkDetail") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_WorkUpdateDate" Text='<%# $"{Eval("WorkUpdateDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("WorkUpdateDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Code">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="lttl_StockOutItemId" Text='<%# Eval("StockOutItemId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_ItemCode" Text='<%# Eval("ItemCode") %>' />
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Used Qty" ItemStyle-HorizontalAlign="Right">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_UsedQty" Text='<%# Eval("UsedQty") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                  <HeaderTemplate>
                    <asp:Literal runat="server" ID="ltrlQty" Text="Qty" /></HeaderTemplate>
                  <ItemTemplate>
                    <asp:TextBox runat="server" ID="tb_Qty" class="form-control form-control-sm" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SKU">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_SKU" Text='<%# Eval("SKU") %>' /></ItemTemplate>
                </asp:TemplateField>
              </Columns>
              <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>

          </Content>
        </f:ContentPanel>
      </Items>
    </f:Panel>
  </form>
</body>
</html>
