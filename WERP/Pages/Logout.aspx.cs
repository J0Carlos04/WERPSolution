using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Logout : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetCookie(CNT.Username);
        SetCookie(CNT.Password);
        SetCookie(CNT.User);
        SetCookie(CNT.SuperAdmin);
        SetCookie(CNT.Admin);
        SetCookie(CNT.Approval);
        Session[CNT.Username] = null;
        Session[CNT.Password] = null;
        Session[CNT.User] = null;
        Session[CNT.SuperAdmin] = null;
        Session[CNT.Admin] = null;
        Session[CNT.Approval] = null;
        Response.Redirect(@"~\Pages\Login.aspx");
    }
    private void SetCookie(string Name)
    {
        HttpCookie cookie = new HttpCookie(Name);        
        cookie.Expires = DateTime.Now.AddDays(-1);
        Response.SetCookie(cookie);
    }
}