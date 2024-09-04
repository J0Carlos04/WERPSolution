<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectStockOutItem.aspx.cs" Inherits="Pages_Inventory_SelectStockOutItem" %>
<%@ Register src="~/UserControls/Common/ucPageSize.ascx" tagname="ucPageSize" tagprefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WERP</title>    
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast"  />    
<f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region" EnableAjaxLoading="true" AjaxLoadingType="Default">
  <Items>        
    <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" Title="Bank Account" ShowBorder="true" ShowHeader="false" AutoScroll="true" EnableAjaxLoading="true" AjaxLoadingType="Default">      
      <Content>          
        <div style=" margin-top:-5px; margin-left:-5px; margin-right:-5px; background-color:#dfeaf2; border: 1px solid #c2c2c2; border-top-width:0px; border-bottom-color:#ccc; padding: 6px 0px 6px 8px">
        <table cellpadding="0" cellspacing="0" style="width:100%;">
          <tr>
            <td style="width:60%; text-align:left;">
              <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />&nbsp;
              <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />&nbsp;                            
            </td>
            <td style="width:20%; text-align:center;"><uc1:ucPageSize ID="ups" runat="server" OnSizeChanged="ups_Changed" /></td>
            <td style="width:20%; text-align:right;"><asp:Label ID="lblRowCount" runat="server" /></td>
          </tr>
        </table>                        
       </div>
            
        <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvData_RowDataBound">
          <Columns>            
            <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
              <ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center"><ItemTemplate>
              <asp:RadioButton runat="server" ID="rb" />
              <div style="display:none"><asp:Button runat="server" ID="btnSelect" OnClick="btnSelect_Click" /></div>
            </ItemTemplate></asp:TemplateField>            
            
            <asp:TemplateField HeaderText="Item&nbsp;Code" ><ItemTemplate><asp:LinkButton runat="server" ID="lb_ItemCode" Text='<%# Eval("ItemCode") %>' onclick='<%# GetItemUrl(Eval("ItemId")) %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Item&nbsp;Name" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Warehouse" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Warehouse" Text='<%# Eval("Warehouse") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Rack" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Rack" Text='<%# Eval("Rack") %>' /></ItemTemplate></asp:TemplateField>                        
            <asp:TemplateField HeaderText="Qty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Qty" Text='<%# Eval("Qty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="UnusedQty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UnusedQty" Text='<%# Eval("UnusedQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="UsedQty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UsedQty" Text='<%# Eval("UsedQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="ReturQty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReturQty" Text='<%# Eval("ReturQty") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="SKU" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_SKU" Text='<%# Eval("SKU") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="OutDate" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_OutDate" Text='<%# $"{Eval("OutDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("OutDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="StockOutCode" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_StockOutCode" Text='<%# Eval("StockOutCode") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="WorkOrder" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_WorkOrder" Text='<%# Eval("WorkOrder") %>' /></ItemTemplate></asp:TemplateField>            
        </Columns>        
        </asp:GridView>
      <div style=" margin-top:-5px; margin-left:-5px; margin-right:-5px; background-color:#dfeaf2; border: 1px solid #c2c2c2; border-top-width:0px; border-bottom-color:#ccc; padding: 6px 0px 6px 8px">
      <uc2:ucPaging ID="ucPaging" runat="server" OnpageChanged="ucPaging_OpChanged" />          
      </div>
      </Content>
    </f:ContentPanel>
    <f:HiddenField ID="HiddenField1" runat="server"></f:HiddenField>
  </Items>
</f:Panel>
<f:Window ID="wSearch" Title="Filter" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="700px" Height="470px" OnClose="wSearch_Close"></f:Window>
<f:Window ID="wSort" Title="Sorting" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="562px" Height="470px" OnClose="wSort_Close"></f:Window>                  

</form>
<script>        
    function getItemWindowUrl(id) {
        return F.baseUrl + 'Pages/Admin/ItemDetail.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
    }        
</script>
</body>
</html>
