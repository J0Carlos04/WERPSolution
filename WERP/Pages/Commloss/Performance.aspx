<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Performance.aspx.cs" Inherits="Pages_Commloss_Performance" %>
<%@ Register src="~/UserControls/Common/ucPageSize.ascx" tagname="ucPageSize" tagprefix="uc1" %>
<%@ Register Src="~/UserControls/Common/ucPaging.ascx" TagName="ucPaging" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>WERP</title>    
  <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
  <link href="../../Scripts/chart.min.js" rel="stylesheet" />
  <style>
    .chart {
      width:100% !important;
      height:100% !important;
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
                      <div style="font-family: 'Poppins'; font-size:12px;">REVENUE SAVED</div>
                      <div style="font-family: 'Poppins-Bold'; font-weight: bold;font-size:12px;">Rp <asp:Literal runat="server" ID="ltrlRevenueLoss" Text="999,999,999" /></div>
                    </div>
                    <div>
                      <div style="font-family: 'Poppins'; font-size:12px;">VOLUME SAVED</div>
                      <div style="font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlVolumeLoss" Text="999" /> m3</div>
                    </div>
                    <div>
                      <div style="font-family: 'Poppins'; font-size:12px;">FINISHED ISSUES</div>
                      <div style="text-align:right;font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlIssues" Text="999" /></div>
                    </div>
                  </div>
                  <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" CssClass="table table-striped table-bordered table-hover">
                    <Columns>                                    
                      <asp:TemplateField HeaderText="DMA"><ItemTemplate><asp:Literal runat="server" ID="ltrl_StationName" Text='<%# Eval("StationName") %>' /></ItemTemplate></asp:TemplateField>                  
                      <asp:TemplateField HeaderText="Revenue Saved" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_RevenueSaved" Text='<%# Eval("RevenueSaved", "{0:n0}") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="Volume Saved" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_VolumeSaved" Text='<%# Eval("VolumeSaved", "{0:n0}") %>' /></ItemTemplate></asp:TemplateField>
                      <asp:TemplateField HeaderText="Finished Issued" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_FinishedIssued" Text='<%# Eval("FinishedIssued", "{0:n0}") %>' /></ItemTemplate></asp:TemplateField>
                    </Columns>
                    <%--<HeaderStyle BackColor="#157fcc" ForeColor="White" />--%>
                  </asp:GridView>
                </div>
                <div style="width:30px;"></div>
                <div style="flex-grow:1;">
                  <asp:Chart ID="cLosses" runat="server" Height="300px" ImageType="Png" Palette="BrightPastel" BackColor="#D3DFF0" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" CssClass="chart" >
                    <titles>
                      <asp:Title Font="Poppins, 6pt" Text="Revenue Saved Chart" ForeColor="Black"></asp:Title>
                      <asp:Title Font="Poppins, 6pt" Text="in Million Rupiah" ForeColor="Black"></asp:Title>
                    </titles>     
                    <Series>
                      <asp:Series Name="Jan" ChartType="StackedColumn" IsValueShownAsLabel="true">                        
                        <Points>
                          <asp:DataPoint YValues="1000" ToolTip="1000" />
                          <asp:DataPoint YValues="2000" ToolTip="2000"  />
                        </Points>
                      </asp:Series>
                      <asp:Series Name="Feb" ChartType="StackedColumn">
                        <Points>
                          <asp:DataPoint YValues="1000" ToolTip="1000"  />
                          <asp:DataPoint YValues="3000" ToolTip="3000"  />
                          <asp:DataPoint YValues="4000" ToolTip="4000"  />
                        </Points>
                      </asp:Series>
                    </Series>
                    <BorderSkin SkinStyle="Emboss" PageColor="Transparent"></BorderSkin>
                    <Legends><asp:Legend LegendStyle="Table" IsTextAutoFit="true" Docking="Top" Name="Default" BackColor="Transparent" Font="Poppins, 6pt" Alignment="Center" TextWrapThreshold="100"></asp:Legend></Legends>
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
                  <%--<canvas id="myChart" width="600" height="400"></canvas>--%>
                </div>
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
  <script>
    // Static data example
    //var chartData = {
    //  labels: ["January", "February", "March", "April", "May", "June", "July"],
    //  datasets: [
    //    {
    //      label: "DMA 1",
    //      data: [30, 40, 50, 60, 70, 80, 90],
    //      backgroundColor: "rgba(75, 192, 192, 0.5)"
    //    },
    //    {
    //      label: "DMA 2",
    //      data: [20, 35, 45, 55, 65, 75, 85],
    //      backgroundColor: "rgba(153, 102, 255, 0.5)"
    //    }
    //  ]
    //};

    //window.onload = function () {
    //  var ctx = document.getElementById('myChart').getContext('2d');
    //  var myChart = new Chart(ctx, {
    //    type: 'bar',
    //    data: chartData,
    //    options: {
    //      scales: {
    //        x: {
    //          stacked: true,
    //        },
    //        y: {
    //          stacked: true,
    //          beginAtZero: true
    //        }
    //      }
    //    }
    //  });
    //};
</script>
</body>
</html>
