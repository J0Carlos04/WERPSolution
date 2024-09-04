using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class StockMovement
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
    [Column(Field = "StockReceivedItemId")]
    public int StockReceivedItemId { get; set; }
    [Column(Field = "StockOutItemId")]
    public int StockOutItemId { get; set; }
    [Column(Field = "StockReceivedReturId")]
    public int StockReceivedReturId { get; set; }
    [Column(Field = "StockOutReturId")]
    public int StockOutReturId { get; set; }
    [Column(Field = "StockOutReturUsedId")]
    public int StockOutReturUsedId { get; set; }
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }
    [Column(Field = "RequesterUserId")]
    public int RequesterUserId { get; set; }
    #endregion

    #region Fields
    [Column(Field = "Code", SearchName = "b.Code", SortName = "b.Code")]
    public string Code { get; set; }
    [Column(Field = "Name", SearchName = "b.Name", SortName = "b.Name")]
    public string Name { get; set; }
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }
    [Column(Field = "MovementType", SearchName = "a.MovementType", SortName = "a.MovementType")]
    public string MovementType { get; set; }
    [Column(Field = "MovementDate", SearchName = "a.MovementDate", SortName = "a.MovementDate")]
    public DateTime MovementDate { get; set; }
    [Column(Field = "Requester", SearchName = "c.Name", SortName = "c.Name")]
    public string Requester { get; set; }
    #endregion

    #region Additional    
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static StockMovement GetById(object Id)
    {
        return (StockMovement)DataAccess.GetSingleRowByQuery(0, $"Select * from StockMovement where Id = '{Id}'", new StockMovement());
    }
    public static StockMovement GetByStockReceivedItemId(object StockReceivedItemId)
    {
        return (StockMovement)DataAccess.GetSingleRowByQuery(0, $"Select * from StockMovement where StockReceivedItemId = '{StockReceivedItemId}'", new StockMovement());
    }
    public static StockMovement GetByStockReceivedReturId(object StockReceivedReturId)
    {
        return (StockMovement)DataAccess.GetSingleRowByQuery(0, $"Select * from StockMovement where StockReceivedReturId = '{StockReceivedReturId}'", new StockMovement());
    }
    public static StockMovement GetByStockOutItemId(object StockOutItemId)
    {
        return (StockMovement)DataAccess.GetSingleRowByQuery(0, $"Select * from StockMovement where StockOutItemId = '{StockOutItemId}'", new StockMovement());
    }
    public static StockMovement GetByStockOutReturId(object StockOutReturId)
    {
        return (StockMovement)DataAccess.GetSingleRowByQuery(0, $"Select * from StockMovement where StockOutReturId = '{StockOutReturId}'", new StockMovement());
    }
    public static StockMovement GetByStockOutReturUsedId(object StockOutReturUsedId)
    {
        return (StockMovement)DataAccess.GetSingleRowByQuery(0, $"Select * from StockMovement where StockOutReturUsedId = '{StockOutReturUsedId}'", new StockMovement());
    }
    public static StockMovement GetByItemId(object ItemId)
    {
        return (StockMovement)DataAccess.GetSingleRowByQuery(0, $"Select * from StockMovement where ItemId = '{ItemId}'", new StockMovement());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from StockMovement", new StockMovement());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockMovementGetByCriteria", pList, out TotalRow, new StockMovement());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("StockReceivedReturId", StockReceivedReturId));
        pList.Add(new clsParameter("StockOutReturId", StockOutReturId));
        pList.Add(new clsParameter("StockOutReturUsedId", StockOutReturUsedId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("RequesterUserId", RequesterUserId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("MovementType", MovementType));
        pList.Add(new clsParameter("MovementDate", MovementDate.ToSQLDateTime()));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockMovementInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("StockReceivedReturId", StockReceivedReturId));
        pList.Add(new clsParameter("StockOutReturId", StockOutReturId));
        pList.Add(new clsParameter("StockOutReturUsedId", StockOutReturUsedId));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("RequesterUserId", RequesterUserId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("MovementType", MovementType));
        pList.Add(new clsParameter("MovementDate", MovementDate.ToSQLDateTime()));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockMovementUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "StockMovement");
    }
    #endregion

    #endregion
}