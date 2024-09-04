using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using Newtonsoft.Json.Linq;
using System.Text;

public partial class Pages_Admin_Location : PageBase
{    
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
            case "btnAdd":
                SetInitAdd();
                break;
            case "btnAddSave":
                SaveAdd();
                break;

            case "btnEdit":
                SetInitEdit();
                break;
            case "btnEditSave":
                SaveEdit();
                break;

            case "btnAddCancel":
            case "btnEditCancel":
                Cancel();
                break;

            case "btnDelete":
                Delete();
                break;

            case "btnExportExcel":
                ExportExcel();
                break;
        }
    }    
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddl_Type":
                TypeChanged(ddl);
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
            CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");
            TextBox tb_Name = (TextBox)e.Row.FindControl("tb_Name");
            DropDownList ddl_AreaId = (DropDownList)e.Row.FindControl("ddl_AreaId");
            DropDownList ddl_Type = (DropDownList)e.Row.FindControl("ddl_Type");
            DropDownList ddl_FunctionStationId = (DropDownList)e.Row.FindControl("ddl_FunctionStationId");            
            DropDownList ddl_Active = (DropDownList)e.Row.FindControl("ddl_Active");

            List<object> oList = (List<object>)gvData.DataSource;
            Location o = (Location)oList[e.Row.RowIndex];

            U.SetDropDownTypeLocation(ddl_Type);
            ddl_Type.SetValue(o.Type);

            tb_Name.Visible = false;
            ddl_FunctionStationId.Visible = false;
            if (o.Type.Is("Customer"))
            {
                if (o.Mode.IsNotEmpty()) 
                    tb_Name.Visible = true;
            }
            else
            {
                if (o.Mode.IsNotEmpty()) ddl_FunctionStationId.Visible = true;
                U.SetDropDownFunctionStation(ddl_FunctionStationId, ddl_Type.SelectedValue);
                ddl_FunctionStationId.SetValue(o.FunctionStationId);
            }            
            
            U.SetDropDownMasterData(ddl_AreaId, "Area");
            ddl_AreaId.SelectedValue = o.AreaId.ToString();

            cb_IsChecked.Enabled = oList.Exists(a => $"{((Location)a).Mode}" != "") ? false : true;
            ddl_Active.SelectedValue = $"{o.Active}";
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

        List<object> oList = oList = Location.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Location&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Location&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSorting.OnClientClick += "return false;";
        BindData();
    }
    protected decimal NormalizeDecimal(object Value)
    {
        return Value.ToDecimal() / 1.000000000000000000000000000000000m;
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
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Location")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Location")}return false;";
        ViewState[CNT.VS.Table] = "Location";
    }
    private void SetInitControl()
    {
        btnSearch.Hidden = false;
        btnSorting.Hidden = false;

        btnAdd.Hidden = false;
        btnEdit.Hidden = false;
        btnDelete.Hidden = false;

        btnAddSave.Hidden = true;
        btnAddCancel.Hidden = true;
        btnEditSave.Hidden = true;
        btnEditCancel.Hidden = true;

        btnExportExcel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = true;
    }
    private void TypeChanged(DropDownList ddl)
    {
        DropDownList ddl_FunctionStationId = (DropDownList)ddl.FindControl("ddl_FunctionStationId");
        TextBox tb_Name = (TextBox)ddl.FindControl("tb_Name");
        ddl_FunctionStationId.Items.Clear();
        if (ddl.SelectedValue.IsEmpty()) return;
        tb_Name.Visible = false;
        ddl_FunctionStationId.Visible = false;
        if (ddl.SelectedValue.Is("Customer"))
            tb_Name.Visible = true;
        else
        {
            ddl_FunctionStationId.Visible = true;
            U.SetDropDownFunctionStation(ddl_FunctionStationId, ddl.SelectedValue);
        }
        
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
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("StartNo", StartNo));
        if (ups.SelectedValue != "ALL")
        {
            EndNo = StartNo + (Convert.ToInt32(ups.SelectedValue) - 1);
            pList.Add(new clsParameter("EndNo", EndNo));
            pList.Add(new clsParameter("DataType", "data"));
        }
        else pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = Location.GetByCriteria(pList, out TotalRow);

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
        oList.Add(new Location() { No = -1, Name = CNT.DataNotAvailable });
        btnDelete.Hidden = true;
        btnEdit.Hidden = true;
        btnExportExcel.Hidden = true;
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
    }
    private void SetInitAdd()
    {
        #region Set Visibility Control
        btnSearch.Hidden = true;
        btnSorting.Hidden = true;
        btnAdd.Hidden = true;
        btnEdit.Hidden = true;
        btnDelete.Hidden = true;
        btnEditSave.Hidden = true;
        btnEditCancel.Hidden = true;
        btnExportExcel.Hidden = true;
        ups.Enabled = false;
        ucPaging.Visible = false;

        btnAddSave.Hidden = false;
        btnAddCancel.Hidden = false;
        #endregion

        #region Add Empty Row        
        List<object> oList = U.GetGridData(gvData, Location.MyType).ListData.FindAll(a => ((Location)a).Name != CNT.DataNotAvailable);
        oList.ForEach(a => { ((Location)a).IsChecked = false; ((Location)a).Mode = ""; });
        oList.Insert(0, new Location() { No = 0, IsChecked = true, Mode = "add" });
        U.BindGrid(gvData, oList);
        ((CheckBox)gvData.HeaderRow.Cells[0].FindControl("cbCheckAll")).Enabled = false;
        #endregion        
    }
    private void SetInitEdit()
    {
        List<object> oList = U.GetGridData(gvData, Location.MyType).ListData;
        if (!oList.Exists(a => ((Location)a).IsChecked))
        {
            U.ShowMessage("Please select row that you want to edit");
            return;
        }

        #region Set Visibility Control
        btnSearch.Hidden = true;
        btnSorting.Hidden = true;
        btnAdd.Hidden = true;
        btnAddSave.Hidden = true;
        btnAddCancel.Hidden = true;
        btnEdit.Hidden = true;
        btnDelete.Hidden = true;
        btnExportExcel.Hidden = true;

        btnEditSave.Hidden = false;
        btnEditCancel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = false;
        #endregion                       

        oList.ForEach(a => { ((Location)a).Mode = ((Location)a).IsChecked ? "edit" : ""; });
        U.BindGrid(gvData, oList);
        ((CheckBox)gvData.HeaderRow.Cells[0].FindControl("cbCheckAll")).Enabled = false;
    }
    private void Cancel()
    {
        SetInitControl();
        BindData();
    }
    private void SaveAdd()
    {
        try
        {
            string Result = "";
            List<object> oList = U.GetGridData(gvData, Location.MyType).ListData;
            Location o = (Location)oList[0];
            if (o.Type.IsNot("Customer"))
            {
                FunctionStation fs = FunctionStation.GetById(o.FunctionStationId);
                o.StationId = fs.StationId;
                o.Name = fs.Station;
            }            

            #region Validation			
            Result = U.SingleRequiredValidation(o);
            if (Result != "")
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{Result}");
                return;
            }

            StringBuilder sb = new StringBuilder();
            int Seq = 0;
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. Location with Name : {o.Name} already exist");
            if (!sb.ToText().IsEmpty())
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{sb.ToText()}");
                return;
            }
            #endregion
            
            o.Active = true;
            Result = o.Insert();
            if (Result.Contains("Error Message"))
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{Result}");
                return;
            }
            o.Id = Convert.ToInt32(Result);

            SetInitControl();
            BindData();
        }
        catch (Exception ex) { U.ShowMessage($"Save Failed, {Environment.NewLine}{ex.Message}"); }
    }
    private void SaveEdit()
    {
        string Result = "";
        List<object> oList = U.GetGridData(gvData, Location.MyType).ListData;
        foreach (Location o in oList.FindAll(a => ((Location)a).IsChecked))
        {
            FunctionStation fs = FunctionStation.GetById(o.FunctionStationId);
            o.StationId = fs.StationId;
            o.Name = fs.Station;
        }
        Result = U.MultipleRequiredValidation(oList.FindAll(a => ((Location)a).IsChecked));
        if (!Result.IsEmpty())
        {
            U.ShowMessage($"Save Failed,{Environment.NewLine} {Result}");
            return;
        }

        int Seq = 0;
        StringBuilder sb = new StringBuilder();
        foreach (Location o in oList.FindAll(a => ((Location)a).IsChecked))
        {
            if (!o.IsChecked) continue;

            #region Validation            
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. Location with Name : {o.Name} already exist");
            if (!sb.ToText().IsEmpty()) continue;
            #endregion

            o.ModifiedBy = $"{ViewState[CNT.VS.Username]}";
            Result = o.Update();
            if (Result.Contains("Error Message"))
            {
                U.ShowMessage($"Save Failed,{Environment.NewLine} {Result}");
                return;
            }
            o.Mode = "";
            o.IsChecked = false;
        }
        if (!sb.ToText().IsEmpty())
        {
            U.ShowMessage($"Save Failed, {Environment.NewLine}{sb.ToText()}");
            U.BindGrid(gvData, oList);
            return;
        }

        U.BindGrid(gvData, oList);
        SetInitControl();
        
    }
    private void Delete()
    {
        try
        {
            string Result = "";
            List<object> oList = U.GetGridData(gvData, Location.MyType).ListData;
            if (oList.FindAll(a => ((Location)a).IsChecked).Count == 0)
            {
                U.ShowMessage("Please select row that you want to delete");
                return;
            }

            for (int i = 0; i < oList.Count; i++)
            {
                Location o = (Location)oList[i];
                if (!o.IsChecked) continue;

                Result = DataAccess.IsDataExist(0, "LocationId", o.Id);
                if (!Result.IsEmpty())
                {
                    U.ShowMessage($"Delete Failed, {Environment.NewLine}Data used in {Result}, please delete these data first");
                    return;
                }

                Result = Location.Delete(o.Id);
                if (Result.Contains("Error Message"))
                {
                    U.ShowMessage(string.Format("Delete Failed, \\n{0}", Result));
                    return;
                }
                oList.Remove(o);
                i -= 1;
            }

            double StartNo = (double)ViewState[CNT.VS.StartNo];
            if (oList.Count == 0 && StartNo != 1)
            {
                ViewState[CNT.VS.StartNo] = (StartNo - 10);
                DropDownList ddlPage = ((DropDownList)ucPaging.FindControl("ddlPage"));
                int value = Convert.ToInt32(ddlPage.SelectedValue);
                ((DropDownList)ucPaging.FindControl("ddlPage")).SelectedValue = (value - 1).ToString();
            }
            Search();
        }
        catch (Exception ex) { U.ShowMessage($"Delete Failed, {Environment.NewLine}{ex.Message}"); }
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

        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("StartNo", ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo]));
        pList.Add(new clsParameter("DataType", "all"));
        List<object> oList = Location.GetByCriteria(pList, out double TotalRow);
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };

        #region HeaderExcel	
        w.SetExcel("No");
        w.SetExcel("Area");
        w.SetExcel("Type");
        w.SetExcel("Name");
        w.SetExcel("RT");
        w.SetExcel("RW");
        w.SetExcel("UrbanVilage");
        w.SetExcel("SubDistrict");
        w.SetExcel("Longitude");
        w.SetExcel("Latitude");        
        w.SetExcel("Active");
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (Location o in oList)
        {
            w.Row += 1;
            w.SetExcel(o.No, true);
            w.SetExcel(o.Area);
            w.SetExcel(o.Type);
            w.SetExcel(o.Name);
            w.SetExcel(o.RT);
            w.SetExcel(o.RW);
            w.SetExcel(o.UrbanVilage);
            w.SetExcel(o.SubDistrict);
            w.SetExcel(o.Longitude);
            w.SetExcel(o.Latitude);            
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
        resp.AppendHeader("content-disposition", string.Format("attachment; filename=Location.xlsx"));
        resp.AppendHeader("Content-Transfer-Encoding", "binary");
        resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        byte[] buffer = null;
        string tempFilename = $@"{U.PathTempFolder}Location.xlsx";

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