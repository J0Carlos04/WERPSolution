using System;
using System.Collections.Generic;
using U = Utility;
using DAL;

public class StationMapping
{
    #region Properties
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "ParentStationId", Required = true)]
    public int ParentStationId { get; set; }
    [Column(Field = "ChildStationId", Required = true)]
    public int ChildStationId { get; set; }
    [Column(Field = "ParentStation", SearchName = "b.Name", SortName = "b.Name")]
    public string ParentStation { get; set; }
    [Column(Field = "ChildStation", SearchName = "c.Name", SortName = "c.Name")]
    public string ChildStation { get; set; }

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
    public static StationMapping GetById(object Id)
    {
        return (StationMapping)DataAccess.GetSingleRowByQuery(0, $"Select * from StationMapping where Id = '{Id}'", new StationMapping());
    }    
    public static StationMapping GetByKey(object ParentStationId, object ChildStationId, object Id)
    {
        return (StationMapping)DataAccess.GetSingleRowByQuery(0, $"Select * from StationMapping where ParentStationId = '{ParentStationId}' and ChildStationId = '{ChildStationId}' AND Id <> '{Id}'", new StationMapping());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StationMappingGetByCriteria", pList, out TotalRow, new StationMapping());
    }

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("ParentStationId", ParentStationId));
        pList.Add(new clsParameter("ChildStationId", ChildStationId));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "StationMappingInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("ParentStationId", ParentStationId));
        pList.Add(new clsParameter("ChildStationId", ChildStationId));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "StationMappingUpdate", pList);
    }
    #endregion
    #endregion
}