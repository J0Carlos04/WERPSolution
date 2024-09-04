using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class Location
{
    #region Properties

    #region Fields    
    [Column(Field = "Name", Title = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Address", Title = "Address", Required = true, SearchName = "a.Address", SortName = "a.Address")]
    public string Address { get; set; }
    [Column(Field = "Type", Title = "Type", Required = true, SearchName = "a.Type", SortName = "a.Type")]
    public string Type { get; set; }
    [Column(Field = "RT", Title = "RT", Required = true, SearchName = "a.RT", SortName = "a.RT")]
    public string RT { get; set; }
    [Column(Field = "RW", Title = "RW", Required = true, SearchName = "a.RW", SortName = "a.RW")]
    public string RW { get; set; }
    [Column(Field = "UrbanVilage", Title = "UrbanVilage", Required = true, SearchName = "a.UrbanVilage", SortName = "a.UrbanVilage")]
    public string UrbanVilage { get; set; }
    [Column(Field = "SubDistrict", Title = "SubDistrict", Required = true, SearchName = "a.SubDistrict", SortName = "a.SubDistrict")]
    public string SubDistrict { get; set; }
    [Column(Field = "Latitude", Title = "Latitude", Required = true, SearchName = "a.Latitude", SortName = "a.Latitude")]
    public decimal Latitude { get; set; }
    [Column(Field = "Longitude", Title = "Longitude", Required = true, SearchName = "a.Longitude", SortName = "a.Longitude")]
    public decimal Longitude { get; set; }
    [Column(Field = "Area", Title = "Area", SearchName = "b.Name", SortName = "b.Name")]
    public string Area { get; set; }
    [Column(Field = "Active", Title = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
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
    public static readonly Type MyType = typeof(Location);
    #endregion

    #region Key
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "AreaId", Required = true)]
    public int AreaId { get; set; }
    [Column(Field = "StationId")]
    public int StationId { get; set; }
    [Column(Field = "FunctionStationId")]
    public int FunctionStationId { get; set; }
    #endregion    

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Location GetById(object Id)
    {
        return (Location)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Name Area from Location a Left Join Area b On b.Id = a.AreaId where a.Id = '{Id}'", new Location());
    }
    public static Location GetByKey(object Name, object Id)
    {
        return (Location)DataAccess.GetSingleRowByQuery(0, $"Select * from Location where Name = '{Name}' AND Id <> '{Id}'", new Location());
    }
    public static List<object> GetByAreaId(object AreaId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Name Area from Location a Left Join Area b On b.Id = a.AreaId where a.AreaId = '{AreaId}'", new Location());
    }    
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select a.*, b.Name Area from Location a Left Join Area b On b.Id = a.AreaId", new Location());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select a.*, b.Name Area from Location a Left Join Area b On b.Id = a.AreaId where a.Active = 1", new Location());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "LocationGetByCriteria", pList, out TotalRow, new Location());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("AreaId", AreaId));
        pList.Add(new clsParameter("StationId", StationId));
        pList.Add(new clsParameter("FunctionStationId", FunctionStationId));
        pList.Add(new clsParameter("Type", Type));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Address", Address));
        pList.Add(new clsParameter("RT", RT));
        pList.Add(new clsParameter("RW", RW));
        pList.Add(new clsParameter("UrbanVilage", UrbanVilage));
        pList.Add(new clsParameter("SubDistrict", SubDistrict));
        pList.Add(new clsParameter("Longitude", Longitude));
        pList.Add(new clsParameter("Latitude", Latitude));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "LocationInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("AreaId", AreaId));
        pList.Add(new clsParameter("StationId", StationId));
        pList.Add(new clsParameter("FunctionStationId", FunctionStationId));
        pList.Add(new clsParameter("Type", Type));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Address", Address));
        pList.Add(new clsParameter("RT", RT));
        pList.Add(new clsParameter("RW", RW));
        pList.Add(new clsParameter("UrbanVilage", UrbanVilage));
        pList.Add(new clsParameter("SubDistrict", SubDistrict));
        pList.Add(new clsParameter("Longitude", Longitude));
        pList.Add(new clsParameter("Latitude", Latitude));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "LocationUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "Location");
    }
    #endregion

    #endregion
}