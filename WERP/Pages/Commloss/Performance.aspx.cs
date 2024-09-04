using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using DAL;
using GemBox.Spreadsheet;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using Microsoft.Exchange.WebServices.Data;
using System.Drawing;
using static System.Collections.Specialized.BitVector32;
using System.Web.Script.Serialization;
using System.Activities.Statements;

public partial class Pages_Commloss_Performance : System.Web.UI.Page
{
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) Initialize();
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
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbEdit = (LinkButton)e.Row.FindControl("lbEdit");
            lbEdit.Visible = U.UpdateAccess(ViewState[CNT.VS.PageName]);

            LinkButton lbOrder = (LinkButton)e.Row.FindControl("lbOrder");
            lbOrder.Visible = U.ViewAccess("WorkOrder");

            Literal ltrl_WorkOrderType = (Literal)e.Row.FindControl("ltrl_WorkOrderType");
            if (ltrl_WorkOrderType.Text == CNT.DataNotAvailable)
            {
                lbEdit.Visible = false;
                lbOrder.Visible = false;
            }
        }
    }
    protected void ucPaging_OpChanged(object sender, EventArgs e)
    {
        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("DataType", "data"));
        pList.Add(new clsParameter("Month", U.QSMonth));
        pList.Add(new clsParameter("Year", U.QSYear));
        #endregion

        double CurrentPage = 0;
        if (sender.ToString().Contains("LinkButton"))
            CurrentPage = Convert.ToDouble(((DropDownList)((LinkButton)sender).Parent.FindControl("ddlPage")).SelectedValue);
        else CurrentPage = Convert.ToDouble(((DropDownList)sender).SelectedValue);

        double StartNo = (CurrentPage * Convert.ToInt32(ups.SelectedValue)) - (Convert.ToInt32(ups.SelectedValue) - 1);
        double EndNo = (StartNo + (Convert.ToInt32(ups.SelectedValue) - 1));
        
        pList.Add(new clsParameter("StartNo", StartNo));
        pList.Add(new clsParameter("EndNo", EndNo));

        List<object> oList = oList = StationMetric.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=StationMetric&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=StationMetric&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSorting.OnClientClick += "return false;";
        BindData();
    }
    #endregion

    #region Methods
    private void Initialize()
    {
        InitControl();
        Search();
    }
    private void InitControl()
    {

    }    
    private void Search()
    {
        #region ViewState        
        ViewState[CNT.VS.StartNo] = null;
        ViewState[CNT.VS.EndNo] = null;
        ViewState[CNT.VS.TotalRow] = null;
        ViewState[CNT.VS.TotalPage] = null;

        #endregion

        InitControl();

        BindData();
        ShowChart();
    }
    private void BindData()
    {
        double StartNo = ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo];
        double EndNo = 0;
        double TotalRow = 0;

        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("Month", U.QSMonth));
        pList.Add(new clsParameter("Year", U.QSYear));
        pList.Add(new clsParameter("StartNo", StartNo));
        if (ups.SelectedValue != "ALL")
        {
            EndNo = StartNo + (Convert.ToInt32(ups.SelectedValue) - 1);
            pList.Add(new clsParameter("EndNo", EndNo));
            pList.Add(new clsParameter("DataType", "data"));
        }
        else pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = StationMetric.GetByCriteria(pList, out TotalRow);

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
    private void ShowChart()
    {
        #region Chart Configuration         
        cLosses.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;
        cLosses.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new System.Drawing.Font("Poppins", 6f);
        cLosses.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Poppins", 6f);
        cLosses.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        cLosses.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "N0"; // Format Y-axis values as number with comma separators
        cLosses.Palette = ChartColorPalette.Pastel;  // Set a different palette if desired
        #endregion

        // Fetch data from your data access layer
        List<object> oList = StationMetric.GetByYear(U.QSYear);
        // Get distinct station names
        List<string> stations = oList.Select(o => ((StationMetric)o).StationName).Distinct().ToList();
        int MaxMonth = oList.Max(a => ((StationMetric)a).MonthIssuedDate);

        // Clear existing series and points
        //cLosses.Series.Clear();
        //cLosses.ChartAreas["ChartArea1"].AxisX.CustomLabels.Clear();

        List<Series> sList = new List<Series>();
        for (int i = 1; i <= MaxMonth; i++)
        {
            Series s = new Series();
            s.ChartType = SeriesChartType.StackedColumn;
            s.IsValueShownAsLabel = true;
            //s.Name = U.GetShortMonth(i);
            s.IsVisibleInLegend = true;
            //s.ToolTip = "station test";  // Enable tooltip for series
            sList.Add(s);
        }
        foreach (string station in stations)
        {
            List<object> smlList = oList.FindAll(a => ((StationMetric)a).StationName == station);
            if (smlList.Count == 0) continue;
            int i = -1;
            foreach (StationMetric sm in smlList.OrderBy(a => ((StationMetric)a).RevenueSaved))
            {
                i++;
                DataPoint dp = new DataPoint();
                dp.ToolTip = $"Station";
                //dp.SetValueXY(1, sm.RevenueSaved);
                //dp.SetValueXY(2, sm.RevenueSaved);
                //dp.AxisLabel = station;
                //dp.Font = new System.Drawing.Font("Poppins", 5f);
                //dp.LabelFormat = "#,##0";
                sList[i].Points.Add(dp);
                break;
            }
            break;
        }
        foreach (Series s in sList)
        {
            if (s.Points.Count == 0) continue;
            cLosses.Series.Add(s);
        }
    }




    private bool PrepareEmptyData(List<object> oList)
    {
        oList.Add(new StationMetric() {StationName = CNT.DataNotAvailable });
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

        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], "", true, U.ViewAllDataAccess(ViewState[CNT.VS.PageName]));
        pList.Add(new clsParameter("StartNo", ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo]));
        pList.Add(new clsParameter("DataType", "all"));
        List<object> oList = StationMetric.GetByCriteria(pList, out double TotalRow);
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };

        #region HeaderExcel	
        w.SetExcel("DMA");
        w.SetExcel("Revenue Saved");
        w.SetExcel("Volume Saved");
        w.SetExcel("Finished Issued");        
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (StationMetric o in oList)
        {
            w.Row += 1;            
            w.SetExcel(o.StationName);
            w.SetExcel(o.RevenueSaved, true);
            w.SetExcel(o.VolumeSaved, true);            
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