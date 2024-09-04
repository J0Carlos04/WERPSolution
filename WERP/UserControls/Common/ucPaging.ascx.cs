using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class UserControls_Common_ucPaging : System.Web.UI.UserControl
{
    public event System.EventHandler pageChanged;

    protected void Page_Load(object sender, EventArgs e)
    {        
    }

    public void SetTotalRow(double TotalPage, double CurrentPage)
    {
        ddlPage.Items.Clear();
        for (double i = 1; i <= TotalPage; i++)
            ddlPage.Items.Add(new ListItem(i.ToString(), i.ToString()));

        if (ddlPage.Items.Count < CurrentPage) ddlPage.SelectedValue = "1";
        else ddlPage.SelectedValue = CurrentPage.ToString();

        if (TotalPage == 1)
        {
            lnkPrevious.Enabled = false;
            lnkFirst.Enabled = false;
            lnkNext.Enabled = false;
            lnkLast.Enabled = false;
        }
        else if (ddlPage.SelectedValue == "1")
        {
            lnkPrevious.Enabled = false;
            lnkFirst.Enabled = false;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
        }
        else if (ddlPage.SelectedValue == ddlPage.Items.Count.ToString())
        {
            lnkPrevious.Enabled = true;
            lnkFirst.Enabled = true;
            lnkNext.Enabled = false;
            lnkLast.Enabled = false;
        }
        else
        {
            lnkPrevious.Enabled = true;
            lnkFirst.Enabled = true;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
        }
    }

    protected void linkPaging_Click(object sender, EventArgs e)
    {
        lnkFirst.Enabled = true;
        lnkPrevious.Enabled = true;
        lnkNext.Enabled = true;
        lnkLast.Enabled = true;

        LinkButton lb = (LinkButton)sender;

        switch (lb.ID)
        {
            case "lnkFirst":
                ddlPage.SelectedValue = "1";
                //                GetOnePageGridView();
                lnkPrevious.Enabled = false;
                lnkFirst.Enabled = false;
                break;
            case "lnkPrevious":
                ddlPage.SelectedValue = (Convert.ToInt32(ddlPage.SelectedValue) - 1).ToString();
                if (ddlPage.SelectedValue == "1")
                {
                    lnkPrevious.Enabled = false;
                    lnkFirst.Enabled = false;
                }
                break;
            case "lnkNext":
                ddlPage.SelectedValue = (Convert.ToInt32(ddlPage.SelectedValue) + 1).ToString();
                if (ddlPage.SelectedValue == ddlPage.Items.Count.ToString())
                {
                    lnkNext.Enabled = false;
                    lnkLast.Enabled = false;
                }
                break;
            case "lnkLast":
                ddlPage.SelectedValue = (ddlPage.Items.Count).ToString();
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
                break;
        }

        pageChanged(sender, e);
    }

    protected void ddl_paging_ItemCommand(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        switch (ddl.ID)
        {
            case "ddlPage":
                lnkFirst.Enabled = true;
                lnkPrevious.Enabled = true;
                lnkNext.Enabled = true;
                lnkLast.Enabled = true;

                if (ddlPage.SelectedValue == "1")
                {
                    lnkFirst.Enabled = false;
                    lnkPrevious.Enabled = false;
                }
                if (ddlPage.SelectedValue == ddlPage.Items.Count.ToString())
                {
                    lnkNext.Enabled = false;
                    lnkLast.Enabled = false;
                }
                break;
        }
        pageChanged(sender, e);
    }     
}
