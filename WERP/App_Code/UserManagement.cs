using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using U = Utility;

public class UserManagement
{
    #region Properties

    #region Fields   
    [Column(Field = "Users", Title = "Users", SearchName = "a.Create", SortName = "a.Create")]
    public string Users { get; set; }
    [Column(Field = "Operators", Title = "Operators", SearchName = "a.Create", SortName = "a.Create")]
    public string Operators { get; set; }
    [Column(Field = "Roles", Title = "Roles", SearchName = "a.Create", SortName = "a.Create")]
    public string Roles { get; set; }
    [Column(Field = "Modules", Title = "Modules", SearchName = "a.Create", SortName = "a.Create")]
    public string Modules { get; set; }
    [Column(Field = "AllUsers", Required = true, SearchName = "a.AllUsers", SortName = "a.AllUsers")]
    public bool AllUsers { get; set; }
    [Column(Field = "AllOperators", Required = true, SearchName = "a.AllOperators", SortName = "a.AllOperators")]
    public bool AllOperators { get; set; }
    [Column(Field = "AllModules", Required = true, SearchName = "a.AllModules", SortName = "a.AllModules")]
    public bool AllModules { get; set; }
    [Column(Field = "Create", Title = "Create", Required = true, SearchName = "a.Create", SortName = "a.Create")]
    public bool Create { get; set; }
    [Column(Field = "Read", Title = "View", Required = true, SearchName = "a.Read", SortName = "a.Read")]
    public bool Read { get; set; }
    [Column(Field = "Update", Title = "Update", Required = true, SearchName = "a.Update", SortName = "a.Update")]
    public bool Update { get; set; }
    [Column(Field = "Delete", Title = "Delete", Required = true, SearchName = "a.Delete", SortName = "a.Delete")]
    public bool Delete { get; set; }
    [Column(Field = "ViewAllData", Title = "View All Data", Required = true, SearchName = "a.ViewAllData", SortName = "a.ViewAllData")]
    public bool ViewAllData { get; set; }
    [Column(Field = "Deviation", Title = "Deviation", Required = true, SearchName = "a.Deviation", SortName = "a.Deviation")]
    public bool Deviation { get; set; }

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
    public static readonly Type MyType = typeof(UserManagement);
    #endregion

    #region Additional    
    public List<object> UserList { get; set; } = new List<object>();
    public List<object> OperatorList { get; set; } = new List<object>();
    public List<object> RoleList { get; set; } = new List<object>();
    public List<object> ModuleList { get; set; } = new List<object>();    
    #endregion

    #endregion

    #region Methods

    #region Get Data
    public static UserManagement GetById(object Id)
    {
        return (UserManagement)DataAccess.GetSingleRowByQuery(0, $"Select * from UserManagement where Id = '{Id}'", new UserManagement());
    }    
    public static bool IsFieldExist(object FieldName, object FieldValue, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from UserManagement where {FieldName} = '{FieldValue}' AND Id <> '{Id}' ").ToBool();
    }  
    public static bool UsersIsMemberRole(object RoleName)
    {
        return DataAccess.GetSingleValueByQuery(0, $"select 1 from UserManagement where (AllUsers = 1 or Id In (Select UserManagementId from UserManagementUsers where UsersId = (Select top 1 Id from Users where Username = '{U.GetUsername()}'))) and Id In (select UserManagementId from UserManagementRole where RoleId = (select top 1 Id From Role where Name = '{RoleName}'))").ToBool();
    }
    public static bool OperatorsIsMemberRole(object RoleName)
    {
        return DataAccess.GetSingleValueByQuery(0, $"select 1 from UserManagement where (AllOperators = 1 or Id In (Select UserManagementId from UserManagementOperators where OperatorsId = (Select top 1 Id from Operators where Username = '{U.GetUsername()}'))) and Id In (select UserManagementId from UserManagementRole where RoleId = (select top 1 Id From Role where Name = '{RoleName}'))").ToBool();
    }
    public static List<object> UsersGetByPageName(object PageName)
    {
        return DataAccess.GetDataByQuery(0, $"select * from UserManagement where (AllUsers = 1 or Id In (Select UserManagementId from UserManagementUsers where UsersId = (Select top 1 Id from Users where Username = '{U.GetUsername()}'))) and (AllModules = 1 or Id In (select UserManagementId from UserManagementModule where ModuleId = (select top 1 Id From Module where Page = '{PageName}')))", new UserManagement());
    }
    public static List<object> OperatorsGetByPageName(object PageName)
    {
        return DataAccess.GetDataByQuery(0, $"select * from UserManagement where (AllOperators = 1 or Id In (Select UserManagementId from UserManagementOperators where OperatorsId = (Select top 1 Id from Operators where Username = '{U.GetUsername()}'))) and (AllModules = 1 or Id In (select UserManagementId from UserManagementModule where ModuleId = (select top 1 Id From Module where Page = '{PageName}')))", new UserManagement());
    }
    public static List<object> GetByRole(object RoleName)
    {
        return DataAccess.GetDataByQuery(0, $"select * from UserManagement where Id In (select UserManagementId from UserManagementRole where RoleId = (select top 1 Id From Role where Name = '{RoleName}'))", new UserManagement());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from UserManagement", new UserManagement());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "UserManagementGetByCriteria", pList, out TotalRow, new UserManagement());
    }
    #endregion

    #region Save Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("AllUsers", AllUsers));
        pList.Add(new clsParameter("AllOperators", AllOperators));
        pList.Add(new clsParameter("AllModules", AllModules));
        pList.Add(new clsParameter("Create", Create));
        pList.Add(new clsParameter("Read", Read));
        pList.Add(new clsParameter("Update", Update));
        pList.Add(new clsParameter("Delete", Delete));
        pList.Add(new clsParameter("ViewAllData", ViewAllData));
        pList.Add(new clsParameter("Deviation", Deviation));

        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "UserManagementInsert", pList);
    }
    public string Change()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("AllUsers", AllUsers));
        pList.Add(new clsParameter("AllOperators", AllOperators));
        pList.Add(new clsParameter("AllModules", AllModules));
        pList.Add(new clsParameter("Create", Create));
        pList.Add(new clsParameter("Read", Read));
        pList.Add(new clsParameter("Update", Update));
        pList.Add(new clsParameter("Delete", Delete));
        pList.Add(new clsParameter("ViewAllData", ViewAllData));
        pList.Add(new clsParameter("Deviation", Deviation));

        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "UserManagementUpdate", pList);
    }
    public static string Remove(object Id)
    {
        return DataAccess.ExecNonReturnValueBySP(0, "UserManagementDelete", new List<clsParameter> { new clsParameter("Id", Id) });
    }
    #endregion

    #endregion            
}