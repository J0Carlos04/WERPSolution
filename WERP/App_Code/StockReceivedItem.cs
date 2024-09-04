using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using U = Utility;

public class StockReceivedItem
{
    #region Properties

    #region Fields
    [Column(Field = "StockOrderCode", Title = "Stock Order Code", SearchName = "h.Code", SortName = "h.Code")]
    public string StockOrderCode { get; set; }
    [Column(Field = "ItemCode", Title = "Item Code", SearchName = "b.Code", SortName = "b.Code")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName", Title = "Item Name", SearchName = "b.Name", SortName = "b.Name")]
    public string ItemName { get; set; }
    [Column(Field = "Warehouse", SearchName = "c.Name", SortName = "c.Name")]
    public string Warehouse { get; set; }
    [Column(Field = "Rack", SearchName = "d.Name", SortName = "d.Name")]
    public string Rack { get; set; }
    [Column(Field = "ApprovedQty", SearchName = "g.ApprovedQty", SortName = "g.ApprovedQty")]
    public int ApprovedQty { get; set; }
    [Column(Field = "PendingQty", SearchName = "g.PendingQty", SortName = "g.PendingQty")]
    public int PendingQty { get; set; }
    [Column(Field = "ReceivedQty", SearchName = "a.ReceivedQty", SortName = "a.ReceivedQty")]
    public int ReceivedQty { get; set; }
    [Column(Field = "AvailableQty", SearchName = "a.AvailableQty", SortName = "a.AvailableQty")]
    public int AvailableQty { get; set; }
    [Column(Field = "ReturQty", SearchName = "a.ReturQty", SortName = "a.ReturQty")]
    public int ReturQty { get; set; }
    [Column(Field = "StockOutQty", SearchName = "a.StockOutQty", SortName = "a.StockOutQty")]
    public int StockOutQty { get; set; }
    [Column(Field = "StockOutReturQty", SearchName = "a.StockOutReturQty", SortName = "a.StockOutReturQty")]
    public int StockOutReturQty { get; set; }
    [Column(Field = "UnitPrice", SearchName = "a.UnitPrice", SortName = "a.UnitPrice")]
    public decimal UnitPrice { get; set; }
    [Column(Field = "Receiver", SearchName = "f.Name", SortName = "f.Name")]
    public string Receiver { get; set; }
    [Column(Field = "ReceivedDate", Title = "Received Date", SearchName = "e.ReceivedDate", SortName = "e.ReceivedDate")]
    public DateTime ReceivedDate { get; set; }
    [Column(Field = "InvoiceNo", Title = "Invoice No", SearchName = "e.InvoiceNo", SortName = "e.InvoiceNo")]
    public string InvoiceNo { get; set; }
    [Column(Field = "InvoiceDate", Title = "Invoice Date", SearchName = "e.InvoiceDate", SortName = "e.InvoiceDate")]
    public DateTime InvoiceDate { get; set; }
    [Column(Field = "SKU", Title = "SKU", SearchName = "SKU", SortName = "SKU")]
    public string SKU { get; set; }

    [Column(Field = "InvoiceFileName", SearchName = "e.InvoiceFileName", SortName = "e.InvoiceFileName")]
    public string InvoiceFileName { get; set; }
    [Column(Field = "BastFileName", SearchName = "e.BastFileName", SortName = "e.BastFileName")]
    public string BastFileName { get; set; }
    #endregion    

    #region Default

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
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "StockOrderId")]
    public int StockOrderId { get; set; }
    [Column(Field = "StockReceivedId")]
    public int StockReceivedId { get; set; }
    [Column(Field = "StockOrderItemId")]
    public int StockOrderItemId { get; set; }
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }
    [Column(Field = "WarehouseId")]
    public int WarehouseId { get; set; }
    [Column(Field = "RackId")]
    public int RackId { get; set; }
    #endregion

    #region Additional
    [Column(Field = "UseSKU")]
    public bool UseSKU { get; set; }
    public bool ShowFirstSKU { get; set; }
    #endregion

    #endregion

    #region Methods

    public StockReceivedItem Clone()
    {
        return (StockReceivedItem)this.MemberwiseClone();
    }

    #region Get Data
    public static StockReceivedItem GetById(object Id)
    {
        return (StockReceivedItem)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Code ItemCode, b.Name ItemName, c.Name Warehouse, d.Name Rack from StockReceivedItem a Left Join Item b On b.Id = a.ItemId Left Join Warehouse c On c.Id = a.WarehouseId Left Join Rack d On d.Id = a.RackId where a.Id = '{Id}'", new StockReceivedItem());
    }
    public static StockReceivedItem GetBySKU(object ItemId, object SKU)
    {
        return (StockReceivedItem)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Code ItemCode, b.Name ItemName from StockReceivedItem a Left Join Item b On b.Id = a.ItemId where a.ItemId = '{ItemId}' and SKU = '{SKU}'", new StockReceivedItem());
    }
    public static List<object> GetByStockOrderId(object StockOrderId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.Id StockOrderItemId, b.Id ItemId, b.Code ItemCode, b.Name ItemName, a.ApprovedQty, a.PendingQty, a.UnitPrice, b.UseSKU from StockOrderItem a Left Join Item b On b.Id = a.ItemId where PendingQty > 0 and a.StockOrderId = '{StockOrderId}' ", new StockReceivedItem());
    }
    public static List<object> GetByItemId(object ItemId)
    {
        return DataAccess.GetDataByQuery(0, $"select a.*, b.ReceivedDate, c.Name Warehouse, d.Name Rack from StockReceivedItem a Inner Join StockReceived b On b.Id = a.StockReceivedId Inner Join Warehouse c On c.Id = a.WarehouseId Inner Join Rack d On d.Id = a.RackId where ItemId = '{ItemId}' Order By b.ReceivedDate asc ", new StockReceivedItem());
    }
    public static List<object> GetAvailableByItemId(object ItemId)
    {
        return DataAccess.GetDataByQuery(0, $"select a.*, b.ReceivedDate, c.Name Warehouse, d.Name Rack from StockReceivedItem a Inner Join StockReceived b On b.Id = a.StockReceivedId Inner Join Warehouse c On c.Id = a.WarehouseId Inner Join Rack d On d.Id = a.RackId where ItemId = '{ItemId}' and AvailableQty <> 0 Order By b.ReceivedDate asc ", new StockReceivedItem());
    }
    public static List<object> GetByStockReceivedId(object StockReceivedId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.Id, a.StockOrderItemId, a.ItemId, b.Code ItemCode, b.Name ItemName, a.WarehouseId, a.RackId, c.ApprovedQty, c.PendingQty, a.ReceivedQty, a.AvailableQty, a.ReturQty, a.StockOutQty, a.StockOutReturQty, c.UnitPrice, a.SKU, b.UseSKU from StockReceivedItem a Left Join Item b On b.Id = a.ItemId Left Join StockOrderItem c On c.Id = a.StockOrderItemId where a.StockReceivedId = '{StockReceivedId}' ", new StockReceivedItem());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from StockReceivedItem", new StockReceivedItem());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockReceivedItemGetByCriteria", pList, out TotalRow, new StockReceivedItem());
    }
    public static bool IsSKUExist(object ItemId, string SKU)
    {
        return DataAccess.IsDataExist(0, $"select * from StockReceivedItem where AvailableQty > 0 and ItemId = {ItemId} and SKU = '{SKU}'").ToBool();
    }
    public static bool IsSKUExist(object ItemId, string SKU, object Id)
    {
        return DataAccess.IsDataExist(0, $"select * from StockReceivedItem where ItemId = {ItemId} and SKU = '{SKU}' and AvailableQty <> '{Id}' ").ToBool();
    }
    public static bool IsSKUInWarehouseExist(object ItemId, object WarehouseId, object RackId, string SKU)
    {
        return DataAccess.IsDataExist(0, $"select * from StockReceivedItem where ItemId = {ItemId} and WarehouseId = '{WarehouseId}' and RackId = '{RackId}' and AvailableQty <> 0 and SKU = '{SKU}'").ToBool();
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("StockReceivedId", StockReceivedId));
        pList.Add(new clsParameter("StockOrderItemId", StockOrderItemId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("WarehouseId", WarehouseId));
        pList.Add(new clsParameter("RackId", RackId));
        pList.Add(new clsParameter("ReceivedQty", ReceivedQty));
        pList.Add(new clsParameter("AvailableQty", AvailableQty));
        pList.Add(new clsParameter("UnitPrice", UnitPrice));
        pList.Add(new clsParameter("SKU", SKU));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "StockReceivedItemInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("StockReceivedId", StockReceivedId));
        pList.Add(new clsParameter("StockOrderItemId", StockOrderItemId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("WarehouseId", WarehouseId));
        pList.Add(new clsParameter("RackId", RackId));
        pList.Add(new clsParameter("ReceivedQty", ReceivedQty));
        pList.Add(new clsParameter("AvailableQty", AvailableQty));
        pList.Add(new clsParameter("ReturQty", ReturQty));
        pList.Add(new clsParameter("StockOutQty", StockOutQty));
        pList.Add(new clsParameter("StockOutReturQty", StockOutReturQty));
        pList.Add(new clsParameter("UnitPrice", UnitPrice));
        pList.Add(new clsParameter("SKU", SKU));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "StockReceivedItemUpdate", pList);
    }
    public string UpdateAvailableQty()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockReceivedItem Set AvailableQty = '{AvailableQty}' where Id = '{Id}' ");
    }
    public string UpdateRetur()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockReceivedItem Set ReturQty = {ReturQty}, AvailableQty = {AvailableQty} Where Id = {Id}");
    }
    public string UpdateStockOut()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockReceivedItem Set StockOutQty = {StockOutQty}, AvailableQty = {AvailableQty} Where Id = {Id}");
    }
    public string UpdateStockOutRetur()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockReceivedItem Set StockOutReturQty = {StockOutReturQty}, AvailableQty = {AvailableQty} Where Id = {Id}");
    }
    public string UpdateQty()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockReceivedItem Set ReceivedQty = '{ReceivedQty}', AvailableQty = '{AvailableQty}', ReturQty = '{ReturQty}', StockOutQty = '{StockOutQty}', StockOutReturQty = {StockOutReturQty}, ModifiedBy = {U.GetUsername()}, Modified = GETDATE() Where Id = {Id}");
    }
    public static string DeleteByOwner(object StockReceivedId)
    {
        return DataAccess.Delete(0, StockReceivedId, "StockReceivedId", "StockReceivedItem");
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "StockReceivedItem");
    }
    #endregion

    #endregion
}