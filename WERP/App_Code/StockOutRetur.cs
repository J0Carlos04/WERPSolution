using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;


public class StockOutRetur
{
    #region Properties
    static string Qry = "Select a.*, b.Code WorkOrderCode, e.Code ItemCode, e.Name ItemName from StockOutRetur a Left Join WorkOrder b on b.id = a.WorkOrderId Left Join StockOut c on c.Id = a.StockOutId Left Join StockOutItem d On d.Id = a.StockOutItemId Left Join Item e on e.Id = d.ItemId Left Join Users f On f.Id = a.ReceiverUserId";
    #region Fields
    [Column(Field = "Code", SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "WorkOrder", SearchName = "g.Code", SortName = "g.Code")]
    public string WorkOrder { get; set; }
    [Column(Field = "Reason", SearchName = "a.Reason", SortName = "a.Reason")]
    public string Reason { get; set; }
    [Column(Field = "ReturDate", SearchName = "a.ReturDate", SortName = "a.ReturDate")]
    public DateTime ReturDate { get; set; }    
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }
    [Column(Field = "Receiver", SearchName = "b.Name", SortName = "b.Name")]
    public string Receiver { get; set; }
    #endregion

    #region Key
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "WorkOrderId")]
    public int WorkOrderId { get; set; }
    [Column(Field = "StockReceivedItemId")]
    public int StockReceivedItemId { get; set; }
    [Column(Field = "StockOutId")]
    public int StockOutId { get; set; }
    [Column(Field = "StockOutItemId")]
    public int StockOutItemId { get; set; }
    [Column(Field = "ReceiverUserId")]
    public int ReceiverUserId { get; set; }
    #endregion

    #region Additional
    [Column(Field = "WorkOrderCode", Title = "Work Order Code", SearchName = "h.code", SortName = "h.code")]
    public string WorkOrderCode { get; set; }
    [Column(Field = "ItemCode", Title = "Item Code", SearchName = "d.Code", SortName = "d.Code")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName", Title = "Item Name", SearchName = "d.Name", SortName = "d.Name")]
    public string ItemName { get; set; }
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
        
    #endregion

    #region Methods
    #region Get Data
    public static StockOutRetur GetById(object Id)
    {
        return (StockOutRetur)DataAccess.GetSingleRowByQuery(0, $"{Qry} where a.Id = '{Id}'", new StockOutRetur());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from StockOutRetur", new StockOutRetur());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockOutReturGetByCriteria", pList, out TotalRow, new StockOutRetur());
    }
    public static List<object> GetByWorkOrderAndItem(object WorkOrderId, object ItemId)
    {
        return DataAccess.GetDataByQuery(0, $"{Qry} where a.WorkOrderId = '{WorkOrderId}' and e.Id = '{ItemId}' ", new StockOutRetur());
    }
    public static int GetTotalQtyByWorkOrderItem(object WorkOrderId, object ItemId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select SUM(a.Qty) from StockOutRetur a Left Join WorkOrder b On b.Id = a.WorkOrderId Left Join StockOutItem d On d.Id = a.StockOutItemId Left Join Item e on e.Id = d.ItemId where a.WorkOrderId = '{WorkOrderId}' and e.Id = '{ItemId}' ").ToInt();
    }
    public static int GetQty(object StockOutId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select SUM(Qty) from StockOutRetur where StockOutId = '{StockOutId}' ").ToInt();
    }
    #endregion
    #region ChangeData
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("StockOutId", StockOutId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("ReceiverUserId", ReceiverUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Reason", Reason));
        pList.Add(new clsParameter("ReturDate", ReturDate));
        pList.Add(new clsParameter("Qty", Qty));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockOutReturInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("StockOutId", StockOutId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("ReceiverUserId", ReceiverUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Reason", Reason));
        pList.Add(new clsParameter("ReturDate", ReturDate));
        pList.Add(new clsParameter("Qty", Qty));        
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockOutReturUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "StockOutRetur");
    }
    #endregion
    #endregion
}