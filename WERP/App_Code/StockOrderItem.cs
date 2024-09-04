using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class StockOrderItem
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
    [Column(Field = "StockOrderId")]
    public int StockOrderId { get; set; }
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }    
    #endregion

    #region Fields
    [Column(Field = "Code", Title = "Code", SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "Name", Title = "Name", SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }        
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }
    [Column(Field = "ApprovedQty", SearchName = "a.ApprovedQty", SortName = "a.ApprovedQty")]
    public int ApprovedQty { get; set; }
    [Column(Field = "PendingQty", SearchName = "a.PendingQty", SortName = "a.PendingQty")]
    public int PendingQty { get; set; }
    [Column(Field = "ReceivedQty", SearchName = "a.ReceivedQty", SortName = "a.ReceivedQty")]
    public int ReceivedQty { get; set; }
    [Column(Field = "ReturQty", SearchName = "a.ReturQty", SortName = "a.ReturQty")]
    public int ReturQty { get; set; }
    [Column(Field = "UnitPrice", SearchName = "a.UnitPrice", SortName = "a.UnitPrice")]
    public decimal UnitPrice { get; set; }    

    [Column(Field = "Status", SearchName = "a.Status", SortName = "a.Status")]
    public string Status { get; set; }
    [Column(Field = "StatusDate", SearchName = "a.StatusDate", SortName = "a.StatusDate")]
    public DateTime StatusDate { get; set; }
    [Column(Field = "RejectReason")]
    public string RejectReason { get; set; }
    [Column(Field = "IsCheckedApprove")]
    public bool IsCheckedApprove { get; set; }
    [Column(Field = "IsCheckedReject")]
    public bool IsCheckedReject { get; set; }
    #endregion

    #region Additional    
    #endregion
    #endregion

    #region Methods
    #region Get Data
    public static StockOrderItem GetById(object Id)
    {
        return (StockOrderItem)DataAccess.GetSingleRowByQuery(0, $"Select * from StockOrderItem where Id = '{Id}'", new StockOrderItem());
    }
    public static List<object> GetByParendId(object StockOrderId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Code, b.Name from StockOrderItem a Left Join Item b On b.Id = a.ItemId where a.StockOrderId = '{StockOrderId}'", new StockOrderItem());
    }
    public static bool IsPendingQtyExist(object Id)
    {
        StockOrderItem si = (StockOrderItem)DataAccess.GetSingleRowByQuery(0, $"select * from StockOrderItem where (Status = 'Approved' or Status = 'Partial Approved') and PendingQty > 0 and StockOrderId = '{Id}'", new StockOrderItem());
        if (si.Id == 0) return false;
        return true;
    }
    public int GetReceivedQty()
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select ReceivedQty from StockOrderItem where Id = '{Id}'").ToInt();
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("StockOrderId", StockOrderId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("UnitPrice", UnitPrice));             
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockOrderItemInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("StockOrderId", StockOrderId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("ApprovedQty", ApprovedQty));
        pList.Add(new clsParameter("PendingQty", PendingQty));
        pList.Add(new clsParameter("ReceivedQty", ReceivedQty));
        pList.Add(new clsParameter("ReturQty", ReturQty));
        pList.Add(new clsParameter("UnitPrice", UnitPrice));
        pList.Add(new clsParameter("Status", Status));
        if (StatusDate == DateTime.MinValue || StatusDate.Date == new DateTime(1, 1, 1)) pList.Add(new clsParameter("StatusDate", DBNull.Value));
        else pList.Add(new clsParameter("StatusDate", StatusDate.ToSQLDate()));
        pList.Add(new clsParameter("RejectReason", RejectReason));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockOrderItemUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "StockOrderItem");
    }
    public string Approved()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOrderItem Set ApprovedQty = '{ApprovedQty}', PendingQty = {ApprovedQty}, Status = '{Status}', StatusDate = GETDATE(), RejectReason = '' Where Id = '{Id}' ");
    }
    public string Rejected()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOrderItem Set Status = 'Rejected', StatusDate = GETDATE(), RejectReason = '{RejectReason}' where Id = '{Id}'");
    }
    public string UpdateReceivedQty()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOrderItem Set PendingQty = '{PendingQty}', ReceivedQty = '{ReceivedQty}' where Id = '{Id}' ");
    }
    public string UpdateRetur()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOrderItem Set PendingQty = '{PendingQty}', ReturQty = '{ReturQty}' where Id = '{Id}' ");
    }
    #endregion
    #endregion
}