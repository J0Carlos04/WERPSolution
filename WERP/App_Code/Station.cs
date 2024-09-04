using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class Station
{
    #region Properties

    #region Fields    
    [Column(Field = "SiteNumber", Required = true, SearchName = "SiteNumber", SortName = "SiteNumber")]
    public int SiteNumber { get; set; }
    [Column(Field = "Name", Required = true, SearchName = "Name", SortName = "Name")]
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
    public static readonly Type MyType = typeof(Station);
    #endregion

    #region Additional 
    [Column(Field = "Level")]
    public int Level { get; set; }
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Station GetById(object Id)
    {
        return (Station)DataAccess.GetSingleRowByQuery(0, $"Select * from Station where Id = '{Id}'", new Station());
    }
    public static Station LowestLevelGetById(object Id)
    {
        return (Station)DataAccess.GetSingleRowByQuery(0, $"SELECT s.Id, s.Name, Max(f.Level) AS [Level] FROM Station s JOIN FunctionStation fs ON s.Id = fs.StationId JOIN [Function] f ON fs.FunctionId = f.Id where s.Id = '{Id}' GROUP BY s.Id, s.Name", new Station());
    }
    public static Station GetBySiteNumber(object SiteNumber)
    {
        return (Station)DataAccess.GetSingleRowByQuery(0, $"Select * from Station where SiteNumber = '{SiteNumber}'", new Station());
    }
    public static Station GetByKey(object Name, object Id)
    {
        return (Station)DataAccess.GetSingleRowByQuery(0, $"Select * from Station where Name = '{Name}' AND Id <> '{Id}'", new Station());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Station", new Station());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Station where Active = 1", new Station());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new Station());
    }
    public static List<object> GetLowerStation(object Level)
    {
        return DataAccess.GetDataByQuery(0, $"SELECT s.Id, s.Name, Max(f.Level) AS LowestLevel FROM Station s JOIN FunctionStation fs ON s.Id = fs.StationId JOIN [Function] f ON fs.FunctionId = f.Id GROUP BY s.Id, s.Name HAVING MIN(f.Level) > '{Level}' ", new Station());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("SiteNumber", SiteNumber));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StationInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("SiteNumber", SiteNumber));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StationUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "Station");
    }
    #endregion

    #endregion
}