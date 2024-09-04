using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using DAL;

public class Module
{
    #region Properties

    #region Fields
    [Column(Field = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Page", Required = true, SearchName = "a.Page", SortName = "a.Page")]
    public string Page { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }

    #endregion

    #region Keys
    [Column(Field = "Id")]
    public int Id { get; set; }

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
    public static readonly Type MyType = typeof(Module);
    #endregion

    #endregion

    #region Methods

    #region Get Data    
    public static Module GetById(object Id)
    {
        return (Module)DataAccess.GetSingleRowByQuery(0, $"Select * from Module where Id = '{Id}'", new Module());
    }
    public static bool IsFieldExist(object FieldName, object FieldValue, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from Module where {FieldName} = '{FieldValue}' AND Id <> '{Id}' ").ToBool();
    }

    public static Module GetByKey(object Name, object Page, object Id)
    {
        return (Module)DataAccess.GetSingleRowByQuery(0, $"Select * from Module where (Name = '{Name}' or Page = '{Page}') AND Id <> '{Id}'", new Module());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Module", new Module());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new Module());
    }
    #endregion

    #region Save Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Page", Page));
        pList.Add(new clsParameter("Active", 1));

        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ModuleInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Page", Page));
        pList.Add(new clsParameter("Active", Active));

        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "ModuleUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.ExecNonReturnValueBySP(0, "ModuleDelete", new List<clsParameter> { new clsParameter("Id", Id) });
    }
    #endregion

    #endregion            
}