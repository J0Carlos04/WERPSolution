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
using DAL;
using Microsoft.Exchange.WebServices.Data;

public partial class Pages_WorkOrder_WorkOrderInput : PageBase
{
    #region Fields    
    private const string SMTableName = "ScheduleMaintenanceAttachment";
    private const string HTableName = "HelpdeskAttachment";
    private const string WOTableName = "WorkOrderAttachment";                
    private const string vsWorkUpdateShow = "WorkUpdateShow";
    private const string TableWorkOrderWorkUpdateAttachment = "WorkOrderWorkUpdateAttachment";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) Initialize(); 
        if (lblParentTitle.Text == "Helpdesk") U.Hide("dvScheduler");
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnAddWorkUpdate":
                AddWorkUpdate();
                break;
            case "btnDeleteWorkUpdate":
                DeleteWorkUpdate();
                break;
            case "btnAddItem":
                AddItem();
                break;
            case "btnDeleteItem":
                DeleteItem();
                break;
            case "btnStartDateChanged":
                TargetCompletionChanged();
                break;
            case "btnSubmit":
                Submit();
                break;
            case "btnUpload":
                AddAttachment(fuWorkOrder, gvWOAttachment);
                break;
            case "btnDownload":
                DownloadFile(btn, "lb_FileName");
                break;
            case "btnSubmitAchievement":
                SubmitAchievement();
                break;
        }
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {                        
            case "btnUploadWorkUpdate":
                FileUpload fuWorkUpdate = (FileUpload)btn.FindControl("fuWorkUpdate");
                GridView gvAttachmentWorkUpdate = (GridView)btn.FindControl("gvAttachmentWorkUpdate");
                AddAttachment(fuWorkUpdate, gvAttachmentWorkUpdate);
                SetGPS(gvAttachmentWorkUpdate);
                break;
            case "btnDownload":                
                U.DownloadFile(btn, "lb_FileName");
                break;
            case "btnCodeLookup":
                ItemLookup(btn);
                break;
        }
        StatusChanged();
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddlStatus":
                StatusChanged();
                break;
            case "ddlOperatorType":
                OperatorTypeChanged();
                break;
            case "ddlVendor":
                VendorChanged();
                break;
            case "ddl_WorkType":
                WorkTypeChanged(ddl);
                break;
        }
    }
    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {                        
            case "imbDelWorkUpdateItem":
                DeleteWorkUpdateItem(imb);
                break;
            case "imbDelAttachWorkUpdate":               
            case "imbDelAttachWorkOrder":
                DeleteAttachment(imb);
                break;
        }
        StatusChanged();
    }
    protected void gvWorkUpdate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvWorkUpdate.ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            WorkOrderWorkUpdate o = (WorkOrderWorkUpdate)e.Row.DataItem;

            CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            HtmlInputGenericControl tbDate = (HtmlInputGenericControl)e.Row.FindControl("tbDate");
            TextBox tbWorkDetail = (TextBox)e.Row.FindControl("tbWorkDetail");
            HtmlGenericControl dvItemWO = (HtmlGenericControl)e.Row.FindControl("dvItemWO");

            ltrl_No.Text = $"{e.Row.RowIndex + 1}";
            if (o.Date != DateTime.MinValue) tbDate.Value = o.Date.ToString("yyyy-MM-dd HH:mm");
            tbWorkDetail.Text = o.WorkDetail;

            DropDownList ddl_WorkType = (DropDownList)e.Row.FindControl("ddl_WorkType");
            ddl_WorkType.SetValue(o.WorkType);
            WorkTypeChanged(ddl_WorkType);

            Button btnLookupWO = (Button)e.Row.FindControl("btnLookupWO");
            btnLookupWO.OnClientClick = wWo.GetShowReference($"~/Pages/Inventory/SelectWO.aspx?Seq={e.Row.RowIndex}&Id={U.Id}");
            btnLookupWO.OnClientClick += "return false;";            

            Button btnUploadWorkUpdate = (Button)e.Row.FindControl("btnUploadWorkUpdate");
            FileUpload fuWorkUpdate = (FileUpload)e.Row.FindControl("fuWorkUpdate");
            fuWorkUpdate.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUploadWorkUpdate.ClientID));

            GridView gvUsedItem = (GridView)e.Row.FindControl("gvUsedItem");
            U.BindGrid(gvUsedItem, o.Items);
            gvUsedItem.Columns[4].Visible = false;
            gvUsedItem.Columns[5].Visible = false;
            if (o.Items != null)
            {
                if (o.Items.Exists(a => ((WorkOrderWorkUpdateItem)a).ReturQty != 0))
                    gvUsedItem.Columns[4].Visible = true;
                if (o.Items.Exists(a => ((WorkOrderWorkUpdateItem)a).DisposalQty != 0))
                    gvUsedItem.Columns[5].Visible = true;
            }            

            GridView gvAttachmentWorkUpdate = (GridView)e.Row.FindControl("gvAttachmentWorkUpdate");            
            U.BindGrid(gvAttachmentWorkUpdate, o.Attachs);

            #region Retur
            if (o.WorkType.Is(CNT.WorkOrder.WorkType.Retur))
            {
                StockOutReturUsed soru = StockOutReturUsed.GetByWorkOrderWorkUpdateId(o.Id);
                if (soru.Id.IsNotZero())
                {
                    cb_IsChecked.Visible = false;
                    ddl_WorkType.Enabled = false;
                    btnLookupWO.Visible = false;
                }                    
            }
            #endregion
        }
    }
    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_Seq = (Literal)e.Row.FindControl("ltrl_Seq");
            ltrl_Seq.Text = (e.Row.RowIndex + 1).ToString();

            Button btnDownload = (Button)e.Row.FindControl("btnDownload");
            LinkButton lb_FileName = (LinkButton)e.Row.FindControl("lb_FileName");
            lb_FileName.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownload.ClientID));
        }
    }
    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvItems.ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_Seq = (Literal)e.Row.FindControl("ltrl_Seq");
            ltrl_Seq.Text = $"{e.Row.RowIndex + 1}";

            Literal ltrlItemCode = (Literal)e.Row.FindControl("ltrlItemCode");
            Literal ltrl_ItemId = (Literal)e.Row.FindControl("ltrl_ItemId");
            Literal ltrlQty = (Literal)e.Row.FindControl("ltrlQty");
            TextBox tb_Qty = (TextBox)e.Row.FindControl("tb_Qty");
            HtmlGenericControl dvCode = (HtmlGenericControl)e.Row.FindControl("dvCode");
            HtmlGenericControl dvCodeLookup = (HtmlGenericControl)e.Row.FindControl("dvCodeLookup");
            CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");                        

            Button btnItemLookup = (Button)e.Row.FindControl("btnItemLookup");
            btnItemLookup.OnClientClick = wItem.GetShowReference($"~/Pages/Inventory/SelectItem.aspx?RowIndex={e.Row.RowIndex}&ShowStock=1");
            btnItemLookup.OnClientClick += "return false;";
            
            if (ddlStatus.SelectedValue != CNT.Status.Preparation)
            {                
                ltrlItemCode.Visible = true;
                ltrlQty.Visible = true;
                dvCode.Visible = false;
                dvCodeLookup.Visible = false;
                tb_Qty.Visible = false;
                cb_IsChecked.Visible = false;
            }
            else
            {
                ltrlItemCode.Visible = false;
                ltrlQty.Visible = false;
                dvCode.Visible = true;
                dvCodeLookup.Visible = true;
                tb_Qty.Visible = true;
                cb_IsChecked.Visible = true;
            }
               
        }
    }
    protected void gvUsedItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            ImageButton imbDelWorkUpdateItem = (ImageButton)e.Row.FindControl("imbDelWorkUpdateItem");
            ltrl_No.Text = $"{e.Row.RowIndex + 1}";

            WorkOrderWorkUpdateItem wowui = (WorkOrderWorkUpdateItem)e.Row.DataItem;
            StockOutReturUsed soru = StockOutReturUsed.GetByWorkOrderWorkUpdateItemId(wowui.WorkOrderWorkUpdateId, wowui.Id);
            if (soru.Id.IsNotZero()) imbDelWorkUpdateItem.Visible = false;
        }
    }
    protected void wWo_Close(object sender, F.WindowCloseEventArgs e)
    {
        Wrapping w = (Wrapping)Session[CNT.Session.Wrapping];
        foreach (GridViewRow Row in gvWorkUpdate.Rows)
        {
            if (Row.RowIndex == w.Seq)
            {
                Literal ltrl_WorkOrderReferenceId = (Literal)Row.FindControl("ltrl_WorkOrderReferenceId");
                Literal ltrl_StockOutReturId = (Literal)Row.FindControl("ltrl_StockOutReturId");
                TextBox tb_WoCode = (TextBox)Row.FindControl("tb_WoCode");                

                if (ltrl_WorkOrderReferenceId.Text.IsNot(w.Id))
                {
                    GridView gvUsedItem = (GridView)Row.FindControl("gvUsedItem");
                    U.BindGrid(gvUsedItem, null);
                }                

                ltrl_WorkOrderReferenceId.Text = w.Id.ToText();
                ltrl_StockOutReturId.Text = w.KeyId.ToText();
                tb_WoCode.Text = w.Code;
            }
        }
        Session[CNT.Session.Wrapping] = null;
    }
    protected void wWorkUpdateItem_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrResult = e.CloseArgument.Split('~');
        string RowIndex = arrResult[0];
        string[] arrRows = arrResult[1].Split('|');
        List<object> oList = new List<object>();
        int No = 0;
        foreach (string Row in arrRows)
        {
            No += 1;
            string[] arrData = Row.Split('-');
            WorkOrderWorkUpdateItem wowui = new WorkOrderWorkUpdateItem { No = No, StockOutItemId = arrData[0].ToInt(), Qty = arrData[1].ToInt() };            
            StockOutItem soi = StockOutItem.GetById(arrData[0]);
            wowui.StockOutItemId = soi.Id;
            wowui.ItemId = soi.ItemId;
            wowui.ItemCode = soi.ItemCode;
            wowui.ItemName = soi.ItemName;
            wowui.SKU = soi.SKU;
            oList.Add(wowui);
        }
        GridViewRow gvr = gvWorkUpdate.Rows[RowIndex.ToInt()];
        GridView gvUsedItem = (GridView)gvr.FindControl("gvUsedItem");
        List<object> oListgv = U.GetGridData(gvUsedItem, typeof(WorkOrderWorkUpdateItem)).ListData;
        foreach (WorkOrderWorkUpdateItem o in oListgv)
        {
            if (!oList.Exists(a => ((WorkOrderWorkUpdateItem)a).StockOutItemId == o.StockOutItemId))
                oList.Add(o);
            else
            {
                WorkOrderWorkUpdateItem wowui = (WorkOrderWorkUpdateItem)oList.Find(a => ((WorkOrderWorkUpdateItem)a).StockOutItemId == o.StockOutItemId);
                wowui.Qty += o.Qty;
                wowui.Id = o.Id;
            }
        }        
        U.BindGrid(gvUsedItem, oList);        
    }
    protected void wWorkUpdateItemReference_Close(object sender, F.WindowCloseEventArgs e)
    {        
        List<object> oListSession = (List<object>)Session[CNT.Session.WorkUpdateItem];
        int Seq = e.CloseArgument.ToInt();
        GridViewRow gvr = gvWorkUpdate.Rows[Seq];
        DropDownList ddl_WorkType = (DropDownList)gvr.FindControl("ddl_WorkType");
        GridView gvUsedItem = (GridView)gvr.FindControl("gvUsedItem");
        List<object> oListgv = U.GetGridData(gvUsedItem, typeof(WorkOrderWorkUpdateItem)).ListData;
        foreach (WorkOrderWorkUpdateItem o in oListSession)
        {
            if (oListgv.Exists(a => ((WorkOrderWorkUpdateItem)a).ReferenceId.Is(o.ReferenceId)))
            {
                if (ddl_WorkType.SelectedValue.Is("Retur")) o.ReturQty += o.Qty;
                else o.DisposalQty += o.Qty;
            }
                
            else oListgv.Add(o);
        }                    
        U.BindGrid(gvUsedItem, oListgv);
        gvUsedItem.Columns[3].Visible = false;
        gvUsedItem.Columns[4].Visible = false;
        gvUsedItem.Columns[5].Visible = false;
        if (ddl_WorkType.SelectedValue.Is("Retur")) gvUsedItem.Columns[4].Visible = true;
        else gvUsedItem.Columns[5].Visible = true;
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
        ViewState[CNT.Username] = U.GetUsername();
        if (ViewState[CNT.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        
        tbStartDate.Attributes.Add("onchange", $"ClientChanged('{btnStartDateChanged.ClientID}');");
        fuWorkOrder.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUpload.ClientID));

        U.SetDropDownMasterData(ddlVendor, "Vendor");
        InitEdit();
        if (ddlStatus.SelectedValue == CNT.Status.Completed)
            btnSubmit.Hidden = true;
    }
    private void InitEdit()
    {
        if (U.Id.IsNull()) return;
        WorkOrder wo = WorkOrder.GetById(U.Id);
        if (wo.Id.IsZero())
        {
            U.ShowMessage("Work Order not exist");
            return;
        }
        ViewState[CNT.VS.StockOutId] = wo.StockOutId;

        #region Helpdesk / Schedule Maintenance
        tbHelpdeskCode.Value = wo.ParentCode;
        tbCode.Value = wo.Code;
        tbArea.Value = wo.Area;
        tbLocation.Value = wo.Location;
        tbCategory.Value = wo.Category;
        tbSubject.Value = wo.Subject;
        tbWorkDescription.Value = wo.WorkDescription;

        string TableName = "";
        int ParentId = 0;        
        if (wo.HelpdeskId == 0)
        {            
            U.Display("dvScheduler");
            lblParentTitle.Text = "Maintenance Schedule";
            ltrlDescTitle.Text = "Work Description";
            Schedule s = Schedule.GetById(wo.ScheduleId);
            s.OrderDate = wo.OrderDate;
            s.EarliestStartDate = wo.EarliestStartDate;
            s.LatestStartDate = wo.LatestStartDate;
            List<object> sList = new List<object> { s };
            U.BindGrid(gvSchedule, sList);
            TableName = SMTableName;
            ParentId = wo.ScheduleMaintenanceId;
        }
        else
        {
            U.Hide("dvScheduler");
            lblParentTitle.Text = "Helpdesk";
            ltrlDescTitle.Text = "Request Detail";
            TableName = HTableName;
            ParentId = wo.HelpdeskId;
        }
        #endregion        

        #region Helpdesk / Schedule Maintenance Attachment
        List<object> aList = Attachment.GetByOwnerID(TableName, ParentId);
        if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
        foreach (Attachment att in aList)
            U.SaveAttachmentToTempFolder(att);
        U.BindGrid(gvScheduleAttachment, aList);        
        #endregion

        #region Work Order
        if (wo.StartDate != DateTime.MinValue) tbStartDate.Value = wo.StartDate.ToString("yyyy-MM-dd HH:mm");

        #region Work Duration
        Subject sbj = Subject.GetById(wo.SubjectId);
        ViewState[CNT.VS.UseItem] = sbj.UseItem;
        ViewState[CNT.VS.UsePhoto] = sbj.UsePhoto;
        ViewState[CNT.VS.MeterCondition] = sbj.MeterCondition;
        if (ViewState[CNT.VS.MeterCondition].ToBool())
        {
            ddlSealMeterCondition.SelectedValue = wo.SealMeterCondition;
            ddlBodyMeterCondition.SelectedValue = wo.BodyMeterCondition;
            ddlCoverMeterCondition.SelectedValue = wo.CoverMeterCondition;
            ddlGlassMeterCondition.SelectedValue = wo.GlassMeterCondition;
            ddlMachiveMeterCondition.SelectedValue = wo.MachineMeterCondition;
        }
        U.Hide(CNT.DV.WorkOrder.Customer);
        if (sbj.NeedCustNo)
        {
            U.Display(CNT.DV.WorkOrder.Customer);
            Customer cust = BaseModel.GetById<Customer>(wo.CustomerId);
            tbCustomerNo.Value = cust.CustNo;
            tbCustomer.Value = cust.Name;
        }            
        if (sbj.WorkDuration != 0)
        {
            tbWorkDuration.Attributes.Add("readonly", "true");
            ddlWorkDurationType.Enabled = false;
            tbWorkDuration.Value = $"{sbj.WorkDuration}";
            ddlWorkDurationType.SelectedValue = sbj.WorkDurationType;
        }
        else if (wo.WorkDuration != 0)
        {
            tbWorkDuration.Value = $"{wo.WorkDuration}";
            ddlWorkDurationType.SelectedValue = wo.WorkDurationType;
            tbTargetCompletion.Value = wo.TargetCompletion.ToString("yyyy-MM-dd");
        }
        TargetCompletionChanged();
        if (ViewState[CNT.VS.MeterCondition].ToBool())
        {
            wo.SealMeterCondition = ddlSealMeterCondition.SelectedValue;
            wo.SealMeterCondition = ddlBodyMeterCondition.SelectedValue;
            wo.SealMeterCondition = ddlCoverMeterCondition.SelectedValue;
            wo.SealMeterCondition = ddlGlassMeterCondition.SelectedValue;
            wo.SealMeterCondition = ddlMachiveMeterCondition.SelectedValue;
        }
        #endregion

        #region Operator
        ddlOperatorType.SelectedValue = wo.OperatorType;
        OperatorTypeChanged();
        if (wo.VendorId != 0)
        {
            ddlVendor.SelectedValue = $"{wo.VendorId}";
            VendorChanged();
            ddlOperator.SelectedValue = $"{wo.OperatorsId}";
        }
        else ddlOperator.SelectedValue = $"{wo.OperatorUserId}";
        #endregion
        
        ddlResult.SelectedValue = wo.Result;
        tbRemarks.Value = wo.Remarks;
        if (wo.CloseDate != DateTime.MinValue) tbCloseDate.Value = wo.CloseDate.ToString("yyyy-MM-dd HH:mm");
        if (!wo.ActualAchievement.IsEmpty()) tbActualAchievement.Value = wo.ActualAchievement;
        ddlAchievement.SelectedValue = wo.Achievement;
        #endregion

        #region Work Update
        List<object> wowuList = WorkOrderWorkUpdate.GetByWorkOrderId(wo.Id);
        if (wowuList.Count == 0) wowuList.Add(new WorkOrderWorkUpdate { StockOutId = wo.StockOutId });
        else
        {
            foreach (WorkOrderWorkUpdate wowu in wowuList)
            {
                wowu.Items = WorkOrderWorkUpdateItem.GetByWorkOrderWorkUpdateId(wowu.Id);
                //if (wowu.Items.Count != 0) sbj.UseItem = true;
                wowu.Attachs = Attachment.GetByOwnerID("WorkOrderWorkUpdateAttachment", wowu.Id);
                foreach (Attachment att in wowu.Attachs)
                    U.SaveAttachmentToTempFolder(att);
            }
        }        
        U.BindGrid(gvWorkUpdate, wowuList);
        gvWorkUpdate.Columns[4].Visible = false;
        gvWorkUpdate.Columns[5].Visible = false;
        if (sbj.UseItem.Is("Both") || sbj.UseItem.Is("Yes"))
            gvWorkUpdate.Columns[4].Visible = true;
         if (sbj.UsePhoto)
            gvWorkUpdate.Columns[5].Visible = true;
        #endregion

        #region Work Order Item
        bool IsItemEmpty = false;
        List<object> woItemList = WorkOrderItem.GetByWorkOrderId(wo.Id);
        if (woItemList.Count == 0)
        {
            IsItemEmpty = true;
            woItemList.Add(new WorkOrderItem { });
        }
        else ViewState[CNT.VS.ItemExist] = true;
        U.BindGrid(gvItems, woItemList);
        SettingStatus(wo.Status);
        #endregion

        #region Work Order Attachment
        List<object> woaList = Attachment.GetByOwnerID(WOTableName, wo.Id);
        if (woaList.Count == 0) woaList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
        foreach (Attachment att in woaList)
            U.SaveAttachmentToTempFolder(att);
        U.BindGrid(gvWOAttachment, woaList);
        #endregion

        if (U.DeviationAccess("WorkOrder") && ddlStatus.SelectedValue == "Completed")
        {
            pnlAchievement.Visible = true;
            btnSubmitAchievement.Hidden = false;
        }            
        if (U.IsMember(CNT.Operator))
        {
            if (!(U.IsMember(CNT.SuperAdmin) || U.IsMember(CNT.Admin)))
            {
                ddlOperatorType.Enabled = false;
                ddlVendor.Enabled = false;
                ddlOperator.Enabled = false;
            }
        }
        if (wo.OperatorType.IsEmpty() || ((ViewState[CNT.VS.UseItem].Is("Yes")) && IsItemEmpty))
        {
            U.Hide("dvProgress");
            return;
        }        
        if ((ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes")) && !IsItemEmpty)
        {
            StockOut so = StockOut.GetByWorkOrderId(U.Id);
            if (so.Id == 0)
            {
                so = StockOut.GetByHelpdeskId(wo.HelpdeskId);
                if (so.Id.IsZero())
                    U.Hide("dvProgress");
            }           
        }
    }    
    private void SettingStatus(string Status)
    {        
        ddlOperatorType.Enabled = false;
        ddlVendor.Enabled = false;
        ddlOperator.Enabled = false;
        ddlStatus.Items.Clear();        
        switch (Status)
        {
            case CNT.Status.Assignment:
                ddlOperatorType.Enabled = true;
                ddlOperator.Enabled = true;
                ddlStatus.Items.Add(CNT.Status.Assignment);
                ddlStatus.Items.Add(CNT.Status.Preparation);                
                break;
            case CNT.Status.Preparation:
                ddlStatus.Items.Add(CNT.Status.Preparation);                
                if (!(ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes"))) ddlStatus.Items.Add(CNT.Status.Started);
                else if ((ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes")) && ViewState[CNT.VS.ItemExist].ToBool()) ddlStatus.Items.Add(CNT.Status.StockOut);
                break;
            case CNT.Status.StockOut:
                ddlStatus.Items.Add(CNT.Status.StockOut);
                ddlStatus.Items.Add(CNT.Status.ItemReceived);
                break;
            case CNT.Status.ItemReceived:
                ddlStatus.Items.Add(CNT.Status.ItemReceived);
                ddlStatus.Items.Add(CNT.Status.Started);                
                break;
            case CNT.Status.Started:
                ddlStatus.Items.Add(CNT.Status.Started);
                ddlStatus.Items.Add(CNT.Status.Inprogress);                
                break;
            case CNT.Status.Inprogress:
                ddlStatus.Items.Add(CNT.Status.Inprogress);
                ddlStatus.Items.Add(CNT.Status.Completed);
                break;
            case CNT.Status.Completed:
                ddlStatus.Items.Add(CNT.Status.Completed);
                break;
        }
        ddlStatus.Items.Add(CNT.Status.Pending);
        ddlStatus.Items.Add(CNT.Status.Cancel);
        ViewState[CNT.VS.PreviousStatus] = Status;
        ddlStatus.SelectedValue = Status;
        StatusChanged();
    }
    
    #region Work Update
    private void AddWorkUpdate()
    {
        Wrapping w = new Wrapping();
        List<object> wowuList = GetGridDataWorkUpdate();
        WorkUpdateValidation(w, wowuList);
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }
        wowuList.Add(new WorkOrderWorkUpdate { StockOutId = ViewState[CNT.VS.StockOutId].ToInt() });        
        U.BindGrid(gvWorkUpdate, wowuList);
    }
    private void DeleteWorkUpdate()
    {        
        List<object> oList = GetGridDataWorkUpdate();
        if (!oList.Exists(a => ((WorkOrderWorkUpdate)a).IsChecked))
        {
            U.ShowMessage("Please select Work Update you want to delete");
            return;
        }
        oList.RemoveAll(a => ((WorkOrderWorkUpdate)a).IsChecked);
        
        if (oList.Count == 0) U.BindGrid(gvWorkUpdate, new List<object> { new WorkOrderWorkUpdate { StockOutId = ViewState[CNT.VS.StockOutId].ToInt() } });
        else U.BindGrid(gvWorkUpdate, oList);
    }
    private void DeleteWorkUpdateItem(ImageButton imb)
    {
        GridViewRow row = (GridViewRow)imb.Parent.Parent;
        GridView gvUsedItem = (GridView)row.Parent.Parent;
        List<object> oList = U.GetGridData(gvUsedItem, typeof(WorkOrderWorkUpdateItem)).ListData;
        oList.RemoveAt(row.RowIndex);
        U.BindGrid(gvUsedItem, oList);        
    }
    private List<object> GetGridDataWorkUpdate()
    {
        List<object> oList = new List<object>();
        foreach (GridViewRow Row in gvWorkUpdate.Rows)
        {
            Literal ltrl_Id = (Literal)Row.FindControl("ltrl_Id");
            Literal ltrl_WorkOrderId = (Literal)Row.FindControl("ltrl_WorkOrderId");
            Literal ltrl_WorkOrderReferenceId = (Literal)Row.FindControl("ltrl_WorkOrderReferenceId");
            Literal ltrl_StockOutId = (Literal)Row.FindControl("ltrl_StockOutId");
            Literal ltrl_StockOutReturId = (Literal)Row.FindControl("ltrl_StockOutReturId");
            Literal ltrl_No = (Literal)Row.FindControl("ltrl_No");
            CheckBox cb_IsChecked = (CheckBox)Row.FindControl("cb_IsChecked");
            HtmlInputGenericControl tbDate = (HtmlInputGenericControl)Row.FindControl("tbDate");
            DropDownList ddl_WorkType = (DropDownList)Row.FindControl("ddl_WorkType");
            TextBox tb_WoCode = (TextBox)Row.FindControl("tb_WoCode");
            TextBox tbWorkDetail = (TextBox)Row.FindControl("tbWorkDetail");
            GridView gvUsedItem = (GridView)Row.FindControl("gvUsedItem");
            GridView gvAttachmentWorkUpdate = (GridView)Row.FindControl("gvAttachmentWorkUpdate");

            WorkOrderWorkUpdate wowu = new WorkOrderWorkUpdate { Id = ltrl_Id.Text.ToInt(), WorkOrderId = ltrl_WorkOrderId.Text.ToInt(), WorkOrderReferenceId = ltrl_WorkOrderReferenceId.Text.ToInt(), StockOutId = ltrl_StockOutId.Text.ToInt(), StockOutReturId = ltrl_StockOutReturId.Text.ToInt(), No = ltrl_No.Text.ToInt(), IsChecked = cb_IsChecked.Checked, WorkDetail = tbWorkDetail.Text, WorkType = ddl_WorkType.SelectedValue, WoCode = tb_WoCode.Text };
            if (!tbDate.Value.IsEmpty()) wowu.Date = tbDate.Value.ToHTML5DateTime();
            wowu.Items = U.GetGridData(gvUsedItem, typeof(WorkOrderWorkUpdateItem)).ListData;
            wowu.Attachs = Utility.GetGridData(gvAttachmentWorkUpdate, typeof(Attachment)).ListData;
            oList.Add(wowu);
        }
        return oList;
    }
    private void ItemLookup(Button btn)
    {
        GridViewRow Row = (GridViewRow)btn.Parent.Parent.Parent;
        DropDownList ddl_WorkType = (DropDownList)btn.FindControl("ddl_WorkType");
        Session[CNT.Session.WorkUpdate] = GetGridDataWorkUpdate();

        if (ddl_WorkType.SelectedValue.Is(CNT.Installation))
        {
            Literal ltrl_StockOutReturId = (Literal)btn.FindControl("ltrl_StockOutReturId");
            string StockOutId = ddl_WorkType.SelectedValue == CNT.Installation ? ViewState[CNT.VS.StockOutId].ToText() : ltrl_StockOutReturId.Text;
            if ((ddl_WorkType.SelectedValue == CNT.Retur || ddl_WorkType.SelectedValue == CNT.Disposal) && ltrl_StockOutReturId.Text.IsZero())
            {
                U.ShowMessage($"Work Order {ddl_WorkType.SelectedValue} is required");
                return;
            }
            
            U.ExecClientScript(wWorkUpdateItem.GetShowReference($"~/Pages/WorkOrder/SelectSOItem.aspx?StockOutId={StockOutId}&RowIndex={Row.RowIndex}&WorkType={ddl_WorkType.SelectedValue}&WorkOrderId={U.Id}"));
            
        }
        else
        {
            Literal ltrl_WorkOrderReferenceId = (Literal)btn.FindControl("ltrl_WorkOrderReferenceId");
            U.ExecClientScript(wWorkUpdateItemReference.GetShowReference($"~/Pages/WorkOrder/SelectWOItem.aspx?WorkOrderId={ltrl_WorkOrderReferenceId.Text}&Seq={Row.RowIndex}&WorkType={ddl_WorkType.SelectedValue}"));
        }
        StatusChanged();
    }
    private void SetGPS(GridView gvAttachmentWorkUpdate)
    {
        GridViewRow Row = (GridViewRow)gvAttachmentWorkUpdate.Rows[gvAttachmentWorkUpdate.Rows.Count - 1];
        Label lblLatitude = (Label)Row.FindControl("lblLatitude");
        Label lblLongitude = (Label)Row.FindControl("lblLongitude");
        HiddenField hf_Latitude = (HiddenField)Row.FindControl("hf_Latitude");
        HiddenField hf_Longitude = (HiddenField)Row.FindControl("hf_Longitude");
        U.ExecClientScript($"getLocation({lblLatitude.ClientID}, {lblLongitude.ClientID}, {hf_Latitude.ClientID}, {hf_Longitude.ClientID});");
    }
    #endregion

    #region Items
    private void AddItem()
    {
        Wrapping w = new Wrapping();
        ItemValidation(w);
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }

        w.ListData.Add(new WorkOrderItem { });
        U.BindGrid(gvItems, w.ListData);
    }
    private void DeleteItem()
    {
        List<object> oList = U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData;
        if (!oList.Exists(a => ((WorkOrderItem)a).IsChecked))
        {
            U.ShowMessage("Please select Item you want to delete");
            return;
        }

        oList.RemoveAll(a => ((WorkOrderItem)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvItems, new List<object> { new WorkOrderItem { } });
        else U.BindGrid(gvItems, oList);
    }
    private void ItemValidation(Wrapping w)
    {
        w.ListData = U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData;
        int idx = 0;
        foreach (WorkOrderItem o in w.ListData)
        {
            idx += 1;
            if (o.ItemId == 0) w.ErrorValidation = $"Item is required at row {idx}";
            if (o.Qty == 0) w.ErrorValidation = $"Qty is required at row {idx}";
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
    private void DownloadFile(F.Button btn, string lbFileName)
    {
        GridViewRow Row = (GridViewRow)btn.Parent.Parent;
        LinkButton lb = (LinkButton)Row.FindControl(lbFileName);
        Literal ltrl_FileNameUniq = (Literal)Row.FindControl("ltrl_FileNameUniq");
        if (ltrl_FileNameUniq.Text == "") return;
        byte[] Data = U.GetFile(ltrl_FileNameUniq.Text);
        U.OpenFile(lb.Text, U.GetContentType(lb.Text), Data);
    }    
    private void AddAttachment(FileUpload fu, GridView gv)
    {
        if (!fu.HasFile)
        {
            U.ShowMessage("File is required");
            return;
        }

        Attachment att = new Attachment();
        List<object> attList = U.GetGridData(gv, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).FileName != CNT.DataNotAvailable);
        attList.RemoveAll(a => ((Attachment)a).Seq == 0);
        att.Seq = attList.Count == 0 ? 1 : attList.Count + 1;
        att.FileName = fu.FileName;
        att.FileNameUniq = $"{ViewState[CNT.Username]}_{fu.FileName}";
        attList.Add(att);
        string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";
        File.WriteAllBytes(Path, fu.FileBytes);
        Utility.BindGrid(gv, attList);
    }
    private void DeleteAttachment(ImageButton imb)
    {
        try
        {
            GridViewRow row = (GridViewRow)imb.Parent.Parent;
            Literal ltrl_Seq = (Literal)row.FindControl("ltrl_Seq");
            GridView gv = (GridView)row.Parent.Parent;

            List<object> aList = U.GetGridData(gv, typeof(Attachment)).ListData;
            Attachment att = (Attachment)aList[row.RowIndex];
            string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";            
            if (File.Exists(Path))
            {
                File.SetAttributes(Path, FileAttributes.Normal);
                File.Delete(Path);
            }
            aList.RemoveAll(a => ((Attachment)a).Seq.ToString() == ltrl_Seq.Text);
            if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
            Utility.BindGrid(gv, aList);
        }
        catch (Exception ex)
        {
            U.ShowMessage(ex.Message);
        }        
    }
    #endregion

    private void StatusChanged()
    {
        U.Hide("dvProgress");
        U.Hide("dvCompletion");
        U.Hide("dvWorkUpdate");
        U.Hide("dvItem");
        U.Hide("dvModifiedItem");
        U.Hide(CNT.DV.WorkOrder.MeterCondition);
        U.Hide("dvAttachment");
        tbStartDate.Disabled = true;
        tbWorkDuration.Disabled = true;
        switch (ddlStatus.SelectedValue)
        {
            case CNT.Status.Assignment:                
                break;
            case CNT.Status.Preparation:                
                if (ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes"))
                {
                    U.Display("dvModifiedItem");
                    U.Display("dvItem");
                }
                    
                break;
            case CNT.Status.StockOut:
            case CNT.Status.ItemReceived:                
                if (ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes"))
                    U.Display("dvItem");               
                break;
            case CNT.Status.Started:
                U.Display("dvProgress");
                tbStartDate.Disabled = false;
                tbWorkDuration.Disabled = false;
                if (ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes"))
                    U.Display("dvItem");
                break;
            case CNT.Status.Inprogress:
                U.Display("dvProgress");
                if (ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes"))
                    U.Display("dvItem");
                U.Display("dvWorkUpdate");
                U.Display("dvAttachment");
                break;
            case CNT.Status.Completed:
                U.Display("dvProgress");
                if (ViewState[CNT.VS.UseItem].Is("Both") || ViewState[CNT.VS.UseItem].Is("Yes"))
                    U.Display("dvItem");
                U.Display("dvWorkUpdate");
                U.Display("dvCompletion");
                if (ViewState[CNT.VS.MeterCondition].ToBool()) U.Display(CNT.DV.WorkOrder.MeterCondition);
                U.Display("dvAttachment");
                break;
            case CNT.Status.Pending:
                break;
            case CNT.Status.Cancel:
                break;
        }        
        U.BindGrid(gvItems, U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData);
        U.Hide("dvVendor");
        if (ddlOperatorType.SelectedValue == "External")
            U.Display("dvVendor");
    }
    private void TargetCompletionChanged()
    {
        if (tbStartDate.Value.IsEmpty() || tbWorkDuration.Value.IsEmpty() || ddlWorkDurationType.SelectedValue.IsEmpty()) return;
        DateTime dtRequested = tbStartDate.Value.ToHTML5DateTime();
        if (ddlWorkDurationType.SelectedValue == "Hour") dtRequested = dtRequested.AddHours(tbWorkDuration.Value.ToInt());
        else if (ddlWorkDurationType.SelectedValue == "Day") dtRequested = dtRequested.AddDays(tbWorkDuration.Value.ToInt());
        else if (ddlWorkDurationType.SelectedValue == "Month") dtRequested = dtRequested.AddMonths(tbWorkDuration.Value.ToInt());
        else if (ddlWorkDurationType.SelectedValue == "Year") dtRequested = dtRequested.AddYears(tbWorkDuration.Value.ToInt());
        tbTargetCompletion.Value = dtRequested.ToString("yyyy-MM-dd HH:mm");
        if (ddlStatus.SelectedValue.IsEmpty()) ddlStatus.SelectedValue = "Started";
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
    private void VendorChanged()
    {
        ddlOperator.Items.Clear();
        if (ddlVendor.SelectedValue.IsEmpty()) return;
        U.SetOperatorsByVendor(ddlOperator, ddlVendor.SelectedValue);
    }
    private void WorkTypeChanged(DropDownList ddl)
    {
        GridViewRow Row = (GridViewRow)ddl.Parent.Parent;
        HtmlGenericControl dvWO = (HtmlGenericControl)ddl.FindControl("dvWO");
        DropDownList ddl_WorkType = (DropDownList)ddl.FindControl("ddl_WorkType");
        Literal ltrl_StockOutId = (Literal)ddl.FindControl("ltrl_StockOutId");
        Literal ltrl_StockOutReturId = (Literal)ddl.FindControl("ltrl_StockOutReturId");
        GridView gvUsedItem = (GridView)ddl.FindControl("gvUsedItem");
        HtmlGenericControl dvItemWO = (HtmlGenericControl)ddl.FindControl("dvItemWO");

        U.BindGrid(gvUsedItem, null);
        dvWO.Visible = false;
        dvItemWO.Visible = true;

        string StockOutId = ddl.SelectedValue == CNT.Installation ? ltrl_StockOutId.Text : ltrl_StockOutReturId.Text;        

        if (ddl.SelectedValue.Is(CNT.Retur) || ddl.SelectedValue.Is(CNT.Disposal))
            dvWO.Visible = true;
        if (ddl.SelectedValue.Is(CNT.NonInventory))
            dvItemWO.Visible = false;
    }

    private void Submit()
    {
        Wrapping w = new Wrapping();
        Validation(w);
        if ($"{w.Sb}" != "") U.ShowMessage($"{w.Sb}");
        else
        {
            if (ddlStatus.SelectedValue.Or("Pending,Cancel"))
            {
                bool IsUsedWorkUpdateItemExist = WorkOrderWorkUpdateItem.IsUseItemExistByWorkOrder(U.Id);
                if (U.ShowError(IsUsedWorkUpdateItemExist, "Please create retur for Stock Out Item"))
                    return;
                bool IsStockOutExist = WorkOrder.IsStockOut(U.Id);
                if (U.ShowError(IsStockOutExist, "Please delete or reverse stock out for this work order"))
                    return;
                if (U.ShowError(WorkOrder.UpdateStatus(U.Id, ddlStatus.SelectedValue).ContainErrorMessage(out string Result), Result))
                    return;
            }
            else
            {
                if (U.ShowError(Save(w).ContainErrorMessage(out string Result), Result))
                    return;
            }                
            F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
        }
    }
    private void SubmitAchievement()
    {
        if (ddlAchievement.SelectedValue.IsEmpty())
        {
            U.ShowMessage("Achievement is required");
            return;
        }
        string Result = WorkOrder.SaveAchievement(ddlAchievement.SelectedValue);
        F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
    }
    private void Validation(Wrapping w)
    {
        List<object> wowuList = GetGridDataWorkUpdate();
        List<object> woItemList = U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData.FindAll(a => ((WorkOrderItem)a).ItemCode != "");

        switch (ddlStatus.SelectedValue)
        {
            case CNT.Status.Assignment:
                if (ddlStatus.SelectedValue == CNT.Status.Assignment) w.ErrorValidation = $"Status {CNT.Status.Assignment} needs to be changed to another status";
                break;
            case CNT.Status.Preparation:
                if (ddlOperatorType.SelectedValue.IsEmpty()) w.RequiredValidation = "Operator Type";
                if (ddlOperatorType.SelectedValue == "External" && ddlVendor.SelectedValue.IsEmpty()) w.RequiredValidation = "Vendor";
                if (ddlOperator.SelectedValue.IsEmpty()) w.RequiredValidation = "Conducted By";
                
                break;
            case CNT.Status.StockOut:
                if (ViewState[CNT.VS.PreviousStatus].Is(CNT.Status.StockOut) && ddlStatus.SelectedValue == CNT.Status.StockOut) w.ErrorValidation = $"Status {CNT.Status.StockOut} needs to be changed to another status";
                break;
            case CNT.Status.ItemReceived:
                List<object> WorkOrderItems = U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData.FindAll(a => ((WorkOrderItem)a).ItemCode != "");
                if (WorkOrderItems.Count != 0)
                {
                    var wo = WorkOrder.GetById(U.Id);
                    if (wo.StockOutId.IsZero())
                        w.ErrorValidation = $"Please Stock Out Items before change status to Stock Received";                    
                }                
                break;
            case CNT.Status.Started:
                if (tbStartDate.Value.IsEmpty())
                    w.RequiredValidation = "Start Date";
                if (tbWorkDuration.Value.IsEmpty() || tbWorkDuration.Value.IsZero())
                    w.RequiredValidation = "Work Duration";
                if (ddlWorkDurationType.SelectedValue.IsEmpty())
                    w.RequiredValidation = "Work Duration Type";
                break;
            case CNT.Status.Inprogress:
                if (wowuList.Count == 0) w.RequiredValidation = "Work Update";
                else 
                    WorkUpdateValidation(w, wowuList, ddlStatus.SelectedValue);
                break;
            case CNT.Status.Completed:
                if (tbCloseDate.Value.IsEmpty()) w.RequiredValidation = "Close Date";                                
                if (ViewState[CNT.VS.MeterCondition].ToBool())
                {
                    if (ddlSealMeterCondition.SelectedValue.IsEmpty()) w.RequiredValidation = "Seal Meter Condition";
                    if (ddlBodyMeterCondition.SelectedValue.IsEmpty()) w.RequiredValidation = "Body Meter Condition";
                    if (ddlCoverMeterCondition.SelectedValue.IsEmpty()) w.RequiredValidation = "Cover Meter Condition";
                    if (ddlGlassMeterCondition.SelectedValue.IsEmpty()) w.RequiredValidation = "Glass Meter Condition";
                    if (ddlMachiveMeterCondition.SelectedValue.IsEmpty()) w.RequiredValidation = "Machine Meter Condition";
                }
                if (ddlResult.SelectedValue.IsEmpty()) w.RequiredValidation = "Result";
                if (wowuList.Count == 0) w.RequiredValidation = "Work Update";
                else WorkUpdateValidation(w, wowuList, ddlStatus.SelectedValue);                                                
                foreach (WorkOrderItem woi in woItemList)
                {                    
                    int TotalItem = wowuList.SelectMany(a => ((WorkOrderWorkUpdate)a).Items).Where(b => ((WorkOrderWorkUpdateItem)b).ItemId.Is(woi.ItemId)).Sum(c => ((WorkOrderWorkUpdateItem)c).Qty);
                    int TotalRetur = StockOutRetur.GetTotalQtyByWorkOrderItem(U.Id, woi.ItemId);

                    int UnusedQty = woi.Qty - TotalItem;
                    if (UnusedQty > 0)
                    {
                        if ((TotalItem + TotalRetur) < woi.Qty)
                            w.ErrorValidation = $"Please make a retur of {UnusedQty} unused {woi.ItemName} items";
                    }                        
                }                
                break;
            case CNT.Status.Pending:
                break;
            case CNT.Status.Cancel:
                break;
        }                
    }
    private void WorkUpdateValidation(Wrapping w, List<object> wowuList, string Status = "")
    {
        int row = 0;
        foreach (WorkOrderWorkUpdate wowu in wowuList)
        {
            row +=1;
            if (wowu.Date == DateTime.MinValue) w.RequiredValidation = $"Date Work Update at row {row}";
            if ((wowu.WorkType == CNT.Retur || wowu.WorkType == CNT.Disposal) && wowu.WorkOrderReferenceId.IsZero()) w.ErrorValidation = $"Work Order {wowu.WorkType} at row {row} is required if Work Type is {wowu.WorkType}";
            if (wowu.WorkDetail.Trim().IsEmpty()) w.RequiredValidation = $"Work Detail Work Update at row {row}";
            if (ViewState[CNT.VS.UseItem].Is("Yes") && wowu.Items.Count == 0) w.RequiredValidation = $"Item Work Update at row {row}";
            if (ViewState[CNT.VS.UsePhoto].ToBool() && wowu.Attachs.Count == 0) w.RequiredValidation = $"Photo Work Update at row {row}";
            if (Status.Is(CNT.Status.Completed))
            {
                if (wowu.WorkType == CNT.Retur)
                {
                    foreach (WorkOrderWorkUpdateItem wowui in wowu.Items)
                    {
                        StockOutReturUsed Soru = StockOutReturUsed.GetByWorkOrderWorkUpdateItemId(wowui.Id);
                        if (Soru.Id.IsZero()) w.ErrorValidation = $"Please create Retur for Used Item {wowui.ItemName}";
                    }                   
                }                
            }
        }
    }
    
    private string Save(Wrapping w)
    {
        #region Check is Attachment file exist
        string Result = U.AttachmentValidation(gvWOAttachment, out List<object> attList);
        if (Result.ContainErrorMessage()) return Result;

        List<object> wowuList = GetGridDataWorkUpdate();
        foreach (WorkOrderWorkUpdate wowu in wowuList)
        {
            foreach (Attachment att in wowu.Attachs)
            {
                try { att.Data = U.GetFile(att.FileNameUniq); }
                catch { return "Error Message : Your page is expired please reload with klik Button Refresh"; }
            }
        }
        #endregion

        #region Work Order
        WorkOrder o = WorkOrder.GetById(U.Id);
        o.Items = U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData.FindAll(a => ((WorkOrderItem)a).ItemCode != "");
        o.Used = IsChangedExist(o);
        o.OperatorType = ddlOperatorType.SelectedValue;
        if (o.OperatorType == "External")
        {            
            o.VendorId = ddlVendor.SelectedValue.ToInt();
            o.OperatorsId = ddlOperator.SelectedValue.ToInt();
        }
        else o.OperatorUserId = ddlOperator.SelectedValue.ToInt();
        if (!tbStartDate.Value.IsEmpty()) o.StartDate = tbStartDate.Value.ToHTML5DateTime();
        if (!tbWorkDescription.Value.IsEmpty()) o.WorkDuration = tbWorkDuration.Value.ToInt();
        if (!ddlWorkDurationType.SelectedValue.IsEmpty()) o.WorkDurationType = ddlWorkDurationType.SelectedValue;
        if (!tbTargetCompletion.Value.IsEmpty() && tbTargetCompletion.Value != "Auto Generated by System") o.TargetCompletion = tbTargetCompletion.Value.ToHTML5DateTime();
        if (!ddlStatus.SelectedValue.IsEmpty()) o.Status = ddlStatus.SelectedValue;
        if (!ddlResult.SelectedValue.IsEmpty()) o.Result = ddlResult.SelectedValue;
        if (!tbRemarks.Value.IsEmpty()) o.Remarks = tbRemarks.Value;
        if (!tbCloseDate.Value.IsEmpty())
        {
            o.CloseDate = tbCloseDate.Value.ToHTML5DateTime();
            if (pnlAchievement.Visible)
            {
                o.ActualAchievement = ddlAchievement.SelectedValue;
                o.Achievement = ddlAchievement.SelectedValue;
            }                
            else
            {
                if (o.ActualAchievement.IsEmpty())
                {
                    if (lblParentTitle.Text == "Helpdesk")
                    {
                        if (o.CloseDate <= o.TargetCompletion)
                        {
                            o.ActualAchievement = "Achieved";
                            o.Achievement = "Achieved";
                        }
                        else
                        {
                            o.ActualAchievement = "Not Achieved";
                            o.Achievement = "Not Achieved";
                        }
                    }
                    else
                    {
                        if (o.StartDate >= o.EarliestStartDate && o.StartDate <= o.LatestStartDate && o.CloseDate <= o.TargetCompletion)
                        {
                            o.ActualAchievement = "Achieved";
                            o.Achievement = "Achieved";
                        }
                        else
                        {
                            o.ActualAchievement = "Not Achieved";
                            o.Achievement = "Not Achieved";
                        }
                    }
                }
            }            
        }

        if (ViewState[CNT.VS.MeterCondition].ToBool() && ddlStatus.SelectedValue.Is(CNT.Status.Completed))
        {
            o.SealMeterCondition = ddlSealMeterCondition.SelectedValue;
            o.BodyMeterCondition = ddlBodyMeterCondition.SelectedValue;
            o.CoverMeterCondition = ddlCoverMeterCondition.SelectedValue;
            o.GlassMeterCondition = ddlGlassMeterCondition.SelectedValue;
            o.MachineMeterCondition = ddlMachiveMeterCondition.SelectedValue;
        }
        
        o.ModifiedBy = $"{ViewState[CNT.Username]}";
        Result = o.Update();
        if (Result.ContainErrorMessage()) return Result;
        #endregion

        #region Attachment
        string TableName = "WorkOrderAttachment";
        Result = Attachment.DeleteByOwnerId(o.Id, TableName);
        if (Result.Contains("Error Message :")) return Result;

        foreach (Attachment att in attList)
        {            
            att.Table = TableName;
            att.OwnerId = o.Id;
            att.Data = U.GetFile(att.FileNameUniq);
            att.CreatedBy = $"{ViewState[CNT.Username]}";
            Result = att.Insert(att);
            if (Result.Contains("Error Message :")) return Result;

            string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";
            //File.Delete(Path);
        }
        #endregion

        #region Item 
        if (o.Status.Is(CNT.Status.Preparation))
        {
            List<object> iDBList = WorkOrderItem.GetByWorkOrderId(o.Id);
            foreach (WorkOrderItem iDB in iDBList)
            {
                if (!o.Items.Exists(a => ((WorkOrderItem)a).Id == iDB.Id))
                {
                    Result = iDB.Delete();
                    if (Result.ContainErrorMessage()) return Result;
                }
            }
            foreach (WorkOrderItem i in o.Items)
            {
                i.WorkOrderId = o.Id;
                if (i.Id == 0)
                {
                    i.CreatedBy = $"{ViewState[CNT.Username]}";
                    Result = i.Insert();
                    if (Result.ContainErrorMessage()) return Result;
                }
                else
                {
                    i.ModifiedBy = $"{ViewState[CNT.Username]}";
                    Result = i.Update();
                    if (Result.ContainErrorMessage()) return Result;
                }
            }
        }        
        #endregion

        #region Work Update
        if (ddlStatus.SelectedValue.Is(CNT.Status.Started) || ddlStatus.SelectedValue == CNT.Status.Inprogress || ddlStatus.SelectedValue == CNT.Status.Completed)
        {
            if (ddlStatus.SelectedValue.Is(CNT.Status.Started)) wowuList = new List<object>();
            #region Delete Work Update From Database if Not Exist in Form and Reverse Stock Out and Work Order Qty
            List<object> wowuDBList = WorkOrderWorkUpdate.GetByWorkOrderId(o.Id);
            foreach (WorkOrderWorkUpdate wowu in wowuDBList)
            {
                if (!wowuList.Exists(a => ((WorkOrderWorkUpdate)a).Id == wowu.Id))
                {                    
                    List<object> wowuiDBList = WorkOrderWorkUpdateItem.GetByWorkOrderWorkUpdateId(wowu.Id);
                    Result = wowu.Delete();
                    if (Result.ContainErrorMessage()) return Result;

                    foreach (WorkOrderWorkUpdateItem wui in wowuiDBList)
                    {
                        StockOutItem soi = StockOutItem.GetById(wui.StockOutItemId);
                        WorkOrderWorkUpdateItem wuiReference = WorkOrderWorkUpdateItem.GetById(wui.ReferenceId);                       

                        Result = ReverseStockOutItemAndWorkUpdateItem(wowu.WorkType, soi, wui, wuiReference);
                        if (Result.IsNotEmpty()) return Result;
                    }
                }
            }
            #endregion

            foreach (WorkOrderWorkUpdate wowu in wowuList)
            {
                #region Work Update                
                wowu.WorkOrderId = o.Id;
                if (wowu.Id == 0)
                {
                    wowu.CreatedBy = $"{ViewState[CNT.Username]}";
                    Result = wowu.Insert();
                    if (Result.ContainErrorMessage()) return Result;
                    wowu.Id = Result.ToInt();
                }
                else
                {
                    wowu.ModifiedBy = $"{ViewState[CNT.Username]}";
                    Result = wowu.Update();
                    if (Result.ContainErrorMessage()) return Result;
                }
                #endregion

                #region Work Update Item   
                #region Delete Work Update Item from Database if Not Exist in Form and Reverse Stock Out and Work Order Qty
                List<object> wowuiDBList = WorkOrderWorkUpdateItem.GetByWorkOrderWorkUpdateId(wowu.Id);
                foreach (WorkOrderWorkUpdateItem wui in wowuiDBList)
                {
                    if (!wowu.Items.Exists(a => ((WorkOrderWorkUpdateItem)a).Id == wui.Id))
                    {
                        Result = wui.Delete();
                        if (Result.ContainErrorMessage()) return Result;

                        StockOutItem soi = StockOutItem.GetById(wui.StockOutItemId);
                        WorkOrderWorkUpdateItem wuiReference = WorkOrderWorkUpdateItem.GetById(wui.ReferenceId);

                        Result = ReverseStockOutItemAndWorkUpdateItem(wowu.WorkType, soi, wui, wuiReference);
                        if (Result.IsNotEmpty()) return Result;
                    }
                }
                #endregion

                foreach (WorkOrderWorkUpdateItem wui in wowu.Items)
                {
                    wui.WorkOrderWorkUpdateId = wowu.Id;
                    StockOutItem soi = StockOutItem.GetById(wui.StockOutItemId);
                    if (soi.HelpdeskId.IsNotZero() && soi.UseSKU)
                        soi.WorkOrderId = o.Id;
                    else soi.WorkOrderId = 0;
                    WorkOrderWorkUpdateItem wuiReference = WorkOrderWorkUpdateItem.GetById(wui.ReferenceId);
                    #region Insert
                    if (wui.Id == 0)
                    {
                        wui.CreatedBy = $"{ViewState[CNT.Username]}";
                        Result = wui.Insert();
                        if (Result.ContainErrorMessage()) return Result;
                        wui.Id = Result.ToInt();

                        Result = UpdateStockOutItemAndWorkUpdateItem(wowu.WorkType, soi, wui, wuiReference);
                        if (Result.IsNotEmpty()) return Result;
                    }
                    #endregion
                    #region Update
                    else
                    {                        
                        WorkOrderWorkUpdateItem wowuiDB = WorkOrderWorkUpdateItem.GetById(wui.Id);
                        Result = ReverseStockOutItemAndWorkUpdateItem(wowu.WorkType, soi, wowuiDB, wuiReference);
                        if (Result.ContainErrorMessage()) return Result;

                        Result = UpdateStockOutItemAndWorkUpdateItem(wowu.WorkType, soi, wui, wuiReference);
                        if (Result.ContainErrorMessage()) return Result;

                        wui.ModifiedBy = $"{ViewState[CNT.Username]}";
                        Result = wui.Update();
                        if (Result.ContainErrorMessage()) return Result;

                    }
                    #endregion                                      
                }
                #endregion

                #region Attachment                
                Result = Attachment.DeleteByOwnerId(wowu.Id, TableWorkOrderWorkUpdateAttachment);
                if (Result.Contains("Error Message :")) return Result;

                foreach (Attachment att in wowu.Attachs)
                {
                    att.Table = TableWorkOrderWorkUpdateAttachment;
                    att.OwnerId = wowu.Id;
                    att.Data = U.GetFile(att.FileNameUniq);
                    att.CreatedBy = $"{ViewState[CNT.Username]}";                    
                    Result = att.Insert(att);
                    if (Result.Contains("Error Message :")) return Result;

                    string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";
                    //File.Delete(Path);
                }
                #endregion
            }
        }
        #endregion

        return "";
    }    
    private bool IsChangedExist(WorkOrder wo)
    {
        bool Result = false;
        if (wo.OperatorType.IsNot(ddlOperatorType.SelectedValue)) return true;
        if (wo.OperatorType.Is("Internal"))
        {
            if (wo.OperatorUserId.IsNot(ddlOperator.SelectedValue)) return true;
        }
        else if (wo.OperatorType.Is("External"))
        {
            if (wo.VendorId.IsNot(ddlVendor.SelectedValue)) return true;
            if (wo.OperatorsId.IsNot(ddlOperator.SelectedValue)) return true;
        }
        if (wo.Status.IsNot(ddlStatus.SelectedValue)) return true;
        List<object> woiList = WorkOrderItem.GetByWorkOrderId(wo.Id);
        if (woiList.Count != wo.Items.Count) return true;
        foreach (WorkOrderItem woi in wo.Items)
        {
            if (!woiList.Exists(a => ((WorkOrderItem)a).Id.Is(woi.Id) && ((WorkOrderItem)a).ItemId.Is(woi.ItemId) && ((WorkOrderItem)a).Qty.Is(woi.Qty))) return true;
        }
        
        return Result;
    }
    private string ReverseStockOutItemAndWorkUpdateItem(string WorkType, StockOutItem soi, WorkOrderWorkUpdateItem wowui, WorkOrderWorkUpdateItem wowuiReference)
    {
        string Result = "";
        if (WorkType == CNT.Installation)
        {
            soi.UsedQty -= wowui.Qty;
            soi.UnusedQty += wowui.Qty;
            Result = soi.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
        }
        else if (WorkType == CNT.Retur)
        {
            wowuiReference.ReturQty -= wowui.ReturQty;
            Result = wowuiReference.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
        }
        else if (WorkType == CNT.Disposal)
        {            
            wowuiReference.DisposalQty -= wowui.DisposalQty;
            Result = wowuiReference.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
        }        
        
        return "";
    }
    private string UpdateStockOutItemAndWorkUpdateItem(string WorkType, StockOutItem soi, WorkOrderWorkUpdateItem wowui, WorkOrderWorkUpdateItem wowuiReference)
    {
        string Result = "";
        if (WorkType == CNT.Installation)
        {            
            soi.UsedQty += wowui.Qty;
            soi.UnusedQty -= wowui.Qty;
            Result = soi.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
        }
        else if (WorkType == CNT.Retur)
        {
            wowuiReference.ReturQty += wowui.ReturQty;
            Result = wowuiReference.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;                                  
        }
        else if (WorkType == CNT.Disposal)
        {
            wowuiReference.DisposalQty += wowui.DisposalQty;
            Result = wowuiReference.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
        }        

        return Result;
    }
    #endregion            
}