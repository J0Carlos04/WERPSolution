using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using System.Web.UI.HtmlControls;
using System.Xml;

public partial class _Default : PageBase
{
    #region Fields
    private string _menuType = "menu";
    private bool _showOnlyNew = false;
    private int _examplesCount = 0;

    
    
    private const string vsSortField = "SortField";
    private const string vsSortDirection = "SortDirection";
    
    
    private const string InitSortField = "a.Modified";
    private const string vsMode = "Mode";    
    #endregion

    #region Handlers
    protected void Page_Init(object sender, EventArgs e)
    {
        string UserName = U.GetUsername();
        if (UserName == "")
        {
            Response.Write("<script language='javascript'>self.parent.location='Login.aspx';</script>");
            return;
        }
        HttpCookie menuCookie = Request.Cookies["MenuStyle_v6"];
        if (menuCookie != null)
            _menuType = menuCookie.Value;

        if (_menuType == "accordion")
            InitAccordionMenu();
        else
            InitTreeMenu();

        LeftPanel.Title = "MENU";
    }
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack) LoadFirstTime();       
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {            
            
        }
    }    
    protected void lb_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        switch (lb.ID)
        {
            
        }
    }               
    #endregion

    #region Methods
    #region Page_Init    
    private F.Accordion InitAccordionMenu()
    {
        F.Accordion accordionMenu = new F.Accordion();
        accordionMenu.ID = "accordionMenu";
        accordionMenu.ShowBorder = false;
        accordionMenu.ShowHeader = false;
        LeftPanel.Items.Add(accordionMenu);

        XmlDataSource1.DataFile = "~/res/Menu/Admin.xml";
        XmlDocument xmlDoc = XmlDataSource1.GetXmlDocument();        
        XmlNodeList xmlNodes = xmlDoc.SelectNodes("/Tree/TreeNode");
        foreach (XmlNode xmlNode in xmlNodes)
        {
            if (xmlNode.HasChildNodes)
            {
                F.AccordionPane accordionPane = new F.AccordionPane();
                accordionPane.Title = xmlNode.Attributes["Text"].Value;
                accordionPane.Layout = F.Layout.Fit;
                accordionPane.ShowBorder = false;

                var accordionPaneIconAttr = xmlNode.Attributes["Icon"];
                if (accordionPaneIconAttr != null)
                {
                    accordionPane.Icon = (F.Icon)Enum.Parse(typeof(F.Icon), accordionPaneIconAttr.Value, true);
                }

                accordionMenu.Items.Add(accordionPane);

                F.Tree innerTree = new F.Tree();
                innerTree.ShowBorder = false;
                innerTree.ShowHeader = false;
                innerTree.EnableIcons = true;
                innerTree.AutoScroll = true;
                innerTree.EnableSingleClickExpand = true;
                accordionPane.Items.Add(innerTree);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(String.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Tree>{0}</Tree>", xmlNode.InnerXml));
                ResolveXmlDocument(doc);

                // 绑定AccordionPane内部的树控件
                innerTree.NodeDataBound += treeMenu_NodeDataBound;
                innerTree.PreNodeDataBound += treeMenu_PreNodeDataBound;
                innerTree.DataSource = doc;
                innerTree.DataBind();
            }
        }

        return accordionMenu;
    }
    private F.Tree InitTreeMenu()
    {
        F.Tree treeMenu = new F.Tree();
        treeMenu.ID = "treeMenu";
        treeMenu.ShowBorder = false;
        treeMenu.ShowHeader = false;
        treeMenu.EnableIcons = true;
        treeMenu.AutoScroll = true;
        treeMenu.EnableSingleClickExpand = true;
        LeftPanel.Items.Add(treeMenu);

        //if ((bool)ViewState[vsIsUserFinance]) XmlDataSource1.DataFile = "~/common/Admin.xml";
        //else XmlDataSource1.DataFile = "~/common/VendorMenu.xml";
        XmlDataSource1.DataFile = "~/res/Menu/Admin.xml";

        XmlDocument doc = XmlDataSource1.GetXmlDocument();
        ResolveXmlDocument(doc);

        treeMenu.NodeDataBound += treeMenu_NodeDataBound;
        treeMenu.PreNodeDataBound += treeMenu_PreNodeDataBound;
        treeMenu.DataSource = doc;
        treeMenu.DataBind();

        return treeMenu;
    }

    #region ResolveXmlDocument
    private void ResolveXmlDocument(XmlDocument doc)
    {
        if (!U.IsMember(CNT.SuperAdmin)) SetDisplayMenu(doc, doc.DocumentElement.ChildNodes);
        XmlAttribute removedAttr = doc.CreateAttribute("Removed");
        removedAttr.Value = "true";

        //if (!U.IsMember(CNT.SuperAdmin))
        //{
        //    doc.DocumentElement.ChildNodes[0].Attributes.Append(removedAttr);
        //    if (!U.IsMember(CNT.Admin))
        //    {
        //        removedAttr = doc.CreateAttribute("Removed");
        //        doc.DocumentElement.ChildNodes[1].Attributes.Append(removedAttr);
        //    }           
        //}        
        //ResolveXmlDocument(doc, doc.DocumentElement.ChildNodes);
    }
    private int SetDisplayMenu(XmlDocument doc, XmlNodeList nodes)
    {
        int nodeVisibleCount = 0;

        foreach (XmlNode node in nodes)
        {
            if (node.NodeType != XmlNodeType.Element) continue;
            if (node.Attributes.Count == 0) continue;
            XmlAttribute Hide = doc.CreateAttribute("Removed");
            Hide.Value = "true";

            string PageName = "";
            if (node.Attributes.GetNamedItem(CNT.NavigateUrl) != null)
            {
                PageName = node.Attributes[CNT.NavigateUrl].Value;
                PageName = PageName.Substring(PageName.LastIndexOf("/") + 1).Replace(".aspx", "");
            }

            switch (node.Attributes[CNT.Text].Value)
            {
                case CNT.Menu.SuperAdmin:
                    node.Attributes.Append(Hide);
                    break;
                case CNT.Menu.Admin:
                    if (!U.IsMember(CNT.Admin)) node.Attributes.Append(Hide);
                    break;
                case CNT.Menu.StockOrder:                
                    if (!(U.ViewAccess(PageName) || U.IsMember(CNT.Approval))) node.Attributes.Append(Hide);
                    else nodeVisibleCount++;
                    break;
                case CNT.Menu.StockReceivedPending:
                    if (!(U.ViewAccess("StockReceived"))) node.Attributes.Append(Hide);
                    else nodeVisibleCount++;
                    break;
                case CNT.Menu.WorkOrder:
                    bool IsOperator = U.IsMember(CNT.Role.Operator);                   
                    if (!IsOperator)
                    {
                        if (!U.ViewAccess(PageName)) node.Attributes.Append(Hide);
                        else nodeVisibleCount++;
                    }

                    SetDisplayMenu(doc, node.ChildNodes);
                    break;
                default:
                    int TotalVisibleNode = 0;
                    if (node.ChildNodes.Count != 0)
                        TotalVisibleNode = SetDisplayMenu(doc, node.ChildNodes);

                    if (node.Attributes.GetNamedItem(CNT.NavigateUrl) == null)
                    {
                        if (TotalVisibleNode == 0)
                            node.Attributes.Append(Hide);
                    }
                    else
                    {
                        if (!U.ViewAccess(PageName)) node.Attributes.Append(Hide);
                        else nodeVisibleCount++;
                    }
                    break;
            }
        }

        return nodeVisibleCount;
    }
    private int ResolveXmlDocument(XmlDocument doc, XmlNodeList nodes)
    {
        int nodeVisibleCount = 0;

        foreach (XmlNode node in nodes)
        {
            // Only process Xml elements (ignore comments, etc)
            if (node.NodeType == XmlNodeType.Element)
            {
                XmlAttribute removedAttr;
                
                bool isLeaf = node.ChildNodes.Count == 0;


                // All filter conditions are for leaf nodes, and whether to display the directory depends on whether there are leaf nodes
                if (isLeaf)
                {
                    // 如果仅显示最新示例
                    if (_showOnlyNew)
                    {
                        XmlAttribute isNewAttr = node.Attributes["IsNew"];
                        if (isNewAttr == null)
                        {
                            removedAttr = doc.CreateAttribute("Removed");
                            removedAttr.Value = "true";

                            node.Attributes.Append(removedAttr);

                        }
                    }
                }
                
                if (!isLeaf)
                {
                    // 递归
                    int childVisibleCount = ResolveXmlDocument(doc, node.ChildNodes);

                    if (childVisibleCount == 0)
                    {
                        removedAttr = doc.CreateAttribute("Removed");
                        removedAttr.Value = "true";

                        node.Attributes.Append(removedAttr);
                    }
                }


                removedAttr = node.Attributes["Removed"];
                if (removedAttr == null)
                {
                    nodeVisibleCount++;
                }
            }
        }

        return nodeVisibleCount;
    }
    #endregion

    private void treeMenu_NodeDataBound(object sender, FineUI.TreeNodeEventArgs e)
    {        
        bool isLeaf = e.XmlNode.ChildNodes.Count == 0;

        string isNewHtml = GetIsNewHtml(e.XmlNode);
        if (!String.IsNullOrEmpty(isNewHtml))
        {
            e.Node.Text += isNewHtml;
        }

        if (isLeaf)
        {            
            e.Node.ToolTip = e.Node.Text;
        }
        
        if (_showOnlyNew && !e.Node.Leaf)
        {
            e.Node.Expanded = true;
        }

    }

    private void treeMenu_PreNodeDataBound(object sender, F.TreePreNodeEventArgs e)
    {
        /*
        // 如果仅显示最新示例
        if (showOnlyNew)
        {
            string isNewHtml = GetIsNewHtml(e.XmlNode);
            if (String.IsNullOrEmpty(isNewHtml))
            {
                e.Cancelled = true;
            }
        }

        // 更新示例总数
        if (e.XmlNode.ChildNodes.Count == 0)
        {
            examplesCount++;
        }
        */

        // 是否叶子节点
        bool isLeaf = e.XmlNode.ChildNodes.Count == 0;

        XmlAttribute removedAttr = e.XmlNode.Attributes["Removed"];
        if (removedAttr != null)
        {
            e.Cancelled = true;
        }

        if (isLeaf && !e.Cancelled)
        {
            _examplesCount++;
        }
    }

    private string GetIsNewHtml(XmlNode node)
    {
        string result = String.Empty;

        XmlAttribute isNewAttr = node.Attributes["IsNew"];
        if (isNewAttr != null)
        {
            if (Convert.ToBoolean(isNewAttr.Value))
            {
                result = "&nbsp;<span class=\"isnew\">New!</span>";
            }
        }

        return result;
    }
    #endregion
    #region Page_Load
    private void LoadFirstTime()
    {
        InitMenuStyleButton();
        InitThemeMenuButton();
        litOnlineUserCount.Text = $"{Application["OnlineUserCount"]}";
        btnMenu.Text = $"Welcome, {U.GetUsername()}";
    }    
    private void InitMenuStyleButton()
    {
        string menuStyleID = "MenuStyleTree";

        HttpCookie menuStyleCookie = Request.Cookies["MenuStyle_v6"];
        if (menuStyleCookie != null)
        {
            switch (menuStyleCookie.Value)
            {
                case "menu":
                    menuStyleID = "MenuStyleTree";
                    break;
                case "accordion":
                    menuStyleID = "MenuStyleAccordion";
                    break;
            }
        }


        SetSelectedMenuID(MenuStyle, menuStyleID);
    }
    private void InitThemeMenuButton()
    {
        string themeMenuID = "MenuThemeNeptune";

        string themeValue = pm.Theme.ToString().ToLower();
        switch (themeValue)
        {
            case "classic":
            case "blue":
                themeMenuID = "MenuThemeBlue";
                break;
            case "gray":
                themeMenuID = "MenuThemeGray";
                break;
            case "neptune":
                themeMenuID = "MenuThemeNeptune";
                break;
            case "crisp":
                themeMenuID = "MenuThemeCrisp";
                break;
            case "triton":
                themeMenuID = "MenuThemeTriton";
                break;
        }

        SetSelectedMenuID(MenuTheme, themeMenuID);
    }
    private void SetSelectedMenuID(F.MenuButton menuButton, string selectedMenuID)
    {
        foreach (FineUI.MenuItem item in menuButton.Menu.Items)
        {
            F.MenuCheckBox menu = (item as F.MenuCheckBox);
            if (menu != null && menu.ID == selectedMenuID)
                menu.Checked = true;
            else
                menu.Checked = false;
        }
    }
    #endregion    
    #endregion    
}