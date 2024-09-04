using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using Microsoft.Exchange.WebServices.Data;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Reflection;
using DAL;
using System.Collections;

public partial class Pages_Inventory_StockReceivedInput : PageBase
{
    #region Fields
    private const string vsUserName = "Username";
    private const string vsStockOrderId = "StockOrderId";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            LoadFirstTime();
        string Script = "InitSelect2StockReceivedInput();";
        F.PageContext.RegisterStartupScript(Script);
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnUploadInvoice":
                U.UploadSingleFile(fuInvoice, lbInvoice);                
                break;
            case "btnDownloadInvoice":
                U.OpenFile(lbInvoice.Text, U.GetContentType(lbInvoice.Text), U.GetFile($"{ViewState[vsUserName]}_{lbInvoice.Text}"));
                break;
            case "btnUploadBast":
                U.UploadSingleFile(fuBast, lbBast);                
                break;
            case "btnDownloadBast":
                U.OpenFile(lbBast.Text, U.GetContentType(lbBast.Text), U.GetFile($"{ViewState[vsUserName]}_{lbBast.Text}"));
                break;
            case "btnDeleteItem":
                DeleteItem();
                break;
            case "btnSubmit":
                Submit();
                break;
            case "btnDelete":
                Delete();
                break;
        }
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddl_WarehouseId":
                WarehouseChanged(ddl);
                break;
        }
    }
    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {
            case "imbDeleteInvoice":
                U.DeleteSingleFile(lbInvoice);
                break;
            case "imbDeleteBast":
                U.DeleteSingleFile(lbBast);
                break;                        
            case "imbSeries":
                GeneratedSKU(imb);
                break;
        }
    }
    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, ((GridView)sender).ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            TextBox tb_SKU = (TextBox)e.Row.FindControl("tb_SKU");
            CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");
            DropDownList ddl_WarehouseId = (DropDownList)e.Row.FindControl("ddl_WarehouseId");
            DropDownList ddl_RackId = (DropDownList)e.Row.FindControl("ddl_RackId");
            ltrl_No.Text = (e.Row.RowIndex + 1).ToText();
            U.SetDropDownMasterData(ddl_WarehouseId, "Warehouse");

            StockReceivedItem o = (StockReceivedItem)e.Row.DataItem;

            if (o.Id.IsNotZero())
            {
                if (o.ReturQty.IsNotZero() || o.StockOutQty.IsNotZero() || o.StockOutReturQty.IsNotZero())
                    cb_IsChecked.Visible = false;
            }
            if (o.WarehouseId != 0)
            {
                ddl_WarehouseId.SelectedValue = $"{o.WarehouseId}";
                U.SetDropDownRack(ddl_RackId, o.WarehouseId);
                ddl_RackId.SelectedValue = $"{o.RackId}";
            }
        }
    }
    protected void wSO_Close(object sender, F.WindowCloseEventArgs e)
    {
        SetInitControl(e.CloseArgument);
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        ViewState[vsUserName] = U.GetUsername();
        if ($"{ViewState[vsUserName]}" == "") Response.Redirect(@"~\Pages\default.aspx");
        if (!U.CreateAccess("StockReceived")) Response.Redirect($"../{CNT.Unauthorized}.aspx");

        U.SetDropDownMasterData(ddlReceiverUserId, "Users");
        fuInvoice.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUploadInvoice.ClientID));
        fuBast.Attributes.Add("onchange", string.Format("ClientChanged('{0}');", btnUploadBast.ClientID));
        lbInvoice.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownloadInvoice.ClientID));
        lbBast.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownloadBast.ClientID));

        if (U.StockOrderId.IsNull())
        {
            pnlCodeLookup.Visible = true;
            btnSelectStockOrder.OnClientClick = wSO.GetShowReference($"~/Pages/Inventory/SelectSO.aspx");
            btnSelectStockOrder.OnClientClick += "return false;";
            U.BindGrid(gvItems, new List<object> { new StockReceivedItem { ItemCode = CNT.DataNotAvailable } });
        }
        else
        {            
            pnlCode.Visible = true;
            SetInitControl(U.StockOrderId);
        }
        SetInitEdit();
    }
    private void SetInitControl(object StockOrderId)
    {        
        ViewState[vsStockOrderId] = StockOrderId;
        StockOrder o = StockOrder.GetById(StockOrderId);
        tbCode.Value = o.Code;
        tbCodeLookup.Value = o.Code;
        tbDescription.Value = o.Description;
        tbRequester.Value = o.Requester;
        tbProcurementType.Value = o.ProcurementType;
        U.Hide(CNT.DV.ReceivedItem.PO);        
        if (o.ProcurementType == "Regular")
        {
            U.Display(CNT.DV.ReceivedItem.PO);
            tbPO.Value = o.PONo;
        }
        tbVendor.Value = o.Vendor;
        tbApprover.Value = o.Approver;
        tbStatus.Value = o.Status;

        List<object> sriDBList = StockReceivedItem.GetByStockOrderId(o.Id);
        List<object> sriList = new List<object>();
        
        foreach (StockReceivedItem sri in sriDBList)
        {
            if (sri.UseSKU)
            {                  
                for (int i = 0; i < sri.PendingQty; i++)
                {
                    StockReceivedItem sriNew = sri.Clone();
                    if (i.IsZero()) sriNew.ShowFirstSKU = true;
                    else sriNew.ShowFirstSKU = false;
                    sriNew.ReceivedQty = 1;
                    sriList.Add(sriNew);
                }
            }
            else sriList.Add(sri);
        }
        U.BindGrid(gvItems, sriList);
        if (sriList.IsEmptyList() && U.Id.IsEmpty())
        {
            tfInformation.Text = "This Stock Order has been fully received";
            btnSubmit.Hidden = true;
        }
            
    }
    private void SetInitEdit()
    {
        if (U.Id.IsNull())
        {
            btnDelete.Hidden = true;
            return;
        }
        else if (StockReceived.IsReceivedItemInUsed(U.Id)) btnDelete.Hidden = true;
        StockReceived sr = StockReceived.GetById(U.Id);
        ddlReceiverUserId.SelectedValue = $"{sr.ReceiverUserId}";
        tbReceivedDate.Value = sr.ReceivedDate.ToString("yyyy-MM-dd HH:mm");
        tbInvoiceNo.Value = sr.InvoiceNo;
        tbInvoiceDate.Value = sr.InvoiceDate.ToString("yyyy-MM-dd");

        if (sr.InvoiceFileName.IsNotEmpty())
        {
            Attachment att = new Attachment { FileName = sr.InvoiceFileName, Data = sr.InvoiceFileData };
            U.SaveAttachmentToTempFolder(att);
            lbInvoice.Text = sr.InvoiceFileName;
            lbInvoice.CommandName = att.FileNameUniq;
        }        

        if (sr.BastFileName.IsNotEmpty())
        {
            Attachment att = new Attachment { FileName = sr.BastFileName, Data = sr.BastFileData };
            U.SaveAttachmentToTempFolder(att);
            lbBast.Text = sr.BastFileName;
            lbBast.CommandName = att.FileNameUniq;
        }
                
        List<object> siList = StockReceivedItem.GetByStockReceivedId(sr.Id);
        U.BindGrid(gvItems, siList);
    }
    private void WarehouseChanged(DropDownList ddl)
    {
        DropDownList ddl_RackId = (DropDownList)ddl.FindControl("ddl_RackId");
        ddl_RackId.Items.Clear();
        if (ddl.SelectedValue == "") return;
        U.SetDropDownRack(ddl_RackId, ddl.SelectedValue);
    }
    private void DeleteItem()
    {
        List<object> oList = U.GetGridData(gvItems, typeof(StockReceivedItem)).ListData;
        if (U.ShowError(oList.FindAll(a => ((StockReceivedItem)a).IsChecked).Count.IsZero(), "Please select item you want to delete")) return;
        oList.RemoveAll(a => ((StockReceivedItem)a).IsChecked);
        if (U.ShowError(oList.Count.IsZero(), "Requires at least 1 item to do stock received")) return;
        U.BindGrid(gvItems, oList);             
    }
    private void GeneratedSKU(ImageButton imb)
    {        
        List<object> sriList = U.GetGridData(gvItems, typeof(StockReceivedItem)).ListData;
        string SKU = ((TextBox)imb.FindControl("tb_SKU")).Text;
        if (U.ShowError(SKU.IsEmpty(), "SKU is required")) return;
        Literal ltrl_ItemId = (Literal)imb.FindControl("ltrl_ItemId");
        DropDownList ddl_WarehouseId = (DropDownList)imb.FindControl("ddl_WarehouseId");
        DropDownList ddl_RackId = (DropDownList)imb.FindControl("ddl_RackId");
        char LastChar = SKU.Last();
        int Index = -1;
        string newLastChar = "";
        foreach (StockReceivedItem sri in sriList.FindAll(a => ((StockReceivedItem)a).ItemId.Is(ltrl_ItemId.Text)))
        {            
            Index++;
            if (Index.IsZero()) continue;
            if (char.IsDigit(LastChar))
            {
                int numericStartIndex = SKU.Select((ch, idx) => new { ch, idx }).LastOrDefault(x => !char.IsDigit(x.ch))?.idx + 1 ?? 0;
                string prefix = SKU.Substring(0, numericStartIndex);
                string numericPart = SKU.Substring(numericStartIndex);
                int numericValue = int.Parse(numericPart) + Index;
                string newNumericPart = numericValue.ToString().PadLeft(numericPart.Length, '0');
                sri.SKU = prefix + newNumericPart;               
            }
            else if (char.IsLetter(LastChar))
            {
                if (LastChar == 'z') newLastChar = "a";                
                else if (LastChar == 'Z') newLastChar = "A";
                else
                    newLastChar = ((char)(LastChar + Index)).ToString();
                sri.SKU = SKU.Substring(0, SKU.Length - 1) + newLastChar;
            }
            sri.WarehouseId = ddl_WarehouseId.SelectedValue.ToInt();
            sri.RackId = ddl_RackId.SelectedValue.ToInt();
        }
        U.BindGrid(gvItems, sriList);
    }    

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
        if (ViewState[vsStockOrderId].ToInt().IsZero()) w.RequiredValidation = "Stock Order";
        if (ddlReceiverUserId.SelectedValue == "") w.RequiredValidation = "Receiver";
        if (tbReceivedDate.Value.IsEmpty()) w.RequiredValidation = "Received Date";
        //if (tbInvoiceNo.Value.IsEmpty()) w.RequiredValidation = "Invoice No";
        //if (tbInvoiceDate.Value.IsEmpty()) w.RequiredValidation = "Invoice Date";
        //if (lbInvoice.Text.IsEmpty()) w.RequiredValidation = "Invoice Attachment";
        if (lbBast.Text.IsEmpty()) w.RequiredValidation = "Bast Attachment";
        w.ListData = U.GetGridData(gvItems, typeof(StockReceivedItem)).ListData;
        
        if (w.ListData.Count(a => ((StockReceivedItem)a).ReceivedQty == 0) == w.ListData.Count)
            w.ErrorValidation = "Please fill at least 1 Received Qty";
        else
        {
            int idx = 0;
            List<int> ItemIdList = w.ListData.Select(a => ((StockReceivedItem)a).ItemId).Distinct().ToList();
            List<object> sriList = new List<object>();
            foreach (int ItemId in ItemIdList)
            {
                List<object> sriDBList = StockReceivedItem.GetByItemId(ItemId);
                foreach (StockReceivedItem sriDB in sriDBList)
                {
                    if (!w.ListData.Exists(a => ((StockReceivedItem)a).Id.Is(sriDB.Id)))
                        sriList.Add(sriDB);
                }
                    
            }
            foreach (StockReceivedItem o in w.ListData)
            {
                idx += 1;
                if (o.WarehouseId == 0) w.ErrorValidation = $"Warehouse is required at row {idx}";
                if (o.RackId == 0) w.ErrorValidation = $"Rack is required at row {idx}";
                if (o.Id == 0)
                {
                    if (o.ReceivedQty > o.PendingQty) w.ErrorValidation = $"Received Qty can't bigger then Pending Qty at row {idx}";
                }
                else
                {
                    StockOrderItem soi = StockOrderItem.GetById(o.StockOrderItemId);
                    StockReceivedItem oDB = StockReceivedItem.GetById(o.Id);
                    soi.PendingQty += oDB.ReceivedQty;
                    StockReceivedRetur srr = StockReceivedRetur.GetByStockReceivedItemId(o.Id);
                    soi.PendingQty -= srr.Qty;

                    if (o.ReceivedQty > soi.PendingQty) w.ErrorValidation = $"Received Qty can't bigger then Pending Qty at row {idx}";
                    if (o.ReceivedQty < srr.Qty) w.ErrorValidation = $"Received Qty can't less then Retur Qty ({srr.Qty}) at row {idx}";
                }
                if (o.UseSKU)
                {
                    if (o.SKU.IsEmpty()) w.ErrorValidation = $"SKU is required at row {idx}";
                    else
                    {
                        if (w.ListData.Exists(a => ((StockReceivedItem)a).SKU.Is(o.SKU) && ((StockReceivedItem)a).No < o.No)) w.ErrorValidation = $"SKU is already exist at row {idx}";
                        else
                        {
                            if (U.Id.IsZero())
                            {
                                if (StockReceivedItem.IsSKUExist(o.ItemId, o.SKU, o.Id)) w.ErrorValidation = $"SKU is already exist at row {idx}";
                            }
                            else if (sriList.Exists(a => ((StockReceivedItem)a).ItemId.Is(o.ItemId) && ((StockReceivedItem)a).SKU.Is(o.SKU))) w.ErrorValidation = $"SKU is already exist at row {idx}";                            
                        }                        
                    }
                    
                }                    
            }
        }

    }
    private string Save(Wrapping w)
    {
        #region Stock Received
        string Result = "";
        StockReceived sr = new StockReceived();
        sr.StockOrderId = ViewState[vsStockOrderId].ToInt();
        sr.ReceiverUserId = ddlReceiverUserId.SelectedValue.ToInt();
        sr.ReceivedDate = tbReceivedDate.Value.ToHTML5DateTime();
        sr.InvoiceNo = tbInvoiceNo.Value;
        if (tbInvoiceDate.Value.IsNotEmpty())
            sr.InvoiceDate = tbInvoiceDate.Value.ToHTML5Date();
        if (lbInvoice.Text.IsNotEmpty())
        {
            sr.InvoiceFileName = lbInvoice.Text;
            sr.InvoiceFileData = U.GetFile(lbInvoice.CommandName);
        }
        if (lbBast.Text.IsNotEmpty())
        {
            sr.BastFileName = lbBast.Text;
            sr.BastFileData = U.GetFile(lbBast.CommandName);
        }        
        if (U.Id.IsNull())
        {
            sr.CreatedBy = $"{ViewState[vsUserName]}";
            Result = sr.Insert();
            if (Result.ContainErrorMessage()) return Result;
            sr.Id = Result.ToInt();
        }
        else
        {
            sr.ModifiedBy = $"{ViewState[vsUserName]}";
            sr.Id = U.Id.ToInt();
            Result = sr.Update();
            if (Result.ContainErrorMessage()) return Result;
        }        
        #endregion

        #region Stock Received Item
        List<object> ItemDBList = StockReceivedItem.GetByStockReceivedId(sr.Id);
        foreach (StockReceivedItem sri in ItemDBList)
        {
            if (!w.ListData.Exists(a => ((StockReceivedItem)a).Id.Is(sri.Id)))
            {                
                StockMovement sm = StockMovement.GetByStockReceivedItemId(sri.Id);
                if (sm.Delete().ContainErrorMessage(out Result)) return Result;

                Stock s = Stock.GetByItemId(sri.ItemId);                
                s.Qty -= sri.ReceivedQty;
                s.ModifiedBy = $"{ViewState[vsUserName]}";
                if (s.Update().ContainErrorMessage(out Result)) return Result;                

                StockOrderItem soi = StockOrderItem.GetById(sri.StockOrderItemId);
                soi.PendingQty += sri.ReceivedQty;
                soi.ReceivedQty -= sri.ReceivedQty;
                soi.ModifiedBy = $"{ViewState[vsUserName]}";
                if (soi.Update().ContainErrorMessage(out Result)) return Result;

                Result = sri.Delete();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        foreach (StockReceivedItem o in w.ListData)
        {
            o.StockReceivedId = sr.Id;

            StockMovement sm = new StockMovement();
            StockOrderItem soi = StockOrderItem.GetById(o.StockOrderItemId);

            #region Insert
            if (o.Id == 0)
            {
                o.AvailableQty += o.ReceivedQty;
                o.CreatedBy = $"{ViewState[vsUserName]}";
                Result = o.Insert();
                if (Result.ContainErrorMessage()) return Result;
                o.Id = Result.ToInt();

                #region Stock Movement            
                sm.StockReceivedItemId = o.Id;
                sm.ItemId = o.ItemId;
                sm.RequesterUserId = sr.ReceiverUserId;
                sm.Qty = o.ReceivedQty;
                sm.MovementType = "StockReceived";
                sm.MovementDate = tbReceivedDate.Value.ToHTML5DateTime();
                sm.CreatedBy = $"{ViewState[vsUserName]}";
                Result = sm.Insert();
                if (Result.ContainErrorMessage()) return Result;
                #endregion

                #region Stock
                Stock s = Stock.GetByItemId(o.ItemId);
                s.ItemId = o.ItemId;
                s.Qty += o.ReceivedQty;
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
                StockReceivedItem oDB = StockReceivedItem.GetById(o.Id);
                o.AvailableQty -= oDB.ReceivedQty;
                o.AvailableQty += o.ReceivedQty;
                o.ModifiedBy = $"{ViewState[vsUserName]}";

                soi.PendingQty += oDB.ReceivedQty;
                soi.ReceivedQty -= oDB.ReceivedQty;

                Result = o.Update();
                if (Result.ContainErrorMessage()) return Result;

                #region Stock Movement
                sm = StockMovement.GetByStockReceivedItemId(o.Id);
                sm.RequesterUserId = sr.ReceiverUserId;
                sm.Qty = o.ReceivedQty;
                sm.MovementDate = tbReceivedDate.Value.ToHTML5DateTime();
                sm.ModifiedBy = $"{ViewState[vsUserName]}";
                Result = sm.Update();
                if (Result.ContainErrorMessage()) return Result;
                #endregion

                #region Stock
                Stock s = Stock.GetByItemId(o.ItemId);
                s.ItemId = o.ItemId;                
                if (s.Id == 0)
                {
                    s.Qty += o.ReceivedQty;
                    s.CreatedBy = $"{ViewState[vsUserName]}";
                    Result = s.Insert();
                    if (Result.ContainErrorMessage()) return Result;
                }
                else
                {
                    s.Qty -= oDB.ReceivedQty;
                    s.Qty += o.ReceivedQty;
                    s.ModifiedBy = $"{ViewState[vsUserName]}";
                    Result = s.Update();
                    if (Result.ContainErrorMessage()) return Result;
                }
                #endregion
            }
            #endregion            

            soi.PendingQty -= o.ReceivedQty;
            soi.ReceivedQty += o.ReceivedQty;
            Result = soi.UpdateReceivedQty();
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion

        U.DeleteSingleFile(lbInvoice);
        U.DeleteSingleFile(lbBast);
        return Result;
    }
    private void Delete()
    {
        StockReceived sr = StockReceived.GetById(U.Id);
        List<object> oList = StockReceivedItem.GetByStockReceivedId(U.Id);        

        string Result = StockReceivedItem.DeleteByOwner(U.Id);
        if (Result.ContainErrorMessage())
        {
            U.ShowMessageDelete(Result);
            return;
        }

        Result = StockReceived.Delete(U.Id);
        if (Result.ContainErrorMessage())
        {
            U.ShowMessageDelete(Result);
            return;
        }

        foreach (StockReceivedItem o in oList)
        {
            StockOrderItem soi = StockOrderItem.GetById(o.StockOrderItemId);
            soi.PendingQty += o.ReceivedQty;
            soi.ReceivedQty -= o.ReceivedQty;
            soi.ModifiedBy = ViewState[CNT.VS.Username].ToText();
            if (soi.Update().ContainErrorMessage(out Result)) return;           

            #region Stock Movement            
            StockMovement sm = StockMovement.GetByStockReceivedItemId(o.Id); 
            if (sm.Delete().ContainErrorMessage(out Result)) return;            
            #endregion

            #region Stock
            Stock s = Stock.GetByItemId(o.ItemId);
            s.ItemId = o.ItemId;
            s.Qty -= o.ReceivedQty;
            s.ModifiedBy = $"{ViewState[vsUserName]}";
            if (s.Update().ContainErrorMessage(out Result)) return;           
            #endregion
        }
        U.CloseUpdate("Stock Received has been Deleted Successfully");

    }
    #endregion    
}