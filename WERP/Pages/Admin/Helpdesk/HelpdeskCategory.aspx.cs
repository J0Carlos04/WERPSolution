using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;

public partial class Pages_Admin_Helpdesk_HelpdeskCategory : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadFirstTime();
    }

    private void LoadFirstTime()
    {
        U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath));

        
    }
}