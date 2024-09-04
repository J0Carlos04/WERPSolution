using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using DAL;
using System.Text;
using System.IO;

public partial class Pages_Admin_Parameters : PageBase
{    
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadFirstTime();
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnSubmit":
                Submit();
                break;
        }
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInit();

        List<object> oList = Parameters.GetALL();
        Parameters p = (Parameters)oList.Find(a => ((Parameters)a).Key == "WODeviationDay");
        if (p != null) tbWoDeviationDays.Value = p.Text;
        p = (Parameters)oList.Find(a => ((Parameters)a).Key == "WODeviationMonth");
        if (p != null) tbWoDeviationMonths.Value = p.Text;
        p = (Parameters)oList.Find(a => ((Parameters)a).Key == "WODeviationYear");
        if (p != null) tbWoDeviationYears.Value = p.Text;
        p = (Parameters)oList.Find(a => ((Parameters)a).Key == "Latitude");
        if (p != null) tbLatitude.Value = p.Text;
        p = (Parameters)oList.Find(a => ((Parameters)a).Key == "Longitude");
        if (p != null) tbLongitude.Value = p.Text;
        p = (Parameters)oList.Find(a => ((Parameters)a).Key == "Version");
        if (p != null) tbVersion.Value = p.Text;
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
    }
    private void Submit()
    {
        Wrapping w = new Wrapping();
        Validation(w);
        if ($"{w.Sb}" != "") U.ShowMessage($"{w.Sb}");
        else
        {
            string Result = Save("WODeviationDay", tbWoDeviationDays.Value, "", "");
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            Result = Save("WODeviationMonth", tbWoDeviationMonths.Value, "", "");
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            Result = Save("WODeviationYear", tbWoDeviationYears.Value, "", "");
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            Result = Save("Latitude", tbLatitude.Value, "", "");
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            Result = Save("Longitude", tbLongitude.Value, "", "");
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            Result = Save("Version", tbVersion.Value, "", "");
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            F.Alert.Show("Data has been Save Successfully", "Information", F.Alert.DefaultMessageBoxIcon, "parent.removeActiveTab();");
        }
    }
    private void Validation(Wrapping w)
    {
        if (tbWoDeviationDays.Value.IsEmpty()) w.RequiredValidation = "Work Order Performance Deviation (in days)";
        if (tbWoDeviationMonths.Value.IsEmpty()) w.RequiredValidation = "Work Order Performance Deviation (in months)";
        if (tbWoDeviationYears.Value.IsEmpty()) w.RequiredValidation = "Work Order Performance Deviation (in years)";
        if (tbLatitude.Value.IsEmpty()) w.RequiredValidation = "Latitude";
        if (tbLongitude.Value.IsEmpty()) w.RequiredValidation = "Longitude";
        if (tbVersion.Value.IsEmpty()) w.RequiredValidation = "Version";
    }
    private string Save(string Key, string Text, string Value, string Atribute)
    {        
        Parameters p = Parameters.GetByKey(Key);
        p.Key = Key;
        p.Text = Text;
        p.Value = Value;
        p.Attribute = Atribute;
        if (p.Id == 0)
        {
            p.CreatedBy = $"{ViewState[CNT.VS.Username]}";
            return p.Insert();
        }
        else
        {
            p.ModifiedBy = $"{ViewState[CNT.VS.Username]}";
            return p.Update();
        }                
    }

    private void SynchCustomer()
    {

    }
    #endregion

}