<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StationMapping.aspx.cs" Inherits="Pages_Logger_StationMapping
  " %>

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
    <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvFilter,gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast" />

    <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region" EnableAjaxLoading="true" AjaxLoadingType="Default">
      <Toolbars>
        <f:Toolbar runat="server" ID="tTop" Position="Top"><Items>
          <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />
          <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />
          <f:Button runat="server" ID="btnAdd" Text="Add" Icon="Add" OnClick="btn_Click" />
          <f:Button runat="server" ID="btnAddSave" Text="Save" Icon="DatabaseSave" OnClick="btn_Click" Hidden="true" />
          <f:Button runat="server" ID="btnAddCancel" Text="Cancel" Icon="Cancel" OnClick="btn_Click" Hidden="true" />
          <f:Button runat="server" ID="btnEdit" Text="Edit" Icon="ApplicationEdit" OnClick="btn_Click" />
          <f:Button runat="server" ID="btnEditSave" Text="Save" Icon="DatabaseSave" OnClick="btn_Click" Hidden="true" />
          <f:Button runat="server" ID="btnEditCancel" Text="Cancel" Icon="Cancel" OnClick="btn_Click" Hidden="true" />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="Delete" OnClick="btn_Click" ConfirmText="Are you sure you want to delete this Station Mapping?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning" />
          <f:Button runat="server" ID="btnExportExcel" Text="Export Excel" Icon="Excel" OnClick="btn_Click" EnableAjax="false" DisableControlBeforePostBack="false" />
          <f:ToolbarFill runat="server" ID="tf1" />
          <f:ContentPanel runat="server" ID="cpPageSize" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc1:ucPageSize ID="ups" runat="server" OnSizeChanged="ups_Changed" /></Content></f:ContentPanel>
          <f:ToolbarFill runat="server" ID="tf2" />
          <f:ContentPanel runat="server" ID="cpRowCount" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content>
            <asp:Label ID="lblRowCount" runat="server" /></Content></f:ContentPanel>
          <f:Button runat="server" ID="btnRefresh" Text="Refresh" Icon="Reload" EnablePostBack="false" OnClientClick="javascript:self.location.reload();" />
        </Items></f:Toolbar>
      </Toolbars>
      <Items>
        <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" Title="Bank Account" ShowBorder="true" ShowHeader="false" AutoScroll="true" EnableAjaxLoading="true" AjaxLoadingType="Default">
          <Content>
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvData_RowDataBound" Width="100%" HeaderStyle-HorizontalAlign="Center">
              <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                  <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                  <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
                  </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Parent Station">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_ParentStation" Text='<%# Eval("ParentStation") %>' Visible='<%# $"{Eval("Mode")}" == "" ? true : false %>' />
                    <asp:DropDownList runat="server" ID="ddl_ParentStationId" Visible='<%# $"{Eval("Mode")}" == "" ? false : true %>' CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" />
                  </ItemTemplate>
                </asp:TemplateField>
               <asp:TemplateField HeaderText="Child Station">
                <ItemTemplate>
                  <asp:Literal runat="server" ID="ltrl_ChildStation" Text='<%# Eval("ChildStation") %>' Visible='<%# $"{Eval("Mode")}" == "" ? true : false %>' />
                  <asp:DropDownList runat="server" ID="ddl_ChildStationId" Visible='<%# $"{Eval("Mode")}" == "" ? false : true %>' CssClass="form-select form-select-sm" />
                </ItemTemplate>
              </asp:TemplateField>
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
</body>
</html>
