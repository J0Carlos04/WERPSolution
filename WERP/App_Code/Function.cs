using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class Function
{
    #region Fields    
    [Column(Field = "Name", Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
    [Column(Field = "Level", Required = true, SearchName = "a.Level", SortName = "a.Level")]
    public int Level { get; set; }
    [Column(Field = "UseLocation", Required = true, SearchName = "a.UseLocation", SortName = "a.UseLocation")]
    public bool UseLocation { get; set; }
    [Column(Field = "UseCustomer", Required = true, SearchName = "a.UseCustomer", SortName = "a.UseCustomer")]
    public bool UseCustomer { get; set; }
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
    #endregion

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Function GetById(object Id)
    {
        return (Function)DataAccess.GetSingleRowByQuery(0, $"Select * from [Function] where Id = '{Id}'", new Function());
    }
    public static Function GetByKey(object Name, object Id)
    {
        return (Function)DataAccess.GetSingleRowByQuery(0, $"Select * from [Function] where Name = '{Name}' AND Id <> '{Id}'", new Function());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from [Function]", new Function());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from [Function] where Active = 1", new Function());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new Function());
    }
    public static List<object> GetByUseLocation(bool UseLocation = true)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from [Function] where UseLocation = '{UseLocation}'", new Function());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Level", Level));
        pList.Add(new clsParameter("UseLocation", UseLocation));
        pList.Add(new clsParameter("UseCustomer", UseCustomer));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "FunctionInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Level", Level));
        pList.Add(new clsParameter("UseLocation", UseLocation));
        pList.Add(new clsParameter("UseCustomer", UseCustomer));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "FunctionUpdate", pList);
    }
    #endregion

    #endregion
}