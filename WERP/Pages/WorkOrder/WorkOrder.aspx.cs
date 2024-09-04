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

public partial class Pages_WorkOrder_WorkOrder : PageBase
{    
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
        else
        {
            if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
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
            case "btnExportExcel":
                ExportExcel();
                break;
        }
    }
    protected void lb_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        switch(lb.ID)
        {
            case "lbEdit":
                U.OpenNewTab($"EditWorkOrder{lb.CommandName}", $"Edit Work Order {lb.CommandName}", $"getEditWindowUrl('{lb.CommandArgument}')");
                break;
            case "lbStockOut":
                U.OpenNewTab($"ViewStockOutOrder{lb.CommandName}", $"View Stock Out Order {lb.CommandName}", $"getStockOutWindowUrl('{lb.CommandArgument}')");
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
            WorkOrder wo = (WorkOrder)e.Row.DataItem;
            LinkButton lbEdit = (LinkButton)e.Row.FindControl("lbEdit");
            LinkButton lbStockOut = (LinkButton)e.Row.FindControl("lbStockOut");

            if (!U.IsMember(CNT.Role.Operator)) lbEdit.Visible = U.UpdateAccess(ViewState[CNT.VS.PageName]);            
            
            if (wo.Status.Is(CNT.Status.StockOut) && U.ViewAccess("StockOut") && wo.StockOutId == 0) lbStockOut.Visible = true;
            else lbStockOut.Visible = false;

            Literal ltrl_Subject = (Literal)e.Row.FindControl("ltrl_Subject");
            if (ltrl_Subject.Text == CNT.DataNotAvailable)
            {
                lbEdit.Visible = false;
                lbStockOut.Visible = false;
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

        List<object> oList = oList = WorkOrder.GetByCriteria(pList, out double TotalRow);
        U.BindGrid(gvData, oList);
        lblRowCount.Text = string.Format("Page {0} of {1}, Total Record : {2}", CurrentPage, (double)ViewState[CNT.VS.TotalPage], (double)ViewState[CNT.VS.TotalRow]);

        ViewState[CNT.VS.StartNo] = StartNo;
    }
    protected void ups_Changed(object sender, EventArgs e)
    {
        ViewState[CNT.VS.StartNo] = null;
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
        ViewState[CNT.VS.PageName] = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
        if (!(U.ViewAccess(ViewState[CNT.VS.PageName]) || U.IsMember(CNT.Operator))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=WorkOrder")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=WorkOrder")}return false;";
    }
    private void SetInitControl()
    {
        btnSearch.Hidden = false;
        btnSorting.Hidden = false;        

        btnExportExcel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = true;
    }
    private object SetCriteria(object Criteria)
    {
        if (!U.ScheduleId.IsNull())
        {
            if (Criteria == null) Criteria = " Where ";
            else Criteria = $" {Criteria} and ";
            Criteria = $"{Criteria} a.ScheduleId = '{U.ScheduleId}' ";
        }
        else if (!U.ScheduleMaintenanceId.IsNull())
        {
            if (Criteria == null) Criteria = " Where ";
            else Criteria = $" {Criteria} and ";
            Criteria = $"{Criteria} a.ScheduleMaintenanceId = '{U.ScheduleMaintenanceId}' ";
        }
        else if (!U.HelpdeskId.IsNull())
        {
            if (Criteria == null) Criteria = " Where ";
            else Criteria = $" {Criteria} and ";
            Criteria = $"{Criteria} a.HelpdeskId = '{U.HelpdeskId}' ";
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
        oList.Add(new WorkOrder() { No = -1, Subject = CNT.DataNotAvailable });
        btnExportExcel.Hidden = true;
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
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
        List<object> oList = WorkOrder.GetByCriteria(pList, out double TotalRow);
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };

        #region HeaderExcel	
        w.SetExcel("No");
        w.SetExcel("Code");
        w.SetExcel("Order Date");
        w.SetExcel("Earliest Start Date");
        w.SetExcel("Latest Start Date");
        w.SetExcel("Maintenance Start Date");
        w.SetExcel("Operator");
        w.SetExcel("Vendor");
        w.SetExcel("Conducted By");
        w.SetExcel("Status");
        w.SetExcel("Result");
        w.SetExcel("Remarks");
        w.SetExcel("Maintenance Close Date");
        w.SetExcel("Achievement");
        w.SetExcel("Subject");
        w.SetExcel("Work Description");
        w.SetExcel("Area");
        w.SetExcel("Location");
        w.SetExcel("Category");
        w.SetExcel("Start Date");
        w.SetExcel("Period");
        w.SetExcel("End Date");        
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (WorkOrder o in oList)
        {
            w.Row += 1;
            w.SetExcel(o.No, true);
            w.SetExcel(o.Code);
            w.SetExcel(o.OrderDate);
            w.SetExcel(o.EarliestStartDate);
            w.SetExcel(o.LatestStartDate);
            w.SetExcel(o.StartDate);
            w.SetExcel(o.OperatorType);
            w.SetExcel(o.Vendor);
            w.SetExcel(o.ConductedBy);
            w.SetExcel(o.Status);
            w.SetExcel(o.Result);
            w.SetExcel(o.Remarks);
            //w.SetExcel(o.MaintenanceCloseDate);
            w.SetExcel(o.Achievement);
            w.SetExcel(o.Subject);
            w.SetExcel(o.WorkDescription);
            w.SetExcel(o.Area);
            w.SetExcel(o.Location);
            w.SetExcel(o.Category);
            w.SetExcel(o.StartDate);
            w.SetExcel($"{o.Period} - {o.PeriodType}");
            //w.SetExcel(o.EndDate);
            w.SetExcel(o.Active);
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
        resp.AppendHeader("content-disposition", string.Format("attachment; filename=MaintenanceSchedule.xlsx"));
        resp.AppendHeader("Content-Transfer-Encoding", "binary");
        resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        byte[] buffer = null;
        string tempFilename = $@"{U.PathTempFolder}MaintenanceSchedule.xlsx";

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