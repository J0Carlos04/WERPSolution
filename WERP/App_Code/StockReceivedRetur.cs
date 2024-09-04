using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class StockReceivedRetur
{
    #region Properties

    #region Fields
    [Column(Field = "Code", SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "Reason", SearchName = "a.Reason", SortName = "a.Reason")]
    public string Reason { get; set; }
    [Column(Field = "ReturDate", SearchName = "a.ReturDate", SortName = "a.ReturDate")]
    public DateTime ReturDate { get; set; }
    [Column(Field = "ItemCode", Title = "Item Code", SearchName = "d.Code", SortName = "d.Code")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName", Title = "Item Name", SearchName = "d.Name", SortName = "d.Name")]
    public string ItemName { get; set; }
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }
    [Column(Field = "Requester", SearchName = "b.Name", SortName = "b.Name")]
    public string Requester { get; set; }
    #endregion

    #region Key
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "StockReceivedItemId")]
    public int StockReceivedItemId { get; set; }
    [Column(Field = "RequesterUserId")]
    public int RequesterUserId { get; set; }
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
    public static StockReceivedRetur GetById(object Id)
    {
        return (StockReceivedRetur)DataAccess.GetSingleRowByQuery(0, $"Select a.*, c.Code ItemCode, c.Name ItemName from StockReceivedRetur a Left Join StockReceivedItem b On b.Id = a.StockReceivedItemId Left Join Item c On c.Id = b.ItemId where a.Id = '{Id}'", new StockReceivedRetur());
    }
    public static StockReceivedRetur GetByStockReceivedItemId(object StockReceivedItemId)
    {
        return (StockReceivedRetur)DataAccess.GetSingleRowByQuery(0, $"Select a.*, c.Code ItemCode, c.Name ItemName from StockReceivedRetur a Left Join StockReceivedItem b On b.Id = a.StockReceivedItemId Left Join Item c On c.Id = b.ItemId where b.Id = '{StockReceivedItemId}'", new StockReceivedRetur());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from StockReceivedRetur", new StockReceivedRetur());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockReceivedReturGetByCriteria", pList, out TotalRow, new StockReceivedRetur());
    }
    #endregion
    #region ChangeData
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("RequesterUserId", RequesterUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Reason", Reason));
        pList.Add(new clsParameter("ReturDate", ReturDate));
        pList.Add(new clsParameter("Qty", Qty));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockReceivedReturInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("RequesterUserId", RequesterUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Reason", Reason));
        pList.Add(new clsParameter("ReturDate", ReturDate));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockReceivedReturUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "StockReceivedRetur");
    }
    #endregion
    #endregion
}