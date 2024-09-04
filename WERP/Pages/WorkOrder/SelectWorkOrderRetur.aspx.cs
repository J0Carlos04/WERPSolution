using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using DAL;
using System.Linq;

public partial class Pages_WorkOrder_SelectWorkOrderRetur : System.Web.UI.Page
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
        Literal ltrl_StockOutId = (Literal)btn.FindControl("ltrl_StockOutId");
        Literal ltrl_Code = (Literal)btn.FindControl("ltrl_Code");

        if (ltrl_Id.Text.ToInt().IsZero().ShowError("Work Order is required"))
            return;

        Session[CNT.Session.Wrapping] = new Wrapping { Id = ltrl_Id.Text.ToInt(), KeyId = ltrl_StockOutId.Text.ToInt(), Seq = U.QSSeq.ToInt(), Code = ltrl_Code.Text };
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
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], "", true, U.ViewAllDataAccess(ViewState[CNT.VS.PageName]));
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

        List<object> oList = oList = WorkOrder.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=WorkOrder&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=WorkOrder&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
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
        SetInit();
        SetInitControl();
        BindData();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        ViewState[CNT.VS.PageName] = "WorkOrder";
        if (!(U.ViewAccess(ViewState[CNT.VS.PageName]) || U.IsMember(CNT.Operator))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=WorkOrder")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=WorkOrder")}return false;";
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
        if (U.QSWorkType.Is("Retur"))
            Criteria = $"{Criteria} a.Status = 'InProgress' and a.Id in (SELECT Distinct WorkOrderID FROM WorkOrderWorkUpdate WHERE WorkType = 'Retur')";
        else
        {
            Criteria = $"{Criteria} (a.UseItem = 'Yes' or a.UseItem = 'Both') and a.Status = 'InProgress' and (Select SUM(QTY) from WorkOrderItem where WorkOrderId = a.Id) <> (Select SUM(QTY) from WorkOrderWorkUpdateItem where WorkOrderWorkUpdateId IN (Select Id from WorkOrderWorkUpdate where WorkOrderId = a.Id))";
            if (U.QSStockOutId.IsNotEmpty())
            {
                Criteria = $"{Criteria} and a.StockOutId = '{U.QSStockOutId}' ";
                if (U.QSStockOutItemId.IsNotEmpty())
                {
                    string ExcludeWorkOrder = GetExcludeWorkOrderId();
                    if (ExcludeWorkOrder.IsNotEmpty())
                        Criteria = $"{Criteria} and a.Id in ({ExcludeWorkOrder})";
                }
            }
        }                

        return Criteria;
    }
    private string GetExcludeWorkOrderId()
    {
        string Result = "";
        StockOutItem soi = StockOutItem.GetById(U.QSStockOutItemId);
        List<object> woList = WorkOrder.GetByStockOutId(U.QSStockOutId);
        foreach (WorkOrder wo in woList)
        {
            List<object> woiList = WorkOrderItem.GetByWorkOrderItem(wo.Id, soi.ItemId);
            List<object> wowuiList = WorkOrderWorkUpdateItem.GetByWorkOrderId(wo.Id);
            foreach (WorkOrderItem woi in woiList)
            {
                int TotalItem = wowuiList.Where(b => ((WorkOrderWorkUpdateItem)b).ItemId.Is(woi.ItemId)).Sum(c => ((WorkOrderWorkUpdateItem)c).Qty);
                int TotalRetur = StockOutRetur.GetTotalQtyByWorkOrderItem(wo.Id, woi.ItemId);

                int UnusedQty = woi.Qty - TotalItem;
                if (UnusedQty > 0)
                {
                    if ((TotalItem + TotalRetur) < woi.Qty)
                    {
                        Result = $"{Result}{wo.Id},";
                        break;
                    }
                }
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

        List<object> oList = WorkOrder.GetByCriteria(pList, out TotalRow);

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
        oList.Add(new WorkOrder() { No = -1, Code = CNT.DataNotAvailable });
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
    }
    #endregion
}