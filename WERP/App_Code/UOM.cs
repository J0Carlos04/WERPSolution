using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class UOM
{
    #region Properties

    #region Fields    
    [Column(Field = "Code", Required = true, SearchName = "Code", SortName = "Code")]
    public string Code { get; set; }
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
    public static readonly Type MyType = typeof(UOM);
    #endregion    

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static UOM GetById(object Id)
    {
        return (UOM)DataAccess.GetSingleRowByQuery(0, $"Select * from UOM where Id = '{Id}'", new UOM());
    }
    public static UOM GetByCode(object Code)
    {
        return (UOM)DataAccess.GetSingleRowByQuery(0, $"Select * from UOM where Code = '{Code}'", new UOM());
    }
    public static UOM GetByKey(object Name, object Id)
    {
        return (UOM)DataAccess.GetSingleRowByQuery(0, $"Select * from UOM where Name = '{Name}' AND Id <> '{Id}'", new UOM());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from UOM", new UOM());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from UOM where Active = 1", new UOM());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new UOM());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "UOMInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "UOMUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "UOM");
    }
    #endregion

    #endregion
}