using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Linq;

public partial class Pages_WorkOrder_HelpdeskInput : PageBase
{
    #region Fields
    private const string vsUserName = "Username";    
    private const string TableName = "HelpdeskAttachment";    
    private const string vsNeedCust = "NeedCust";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) Initialize();
        string Script = "InitSelect2HelpdeskInput();";
        F.PageContext.RegisterStartupScript(Script);
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnRequestedChanged":
                RequestedChanged();
                break;
            case "btnUpload":
                //float mb = (fu.FileBytes.Length / 1024f) / 1024f;
                //if (mb > 10)
                //{
                //    U.ShowMessage("Max Attachment size is 100 mb");
                //    return;
                //}
                U.AddAttachment(fu, gvAttachment);                
                break;            
            case "btnDeleteCust":
                DeleteCust();
                break;
            case "btnAddConductedBy":
                AddConductedBy();
                break;
            case "btnDeleteConductedBy":
                DeleteConductedBy();
                break;
            case "btnAddItem":
                AddItem();
                break;
            case "btnDeleteItem":
                DeleteItem();
                break;
            case "btnDelete":
                Delete();
                break;
            case "btnDeleteAttachment":
                U.DeleteAttachment(gvAttachment);
                break;
            case "btnViewOrder":
                U.OpenNewTab($"View{tbCode.Value}WorkOrder", $"View {tbCode.Value} Work Order", $"getOrderWindowUrl('{U.Id}')");
                break;            
            case "btnSubmit":
                Submit();
                break;           
        }
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnDownload":
                U.DownloadFile(btn, "lb_FileName");
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
            case "ddlRequestSource":
                RequestSourceChanged();
                break;
            case "ddlOperatorTypeMultiple":
                OperatorTypeMultipleChanged(ddl);
                break;
            case "ddlVendorMultiple":
                VendorMultipleChanged(ddl);
                break;
            case "ddlOperatorType":
                OperatorTypeChanged();
                break;
            case "ddlVendor":
                VendorChanged();
                break;           
            case "ddlStatus":
                StatusChanged();
                break;
        }
    }
    protected void gvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvCustomer.ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Customer c = (Customer)e.Row.DataItem;
            CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            ltrl_No.Text = $"{e.Row.RowIndex + 1}";
            cb_IsChecked.Enabled = true;
            if (ViewState[CNT.VS.IsUsed].ToBool())
                if (c.Id.IsNotZero()) cb_IsChecked.Enabled = !c.Used;
        }
    }
    protected void gvConductedBy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvConductedBy.ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_Seq = (Literal)e.Row.FindControl("ltrl_Seq");
            DropDownList ddlOperatorTypeMultiple = (DropDownList)e.Row.FindControl("ddlOperatorTypeMultiple");
            DropDownList ddlVendorMultiple = (DropDownList)e.Row.FindControl("ddlVendorMultiple");
            DropDownList ddlOperatorMultiple = (DropDownList)e.Row.FindControl("ddlOperatorMultiple");
            ltrl_Seq.Text = (e.Row.RowIndex + 1).ToText();
            U.SetDropDownMasterData(ddlVendorMultiple, "Vendor");
            if (ViewState[CNT.VS.IsUsed].ToBool())
            {
                ddlOperatorTypeMultiple.Enabled = false;
                ddlVendorMultiple.Enabled = false;
                ddlOperatorMultiple.Enabled = false;
            }

            HelpdeskConductedBy hc = (HelpdeskConductedBy)e.Row.DataItem;
            ddlOperatorTypeMultiple.SetValue(hc.OperatorType);
            OperatorTypeMultipleChanged(ddlOperatorTypeMultiple);
            if (hc.OperatorType.Is("Internal"))
                ddlOperatorMultiple.SetValue(hc.OperatorUserId);
            else if (hc.OperatorType.Is("External"))
            {                
                if (hc.VendorId.IsNotZero())
                {
                    ddlVendorMultiple.SetValue(hc.VendorId);
                    VendorMultipleChanged(ddlVendorMultiple);
                    ddlOperatorMultiple.SetValue(hc.OperatorsId);
                }
                    
            }
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

            if (ViewState[CNT.VS.IsUsed].ToBool())
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
    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvAttachment.ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btnDownload = (Button)e.Row.FindControl("btnDownload");
            LinkButton lb_FileName = (LinkButton)e.Row.FindControl("lb_FileName");
            lb_FileName.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownload.ClientID));
        }
    }    
    protected void wCust_Close(object sender, F.WindowCloseEventArgs e)
    {
        List<object> oList = (List<object>)Session[CNT.Session.Customer];
        List<object> Customers = U.GetGridData(gvCustomer, typeof(Customer)).ListData;
        Customers.RemoveAll(a => ((Customer)a).Name.Is(CNT.DataNotAvailable));
        foreach (Customer c in oList)
        {
            c.IsChecked = false;
            if (!Customers.Exists(a => ((Customer)a).CustNo.Is(c.CustNo)))
                Customers.Add(c);
        }
        U.BindGrid(gvCustomer, Customers);
        if (Customers.Count.IsZero())
            U.BindGrid(gvCustomer, new List<object> { new Customer { No = 1, Name = CNT.DataNotAvailable } });
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

        List<object> oList = U.GetGridData(gvItems, typeof(HelpdeskItem)).ListData;
        if (oList.Exists(a => ((HelpdeskItem)a).ItemId == g.Id))
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
        ViewState[vsNeedCust] = false;
    }
    private void InitControl()
    {
        U.Hide("dvCode");
        U.Hide("dvCust");        
        U.Hide("dvVendor");
        U.Hide("dvSocialMedia");
        U.Hide("dvCompletion");
        U.Hide("dvItem");
        U.Hide(CNT.DV.Helpdesk.ConductedBy);
        U.Hide(CNT.DV.Helpdesk.ConductedByCust);
        U.SetDropDownMasterData(ddlArea, "Area");
        
        U.SetDropDownMasterData(ddlWorkOrderType, "WorkOrderType");
        U.SetDropDownMasterData(ddlCategory, "HelpdeskCategory");
        U.SetDropDownMasterData(ddlSubject, "Subject");        
        U.BindGrid(gvCustomer, new List<object> { new Customer { No = 1, Name = CNT.DataNotAvailable } });
        btnAddCust.OnClientClick = wCust.GetShowReference($"~/Pages/WorkOrder/SelectCust.aspx");
        U.BindGrid(gvConductedBy, new List<object> { new HelpdeskConductedBy { No = 1, OperatorType = "External" } });
        U.SetDropDownMasterData(ddlVendor, "Vendor");
        U.SetDropDownMasterData(ddlRequestType, "RequestType");
        U.SetDropDownMasterData(ddlRequestSource, "RequestSource");
        U.SetDropDownMasterData(ddlSocialMedia, "SocialMedia");
        U.BindGrid(gvItems, new List<object> { new HelpdeskItem { } });
        fu.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUpload.ClientID));
        U.BindGrid(gvAttachment, new List<object> { new Attachment { Seq = 0, FileName = CNT.DataNotAvailable } });
    }
    private void InitEdit()
    {
        if (U.Id.IsNull())
        {
            btnDelete.Hidden = true;
            btnViewOrder.Hidden = true;
            return;
        }
        #region Helpdesk
        Helpdesk o = Helpdesk.GetById(U.Id);
        if (o.Id.IsZero().ShowError("Helpdesk is not exist")) return;
        bool IsUsed = WorkOrder.GetTotalWorkOrderUsedByHelpdeskId(o.Id).IsNotZero();
        ViewState[CNT.VS.IsUsed] = IsUsed;
        U.Display("dvCode");        
        ddlStatus.Enabled = true;
        tbCode.Value = o.Code;

        ddlWorkOrderType.SetValue(o.WorkOrderTypeId);
        if (ddlWorkOrderType.SelectedValue.IsEmpty())
        {
            WorkOrderType wot = WorkOrderType.GetById(o.WorkOrderTypeId);
            ddlWorkOrderType.Items.Add(new ListItem(wot.Name, "0"));
            ddlWorkOrderType.SetValue("0");
        }

        ddlCategory.SetValue(o.HelpdeskCategoryId);
        if (ddlCategory.SelectedValue.IsEmpty())
        {
            MasterData mdc = MasterData.GetById(o.HelpdeskCategoryId, "HelpdeskCategory");
            ddlCategory.Items.Add(new ListItem(mdc.Name, "0"));
            ddlCategory.SetValue("0");
        }        

        ddlStatus.SelectedValue = o.Status;
        if (o.Status == "Completed")
        {
            U.Display("dvCompletion");
            tbCompletion.Value = o.Completion.ToString("yyyy-MM-dd HH:mm");
        }
        tbTargetCompletionStatus.Value = o.TargetCompletionStatus;
        ddlRequestType.SelectedValue = $"{o.RequestTypeId}";
        ddlRequestSource.SelectedValue = $"{o.RequestSourceId}";
        tbRequesterName.Value = o.RequesterName;
        tbRequesterEmail.Value = o.RequesterEmail;
        tbRequesterPhone.Value = o.RequesterPhone;
        if (ddlRequestSource.SelectedItem.Text == "Social Media")
        {
            U.Display("dvSocialMedia");
            ddlSocialMedia.SelectedValue = $"{o.SocialMediaId}";
        }
        tbRequestDetail.Value = o.RequestDetail;
        tbRequestedDateTime.Value = o.Requested.ToString("yyyy-MM-dd HH:mm");
        tbResponseStatus.Value = o.ResponseStatus;
        tbNumberCust.Value = o.CustEachOperator.ToText();
        #endregion

        #region Subject
        Subject s = Subject.GetById(o.SubjectId);
        ddlSubject.SetValue(o.SubjectId);
        if (ddlSubject.SelectedValue.IsEmpty())
        {
            ddlSubject.Items.Add(new ListItem(s.Name, "0"));
            ddlSubject.SetValue("0");
        }

        U.Display("dvLocation");
        U.Hide("dvCust");
        U.Hide("dvItem");                        
        ViewState[vsNeedCust] = s.NeedCustNo;
        if (s.NeedCustNo)
        {
            U.Hide("dvLocation");
            U.Display("dvCust");
            List<object> Customers = Customer.GetByHelpdeskId(o.Id);
            U.BindGrid(gvCustomer, Customers);
            #region ConductedBy
            U.Display(CNT.DV.Helpdesk.ConductedByCust);
            List<object> cList = HelpdeskConductedBy.GetByHelpdesk(o.Id);
            U.BindGrid(gvConductedBy, cList);
            #endregion
        }
        else
        {
            ddlArea.SelectedValue = $"{o.AreaId}";   
            tbLocation.Value = o.Location;
            hfLocationId.Text = o.LocationId.ToText();
            btnSelectLocation.OnClientClick = wLocation.GetShowReference($"~/Pages/WorkOrder/SelectLocation.aspx?Id={ddlArea.SelectedValue}");

            #region Conducted
            U.Display(CNT.DV.Helpdesk.ConductedBy);
            ddlOperatorType.SetValue(o.OperatorType);
            OperatorTypeChanged();
            if (o.VendorId != 0)
            {
                ddlVendor.SetValue(o.VendorId);
                VendorChanged();
                ddlOperator.SetValue(o.OperatorsId);
                if (ddlVendor.SelectedValue.IsEmpty())
                {
                    Vendor v = Vendor.GetById(o.VendorId);
                    ddlVendor.Items.Add(new ListItem(v.Name, "0"));
                    ddlVendor.SelectedValue = "0";
                }
                if (ddlOperator.SelectedValue.IsEmpty())
                {
                    Operators op = Operators.GetById(o.OperatorsId);
                    ddlOperator.Items.Add(new ListItem(op.Name, "0"));
                    ddlOperator.SelectedValue = "0";
                }
            }
            else
            {
                ddlOperator.SetValue(o.OperatorUserId);
                if (ddlOperator.SelectedValue.IsEmpty())
                {
                    Users u = Users.GetById(o.OperatorsId);
                    ddlOperator.Items.Add(new ListItem(u.Name, "0"));
                    ddlOperator.SelectedValue = "0";
                }
            }
            #endregion
        }
        if (s.WorkDuration != 0)
            RequestedChanged();
        if (s.UseItem.IsNot("No"))
        {
            List<object> Items = HelpdeskItem.GetByHelpdeskId(o.Id);
            if (Items.Count.IsZero()) Items.Add(new HelpdeskItem());
            U.BindGrid(gvItems, Items);
            U.Display("dvItem");
        }            
        #endregion
                
        if (IsUsed)
        {
            btnAddCust.Hidden = true;
            btnDeleteCust.Hidden = true;
            btnAddConductedBy.Hidden = true;
            btnDeleteConductedBy.Hidden = true;
            ddlWorkOrderType.Enabled = false;
            ddlArea.Enabled = false;           
            ddlCategory.Enabled = false;            
            ddlSubject.Enabled = false;    
            ddlOperatorType.Enabled = false;
            ddlOperator.Enabled = false;
            ddlVendor.Enabled = false;
            tbNumberCust.Disabled = true;
            tbRequestedDateTime.Disabled = true;
            ddlRequestType.Enabled = false;
            btnAddItem.Hidden = true;
            btnDeleteItem.Hidden = true;            
            ddlStatus.Enabled = false;
            ddlStatus.SelectedValue = WorkOrder.GetStatusByHeldesk(o.Id);
            if (s.UseItem.IsNot("No"))
                btnDelete.Hidden = true;

        }
        else if (!U.IsMember(CNT.SuperAdmin)) btnDelete.Hidden = false;

        List<object> aList = Attachment.GetByOwnerID(TableName, o.Id);
        foreach (Attachment att in aList)
            U.SaveAttachmentToTempFolder(att);
        if (IsUsed) aList.ForEach(a => { ((Attachment)a).Mode = "Edit"; });
        if (aList.Count > 0) U.BindGrid(gvAttachment, aList);
    }
    private void DeleteCust()
    {
        List<object> oList = U.GetGridData(gvCustomer, typeof(Customer)).ListData;
        if (U.ShowError(oList.FindAll(a => ((Customer)a).IsChecked).Count.IsZero(), "Please select Customer you want to delete"))
            return;
        oList.RemoveAll(a => ((Customer)a).IsChecked);
        if (oList.Count.IsZero()) oList.Add(new Customer { No = 1, Name = CNT.DataNotAvailable });
        U.BindGrid(gvCustomer, oList);
    }

    #region Control Changed
    private void AreaChanged()
    {
        btnSelectLocation.OnClientClick = "return false;";
        if (ddlArea.SelectedValue == "") return;
        btnSelectLocation.OnClientClick = wLocation.GetShowReference($"~/Pages/WorkOrder/SelectLocation.aspx?Id={ddlArea.SelectedValue}");        
    }   
    private void SubjectChanged()
    {
        U.Display("dvLocation");
        U.Hide("dvCust");
        U.Hide(CNT.DV.Helpdesk.ConductedBy);
        U.Hide(CNT.DV.Helpdesk.ConductedByCust);
        U.Hide("dvItem");
        ViewState[vsNeedCust] = false;
        if (ddlSubject.SelectedValue == "") return;
        Subject s = Subject.GetById(ddlSubject.SelectedValue);
        ViewState[vsNeedCust] = s.NeedCustNo;
        if (s.NeedCustNo)
        {
            U.Hide("dvLocation");
            U.Display("dvCust");
            U.Display(CNT.DV.Helpdesk.ConductedByCust);
        }
        else U.Display(CNT.DV.Helpdesk.ConductedBy);

        if (s.WorkDuration != 0)
            RequestedChanged();
        if (s.UseItem.IsNot("No"))
            U.Display("dvItem");
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
    private void OperatorTypeMultipleChanged(DropDownList ddl)
    {
        DropDownList ddlVendorMultiple = (DropDownList)ddl.FindControl("ddlVendorMultiple");
        DropDownList ddlOperatorMultiple = (DropDownList)ddl.FindControl("ddlOperatorMultiple");

        ddlVendorMultiple.Visible = false;
        if (ddl.SelectedValue.Is("External"))
        {
            ddlVendorMultiple.Visible = true;
            ddlVendorMultiple.SelectedValue = "";
            ddlOperatorMultiple.Items.Clear();
        }  
        else if (ddl.SelectedValue == "Internal") U.SetDropDownMasterData(ddlOperatorMultiple, "Users");
    }
    private void VendorChanged()
    {
        ddlOperator.Items.Clear();
        if (ddlVendor.SelectedValue.IsEmpty()) return;
        U.SetOperatorsByVendor(ddlOperator, ddlVendor.SelectedValue);
    }
    private void VendorMultipleChanged(DropDownList ddl)
    {        
        DropDownList ddlOperatorMultiple = (DropDownList)ddl.FindControl("ddlOperatorMultiple");

        ddlOperatorMultiple.Items.Clear();
        if (ddl.SelectedValue.IsEmpty()) return;
        U.SetOperatorsByVendor(ddlOperatorMultiple, ddl.SelectedValue);
    }
    private void RequestedChanged()
    {
        //if (tbRequested.Value.IsEmpty() || tbWorkDuration.Value.IsEmpty() || ddlWorkDurationType.SelectedValue.IsEmpty()) return;
        //DateTime dtRequested = tbRequested.Value.ToHTML5DateTime();
        //if (ddlWorkDurationType.SelectedValue == "Hour") dtRequested = dtRequested.AddHours(tbWorkDuration.Value.ToInt());
        //else if (ddlWorkDurationType.SelectedValue == "Day") dtRequested = dtRequested.AddDays(tbWorkDuration.Value.ToInt());
        //else if (ddlWorkDurationType.SelectedValue == "Month") dtRequested = dtRequested.AddMonths(tbWorkDuration.Value.ToInt());
        //else if (ddlWorkDurationType.SelectedValue == "Year") dtRequested = dtRequested.AddYears(tbWorkDuration.Value.ToInt());
        //tbTargetCompletion.Value = dtRequested.ToString("yyyy-MM-dd HH:mm");
    }
    private void RequestSourceChanged()
    {
        U.Hide("dvSocialMedia");
        if (ddlRequestSource.SelectedItem.Text == "Social Media")
            U.Display("dvSocialMedia");
    }    
    private void StatusChanged()
    {
        U.Hide("dvCompletion");
        if (ddlStatus.SelectedValue == "Completed")
        {
            U.Display("dvCompletion");
            tbCompletion.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
    #endregion

    #region Conducted By
    private void AddConductedBy()
    {
        Wrapping w = new Wrapping();
        ConductedByValidation(w);
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }

        w.ListData.Add(new HelpdeskConductedBy { OperatorType = "External" });
        U.BindGrid(gvConductedBy, w.ListData);
    }
    private void DeleteConductedBy()
    {
        List<object> oList = GetGridDataConductedBy();
        if (!oList.Exists(a => ((HelpdeskConductedBy)a).IsChecked))
        {
            U.ShowMessage("Please select Operator you want to delete");
            return;
        }

        oList.RemoveAll(a => ((HelpdeskConductedBy)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvConductedBy, new List<object> { new HelpdeskConductedBy { OperatorType = "External" } });
        else U.BindGrid(gvConductedBy, oList);
    }
    private void ConductedByValidation(Wrapping w)
    {
        w.ListData = GetGridDataConductedBy();
        int idx = 0;
        foreach (HelpdeskConductedBy hc in w.ListData)
        {           
            idx += 1;
            if (hc.OperatorType.Is("Internal"))
            {
                if (hc.OperatorUserId.IsEmpty()) w.ErrorValidation = $"Operator is required at row {idx}";
            }
            else if (hc.OperatorType.Is("External"))
            {
                if (hc.VendorId.IsZero() || hc.OperatorsId.IsEmpty()) w.ErrorValidation = $"Vendor and Operator is required at row {idx}";
            }           
        }
    }
    private List<object> GetGridDataConductedBy()
    {
        List<object> oList = new List<object>();
        foreach (GridViewRow Row in gvConductedBy.Rows)
        {
            Literal ltrl_Id = (Literal)Row.FindControl("ltrl_Id");
            Literal ltrl_Seq = (Literal)Row.FindControl("ltrl_Seq");
            CheckBox cb_IsChecked = (CheckBox)Row.FindControl("cb_IsChecked");
            DropDownList ddlOperatorTypeMultiple = (DropDownList)Row.FindControl("ddlOperatorTypeMultiple");
            DropDownList ddlVendorMultiple = (DropDownList)Row.FindControl("ddlVendorMultiple");
            DropDownList ddlOperatorMultiple = (DropDownList)Row.FindControl("ddlOperatorMultiple");
            HelpdeskConductedBy hc = new HelpdeskConductedBy { Id = ltrl_Id.Text.ToInt(), Seq = ltrl_Seq.Text.ToInt(), IsChecked = cb_IsChecked.Checked, OperatorType = ddlOperatorTypeMultiple.SelectedValue };
            if (hc.OperatorType.Is("Internal")) hc.OperatorUserId = ddlOperatorMultiple.SelectedValue.ToInt();
            else if (hc.OperatorType.Is("External"))
            {
                hc.VendorId = ddlVendorMultiple.SelectedValue.ToInt();
                hc.OperatorsId = ddlOperatorMultiple.SelectedValue.ToInt();
            }
            oList.Add(hc);
        }
        return oList;
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

        w.ListData.Add(new HelpdeskItem { });
        U.BindGrid(gvItems, w.ListData);
    }
    private void DeleteItem()
    {
        List<object> oList = U.GetGridData(gvItems, typeof(HelpdeskItem)).ListData;
        if (!oList.Exists(a => ((HelpdeskItem)a).IsChecked))
        {
            U.ShowMessage("Please select Item you want to delete");
            return;
        }

        oList.RemoveAll(a => ((HelpdeskItem)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvItems, new List<object> { new HelpdeskItem { } });
        else U.BindGrid(gvItems, oList);
    }
    private void ItemValidation(Wrapping w)
    {
        w.ListData = U.GetGridData(gvItems, typeof(HelpdeskItem)).ListData;
        int idx = 0;
        foreach (HelpdeskItem o in w.ListData)
        {
            idx += 1;
            if (o.ItemId == 0) w.ErrorValidation = $"Item is required at row {idx}";
            if (o.Qty == 0) w.ErrorValidation = $"Qty is required at row {idx}";
        }
    }
    #endregion

    private void Submit()
    {
        Wrapping w = new Wrapping();
        Validation(w);
        if ($"{w.Sb}" != "") U.ShowMessage($"{w.Sb}");
        else
        {
            if (U.ShowError(Save(w).ContainErrorMessage(out string Result), Result))
                return;                        
            F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
        }
    }
    private void Validation(Wrapping w)
    {
        if (ddlSubject.SelectedValue.IsEmpty()) w.RequiredValidation = "Subject";
        else if (ddlSubject.SelectedValue.Is("0")) w.ErrorValidation = "Subject is not active, please change with other Subject";

        if (!ViewState[vsNeedCust].ToBool())
        {
            if (ddlArea.SelectedValue.IsEmpty()) w.RequiredValidation = "Area";
            if (hfLocationId.Text.IsEmpty()) w.RequiredValidation = "Location";
        }
        else
        {
            List<object> Customers = U.GetGridData(gvCustomer, typeof(Customer)).ListData.FindAll(a => ((Customer)a).CustNo.IsNotEmpty());
            if (Customers.Count.IsZero()) w.RequiredValidation = "Customer";
            List<object> oList = GetGridDataConductedBy();
            oList.RemoveAll(a => ((HelpdeskConductedBy)a).OperatorType.Is("External") && ((HelpdeskConductedBy)a).VendorId.IsZero());
            oList.RemoveAll(a => ((HelpdeskConductedBy)a).OperatorType.Is("Internal") && ((HelpdeskConductedBy)a).OperatorUserId.IsZero());
            if (oList.Count.IsZero()) w.RequiredValidation = "Operator";
            if (oList.Exists(a => ((HelpdeskConductedBy)a).OperatorsId.IsZero() && ((HelpdeskConductedBy)a).OperatorUserId.IsZero()))
                w.RequiredValidation = "Operator";
        }

        if (ddlWorkOrderType.SelectedValue.IsEmpty()) w.RequiredValidation = "Work Order Type";
        else if (ddlWorkOrderType.SelectedValue.Is("0")) w.ErrorValidation = "Work Order Type is not active, please change with other Work Order Type";
        if (ddlCategory.SelectedValue.IsEmpty()) w.RequiredValidation = "Category";
        else if (ddlCategory.SelectedValue.Is("0")) w.ErrorValidation = "Category is not active, please change with other Category";

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
                
        if (ddlStatus.SelectedValue == "Completed" && tbCompletion.Value.IsEmpty()) w.RequiredValidation = "Completion Date Time";


        if (ddlRequestType.SelectedValue.IsEmpty()) w.RequiredValidation = "Request Type";
        if (ddlRequestSource.SelectedValue.IsEmpty()) w.RequiredValidation = "Request Source";
        if (tbRequesterName.Value.IsEmpty()) w.RequiredValidation = "Requester Name";
        if (!tbRequesterEmail.Value.IsEmpty() && !U.IsValidEmail(tbRequesterEmail.Value)) w.ErrorValidation = "Invalid email address";
        if (ddlRequestSource.SelectedItem.Text == "Social Media" && ddlSocialMedia.SelectedValue.IsEmpty()) w.RequiredValidation = "Requester Social Media";
        if (tbRequestDetail.Value.IsEmpty()) w.RequiredValidation = "Request Detail";
        if (tbRequestedDateTime.Value.IsEmpty()) w.RequiredValidation = "Request Date Time";

        #region Items
        List<object> Items = U.GetGridData(gvItems, typeof(HelpdeskItem)).ListData.FindAll(a => ((HelpdeskItem)a).ItemId.IsNotZero());
        if (Items.Count.IsNotZero())
        {
            if (Items.Exists(a => ((HelpdeskItem)a).Qty.IsZero())) w.ErrorValidation = "Qty in each Item is required";
        }
        #endregion

    }
    private string Save(Wrapping w)
    {
        string Result = "";

        List<object> attList = Utility.GetGridData(gvAttachment, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).FileName != CNT.DataNotAvailable);
        foreach (Attachment att in attList)
        {
            try { att.Data = U.GetFile(att.FileNameUniq); }
            catch { return "Error Message : Your page is expired please reload with klik Button Refresh"; }
        }

        #region Helpdesk
        bool UsedCustomer = ViewState[vsNeedCust].ToBool();
        Helpdesk h = new Helpdesk();
        if (!U.Id.IsNull()) h = Helpdesk.GetById(U.Id);
        h.WorkOrderTypeId = ddlWorkOrderType.SelectedValue.ToInt();
        h.HelpdeskCategoryId = ddlCategory.SelectedValue.ToInt();
        h.SubjectId = ddlSubject.SelectedValue.ToInt();
        if (!UsedCustomer)
        {
            h.AreaId = ddlArea.SelectedValue.ToInt();
            h.LocationId = hfLocationId.Text.ToInt();
        }  
        if (ddlOperatorType.SelectedValue.IsNotEmpty())
        {
            h.OperatorType = ddlOperatorType.SelectedValue;
            if (h.OperatorType == "External")
            {                
                h.VendorId = ddlVendor.SelectedValue.ToInt();
                h.OperatorsId = ddlOperator.SelectedValue.ToInt();
            }
            else h.OperatorUserId = ddlOperator.SelectedValue.ToInt();
        }
        h.RequestTypeId = ddlRequestType.SelectedValue.ToInt();
        h.RequestSourceId = ddlRequestSource.SelectedValue.ToInt();
        h.RequesterName = tbRequesterName.Value;
        h.RequesterEmail = tbRequesterEmail.Value;
        h.RequesterPhone = tbRequesterPhone.Value;
        if (ddlRequestSource.SelectedItem.Text == "Social Media") h.SocialMediaId = ddlSocialMedia.SelectedValue.ToInt();
        h.RequestDetail = tbRequestDetail.Value;
        h.Requested = tbRequestedDateTime.Value.ToHTML5DateTime();
        
        RequestType rt = RequestType.GetById(h.RequestTypeId);
        if (U.Id.IsNull()) h.Response = DateTime.Now;
        double TotalHour = (h.Response - h.Requested).TotalHours;
        if (TotalHour > rt.ResponseTime || TotalHour < 0) h.ResponseStatus = "Not Achived";
        else h.ResponseStatus = "Achived";
        h.Status = ddlStatus.SelectedValue;
        h.CustEachOperator = tbNumberCust.Value.ToInt();

        if (U.Id.IsNull())
        {
            h.CreatedBy = $"{ViewState[vsUserName]}";
            Result = h.Insert();
            if (Result.ContainErrorMessage()) return Result;
            h.Id = Result.ToInt();
        }
        else
        {
            h.Id = U.Id.ToInt();
            h.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = h.Update();
            if (Result.ContainErrorMessage()) return Result;
            if (!UsedCustomer)
            {
                Result = HelpdeskCustomer.DeleteByHelpdeskId(h.Id);
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        #endregion

        #region Attachment        
        string TableName = "HelpdeskAttachment";
        Result = Attachment.DeleteByOwnerId(h.Id, TableName);
        if (Result.Contains("Error Message :")) return Result;

        foreach (Attachment att in Utility.GetGridData(gvAttachment, typeof(Attachment)).ListData)
        {
            if (att.FileName == "Data not available") continue;
            att.Table = TableName;
            att.OwnerId = h.Id;
            att.Data = U.GetFile(att.FileNameUniq);
            att.CreatedBy = $"{ViewState[vsUserName]}";
            Result = att.Insert(att);
            if (Result.Contains("Error Message :")) return Result;

            string Path = $@"{U.PathTempFolder}{att.FileNameUniq}";
            File.Delete(Path);
        }
        #endregion

        if (ViewState[CNT.VS.IsUsed].ToBool())
            return "";

        #region Customer
        h.Customers = U.GetGridData(gvCustomer, typeof(Customer)).ListData.FindAll(a => ((Customer)a).CustNo.IsNotEmpty());
        if (UsedCustomer)
        {            
            List<object> CustDBList = HelpdeskCustomer.GetByHelpdesk(h.Id);
            foreach (HelpdeskCustomer hc in CustDBList)
            {
                if (!h.Customers.Exists(a => ((Customer)a).Id.Is(hc.CustomerId)))
                    hc.Delete();
            }
            foreach (Customer c in h.Customers)
            {
                HelpdeskCustomer hc = new HelpdeskCustomer { HelpdeskId = h.Id, CustomerId = c.Id, Seq = c.No.ToInt() };
                if (hc.Id.IsZero())
                {
                    hc.CreatedBy = ViewState[vsUserName].ToText();
                    Result = hc.Insert();
                    if (Result.ContainErrorMessage()) return Result;
                    hc.Id = Result.ToInt();
                }
                else
                {
                    hc.ModifiedBy = ViewState[vsUserName].ToText();
                    Result = hc.Update();
                    if (Result.ContainErrorMessage()) return Result;
                }
            }
        }
        #endregion

        #region Operators
        List<object> hcList = GetGridDataConductedBy();
        List<object> hcDBList = HelpdeskConductedBy.GetByHelpdesk(h.Id);
        foreach (HelpdeskConductedBy hc in hcDBList)
        {
            if (!hcList.Exists(a => ((HelpdeskConductedBy)a).Id.Is(hc.Id)))
                hc.Delete();
        }
        foreach (HelpdeskConductedBy hc in hcList)
        {
            hc.HelpdeskId = h.Id;
            if (hc.Id.IsZero())
            {
                hc.CreatedBy = ViewState[vsUserName].ToText();
                Result = hc.Insert();
                if (Result.ContainErrorMessage()) return Result;
                hc.Id = Result.ToInt();
            }
            else
            {
                hc.ModifiedBy = ViewState[vsUserName].ToText();
                Result = hc.Update();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        #endregion

        #region Items
        h.Items = U.GetGridData(gvItems, typeof(HelpdeskItem)).ListData.FindAll(a => ((HelpdeskItem)a).ItemId.IsNotZero());
        List<object> ItemDBList = HelpdeskItem.GetByHelpdeskId(h.Id);
        foreach (HelpdeskItem hi in ItemDBList)
        {
            if (!h.Items.Exists(a => ((HelpdeskItem)a).ItemId.Is(hi.ItemId)))
                hi.Delete();
        }
        foreach (HelpdeskItem hi in h.Items)
        {
            hi.HelpdeskId = h.Id;
            if (hi.Id.IsZero())
            {
                hi.CreatedBy = ViewState[vsUserName].ToText();
                Result = hi.Insert();
                if (Result.ContainErrorMessage()) return Result;
                hi.Id = Result.ToInt();
            }
            else
            {
                hi.ModifiedBy = ViewState[vsUserName].ToText();
                Result = hi.Update();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        #endregion

        #region Work Order
        #region New Helpdesk
        if (U.Id.IsEmpty())
        {
            if (UsedCustomer)
            {
                h.Customers = ReorderCustomer(h.Customers);
                h.Customers = DistributeCustomers(h.Customers, hcList);
                foreach (Customer c in h.Customers)
                {
                    Location l = Location.GetById(c.LocationId);
                    h.AreaId = l.AreaId;
                    h.LocationId = l.Id;
                    h.OperatorType = c.OperatorType;
                    h.VendorId = c.VendorId;
                    h.OperatorsId = c.OperatorsId;
                    h.OperatorUserId = c.OperatorUserId;
                    Result = InsertWorkOrder(h, c.Id);
                }
                return "";
            }
            Result = InsertWorkOrder(h);
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion
        #region Update Helpdesk
        else if (!ViewState[CNT.VS.IsUsed].ToBool())
        {
            Result = h.DeleteWorkOrder();
            if (Result.ContainErrorMessage()) return Result;
            if (UsedCustomer)
            {
                h.Customers = ReorderCustomer(h.Customers);
                h.Customers = DistributeCustomers(h.Customers, hcList);
                foreach (Customer c in h.Customers)
                {
                    Location l = Location.GetById(c.LocationId);
                    h.AreaId = l.AreaId;
                    h.LocationId = l.Id;
                    h.OperatorType = c.OperatorType;
                    h.VendorId = c.VendorId;
                    h.OperatorsId = c.OperatorsId;
                    h.OperatorUserId = c.OperatorUserId;
                    Result = InsertWorkOrder(h, c.Id);
                }
                return "";
            }
            Result = InsertWorkOrder(h);
        }
        #endregion
        #endregion

        return "";
    }
    private List<object> ReorderCustomer(List<object> Customers)
    {
        List<object> orderedCustomers = new List<object>();                
        Customer baseCustomer = new Customer { Latitude = Parameters.GetByKey("Latitude").Text, Longitude = Parameters.GetByKey("Longitude").Text };

        while (Customers.Count > 0)
        {
            Customer nearestCustomer = FindNearestCustomer(baseCustomer, Customers);
            orderedCustomers.Add(nearestCustomer);
            Customers.Remove(nearestCustomer);
            baseCustomer = nearestCustomer;
        }

        return orderedCustomers;        
    }
    public static Customer FindNearestCustomer(Customer currentCustomer, List<object> customers)
    {
        Customer nearestCustomer = null;
        double nearestDistance = double.MaxValue;

        foreach (Customer customer in customers)
        {
            double distance = U.CalculateDistance(currentCustomer.Latitude.ToDouble(), currentCustomer.Longitude.ToDouble(), customer.Latitude.ToDouble(), customer.Longitude.ToDouble());
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCustomer = customer;
            }
        }

        return nearestCustomer;
    }
    private List<object> DistributeCustomers(List<object> Customers, List<object> Operators)
    {
        if (tbNumberCust.Value.IsEmpty() || tbNumberCust.Value.IsZero())
        {
            int baseCustomersPerOperator = Customers.Count / Operators.Count;
            int remainingCustomers = Customers.Count % Operators.Count;

            int customerIndex = 0;
            for (int i = 0; i < Operators.Count; i++)
            {
                HelpdeskConductedBy op = (HelpdeskConductedBy)Operators[i];
                int customersForThisOperator = baseCustomersPerOperator + (i == Operators.Count - 1 ? remainingCustomers : 0);

                for (int j = 0; j < customersForThisOperator; j++)
                {
                    Customer c = (Customer)Customers[customerIndex];
                    c.OperatorType = op.OperatorType;
                    c.VendorId = op.VendorId;
                    c.OperatorsId = op.OperatorsId;
                    c.OperatorUserId = op.OperatorUserId;
                    customerIndex++;
                }
            }
        }
        else
        {
            // Use the provided number of customers per operator
            int customerIndex = 0;
            int numberOfCustomerEachOperator = tbNumberCust.Value.ToInt();
            foreach (HelpdeskConductedBy op in Operators)
            {
                for (int j = 0; j < numberOfCustomerEachOperator && customerIndex < Customers.Count; j++)
                {
                    Customer c = (Customer)Customers[customerIndex];
                    c.VendorId = op.VendorId;
                    c.OperatorsId = op.OperatorsId;
                    c.OperatorUserId = op.OperatorUserId;
                    customerIndex++;
                }
            }

            // Assign remaining customers to the last operator
            while (customerIndex < Customers.Count)
            {
                Customer c = (Customer)Customers[customerIndex];
                HelpdeskConductedBy op = (HelpdeskConductedBy)Operators.Last();
                c.VendorId = op.VendorId;
                c.OperatorsId = op.OperatorsId;
                c.OperatorUserId = op.OperatorUserId;
                customerIndex++;
            }
        }
        return Customers;
    }
    private string InsertWorkOrder(Helpdesk h, int CustomerId = 0)
    {
        WorkOrder wo = new WorkOrder();
        wo.HelpdeskId = h.Id;
        wo.AreaId = h.AreaId;
        wo.LocationId = h.LocationId;
        wo.OperatorType = h.OperatorType;
        wo.VendorId = h.VendorId;
        wo.OperatorsId = h.OperatorsId; 
        wo.OperatorUserId = h.OperatorUserId;
        wo.CustomerId = CustomerId;
        Subject s = Subject.GetById(h.SubjectId);
        if (s.WorkDuration != 0)
        {
            wo.WorkDuration = s.WorkDuration;
            wo.WorkDurationType = s.WorkDurationType;
        }
        wo.CreatedBy = $"{ViewState[vsUserName]}";

        #region Status
        if (h.Items.Count.IsZero())
        {
            if (CustomerId.IsNotZero())
                wo.Status = CNT.Status.Preparation;
            else
            {
                if (ddlOperatorType.SelectedValue.Or("Internal,External"))
                    wo.Status = CNT.Status.Preparation;
                else wo.Status = CNT.Status.Assignment;
            }           
        }
        else
        {
            if (CustomerId.IsNotZero())
                wo.Status = CNT.Status.StockOut;
            else
            {
                if (ddlOperatorType.SelectedValue.Or("Internal,External"))
                    wo.Status = CNT.Status.StockOut;
                else wo.Status = CNT.Status.Assignment;
            }            
        }            
        #endregion

        string Result = wo.InsertByHelpdesk();
        if (Result.ContainErrorMessage()) return Result;
        wo.Id = Result.ToInt();

        #region Items
        if (h.Items.Count.IsNotZero())
        {
            List<object> woiDBList = WorkOrderItem.GetByWorkOrderId(wo.Id);
            foreach (WorkOrderItem woi in woiDBList)
            {
                if (h.Items.Exists(a => ((HelpdeskItem)a).ItemId.Is(woi.ItemId)))
                    woi.Delete();
            }
            foreach (HelpdeskItem hi in h.Items)
            {
                WorkOrderItem woi = new WorkOrderItem { WorkOrderId = wo.Id, ItemId = hi.ItemId, Seq = hi.Seq, Qty = hi.Qty };
                woi.CreatedBy = $"{ViewState[vsUserName]}";
                Result = woi.Insert();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        #endregion

        return Result;
    }    
    private void Delete()
    {
        string Result = Helpdesk.Delete(U.Id);
        if (Result.ContainErrorMessage()) U.ShowMessage(Result, F.Icon.ErrorDelete, "Delete Failed!");
        else F.Alert.Show("Data has been Delete Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
    }
    #endregion            
}