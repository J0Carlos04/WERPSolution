using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using System.Linq;
using DAL;

public partial class Pages_Inventory_SelectStockOutItem : PageBase
{
    #region Fields   
    
    
    
    
    
    
    
    
    
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        RadioButton rb = (RadioButton)btn.FindControl("rb");
        Literal ltrl_Id = (Literal)btn.FindControl("ltrl_Id");

        if (ltrl_Id.Text.ToInt().IsZero().ShowError("Item is required"))
            return;

        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference(ltrl_Id.Text));
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {

        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        GridViewRow Row = (GridViewRow)btn.Parent.Parent;
        LinkButton lb_FileName = (LinkButton)Row.FindControl("lb_FileName");
        PO p = PO.GetById(lb_FileName.CommandArgument);
        U.OpenFile(lb_FileName.Text, U.GetContentType(lb_FileName.Text), p.FileData);
    }
    protected void lb_Click(object sender, EventArgs e)
    {
        F.LinkButton lb = (F.LinkButton)sender;
        switch (lb.ID)
        {
            case "lb_POFileName":
                U.OpenFile(lb.Text, U.GetContentType(lb.Text), Convert.FromBase64String(((Label)lb.Parent.Parent.FindControl("lbl_DataText")).Text));
                break;
        }
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadioButton rb = (RadioButton)e.Row.FindControl("rb");
            Button btnSelect = (Button)e.Row.FindControl("btnSelect");
            rb.Attributes.Clear();
            rb.Attributes.Add("onclick", $"ClientChanged('{btnSelect.ClientID}');");

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

        List<object> oList = oList = StockOutItem.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=StockOutItem&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=StockOutItem&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSorting.OnClientClick += "return false;";
        BindData();
    }
    protected string GetItemUrl(object id)
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", "ViewItem");
        joBuilder.AddProperty("title", "View Item");
        joBuilder.AddProperty("iframeUrl", $"getItemWindowUrl('{id}')", true);
        joBuilder.AddProperty("refreshWhenExist", true);
        joBuilder.AddProperty("iconFont", "pencil");
        return String.Format("parent.addExampleTab({0});", joBuilder);
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=StockOutItem");
        btnSearch.OnClientClick += "return false;";
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=StockOutItem");
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
    private object SetCriteria(object Criteria)
    {
        if (Criteria == null) Criteria = " Where ";
        else Criteria = $"{Criteria} and ";
        Criteria = $"{Criteria} ISNULL(a.UnusedQty, 0) <> 0 ";
        if (U.QSStockOutId.IsNotEmpty())
            Criteria = $"{Criteria} and a.StockOutId = '{U.QSStockOutId}' ";
        if (U.QSWorkOrderId.IsNotEmpty())
            Criteria = $"{Criteria} and a.ItemId In ({GetItemId()}) ";
        return Criteria;
    }
    private string GetItemId()
    {
        string Result = "";
        List<object> woiList = WorkOrderItem.GetByWorkOrderId(U.QSWorkOrderId);
        List<object> wowuiList = WorkOrderWorkUpdateItem.GetByWorkOrderId(U.QSWorkOrderId);
        foreach (WorkOrderItem woi in woiList)
        {
            int TotalItem = wowuiList.Where(b => ((WorkOrderWorkUpdateItem)b).ItemId.Is(woi.ItemId)).Sum(c => ((WorkOrderWorkUpdateItem)c).Qty);
            int TotalRetur = StockOutRetur.GetTotalQtyByWorkOrderItem(U.QSWorkOrderId, woi.ItemId);

            int UnusedQty = woi.Qty - TotalItem;
            if (UnusedQty > 0)
            {
                if ((TotalItem + TotalRetur) < woi.Qty)
                    Result = $"{Result}{woi.ItemId},";
            }
        }
        if (Result.IsNotEmpty()) Result = Result.Remove(Result.Length - 1, 1);
        return Result;
    }
    private void BindData()
    {
        double StartNo = ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo];
        double EndNo = 0;
        double TotalRow = 0;

        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(SetCriteria(ViewState[CNT.VS.Criteria]), ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("StartNo", StartNo));
        if (ups.SelectedValue != "ALL")
        {
            EndNo = StartNo + (Convert.ToInt32(ups.SelectedValue) - 1);
            pList.Add(new clsParameter("EndNo", EndNo));
            pList.Add(new clsParameter("DataType", "data"));
        }
        else pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = StockOutItem.GetByCriteria(pList, out TotalRow);

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
        oList.Add(new StockOutItem() { No = -1, ItemCode = CNT.DataNotAvailable });
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
    }
    #endregion
}