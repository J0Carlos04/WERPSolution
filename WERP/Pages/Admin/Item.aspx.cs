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

public partial class Pages_Item : PageBase
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
            case "btnExportExcel":
                ExportExcel();
                break;
            case "btnUpload":
                Upload();
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

        List<object> oList = oList = Goods.GetByCriteria(pList, out double TotalRow);
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
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Goods&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Goods&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
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
        SetInitAdd();
        BindData();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Goods")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Goods")}return false;";
        fuUpload.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUpload.ClientID));
    }
    private void SetInitControl()
    {
        btnSearch.Hidden = false;
        btnSorting.Hidden = false;

        btnAdd.Hidden = false;        

        btnExportExcel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = true;
    }
    private void SetInitAdd()
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", "ItemInput");
        joBuilder.AddProperty("title", "New Item");
        joBuilder.AddProperty("iframeUrl", "getNewWindowUrl()", true);
        joBuilder.AddProperty("refreshWhenExist", true);
        joBuilder.AddProperty("iconFont", "plus");
        btnAdd.OnClientClick = String.Format("parent.addExampleTab({0});", joBuilder);
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

        List<object> oList = Goods.GetByCriteria(pList, out TotalRow);

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
        oList.Add(new Goods() { No = -1, Name = CNT.DataNotAvailable });        
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

        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("StartNo", ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo]));
        pList.Add(new clsParameter("DataType", "all"));
        List<object> oList = Goods.GetByCriteria(pList, out double TotalRow);
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };

        #region HeaderExcel
        w.SetExcel("Username");
        w.SetExcel(U.GetUsername());
        w.Row += 1;
        w.Column = 0;
        w.SetExcel("No");
        w.SetExcel("Code");
        w.SetExcel("Name");
        w.SetExcel("Description");
        w.SetExcel("Category");
        w.SetExcel("Group");
        w.SetExcel("Brand");
        w.SetExcel("Model");
        w.SetExcel("Material");
        w.SetExcel("Specs");
        w.SetExcel("UOM");
        w.SetExcel("Size");
        w.SetExcel("Threshold");
        w.SetExcel("Use SKU");
        w.SetExcel("Active");
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (Goods o in oList)
        {
            w.Row += 1;
            w.SetExcel(o.No, true);
            w.SetExcel(o.Code);
            w.SetExcel(o.Name);
            w.SetExcel(o.Description);
            w.SetExcel(o.Category);
            w.SetExcel(o.ItemGroup);
            w.SetExcel(o.Brand);
            w.SetExcel(o.Model);
            w.SetExcel(o.Material);
            w.SetExcel(o.Specs);
            w.SetExcel(o.UOM);
            w.SetExcel(o.Size);
            w.SetExcel(o.Threshold);
            w.SetExcel(o.UseSKU);
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
    private void Upload()
    {
        #region Check Excel File
        if (fuUpload.PostedFile.InputStream.Length == 0)
        {
            Utility.ShowMessage("Please choose excel file", btnUpload);
            return;
        }
        SpreadsheetInfo.SetLicense("E43Y-EJC3-221R-AA38");
        ExcelFile excel = new ExcelFile();
        string fileExt = Path.GetExtension(fuUpload.FileName);
        if (fileExt.ToUpper() == ".XLS") excel.LoadXls(fuUpload.PostedFile.InputStream);
        else if (fileExt.ToUpper() == ".XLSX") excel.LoadXlsx(fuUpload.PostedFile.InputStream, XlsxOptions.None);
        else
        {
            Utility.ShowMessage("Please choose excel file", btnUpload);
            return;
        }
        ExcelWorksheet sheet = excel.Worksheets[0];
        #endregion

        List<object> oList = new List<object>();
        List<object> euList = new List<object>();
        #region Data
        int Row = 0;        
        foreach (ExcelRow row in sheet.Rows)
        {
            Goods o = new Goods();
            MasterData md = new MasterData();

            if (Row == 0)
            {
                Row += 1;
                o.CreatedBy = row.Cells[1].Value.ToText();
                if (o.CreatedBy.IsEmpty())
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "Uploader", ErrorMessage = "Uploader Username is required" });
                else
                {
                    Users u = Users.GetByUsername(o.CreatedBy);
                    if (u.Id == 0) euList.Add(new ErrorUpload { LineNo = Row, Key = "Uploader", ErrorMessage = "Uploader Username is not registered" });
                }
                o.ModifiedBy = row.Cells[1].Value.ToText();
                continue;
            }
            else if (Row == 1)
            {
                Row += 1;
                continue;
            }
            int Column = 1;
            
            #region Code
            o.Code = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Code.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Code", ErrorMessage = "Item Code is required" });
            if (o.Code.Length > 30)
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Code", ErrorMessage = "Max Item Code is 30 character" });
            #endregion

            #region Name
            o.Name = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Name.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Name", ErrorMessage = "Item Name is required" });
            else
            {
                //Goods g = Goods.GetByCode(o.Code);
                //if (g.Id != 0 && g.Name != o.Name)
                //    euList.Add(new ErrorUpload { LineNo = Row, Key = "Name", ErrorMessage = $"Item with Code {o.Code} already exist" });
            }
            #endregion

            #region Description
            o.Description = row.Cells[Column].Value.ToText();
            Column += 1;
            #endregion

            #region Category
            o.Category = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Category.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Category", ErrorMessage = "Category is required" });
            else
            {
                md = MasterData.GetByName(o.Category, "Category");
                if (md.Id == 0)
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "Category", ErrorMessage = $"Category {o.Category} is not found in database, please add it to Master Data Category first" });
                else o.CategoryId = md.Id;
            }            
            #endregion

            #region ItemGroup
            o.ItemGroup = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.ItemGroup.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "ItemGroup", ErrorMessage = "Item Group is required" });
            else
            {
                md = MasterData.GetByName(o.ItemGroup, "ItemGroup");
                if (md.Id == 0)
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "ItemGroup", ErrorMessage = $"Item Group {o.ItemGroup} is not found in database, please add it to Master Data Item Group first" });
                else o.ItemGroupId = md.Id;
            }            
            #endregion

            #region Brand
            o.Brand = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Brand.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Brand", ErrorMessage = "Brand is required" });
            else
            {
                md = MasterData.GetByName(o.Brand, "Brand");
                if (md.Id == 0)
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "Brand", ErrorMessage = $"Brand {o.Brand} is not found in database, please add it to Master Data Brand first" });
                else o.BrandId = md.Id;
            }            
            #endregion

            #region Model
            o.Model = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Model.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Model", ErrorMessage = "Model is required" });
            else
            {
                md = MasterData.GetByName(o.Model, "Model");
                if (md.Id == 0)
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "Model", ErrorMessage = $"Model {o.Model} is not found in database, please add it to Master Data Model first" });
                else o.ModelId = md.Id;
            }           
            #endregion

            #region Material
            o.Material = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Material.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Material", ErrorMessage = "Material is required" });
            else
            {
                md = MasterData.GetByName(o.Material, "Material");
                if (md.Id == 0)
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "Material", ErrorMessage = $"Material {o.Material} is not found in database, please add it to Master Data Material first" });
                else o.MaterialId = md.Id;
            }            
            #endregion

            #region Specs
            o.Specs = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Specs.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Specs", ErrorMessage = "Specs is required" });
            else
            {
                md = MasterData.GetByName(o.Specs, "Specs");
                if (md.Id == 0)
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "Specs", ErrorMessage = $"Specs {o.Specs} is not found in database, please add it to Master Data Specs first" });
                else o.SpecsId = md.Id;
            }            
            #endregion

            #region UOM
            o.UOM = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.UOM.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "UOM", ErrorMessage = "UOM is required" });
            else
            {
                UOM uom = UOM.GetByCode(o.UOM);
                if (uom.Id == 0)
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "UOM", ErrorMessage = $"UOM {o.UOM} is not found in database, please add it to Master Data UOM first" });
                else o.UOMId = uom.Id;
            }            
            #endregion

            #region Size
            o.Size = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Size.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Size", ErrorMessage = "Size is required" });            
            #endregion

            #region Threshold
            o.Threshold = row.Cells[Column].Value.ToInt();
            Column += 1;
            if (o.Threshold.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Threshold", ErrorMessage = "Threshold is required" });
            #endregion

            #region SKU
            o.UseSKU = row.Cells[Column].Value.ToBool();
            #endregion

            #region Active
            string Active = row.Cells[Column].Value.ToText();
            Column += 1;
            if (o.Active.IsEmpty())
                euList.Add(new ErrorUpload { LineNo = Row, Key = "Active", ErrorMessage = "Active is required" });
            else
            {
                if (!(Active.ToLower() == "false" || Active.ToLower() == "true"))
                    euList.Add(new ErrorUpload { LineNo = Row, Key = "Active", ErrorMessage = "Active must be only false or true" });
            }
            #endregion
            
            oList.Add(o);
        }
        #endregion

        if (!euList.IsEmptyList())
        {
            Session[CNT.Session.ErrorList] = euList;
            U.OpenNewTab("ErrorList", "Error List Item", "getErrorWindowUrl()");
            return;
        }

        #region Save Data
        foreach (Goods o in oList)
        {
            string Result = "";
            Goods oDB = Goods.GetByCodeName(o.Code, o.Name);
            if (oDB.Id == 0)
            {
                Result = o.Insert();
                if (Result.ContainErrorMessage())
                {
                    U.ShowMessage(Result);
                    return;
                }
            }
            else
            {
                o.Id = oDB.Id;
                Result = o.Update();
                return;
            }
        }        
        #endregion
        U.ShowMessage("Item has been Upload Successfully", F.Icon.Information, "Success");
        Search();
    }
    #endregion
}