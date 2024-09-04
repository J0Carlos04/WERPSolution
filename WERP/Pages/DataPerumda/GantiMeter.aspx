<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GantiMeter.aspx.cs" Inherits="Pages_DataPerumda_GantiMeter" %>
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
<f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" EnableAjaxLoading="true" AjaxLoadingType="Default">
  <Toolbars>
    <f:Toolbar runat="server" ID="tTop" Position="Top"><Items>
      <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />
      <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />      
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
                <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
              </ItemTemplate>
            </asp:TemplateField>            
            
            <asp:TemplateField HeaderText="no_mohonan_ganmeter" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_no_mohonan_ganmeter" Text='<%# Eval("no_mohonan_ganmeter") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="nolangganan" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_nolangganan" Text='<%# Eval("nolangganan") %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="tgl_pasang" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_tgl_pasang" Text='<%# Eval("tgl_pasang") %>' /></ItemTemplate></asp:TemplateField>            
            <asp:TemplateField HeaderText="nometer_lama" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_nometer_lama" Text='<%# Eval("nometer_lama") %>' /></ItemTemplate></asp:TemplateField>                        
            <asp:TemplateField HeaderText="kd_merek_lama" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_kd_merek_lama" Text='<%# Eval("kd_merek_lama") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="ukr_lama" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ukr_lama" Text='<%# Eval("ukr_lama") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="ukmeter_lama" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ukmeter_lama" Text='<%# Eval("ukmeter_lama") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="nometer" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_nometer" Text='<%# Eval("nometer") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="kd_merekmeter" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_kd_merekmeter" Text='<%# Eval("kd_merekmeter") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="kd_ukmeter" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_kd_ukmeter" Text='<%# Eval("kd_ukmeter") %>' /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="ukr" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ukr" Text='<%# Eval("ukr") %>' /></ItemTemplate></asp:TemplateField>            
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
</body>
</html>
