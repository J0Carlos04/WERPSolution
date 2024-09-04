using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.Drawing;

public class WorkOrderWorkUpdate : BaseModel
{
    #region Properties
    //#region Default
    //[Column(Field = "Id")]
    //public int Id { get; set; }
    //[Column(Field = "CreatedBy")]
    //public string CreatedBy { get; set; }
    //[Column(Field = "Created")]
    //public DateTime Created { get; set; }
    //[Column(Field = "ModifiedBy")]
    //public string ModifiedBy { get; set; }
    //[Column(Field = "Modified")]
    //public DateTime Modified { get; set; }
    //[Column(Field = "No")]
    //public double No { get; set; }
    //public bool IsChecked { get; set; }
    //public string Mode { get; set; }
    //#endregion

    #region Key
    [Column(Field = "WorkOrderId")]
    public int WorkOrderId { get; set; }
    [Column(Field = "WorkOrderReferenceId")]
    public int WorkOrderReferenceId { get; set; }
    [Column(Field = "StockOutReturId")]
    public int StockOutReturId { get; set; }
    #endregion

    #region Fields
    [Column(Field = "WorkType", SearchName = "WorkType", SortName = "WorkType")]
    public string WorkType { get; set; }
    [Column(Field = "Date", SearchName = "Date", SortName = "Date")]
    public DateTime Date { get; set; }
    [Column(Field = "WorkDetail", Title = "WorkDetail", SearchName = "WorkDetail", SortName = "WorkDetail")]
    public string WorkDetail { get; set; }

    #endregion
    #region Additional Fields
    [Column(Field = "WoCode", SearchName = "WoCode", SortName = "WoCode")]
    public string WoCode { get; set; }
    public List<object> Items { get; set; }
    public List<object> Attachs { get; set; }
    public int StockOutId { get; set; }    
    #endregion
    #endregion

    #region Methods

    #region Get Data    
    public static List<object> GetByWorkOrderId(object WorkOrderId)
    {
        return DataAccess.GetDataByQuery(0, $"select a.*, b.StockOutId, c.StockOutId StockOutReturId, c.Code WoCode from WorkOrderWorkUpdate a Left Join WorkOrder b On b.Id = a.WorkOrderId Left Join WorkOrder c On c.Id = a.WorkOrderReferenceId where a.WorkOrderId = '{WorkOrderId}' ", new WorkOrderWorkUpdate());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("WorkOrderReferenceId", WorkOrderReferenceId));
        pList.Add(new clsParameter("Date", Date));
        pList.Add(new clsParameter("WorkType", WorkType));
        pList.Add(new clsParameter("WorkDetail", WorkDetail));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "WorkOrderWorkUpdateInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("WorkOrderReferenceId", WorkOrderReferenceId));
        pList.Add(new clsParameter("Date", Date));
        pList.Add(new clsParameter("WorkType", WorkType));
        pList.Add(new clsParameter("WorkDetail", WorkDetail));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "WorkOrderWorkUpdateUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.ExecNonReturnValueBySP(0, "WorkOrderWorkUpdateDelete", new List<clsParameter> { new clsParameter { Name = "Id", Value = Id } });
    }
    #endregion

    #endregion
}