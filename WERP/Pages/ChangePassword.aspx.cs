using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;

public partial class Pages_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Wrapping w = new Wrapping();
        if (tbCurrentPassword.Value.IsEmpty()) w.RequiredValidation = "Current Password";
        if (tbNewPassword.Value.IsEmpty()) w.RequiredValidation = "New Password";
        if (tbRepeatPassword.Value.IsEmpty()) w.RequiredValidation = "Repeat Password";
        if (tbCurrentPassword.Value.Is(tbNewPassword.Value)) w.ErrorValidation = "New Password already Used";
        if (tbNewPassword.Value != tbRepeatPassword.Value) w.ErrorValidation = "New Password and Repeat Password not same";
        if (w.Sb.IsNotEmpty())
        {
            U.ShowMessage(w.Sb);
            return;
        }

        string ErrorMessage = "";
        string Username = U.GetUsername();
        string Password = "";
        Users u = Users.GetByUsername(Username);
        Operators o = Operators.GetByUsername(Username);
        bool IsUser = false;
        if (u.Id.IsNotZero())
        {
            Password = U.DecryptString(u.Password);
            IsUser = true;
        }
        else
        {
            if (o.Id.IsNotZero())
                Password = U.DecryptString(o.Password);
        }
        if (Password.IsEmpty())
        {
            U.ShowMessage("User / Operator is not found");
            return;
        }
        if (Password.IsNot(tbCurrentPassword.Value))
        {
            U.ShowMessage("Invalid Current Password");
            return;
        }
        if (IsUser)
        {
            u.Password = U.EncryptString(tbNewPassword.Value);
            ErrorMessage = u.ChangePassword();
            if (ErrorMessage.ContainErrorMessage())
                U.ShowMessage(ErrorMessage);
        }
        else
        {
            o.Password = U.EncryptString(tbNewPassword.Value);
            ErrorMessage = o.ChangePassword();
            if (ErrorMessage.ContainErrorMessage())
                U.ShowMessage(ErrorMessage);
        }
        U.ShowMessage("Password has been changed successfully, Please Logout and login with new password", FineUI.Icon.Information);        
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {       
        Response.Redirect(@"~\Pages\Logout.aspx");
    }
}