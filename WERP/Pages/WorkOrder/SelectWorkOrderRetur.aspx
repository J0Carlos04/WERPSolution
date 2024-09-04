<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectWorkOrderRetur.aspx.cs" Inherits="Pages_WorkOrder_SelectWorkOrderRetur" %>
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
      <f:ToolbarFill runat="server" ID="tf1" />
      <f:ContentPanel runat="server" ID="cpPageSize" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc1:ucPageSize ID="ups" runat="server" OnSizeChanged="ups_Changed" /></Content></f:ContentPanel>
      <f:ToolbarFill runat="server" ID="tf2" />
      <f:ContentPanel runat="server" ID="cpRowCount" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><asp:Label ID="lblRowCount" runat="server" /></Content></f:ContentPanel>
      <f:Button runat="server" ID="btnRefresh" Text="Refresh" Icon="Reload" EnablePostBack="false" OnClientClick="javascript:self.location.reload();" />
    </Items></f:Toolbar>
  </Toolbars>
  <Items>        
    <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" Title="Bank Account" ShowBorder="true" ShowHeader="false" AutoScroll="true" EnableAjaxLoading="true" AjaxLoadingType="Default">      
      <Content>                              
        <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvData_RowDataBound">
          <Columns>            
            <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
              <ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_StockOutId" Text='<%# Eval("StockOutId")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center"><ItemTemplate>
              <asp:RadioButton runat="server" ID="rb" />
              <div style="display:none"><asp:Button runat="server" ID="btnSelect" OnClick="btnSelect_Click" /></div>
            </ItemTemplate></asp:TemplateField>            
            
            <asp:TemplateField HeaderText="Code" ><ItemTemplate><div style="width:100px;"><asp:Literal runat="server" ID="ltrl_Code" Text='<%# Eval("Code") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Subject" ><ItemTemplate><div style="width:200px;"><asp:Literal runat="server" ID="ltrl_Subject" Text='<%# Eval("Subject") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Work&nbsp;Description" ><ItemTemplate><div style="width:200px;"><asp:Literal runat="server" ID="ltrl_WorkDescription" Text='<%# Eval("WorkDescription") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Area" ><ItemTemplate><div style="width:200px;"><asp:Literal runat="server" ID="ltrl_Area" Text='<%# Eval("Area") %>' /></div></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Location" ><ItemTemplate><div style="width:150px;"><asp:Literal runat="server" ID="ltrl_Location" Text='<%# Eval("Location") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Category" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Category" Text='<%# Eval("Category") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Operator&nbsp;Type" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_OperatorType" Text='<%# Eval("OperatorType") %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Vendor" ><ItemTemplate><div style="width:200px;"><asp:Literal runat="server" ID="ltrl_Vendor" Text='<%# Eval("Vendor") %>' /></div></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="Conducted&nbsp;By" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ConductedBy" Text='<%# Eval("ConductedBy") %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="StartDate" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_StartDate" Text='<%# $"{Eval("StartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("StartDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate></asp:TemplateField>            

            <asp:TemplateField HeaderText="Status" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Status" Text='<%# Eval("Status") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Result" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Result" Text='<%# Eval("Result") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Remarks" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Remarks" Text='<%# Eval("Remarks") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Close&nbsp;Date" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_CloseDate" Text='<%# $"{Eval("CloseDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("CloseDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Achievement" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Achievement" Text='<%# Eval("Achievement") %>' /></ItemTemplate></asp:TemplateField>
            
            <asp:TemplateField HeaderText="Order&nbsp;Date" ><ItemTemplate><div style="width:75px;"><asp:Literal runat="server" ID="ltrl_OrderDate" Text='<%# $"{Eval("OrderDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("OrderDate", "{0: dd-MMM-yyyy}")}" %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Earliest&nbsp;Start&nbsp;Date" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_EarliestStartDate" Text='<%# $"{Eval("EarliestStartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("EarliestStartDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Latest&nbsp;Start&nbsp;Date" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_LatestStartDate" Text='<%# $"{Eval("LatestStartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("LatestStartDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate></asp:TemplateField>                                    
            <asp:TemplateField HeaderText="Schedule&nbsp;Start&nbsp;Date" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ScheduleStartDate" Text='<%# $"{Eval("ScheduleStartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("ScheduleStartDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Period" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Period" Text='<%# $"{Eval("No")}" == "-1" ? "" : $"{Eval("Period")} {Eval("PeriodType")}" %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Schedule&nbsp;End&nbsp;Date" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ScheduleEndDate" Text='<%# $"{Eval("ScheduleEndDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("ScheduleEndDate", "{0: dd-MMM-yyyy}")}" %>' /></ItemTemplate></asp:TemplateField>            
        </Columns>        
        </asp:GridView>      
      </Content>
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
    function getItemWindowUrl(id) {
        return F.baseUrl + 'Pages/Admin/ItemDetail.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
    }        
</script>
</body>
</html>
