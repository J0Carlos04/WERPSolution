using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class WorkOrderType
{
    #region Properties

    #region Fields    
    [Column(Field = "Code", Title = "Code", Required = true, SearchName = "Code", SortName = "Code")]
    public string Code { get; set; }
    [Column(Field = "Name", Title = "Name", Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    #endregion

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
    public static readonly Type MyType = typeof(WorkOrderType);
    #endregion    

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static WorkOrderType GetById(object Id)
    {
        return (WorkOrderType)DataAccess.GetSingleRowByQuery(0, $"Select * from WorkOrderType where Id = '{Id}'", new WorkOrderType());
    }
    public static WorkOrderType GetByKey(object Name, object Id)
    {
        return (WorkOrderType)DataAccess.GetSingleRowByQuery(0, $"Select * from WorkOrderType where Name = '{Name}' AND Id <> '{Id}'", new WorkOrderType());
    }    
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from WorkOrderType", new WorkOrderType());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from WorkOrderType where Active = 1", new WorkOrderType());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new WorkOrderType());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "WorkOrderTypeInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "WorkOrderTypeUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "WorkOrderType");
    }
    #endregion

    #endregion
}