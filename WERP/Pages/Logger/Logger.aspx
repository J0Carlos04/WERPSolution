<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logger.aspx.cs" Inherits="Pages_Logger_Logger" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WERP</title>    
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script> 
    <script lang ="javascript" type="text/javascript">
        
    </script>
</head>
<body>
<form id="form1" runat="server">
  <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="ddlFunction,ddlTotalizer,gvData" />     
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlbTop"><Items>        
        <f:ContentPanel runat="server" ID="cpFunction" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><asp:DropDownList runat="server" id="ddlFunction" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" /></Content></f:ContentPanel>        
        <f:ToolbarSeparator runat="server" ID="ts1" />        
        <f:ContentPanel runat="server" ID="cpTotalizer" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><asp:DropDownList runat="server" id="ddlTotalizer" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="200" /></Content></f:ContentPanel>
        <f:ToolbarSeparator runat="server" ID="ts2" />
        <f:ToolbarText runat="server" ID="ttStart" Text="Waktu Awal" />
        <f:ContentPanel runat="server" ID="cpStart" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><input runat="server" id="tbStart" type="datetime-local" class="form-control form-control-sm" onchange="SetTotalHour()" /></Content></f:ContentPanel>
        <f:ToolbarText runat="server" ID="tsDash" Text=" - " />
        <f:ToolbarText runat="server" ID="ttEnd" Text="Waktu Akhir" />
        <f:ContentPanel runat="server" ID="cpEnd" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><input runat="server" id="tbEnd" type="datetime-local" class="form-control form-control-sm" onchange="SetTotalHour()" /></Content></f:ContentPanel>        
        <f:ToolbarText runat="server" ID="ttTotal" Text="100 Hari 100 Jam" />
        <f:ToolbarSeparator runat="server" ID="ts3" />
        <f:Button runat="server" ID="btnView" Text="Tampilkan" Icon="ApplicationViewDetail" Size="Medium" OnClick="btn_Click" />
        <f:Button runat="server" ID="btnExportExcel" Text="Excel" Icon="Excel" Size="Medium" OnClick="btn_Click" EnableAjax="false" DisableControlBeforePostBack="false" />
      </Items></f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content>
          <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvData_RowDataBound" Width="100%" HeaderStyle-HorizontalAlign="Center">
            <Columns>
              <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_No" /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Fungsi"><ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Level" Text='<%# Eval("Level") %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrl_Function" Text='<%# Eval("Function") %>' />
              </ItemTemplate></asp:TemplateField>              
              <asp:TemplateField HeaderText="Lokasi"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Totalizer" Text='<%# Eval("Totalizer") %>' /></ItemTemplate></asp:TemplateField>              
              <asp:TemplateField HeaderText="Meter Awal" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_StartMeter" Text='<%# Eval("StartMeter").ToDouble().ToString("n2").Replace(".00", "")  %>' Visible='<%# $"{Eval("Totalizer")}" == "" ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Meter Akhir" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_EndMeter" Text='<%# Eval("EndMeter").ToDouble().ToString("n2").Replace(".00", "") %>' Visible='<%# $"{Eval("Totalizer")}" == "" ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Total Meter Awal Anggota" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_TotalStartMeter" Text='<%# Eval("TotalStartMeter").ToDouble().ToString("n2").Replace(".00", "") %>' Visible='<%# $"{Eval("Totalizer")}" == "" ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Total Meter Akhir Anggota" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_TotalEndMeter" Text='<%# Eval("TotalEndMeter").ToDouble().ToString("n2").Replace(".00", "") %>' Visible='<%# $"{Eval("Totalizer")}" == "" ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Kubikasi" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Cubication" Text='<% #Eval("Cubication").ToDouble().ToString("n2").Replace(".00", "") %>' Visible='<%# $"{Eval("Totalizer")}" == "" ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Total Kubikasi Anggota" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_TotalCubicationAnggota" Text='<% #Eval("TotalCubicationAnggota").ToDouble().ToString("n2").Replace(".00", "") %>' Visible='<%# $"{Eval("Totalizer")}" == "" ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Unit"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Unit" Text='<%# Eval("Unit") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>
          </asp:GridView>
        </Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
</form>
</body>
</html>
