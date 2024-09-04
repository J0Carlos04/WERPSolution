using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using System.Web.UI.HtmlControls;

public partial class UserControls_Popup_ucShowColumn : System.Web.UI.UserControl
{
    #region Field
    private const string vsList = "ShowColumnList";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {            
            case "btnSave":
                Save();
                break;
            case "btnClose":
                Close();
                break;
        }
        
    }
    protected void tbColumnName_TextChanged(object sender, EventArgs e)
    {
        Search();
    }    
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvData.ClientID.Replace("_", "$")));

            List<object> oList = (List<object>)gvData.DataSource;
            if (!oList.Exists(a => ((ShowColumn)a).IsChecked == false)) cbCheckAll.Checked = true;
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }    
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        bool ViewAccess = true;  
        List<object> oList = ShowColumn.GetInitColumn(Request.QueryString["Name"], ViewAccess).OrderBy(a => ((ShowColumn)a).Seq).ToList<object>();        
        List<object> scList = ShowColumn.GetByUserName(U.GetUsername(), Request.QueryString["Name"]);
        if (scList.Count != 0)
        {
            foreach (ShowColumn sc in oList)
            {
                ShowColumn scDB = (ShowColumn)scList.Find(a => ((ShowColumn)a).Seq == sc.Seq);
                if (scDB != null) sc.IsChecked = scDB.Visible;
            }
        }
        U.BindGrid(gvData, oList);
        ViewState[vsList] = oList;
    }
    private void Search()
    {
        bool ViewAccess = true;        
        List<object> oListViewState = SynchListViewStateGrid();
        List<object> oList = ShowColumn.GetInitColumn(Request.QueryString["Name"], tbColumnName.Text, ViewAccess).OrderBy(a => ((ShowColumn)a).Seq).ToList<object>();
        foreach (ShowColumn sc in oList)
        {
            ShowColumn scViewState = (ShowColumn)oListViewState.Find(a => ((ShowColumn)a).Seq == sc.Seq);
            if (scViewState != null) sc.IsChecked = scViewState.IsChecked;
        }
        U.BindGrid(gvData, oList);
        ViewState[vsList] = oList;
    }
    private List<object> SynchListViewStateGrid()
    {
        List<object> oListGrid = U.GetGridData(gvData, typeof(ShowColumn)).ListData;
        List<object> oListViewState = (List<object>)ViewState[vsList];
        foreach (ShowColumn sc in oListViewState)
        {
            ShowColumn scGrid = (ShowColumn)oListGrid.Find(a => ((ShowColumn)a).Seq == sc.Seq);
            if (scGrid != null) sc.IsChecked = scGrid.IsChecked;
        }
        return oListViewState;
    }
    private void Close()
    {        
        List<object> oListViewState = SynchListViewStateGrid();        

        string result = "";
        foreach (ShowColumn sc in oListViewState)
        {
            if (!sc.IsChecked)
                result = $"{result},{sc.Seq}";
        }
        if (result != "") result = result.Remove(0, 1);
        ViewState[vsList] = null;
        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference(result));
    }
    private void Save()
    {
        List<object> oList = SynchListViewStateGrid();
        string Result = ShowColumn.DeleteByUserNameModule(U.GetUsername(), Request.QueryString["Name"]);
        if (Result != "")
        {
            U.ShowMessage($"Saved Failed, {Environment.NewLine}{Result}");
            return;
        }
        foreach (ShowColumn sc in oList)
        {
            sc.Visible = sc.IsChecked;
            sc.UserName = U.GetUsername();
            sc.ModuleName = $"{Request.QueryString["Name"]}";
            Result = sc.Insert();
            if (Result != "")
            {
                U.ShowMessage($"Saved Failed, {Environment.NewLine}{Result}");
                return;
            }
        }
        Close();
    }
    #endregion        
}