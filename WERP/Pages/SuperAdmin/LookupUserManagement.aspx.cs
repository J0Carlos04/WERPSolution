using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;

public partial class Pages_SuperAdmin_LookupUserManagement : PageBase
{    
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        StoreSelectedData();
        List<string> IdList = (List<string>)ViewState[CNT.VS.SelectedData];
        if (IdList.Count == 0)
        {
            U.ShowMessage($"{ViewState[CNT.VS.Table]} is required");
            return;
        }
        string Result = "";
        foreach (string Id in IdList)
            Result = $"{Result}{Id},";
        if (Result != "") Result = Result.Remove(Result.Length - 1, 1);

        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference(Result));
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvData.ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ($"{ViewState[CNT.VS.SelectedData]}".IsEmpty()) return;
            Literal ltrl_Id = (Literal)e.Row.FindControl("ltrl_Id");
            CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");
            List<string> IdList = (List<string>)ViewState[CNT.VS.SelectedData];
            if (IdList.Exists(a => a == ltrl_Id.Text))
                cb_IsChecked.Checked = true;

        }
    }
    protected void ucPaging_OpChanged(object sender, EventArgs e)
    {
        StoreSelectedData();

        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], ViewState[CNT.VS.Table].ToText());
        pList.Add(new clsParameter("DataType", "data"));
        #endregion

        double CurrentPage = 0;
        if (sender.ToString().Contains("LinkButton"))
            CurrentPage = Convert.ToDouble(((DropDownList)((LinkButton)sender).Parent.FindControl("ddlPage")).SelectedValue);
        else CurrentPage = Convert.ToDouble(((DropDownList)sender).SelectedValue);

        double StartNo = (CurrentPage * Convert.ToInt32(ups.SelectedValue)) - (Convert.ToInt32(ups.SelectedValue) - 1);
        double EndNo = (StartNo + (Convert.ToInt32(ups.SelectedValue) - 1));

        pList.Add(new clsParameter("StartNo", StartNo));
        pList.Add(new clsParameter("EndNo", EndNo));

        List<object> oList = oList = UserManagementLookup.GetByCriteria(pList, out double TotalRow);
        U.BindGrid(gvData, oList);
        lblRowCount.Text = string.Format("Page {0} of {1}, Total Record : {2}", CurrentPage, (double)ViewState[CNT.VS.TotalPage], (double)ViewState[CNT.VS.TotalRow]);

        ViewState[CNT.VS.StartNo] = StartNo;
    }
    protected void ups_Changed(object sender, EventArgs e)
    {
        ViewState[CNT.VS.StartNo] = null;
        SetInitControl();
        BindData();
    }
    protected void wSearch_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = Session[CNT.Session.Criteria].ToText().Split(new string[] { "~|" }, StringSplitOptions.None);
        Session[CNT.Session.Criteria] = null;
        ViewState[CNT.VS.Criteria] = arrArgument[0];
        ViewState[CNT.VS.SearchData] = arrArgument[1];
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=UserManagementLookup&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=UserManagementLookup&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSorting.OnClientClick += "return false;";
        BindData();
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=UserManagementLookup");
        btnSearch.OnClientClick += "return false;";
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=UserManagementLookup");
        btnSorting.OnClientClick += "return false;";
        ViewState[CNT.VS.Table] = U.Tbl;

        SetInitControl();
        BindData();
    }
    private void SetInitControl()
    {
        btnSearch.Hidden = false;
        btnSorting.Hidden = false;

        ups.Enabled = false;
        ucPaging.Visible = true;
    }
    private void Search()
    {
        #region ViewState        
        ViewState[CNT.VS.StartNo] = null;
        ViewState[CNT.VS.EndNo] = null;
        ViewState[CNT.VS.TotalRow] = null;
        ViewState[CNT.VS.TotalPage] = null;
        
        #endregion

        SetInitControl();

        BindData();
    }    
    private void BindData()
    {
        double StartNo = ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo];
        double EndNo = 0;
        double TotalRow = 0;

        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], ViewState[CNT.VS.Table].ToText());
        pList.Add(new clsParameter("StartNo", StartNo));
        if (ups.SelectedValue != "ALL")
        {
            EndNo = StartNo + (Convert.ToInt32(ups.SelectedValue) - 1);
            pList.Add(new clsParameter("EndNo", EndNo));
            pList.Add(new clsParameter("DataType", "data"));
        }
        else pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = UserManagementLookup.GetByCriteria(pList, out TotalRow);

        bool IsDataExist = true;
        if (oList.Count == 0) IsDataExist = PrepareEmptyData(oList);
        else ups.Enabled = true;

        U.BindGrid(gvData, oList);

        #region SetPagingControl
        if (TotalRow == 0) TotalRow = 1;
        double TotalPage = 1;
        if (ups.SelectedValue != "ALL") TotalPage = Convert.ToDouble(Math.Ceiling(Convert.ToDouble(TotalRow) / (Convert.ToInt32(ups.SelectedValue))));

        string sCurrentPage = ((DropDownList)ucPaging.FindControl("ddlPage")).SelectedValue;
        int CurrentPage = Convert.ToInt32(sCurrentPage == "" ? "1" : sCurrentPage);
        ucPaging.SetTotalRow(TotalPage, CurrentPage);
        lblRowCount.Text = string.Format("Page {0} of {1}, Total Record : {2}", CurrentPage, TotalPage, IsDataExist ? TotalRow : 0);
        #endregion

        ViewState[CNT.VS.StartNo] = StartNo;
        ViewState[CNT.VS.EndNo] = EndNo;
        ViewState[CNT.VS.TotalRow] = TotalRow;
        ViewState[CNT.VS.TotalPage] = TotalPage;
    }
    private bool PrepareEmptyData(List<object> oList)
    {
        oList.Add(new UserManagementLookup() { No = -1, Name = CNT.DataNotAvailable });
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
    }
    private void StoreSelectedData()
    {
        if (ViewState[CNT.VS.SelectedData] == null) ViewState[CNT.VS.SelectedData] = new List<string>();
        List<string> IdList = (List<string>)ViewState[CNT.VS.SelectedData];
        foreach (GridViewRow Row in gvData.Rows)
        {
            Literal ltrl_Id = (Literal)Row.FindControl("ltrl_Id");
            CheckBox cb_IsChecked = (CheckBox)Row.FindControl("cb_IsChecked");

            if (ltrl_Id.Text.ToInt() == 0) continue;
            if (cb_IsChecked.Checked && !IdList.Exists(a => a == ltrl_Id.Text))
                IdList.Add(ltrl_Id.Text);
        }
        ViewState[CNT.VS.SelectedData] = IdList;
    }
    #endregion
}