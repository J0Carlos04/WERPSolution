<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectSO.aspx.cs" Inherits="Pages_Inventory_SelectSO" %>
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
  <Toolbars>
    <f:Toolbar runat="server" ID="tTop" Position="Top"><Items>
      <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />
      <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />      
      <f:Button runat="server" ID="btnShowItem" Text="Show Items" Icon="Add" OnClick="btn_Click" />
      <f:Button runat="server" ID="btnHideItem" Text="Hide Items" Icon="Add" OnClick="btn_Click" />      
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

            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
              <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
              <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Code" >
              <ItemTemplate><asp:Literal runat="server" ID="ltrl_Code" Text='<%# Eval("Code") %>' /></ItemTemplate>
              <AlternatingItemTemplate>
                <asp:GridView runat="server" ID="gvItem" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center">
                  <Columns>
                    <asp:TemplateField HeaderText="Item Code" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Code" Text='<%# Eval("Code") %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Name" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Qty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Qty" Text='<%# Eval("Qty") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Approved Qty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ApprovedQty" Text='<%# Eval("ApprovedQty") %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Pending Qty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_PendingQty" Text='<%# Eval("PendingQty") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Received Qty" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReceivedQty" Text='<%# Eval("ReceivedQty") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Unit Price" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UnitPrice" Text='<%# Eval("UnitPrice") %>' /></ItemTemplate></asp:TemplateField>                    
                  </Columns>
                  <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                </asp:GridView>
              </AlternatingItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Description" Text='<%# Eval("Description") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Requester" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Requester" Text='<%# Eval("Requester") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Procurement&nbsp;Type" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ProcurementType" Text='<%# Eval("ProcurementType") %>' /></ItemTemplate></asp:TemplateField>                        
            <asp:TemplateField HeaderText="Vendor" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Vendor" Text='<%# Eval("Vendor") %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="PONo" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_PONo" Text='<%# Eval("PONo") %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="PODate" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_PODate" Text='<%# Eval("PODate", "{0: dd-MMM-yyyy}") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Status" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Status" Text='<%# Eval("Status") %>' /></ItemTemplate></asp:TemplateField>                                    
            <asp:TemplateField HeaderText="Approver" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Approver" Text='<%# Eval("Approver") %>' /></ItemTemplate></asp:TemplateField>                       
        </Columns>        
        </asp:GridView>      
      </div></Content>
    </f:ContentPanel>
    <f:HiddenField ID="HiddenField1" runat="server"></f:HiddenField>
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
</body>
</html>
