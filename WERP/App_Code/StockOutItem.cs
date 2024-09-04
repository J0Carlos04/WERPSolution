using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using U = Utility;

public class StockOutItem
{
    static string Qry = "Select a.*, b.Code ItemCode, b.Name ItemName, d.Id WarehouseId, d.Name Warehouse, e.Id RackId, e.Name Rack, c.SKU, f.Code WorkOrderCode, b.UseSKU, g.HelpdeskId from StockOutItem a Left Join Item b On b.Id = a.ItemId  Left Join StockReceivedItem c On c.Id = a.StockReceivedItemId Left Join Warehouse d On d.Id = c.WarehouseId Left Join Rack e On e.Id = c.RackId Left Join WorkOrder f On f.Id = a.WorkOrderId Left Join StockOut g On g.Id = a.StockOutId";

    #region Properties
    #region Default
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "CreatedBy")]
    public string CreatedBy { get; set; }
    [Column(Field = "Created")]
    public DateTime Created { get; set; }
    [Column(Field = "ModifiedBy")]
    public string ModifiedBy { get; set; }
    [Column(Field = "Modified")]
    public DateTime Modified { get; set; }
    [Column(Field = "No")]
    public double No { get; set; }
    public bool IsChecked { get; set; }
    public string Mode { get; set; }
    #endregion

    #region Key
    [Column(Field = "StockOutId")]
    public int StockOutId { get; set; }
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }
    [Column(Field = "WorkOrderId")]
    public int WorkOrderId { get; set; }
    [Column(Field = "WarehouseId")]
    public int WarehouseId { get; set; }
    [Column(Field = "RackId")]
    public int RackId { get; set; }
    [Column(Field = "StockReceivedItemId")]
    public int StockReceivedItemId { get; set; }    
    #endregion

    #region Fields
    [Column(Field = "ItemCode", Title = "Item Code", SearchName = "d.Code", SortName = "d.Code")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName", Title = "Item Name", SearchName = "d.Name", SortName = "d.Name")]
    public string ItemName { get; set; }
    [Column(Field = "Warehouse", SearchName = "e.Name", SortName = "e.Name")]
    public string Warehouse { get; set; }
    [Column(Field = "Rack", SearchName = "f.Name", SortName = "f.Name")]
    public string Rack { get; set; }
    [Column(Field = "OutDate", SearchName = "b.OutDate", SortName = "b.OutDate")]
    public DateTime OutDate { get; set; }
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }    
    [Column(Field = "UnusedQty", SearchName = "a.UnusedQty", SortName = "a.UnusedQty")]
    public int UnusedQty { get; set; }
    [Column(Field = "UsedQty", SearchName = "a.UsedQty", SortName = "a.UsedQty")]
    public int UsedQty { get; set; }
    [Column(Field = "ReturQty", SearchName = "a.ReturQty", SortName = "a.ReturQty")]
    public int ReturQty { get; set; }
    [Column(Field = "ReturUsedQty", SearchName = "a.ReturUsedQty", SortName = "a.ReturUsedQty")]
    public int ReturUsedQty { get; set; }
    [Column(Field = "DisposalQty", SearchName = "a.DisposalQty", SortName = "a.DisposalQty")]
    public int DisposalQty { get; set; }
    [Column(Field = "SKU", SearchName = "e.SKU", SortName = "e.SKU")]
    public string SKU { get; set; }
    [Column(Field = "StockOutCode", SearchName = "b.Code", SortName = "b.Code")]
    public string StockOutCode { get; set; }
    [Column(Field = "WorkOrder", SearchName = "c.Code", SortName = "c.Code")]
    public string WorkOrder { get; set; }
    #endregion

    #region Additional
    [Column(Field = "HelpdeskId")]
    public int HelpdeskId { get; set; }
    [Column(Field = "UseSKU")]
    public bool UseSKU { get; set; }
    [Column(Field = "WorkOrderCode")]
    public string WorkOrderCode { get; set; }
    #endregion
    #endregion

    #region Methods
    #region Get Data
    public static StockOutItem GetById(object Id)
    {
        return (StockOutItem)DataAccess.GetSingleRowByQuery(0, $"{Qry} where a.Id = '{Id}'", new StockOutItem());
    }
    public static List<object> GetByStockOutId(object StockOutId)
    {
        return DataAccess.GetDataByQuery(0, $"{Qry} where a.StockOutId = '{StockOutId}' Order By a.Created", new StockOutItem());
    }
    public static List<object> GetByStockOutAndItemId(object StockOutId, object ItemId)
    {
        return DataAccess.GetDataByQuery(0, $"{Qry} where a.StockOutId = '{StockOutId}' and a.ItemId = '{ItemId}' and a.Id not IN (Select StockOutItemId from WorkOrderWorkUpdateItem where StockOutItemId = a.Id) ", new StockOutItem());
    }
    public static List<object> GetByWorkOrderId(object WorkOrderId)
    {
        return DataAccess.GetDataByQuery(0, $"{Qry} where a.WorkOrderId = '{WorkOrderId}'", new StockOutItem());
    }
    public static bool IsAvailableQtyExist(object Id)
    {
        StockOutItem si = (StockOutItem)DataAccess.GetSingleRowByQuery(0, $"select * from StockOutItem where AvailableQty > 0 and StockOutId = '{Id}'", new StockOutItem());
        if (si.Id == 0) return false;
        return true;
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockOutItemGetByCriteria", pList, out TotalRow, new StockOutItem());
    }      
    public static int GetUnusedQty(object StockOutId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select SUM(UnusedQty) from StockOutItem where StockOutId = '{StockOutId}' ").ToInt();
    }
    public static int GetUnusedQty(object StockOutId, object ItemId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select SUM(ISNULL(UnusedQty, 0)) from StockOutItem where StockOutId = {StockOutId} and ItemId = {ItemId} and ISNULL(UnusedQty, 0) <> 0 ").ToInt();
    }
    public static bool IsExist(object StockOutId, object ItemId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select Top 1 1 from StockOutItem where StockOutId = '{StockOutId}' and ItemId = '{ItemId}' ").ToBool();
    }    
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("StockOutId", StockOutId));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("ItemId", ItemId));                     
        pList.Add(new clsParameter("Qty", Qty));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockOutItemInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("StockOutId", StockOutId));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));            
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("UnusedQty", UnusedQty));
        pList.Add(new clsParameter("UsedQty", UsedQty));
        pList.Add(new clsParameter("ReturQty", ReturQty));
        pList.Add(new clsParameter("DisposalQty", DisposalQty));       
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockOutItemUpdate", pList);
    }
    public string UpdateUsedItem()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOutItem Set UsedQty = '{UsedQty}', UnUsedQty = '{UnusedQty}', ReturQty = '{ReturQty}', DisposalQty = {DisposalQty}, WorkOrderId = '{WorkOrderId}', ModifiedBy = '{ModifiedBy}', Modified = GETDATE() where Id = '{Id}' ");
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "StockOutItem");
    }    
    public string UpdateRetur()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOutItem Set UnusedQty = '{UnusedQty}', ReturQty = '{ReturQty}', ModifiedBy = '{ModifiedBy}', Modified = GETDATE() where Id = '{Id}' ");
    }
    public string UpdateQty()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOutItem Set Qty = '{Qty}', UnusedQty = '{UnusedQty}', UsedQty = '{UsedQty}', ReturQty = '{ReturQty}', ReturUsedQty = '{ReturUsedQty}', DisposalQty = '{DisposalQty}', ModifiedBy = '{U.GetUsername()}', Modified = GETDATE() where Id = '{Id}' ");
    }
    #endregion
    #endregion
}