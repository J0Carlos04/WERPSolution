using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using U = Utility;

public class WorkOrderWorkUpdateItem
{
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
    [Column(Field = "ReferenceId")]
    public int ReferenceId { get; set; }
    [Column(Field = "WorkOrderWorkUpdateId")]
    public int WorkOrderWorkUpdateId { get; set; }
    [Column(Field = "StockOutItemId")]
    public int StockOutItemId { get; set; }
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }    
    #endregion

    #region Fields    
    [Column(Field = "ItemCode")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName")]
    public string ItemName { get; set; }
    [Column(Field = "Qty")]
    public int Qty { get; set; }
    [Column(Field = "ReturQty")]
    public int ReturQty { get; set; }
    [Column(Field = "DisposalQty")]
    public int DisposalQty { get; set; }
    [Column(Field = "SKU")]
    public string SKU { get; set; }
    #endregion

    #region Additional 
    [Column(Field = "Seq")]
    public int Seq { get; set; }
    public string WorkDetail { get; set; }
    public DateTime WorkUpdateDate { get; set; }
    public int UsedQty { get; set; }
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static WorkOrderWorkUpdateItem GetById(object Id)
    {
        return (WorkOrderWorkUpdateItem)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Id ItemId, b.Code ItemCode, b.Name ItemName from WorkOrderWorkUpdateItem a Left Join Item b On b.Id = a.ItemId where a.Id = '{Id}'", new WorkOrderWorkUpdateItem());
    }
    public static WorkOrderWorkUpdateItem GetByStockOutItemIdAndWorkOrderWorkUpdateId(object StockOutItemId, object WorkOrderWorkUpdateId)
    {
        return (WorkOrderWorkUpdateItem)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Code ItemCode, b.Name ItemName from WorkOrderWorkUpdateItem a Left Join Item b On b.Id = a.ItemId where a.StockOutItemId = '{StockOutItemId}' and WorkOrderWorkUpdateId <> '{WorkOrderWorkUpdateId}' ", new WorkOrderWorkUpdateItem());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from WorkOrderWorkUpdateItem", new WorkOrderWorkUpdateItem());
    }
    public static List<object> GetByWorkOrderId(object WorkOrderId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Code ItemCode, b.Name ItemName from WorkOrderWorkUpdateItem a Left Join Item b On b.Id = a.ItemId where a.WorkOrderWorkUpdateId In (Select Id from WorkOrderWorkUpdate where WorkOrderId = '{WorkOrderId}')", new WorkOrderWorkUpdateItem());
    }
    public static List<object> GetByWorkOrderWorkUpdateId(object WorkOrderWorkUpdateId, bool IncludeReturUsed = true)
    {
        if (IncludeReturUsed)
            return DataAccess.GetDataByQuery(0, $"Select a.*, b.Code ItemCode, b.Name ItemName from WorkOrderWorkUpdateItem a Left Join Item b On b.Id = a.ItemId where WorkOrderWorkUpdateId = '{WorkOrderWorkUpdateId}' ", new WorkOrderWorkUpdateItem());
        else
            return DataAccess.GetDataByQuery(0, $"Select a.*, b.Code ItemCode, b.Name ItemName from WorkOrderWorkUpdateItem a Left Join Item b On b.Id = a.ItemId where WorkOrderWorkUpdateId = '{WorkOrderWorkUpdateId}' and a.Id not in (Select WorkOrderWorkUpdateItemId from StockOutReturUsed where Id <> a.Id)", new WorkOrderWorkUpdateItem());
    }
    public static bool IsUseItemExistByWorkOrder(object WorkOrderId)
    {
        return DataAccess.IsDataExist(0, $"Select * from WorkOrderWorkUpdateItem where WorkOrderWorkUpdateId in (Select Id From WorkOrderWorkUpdate where WorkOrderId = '{WorkOrderId}') and Qty <> ISNULL(ReturQty, 0)").ToBool();
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("ReferenceId", ReferenceId));
        pList.Add(new clsParameter("WorkOrderWorkUpdateId", WorkOrderWorkUpdateId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("ReturQty", ReturQty));
        pList.Add(new clsParameter("DisposalQty", DisposalQty));
        pList.Add(new clsParameter("SKU", SKU));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "WorkOrderWorkUpdateItemInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("ReferenceId", ReferenceId));
        pList.Add(new clsParameter("WorkOrderWorkUpdateId", WorkOrderWorkUpdateId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("ReturQty", ReturQty));
        pList.Add(new clsParameter("DisposalQty", DisposalQty));
        pList.Add(new clsParameter("SKU", SKU));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "WorkOrderWorkUpdateItemUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "WorkOrderWorkUpdateItem");
    }
    public string UpdateQty()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update WorkOrderWorkUpdateItem Set Qty = '{Qty}', ReturQty = '{ReturQty}', DisposalQty = '{DisposalQty}', ModifiedBy = '{U.GetUsername()}', Modified = GETDATE() where Id = '{Id}'  ");
    }
    #endregion

    #endregion
}