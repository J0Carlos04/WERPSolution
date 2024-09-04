using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.IdentityModel.Protocols.WSTrust;

public class StockOut
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
    [Column(Field = "HelpdeskId")]
    public int HelpdeskId { get; set; }
    [Column(Field = "WorkOrderId")]
    public int WorkOrderId { get; set; }
    [Column(Field = "TakesOutUserId", SearchName = "a.TakesOutUserId", SortName = "a.TakesOutUserId")]
    public int TakesOutUserId { get; set; }
    #endregion

    #region Fields
    [Column(Field = "Code", SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "WONo", SearchName = "a.WONo", SortName = "a.WONo")]
    public string WONo { get; set; }
    [Column(Field = "Description", SearchName = "a.Description", SortName = "a.Description")]
    public string Description { get; set; }
    [Column(Field = "OutDate", SearchName = "c.Name", SortName = "a.Name")]
    public DateTime OutDate { get; set; }

    #endregion

    #region Additional
    
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static StockOut GetById(object Id)
    {
        return (StockOut)DataAccess.GetSingleRowByQuery(0, $"Select * from StockOut where Id = '{Id}'", new StockOut());
    }
    public static StockOut GetByWorkOrderId(object WOId)
    {
        return (StockOut)DataAccess.GetSingleRowByQuery(0, $"Select * from StockOut where WorkOrderId = '{WOId}'", new StockOut());
    }
    public static StockOut GetByHelpdeskId(object HelpdeskId)
    {
        return (StockOut)DataAccess.GetSingleRowByQuery(0, $"Select * from StockOut where HelpdeskId = '{HelpdeskId}'", new StockOut());
    }    
    #endregion

    #region Changed Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("HelpdeskId", HelpdeskId));
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("TakesOutUserId", TakesOutUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Description", Description));
        pList.Add(new clsParameter("OutDate", OutDate));
        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockOutInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("HelpdeskId", HelpdeskId));
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("TakesOutUserId", TakesOutUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Description", Description));
        pList.Add(new clsParameter("OutDate", OutDate));

        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockOutUpdate", pList);
    }    
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "StockOut");
    }
    #endregion

    #endregion
}