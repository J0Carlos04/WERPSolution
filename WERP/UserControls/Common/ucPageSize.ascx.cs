using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class ucPageSize : System.Web.UI.UserControl
{
    public event System.EventHandler SizeChanged;    
    public bool Enabled
    {
        get { return ddlNumberItem.Enabled; }
        set { ddlNumberItem.Enabled = value; }
    }
    public string SelectedValue
    {
        get { return ddlNumberItem.SelectedValue; }
        set { ddlNumberItem.SelectedValue = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {        
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        SizeChanged(sender, e);
    }
}