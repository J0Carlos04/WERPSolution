<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectSOItem.aspx.cs" Inherits="Pages_WorkOrder_SelectSOItem" %>

<%@ Register Src="~/UserControls/Common/ucPageSize.ascx" TagName="ucPageSize" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>WERP</title>
  <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
  <script lang="javascript" type="text/javascript" src="../../res/js/JScript.js"></script>
</head>
<body>
  <form id="form1" runat="server">
    <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast" />
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
                    <asp:Literal runat="server" ID="ltrl_UseSKU" Text='<%# Eval("UseSKU")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
                  </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Item Code">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_StockReceivedItemId" Text='<%# Eval("StockReceivedItemId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_WarehouseId" Text='<%# Eval("WarehouseId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_RackId" Text='<%# Eval("RackId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_ItemCode" Text='<%# Eval("ItemCode") %>' />
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Warehouse">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_Warehouse" Text='<%# Eval("Warehouse") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rack">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_Rack" Text='<%# Eval("Rack") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UsedQty">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_UsedQty" Text='<%# Eval("UsedQty") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UnusedQty">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_UnusedQty" Text='<%# Eval("UnusedQty") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                  <HeaderTemplate><asp:Literal runat="server" ID="ltrlQty" Text="Qty" /></HeaderTemplate>
                  <ItemTemplate><div style="width:60px"><asp:TextBox runat="server" ID="tb_Qty" class="form-control form-control-sm" /></div></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SKU">
                  <ItemTemplate>
                    <div style="width:120px">
                      <asp:DropDownList runat="server" ID="ddlSKU" Visible='<%# Eval("StockReceivedItemId").IsZero() %>' AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" class="form-select form-select-sm" />
                      <asp:Literal runat="server" ID="ltrl_SKU" Text='<%# Eval("SKU") %>' Visible='<%# Eval("StockReceivedItemId").IsNotZero() %>' />
                    </div>
                    </ItemTemplate>
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
