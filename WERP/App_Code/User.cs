using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using U = Utility;

public class Users
{
    #region Properties

    #region Fields    
    [Column(Field = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Department", SearchName = "b.Name", SortName = "b.Name")]
    public string Department { get; set; }
    [Column(Field = "Email", Required = true, SearchName = "a.Email", SortName = "a.Email")]
    public string Email { get; set; }
    [Column(Field = "Username", Required = true, SearchName = "a.Username", SortName = "a.Username")]
    public string Username { get; set; }
    [Column(Field = "Password")]
    public string Password { get; set; }
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
    public static readonly Type MyType = typeof(Users);
    #endregion

    #region Foreign Key
    [Column(Field = "DepartmentId")]
    public int DepartmentId { get; set; }
    #endregion    

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Users GetById(object Id)
    {
        return (Users)DataAccess.GetSingleRowByQuery(0, $"Select * from Users where Id = '{Id}'", new Users());
    }
    public static Users GetByUsername(object Username)
    {
        return (Users)DataAccess.GetSingleRowByQuery(0, $"Select * from Users where Username = '{Username}'", new Users());
    }
    public static Users GetByKey(object Usersname, object Id)
    {
        return (Users)DataAccess.GetSingleRowByQuery(0, $"Select * from Users where Usersname = '{Usersname}' AND Id <> '{Id}'", new Users());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Users", new Users());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Users where Active = 1 Order By Name", new Users());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "UsersGetByCriteria", pList, out TotalRow, new Users());
    }

    public static bool IsUsernameExist(object Username)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from Users where Username = '{Username}' ").ToBool();
    }

    public static List<object> GetByRole(object RoleName)
    {
        return DataAccess.GetDataByQuery(0, $"select distinct * from Users where Id IN (select UsersId from UserManagementUsers where UserManagementId IN (select Id from UserManagement where Id In (select UserManagementId from UserManagementRole where RoleId = (select top 1 Id From Role where Name = '{RoleName}')))) Order By Name", new Users());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("DepartmentId", DepartmentId));
        pList.Add(new clsParameter("Username", Username));
        pList.Add(new clsParameter("Password", Password));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Email", Email));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "UsersInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("DepartmentId", DepartmentId));
        pList.Add(new clsParameter("Username", Username));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Email", Email));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "UsersUpdate", pList);
    }
    public string ResetPassword()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Users Set Password = '{U.EncryptString(Username)}' where Id = {Id}");
    }
    public string ChangePassword()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Users Set Password  = '{Password}' where Id = {Id} ");
    }
    #endregion

    #endregion
}