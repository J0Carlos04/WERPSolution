<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Activity.aspx.cs" Inherits="Pages_Commloss_Activity" %>
<%@ Register src="~/UserControls/Common/ucPageSize.ascx" tagname="ucPageSize" tagprefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>WERP</title>    
  <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
  <style>
    @font-face {
      font-family: 'Poppins';
      src: url('../../res/fonts/poppins/Poppins-Regular.ttf') format('truetype');
    }

    @font-face {
      font-family: 'Poppins-Bold';
      src: url('../../res/fonts/poppins/Poppins-Bold.ttf') format('truetype'), url('../../res/fonts/poppins/Poppins-Bold.ttf') format('truetype');
    }
    .chart {
      width:100% !important;
      height:90% !important;
    }   
  </style>
</head>
<body>
  <form id="form1" runat="server">
    <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast" />
    <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" EnableAjaxLoading="true" AjaxLoadingType="Default">
      <Toolbars>
        <f:Toolbar runat="server" ID="tTop" Position="Top"><Items>
          <f:Button runat="server" ID="btnSearch" Text="Search" Icon="Magnifier" EnablePostBack="false" />
          <f:Button runat="server" ID="btnSorting" Text="Sorting" Icon="Sorting" EnablePostBack="false" />          
          <f:Button runat="server" ID="btnExportExcel" Text="Export Excel" Icon="Excel" OnClick="btn_Click" EnableAjax="false" DisableControlBeforePostBack="false" />
          <f:ToolbarFill runat="server" ID="tf1" />
          <f:ContentPanel runat="server" ID="cpPageSize" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc1:ucpagesize id="ups" runat="server" onsizechanged="ups_Changed" /></Content></f:ContentPanel>
          <f:ToolbarFill runat="server" ID="tf2" />
          <f:ContentPanel runat="server" ID="cpRowCount" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content>
            <asp:Label ID="lblRowCount" runat="server" /></Content></f:ContentPanel>
          <f:Button runat="server" ID="btnRefresh" Text="Refresh" Icon="Reload" EnablePostBack="false" OnClientClick="javascript:self.location.reload();" />
        </Items></f:Toolbar>
      </Toolbars>
      <Items>
        <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" Title="Bank Account" ShowBorder="true" ShowHeader="false" AutoScroll="true" EnableAjaxLoading="true" AjaxLoadingType="Default">
          <Content>
            <div class="wrapper-suppress">
              <div style="display:flex">
                <div style="flex-grow:1;">
                  <div style="display:flex; background-image: linear-gradient(to right, #d7fddc, #9cb8f9); justify-content: space-between; border-radius: 15px; padding:10px; margin-bottom:20px; ">
                    <div>
                      <div style="font-family: 'Poppins'; font-size:12px;">REVENUE LOSS</div>
                      <div style="font-family: 'Poppins-Bold'; font-weight: bold;font-size:12px;">Rp <asp:Literal runat="server" ID="ltrlRevenueLoss" Text="999,999,999" /></div>
                    </div>
                    <div>
                      <div>VOLUME LOSS</div>
                      <div style="font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlVolumeLoss" Text="999" /> m3</div>
                    </div>
                    <div>
                      <div>ISSUES</div>
                      <div style="text-align:right;font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlIssues" Text="999" /></div>
                    </div>
                  </div>
                  <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" CssClass="table table-striped table-bordered table-hover" style="font-family: 'Poppins'; font-size:12px;">
                    <Columns>                                    
                      <asp:TemplateField HeaderText="DMA"><ItemTemplate><asp:Literal runat="server" ID="ltrl_StationName" Text='<%# Eval("StationName") %>' /></ItemTemplate></asp:TemplateField>                  
                      <asp:TemplateField HeaderText="Cust No"><ItemTemplate><asp:Literal runat="server" ID="ltrl_CustNo" Text='<%# Eval("CustNo") %>' /></ItemTemplate></asp:TemplateField>                  
                      <asp:TemplateField HeaderText="Revenue Loss" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_RevenueLoss" Text='<%# Eval("RevenueLoss", "{0:n0}") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="Volume Loss" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_VolumeLoss" Text='<%# Eval("VolumeLoss", "{0:n0}") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="Issue" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Issue" Text='<%# Eval("Issue") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Action" Text='<%# Eval("Action") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Status" Text='<%# Eval("Status") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="PreviousWorkOrder" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_PreviousWorkOrder" Text='<%# Eval("PreviousWorkOrder") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="IssueStartDate" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_IssuedStartDate" Text='<%# Eval("IssuedStartDate") %>' /></ItemTemplate></asp:TemplateField>
                    </Columns>
                    <%--<HeaderStyle BackColor="#157fcc" ForeColor="White" />--%>
                  </asp:GridView>
                </div>
                <div style="width:30px;"></div>
                <div>
                  <div style="display: flex; justify-content: flex-end; width:100%;"><asp:DropDownList runat="server" ID="ddlMonthYear" CssClass="form-select form-select-sm" BackColor="#40669f" ForeColor="White" Width="110px" Font-Names="Poppins"/></div>
                  <asp:Chart ID="cLosses" runat="server" ImageType="Png" Palette="BrightPastel" BackColor="#D3DFF0" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" CssClass="chart" >
                    <titles><asp:Title ShadowColor="32, 0, 0, 0" Font="Poppins, 10pt, style=Bold" ShadowOffset="1" Text="Issue Status Chart" ForeColor="Black"></asp:Title></titles>
                    <BorderSkin SkinStyle="Emboss" PageColor="Transparent"></BorderSkin>
                    <%--<Legends><asp:Legend LegendStyle="Table" IsTextAutoFit="true" Docking="Bottom" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold" Alignment="Near" TextWrapThreshold="100"></asp:Legend></Legends>--%>
                    <ChartAreas>
                      <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="VerticalCenter">
                        <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False" WallWidth="0" IsClustered="False" />
                        <AxisY LineColor="64, 64, 64, 64">
                          <LabelStyle Font="Poppins, 6pt, style=Bold" />
                          <MajorGrid LineColor="64, 64, 64, 64" />
                        </AxisY>
                        <AxisX LineColor="64, 64, 64, 64">
                          <LabelStyle Font="Poppins, 6pt, style=Bold" />
                          <MajorGrid LineColor="Transparent" />
                        </AxisX>
                      </asp:ChartArea>
                    </ChartAreas>
                  </asp:Chart>
                </div>
              </div>              
            </div>
          </Content>
        </f:ContentPanel>
      </Items>
      <Toolbars>
        <f:Toolbar runat="server" ID="tBottom" RegionPosition="Bottom" Position="Bottom"><Items>
          <f:ToolbarFill runat="server" ID="tfBottom1" />
          <f:ContentPanel runat="server" ID="cpPaging" ShowHeader="false" ShowBorder="false" BodyStyle="background-color:transparent;"><Content><uc2:ucpaging id="ucPaging" runat="server" onpagechanged="ucPaging_OpChanged" /></Content></f:ContentPanel>
          <f:ToolbarFill runat="server" ID="tfBottom2" />
        </Items></f:Toolbar>
      </Toolbars>
    </f:Panel>
    <f:Window ID="wSearch" Title="Filter" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="700px" Height="470px" OnClose="wSearch_Close"></f:Window>
    <f:Window ID="wSort" Title="Sorting" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="562px" Height="470px" OnClose="wSort_Close"></f:Window>
  </form>
</body>
</html>
