<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockReceived.aspx.cs" Inherits="Pages_Inventory_StockReceived" %>
<%@ Register src="~/UserControls/Common/ucPageSize.ascx" tagname="ucPageSize" tagprefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>W.ERP</title>    
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>     
</head>
<body>
<form id="form1" runat="server">

<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvFilter,gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast"  />    

<f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region" EnableAjaxLoading="true" AjaxLoadingType="Default">
  <Toolbars>
    <f:Toolbar runat="server" ID="tTop" Position="Top"><Items>
      <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />
      <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />
      <f:Button runat="server" ID="btnAdd" Text="Add" Icon="Add" OnClick="btn_Click" />      
      <f:Button runat="server" ID="btnExportExcel" Text="Export Excel" Icon="Excel" OnClick="btn_Click" EnableAjax="false" DisableControlBeforePostBack="false"  />
      <f:ToolbarFill runat="server" ID="tf1" />
      <f:ContentPanel runat="server" ID="cpPageSize" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc1:ucPageSize ID="ups" runat="server" OnSizeChanged="ups_Changed" /></Content></f:ContentPanel>
      <f:ToolbarFill runat="server" ID="tf2" />
      <f:ContentPanel runat="server" ID="cpRowCount" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><asp:Label ID="lblRowCount" runat="server" /></Content></f:ContentPanel>
      <f:Button runat="server" ID="btnRefresh" Text="Refresh" Icon="Reload" EnablePostBack="false" OnClientClick="javascript:self.location.reload();" />
    </Items></f:Toolbar>
  </Toolbars>
  <Items>        
    <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" Title="Bank Account" ShowBorder="true" ShowHeader="false" AutoScroll="true" EnableAjaxLoading="true" AjaxLoadingType="Default">      
      <Content><div class="wrapper-suppress">                    
        <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvData_RowDataBound" Width="100%" HeaderStyle-HorizontalAlign="Center">
          <Columns>            
            <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
              <ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_StockReceivedId" Text='<%# Eval("StockReceivedId")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action"><ItemTemplate>
              <div style="display:flex; justify-content:center; column-gap:4px;">
                <asp:LinkButton runat="server" ID="lbEdit" Text="Edit" CommandArgument='<%# Eval("StockReceivedId")  %>' CommandName='<%# Eval("StockOrderId")  %>' OnClick="lb_Click"  Font-Underline="false" />
                <asp:LinkButton runat="server" ID="lbRetur" Text="Retur" CommandArgument='<%# Eval("Id")  %>' OnClick="lb_Click"  Font-Underline="false" />                
              </div>              
		    </ItemTemplate></asp:TemplateField>                           		    
            
            <asp:TemplateField HeaderText="Stock&nbsp;Order" ><ItemTemplate><asp:LinkButton runat="server" ID="lb_StockOrderCode" Text='<%# Eval("StockOrderCode") %>' CommandArgument='<%# Eval("StockOrderId") %>' onclick="lb_Click" /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Item&nbsp;Code" ><ItemTemplate><asp:LinkButton runat="server" ID="lb_ItemCode" Text='<%# Eval("ItemCode") %>' CommandArgument='<%# Eval("ItemId") %>' onclick="lb_Click" /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Item&nbsp;Name" ><ItemTemplate><div style="min-width:200px;"><asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Warehouse" ><ItemTemplate><div style="min-width:150px;"><asp:Literal runat="server" ID="ltrl_Warehouse" Text='<%# Eval("Warehouse") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Rack" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Rack" Text='<%# Eval("Rack") %>' /></ItemTemplate></asp:TemplateField>                        
            <asp:TemplateField HeaderText="Received&nbsp;Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReceivedQty" Text='<%# Eval("ReceivedQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Retur&nbsp;Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReturQty" Text='<%# Eval("ReturQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Available&nbsp;Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_AvailableQty" Text='<%# Eval("AvailableQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Stock&nbsp;Out&nbsp;Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_StockOutQty" Text='<%# Eval("StockOutQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Stock&nbsp;Out&nbsp;Retur&nbsp;Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_StockOutReturQty" Text='<%# Eval("StockOutReturQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Unit&nbsp;Price" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UnitPrice" Text='<%# Eval("UnitPrice", "{0:#,0}") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="SKU"><ItemTemplate><asp:Literal runat="server" ID="ltrl_SKU" Text='<%# Eval("SKU") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Receiver" ><ItemTemplate><div style="width:150px;"><asp:Literal runat="server" ID="ltrl_Receiver" Text='<%# Eval("Receiver") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Received&nbsp;Date" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReceivedDate" Text='<%# Eval("ReceivedDate", "{0: dd-MMM-yyyy HH:mm:ss}") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Invoice&nbsp;No" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_InvoiceNo" Text='<%# Eval("InvoiceNo") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Invoice&nbsp;Date" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_InvoiceDate" Text='<%# Convert.ToDateTime(Eval("InvoiceDate")) == DateTime.MinValue ? "" : Eval("InvoiceDate", "{0: dd-MMM-yyyy}") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>                                                
            <asp:TemplateField HeaderText="Invoice&nbsp;File&nbsp;Name" ><ItemTemplate><div style="min-width:400px;">              
              <asp:LinkButton runat="server" ID="lb_InvoiceFileName" Text='<%# Eval("InvoiceFileName") %>' CommandArgument='<%# Eval("StockReceivedId") %>' />
              <asp:Button runat="server" ID="btnDownloadInvoice" Hidden="true" OnClick="Download" EnableAjax="false" />
            </div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Bast&nbsp;File&nbsp;Name" ><ItemTemplate><div style="min-width:400px;">
              <asp:LinkButton runat="server" ID="lb_BastFileName" Text='<%# Eval("BastFileName") %>' CommandArgument='<%# Eval("StockReceivedId") %>' />
              <asp:Button runat="server" ID="btnDownloadBast" Hidden="true" OnClick="Download" EnableAjax="false" />
            </div></ItemTemplate></asp:TemplateField>
        </Columns>
        <HeaderStyle BackColor="#157fcc" ForeColor="White" />
        </asp:GridView>      
      </div></Content>
    </f:ContentPanel>    
  </Items>
  <Toolbars>
    <f:Toolbar runat="server" ID="tBottom" RegionPosition="Bottom" Position="Bottom"><Items>
      <f:ToolbarFill runat="server" ID="tfBottom1" />
      <f:ContentPanel runat="server" ID="cpPaging" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc2:ucPaging ID="ucPaging" runat="server" OnpageChanged="ucPaging_OpChanged" /></Content></f:ContentPanel>
      <f:ToolbarFill runat="server" ID="tfBottom2" />
    </Items></f:Toolbar>
  </Toolbars>
</f:Panel>
<f:Window ID="wSearch" Title="Filter" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="700px" Height="470px" OnClose="wSearch_Close"></f:Window>
<f:Window ID="wSort" Title="Sorting" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="562px" Height="470px" OnClose="wSort_Close"></f:Window>                  

</form>
  <script>
      function getNewWindowUrl() {
          return F.baseUrl + 'Pages/Inventory/StockReceivedInput.aspx?parenttabid=' + parent.getActiveTabId();
      }
      function getEditWindowUrl(id, StockOrderId) {
          return F.baseUrl + 'Pages/Inventory/StockReceivedInput.aspx?Id=' + id + '&StockOrderId=' + StockOrderId + '&parenttabid=' + parent.getActiveTabId();
      } 
      function getStockOrderWindowUrl(id) {
          return F.baseUrl + 'Pages/Inventory/StockOrderDetail.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
      }
      function getItemWindowUrl(id) {
          return F.baseUrl + 'Pages/Admin/ItemDetail.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
      }
      function getReturWindowUrl(id) {
          return F.baseUrl + 'Pages/Inventory/StockReceivedReturInput.aspx?StockReceivedItemId=' + id + '&parenttabid=' + parent.getActiveTabId();
      }
      function updatePage(param1) {
          __doPostBack('', 'UpdatePage$' + param1);
      }
  </script>
</body>
</html>
