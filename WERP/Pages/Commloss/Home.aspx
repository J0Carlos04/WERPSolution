<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Commloss_Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>WERP</title>
  <%--<link href="../../res/css/ComlossHome.css" rel="stylesheet" />--%>
  <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
  <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script> 
  <style>
    @font-face {
      font-family: 'Poppins';
      src: url('../../res/fonts/poppins/Poppins-Regular.ttf') format('truetype');
    }

    @font-face {
      font-family: 'Poppins-Bold';
      src: url('../../res/fonts/poppins/Poppins-Bold.ttf') format('truetype'), url('../../res/fonts/poppins/Poppins-Bold.ttf') format('truetype');
    }

    .main {
      width: 95%;
      margin: 20px;
    }

    .header {
      display: flex;
      flex-wrap: wrap;
      margin: 10px;
      justify-content: space-between;
      margin-top: 10px;
    }

      .header > div {
        width: 270px;
        height: 65px;
        background-color: #40669f;
        color: white;
        border-radius: 15px;
        /*margin: 10px;*/
        display: flex;
        align-items: center;
        padding-left: 20px; /* Tambahkan padding kiri */
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        cursor: pointer; /* Change cursor to hand */                    
      }
      .header > div:hover {
        /*background-color: #e0e0e0;*/ /* Change background color on hover */
      }

      .header .image-container {
        display: flex;
        align-items: center;
        margin-right: 10px;
      }

      .header img {
        width: 35px;
        height: 35px;
        margin-right: 10px;
      }

      .header .text-container {
        font-size: 12px;
        font-family: 'Poppins';
        font-weight: normal;
      }

    .body {
      display: flex;
      justify-content: space-between;
      /*margin-top: 20px;*/
      height: 400px; /* contoh tinggi untuk body */
    }

      .body > div {
        flex: 1;
        /*margin: 0 10px;*/
        /*background-color: #e2e2e2;*/
        /* padding: 20px;*/
        text-align: center;
      }

    .body1-container {
      display: flex;
      flex-direction: column;
      height: 100%;
    }

    .row {
      display: flex;
      justify-content: space-between;
      margin: 10px;
    }

      .row > div {
        flex: 1;
        margin: 0 5px;
        background-color: #ffffff; /* Warna latar belakang putih */
        color: #000000; /* Warna teks hitam */
        padding: 10px;
        text-align: left;
        font-size: 12px;
        border-radius: 15px; /* Sudut bulat */
      }

      .row .full-width {
        flex: 2;
      }

    .grow {
      flex-grow: 1;
    }

    .custom-item {
      display: flex;
      align-items: center;
    }

      .custom-item .text-container {
        flex: 1;
      }

      .custom-item .image-container {
        margin-left: auto;
      }

      .custom-item img {
        height: 20px;
      }
  </style>
  <style>
    .smallboxBody {      
      background-color: #ffffff; /* Warna latar belakang putih */
      color: #000000; /* Warna teks hitam */
      padding: 10px;
      text-align: left;
      font-size: 12px;
      border-radius: 15px; /* Sudut bulat */      
    }

    .box {
      background-color: #40669f; /* Background color */
      color: white; /* Text color */
      margin-bottom: 10px; /* Space between boxes */
      padding: 10px; /* Padding inside the boxes */
      border-radius: 15px;
      display:flex;
      justify-content: space-between;
    }   
    .chart {
  width:103% !important;
}   
  </style>
  <script>
    function handleClick() {
      // Handle the click event
      alert("Div clicked!");
      // You can add more actions here, such as navigating to another page
    }
  </script>
</head>
<body style="background-color: #b4d5ec; font-family: 'Poppins';">
  <form id="form1" runat="server">
    <f:PageManager ID="pm" runat="server" Language="EN" />
    <f:Button runat="server" ID="btnRevenueSaved" Hidden="true" OnClick="Fbtn_Click" />
    <f:Button runat="server" ID="btnActivity" Hidden="true" OnClick="Fbtn_Click" />
    <f:Button runat="server" ID="btnActivityPreviousMonth" Hidden="true" OnClick="Fbtn_Click" />
    <div class="main">
      <div style="display:flex;background-color: #b4d5ec; font-family: 'Poppins'; margin-top:-40px;">
        <asp:Label runat="server" ID="lblTitleMonth" Text="Month" Visible="false" />
        <asp:DropDownList runat="server" ID="ddlMonth" Style="width: 130px" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Visible="false">
          <asp:ListItem Text="January" Value="1" />
          <asp:ListItem Text="February" Value="2" />
          <asp:ListItem Text="March" Value="3" />
          <asp:ListItem Text="April" Value="4" />
          <asp:ListItem Text="May" Value="5" />
          <asp:ListItem Text="June" Value="6" />
          <asp:ListItem Text="July" Value="7" />
          <asp:ListItem Text="August" Value="8" />
          <asp:ListItem Text="September" Value="9" />
          <asp:ListItem Text="October" Value="10" />
          <asp:ListItem Text="November" Value="11" />
          <asp:ListItem Text="December" Value="12" />
        </asp:DropDownList>
        <asp:Label runat="server" ID="Label2" Text="Year" Visible="false" />
        <asp:TextBox runat="server" ID="tbYear" CssClass="form-control form-control-sm" Visible="false" />
        <asp:TextBox runat="server" ID="ltrlMonthYear" CssClass="form-control form-control-sm" Style="flex-grow:1; text-align:right;background-color: #b4d5ec; font-family: 'Poppins';" />
      </div>
      <div class="header">
        <div onclick="document.getElementById('<%= btnRevenueSaved.ClientID %>').click();">
          <img src="../../res/images/DollarKuning.png" alt="Image">
          <div class="text-container">
            <div>REVENUE SAVED</div>
            <div style="font-family: 'Poppins-Bold'; font-weight: bold;">Rp&nbsp;<asp:Literal runat="server" ID="ltrlRevenueSaved" Text="Rp 999,999,999" /></div>
          </div>
        </div>
        <div onclick="document.getElementById('<%= btnRevenueSaved.ClientID %>').click();">
          <img src="../../res/images/Volume.png" alt="Image">
          <div class="text-container">
            <div>VOLUME SAVED</div>
            <div style="font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlVolumeSaved" Text="Rp 999,999,999" />&nbsp;m3</div>
          </div>
        </div>
        <div onclick="document.getElementById('<%= btnRevenueSaved.ClientID %>').click();">
          <img src="../../res/images/Flag.png" alt="Image">
          <div class="text-container">
            <div>COMMERCIAL SAVED</div>
            <div style="font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrl_CommercialSaved" Text="999" /></div>
          </div>
        </div>
      </div>

      <div style="display: flex; margin: 30px 10px 10px 10px;">

        <div style="flex-grow: 1;">
          <div style="display: flex;">
            <div style="flex-grow: 1; display: flex; cursor: pointer;" class="smallboxBody" onclick="document.getElementById('<%= btnActivity.ClientID %>').click();">
              <div style="flex-grow: 1;">
                <div>Monthly Revenue Loss</div>
                <div style="font-family: 'Poppins-Bold'; font-weight: bold;">Rp&nbsp;<asp:Literal runat="server" ID="ltrlMonthlyRevenueLoss" Text="Rp 999,999,999" />
                </div>
              </div>
              <div style="flex-grow: 1; text-align: right;"><img src="../../res/images/DollarHitam.png" alt="Image" style="height: 20px"></div>
            </div>
            <div style="width: 20px;"></div>
            <div style="flex-grow: 1; display: flex; cursor: pointer;" class="smallboxBody" onclick="document.getElementById('<%= btnRevenueSaved.ClientID %>').click();">
              <div style="flex-grow: 1;">
                <div>Monthly Ongoing Issue</div>
                <div style="font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlMonthlyOngoingIssue" Text="999" />
                </div>
              </div>
              <div style="flex-grow: 1; text-align: right;"><img src="../../res/images/FlagHitam.png" alt="Image" style="height: 20px"></div>
            </div>
          </div>
          <div style="display: flex; padding-top: 20px;">
            <div style="flex-grow: 1; display: flex; cursor: pointer;" class="smallboxBody" onclick="document.getElementById('<%= btnActivity.ClientID %>').click();">
              <div style="flex-grow: 1;">
                <div>Monthly Volume Loss</div>
                <div style="font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlMonthlyVolumeLoss" Text="Rp 999,999,999" />&nbsp;m3</div>
              </div>
              <div style="flex-grow: 1; text-align: right;"><img src="../../res/images/DollarHitam.png" alt="Image" style="height: 20px"></div>
            </div>
            <div style="width: 20px;"></div>
            <div style="flex-grow: 1; display: flex; cursor: pointer;" class="smallboxBody" onclick="document.getElementById('<%= btnActivityPreviousMonth.ClientID %>').click();">
              <div style="flex-grow: 1;">
                <div>Monthly Finished Issue</div>
                <div style="font-family: 'Poppins-Bold'; font-weight: bold;"><asp:Literal runat="server" ID="ltrlMonthlyFinishedIssued" Text="999" /></div>
              </div>
              <div style="flex-grow: 1; text-align: right;"><img src="../../res/images/FlagHitam.png" alt="Image" style="height: 20px"></div>
            </div>
          </div>

          <div style="padding-top: 15px;">
            <div style="display:flex;">
              <div style="flex-grow:1;">Chart Type</div>
              <div style="flex-grow:1;text-align:right;">
                <asp:DropDownList runat="server" ID="ddlChartType" Style="display: inline-block; width: 250px" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                  <asp:ListItem Text="Overall Volume Losses" Value="VolumeLoss" />
                  <asp:ListItem Text="Overall Revenue Losses" Value="RevenueLoss" />
                </asp:DropDownList>
              </div>
            </div>            
            <asp:Chart ID="cLosses" runat="server" Height="300px" ImageType="Png" Palette="BrightPastel" BackColor="#D3DFF0" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" CssClass="chart" >
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
        <div style="width: 30px;"></div>
        <div style="flex-grow: 1;" class="smallboxBody">
          <div style="font-family: 'Poppins-Bold'; font-weight: bold;">Commloss Issues List</div>
          <div style="font-family: 'Poppins-Bold'; font-weight: bold; display: flex; width: 100%;">
            <div style="flex-grow:1;">
              <asp:Literal runat="server" ID="ltrlRevSaved" Text="Rp 999,999,999" /></div>
            <div style="flex-grow:1; text-align:right;">
              <asp:Literal runat="server" ID="ltrlVolSaved" Text="999 m3" /></div>
          </div>
          <hr style="border: 2px solid black; opacity: 1 !important;" />
          <div style="overflow-y: auto; max-height: 380px;">
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" ShowHeader="false" BorderWidth="0px" Width="100%" OnRowDataBound="gvData_RowDataBound">
              <Columns>
                <asp:TemplateField><ItemTemplate>
                  <asp:Literal runat="server" ID="ltrlSiteNumber" Text='<%# Eval("SiteNumber") %>' Visible="false" />
                  <F:Button runat="server" ID="btnActivityPerStation" Hidden="true" OnClick="Fbtn_Click" />
                  <div runat="server" id="dvStation" class="box" style="cursor: pointer; margin-top:-10px;">
                    <div><asp:Literal runat="server" ID="ltrlStation" Text='<%# Eval("StationName") %>' /></div>
                    <div><asp:Literal runat="server" ID="ltrlRevenueLoss" Text='<%# Eval("RevenueLoss") %>' /></div>
                    <div><asp:Literal runat="server" ID="ltrlVolumeLoss" Text='<%# Eval("VolumeLoss") %>' /></div>
                  </div>
                </ItemTemplate></asp:TemplateField>
              </Columns>
            </asp:GridView>                                    
          </div>
        </div>

      </div>

    </div>
  </form>
  <script>
    function getPerformanceUrl(Month, Year) {      
      return F.baseUrl + 'Pages/Commloss/Performance.aspx?Month=' + Month + '&Year=' + Year + '&parenttabid=' + parent.getActiveTabId();
    }
    function getActivityUrl(Month, Year) {
      return F.baseUrl + 'Pages/Commloss/Activity.aspx?Month=' + Month + '&Year=' + Year + '&parenttabid=' + parent.getActiveTabId();
    }
    function getActivityPerStationUrl(Month, Year, SiteNumber) {
      return F.baseUrl + 'Pages/Commloss/Activity.aspx?Month=' + Month + '&Year=' + Year + '&SiteNumber=' + SiteNumber + '&parenttabid=' + parent.getActiveTabId();
    }
  </script>
</body>
</html>
