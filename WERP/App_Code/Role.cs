using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class Role
{
    #region Properties

    #region Fields
    [Column(Field = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
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
    public static readonly Type MyType = typeof(Role);
    #endregion

    #endregion

    #region Methods

    #region Get Data
    public static Role GetById(object Id)
    {
        return (Role)DataAccess.GetSingleRowByQuery(0, $"Select * from Role where Id = '{Id}'", new Role());
    }
    public static bool IsFieldExist(object FieldName, object FieldValue, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from Role where {FieldName} = '{FieldValue}' AND Id <> '{Id}' ").ToBool();
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Role", new Role());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new Role());
    }
    #endregion

    #region Save Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));

        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "RoleInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));

        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "RoleUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "Role");
    }
    #endregion

    #endregion            
}