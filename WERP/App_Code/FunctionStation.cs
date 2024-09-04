using System;
using System.Collections.Generic;
using U = Utility;
using DAL;

[Serializable]
public class FunctionStation
{
    #region Properties
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "FunctionId", Required = true )]
    public int FunctionId { get; set; }
    [Column(Field = "StationId", Required = true)]
    public int StationId { get; set; }
    [Column(Field = "Function", SearchName = "b.Name", SortName = "b.Name")]
    public string Function { get; set; }
    [Column(Field = "Station", SearchName = "c.Name", SortName = "c.Name")]
    public string Station { get; set; }

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
    public static FunctionStation GetById(object Id)
    {
        return (FunctionStation)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Name [Function], c.Name [Station] from FunctionStation a Left Join [Function] b on b.Id = a.FunctionId Left Join Station c on c.Id = a.StationId where a.Id = '{Id}'", new FunctionStation());
    }
    public static FunctionStation GetByKey(object FunctionId, object StationId)
    {
        return (FunctionStation)DataAccess.GetSingleRowByQuery(0, $"Select * from FunctionStation where FunctionID = '{FunctionId}' and StationId = '{StationId}' ", new FunctionStation());
    }
    public static FunctionStation GetByKey(object FunctionId, object StationId, object Id)
    {
        return (FunctionStation)DataAccess.GetSingleRowByQuery(0, $"Select * from FunctionStation where FunctionID = '{FunctionId}' and StationId = '{StationId}' AND Id <> '{Id}'", new FunctionStation());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "FunctionStationGetByCriteria", pList, out TotalRow, new FunctionStation());
    } 
    public static List<object> GetByFunctionName(object FunctionName)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Name Station, c.Name [Function] from [FunctionStation] a Left Join Station b on b.Id = a.StationId Left Join [Function] c on c.Id = a.FunctionId where a.FunctionId in (Select Id from [Function] where Name = '{FunctionName}')", new FunctionStation());
    }
    public static List<object> GetByFunctionId(object FunctionId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Name Station, c.Name [Function] from [FunctionStation] a Left Join Station b on b.Id = a.StationId Left Join [Function] c on c.Id = a.FunctionId where a.FunctionId = '{FunctionId}'", new FunctionStation());
    }

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("FunctionId", FunctionId));
        pList.Add(new clsParameter("StationId", StationId));        
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "FunctionStationInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("FunctionId", FunctionId));
        pList.Add(new clsParameter("StationId", StationId));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "FunctionStationUpdate", pList);
    }
    #endregion
    #endregion
}