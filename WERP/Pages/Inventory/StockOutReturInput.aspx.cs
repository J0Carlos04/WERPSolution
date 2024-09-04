using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.IO;

public partial class Pages_Inventory_StockOutReturInput : PageBase
{
    #region Fields
    private const string vsUserName = "Username";
    private const string TableName = "StockOutReturAttachment";
    private const string vsStockOutItemId = "StockOutItemId";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            Initialize();
        string Script = "InitSelect2StockOutReturInput();";
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
            case "btnWorkOrderLookup":
                if (ViewState[CNT.VS.StockOutId].IsNotEmpty()) U.ExecClientScript(wWorkOrder.GetShowReference($"~/Pages/WorkOrder/SelectWorkOrderRetur.aspx?StockOutId={ViewState[CNT.VS.StockOutId]}&StockOutItemId={U.QSStockOutItemId}"));
                else U.ExecClientScript(wWorkOrder.GetShowReference($"~/Pages/WorkOrder/SelectWorkOrderRetur.aspx"));
                break;
            case "btnSelectStockOutItemCode":
                if (tbWorkOrderLookup.Value.IsNotEmpty()) U.ExecClientScript(wSR.GetShowReference($"~/Pages/Inventory/SelectStockOutItem.aspx?StockOutId={ViewState[CNT.VS.StockOutId]}&WorkOrderId={ViewState[CNT.VS.WorkOrderId]}"));
                else U.ExecClientScript(wSR.GetShowReference($"~/Pages/Inventory/SelectStockOutItem.aspx"));
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
            
        }
    }
    protected void wSR_Close(object sender, F.WindowCloseEventArgs e)
    {
        StockOutItemChanged(e.CloseArgument);
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

        if (U.QSStockOutItemId.IsNotEmpty())
        {
            pnlWorkOrderLookup.Visible = true;
            pnlCode.Visible = true;
            StockOutItemChanged(U.QSStockOutItemId);            
        }
        else
        {
            if (U.Id.IsNull())
            {
                pnlWorkOrderLookup.Visible = true;
            }
            else
            {
                pnlWorkOrder.Visible = true;
                pnlCode.Visible = true;
            }
        }              
        U.BindGrid(gvAttachment, new List<object> { new Attachment { Seq = 0, FileName = CNT.DataNotAvailable } });
        SetInitEdit();
    }
    private void WorkOrderChanged()
    {        
        if (pnlCode.Visible == false) pnlCodeLookup.Visible = true;
        tbItemCodeLookup.Value = "";
        StockOutItemChanged(ViewState[vsStockOutItemId]);
    }
    private void StockOutItemChanged(object StockOutItemId)
    {        
        StockOutItem o = StockOutItem.GetById(StockOutItemId);        
        tbItemCode.Value = o.ItemCode;
        tbItemCodeLookup.Value = o.ItemCode;
        tbItemName.Value = o.ItemName;
        tbSKU.Value = o.SKU;
        tbUnusedQty.Value = $"{o.UnusedQty}";
        ViewState[vsStockOutItemId] = StockOutItemId;
        ViewState[CNT.VS.StockOutId] = o.StockOutId;

        if (ViewState[CNT.VS.WorkOrderId].ToInt().IsNotZero())
        {
            int woId = ViewState[CNT.VS.WorkOrderId].ToInt();
            tbUnusedQty.Value = $"{GetUnusedQty(woId, o.ItemId)}";
        }
    }
    private int GetUnusedQty(int woId, int ItemId)
    {
        int Result = 0;
        List<object> woiList = WorkOrderItem.GetByWorkOrderItem(woId, ItemId);
        List<object> wowuiList = WorkOrderWorkUpdateItem.GetByWorkOrderId(woId);
        foreach (WorkOrderItem woi in woiList)
        {
            int TotalItem = wowuiList.Where(b => ((WorkOrderWorkUpdateItem)b).ItemId.Is(woi.ItemId)).Sum(c => ((WorkOrderWorkUpdateItem)c).Qty);
            int TotalRetur = StockOutRetur.GetTotalQtyByWorkOrderItem(woId, woi.ItemId);

            int UnusedQty = woi.Qty - TotalItem;
            return UnusedQty;
        }
        return Result;
    }
    private void SetInitEdit()
    {
        if (U.Id.IsNull()) return;
        StockOutRetur o = StockOutRetur.GetById(U.Id);
        ViewState[vsStockOutItemId] = o.StockOutItemId;
        ViewState[CNT.VS.WorkOrderId] = o.WorkOrderId;
        tbWorkOrder.Value = o.WorkOrderCode;
        tbCode.Value = o.Code;
        tbItemCode.Value = o.ItemCode;
        tbItemName.Value = o.ItemName;
        tbReason.Value = o.Reason;
        tbReturDate.Value = o.ReturDate.ToString("yyyy-MM-dd HH:mm");
        ddlReceiver.SelectedValue = $"{o.ReceiverUserId}";
        tbQty.Value = $"{o.Qty}";
        tbUnusedQty.Value = (tbUnusedQty.Value.ToInt() + o.Qty).ToString();        

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
        if (ViewState[vsStockOutItemId].IsEmpty()) w.RequiredValidation = "Stock Out Item";
        if (tbCode.Value.IsEmpty()) w.RequiredValidation = "Retur Code";
        if (tbReason.Value.IsEmpty()) w.RequiredValidation = "Retur Reason";
        if (tbReturDate.Value.IsEmpty()) w.RequiredValidation = "Retur Date";
        if (ViewState[CNT.VS.WorkOrderId].IsEmpty()) w.RequiredValidation = "Work Order";
        if (tbItemCode.Value.IsEmpty() && tbItemCodeLookup.Value.IsEmpty()) w.RequiredValidation = "Item Code";
        if (ddlReceiver.SelectedValue.IsEmpty()) w.RequiredValidation = "Requester";
        if (tbQty.Value.IsEmpty()) w.RequiredValidation = "Qty";
        else if (tbQty.Value.IsZero()) w.RequiredValidation = "Qty";
        else if (tbQty.Value.ToInt() > tbUnusedQty.Value.ToInt()) w.ErrorValidation = "Qty can't be bigger than the unused qty";        

        List<object> aList = U.GetGridData(gvAttachment, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).Seq != 0);
        if (aList.Count == 0) w.RequiredValidation = "Attachment";
    }
    private string Save(Wrapping w)
    {
        string Result = U.AttachmentValidation(gvAttachment, out List<object> attList);
        if (Result.ContainErrorMessage()) return Result;

        #region Stock Out Retur        
        StockOutItem soi = StockOutItem.GetById(ViewState[vsStockOutItemId].ToInt());
        StockReceivedItem sri = StockReceivedItem.GetById(soi.StockReceivedItemId);
        Stock s = Stock.GetByItemId(soi.ItemId);
        StockMovement sm = new StockMovement();

        StockOutRetur o = StockOutRetur.GetById(U.Id);
        o.Code = tbCode.Value;
        o.Reason = tbReason.Value;
        o.ReceiverUserId = ddlReceiver.SelectedValue.ToInt();        
        o.ReturDate = tbReturDate.Value.ToHTML5DateTime();
        o.Qty = tbQty.Value.ToInt();        
        
        #region Insert
        if (U.Id == null)
        {
            o.WorkOrderId = ViewState[CNT.VS.WorkOrderId].ToInt();
            o.StockReceivedItemId = sri.Id;
            o.StockOutId = soi.StockOutId;
            o.StockOutItemId = soi.Id;
            o.CreatedBy = $"{ViewState[vsUserName]}";
            Result = o.Insert();
            if (Result.ContainErrorMessage()) return Result;
            o.Id = Result.ToInt();

            #region Stock Out Item        
            soi.ReturQty += o.Qty;
            soi.UnusedQty -= o.Qty;
            Result = soi.UpdateRetur();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock Received Item
            sri.AvailableQty += o.Qty;
            sri.StockOutReturQty += o.Qty;
            Result = sri.UpdateStockOutRetur();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock Movement            
            sm.StockOutReturId = o.Id;
            sm.ItemId = soi.ItemId;
            sm.Qty = o.Qty;
            sm.MovementType = "StockOutRetur";
            sm.MovementDate = o.ReturDate;
            sm.RequesterUserId = o.ReceiverUserId;
            sm.CreatedBy = ViewState[vsUserName].ToText();
            Result = sm.Insert();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock                            
            s.Qty += o.Qty;
            if (s.Id == 0)
            {
                s.CreatedBy = $"{ViewState[vsUserName]}";
                Result = s.Insert();
                if (Result.ContainErrorMessage()) return Result;
            }
            else
            {
                s.ModifiedBy = $"{ViewState[vsUserName]}";
                Result = s.Update();
                if (Result.ContainErrorMessage()) return Result;
            }
            #endregion
        }
        #endregion
        #region Update
        else
        {
            StockOutRetur oDB = StockOutRetur.GetById(U.Id);
            soi.ReturQty -= oDB.Qty;
            soi.UnusedQty += oDB.Qty;
            sri.AvailableQty -= oDB.Qty;
            sri.StockOutReturQty -= o.Qty;            
            s.Qty -= oDB.Qty;

            o.Id = U.Id.ToInt();
            o.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = o.Update();
            if (Result.ContainErrorMessage()) return Result;

            #region Stock Out Item        
            soi.ReturQty += o.Qty;
            soi.UnusedQty -= o.Qty;
            Result = soi.UpdateRetur();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock Received Item
            sri.AvailableQty += o.Qty;
            sri.StockOutReturQty += o.Qty;
            Result = sri.UpdateStockOutRetur();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock Movement
            sm = StockMovement.GetByStockOutReturId(o.Id);
            sm.Qty = o.Qty;
            sm.MovementDate = o.ReturDate;
            sm.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = sm.Update();
            if (Result.ContainErrorMessage()) return Result;
            #endregion

            #region Stock                
            s.ItemId = soi.ItemId;
            s.Qty += o.Qty;
            if (s.Id == 0)
            {
                s.CreatedBy = $"{ViewState[vsUserName]}";
                Result = s.Insert();
                if (Result.ContainErrorMessage()) return Result;
            }
            else
            {
                s.ModifiedBy = $"{ViewState[vsUserName]}";
                Result = s.Update();
                if (Result.ContainErrorMessage()) return Result;
            }
            #endregion
        }
        #endregion

        #endregion

        #region Stock Out Retur Attachment
        string TableName = "StockOutReturAttachment";
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
        StockOutRetur o = StockOutRetur.GetById(U.Id);
        StockOutItem soi = StockOutItem.GetById(ViewState[vsStockOutItemId].ToInt());
        StockReceivedItem sri = StockReceivedItem.GetById(soi.StockReceivedItemId);
        Stock s = Stock.GetByItemId(soi.ItemId);

        string Result = Attachment.DeleteByOwnerId(U.Id, "StockOutReturAttachment");
        if (Result.ContainErrorMessage())
        {
            U.ShowMessage($"Delete Failed : {Result}");
            return;
        }
        Result = StockOutRetur.Delete(U.Id);
        if (Result.ContainErrorMessage())
        {
            U.ShowMessage($"Delete Failed : {Result}");
            return;
        }

        #region Stock Out Item        
        soi.ReturQty -= o.Qty;
        soi.UnusedQty += o.Qty;
        Result = soi.UpdateRetur();
        if (Result.ContainErrorMessage())
        {
            U.ShowMessage(Result);
            return;
        }
        #endregion

        #region Stock Received Item
        sri.AvailableQty -= o.Qty;
        sri.StockOutReturQty -= o.Qty;
        Result = sri.UpdateStockOutRetur();
        if (Result.ContainErrorMessage())
        {
            U.ShowMessage(Result);
            return;
        }
        #endregion

        #region Stock Movement
        StockMovement sm = StockMovement.GetByStockOutReturId(U.Id);
        Result = sm.Delete();        
        if (Result.ContainErrorMessage())
        {
            U.ShowMessage(Result);
            return;
        }
        #endregion

        #region Stock                
        s.ItemId = soi.ItemId;
        s.Qty -= o.Qty;
        if (s.Id == 0)
        {
            s.CreatedBy = $"{ViewState[vsUserName]}";
            Result = s.Insert();
            if (Result.ContainErrorMessage())
            {
                U.ShowMessage(Result);
                return;
            }
        }
        else
        {
            s.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = s.Update();
            if (Result.ContainErrorMessage())
            {
                U.ShowMessage(Result);
                return;
            }
        }
        #endregion

        U.CloseUpdate("Stock Received has been Deleted Successfully");
    }
    #endregion            
}