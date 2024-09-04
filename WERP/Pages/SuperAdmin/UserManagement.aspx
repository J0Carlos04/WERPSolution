<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="Pages_SuperAdmin_UserManagement" %>
<%@ Register src="~/UserControls/Common/ucPageSize.ascx" tagname="ucPageSize" tagprefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px">
              <ItemTemplate>                
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center"><ItemTemplate><a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id")) %>" >Edit</a></ItemTemplate></asp:TemplateField>
                        
            <asp:TemplateField HeaderText="Users"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Users" Text='<%# Eval("AllUsers").ToBool() ? "All Users" : Eval("Users") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Operators"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Operators" Text='<%# Eval("AllOperators").ToBool() ? "All Operators" : Eval("Operators") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Roles"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Roles" Text='<%# Eval("Roles") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Modules"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Modules" Text='<%# Eval("AllModules").ToBool() ? "All Modules" : Eval("Modules") %>' /></ItemTemplate></asp:TemplateField>            

            <asp:TemplateField HeaderText="Create"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Create" Text='<%# Eval("Create") %>' Visible='<%# $"{Eval("Users")}" == CNT.DataNotAvailable ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="View&nbsp;Page"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Read" Text='<%# Eval("Read") %>' Visible='<%# $"{Eval("Users")}" == CNT.DataNotAvailable ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Update"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Update" Text='<%# Eval("Update") %>' Visible='<%# $"{Eval("Users")}" == CNT.DataNotAvailable ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Delete"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Delete" Text='<%# Eval("Delete") %>' Visible='<%# $"{Eval("Users")}" == CNT.DataNotAvailable ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="View&nbsp;All&nbsp;Data"><ItemTemplate><asp:Literal runat="server" ID="ltrl_ViewAllData" Text='<%# Eval("ViewAllData") %>' Visible='<%# $"{Eval("Users")}" == CNT.DataNotAvailable ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Deviation"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Deviation" Text='<%# Eval("Deviation") %>' Visible='<%# $"{Eval("Users")}" == CNT.DataNotAvailable ? false : true %>' /></ItemTemplate></asp:TemplateField>
            
        </Columns>        
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
        return F.baseUrl + 'Pages/SuperAdmin/UserManagementInput.aspx?parenttabid=' + parent.getActiveTabId();
    }   
    function getEditWindowUrl(id) {
        return F.baseUrl + 'Pages/SuperAdmin/UserManagementInput.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
    }    
    function updatePage(param1) {
        __doPostBack('', 'UpdatePage$' + param1);
    }
</script>
</body>
</html>
