using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.IO;

public partial class Pages_Inventory_StockOutReturUsedInput : System.Web.UI.Page
{
    #region Fields
    private const string vsUserName = "Username";
    private const string TableName = "StockOutReturUsedAttachment";
    private const string vsStockOutItemId = "StockOutItemId";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            Initialize();        
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
            case "btnWorkOrderLookup":
                U.ExecClientScript(wWorkOrder.GetShowReference($"~/Pages/WorkOrder/SelectWorkOrderRetur.aspx?WorkType=Retur"));
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
    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {
            case "imbDeleteAttachment":
                DeleteAttachment(imb);
                break;
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
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddlWorkUpdate":
                WorkUpdateChanged();
                break;
            case "ddlWorkUpdateItem":
                WorkUpdateItemChanged();
                break;
            case "ddlWarehouse":
                WarehouseChanged();
                break;
        }
    }    
    protected void wWorkOrder_Close(object sender, F.WindowCloseEventArgs e)
    {
        Wrapping w = (Wrapping)Session[CNT.Session.Wrapping];
        tbWorkOrderLookup.Value = w.Code;
        ViewState[CNT.VS.WorkOrderId] = w.Id;
        ViewState[CNT.VS.StockOutId] = w.KeyId;
        WorkOrderChanged();
    }
    #endregion

    #region Methods
    private void Initialize()
    {
        ViewState[vsUserName] = U.GetUsername();
        if ($"{ViewState[vsUserName]}" == "") Response.Redirect(@"~\Pages\default.aspx");
        btnCancel.OnClientClick = "parent.removeActiveTab();";
        U.SetDropDownMasterData(ddlReceiver, "Users");
        fu.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUpload.ClientID));
        
        U.SetDropDownMasterData(ddlWarehouse, "Warehouse");
        U.BindGrid(gvAttachment, new List<object> { new Attachment { Seq = 0, FileName = CNT.DataNotAvailable } });
        SetInitEdit();
    }
    private void WorkOrderChanged()
    {        
        ddlWorkUpdate.Items.Clear();
        ddlWorkUpdateItem.Items.Clear();
        tbSKU.Value = "";
        tbReturQty.Value = "";
        if (ViewState[CNT.VS.WorkOrderId].ToInt().IsZero()) return;
        ddlWorkUpdate.Items.Add("");
        foreach (WorkOrderWorkUpdate wowu in WorkOrderWorkUpdate.GetByWorkOrderId(ViewState[CNT.VS.WorkOrderId]))
        {
            if (wowu.WorkType.Is(CNT.WorkOrder.WorkType.Retur))
                ddlWorkUpdate.Items.Add(new ListItem(wowu.WorkDetail, wowu.Id.ToText()));
        }            
    } 
    private void WorkUpdateChanged()
    {
        ddlWorkUpdateItem.Items.Clear();
        tbSKU.Value = "";
        tbReturQty.Value = "";
        if (ddlWorkUpdate.SelectedValue.IsEmpty()) return;
        ddlWorkUpdateItem.Items.Add("");
        foreach (WorkOrderWorkUpdateItem wowui in WorkOrderWorkUpdateItem.GetByWorkOrderWorkUpdateId(ddlWorkUpdate.SelectedValue, false))
            ddlWorkUpdateItem.Items.Add(new ListItem($"{wowui.ItemCode} | {wowui.ItemName}", wowui.Id.ToText()));
    }
    private void WorkUpdateItemChanged()
    {
        tbSKU.Value = "";
        tbReturQty.Value = "";
        if (ddlWorkUpdateItem.SelectedValue.IsEmpty()) return;  
        WorkOrderWorkUpdateItem wowui = WorkOrderWorkUpdateItem.GetById(ddlWorkUpdateItem.SelectedValue);
        StockOutItem soi = StockOutItem.GetById(wowui.StockOutItemId);
        ViewState[CNT.VS.StockOutId] = soi.StockOutId;
        ViewState[CNT.VS.StockOutItemId] = soi.Id;
        ViewState[CNT.VS.StockReceivedItemId] = soi.StockReceivedItemId;
        ViewState[CNT.VS.ItemId] = wowui.ItemId;
        tbSKU.Value = soi.SKU;
        tbReturQty.Value = wowui.ReturQty.ToText();
    }
    private void WarehouseChanged()
    {
        ddlRack.Items.Clear();
        if (ddlWarehouse.SelectedValue.IsEmpty()) return;
        U.SetDropDownRack(ddlRack, ddlWarehouse.SelectedValue);
    }
    private int GetUnusedQty(int woId, int ItemId)
    {
        int Result = 0;
        List<object> woiList = WorkOrderItem.GetByWorkOrderItem(woId, ItemId);
        List<object> wowuiList = WorkOrderWorkUpdateItem.GetByWorkOrderId(woId);
        foreach (WorkOrderItem woi in woiList)
        {
            int TotalItem = wowuiList.Where(b => ((WorkOrderWorkUpdateItem)b).ItemId.Is(woi.ItemId)).Sum(c => ((WorkOrderWorkUpdateItem)c).Qty);
            int TotalRetur = StockOutReturUsed.GetTotalQtyByWorkOrderItem(woId, woi.ItemId);

            int UnusedQty = woi.Qty - TotalItem;
            return UnusedQty;
        }
        return Result;
    }
    private void SetInitEdit()
    {                
        if (U.Id.IsNull()) return;
        pnlWorkOrderLookup.Visible = false;
        pnlWorkOrder.Visible = true;
        StockOutReturUsed o = StockOutReturUsed.GetById(U.Id);        
        tbWorkOrder.Value = o.WorkOrderCode;
        ViewState[CNT.VS.WorkOrderId] = o.WorkOrderId;
        tbCode.Value = o.Code;                
        tbReason.Value = o.Reason;       
        ddlReceiver.SelectedValue = $"{o.ReceiverUserId}";
        tbReturDate.Value = o.ReturDate.ToString("yyyy-MM-dd HH:mm");
        tbReturQty.Value = $"{o.Qty}";
        WorkOrderChanged();
        ddlWorkUpdate.SetValue(o.WorkOrderWorkUpdateId);
        WorkUpdateChanged();        
        if (ddlWorkUpdateItem.Items.FindByValue(o.WorkOrderWorkUpdateItemId.ToText()).IsNull())
        {
            WorkOrderWorkUpdateItem wowui = WorkOrderWorkUpdateItem.GetById(o.WorkOrderWorkUpdateItemId);
            ddlWorkUpdateItem.Items.Add(new ListItem($"{wowui.ItemCode} | {wowui.ItemName}", wowui.Id.ToText()));
        }
        ddlWorkUpdateItem.SetValue(o.WorkOrderWorkUpdateItemId);
        WorkUpdateItemChanged();
        ddlWarehouse.SetValue(o.WarehouseId);
        WarehouseChanged();
        ddlRack.SetValue(o.RackId);

        List<object> aList = Attachment.GetByOwnerID(TableName, o.Id);
        foreach (Attachment att in aList)
            U.SaveAttachmentToTempFolder(att);
        if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
        U.BindGrid(gvAttachment, aList);
    }
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
            string Result = Save(w);
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
        if (tbCode.Value.IsEmpty()) w.RequiredValidation = "Retur Code";
        if (tbReason.Value.IsEmpty()) w.RequiredValidation = "Retur Reason";
        if (ddlReceiver.SelectedValue.IsEmpty()) w.RequiredValidation = "Requester";
        if (tbReturDate.Value.IsEmpty()) w.RequiredValidation = "Retur Date";
        if (ViewState[CNT.VS.WorkOrderId].IsEmpty()) w.RequiredValidation = "Work Order";
        if (ddlWorkUpdate.SelectedValue.IsEmpty()) w.RequiredValidation = "Work Update";
        if (ddlWorkUpdateItem.SelectedValue.IsEmpty()) w.RequiredValidation = "Work Update Item";
        if (tbReturQty.Value.ToInt().IsZero()) w.RequiredValidation = "Retur Qty";
        if (ddlWarehouse.SelectedValue.IsEmpty()) w.RequiredValidation = "Warehouse";
        if (ddlRack.SelectedValue.IsEmpty()) w.RequiredValidation = "Rack";

        List<object> aList = U.GetGridData(gvAttachment, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).Seq != 0);
        if (aList.Count == 0) w.RequiredValidation = "Attachment";
    }
    private string Save(Wrapping w)
    {
        string Result = U.AttachmentValidation(gvAttachment, out List<object> attList);
        if (Result.ContainErrorMessage()) return Result;

        #region Stock Out Retur Used                
        StockOutReturUsed o = StockOutReturUsed.GetById(U.Id);
        o.Code = tbCode.Value;
        o.Reason = tbReason.Value;
        o.ReceiverUserId = ddlReceiver.SelectedValue.ToInt();
        o.ReturDate = tbReturDate.Value.ToHTML5DateTime();
        o.WorkOrderId = ViewState[CNT.VS.WorkOrderId].ToInt();
        o.WorkOrderWorkUpdateId = ddlWorkUpdate.SelectedValue.ToInt();
        o.WorkOrderWorkUpdateItemId = ddlWorkUpdateItem.SelectedValue.ToInt();
        o.StockOutId = ViewState[CNT.VS.StockOutId].ToInt();
        o.StockOutItemId = ViewState[CNT.VS.StockOutItemId].ToInt();
        o.StockReceivedItemId = ViewState[(CNT.VS.StockReceivedItemId)].ToInt();
        o.ItemId = ViewState[(CNT.VS.ItemId)].ToInt();
        o.Qty = tbReturQty.Value.ToInt();
        o.WarehouseId = ddlWarehouse.SelectedValue.ToInt();
        o.RackId = ddlRack.SelectedValue.ToInt();

        StockOutItem soi = StockOutItem.GetById(ViewState[vsStockOutItemId].ToInt());
        Stock s = Stock.GetByItemId(soi.ItemId);
        StockMovement sm = new StockMovement();

        if (U.Id == null)
        {
            Result = o.Insert();
            if (Result.ContainErrorMessage()) return Result;
            o.Id = Result.ToInt();

            #region Stock Out Item
            soi.ReturUsedQty += o.Qty;
            soi.UsedQty -= o.Qty;
            Result = soi.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock Movement            
            sm.StockOutReturUsedId = o.Id;
            sm.ItemId = o.ItemId;
            sm.Qty = o.Qty;
            sm.MovementType = "StockOutReturUsed";
            sm.MovementDate = o.ReturDate;
            sm.RequesterUserId = o.ReceiverUserId;
            sm.CreatedBy = ViewState[vsUserName].ToText();
            Result = sm.Insert();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock                            
            s.UsedQty += o.Qty;
            Result = s.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;            
            #endregion

        }
        else
        {
            StockOutReturUsed oDB = StockOutReturUsed.GetById(U.Id);
            soi.ReturUsedQty -= oDB.Qty;
            soi.UsedQty += oDB.Qty;           
            s.UsedQty -= oDB.Qty;

            Result = o.Update();
            if (Result.ContainErrorMessage()) return Result;

            #region Stock Out Item        
            soi.ReturQty += o.Qty;
            soi.UsedQty -= o.Qty;
            Result = soi.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
            #endregion            

            #region Stock Movement
            sm = StockMovement.GetByStockOutReturUsedId(o.Id);
            sm.Qty = o.Qty;
            sm.MovementDate = o.ReturDate;
            sm.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = sm.Update();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock                            
            s.UsedQty += o.Qty;
            Result = s.UpdateQty();
            if (Result.ContainErrorMessage()) return Result;
            #endregion
        }
        #endregion

        #region Stock Out Retur Attachment
        string TableName = "StockOutReturUsedAttachment";
        Result = Attachment.DeleteByOwnerId(o.Id, TableName);
        if (Result.Contains("Error Message :")) return Result;

        foreach (Attachment att in attList)
        {
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

        return "";
    }
    private void Delete()
    {
        StockOutReturUsed o = StockOutReturUsed.GetById(U.Id);
        StockOutItem soi = StockOutItem.GetById(o.StockOutId);        
        Stock s = Stock.GetByItemId(soi.ItemId);

        string Result = Attachment.DeleteByOwnerId(U.Id, "StockOutReturUsedAttachment");
        if (Result.ContainErrorMessage().ShowError(Result)) return;

        Result = StockOutReturUsed.Delete(U.Id);
        if (Result.ContainErrorMessage().ShowError(Result)) return;

        #region Stock Out Item        
        soi.ReturQty -= o.Qty;
        soi.UsedQty += o.Qty;
        Result = soi.UpdateQty();
        if (Result.ContainErrorMessage().ShowError(Result)) return;
        #endregion        

        #region Stock Movement
        StockMovement sm = StockMovement.GetByStockOutReturUsedId(U.Id);
        Result = sm.Delete();
        if (Result.ContainErrorMessage().ShowError(Result)) return;
        #endregion

        #region Stock                        
        s.UsedQty -= o.Qty;
        Result = s.UpdateQty();
        if (Result.ContainErrorMessage().ShowError(Result)) return;
        #endregion

        U.CloseUpdate("Stock Received has been Deleted Successfully");
    }
    #endregion            
}