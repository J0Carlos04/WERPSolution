<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectCust.aspx.cs" Inherits="Pages_WorkOrder_SelectCust" %>
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
  <Toolbars><f:Toolbar runat="server" ID="tTop" Position="Top"><Items>
    <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="btn_Click" />
    <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />
    <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />                
    <f:ToolbarFill runat="server" ID="tf1" />
    <f:ContentPanel runat="server" ID="cpPageSize" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc1:ucPageSize ID="ups" runat="server" OnSizeChanged="ups_Changed" /></Content></f:ContentPanel>
    <f:ToolbarFill runat="server" ID="tf2" />
    <f:ContentPanel runat="server" ID="cpRowCount" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><asp:Label ID="lblRowCount" runat="server" /></Content></f:ContentPanel>
  </Items></f:Toolbar></Toolbars>
  <Items>        
    <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" Title="Customer" ShowBorder="true" ShowHeader="false" AutoScroll="true" EnableAjaxLoading="true" AjaxLoadingType="Default" >      
      <Content>                             
        <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvData_RowDataBound">
          <Columns>            
            <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
              <ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_LocationId" Text='<%# Eval("LocationId")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_Latitude" Text='<%# Eval("Latitude")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_Longitude" Text='<%# Eval("Longitude")  %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
              <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
              <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
              <HeaderTemplate></HeaderTemplate>
              <ItemTemplate><asp:RadioButton ID="rbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="rbSelect_CheckedChanged" /></ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Cust&nbsp;No" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_CustNo" Text='<%# Eval("CustNo") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Name" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Address" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Address" Text='<%# Eval("Address") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="RT" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_RT" Text='<%# Eval("RT") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="RW" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_RW" Text='<%# Eval("RW") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Kelurahan" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Kelurahan" Text='<%# Eval("Kelurahan") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Kecamatan" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Kecamatan" Text='<%# Eval("Kecamatan") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Phone" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Phone" Text='<%# Eval("Phone") %>' /></ItemTemplate></asp:TemplateField>                        
            <asp:TemplateField HeaderText="Email" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Email" Text='<%# Eval("Email") %>' /></ItemTemplate></asp:TemplateField>                           
        </Columns>
        <HeaderStyle BackColor="#157fcc" ForeColor="White" />
        </asp:GridView>              
      </Content>     
    </f:ContentPanel>        
  </Items>
   <Toolbars><f:Toolbar runat="server" ID="tBottom" RegionPosition="Bottom" Position="Bottom"><Items>
    <f:ToolbarFill runat="server" ID="tfBottom1" />
    <f:ContentPanel runat="server" ID="cpPaging" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc2:ucPaging ID="ucPaging" runat="server" OnpageChanged="ucPaging_OpChanged" /></Content></f:ContentPanel>
    <f:ToolbarFill runat="server" ID="tfBottom2" />
  </Items></f:Toolbar></Toolbars>
</f:Panel>

<f:Window ID="wSearch" Title="Filter" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="700px" Height="470px" OnClose="wSearch_Close"></f:Window>
<f:Window ID="wSort" Title="Sorting" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="562px" Height="470px" OnClose="wSort_Close"></f:Window>                  

</form>
</body>
</html>
