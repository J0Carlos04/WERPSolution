﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ErrorList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Utility.BindGrid(gvData, Session[CNT.Session.ErrorList]);
    }
}