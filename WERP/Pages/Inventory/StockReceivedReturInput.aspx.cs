using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.IO;
using DAL;

public partial class Pages_Inventory_StockReceivedReturInput : PageBase
{
    #region Fields
    private const string vsUserName = "Username";
    private const string TableName = "StockReceivedReturAttachment";
    private const string vsStockReceivedItemId = "StockReceivedItemId";
    private const string vsPendingQty = "PendingQty";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            LoadFirstTime();
        string Script = "InitSelect2StockReceivedReturInput();";
        F.PageContext.RegisterStartupScript(Script);
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnUpload":
                U.AddAttachment(fu, gvAttachment);
                break;
            case "btnSubmit":
                Submit();
                break;
            case "btnDelete":
                Delete();
                break;
        }
    }
    protected void Download(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnDownload":
                U.DownloadFile(btn, "lb_FileName");                
                break;            
        }
    }
    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {
            case "imbDeleteAttachment":
                U.DeleteAttachment(imb);
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
    protected void wSR_Close(object sender, F.WindowCloseEventArgs e)
    {        
        SetInitControl(e.CloseArgument);
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        ViewState[vsUserName] = U.GetUsername();
        if ($"{ViewState[vsUserName]}" == "") Response.Redirect(@"~\Pages\default.aspx");
        
        U.SetDropDownMasterData(ddlRequester, "Users");
        fu.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUpload.ClientID));

        if (U.StockReceivedItemId.IsNull())
        {
            pnlCodeLookup.Visible = true;
            btnSelectStockReceivedItemCode.OnClientClick = wSR.GetShowReference($"~/Pages/Inventory/SelectSR.aspx");
            btnSelectStockReceivedItemCode.OnClientClick += "return false;";
        }
        else
        {
            pnlCode.Visible = true;
            SetInitControl(U.StockReceivedItemId);
        }
        SetInitEdit();
    }
    private void SetInitControl(object StockReceivedItemId)
    {
        ViewState[vsStockReceivedItemId] = StockReceivedItemId;
        StockReceivedItem o = StockReceivedItem.GetById(StockReceivedItemId);
        tbItemCode.Value = o.ItemCode;
        tbItemCodeLookup.Value = o.ItemCode;
        tbItemName.Value = o.ItemName;
        tbAvailableQty.Value = $"{o.AvailableQty}";
        ViewState[vsPendingQty] = o.PendingQty;
        U.BindGrid(gvAttachment, new List<object> { new Attachment { Seq = 0, FileName = CNT.DataNotAvailable } });
    }
    private void SetInitEdit()
    {
        if (U.Id.IsNull())
        {
            btnDelete.Hidden = true;
            return;
        }
            
        StockReceivedRetur o = StockReceivedRetur.GetById(U.Id);
        tbCode.Value = o.Code;
        tbReason.Value = o.Reason;
        tbReturDate.Value = o.ReturDate.ToString("yyyy-MM-dd HH:mm");
        ddlRequester.SelectedValue = $"{o.RequesterUserId}";
        tbQty.Value = $"{o.Qty}";        
        tbAvailableQty.Value = (tbAvailableQty.Value.ToInt() + o.Qty).ToString();

        List<object> aList = Attachment.GetByOwnerID(TableName, o.Id);

        foreach (Attachment att in aList)
            U.SaveAttachmentToTempFolder(att);
        if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
        U.BindGrid(gvAttachment, aList);
    }
      
    private void Submit()
    {
        try
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
        catch (Exception ex)
        {
            U.ShowMessage(ex.Message);
        }        
    }
    private void Validation(Wrapping w)
    {
        if (tbCode.Value.IsEmpty()) w.RequiredValidation = "Retur Code";
        else if (DataAccess.IsFieldExist("StockReceivedRetur", "Code", tbCode.Value, U.Id)) w.ErrorValidation = $"Stock Received Retur with code : {tbCode.Value} already exist";
        if (tbReason.Value.IsEmpty()) w.RequiredValidation = "Retur Reason";
        if (tbItemCode.Value.IsEmpty() && tbItemCodeLookup.Value.IsEmpty()) w.RequiredValidation = "Item Code";
        if (ddlRequester.SelectedValue.IsEmpty()) w.RequiredValidation = "Requester";
        if (tbQty.Value.ToInt() > tbAvailableQty.Value.ToInt()) w.ErrorValidation = "Qty can't be bigger than the available qty";
        if (tbQty.Value.IsEmpty()) w.RequiredValidation = "Qty";

        List<object> aList = U.GetGridData(gvAttachment, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).Seq != 0);
        if (aList.Count == 0) w.RequiredValidation = "Attachment";

        StockReceivedItem sri = StockReceivedItem.GetById(ViewState[vsStockReceivedItemId].ToInt());
        StockReceivedRetur oDB = StockReceivedRetur.GetById(U.Id);
        if (oDB.Id != 0)
        {
            int PendingQty = sri.PendingQty - oDB.Qty + tbQty.Value.ToInt();
            if (PendingQty < 0)
                w.ErrorValidation = $"Min Retur qty = {System.Math.Abs(sri.PendingQty - oDB.Qty)} because some of the items retur have been sent back by the vendor";
        }        
    }    
    private string Save(Wrapping w)
    {
        #region Stock Received Retur        
        StockReceivedItem sri = StockReceivedItem.GetById(ViewState[vsStockReceivedItemId].ToInt());
        StockOrderItem soi = StockOrderItem.GetById(sri.StockOrderItemId);
        Stock s = Stock.GetByItemId(sri.ItemId);
        StockReceivedRetur o = new StockReceivedRetur();
        o.StockReceivedItemId = sri.Id;
        o.RequesterUserId = ddlRequester.SelectedValue.ToInt();
        o.Code = tbCode.Value;
        o.Reason = tbReason.Value;
        o.ReturDate = tbReturDate.Value.ToHTML5DateTime();
        o.Qty = tbQty.Value.ToInt();
                
        string Result = "";
        if (U.Id == null)
        {            
            o.CreatedBy = $"{ViewState[vsUserName]}";
            Result = o.Insert();
            if (Result.ContainErrorMessage()) return Result;
            o.Id = Result.ToInt();
        }
        else
        {
            StockReceivedRetur oDB = StockReceivedRetur.GetById(U.Id);
            sri.ReturQty -= oDB.Qty;
            sri.AvailableQty += oDB.Qty;
            soi.PendingQty -= oDB.Qty;
            soi.ReturQty -= oDB.Qty;
            s.Qty += oDB.Qty;

            o.Id = U.Id.ToInt();
            o.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = o.Update();
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion

        #region Stock Received Retur Attachment
        string TableName = "StockReceivedReturAttachment";
        Result = Attachment.DeleteByOwnerId(o.Id, TableName);
        if (Result.Contains("Error Message :")) return Result;

        foreach (Attachment att in Utility.GetGridData(gvAttachment, typeof(Attachment)).ListData)
        {
            att.Table = TableName;
            att.OwnerId = o.Id;
            att.Data = U.GetFile(att.FileNameUniq);            
            att.CreatedBy = $"{ViewState[vsUserName]}";
            Result = att.Insert(att);
            if (Result.Contains("Error Message :")) return Result;

            string Path = $@"{Server.MapPath("~")}\TempFiles\{att.FileNameUniq}";
            File.Delete(Path);
        }
        #endregion

        #region Stock Received Item        
        sri.ReturQty += o.Qty;
        sri.AvailableQty -= o.Qty;
        Result = sri.UpdateRetur();
        if (Result.ContainErrorMessage()) return Result;
        #endregion

        #region Stock Order Item        
        soi.PendingQty += o.Qty;
        soi.ReturQty += o.Qty;
        Result = soi.UpdateRetur();
        if (Result.ContainErrorMessage()) return Result;
        #endregion

        #region Stock         
        s.Qty -= o.Qty;
        Result = s.Update();
        #endregion

        #region Stock Movement
        StockMovement sm = StockMovement.GetByStockReceivedReturId(o.Id);
        sm.StockReceivedReturId = o.Id;
        sm.ItemId = sri.ItemId;
        sm.Qty = o.Qty;
        sm.MovementType = "StockReceivedRetur";
        sm.RequesterUserId = o.RequesterUserId;        
        if (sm.Id == 0)
        {
            sm.CreatedBy = $"{ViewState[vsUserName]}";
            Result = sm.Insert();
            if (Result.ContainErrorMessage()) return Result;
        }
        else
        {
            sm.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = sm.Update();
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion
        return "";
    }
    private void Delete()
    {
        StockReceivedRetur o = StockReceivedRetur.GetById(U.Id);
        StockReceivedItem sri = StockReceivedItem.GetById(o.StockReceivedItemId);
        StockOrderItem soi = StockOrderItem.GetById(sri.StockOrderItemId);
        Stock s = Stock.GetByItemId(sri.ItemId);

        string Result = Attachment.DeleteByOwnerId(o.Id, "StockReceivedReturAttachment");
        if (Result.ContainErrorMessage())
        {
            U.ShowMessageDelete(Result);
            return;
        }

        Result = StockReceivedRetur.Delete(o.Id);
        if (Result.ContainErrorMessage())
        {
            U.ShowMessageDelete(Result);
            return;
        }

        #region Stock Received Item
        sri.ReturQty -= o.Qty;
        sri.AvailableQty += o.Qty;
        Result = sri.UpdateRetur();
        if (Result.ContainErrorMessage())
        {
            U.ShowMessageDelete(Result);
            return;
        }
        #endregion

        #region Stock Order Item
        soi.ReturQty -= o.Qty;
        soi.PendingQty -= o.Qty;
        Result = soi.UpdateRetur();
        if (Result.ContainErrorMessage())
        {
            U.ShowMessageDelete(Result);
            return;
        }
        #endregion

        StockMovement sm = StockMovement.GetByStockReceivedReturId(o.Id);
        Result = sm.Delete();
        if (Result.ContainErrorMessage())
        {
            U.ShowMessageDelete(Result);
            return;
        }

        s.Qty += o.Qty;
        Result = s.Update();

        U.CloseUpdate("Retur has been deleted successfully");
    }
    #endregion
}