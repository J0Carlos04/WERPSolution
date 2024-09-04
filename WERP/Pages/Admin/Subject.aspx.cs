using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using System.Text;

public partial class Pages_Admin_Subject : PageBase
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
            DropDownList ddl_WorkDurationType = (DropDownList)e.Row.FindControl("ddl_WorkDurationType");
            DropDownList ddl_UseItem = (DropDownList)e.Row.FindControl("ddl_UseItem");
            DropDownList ddl_Active = (DropDownList)e.Row.FindControl("ddl_Active");

            List<object> oList = (List<object>)gvData.DataSource;
            Subject o = (Subject)oList[e.Row.RowIndex];
            ddl_WorkDurationType.SelectedValue = o.WorkDurationType;
            ddl_UseItem.SetValue(o.UseItem);
            cb_IsChecked.Enabled = oList.Exists(a => $"{((Subject)a).Mode}" != "") ? false : true;
            ddl_Active.SelectedValue = $"{o.Active}";
        }
    }
    protected void ucPaging_OpChanged(object sender, EventArgs e)
    {
        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], $"{ViewState[CNT.VS.Table]}");
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

        List<object> oList = oList = Subject.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Subject&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Subject&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
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
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Subject")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Subject")}return false;";
        ViewState[CNT.VS.Table] = "Subject";
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
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], $"{ViewState[CNT.VS.Table]}");
        pList.Add(new clsParameter("StartNo", StartNo));
        if (ups.SelectedValue != "ALL")
        {
            EndNo = StartNo + (Convert.ToInt32(ups.SelectedValue) - 1);
            pList.Add(new clsParameter("EndNo", EndNo));
            pList.Add(new clsParameter("DataType", "data"));
        }
        else pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = Subject.GetByCriteria(pList, out TotalRow);

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
        oList.Add(new Subject() { No = -1, Name = CNT.DataNotAvailable });
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
        List<object> oList = U.GetGridData(gvData, Subject.MyType).ListData.FindAll(a => ((Subject)a).Name != CNT.DataNotAvailable);
        oList.ForEach(a => { ((Subject)a).IsChecked = false; ((Subject)a).Mode = ""; });
        oList.Insert(0, new Subject() { No = 0, IsChecked = true, Mode = "add" });
        U.BindGrid(gvData, oList);
        ((CheckBox)gvData.HeaderRow.Cells[0].FindControl("cbCheckAll")).Enabled = false;
        #endregion        
    }
    private void SetInitEdit()
    {
        List<object> oList = U.GetGridData(gvData, Subject.MyType).ListData;
        if (!oList.Exists(a => ((Subject)a).IsChecked))
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

        oList.ForEach(a => { ((Subject)a).Mode = ((Subject)a).IsChecked ? "edit" : ""; });
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
            List<object> oList = U.GetGridData(gvData, typeof(Subject)).ListData;
            Subject o = (Subject)oList[0];

            #region Validation			
            Result = U.SingleRequiredValidation(o);
            if (Result != "")
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{Result}");
                return;
            }

            StringBuilder sb = new StringBuilder();
            int Seq = 0;
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Code", o.Code, o.Id)) sb.AppendLine($"{Seq.Inc()}. Subject with Code : {o.Code} already exist");
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. Subject with Name : {o.Name} already exist");
            if (o.WorkDuration != 0 && o.WorkDurationType.IsEmpty()) sb.AppendLine($"{Seq.Inc()}. Work Duration Type is required");
            if (!sb.ToText().IsEmpty())
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{sb.ToText()}");
                return;
            }
            #endregion

            o.CreatedBy = $"{ViewState[CNT.VS.Username]}";
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
        List<object> oList = U.GetGridData(gvData, typeof(Subject)).ListData;
        Result = U.MultipleRequiredValidation(oList);
        if (!Result.IsEmpty())
        {
            U.ShowMessage($"Save Failed,{Environment.NewLine} {Result}");
            return;
        }        

        int Seq = 0;
        StringBuilder sb = new StringBuilder();
        foreach (Subject o in oList.FindAll(a => ((Subject)a).IsChecked))
        {
            if (!o.IsChecked) continue;

            #region Validation      
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Code", o.Code, o.Id)) sb.AppendLine($"{Seq.Inc()}. Subject with Code : {o.Code} already exist");
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. Subject with Name : {o.Name} already exist");
            if (o.WorkDuration != 0 && o.WorkDurationType.IsEmpty()) sb.AppendLine($"{Seq.Inc()}. Work Duration Type is required");
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
            List<object> oList = U.GetGridData(gvData, Subject.MyType).ListData;
            if (oList.FindAll(a => ((Subject)a).IsChecked).Count == 0)
            {
                U.ShowMessage("Please select row that you want to delete");
                return;
            }

            for (int i = 0; i < oList.Count; i++)
            {
                Subject o = (Subject)oList[i];
                if (!o.IsChecked) continue;

                Result = DataAccess.IsDataExist(0, "SubjectId", o.Id);
                if (!Result.IsEmpty())
                {
                    U.ShowMessage($"Delete Failed, {Environment.NewLine}Data used in {Result}, please delete these data first");
                    return;
                }

                Result = Subject.Delete(o.Id);
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

        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], $"{ViewState[CNT.VS.Table]}");
        pList.Add(new clsParameter("StartNo", ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo]));
        pList.Add(new clsParameter("DataType", "all"));
        List<object> oList = Subject.GetByCriteria(pList, out double TotalRow);
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };

        #region HeaderExcel	
        w.SetExcel("No");
        w.SetExcel("Code");
        w.SetExcel("Name");
        w.SetExcel("Need Cust No");
        w.SetExcel("Use Item");
        w.SetExcel("Work Duration");
        w.SetExcel("Use NRW");
        w.SetExcel("Use DPD");
        w.SetExcel("Leak");
        w.SetExcel("Need Restoration");
        w.SetExcel("Need Closed");
        w.SetExcel("Use Time Limit");       
        w.SetExcel("Use Photo");
        w.SetExcel("Use Section");
        w.SetExcel("Update GIS");
        w.SetExcel("Active");
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (Subject o in oList)
        {
            w.Row += 1;
            w.SetExcel(o.No, true);
            w.SetExcel(o.Code);
            w.SetExcel(o.Name);
            w.SetExcel(o.NeedCustNo ? "Yes" : "No");
            w.SetExcel(o.UseItem);
            w.SetExcel(o.WorkDuration);
            w.SetExcel(o.UseNRW ? "Yes" : "No");
            w.SetExcel(o.UseDPD ? "Yes" : "No");
            w.SetExcel(o.Leak ? "Yes" : "No");
            w.SetExcel(o.NeedRestoration ? "Yes" : "No");
            w.SetExcel(o.NeedClosed ? "Yes" : "No");
            w.SetExcel(o.UseTimeLimit ? "Yes" : "No");           
            w.SetExcel(o.UsePhoto ? "Yes" : "No");
            w.SetExcel(o.UseSection ? "Yes" : "No");
            w.SetExcel(o.UpdateGIS ? "Yes" : "No");
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
        resp.AppendHeader("content-disposition", string.Format("attachment; filename=Subject.xlsx"));
        resp.AppendHeader("Content-Transfer-Encoding", "binary");
        resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        byte[] buffer = null;
        string tempFilename = $@"{U.PathTempFolder}Subject.xlsx";

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