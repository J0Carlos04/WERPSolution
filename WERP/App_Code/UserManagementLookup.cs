using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class UserManagementLookup
{
    #region Properties

    #region Fields
    [Column(Field = "Name", SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
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
    public static readonly Type MyType = typeof(UserManagementLookup);
    #endregion

    #endregion

    #region Methods

    #region Get Data
    public static UserManagementLookup GetById(object Id, string TableName)
    {
        return (UserManagementLookup)DataAccess.GetSingleRowByQuery(0, $"Select * from {TableName} where Id = '{Id}' ", new UserManagementLookup());
    }
    public static List<object> GetByParent(object Id, string TableName)
    {
        return DataAccess.GetDataByQuery(0, $"Select b.* from UserManagement{TableName} a Inner Join {TableName} b On a.{TableName}Id = b.Id where a.UserManagementId = '{Id}' ", new UserManagementLookup());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new UserManagementLookup());
    }
    #endregion

    #region Save Data
    public static string DeleteByParent(string TableName, object UserManagementId)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Delete {TableName} where UserManagementId = '{UserManagementId}' ");
    }
    #endregion

    #endregion
}