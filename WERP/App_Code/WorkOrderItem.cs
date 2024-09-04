using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class WorkOrderItem
{
    static string Qry = "Select a.*, b.Id ItemId, b.Code ItemCode, b.Name ItemName, b.UseSKU from WorkOrderItem a Left Join Item b On b.Id = a.ItemId";

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
    [Column(Field = "WorkOrderId")]
    public int WorkOrderId { get; set; }
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }
    #endregion

    #region Fields  
    [Column(Field = "Seq")]
    public int Seq { get; set; }
    [Column(Field = "Qty")]
    public int Qty { get; set; }
    #endregion

    #region AdditionalFields
    [Column(Field = "ItemCode")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName")]
    public string ItemName { get; set; }
    [Column(Field = "UseSKU")]
    public bool UseSKU { get; set; }
    #endregion
    #endregion

    #region Methods
    #region Get Data
    public static List<object> GetByWorkOrderId(object WorkOrderId)
    {
        return DataAccess.GetDataByQuery(0, $"{Qry} where a.WorkOrderId = '{WorkOrderId}'", new WorkOrderItem());
    }
    public static List<object> GetByWorkOrderItem(object WorkOrderId, object ItemId)
    {
        return DataAccess.GetDataByQuery(0, $"{Qry} where a.WorkOrderId = '{WorkOrderId}' and ItemId = '{ItemId}'", new WorkOrderItem());
    }
    public static List<object> GetByHelpdeskId(object HelpddeskId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Id ItemId, b.Code ItemCode, b.Name ItemName from WorkOrderItem a Left Join Item b On b.Id = a.ItemId where a.WorkOrderId In (Select Id from WorkOrder where HelpdeskId = '{HelpddeskId}')", new WorkOrderItem());
    }
    public static List<object> StockoutLookupGetByHelpdeskId(object HelpddeskId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.ItemId, b.Code ItemCode, b.Name ItemName, SUM(a.Qty) Qty from WorkOrderItem a Left Join Item b On b.Id = a.ItemId where a.WorkOrderId In (Select Id from WorkOrder where HelpdeskId = '{HelpddeskId}') Group By a.ItemId, b.Code, b.Name", new WorkOrderItem());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("Qty", Qty));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "WorkOrderItemInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "WorkOrderItemUpdate", pList);
    }
    public static string Delete(object WorkOrderId)
    {
        return DataAccess.Delete(0, WorkOrderId, "WorkOrderId", "WorkOrderItem");
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "WorkOrderItem");
    }
    #endregion
    #endregion
}