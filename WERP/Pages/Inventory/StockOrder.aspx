<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockOrder.aspx.cs" Inherits="Pages_Inventory_StockOrder" %>
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
      <f:Button runat="server" ID="btnShowItem" Text="Show Items" Icon="Add" OnClick="btn_Click" />
      <f:Button runat="server" ID="btnHideItem" Text="Hide Items" Icon="Add" OnClick="btn_Click" />
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
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
              <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
              <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
            </asp:TemplateField>
            <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
              <ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action"><ItemTemplate>
              <asp:Literal runat="server" ID="ltrl_ApproverUserId" Text='<%# Eval("ApproverUserId") %>' Visible="false" />
              <div style="display:flex; justify-content:center; column-gap:4px;">
                <asp:LinkButton runat="server" ID="lbEdit" Text="Edit" CommandArgument='<%# Eval("Id")  %>' OnClick="lb_Click"  Font-Underline="false" />
                <asp:LinkButton runat="server" ID="lbApproval" Text="Approval" CommandArgument='<%# Eval("Id")  %>' OnClick="lb_Click"  Font-Underline="false" />
                <asp:LinkButton runat="server" ID="lbReception" Text="Reception" CommandArgument='<%# Eval("Id")  %>' OnClick="lb_Click"  Font-Underline="false" />
              </div>              
		    </ItemTemplate></asp:TemplateField>
            
            <asp:TemplateField HeaderText="Code" >
              <ItemTemplate><asp:Literal runat="server" ID="ltrl_Code" Text='<%# Eval("Code") %>' /></ItemTemplate>
              <AlternatingItemTemplate>
                <asp:GridView runat="server" ID="gvItem" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center">
                  <Columns>
                    <asp:TemplateField HeaderText="Item Code" ><ItemTemplate><asp:LinkButton runat="server" ID="lb_Code" Text='<%# Eval("Code") %>' onclick='<%# GetItemUrl(Eval("ItemId")) %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Name" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Qty" Text='<%# Eval("Qty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Approved Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ApprovedQty" Text='<%# Eval("ApprovedQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Pending Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_PendingQty" Text='<%# Eval("PendingQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Received Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReceivedQty" Text='<%# Eval("ReceivedQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Retur Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReturQty" Text='<%# Eval("ReturQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UnitPrice" Text='<%# Eval("UnitPrice", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Status" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Status" Text='<%# Eval("Status") %>' /></ItemTemplate></asp:TemplateField>                    
                  </Columns>
                  <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                </asp:GridView>
              </AlternatingItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Description" Text='<%# Eval("Description") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Requester" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Requester" Text='<%# Eval("Requester") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Procurement&nbsp;Type" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ProcurementType" Text='<%# Eval("ProcurementType") %>' /></ItemTemplate></asp:TemplateField>                        
            <asp:TemplateField HeaderText="Vendor" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Vendor" Text='<%# Eval("Vendor") %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="PONo" ><ItemTemplate><asp:LinkButton runat="server" ID="lb_PONo" Text='<%# Eval("PONo") %>' onclick='<%# GetPOUrl(Eval("POId")) %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="PODate" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_PODate" Text='<%# $"{Eval("PODate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("PODate", "{0: dd-MMM-yyyy}")}" %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Status" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Status" Text='<%# Eval("Status") %>' /></ItemTemplate></asp:TemplateField>                                    
            <asp:TemplateField HeaderText="Approver" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Approver" Text='<%# Eval("Approver") %>' /></ItemTemplate></asp:TemplateField>            
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
            return F.baseUrl + 'Pages/Inventory/StockOrderInput.aspx?parenttabid=' + parent.getActiveTabId();
        }
        function getEditWindowUrl(id) {
            return F.baseUrl + 'Pages/Inventory/StockOrderInput.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
        }
        function getApprovalWindowUrl(id) {
            return F.baseUrl + 'Pages/Inventory/StockOrderApproval.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
        }
        function getReceivedWindowUrl(id) {
          return F.baseUrl + 'Pages/Inventory/StockReceivedInput.aspx?StockOrderId=' + id + '&parenttabid=' + parent.getActiveTabId();
        }
        function getItemWindowUrl(id) {
          return F.baseUrl + 'Pages/Admin/ItemDetail.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
        }
        function getPOWindowUrl(id) {
          return F.baseUrl + 'Pages/Admin/PODetail.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
        }
        function updatePage(param1) {
            __doPostBack('', 'UpdatePage$' + param1);
        }
  </script>
</body>
</html>
