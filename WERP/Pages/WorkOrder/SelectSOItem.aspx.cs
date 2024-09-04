using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using Microsoft.Exchange.WebServices.Data;
using System.Collections;
using System.Linq;
using static CNT;

public partial class Pages_WorkOrder_SelectSOItem : PageBase
{
    #region Fields       
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Wrapping w = new Wrapping();
        List<object> soiList = U.GetGridData(gvData, typeof(StockOutItem)).ListData.FindAll(a => ((StockOutItem)a).IsChecked);
        if (soiList.Count.IsZero())
        {
            U.ShowMessage($"Please select the Item you want used ");
            return;
        }
            
        int idx = 0;
        string Result = $"{U.QSRowIndex}~";        
        foreach (StockOutItem soi in soiList)
        {            
            idx += 1;            
            if (soi.Qty == 0) w.ErrorValidation = $"Qty is required in Row {idx}";
            if (U.QSWorkType.Is(CNT.Retur) || U.QSWorkType.Is(CNT.Disposal))
            {
                if (soi.Qty > soi.UsedQty) w.ErrorValidation = $"{U.QSWorkType} Qty cant be bigger then Used Qty in Row {idx}";
            }
            else
            {
                if (soi.Qty > soi.UnusedQty) w.ErrorValidation = $"Qty cant be bigger then Unused Qty in Row {idx}";
            }
            if (soi.UseSKU)
            {
                if (soi.SKU.IsEmpty()) w.ErrorValidation = $"SKU is required in Row {idx}";
                if (soiList.Exists(a => ((StockOutItem)a).SKU.Is(soi.SKU) && ((StockOutItem)a).No < soi.No)) w.ErrorValidation = $"SKU : {soi.SKU} already taken in Row {idx}";
            }            
            Result = $"{Result}{soi.Id}-{soi.Qty}|";
        }       
        Result = Result.Remove(Result.Length - 1, 1);
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }
                
        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference(Result));
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddlSKU":
                SKUChanged(ddl);
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
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            DropDownList ddlSKU = (DropDownList)e.Row.FindControl("ddlSKU");
            ltrl_No.Text = (e.Row.RowIndex + 1).ToString();

            StockOutItem soiItem = (StockOutItem)e.Row.DataItem;

            ddlSKU.Visible = false;
            if (soiItem.UseSKU)
            {
                ddlSKU.Visible = true;
                List<object> soiList = StockOutItem.GetByStockOutAndItemId(U.QSStockOutId, soiItem.ItemId);
                ddlSKU.Items.Clear();
                ddlSKU.Items.Add("");
                if (Session[CNT.Session.WorkUpdateItem] == null)
                {
                    U.ShowMessage("Your Session is expired please reload this page");
                    return;
                }
                List<object> soiSessionList = (List<object>)Session[CNT.Session.WorkUpdateItem];
                foreach (StockOutItem soi in soiList)
                {
                    if (!soiSessionList.Exists(a => ((WorkOrderWorkUpdateItem)a).StockOutItemId.Is(soi.Id)))
                        ddlSKU.Items.Add(new ListItem(soi.SKU, soi.Id.ToText()));
                }                    
                ddlSKU.SetValue(soiItem.Id);
                SKUChanged(ddlSKU);
            }
            else
            {
                StockOutItem soi = StockOutItem.GetById(soiItem.Id);
                SetWarehouse(e.Row, soi);
            }
        }
    }       
    #endregion

    #region Methods
    private void LoadFirstTime()
    {                
        BindData();
    }        
    private void BindData()
    {
        if (Session[CNT.Session.WorkUpdate] == null) Response.Redirect(@"~\Pages\default.aspx");
        
        List<object> wowuList = (List<object>)Session[CNT.Session.WorkUpdate];
        List<object> wowuiList = new List<object>();
        foreach (WorkOrderWorkUpdate wowu in wowuList)
        {
            foreach (WorkOrderWorkUpdateItem wowui in wowu.Items)
                wowuiList.Add(wowui);
        }
        Session[CNT.Session.WorkUpdateItem] = wowuiList;
        WorkOrderWorkUpdate wowuCurrent = (WorkOrderWorkUpdate)wowuList.Find(a => ((WorkOrderWorkUpdate)a).No.Is(U.QSRowIndex.ToInt() + 1));

        StockOut so = StockOut.GetById(U.QSStockOutId);
        List<object> oList = new List<object>();
        if (so.HelpdeskId.IsNotZero())
        {
            List<object> soiList = StockOutItem.GetByStockOutId(U.QSStockOutId).FindAll(a => ((StockOutItem)a).UnusedQty.IsNotZero());
            List<object> woiList = WorkOrderItem.GetByWorkOrderId(U.QSWorkOrderId);            
            foreach (WorkOrderItem woi in woiList)
            {
                int UsedQty = wowuiList.Where(a => ((WorkOrderWorkUpdateItem)a).ItemCode.Is(woi.ItemCode)).Sum(b => ((WorkOrderWorkUpdateItem)b).Qty);
                int UnusedQty = woi.Qty - UsedQty;
                List<object> soiCurrentList = soiList.FindAll(a => ((StockOutItem)a).ItemId.Is(woi.ItemId));
                if (woi.UseSKU)
                {
                    for (int i = 0; i < UnusedQty; i++)
                    {
                        StockOutItem soi = (StockOutItem)soiCurrentList[0];
                        oList.Add(new StockOutItem { StockOutId = so.Id, ItemId = woi.ItemId, ItemCode = woi.ItemCode, ItemName = woi.ItemName, WorkOrderId = U.QSWorkOrderId.ToInt(), UnusedQty = 1, UseSKU = woi.UseSKU });
                        soiList.RemoveAt(0);
                    }
                }
                else
                {
                    int Qty = UnusedQty;
                    int TotalUnsedQty = 0;
                    while (Qty != 0)
                    {                        
                        StockOutItem soi = (StockOutItem)soiCurrentList[0];                        
                        StockOutItem soiAdded = new StockOutItem { Id = soi.Id, StockReceivedItemId = soi.StockReceivedItemId, StockOutId = so.Id, ItemId = woi.ItemId, ItemCode = woi.ItemCode, ItemName = woi.ItemName, WorkOrderId = U.QSWorkOrderId.ToInt(), UnusedQty = soi.UnusedQty, UseSKU = woi.UseSKU };                                       
                        if (TotalUnsedQty + soi.UnusedQty > UnusedQty) soiAdded.UnusedQty -= UnusedQty;
                        int TotalCurrentUnusedQty = wowuiList.Where(a => ((WorkOrderWorkUpdateItem)a).StockOutItemId.Is(soi.Id)).Sum(b => ((WorkOrderWorkUpdateItem)b).Qty);
                        if (TotalCurrentUnusedQty.IsNotZero() && (soi.UnusedQty - soiAdded.UnusedQty) <= 0)
                        {
                            soiCurrentList.RemoveAt(0);
                            continue;
                        }
                            
                        TotalUnsedQty += soiAdded.UnusedQty;
                        Qty -= soi.UnusedQty;
                        if (Qty < 0) Qty = 0;
                        oList.Add(soiAdded);
                        soiCurrentList.RemoveAt(0);
                    }
                    
                    
                }                              
            }
        }
        else
            oList = StockOutItem.GetByStockOutId(U.QSStockOutId);
           
        U.BindGrid(gvData, oList);        
    }
    private void SKUChanged(DropDownList ddl)
    {
        Literal ltrl_Id = (Literal)ddl.FindControl("ltrl_Id");
        Literal ltrl_StockReceivedItemId = (Literal)ddl.FindControl("ltrl_StockReceivedItemId");
        Literal ltrl_WarehouseId = (Literal)ddl.FindControl("ltrl_WarehouseId");
        Literal ltrl_RackId = (Literal)ddl.FindControl("ltrl_RackId");
        Literal ltrl_Warehouse = (Literal)ddl.FindControl("ltrl_Warehouse");
        Literal ltrl_Rack = (Literal)ddl.FindControl("ltrl_Rack");
        Literal ltrl_SKU = (Literal)ddl.FindControl("ltrl_SKU");

        if (ddl.SelectedValue.IsEmpty())
        {
            ltrl_StockReceivedItemId.Text = "0";
            ltrl_WarehouseId.Text = "0";
            ltrl_RackId.Text = "0";
            ltrl_Warehouse.Text = "";
            ltrl_Rack.Text = "";
            return;
        }

        StockOutItem soi = StockOutItem.GetById(ddl.SelectedValue);
        ltrl_Id.Text = soi.Id.ToText();
        ltrl_StockReceivedItemId.Text = soi.StockReceivedItemId.ToText();
        ltrl_WarehouseId.Text = soi.WarehouseId.ToText();
        ltrl_RackId.Text = soi.RackId.ToText();
        ltrl_Warehouse.Text = soi.Warehouse;
        ltrl_Rack.Text = soi.Rack;
        ltrl_SKU.Text = ddl.SelectedItem.Text;
    }
    private void SetWarehouse(GridViewRow row, StockOutItem soi)
    {
        Literal ltrl_Id = (Literal)row.FindControl("ltrl_Id");
        Literal ltrl_StockReceivedItemId = (Literal)row.FindControl("ltrl_StockReceivedItemId");
        Literal ltrl_WarehouseId = (Literal)row.FindControl("ltrl_WarehouseId");
        Literal ltrl_RackId = (Literal)row.FindControl("ltrl_RackId");
        Literal ltrl_Warehouse = (Literal)row.FindControl("ltrl_Warehouse");
        Literal ltrl_Rack = (Literal)row.FindControl("ltrl_Rack");
        
        ltrl_Id.Text = soi.Id.ToText();
        ltrl_StockReceivedItemId.Text = soi.StockReceivedItemId.ToText();
        ltrl_WarehouseId.Text = soi.WarehouseId.ToText();
        ltrl_RackId.Text = soi.RackId.ToText();
        ltrl_Warehouse.Text = soi.Warehouse;
        ltrl_Rack.Text = soi.Rack;
    }
    #endregion    
}