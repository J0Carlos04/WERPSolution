using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.EnterpriseServices;

public partial class Pages_WorkOrder_MaintenanceSchedulerInput : PageBase
{
    #region Fields
    private const string vsUserName = "Username";
    private const string vsUseItem = "UseItem";
    private const string SMTableName = "ScheduleMaintenanceAttachment";
    private const string vsIsUsedExist = "IsUsedExist";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            Initialize();
        string Script = "InitSelect2MaintenanceSchedulerInput();";
        F.PageContext.RegisterStartupScript(Script);
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnUpload":
                AddAttachment();
                break;
            case "btnSubmit":
                Submit();
                break;
            case "btnDelete":
                Delete();
                break;
        }
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnAddSchedule":
                AddSchedule();
                break;
            case "btnAdd":
                AddItem();
                break;
        }
    }
    protected void Download(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnDownload":
                DownloadFile(btn, "lb_FileName");
                break;                
        }
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddlArea":
                AreaChanged();
                break;            
            case "ddlSubject":
                SubjectChanged();
                break;           
            case "ddlOperatorType":
                OperatorTypeChanged();
                break;
            case "ddlVendor":
                VendorChanged();
                break;
        }
    }
    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {
            case "imbDelete":
                DeleteItem(imb);
                break;
            case "imbDeleteAttachment":
                DeleteAttachment(imb);
                break;
        }
    }    
    protected void gvSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            List<object> oList = (List<object>)gvSchedule.DataSource;
            Schedule o = (Schedule)oList[e.Row.RowIndex];
            HtmlInputGenericControl tb_StartDate = (HtmlInputGenericControl)e.Row.FindControl("tb_StartDate");
            HtmlInputGenericControl tb_EndDate = (HtmlInputGenericControl)e.Row.FindControl("tb_EndDate");
            DropDownList ddl_PeriodType = (DropDownList)e.Row.FindControl("ddl_PeriodType");
            LinkButton lbOrder = (LinkButton)e.Row.FindControl("lbOrder");            

            if (o.StartDate != DateTime.MinValue) tb_StartDate.Value = o.StartDate.ToString("yyyy-MM-dd");
            if (o.EndDate != DateTime.MinValue) tb_EndDate.Value = o.EndDate.ToString("yyyy-MM-dd");
            ddl_PeriodType.SelectedValue = o.PeriodType;            
        }
    }
    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_Seq = (Literal)e.Row.FindControl("ltrl_Seq");
            ltrl_Seq.Text = $"{e.Row.RowIndex + 1}";

            Button btnCodeLookup = (Button)e.Row.FindControl("btnCodeLookup");
            btnCodeLookup.OnClientClick = wItem.GetShowReference($"~/Pages/Inventory/SelectItem.aspx?RowIndex={e.Row.RowIndex}");
            btnCodeLookup.OnClientClick += "return false;";
        }
    }
    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btnDownload = (Button)e.Row.FindControl("btnDownload");
            LinkButton lb_FileName = (LinkButton)e.Row.FindControl("lb_FileName");
            lb_FileName.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownload.ClientID));
        }
    }
    protected string GetWorkOrderUrl(object id)
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", "ViewWorkOrder");
        joBuilder.AddProperty("title", "View Work Order");
        joBuilder.AddProperty("iframeUrl", $"getWorkOrderWindowUrl('{id}')", true);
        joBuilder.AddProperty("refreshWhenExist", true);
        joBuilder.AddProperty("iconFont", "pencil");
        return String.Format("parent.addExampleTab({0});", joBuilder);
    }    
    protected void wLocation_Close(object sender, F.WindowCloseEventArgs e)
    {
        hfLocationId.Text = e.CloseArgument;
        Location l = Location.GetById(e.CloseArgument);
        tbLocation.Value = l.Name;
    }
    protected void wItem_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrResult = e.CloseArgument.Split('|');
        Goods g = Goods.GetById(arrResult[1]);

        List<object> oList = U.GetGridData(gvItems, typeof(ScheduleMaintenanceItem)).ListData;
        if (oList.Exists(a => ((ScheduleMaintenanceItem)a).ItemId == g.Id))
        {
            U.ShowMessage("Item already exist");
            return;
        }

        foreach (GridViewRow row in gvItems.Rows)
        {
            if (row.RowIndex != arrResult[0].ToInt()) continue;
            Literal ltrl_ItemId = (Literal)row.FindControl("ltrl_ItemId");
            TextBox tb_ItemCode = (TextBox)row.FindControl("tb_ItemCode");
            Literal ltrl_ItemName = (Literal)row.FindControl("ltrl_ItemName");
            ltrl_ItemId.Text = g.Id.ToString();
            tb_ItemCode.Text = g.Code;
            ltrl_ItemName.Text = g.Name;
        }
    }
    #endregion

    #region Methods
    private void Initialize()
    {
        InitState();
        InitControl();
        InitEdit();
    }
    private void InitState()
    {
        ViewState[vsUserName] = U.GetUsername();
        if ($"{ViewState[vsUserName]}" == "") Response.Redirect(@"~\Pages\default.aspx");
    }
    private void InitControl()
    {
        U.Hide("dvCode");
        btnCancel.OnClientClick = "parent.removeActiveTab();";
        fu.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUpload.ClientID));
        U.SetSubjectNotUsedCustomer(ddlSubject);        
        U.SetDropDownMasterData(ddlArea, "Area");
        U.SetDropDownMasterData(ddlWorkOrderCategory, "WorkOrderCategory");
        U.SetDropDownMasterData(ddlVendor, "Vendor");
        U.BindGrid(gvSchedule, new List<object> { new Schedule { Active = true } });
        U.BindGrid(gvAttachment, new List<object> { new Attachment { Seq = 0, FileName = CNT.DataNotAvailable } });
        U.BindGrid(gvItems, new List<object> { new ScheduleMaintenanceItem() });
    }    
    private void InitEdit()
    {
        if (U.Id.IsNull())
        {
            btnDelete.Hidden = true;            
            return;
        }
        ScheduleMaintenance sm = ScheduleMaintenance.GetById(U.Id);
        if (sm.Id.IsZero().ShowError("Schedule Maintenance is not exist")) return;
        U.Display("dvCode");
        tbCode.Value = sm.Code;
        #region AreaLocation
        ddlArea.SelectedValue = $"{sm.AreaId}";
        tbLocation.Value = sm.Location;
        hfLocationId.Text = sm.LocationId.ToText();
        btnSelectLocation.OnClientClick = wLocation.GetShowReference($"~/Pages/Inventory/SelectLocation.aspx?Id={ddlArea.SelectedValue}");
        #endregion

        ddlWorkOrderCategory.SelectedValue = $"{sm.WorkOrderCategoryId}";
        ddlSubject.SelectedValue = $"{sm.SubjectId}";
        tbWorkDescription.Value = sm.WorkDescription;

        #region Conducted
        if (sm.OperatorType.IsNotEmpty())
        {
            ddlOperatorType.SetValue(sm.OperatorType);
            OperatorTypeChanged();
            if (sm.VendorId != 0)
            {
                ddlVendor.SetValue(sm.VendorId);
                VendorChanged();
                ddlOperator.SetValue(sm.OperatorsId);
                if (ddlVendor.SelectedValue.IsEmpty())
                {
                    Vendor v = Vendor.GetById(sm.VendorId);
                    ddlVendor.Items.Add(new ListItem(v.Name, "0"));
                    ddlVendor.SelectedValue = "0";
                }
                if (ddlOperator.SelectedValue.IsEmpty())
                {
                    Operators op = Operators.GetById(sm.OperatorsId);
                    ddlOperator.Items.Add(new ListItem(op.Name, "0"));
                    ddlOperator.SelectedValue = "0";
                }
            }
            else
            {
                ddlOperator.SetValue(sm.OperatorUserId);
                if (ddlOperator.SelectedValue.IsEmpty())
                {
                    Users u = Users.GetById(sm.OperatorsId);
                    ddlOperator.Items.Add(new ListItem(u.Name, "0"));
                    ddlOperator.SelectedValue = "0";
                }
            }
        }  
        else OperatorTypeChanged();
        #endregion

        List<object> woList = WorkOrder.GetByScheduleMaintenanceId(sm.Id);
        #region Used
        bool IsUsedExist = woList.Exists(a => ((WorkOrder)a).Used);
        ViewState[vsIsUsedExist] = IsUsedExist;
        if (IsUsedExist)
        {
            ddlArea.Enabled = false;
            ddlWorkOrderCategory.Enabled = false;
            ddlSubject.Enabled = false;
            tbWorkDescription.Disabled = true;
            ddlOperatorType.Enabled = false;
            ddlOperator.Enabled = false;
            ddlVendor.Enabled = false;
            btnAdd.Visible = false;
            fu.Visible = false;
            btnDelete.Hidden = true;
        }
        #endregion

        List<object> sList = Schedule.GetByScheduleMaintenanceId(U.Id);
        if (IsUsedExist)
        {
            sList.ForEach(a => { ((Schedule)a).Active = false; });
            sList.Insert(0, new Schedule { Active = true });
        }
        U.BindGrid(gvSchedule, sList);

        Subject sbj = Subject.GetById(sm.SubjectId);
        ViewState[vsUseItem] = sbj.UseItem;
        if (sbj.UseItem.Is("Both") || sbj.UseItem.Is("Yes"))
        {
            pnlItem.Visible = true;
            List<object> smiList = ScheduleMaintenanceItem.GetByScheduleMaintenanceId(sm.Id);
            if (IsUsedExist) smiList.ForEach(a => { ((ScheduleMaintenanceItem)a).Mode = "Edit"; });
            U.BindGrid(gvItems, smiList);
        }

        List<object> aList = Attachment.GetByOwnerID(SMTableName, sm.Id);
        foreach (Attachment att in aList)
            U.SaveAttachmentToTempFolder(att);
        if (IsUsedExist) aList.ForEach(a => { ((Attachment)a).Mode = "Edit"; });
        if (aList.Count > 0) U.BindGrid(gvAttachment, aList);
    }    
        
    private void AddSchedule()
    {

    }
    private void AddItem()
    {
        Wrapping w = new Wrapping();
        ItemValidation(w);
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }

        w.ListData.Add(new ScheduleMaintenanceItem { });
        U.BindGrid(gvItems, w.ListData);
    }
    private void DeleteItem(ImageButton imb)
    {
        GridViewRow row = (GridViewRow)imb.Parent.Parent;
        List<object> oList = U.GetGridData(gvItems, typeof(ScheduleMaintenanceItem)).ListData;
        oList.RemoveAt(row.RowIndex);
        if (oList.Count == 0) U.BindGrid(gvItems, new List<object> { new ScheduleMaintenanceItem { } });
        else U.BindGrid(gvItems, oList);
    }

    #region Control Changed
    private void SubjectChanged()
    {
        pnlItem.Visible = false;
        Subject s = Subject.GetById(ddlSubject.SelectedValue);
        ViewState[vsUseItem] = s.UseItem;
        if (s.UseItem.Is("Both") || s.UseItem.Is("Yes"))
            pnlItem.Visible = true;
    }
    private void AreaChanged()
    {
        btnSelectLocation.OnClientClick = "return false;";
        if (ddlArea.SelectedValue == "") return;
        btnSelectLocation.OnClientClick = wLocation.GetShowReference($"~/Pages/WorkOrder/SelectLocation.aspx?Id={ddlArea.SelectedValue}");
    }
    private void VendorChanged()
    {
        ddlOperator.Items.Clear();
        if (ddlVendor.SelectedValue.IsEmpty()) return;
        U.SetOperatorsByVendor(ddlOperator, ddlVendor.SelectedValue);
    }
    private void OperatorTypeChanged()
    {
        U.Hide("dvVendor");
        if (ddlOperatorType.SelectedValue == "") return;
        if (ddlOperatorType.SelectedValue == "Internal")
            U.SetDropDownMasterData(ddlOperator, "Users");
        else
        {
            U.Display("dvVendor");
            ddlVendor.SelectedValue = "";
            ddlOperator.Items.Clear();
        }
    }
    #endregion

    #region  Attachment
    private void DownloadFile(Button btn, string lbFileName)
    {
        GridViewRow Row = (GridViewRow)btn.Parent.Parent;
        LinkButton lb = (LinkButton)Row.FindControl(lbFileName);
        Literal ltrl_FileNameUniq = (Literal)Row.FindControl("ltrl_FileNameUniq");
        if (ltrl_FileNameUniq.Text == "") return;
        byte[] Data = U.GetFile(ltrl_FileNameUniq.Text);
        U.OpenFile(lb.Text, U.GetContentType(lb.Text), Data);
    }
    private void AddAttachment()
    {
        if (!fu.HasFile)
        {
            U.ShowMessage("File is required");
            return;
        }

        Attachment att = new Attachment();
        List<object> attList = U.GetGridData(gvAttachment, typeof(Attachment)).ListData;
        attList.RemoveAll(a => ((Attachment)a).Seq == 0);
        att.Seq = attList.Count == 0 ? 1 : attList.Count + 1;
        att.FileName = fu.FileName;
        att.FileNameUniq = $"{ViewState[vsUserName]}_{fu.FileName}";
        attList.Add(att);
        string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";
        File.WriteAllBytes(Path, fu.FileBytes);
        Utility.BindGrid(gvAttachment, attList);
    }
    private void DeleteAttachment(ImageButton imb)
    {
        GridViewRow row = (GridViewRow)imb.Parent.Parent;
        Literal ltrl_Seq = (Literal)row.FindControl("ltrl_Seq");
        GridView gv = (GridView)row.Parent.Parent;

        List<object> aList = U.GetGridData(gvAttachment, typeof(Attachment)).ListData;
        Attachment att = (Attachment)aList[row.RowIndex];
        string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";
        File.Delete(Path);
        aList.RemoveAll(a => ((Attachment)a).Seq.ToString() == ltrl_Seq.Text);
        if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
        Utility.BindGrid(gv, aList);
    }
    #endregion

    private void Submit()
    {
        Wrapping w = new Wrapping();
        Validation(w);
        if ($"{w.Sb}" != "") U.ShowMessage($"{w.Sb}");
        else
        { 
            string Result = "";
            if ($"{ViewState[vsIsUsedExist]}" == "True") Result = SaveSchedule();
            else Result = Save(w);
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
        }
    }
    private void Validation(Wrapping w)
    {
        if (ddlSubject.SelectedValue.IsEmpty()) w.RequiredValidation = "Subject";
        if (ddlArea.SelectedValue.IsEmpty()) w.RequiredValidation = "Area";
        if (hfLocationId.Text.IsEmpty()) w.RequiredValidation = "Location";
        if (ddlWorkOrderCategory.SelectedValue.IsEmpty()) w.RequiredValidation = "Order Category";

        #region Assigned
        if (ddlOperatorType.SelectedValue.IsNotEmpty())
        {
            if (ddlOperatorType.SelectedValue == "External")
            {
                if (ddlVendor.SelectedValue.IsEmpty()) w.RequiredValidation = "Vendor";
                else if (ddlVendor.SelectedValue.Is("0")) w.ErrorValidation = "Vendor is not active, please change with other vendor";
            }
            if (ddlOperator.SelectedValue.IsEmpty()) w.RequiredValidation = "Conducted By";
            else if (ddlOperator.SelectedValue.Is("0")) w.ErrorValidation = "Conducted By is not active, please change with someone else";
        }
        #endregion

        List<object> sList = U.GetGridData(gvSchedule, typeof(Schedule)).ListData;
        int i = 0;
        foreach (Schedule s in sList)
        {
            i += 1;
            if (s.StartDate == DateTime.MinValue) w.ErrorValidation = $"Start Date is required at Row {i}";
            if (s.Period == 0) w.ErrorValidation = $"Period is required at Row {i}";
            if (s.PeriodType == "") w.ErrorValidation = $"Period Type is required at Row {i}";
            if (s.EndDate == DateTime.MinValue) w.ErrorValidation = $"End Date is required at Row {i}";
        }
        if (ViewState[vsUseItem].ToBool()) ItemValidation(w);
    }
    private void ItemValidation(Wrapping w)
    {
        w.ListData = U.GetGridData(gvItems, typeof(ScheduleMaintenanceItem)).ListData;
        int idx = 0;
        foreach (ScheduleMaintenanceItem o in w.ListData)
        {
            idx += 1;
            if (o.ItemId == 0) w.ErrorValidation = $"Item is required at row {idx}";
            if (o.Qty == 0) w.ErrorValidation = $"Qty is required at row {idx}";
        }
    }
    private string Save(Wrapping w)
    {
        string Result = "";

        #region Schedule Maintenance
        ScheduleMaintenance o = new ScheduleMaintenance();
        o.AreaId = ddlArea.SelectedValue.ToInt();
        o.LocationId = hfLocationId.Text.ToInt();
        o.WorkOrderCategoryId = ddlWorkOrderCategory.SelectedValue.ToInt();
        o.SubjectId = ddlSubject.SelectedValue.ToInt();
        o.WorkDescription = tbWorkDescription.Value;
        if (ddlOperatorType.SelectedValue.IsNotEmpty())
        {
            o.OperatorType = ddlOperatorType.SelectedValue;
            if (o.OperatorType == "External")
            {
                o.VendorId = ddlVendor.SelectedValue.ToInt();
                o.OperatorsId = ddlOperator.SelectedValue.ToInt();
            }
            else o.OperatorUserId = ddlOperator.SelectedValue.ToInt();
        }
        #region Conducted By
        if (ddlOperatorType.SelectedValue.IsNotEmpty())
        {
            o.OperatorType = ddlOperatorType.SelectedValue;
            if (o.OperatorType == "External")
            {
                o.VendorId = ddlVendor.SelectedValue.ToInt();
                o.OperatorsId = ddlOperator.SelectedValue.ToInt();
            }
            else o.OperatorUserId = ddlOperator.SelectedValue.ToInt();
        }
        #endregion
        if (U.Id.IsNull())
        {
            o.CreatedBy = $"{ViewState[vsUserName]}";
            Result = o.Insert();
            if (Result.ContainErrorMessage()) return Result;
            o.Id = Result.ToInt();
        }
        else
        {
            o.Id = U.Id.ToInt();
            o.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = o.Update();
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion                

        #region Schedule
        List<object> sList = U.GetGridData(gvSchedule, typeof(Schedule)).ListData;
        foreach (Schedule s in sList)
        {
            s.ScheduleMaintenanceId = o.Id;
            if (s.Id == 0)
            {                
                s.CreatedBy = $"{ViewState[vsUserName]}";
                Result = s.Insert();
                if (Result.ContainErrorMessage()) return Result;
                s.Id = Result.ToInt();
            }
            else
            {
                s.ModifiedBy = $"{ViewState[vsUserName]}";
                Result = s.Update();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        #endregion

        #region Attachment
        string TableName = "ScheduleMaintenanceAttachment";
        Result = Attachment.DeleteByOwnerId(o.Id, TableName);
        if (Result.Contains("Error Message :")) return Result;

        foreach (Attachment att in Utility.GetGridData(gvAttachment, typeof(Attachment)).ListData)
        {
            if (att.FileName == CNT.DataNotAvailable) continue;
            att.Table = TableName;
            att.OwnerId = o.Id;
            att.Data = U.GetFile(att.FileNameUniq);
            att.CreatedBy = $"{ViewState[vsUserName]}";
            Result = att.Insert(att);
            if (Result.Contains("Error Message :")) return Result;

            string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";
            File.Delete(Path);
        }
        #endregion

        #region Item
        List<object> iList = new List<object>();
        if ($"{ViewState[vsUseItem]}".Or("Both,Yes"))
        {
            iList = U.GetGridData(gvItems, typeof(ScheduleMaintenanceItem)).ListData;
            List<object> iDBList = ScheduleMaintenanceItem.GetByScheduleMaintenanceId(o.Id);
            foreach (ScheduleMaintenanceItem iDB in iDBList)
            {
                if (!iList.Exists(a => ((ScheduleMaintenanceItem)a).Id == iDB.Id))
                {
                    Result = iDB.Delete();
                    if (Result.ContainErrorMessage()) return Result;
                }
            }
            foreach (ScheduleMaintenanceItem i in iList)
            {
                i.ScheduleMaintenanceId = o.Id;                
                if (i.Id == 0)
                {
                    i.CreatedBy = $"{ViewState[vsUserName]}";
                    Result = i.Insert();
                    if (Result.ContainErrorMessage()) return Result;
                }
                else
                {
                    i.ModifiedBy = $"{ViewState[vsUserName]}";
                    Result = i.Update();
                    if (Result.ContainErrorMessage()) return Result;
                }
            }
        }
        #endregion

        #region Work Order
        List<object> woList = WorkOrder.GetByScheduleMaintenanceId(o.Id);
        if (!U.Id.IsNull())
        {
            Result = o.DeleteWorkOrder();
            if (Result.ContainErrorMessage()) return Result;
        }
        Schedule sActive = (Schedule)sList.Find(a => ((Schedule)a).Active);
        while (sActive.StartDate <= sActive.EndDate)
        {
            Parameters p = Parameters.GetByKey($"WODeviation{sActive.PeriodType}");

            WorkOrder wo = new WorkOrder();
            wo.ScheduleMaintenanceId = o.Id;
            wo.ScheduleId = sActive.Id;
            wo.Code = $"{DateTime.Now.Year}{DateTime.Now.Month.ToString("00")}";
            wo.OrderDate = sActive.StartDate;
            wo.EarliestStartDate = sActive.StartDate.AddDays((-1) * Convert.ToInt32(p.Text));
            wo.LatestStartDate = sActive.StartDate.AddDays(Convert.ToInt32(p.Text));
            wo.CreatedBy = $"{ViewState[vsUserName]}";

            if (ddlOperatorType.SelectedValue.IsNotEmpty())
            {
                wo.OperatorType = o.OperatorType;
                wo.VendorId = o.VendorId;
                wo.OperatorsId = o.OperatorsId;
                wo.OperatorUserId = o.OperatorUserId;
            }

            #region Status
            if (iList.Count.IsZero())
            {
                if (ddlOperatorType.SelectedValue.Or("Internal,External"))
                    wo.Status = CNT.Status.Preparation;
                else wo.Status = CNT.Status.Assignment;
            }
            else
            {
                if (ddlOperatorType.SelectedValue.Or("Internal,External"))
                    wo.Status = CNT.Status.StockOut;
                else wo.Status = CNT.Status.Assignment;
            }
            #endregion

            Result = wo.InsertByScheduler();
            if (Result.ContainErrorMessage()) return Result;
            wo.Id = Result.ToInt();

            if (sActive.PeriodType == "Year") sActive.StartDate = sActive.StartDate.AddYears(sActive.Period);
            else if (sActive.PeriodType == "Month") sActive.StartDate = sActive.StartDate.AddMonths(sActive.Period);
            else if (sActive.PeriodType == "Day") sActive.StartDate = sActive.StartDate.AddDays(sActive.Period);

            #region Work Order Item
            if ($"{ViewState[vsUseItem]}".Or("Both,Yes"))
            {                
                foreach (ScheduleMaintenanceItem i in iList)
                {
                    WorkOrderItem woi = new WorkOrderItem { WorkOrderId = wo.Id, ItemId = i.ItemId, Seq = i.Seq, Qty = i.Qty } ;
                    woi.CreatedBy = $"{ViewState[vsUserName]}";
                    Result = woi.Insert();
                    if (Result.ContainErrorMessage()) return Result;
                }
            }
            
            #endregion
        }
        #endregion

        return "";
    }
    private string SaveSchedule()
    {
        string Result = Schedule.InactiveByScheduleMaintenanceId(U.Id);
        if (Result.ContainErrorMessage()) return Result;
        List<object> sList = U.GetGridData(gvSchedule, typeof(Schedule)).ListData.FindAll(a => ((Schedule)a).Active);
        Schedule s = (Schedule)sList[0];
        s.ScheduleMaintenanceId = U.Id.ToInt();
        s.CreatedBy = $"{ViewState[vsUserName]}";
        Result = s.Insert();
        if (Result.ContainErrorMessage()) return Result;
        s.Id = Result.ToInt();

        #region Work Order                
        while (s.StartDate < s.EndDate)
        {
            Parameters p = Parameters.GetByKey($"WODeviation{s.PeriodType}");

            WorkOrder wo = new WorkOrder();
            wo.ScheduleMaintenanceId = U.Id.ToInt();
            wo.ScheduleId = s.Id;
            wo.Code = $"{DateTime.Now.Year}{DateTime.Now.Month.ToString("00")}";
            wo.OrderDate = s.StartDate;
            wo.EarliestStartDate = s.StartDate.AddDays((-1) * Convert.ToInt32(p.Text));
            wo.LatestStartDate = s.StartDate.AddDays(Convert.ToInt32(p.Text));
            wo.CreatedBy = $"{ViewState[vsUserName]}";
            Result = wo.InsertByScheduler();
            if (Result.ContainErrorMessage()) return Result;
            wo.Id = Result.ToInt();

            if (s.PeriodType == "Year") s.StartDate = s.StartDate.AddYears(s.Period);
            else if (s.PeriodType == "Month") s.StartDate = s.StartDate.AddMonths(s.Period);
            else if (s.PeriodType == "Day") s.StartDate = s.StartDate.AddDays(s.Period);

            #region Work Order Item
            if ($"{ViewState[vsUseItem]}" == "True")
            {
                foreach (ScheduleMaintenanceItem i in U.GetGridData(gvItems, typeof(ScheduleMaintenanceItem)).ListData)
                {
                    WorkOrderItem woi = new WorkOrderItem { WorkOrderId = wo.Id, ItemId = i.ItemId, Seq = i.Seq, Qty = i.Qty };
                    woi.CreatedBy = $"{ViewState[vsUserName]}";
                    Result = woi.Insert();
                    if (Result.ContainErrorMessage()) return Result;
                }
            }

            #endregion
        }

        #endregion
        return "";
    }
    private void Delete()
    {
        string Result = ScheduleMaintenance.Delete(U.Id);
        if (Result.ContainErrorMessage()) U.ShowMessage(Result, F.Icon.ErrorDelete, "Delete Failed!");
        else F.Alert.Show("Data has been Delete Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
    }
    #endregion        
}