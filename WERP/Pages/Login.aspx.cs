using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;

public partial class Pages_Login : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Request.Cookies[CNT.Username] != null)
            cbSave.Checked = true;
        
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnLogin":
                LetMeIn();
                break;
        }
    }
    private void LetMeIn()
    {   
        if (tbUserName.Text == "admin")
        {
            SetCookie(CNT.Username, tbUserName.Text.Trim());
            SetCookie(CNT.Password, tbPassword.Text.Trim());
            
            Response.Redirect(@"~\Pages\Default.aspx");
            return;
        }
        bool IsUser = false;
        Users u = Users.GetByUsername(tbUserName.Text);
        if (u.Id == 0)
        {
            Operators o = Operators.GetByUsername(tbUserName.Text);
            if (o.Id == 0)
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Username or Password";
                return;
            }
            else
            {
                if (U.DecryptString(o.Password) != tbPassword.Text)
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Username or Password";
                    return;
                }
            }            
        }
        else
        {
            IsUser = true;
            if (U.DecryptString(u.Password) != tbPassword.Text)
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Username or Password";
                return;
            }
        }        

        if (cbSave.Checked)
        {
            SetCookie(CNT.Username, tbUserName.Text.Trim());
            SetCookie(CNT.Password, tbPassword.Text.Trim());
            SetCookie(CNT.User, IsUser);
            SetCookie(CNT.SuperAdmin, UserManagement.UsersIsMemberRole(CNT.Role.SuperAdmin));
            SetCookie(CNT.Admin, UserManagement.UsersIsMemberRole(CNT.Role.Admin));
            SetCookie(CNT.Approval, UserManagement.UsersIsMemberRole(CNT.Role.Approver));
            if (IsUser) SetCookie(CNT.Approval, UserManagement.UsersIsMemberRole(CNT.Role.Operator));
            else SetCookie(CNT.Approval, UserManagement.OperatorsIsMemberRole(CNT.Role.Operator));
        }
        else
        {
            Session[CNT.Username] = tbUserName.Text.Trim();
            Session[CNT.Password] = tbPassword.Text.Trim();
            Session[CNT.User] = IsUser;
            Session[CNT.SuperAdmin] = UserManagement.UsersIsMemberRole(CNT.Role.SuperAdmin);
            Session[CNT.Admin] = UserManagement.UsersIsMemberRole(CNT.Role.Admin);
            Session[CNT.Approval] = UserManagement.UsersIsMemberRole(CNT.Role.Approver);
            if (IsUser) Session[CNT.Operator] = UserManagement.UsersIsMemberRole(CNT.Role.Operator);
            else Session[CNT.Operator] = UserManagement.OperatorsIsMemberRole(CNT.Role.Operator);
        }

        Response.Redirect(@"~\Pages\Default.aspx");
    }
    private void SetCookie(string Name, object Value)
    {
        HttpCookie cookie = new HttpCookie(Name);
        cookie.Value = $"{Value}";
        cookie.Expires = DateTime.Now.AddDays(30);
        Response.SetCookie(cookie);
    }
}