using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using DAL;
using System.IO;
using System.Web.Services.Description;

public partial class Pages_Logger_Logger : PageBase
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
            case "btnView":               
                if (!ddlFunction.SelectedValue.IsEmpty() && ddlTotalizer.SelectedValue.IsEmpty()) ViewByFunction();
                else ViewByTotalizer();
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
            case "ddlFunction":
                FunctionChanged();
                break;
        }
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            ltrl_No.Text = $"{e.Row.RowIndex + 1}";

            Literal ltrl_Function = (Literal)e.Row.FindControl("ltrl_Function");
            Literal ltrl_Level = (Literal)e.Row.FindControl("ltrl_Level");
            
            string Space = "";
            for (int i = 1; i < ltrl_Level.Text.ToInt(); i++)
                Space = $"{Space}&nbsp;&nbsp;&nbsp;&nbsp;";
            ltrl_Function.Text = $"{Space}{ltrl_Function.Text}";
        }
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInit();
        U.SetFunctionLogger(ddlFunction, "Pilih Fungsi");
        ddlTotalizer.Items.Add(new ListItem("Pilih Lokasi", ""));
        tbStart.Value = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");
        tbEnd.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        TimeSpan span = tbEnd.Value.ToHTML5DateTime().Subtract(tbStart.Value.ToHTML5DateTime());
        ttTotal.Text = $"{span.Days} Days {span.Hours} Hours";
        //ViewByTotalizer();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");        
    }
    private void ViewByTotalizer()
    {
        List<object> DataPCWinList = Logger.GetTotalizerPCWin(tbStart.Value.ToHTML5DateTime(), tbEnd.Value.ToHTML5DateTime());
        List<object> Totalizers = Totalizer.GetALLActive();
        List<object> oList = new List<object>();
        foreach (Logger o in DataPCWinList)
        {
            Totalizer t = (Totalizer)Totalizers.Find(a => ((Totalizer)a).Name.Trim() == o.Totalizer.Trim());
            if (t == null) continue;

            o.Id = t.Id;
            o.ParentId = t.ParentId;
            o.FunctionLoggerId = t.FunctionLoggerId;
            o.Function = t.FunctionLogger;
            o.Level = t.Level;
            oList.Add(o);
        }

        List<object> Data = new List<object>();
        if (ddlTotalizer.SelectedValue.IsEmpty())
        {
            int RootLevel = oList.Min(a => ((Logger)a).Level);
            foreach (Logger o in oList.FindAll(a => ((Logger)a).Level == RootLevel))
            {
                double TotalStartMeter = 0;
                double TotalEndMeter = 0;
                Data.Add(o);
                GetChildLogger(o.Id, oList, ref Data, ref TotalStartMeter, ref TotalEndMeter);
                o.TotalStartMeter = TotalStartMeter;
                o.TotalEndMeter = TotalEndMeter;

            }
        }
        else
        {
            Logger l = (Logger)oList.Find(a => ((Logger)a).Id == ddlTotalizer.SelectedValue.ToInt());
            if (l != null)
            {
                double TotalStartMeter = 0;
                double TotalEndMeter = 0;
                Data.Add(l);
                GetChildLogger(l.Id, oList, ref Data, ref TotalStartMeter, ref TotalEndMeter);
                l.TotalStartMeter = TotalStartMeter;
                l.TotalEndMeter = TotalEndMeter;
            }
        }

        if (Data.Count == 0) oList.Add(new Logger { Totalizer = CNT.DataNotAvailable });
        U.BindGrid(gvData, Data);
        SetDisplayColum(true);
    }
    private void ViewByFunction()
    {
        List<object> DataPCWinList = Logger.GetTotalizerPCWin(tbStart.Value.ToHTML5DateTime(), tbEnd.Value.ToHTML5DateTime());
        List<object> Totalizers = Totalizer.GetALLActive();
        List<object> oList = new List<object>();
        foreach (Logger o in DataPCWinList)
        {
            Totalizer t = (Totalizer)Totalizers.Find(a => ((Totalizer)a).Name.Trim() == o.Totalizer.Trim());
            if (t == null) continue;

            o.Id = t.Id;
            o.ParentId = t.ParentId;
            o.FunctionLoggerId = t.FunctionLoggerId;
            o.Function = t.FunctionLogger;
            o.Level = t.Level;
            oList.Add(o);
        }

        List<object> Data = new List<object>();
        foreach (Logger o in oList.FindAll(a => ((Logger)a).FunctionLoggerId == ddlFunction.SelectedValue.ToInt()))
        {
            Data.Add(o);
        }
        if (Data.Count == 0) oList.Add(new Logger { Totalizer = CNT.DataNotAvailable });
        U.BindGrid(gvData, Data);
        SetDisplayColum(false);
    }
    private void GetChildLogger(int ParentId, List<object> oList, ref List<object> Data, ref double TotalStartMeter, ref double TotalEndMeter)
    {
        List<object> cList = oList.FindAll(a => ((Logger)a).ParentId == ParentId);
        foreach (Logger c in cList)
        {
            double TotalChildStartMeter = 0;
            double TotalChildEndMeter = 0;
            TotalChildStartMeter += c.StartMeter;
            TotalChildEndMeter += c.EndMeter;
            Data.Add(c);
            GetChildLogger(c.Id, oList, ref Data, ref TotalChildStartMeter, ref TotalChildEndMeter);
            TotalStartMeter += TotalChildStartMeter;
            TotalEndMeter += TotalChildEndMeter;
            c.TotalStartMeter = TotalChildStartMeter - c.StartMeter;
            c.TotalEndMeter = TotalChildEndMeter - c.EndMeter;
            
        }
    }
    private void FunctionChanged()
    {
        ddlTotalizer.Items.Clear();
        ddlTotalizer.Items.Add(new ListItem("Pilih Lokasi", ""));
        if (ddlFunction.SelectedValue.IsEmpty()) return;
        foreach (FunctionStation f in FunctionStation.GetByFunctionId(ddlFunction.SelectedValue))
            ddlTotalizer.Items.Add(new ListItem(f.Station, f.StationId.ToText()));
    }
    private void SetDisplayColum(bool Value)
    {
        gvData.Columns[1].Visible = Value;
        gvData.Columns[5].Visible = Value;
        gvData.Columns[6].Visible = Value;
        gvData.Columns[8].Visible = Value;
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


        List<object> oList = U.GetGridData(gvData, typeof(Logger)).ListData;
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };
        

        #region HeaderExcel	
        w.SetExcel("No");
        if (!(!ddlFunction.SelectedValue.IsEmpty() && ddlTotalizer.SelectedValue.IsEmpty()))
            w.SetExcel("Fungsi");
        w.SetExcel("Lokasi");
        w.SetExcel("Meter Awal");
        w.SetExcel("Meter Akhir");
        w.SetExcel("Kubikasi");
        w.SetExcel("Unit");
        if (!(!ddlFunction.SelectedValue.IsEmpty() && ddlTotalizer.SelectedValue.IsEmpty()))
        {
            w.SetExcel("Total Meter Awal Anggota");
            w.SetExcel("Total Meter Akhir Anggota");
            w.SetExcel("Total Kubikasi Anggota");
        }
        #endregion

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (Logger o in oList)
        {
            w.Row += 1;
            w.SetExcel(w.Row, true);
            if (!(!ddlFunction.SelectedValue.IsEmpty() && ddlTotalizer.SelectedValue.IsEmpty()))
                w.SetExcel(o.Function.Replace("&nbsp;", " "));
            w.SetExcel(o.Totalizer);
            w.SetExcel(o.StartMeter, true);
            w.SetExcel(o.EndMeter, true);
            w.SetExcel(o.Cubication, true);
            w.SetExcel(o.Unit);
            if (!(!ddlFunction.SelectedValue.IsEmpty() && ddlTotalizer.SelectedValue.IsEmpty()))
            {
                w.SetExcel(o.TotalStartMeter, true);
                w.SetExcel(o.TotalEndMeter, true);
                w.SetExcel(o.TotalCubicationAnggota, true);
            }
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
        resp.AppendHeader("content-disposition", string.Format("attachment; filename=Logger.xlsx"));
        resp.AppendHeader("Content-Transfer-Encoding", "binary");
        resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        byte[] buffer = null;
        string tempFilename = $@"{U.PathTempFolder}Logger.xlsx";

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