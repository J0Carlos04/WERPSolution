using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.IO;

public partial class Pages_Admin_POInput : PageBase
{
    #region Fields
    private const string vsUserName = "Username";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadFirstTime();
        }
        ViewState[vsUserName] = U.GetUsername();
        if ($"{ViewState[vsUserName]}" == "") Response.Redirect(@"~\Pages\default.aspx");
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnPO":
                U.Upload(fuPO, lblPO, lbPO);
                break;
            case "btnDownload":
                U.OpenFile(lbPO.Text, U.GetContentType(lbPO.Text), Convert.FromBase64String(lblPO.Text));
                break;
            case "btnSubmit":
                Submit();
                break;
            case "btnDelete":
                Delete();
                break;
        }
    }    
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInit();
        SetInitControl();
        SetInitEdit();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");        
    }
    private void SetInitControl()
    {
        lbPO.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownload.ClientID));
        fuPO.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnPO.ClientID));                 
        btnCancel.OnClientClick = "parent.removeActiveTab();";
    }
    private void SetInitEdit()
    {
        if (U.Id.IsNull()) return;
        PO p = PO.GetById(U.Id);
        tbPONo.Value = p.PONo;
        tbPODate.Value = p.PODate.ToString("yyyy-MM-dd");
        tbPRNo.Value = p.PRNo;
        tbQuotNo.Value = p.QuotNo;                
        tbDetail.Value = p.Detail;
        lbPO.Text = p.FileName;
        lblPO.Text = Convert.ToBase64String(p.FileData);
    }

    private void Submit()
    {
        string Result = Validation(new Wrapping());
        if (Result != "") U.ShowMessage(Result);
        else
        {
            Result = Save();
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");

        }
    }
    private string Validation(Wrapping w)
    {
        if (tbPONo.Value.IsEmpty()) w.RequiredValidation = "PO No.";
        if (tbPODate.Value.IsEmpty()) w.RequiredValidation = "PO Date";
        if (tbPRNo.Value.IsEmpty()) w.RequiredValidation = "PR No.";
        if (tbQuotNo.Value.IsEmpty()) w.RequiredValidation = "Quot No.";
        
        if (lblPO.Text.IsEmpty()) w.RequiredValidation = "PO Attachment";        
        return $"{w.Sb}";
    }
    private string Save()
    {
        PO o = new PO();
        o.PONo = tbPONo.Value;
        o.PODate = U.GetDate(tbPODate.Value);
        o.PRNo = tbPRNo.Value;
        o.QuotNo = tbQuotNo.Value;        
        o.Detail = tbDetail.Value;
        o.FileName = lbPO.Text;
        o.FileData = Convert.FromBase64String(lblPO.Text);
        string Result = "";
        if (U.Id == null)
        {
            o.CreatedBy = $"{ViewState[CNT.VS.Username]}";
            Result = o.Insert();
            if (Result.ContainErrorMessage()) return Result;
            o.Id = Result.ToInt();
        }
        else
        {
            o.Id = U.Id.ToInt();
            o.ModifiedBy = $"{ViewState[CNT.VS.Username]}";
            Result = o.Update();
            if (Result.ContainErrorMessage()) return Result;
        }

        return "";
    }
    private void Delete()
    {
        string Result = PO.Delete(U.Id);
        if (Result.ContainErrorMessage()) U.ShowMessage(Result);
        else F.Alert.Show("Data has been Delete Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
    }
    #endregion
}