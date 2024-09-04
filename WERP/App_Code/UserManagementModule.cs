using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class UserManagementModule
{
    #region Properties

    #region Fields
    [Column(Field = "Name")]
    public string Name { get; set; }
    #endregion

    #region Keys
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "ModuleId")]
    public int ModuleId { get; set; }
    [Column(Field = "UserManagementId")]
    public int UserManagementId { get; set; }

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
    public static readonly Type MyType = typeof(UserManagementModule);
    #endregion

    #endregion

    #region Methods

    #region Get Data
    public static UserManagementModule GetById(object Id)
    {
        return (UserManagementModule)DataAccess.GetSingleRowByQuery(0, $"Select * from UserManagementModule where Id = '{Id}'", new UserManagementModule());
    }
    public static bool IsFieldExist(object FieldName, object FieldValue, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from UserManagementModule where {FieldName} = '{FieldValue}' AND Id <> '{Id}' ").ToBool();
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from UserManagementModule", new UserManagementModule());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new UserManagementModule());
    }
    #endregion

    #region Save Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("ModuleId", ModuleId));
        pList.Add(new clsParameter("UserManagementId", UserManagementId));

        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "UserManagementModuleInsert", pList);
    }    
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "UserManagementModule");
    }
    #endregion

    #endregion            
}