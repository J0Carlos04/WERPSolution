using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class Rack
{
    #region Properties

    #region Fields            
    [Column(Field = "Warehouse", Title = "Warehouse", SearchName = "b.Name", SortName = "b.Name")]
    public string Warehouse { get; set; }
    [Column(Field = "Name", Title = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
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
    public static readonly Type MyType = typeof(Rack);
    #endregion

    #region Key
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "WarehouseId", Required = true)]
    public int WarehouseId { get; set; }
    #endregion    

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Rack GetById(object Id)
    {
        return (Rack)DataAccess.GetSingleRowByQuery(0, $"Select * from Rack where Id = '{Id}'", new Rack());
    }
    public static Rack GetByKey(object Name, object WarehouseId)
    {
        return (Rack)DataAccess.GetSingleRowByQuery(0, $"Select * from Rack where Name = '{Name}' AND WarehouseId = '{WarehouseId}'", new Rack());
    }
    public static Rack GetByKey(object Name, object WarehouseId, object Id)
    {
        return (Rack)DataAccess.GetSingleRowByQuery(0, $"Select * from Rack where Name = '{Name}' AND WarehouseId = '{WarehouseId}' And Id <> '{Id}'", new Rack());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Rack", new Rack());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Rack where Active = 1", new Rack());
    }
    public static List<object> GetALLByWarehouseId(object WarehouseId)
    {
        return DataAccess.GetDataByQuery(0, "Select * from Rack where WarehouseId = '{WarehouseId}'", new Rack());
    }
    public static List<object> GetALLActiveByWarehouseId(object WarehouseId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from Rack where Active = 1 and WarehouseId = '{WarehouseId}'", new Rack());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "RackGetByCriteria", pList, out TotalRow, new Rack());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("WarehouseId", WarehouseId));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "RackInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("WarehouseId", WarehouseId));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "RackUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "Rack");
    }
    #endregion

    #endregion
}