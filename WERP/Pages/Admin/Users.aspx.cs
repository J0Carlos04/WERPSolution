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

public partial class Pages_Admin_Users : PageBase
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

            case "btnResetPassword":
                ResetPassword();
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
            DropDownList ddl_DepartmentId = (DropDownList)e.Row.FindControl("ddl_DepartmentId");
            DropDownList ddl_Active = (DropDownList)e.Row.FindControl("ddl_Active");

            List<object> oList = (List<object>)gvData.DataSource;
            Users o = (Users)oList[e.Row.RowIndex];

            U.SetDropDownMasterData(ddl_DepartmentId, "Department");
            ddl_DepartmentId.SetValue(o.DepartmentId);

            cb_IsChecked.Enabled = oList.Exists(a => $"{((Users)a).Mode}" != "") ? false : true;
            ddl_Active.SetValue(o.Active);
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

        List<object> oList = oList = Users.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Users&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Users&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSorting.OnClientClick += "return false;";
        BindData();
    }
    protected string GetEditUrl(object id)
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", "ItemInput");
        joBuilder.AddProperty("title", "Edit Item");
        joBuilder.AddProperty("iframeUrl", $"getEditWindowUrl('{id}')", true);
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
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Users")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Users")}return false;";
        ViewState[CNT.VS.Table] = "Users";
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

        btnResetPassword.Hidden = false;
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

        List<object> oList = Users.GetByCriteria(pList, out TotalRow);

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
        oList.Add(new Users() { No = -1, Name = CNT.DataNotAvailable });
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
        btnResetPassword.Hidden = true;
        btnExportExcel.Hidden = true;
        ups.Enabled = false;
        ucPaging.Visible = false;

        btnAddSave.Hidden = false;
        btnAddCancel.Hidden = false;
        #endregion

        #region Add Empty Row        
        List<object> oList = U.GetGridData(gvData, Users.MyType).ListData.FindAll(a => ((Users)a).Name != CNT.DataNotAvailable);
        oList.ForEach(a => { ((Users)a).IsChecked = false; ((Users)a).Mode = ""; });
        oList.Insert(0, new Users() { No = 0, IsChecked = true, Mode = "add" });
        U.BindGrid(gvData, oList);
        ((CheckBox)gvData.HeaderRow.Cells[0].FindControl("cbCheckAll")).Enabled = false;
        #endregion        
    }
    private void SetInitEdit()
    {
        List<object> oList = U.GetGridData(gvData, Users.MyType).ListData;
        if (!oList.Exists(a => ((Users)a).IsChecked))
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
        btnResetPassword.Hidden = true;
        btnExportExcel.Hidden = true;

        btnEditSave.Hidden = false;
        btnEditCancel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = false;
        #endregion                       

        oList.ForEach(a => { ((Users)a).Mode = ((Users)a).IsChecked ? "edit" : ""; });
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
            List<object> oList = U.GetGridData(gvData, typeof(Users)).ListData;
            Users o = (Users)oList[0];

            #region Validation	            
            Result = U.SingleRequiredValidation(o);
            if (Result != "")
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{Result}");
                return;
            }

            StringBuilder sb = new StringBuilder();
            int Seq = 0;
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. User with Name : {o.Name} already exist");            
            if (o.DepartmentId == 0) sb.AppendLine($"{Seq.Inc()}. Department is required");
            if (!U.IsValidEmail(o.Email)) sb.AppendLine("Invalid Email Address");
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Username", o.Username, o.Id)) sb.AppendLine($"{Seq.Inc()}. User with Username : {o.Username} already exist");
            if (DataAccess.IsFieldExist("Operators", "Username", o.Username, 0)) sb.AppendLine($"{Seq.Inc()}. Operator with Username : {o.Username} already exist");
            if (Users.IsUsernameExist(o.Username)) sb.AppendLine($"Username : {o.Username} already exist");

            if (!sb.ToText().IsEmpty())
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{sb.ToText()}");
                return;
            }
            #endregion

            o.Password = U.EncryptString(o.Username);
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
        List<object> oList = U.GetGridData(gvData, typeof(Users)).ListData;
        Result = U.MultipleRequiredValidation(oList);
        if (!Result.IsEmpty())
        {
            U.ShowMessage($"Save Failed,{Environment.NewLine} {Result}");
            return;
        }

        int Seq = 0;
        StringBuilder sb = new StringBuilder();
        foreach (Users o in oList)
        {
            if (!o.IsChecked) continue;
            #region Validation            
            if (DataAccess.IsFieldExist(ViewState[CNT.VS.Table], "Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. User with Name : {o.Name} already exist");
            if (DataAccess.IsFieldExist("Operators", "Username", o.Username, 0)) sb.AppendLine($"{Seq.Inc()}. Operator with Name : {o.Username} already exist");
            if (o.DepartmentId == 0) sb.AppendLine($"{Seq.Inc()}. Department is required");
            if (!U.IsValidEmail(o.Email)) sb.AppendLine($"{Seq.Inc()}. {o.Email} is Invalid Email Address");            
            if (!sb.ToText().IsEmpty()) continue;
            #endregion

            o.ModifiedBy = $"{ViewState[CNT.VS.Username]}";
            Result = o.Update();
            if (Result.Contains("Error Message"))
            {
                U.ShowMessage($"Save Failed,{Environment.NewLine} {Result}");
                return;
            }
            MasterData md = MasterData.GetById(o.DepartmentId, "Department");
            o.Department = md.Name;
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
            List<object> oList = U.GetGridData(gvData, typeof(Users)).ListData;
            if (oList.FindAll(a => ((Users)a).IsChecked).Count == 0)
            {
                U.ShowMessage("Please select row that you want to delete");
                return;
            }

            for (int i = 0; i < oList.Count; i++)
            {
                Users o = (Users)oList[i];
                if (!o.IsChecked) continue;

                Result = DataAccess.IsDataExist(0, "UsersId", o.Id);
                if (!Result.IsEmpty())
                {
                    U.ShowMessage($"Delete Failed, {Environment.NewLine}Data used in {Result}, please delete those data first");
                    return;
                }

                Result = DataAccess.Delete(0, o.Id, "Id", "Users");
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
    private void ResetPassword()
    {
        List<object> oList = U.GetGridData(gvData, Users.MyType).ListData;
        if (!oList.Exists(a => ((Users)a).IsChecked))
        {
            U.ShowMessage("Please select User you want to reset the password");
            return;
        }
        foreach (Users o in oList.FindAll(a => ((Users)a).IsChecked))
        {
            string Result = o.ResetPassword();
            if (Result.ContainErrorMessage())
            {
                U.ShowMessage($"Reset password for User : {o.Name} failed, {Result}");
                return;
            }
        }
        BindData();
        U.ShowMessage("Password has been Reset Successfully");
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
        List<object> oList = Users.GetByCriteria(pList, out double TotalRow);
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };

        #region HeaderExcel	
        w.SetExcel("No");        
        w.SetExcel("Name");
        w.SetExcel("Department");
        w.SetExcel("Email");
        w.SetExcel("Username");        
        w.SetExcel("Active");
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (Users o in oList)
        {
            w.Row += 1;
            w.SetExcel(o.No, true);            
            w.SetExcel(o.Name);
            w.SetExcel(o.Department);
            w.SetExcel(o.Email);
            w.SetExcel(o.Username);            
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
        resp.AppendHeader("content-disposition", string.Format("attachment; filename=Users.xlsx"));
        resp.AppendHeader("Content-Transfer-Encoding", "binary");
        resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        byte[] buffer = null;
        string tempFilename = $@"{U.PathTempFolder}Users.xlsx";

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