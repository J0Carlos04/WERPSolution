using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;

public partial class Pages_Admin_PODetail : PageBase
{    
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadFirstTime();
        }        
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {            
            case "btnDownload":
                U.OpenFile(lbPO.Text, U.GetContentType(lbPO.Text), Convert.FromBase64String(lblPO.Text));
                break;            
        }
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        lbPO.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownload.ClientID));
        SetInitControl();
        SetInitEdit();
    }
    private void SetInitControl()
    {        
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
    #endregion
}