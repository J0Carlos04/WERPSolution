<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestType.aspx.cs" Inherits="Pages_Admin_Helpdesk_RequestType" %>
<%@ Register src="~/UserControls/Common/ucPageSize.ascx" tagname="ucPageSize" tagprefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WERP</title>    
    <link href="../../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../../res/js/JScript.js" ></script>     
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
      <f:Button runat="server" ID="btnAddSave" Text="Save" Icon="DatabaseSave" OnClick="btn_Click" Hidden="true"  />           
      <f:Button runat="server" ID="btnAddCancel" Text="Cancel" Icon="Cancel" OnClick="btn_Click" Hidden="true" />
      <f:Button runat="server" ID="btnEdit" Text="Edit" Icon="ApplicationEdit" OnClick="btn_Click"  />
      <f:Button runat="server" ID="btnEditSave" Text="Save" Icon="DatabaseSave" OnClick="btn_Click" Hidden="true"  />           
      <f:Button runat="server" ID="btnEditCancel" Text="Cancel" Icon="Cancel" OnClick="btn_Click" Hidden="true" />
      <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="Delete" OnClick="btn_Click" ConfirmText="Are you sure you want to delete this Request Type?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
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
                <asp:Literal runat="server" ID="ltrl_Mode" Text='<%# Eval("Mode")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>
                        
            <asp:TemplateField HeaderText="Name" >              
              <ItemTemplate>		    
                <asp:Literal runat="server" ID="ltrlName" Text='<%# Eval("Name") %>' Visible='<%# Eval("Mode").IsEmpty() %>' />                        
                <asp:TextBox runat="server" ID="tb_Name" Text='<%# $"{Eval("Mode")}" == "add" ? "" : $"{Eval("Name")}" %>' Visible='<%# !Eval("Mode").IsEmpty() %>' CssClass="form-control form-control-sm" />
              </ItemTemplate>
            </asp:TemplateField>   
            <asp:TemplateField HeaderText="Response&nbsp;Time&nbsp;(Hours)" >              
              <ItemTemplate>		    
                <asp:Literal runat="server" ID="ltrlResponseTime" Text='<%# Eval("ResponseTime") %>' Visible='<%# string.Format("{0}", Eval("Mode")) == "" && $"{Eval("No")}" != "-1" ? true : false %>' />                        
                <asp:TextBox runat="server" ID="tb_ResponseTime" Text='<%# $"{Eval("Mode")}" == "add" ? "" : $"{Eval("ResponseTime")}" %>' TextMode="Number" Visible='<%# !Eval("Mode").IsEmpty() %>' CssClass="form-control form-control-sm" />
              </ItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Active" >              
              <ItemTemplate>		    
                <asp:Literal runat="server" ID="ltrlActive" Text='<%# $"{Eval("No")}" == "-1" ? "" : $"{Eval("Active")}" %>' Visible='<%# Eval("Mode").IsEmpty() %>' />                        
                <asp:DropDownList runat="server" ID="ddl_Active" Visible='<%# Eval("Mode").ToText() == "edit" %>' CssClass="form-select form-select-sm" >
                  <asp:ListItem>True</asp:ListItem>
                  <asp:ListItem>False</asp:ListItem>
                </asp:DropDownList>
              </ItemTemplate>
            </asp:TemplateField>
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
</body>
</html>
