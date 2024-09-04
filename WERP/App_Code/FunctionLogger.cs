using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using FineUI;

[Serializable]
public class FunctionLogger
{
    #region Fields    
    [Column(Field = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }    
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    #endregion

    #region Properties
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
    [Column(Field = "StationId", Required = true)]
    public int StationId { get; set; }
    [Column(Field = "UnitId", Required = true)]
    public int UnitId { get; set; }
    #endregion

    #region Additional   
    [Column(Field = "Station", SearchName = "b.Name", SortName = "b.Name")]
    public string Station { get; set; }
    [Column(Field = "Unit", SearchName = "c.Name", SortName = "c.Name")]
    public string Unit { get; set; }
    #endregion
    #endregion

    #region Methods

    #region Get Data    
    public static FunctionLogger GetById(object Id)
    {
        return (FunctionLogger)DataAccess.GetSingleRowByQuery(0, $"Select * from FunctionLogger where Id = '{Id}'", new FunctionLogger());
    }
    public static FunctionLogger GetByKey(object Name, object Id)
    {
        return (FunctionLogger)DataAccess.GetSingleRowByQuery(0, $"Select * from FunctionLogger where Name = '{Name}' AND Id <> '{Id}'", new FunctionLogger());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from FunctionLogger", new FunctionLogger());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from FunctionLogger where Active = 1", new FunctionLogger());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "FunctionLoggerGetByCriteria", pList, out TotalRow, new FunctionLogger());
    }
    public static List<object> GetByStation(object StationId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from FunctionLogger where StationId = '{StationId}'", new FunctionLogger());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();        
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("StationId", StationId));
        pList.Add(new clsParameter("UnitId", UnitId));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "FunctionLoggerInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("StationId", StationId));
        pList.Add(new clsParameter("UnitId", UnitId));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "FunctionLoggerUpdate", pList);
    }
    #endregion

    #endregion
}