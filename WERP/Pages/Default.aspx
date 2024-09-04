<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WERP</title>
    <link rel="shortcut icon" type="image/x-icon" href="../favicon.png" />
    <meta name="Title" content="WERP" />
    <meta name="Description" content="WERP" />
    <meta name="Keywords" content="WERP" />
    <link type="text/css" rel="stylesheet" href="../res/css/default.css" />
    <link href="../res/css/BootStrap.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>  
</head>
<body>
    <form id="form1" runat="server">       
    <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvFilter,gvData,lblRowCount,lnkFirst,lnkPrevious,ddlPage,lnkNext,lnkLast"></f:PageManager>
    <f:Panel ID="pContent" runat="server" Layout="Region" ShowBorder="false">
      <Items>
        <f:ContentPanel runat="server" ID="TopPanel" ShowBorder="false" ShowHeader="false" RegionPosition="Top" >
          <div id="header" style="background-image: url('../res/images/Header.png'); background-repeat: no-repeat; background-position:top left; background-size: 100% 100%; height:70px;">
            <table style="height:100%;">
              <tr>
                <td style="padding-top:15px; width:270px; background-color:white;border-bottom-right-radius: 15px;" >
                  <a href="./default.aspx" title="Home"><img src="../res/images/AdaroLogo.png" alt="Home" style="height:35px; margin-bottom:19px; " /></a>
                  <a class="logo" href="./default.aspx" title="WERP" style="color:#8b8480;">WERP</a>                                              
                </td>
                <td style="text-align: right;">                            
                  <f:Button runat="server" ID="btnMenu" Text="Welcome JCarlos" Icon="User" IconAlign="Left" EnablePostBack="false" CssStyle="color:#157fcc !important;">
                    <Menu runat="server">                           
                      <f:MenuSeparator ID="MenuSeparator1" runat="server" />                    
                      <f:MenuButton ID="MenuTheme" EnablePostBack="true" Text="Theme" runat="server">
                        <Menu ID="Menu4" runat="server">
                            <f:MenuCheckBox Text="Triton" ID="MenuThemeTriton" Checked="true" GroupName="MenuTheme" runat="server" />
                            <f:MenuCheckBox Text="Cris" ID="MenuThemeCrisp" GroupName="MenuTheme" runat="server" />
                            <f:MenuCheckBox Text="Neptun" ID="MenuThemeNeptune" GroupName="MenuTheme" runat="server" />
                            <f:MenuCheckBox Text="Blue" ID="MenuThemeBlue" GroupName="MenuTheme" runat="server" />
                            <f:MenuCheckBox Text="Gray" ID="MenuThemeGray" GroupName="MenuTheme" runat="server" />
                        </Menu>
                      </f:MenuButton>
                      <f:MenuButton EnablePostBack="false" Text="Menu Style" ID="MenuStyle" runat="server">
                        <Menu runat="server">
                            <f:MenuCheckBox Text="Tree Menu" ID="MenuStyleTree" Checked="true" GroupName="MenuStyle" runat="server" />
                            <f:MenuCheckBox Text="Accordion + Tree Menu" ID="MenuStyleAccordion" GroupName="MenuStyle" runat="server" />
                        </Menu>
                      </f:MenuButton>                    
                      <f:MenuSeparator ID="MenuSeparator2" runat="server" />
                      <f:MenuHyperLink runat="server" Text="Change Password" NavigateUrl="./ChangePassword.aspx" />
                      <f:MenuHyperLink runat="server" Text="Sign Out" NavigateUrl="./Logout.aspx" />
                    </Menu>
                  </f:Button>
                </td>
              </tr>
            </table>
          </div>
        </f:ContentPanel>
        <f:Panel ID="LeftPanel" RegionSplit="true" Width="180px" ShowHeader="true" ShowBorder="true" Title="MENU" EnableCollapse="true" Layout="Fit" Collapsed="false" RegionPosition="Left" runat="server" />        
        <f:Panel ID="mainRegion" ShowHeader="false" Layout="Fit" ShowBorder="true" RegionPosition="Center" runat="server" >
          <Items>
            <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
              <Tabs>
                <f:Tab Title="Home" Layout="Fit" Icon="House" CssClass="maincontent" runat="server">                            
                  <Items>
                    <f:Panel runat="server" ID="pMainContent" ShowBorder="false" BodyPadding="5px" ShowHeader="false" AutoScroll="true" Layout="Region">
                      <Items>                                  
                        <f:ContentPanel runat="server" ID="pData" RegionPosition="Center" ShowBorder="true" ShowHeader="false" AutoScroll="true">      
                          <Content>                                                                                                                           
                          </Content>
                        </f:ContentPanel>
                      </Items>                                  
                    </f:Panel>                                                    
                  </Items>
                </f:Tab>
              </Tabs>
            </f:TabStrip>
            </Items>
        </f:Panel>
        <f:Panel ID="bottomPanel" RegionPosition="Bottom" ShowBorder="false" ShowHeader="false" EnableCollapse="false" runat="server" Layout="Fit">
            <Items>
                <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                    <table class="bottomtable">
                        <tr>
                            <td style="width: 300px;"></td>
                            <td style="text-align: center;">Copyright &copy; 2022 IT AEI</td>
                            <td style="width: 300px; text-align: right;">Total Online User：<asp:Literal runat="server" ID="litOnlineUserCount"></asp:Literal>&nbsp;</td>
                        </tr>
                    </table>
                </f:ContentPanel>
            </Items>
        </f:Panel>
      </Items>
    </f:Panel>    
    <f:Window ID="windowLoadingSelector" Title="Loading Animation" Hidden="true" EnableIFrame="true" IFrameUrl="../Pages/loading.aspx" runat="server" IsModal="true" Width="1000px" Height="625px" EnableClose="true" EnableMaximize="true" EnableResize="true" />
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" EnableCaching="false" DataFile="~/common/admin.xml"></asp:XmlDataSource>
</form>
<script src="../res/js/jquery.min.js"></script>
    <script>

        var LeftPanelClientID = '<%= LeftPanel.ClientID %>';
        var mainTabStripClientID = '<%= mainTabStrip.ClientID %>';        
        var windowLoadingSelectorClientID = '<%= windowLoadingSelector.ClientID %>';

        var MenuStyleClientID = '<%= MenuStyle.ClientID %>';        
        var MenuThemeClientID = '<%= MenuTheme.ClientID %>';        
        
        function addExampleTab(tabOptions) {

            if (typeof (tabOptions) === 'string') {
                tabOptions = {
                    id: arguments[0],
                    iframeUrl: arguments[1],
                    title: arguments[2],
                    icon: arguments[3],
                    createToolbar: arguments[4],
                    refreshWhenExist: arguments[5],
                    iconFont: arguments[6]
                };
            }

            F.addMainTab(F(mainTabStripClientID), tabOptions);
        }
        
        function removeTab(tabId) {
            var mainTabStrip = F(mainTabStripClientID);
            mainTabStrip.removeTab(tabId);            
        }
        
        function removeActiveTab() {
            var mainTabStrip = F(mainTabStripClientID);
            var activeTab = mainTabStrip.getActiveTab();
            mainTabStrip.removeTab(activeTab.id);
            ClientChanged('pContent_mainRegion_mainTabStrip_ctl00_pMainContent_pFilter_btnSearch');
        }
        
        function getActiveTabId() {
            var mainTabStrip = F(mainTabStripClientID);

            var activeTab = mainTabStrip.getActiveTab();
            if (activeTab) {
                return activeTab.id;
            }
            return '';
        }
        
        function activeTabAndRefresh(tabId) {
            var mainTabStrip = F(mainTabStripClientID);
            var targetTab = mainTabStrip.getTab(tabId);
            var oldActiveTabId = getActiveTabId();

            if (targetTab) {
                mainTabStrip.setActiveTab(targetTab);
                // 通过jQuery查找 iframe 节点，并强制刷新 iframe 内的页面
                $(targetTab.el.dom).find('iframe')[0].contentWindow.location.reload();

                // 删除之前的激活选项卡
                mainTabStrip.removeTab(oldActiveTabId);
            }
        }
        
           

        F.ready(function () {
            var LeftPanel = F(LeftPanelClientID);
            var mainTabStrip = F(mainTabStripClientID);           

            var MenuStyle = F(MenuStyleClientID);           
            var MenuTheme = F(MenuThemeClientID);

            var treeMenu = LeftPanel.items.getAt(0);
            var menuType = 'accordion';
            if (treeMenu.isXType('treepanel')) {
                menuType = 'menu';
            }            

            // 点击菜单样式
            function MenuStyleItemCheckChange(cmp, checked) {
                if (checked) {
                    var menuStyle = 'accordion';
                    if (cmp.id.indexOf('MenuStyleTree') >= 0) {
                        menuStyle = 'tree';
                    }
                    F.cookie('MenuStyle_v6', menuStyle, {
                        expires: 100 // 单位：天
                    });
                    top.window.location.reload();
                }
            }
            MenuStyle.menu.items.each(function (item, index) {
                item.on('checkchange', MenuStyleItemCheckChange);
            });            


            // 切换主题
            function MenuThemeItemCheckChange(cmp, checked) {
                if (checked) {
                    var lang = 'neptune';
                    if (cmp.id.indexOf('MenuThemeBlue') >= 0) {
                        lang = 'blue';
                    } else if (cmp.id.indexOf('MenuThemeGray') >= 0) {
                        lang = 'gray';
                    } else if (cmp.id.indexOf('MenuThemeCrisp') >= 0) {
                        lang = 'crisp';
                    } else if (cmp.id.indexOf('MenuThemeTriton') >= 0) {
                        lang = 'triton';
                    }

                    F.cookie('Theme_v6', lang, {
                        expires: 100 // 单位：天
                    });
                    top.window.location.reload();
                }
            }
            MenuTheme.menu.items.each(function (item, index) {
                item.on('checkchange', MenuThemeItemCheckChange);
            });


            function createToolbar(tabConfig) {

                // 由工具栏上按钮获得当前标签页中的iframe节点
                function getCurrentIFrameNode(btn) {
                    return $('#' + btn.id).parents('.f-tab').find('iframe');
                }               

                var openNewWindowButton = new Ext.Button({
                    text: 'Open in new tab',
                    type: 'button',
                    icon: '../res/icon/tab_go.png',
                    listeners: {
                        click: function () {
                            var iframeNode = getCurrentIFrameNode(this);
                            window.open(iframeNode.attr('src'), '_blank');
                        }
                    }
                });

                var refreshButton = new Ext.Button({
                    text: 'Refresh',
                    type: 'button',
                    icon: '../res/icon/reload.png',
                    listeners: {
                        click: function () {
                            var iframeNode = getCurrentIFrameNode(this);
                            iframeNode[0].contentWindow.location.reload();
                        }
                    }
                });

                var toolbar = new Ext.Toolbar({
                    items: ['->', refreshButton, '-', openNewWindowButton]
                });

                tabConfig['tbar'] = toolbar;
            }
            
            F.initTreeTabStrip(treeMenu, mainTabStrip, {
                /*createToolbar: createToolbar,*/
                maxTabCount: 20,
                maxTabMessage: 'Please close some tabs first (up to 15 are allowed)!'
            });          
        });
    </script>
    <script lang="javascript" type="text/javascript" src="../res/js/bootstrap.bundle.js"></script>
</body>
</html>
