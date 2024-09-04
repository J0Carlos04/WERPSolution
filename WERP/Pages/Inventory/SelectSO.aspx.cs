using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using Microsoft.Exchange.WebServices.Data;

public partial class Pages_Inventory_SelectSO : PageBase
{
    #region Fields   
    
    
    
    
    
    
    
    
    
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
            case "btnShowItem":
                ShowItems();
                break;
            case "btnHideItem":
                HideItems();
                break;            
        }
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        RadioButton rb = (RadioButton)btn.FindControl("rb");
        Literal ltrl_Id = (Literal)btn.FindControl("ltrl_Id");

        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference(ltrl_Id.Text));
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
            if (e.Row.RowState == DataControlRowState.Normal)
            {
                RadioButton rb = (RadioButton)e.Row.FindControl("rb");
                Button btnSelect = (Button)e.Row.FindControl("btnSelect");
                CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");
                Literal ltrl_Code = (Literal)e.Row.FindControl("ltrl_Code");
                rb.Attributes.Clear();
                rb.Attributes.Add("onclick", $"ClientChanged('{btnSelect.ClientID}');");

                if (ltrl_Code.Text == CNT.DataNotAvailable)
                {
                    rb.Visible = false;
                    cb_IsChecked.Visible = false;
                }
            }            
            else if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Visible = false;

                Literal ltrl_Id = (Literal)e.Row.FindControl("ltrl_Id");
                RadioButton rb = (RadioButton)e.Row.FindControl("rb");                                
                List<object> siList = StockOrderItem.GetByParendId(ltrl_Id.Text);
                GridView gvItem = (GridView)e.Row.FindControl("gvItem");
                U.BindGrid(gvItem, siList);                
            }
        }
    }
    protected void ucPaging_OpChanged(object sender, EventArgs e)
    {
        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
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

        List<object> oList = oList = PO.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=StockOrder&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=StockOrder&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSorting.OnClientClick += "return false;";
        BindData();
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        ViewState[CNT.VS.PageName] = "StockOrder";
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=StockOrder");
        btnSearch.OnClientClick += "return false;";
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=StockOrder");
        btnSorting.OnClientClick += "return false;";

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
        List<clsParameter> pList = U.GetParameter(SetCriteria(ViewState[CNT.VS.Criteria]), ViewState[CNT.VS.SortBy], "", true, U.ViewAllDataAccess(ViewState[CNT.VS.PageName]));
        pList.Add(new clsParameter("StartNo", StartNo));
        if (ups.SelectedValue != "ALL")
        {
            EndNo = StartNo + (Convert.ToInt32(ups.SelectedValue) - 1);
            pList.Add(new clsParameter("EndNo", EndNo));
            pList.Add(new clsParameter("DataType", "data"));
        }
        else pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = StockOrder.GetByCriteria(pList, out TotalRow);

        bool IsDataExist = true;
        if (oList.Count == 0) IsDataExist = PrepareEmptyData(oList);
        else ups.Enabled = true;

        oList = DuplicateData(oList);
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
    private object SetCriteria(object Criteria)
    {
        if (Criteria == null) Criteria = " Where ";
        else Criteria = $"{Criteria} and ";
        Criteria = $"{Criteria} (e.Status = 'Approved' or e.Status = 'Partial Approved') and e.PendingQty > 0 ";
        return Criteria;
    }
    private List<object> DuplicateData(List<object> oList)
    {
        List<object> Stocks = new List<object>();
        foreach (StockOrder o in oList)
        {
            Stocks.Add(o);
            Stocks.Add(o);
        }
        return Stocks;
    }
    private void ShowItems()
    {
        bool IsCheckExist = false;
        bool IsChecked = false;
        foreach (GridViewRow Row in gvData.Rows)
        {
            if (Row.RowState == DataControlRowState.Normal)
            {
                CheckBox cb_IsChecked = (CheckBox)Row.FindControl("cb_IsChecked");
                IsChecked = cb_IsChecked.Checked;
                if (IsChecked)
                {
                    IsCheckExist = true;
                    Row.Cells[0].RowSpan = 2;
                    Row.Cells[1].RowSpan = 2;
                    Row.Cells[2].RowSpan = 2;
                }
            }
            else if (Row.RowState == DataControlRowState.Alternate)
            {
                if (IsChecked)
                {
                    Row.Visible = true;
                    Row.Cells[0].Visible = false;
                    Row.Cells[1].Visible = false;
                    Row.Cells[2].Visible = false;
                    Row.Cells[3].ColumnSpan = Row.Cells.Count;
                    for (int i = 4; i < Row.Cells.Count; i++)
                        Row.Cells[i].Visible = false;
                }
            }
        }
        if (!IsCheckExist) U.ShowMessage("Please select row that you want see the Items");
    }
    private void HideItems()
    {
        bool IsCheckExist = false;
        bool IsChecked = false;
        foreach (GridViewRow Row in gvData.Rows)
        {
            if (Row.RowState == DataControlRowState.Normal)
            {
                CheckBox cb_IsChecked = (CheckBox)Row.FindControl("cb_IsChecked");
                IsChecked = cb_IsChecked.Checked;
                if (IsChecked)
                {
                    IsCheckExist = true;
                    Row.Cells[0].RowSpan = 1;
                    Row.Cells[1].RowSpan = 1;
                    Row.Cells[2].RowSpan = 1;
                }
            }
            else if (Row.RowState == DataControlRowState.Alternate)
            {
                if (IsChecked) Row.Visible = false;
            }
        }
        if (!IsCheckExist) U.ShowMessage("Please select row that you want see the Items");
    }
    private bool PrepareEmptyData(List<object> oList)
    {
        oList.Add(new StockOrder() { No = -1, Code = CNT.DataNotAvailable });
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
    }
    #endregion
}