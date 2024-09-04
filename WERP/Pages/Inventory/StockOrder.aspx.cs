﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using System.Web.UI.HtmlControls;

public partial class Pages_Inventory_StockOrder : PageBase
{    
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
        else
        {
            var args = GetRequestEventArgument();
            if (args.StartsWith("UpdatePage$"))
            {
                string param1 = args.Substring("UpdatePage$".Length);
                if (param1 == "Search") BindData();
            }            
        }
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
            case "btnExportExcel":
                ExportExcel();
                break;
        }
    }
    protected void lb_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        switch (lb.ID)
        {
            case "lbEdit":
                U.OpenNewTab("EditStockOrder", "Edit Stock Order", $"getEditWindowUrl('{lb.CommandArgument}')");
                break;
            case "lbApproval":
                U.OpenNewTab("StockOrderApproval", "Stock Order Approval", $"getApprovalWindowUrl('{lb.CommandArgument}')");
                break;
            case "lbReception":
                U.OpenNewTab("StockOrderReceived", "Stock Order Reception", $"getReceivedWindowUrl('{lb.CommandArgument}')");
                break;
        }
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
            Literal ltrl_Id = (Literal)e.Row.FindControl("ltrl_Id");
            if (e.Row.RowState == DataControlRowState.Normal)
            {
                #region Approval
                Literal ltrl_ApproverUserId = (Literal)e.Row.FindControl("ltrl_ApproverUserId");
                Literal ltrl_Status = (Literal)e.Row.FindControl("ltrl_Status");
                LinkButton lbApproval = (LinkButton)e.Row.FindControl("lbApproval");
                if (ltrl_ApproverUserId.Text != $"{ViewState[CNT.VS.UsersId]}" || ltrl_Status.Text == "Approved") lbApproval.Visible = false;
                #endregion

                #region Edit 
                LinkButton lbEdit = (LinkButton)e.Row.FindControl("lbEdit");
                bool IsReceivedExist = StockReceived.IsReceivedExist(ltrl_Id.Text);
                if (IsReceivedExist) lbEdit.Visible = false;
                else lbEdit.Visible = U.UpdateAccess(ViewState[CNT.VS.PageName]);
                #endregion

                #region Stock Received              
                LinkButton lbReception = (LinkButton)e.Row.FindControl("lbReception");
                lbReception.Visible = StockOrderItem.IsPendingQtyExist(ltrl_Id.Text);
                #endregion                

                Literal ltrl_Code = (Literal)e.Row.FindControl("ltrl_Code");
                if (ltrl_Code.Text == CNT.DataNotAvailable)
                {
                    lbEdit.Visible = false;
                    lbApproval.Visible = false;
                    lbReception.Visible = false;
                }
            }
            else if (e.Row.RowState == DataControlRowState.Alternate)
            {                
                e.Row.Visible = false;
                
                List<object> siList = StockOrderItem.GetByParendId(ltrl_Id.Text);
                GridView gvItem = (GridView)e.Row.FindControl("gvItem");
                U.BindGrid(gvItem, siList);
            }
        }
    }
    protected void ucPaging_OpChanged(object sender, EventArgs e)
    {
        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(SetCriteria(ViewState[CNT.VS.Criteria]), ViewState[CNT.VS.SortBy], "", true, U.ViewAllDataAccess(ViewState[CNT.VS.PageName]));
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

        List<object> oList = oList = StockOrder.GetByCriteria(pList, out double TotalRow);
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
    protected string GetPOUrl(object id)
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", "ViewPO");
        joBuilder.AddProperty("title", "View PO");
        joBuilder.AddProperty("iframeUrl", $"getPOWindowUrl('{id}')", true);
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
        SetInitAdd();
        BindData();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        ViewState[CNT.VS.PageName] = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
        if (!(U.ViewAccess(ViewState[CNT.VS.PageName]) || U.IsMember(CNT.Approval))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=StockOrder")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=StockOrder")}return false;";
        ViewState[CNT.VS.UsersId] = Users.GetByUsername(U.GetUsername()).Id;
    }
    private void SetInitControl()
    {
        btnSearch.Hidden = false;
        btnSorting.Hidden = false;

        btnAdd.Hidden = !U.CreateAccess(ViewState[CNT.VS.PageName]);

        btnExportExcel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = true;
    }
    private void SetInitAdd()
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", "NewStockOrder");
        joBuilder.AddProperty("title", "New Stock Order");
        joBuilder.AddProperty("iframeUrl", "getNewWindowUrl()", true);
        joBuilder.AddProperty("refreshWhenExist", true);
        joBuilder.AddProperty("iconFont", "plus");
        btnAdd.OnClientClick = String.Format("parent.addExampleTab({0});", joBuilder);
    }
    private object SetCriteria(object Criteria)
    {
        if (U.View != null)
        {
            if (Criteria == null) Criteria = " Where ";
            else Criteria = $" {Criteria} and ";
            if ($"{U.View}" == "Pending") Criteria = $"{Criteria} (e.Status = 'Approved' or e.Status = 'Partial Approved') and e.PendingQty > 0 ";
            else if ($"{U.View}" == "Approved") Criteria = $"{Criteria} a.Status like '%Approved' and a.ApproverUserId = '{ViewState[CNT.VS.UsersId]}' ";
            else if (U.View.ToText() == "Rejected") Criteria = $"{Criteria} a.Status = '{U.View}' and a.ApproverUserId = '{ViewState[CNT.VS.UsersId]}' ";
            else Criteria = $"{Criteria} (a.Status = '{U.View}' or a.Status = 'Partial Approved') and a.ApproverUserId = '{ViewState[CNT.VS.UsersId]}' ";
        }
        return Criteria;
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
    private bool PrepareEmptyData(List<object> oList)
    {
        oList.Add(new StockOrder() { No = -1, Code = CNT.DataNotAvailable });
        btnExportExcel.Hidden = true;
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
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
    private void ExportExcel()
    {
        #region Excel Style
        SpreadsheetInfo.SetLicense("E43Y-EJC3-221R-AA38");
        ExcelFile excel = new ExcelFile();
        ExcelWorksheet sheet = null;
        CellStyle HeaderStyle = new CellStyle(excel);
        CellStyle HeaderTagLineStyle = new CellStyle(excel);
        CellStyle ItemStyleOdd = new CellStyle(excel);
        CellStyle ItemStyleEven = new CellStyle(excel);

        HeaderStyle.Borders.SetBorders(MultipleBorders.Vertical, System.Drawing.Color.Black, LineStyle.Thin);
        HeaderStyle.Borders.SetBorders(MultipleBorders.Horizontal, System.Drawing.Color.Black, LineStyle.Thin);
        HeaderStyle.Font.Name = "Calibri";
        HeaderStyle.Font.Size = 220;
        HeaderStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        HeaderStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
        HeaderStyle.FillPattern.SetSolid(System.Drawing.Color.FromArgb(169, 219, 128));

        ItemStyleOdd.Borders.SetBorders(MultipleBorders.Vertical, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleOdd.Borders.SetBorders(MultipleBorders.Horizontal, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleOdd.Font.Name = "Calibri";
        ItemStyleOdd.Font.Size = 220;
        ItemStyleOdd.HorizontalAlignment = HorizontalAlignmentStyle.Left;
        ItemStyleOdd.VerticalAlignment = VerticalAlignmentStyle.Center;

        ItemStyleEven.Borders.SetBorders(MultipleBorders.Vertical, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleEven.Borders.SetBorders(MultipleBorders.Horizontal, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleEven.Font.Name = "Calibri";
        ItemStyleEven.Font.Size = 220;
        ItemStyleEven.HorizontalAlignment = HorizontalAlignmentStyle.Left;
        ItemStyleEven.VerticalAlignment = VerticalAlignmentStyle.Center;
        #endregion

        sheet = excel.Worksheets.Add("Data");

        List<clsParameter> pList = U.GetParameter(SetCriteria(ViewState[CNT.VS.Criteria]), ViewState[CNT.VS.SortBy], "", true, U.ViewAllDataAccess(ViewState[CNT.VS.PageName]));
        pList.Add(new clsParameter("StartNo", ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo]));
        pList.Add(new clsParameter("DataType", "all"));
        List<object> oList = StockOrder.GetByCriteria(pList, out double TotalRow);
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };

        #region HeaderExcel	
        w.SetExcel("No");
        w.SetExcel("Code");
        w.SetExcel("Description");
        w.SetExcel("Requester");
        w.SetExcel("Procurement Type");        
        w.SetExcel("Vendor");
        w.SetExcel("PO No");
        w.SetExcel("PO Date");        
        w.SetExcel("Status");
        w.SetExcel("Approver");
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (StockOrder o in oList)
        {
            w.Row += 1;
            w.SetExcel(o.No, true);
            w.SetExcel(o.Code);
            w.SetExcel(o.Description);
            w.SetExcel(o.Requester);
            w.SetExcel(o.ProcurementType);            
            w.SetExcel(o.Vendor);
            w.SetExcel(o.PONo);
            w.SetExcel(o.PODate);            
            w.SetExcel(o.Status);
            w.SetExcel(o.Approver);
            w.Column = 0;
        }
        #endregion

        #region AutoFitText
        int columnCount = sheet.CalculateMaxUsedColumns();
        for (int j = 0; j < columnCount; j++)
        {
            sheet.Columns[j].AutoFitAdvanced(1.3f, sheet.Rows[0], sheet.Rows[sheet.Rows.Count - 1]);
            if (sheet.Columns[j].Width >= 30000) sheet.Columns[j].Width = 30000;
        }
        #endregion

        #region SaveExcel
        HttpResponse resp = Context.Response;
        resp.Clear();
        resp.Buffer = false;
        resp.AppendHeader("content-disposition", string.Format("attachment; filename=Items.xlsx"));
        resp.AppendHeader("Content-Transfer-Encoding", "binary");
        resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        byte[] buffer = null;        
        string tempFilename = $@"{U.PathTempFolder}Items.xlsx";

        if (File.Exists(tempFilename)) File.Delete(tempFilename);
        excel.SaveXlsx(tempFilename);


        using (FileStream fs = new FileStream(tempFilename, FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
        }
        if (File.Exists(tempFilename))
            File.Delete(tempFilename);

        resp.BinaryWrite(buffer);
        resp.End();
        #endregion
    }

    #endregion
}