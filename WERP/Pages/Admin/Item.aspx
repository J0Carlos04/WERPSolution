<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Item.aspx.cs" Inherits="Pages_Item" %>
<%@ Register src="~/UserControls/Common/ucPageSize.ascx" tagname="ucPageSize" tagprefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WERP</title>    
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script> 
    <script>
        function UploadClick() {
            var objfile = document.getElementById("pContent_pData_fuUpload");
            objfile.click();
        }        
    </script>
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast"  />    
<f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region" EnableAjaxLoading="true" AjaxLoadingType="Default">
  <Toolbars>
    <f:Toolbar runat="server" ID="tTop" Position="Top"><Items>
      <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />
      <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />
      <f:Button runat="server" ID="btnAdd" Text="Add" Icon="Add" OnClick="btn_Click" />      
      <f:Button runat="server" ID="btnExportExcel" Text="Export Excel" Icon="Excel" OnClick="btn_Click" EnableAjax="false" DisableControlBeforePostBack="false"  />
      <f:Button runat="server" ID="btnUploadExcel" Text="Upload Excel" OnClientClick="UploadClick()" EnablePostBack="false" />
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
        <div style="display:none;">
          <asp:FileUpload runat="server" ID="fuUpload" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" />
          <f:Button runat="server" ID="btnUpload" OnClick="btn_Click" />
        </div>
        <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvData_RowDataBound" Width="100%" HeaderStyle-HorizontalAlign="Center">
          <Columns>            
            <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
              <ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action"><ItemTemplate>              
              <table class="tbl"><tr>
                <td runat="server" id="tdEdit" Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' ><a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id")) %>" >Edit</a></td>                
              </tr></table>
            </ItemTemplate></asp:TemplateField>
            
            <asp:TemplateField HeaderText="Code" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Code" Text='<%# Eval("Code") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Name" ><ItemTemplate><div style="min-width:150px;"><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Description" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Description" Text='<%# Eval("Description") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Category" ><ItemTemplate><div style="min-width:150px;"><asp:Literal runat="server" ID="ltrl_Category" Text='<%# Eval("Category") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Group" ><ItemTemplate><div style="min-width:200px;"><asp:Literal runat="server" ID="ltrl_ItemGroup" Text='<%# Eval("ItemGroup") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Brand" ><ItemTemplate><div style="min-width:100px;"><asp:Literal runat="server" ID="ltrl_Brand" Text='<%# Eval("Brand") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Model" ><ItemTemplate><div style="min-width:100px;"><asp:Literal runat="server" ID="ltrl_Model" Text='<%# Eval("Model") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Material" ><ItemTemplate><div style="min-width:100px;"><asp:Literal runat="server" ID="ltrl_Material" Text='<%# Eval("Material") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Specs" ><ItemTemplate><div style="min-width:150px;"><asp:Literal runat="server" ID="ltrl_Specs" Text='<%# Eval("Specs") %>' /></div></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="UOM" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UOM" Text='<%# Eval("UOM") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Size" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Size" Text='<%# Eval("Size") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Threshold" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Threshold" Text='<%# Eval("Threshold") %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Use&nbsp;SKU" ><ItemTemplate><asp:Literal runat="server" ID="ltrlUseSKU" Text='<%# $"{Eval("UseSKU")}" == "True" ? "Yes" : "No" %>' Visible='<%# $"{Eval("No")}" == "-1" ? false : true %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Active" ><ItemTemplate><asp:Literal runat="server" ID="ltrlActive" Text='<%# $"{Eval("No")}" == "-1" ? "" : $"{Eval("Active")}" %>' /></ItemTemplate></asp:TemplateField>
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
  <script>
        function getNewWindowUrl() {
            return F.baseUrl + 'Pages/Admin/ItemInput.aspx?parenttabid=' + parent.getActiveTabId();
        }
        function getEditWindowUrl(id) {
            return F.baseUrl + 'Pages/Admin/ItemInput.aspx?Id=' + id + '&parenttabid=' + parent.getActiveTabId();
        }
        function getErrorWindowUrl() {
          return F.baseUrl + 'Pages/ErrorList.aspx?parenttabid=' + parent.getActiveTabId();
        }
        function updatePage(param1) {
            __doPostBack('', 'UpdatePage$' + param1);
        }
  </script>
</body>
</html>
