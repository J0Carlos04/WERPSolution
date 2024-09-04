using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using U = Utility;

public class Operators
{
    #region Properties

    #region Fields        
    [Column(Field = "Name", Title = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Address", Required = true, SearchName = "Address", SortName = "Address", Title = "Address")]
    public string Address { get; set; }
    [Column(Field = "Phone", Required = true, SearchName = "Phone", SortName = "Phone", Title = "Phone")]
    public string Phone { get; set; }
    [Column(Field = "Email", Required = true, SearchName = "Email", SortName = "Email", Title = "Email")]
    public string Email { get; set; }
    [Column(Field = "Vendor", SearchName = "b.Name", SortName = "b.Name", Title = "Vendor")]
    public string Vendor { get; set; }
    [Column(Field = "Username", Required = true, SearchName = "a.Username", SortName = "a.Username", Title = "Username")]
    public string Username { get; set; }
    [Column(Field = "Password")]
    public string Password { get; set; }
    [Column(Field = "Active", Title = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
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
    public static readonly Type MyType = typeof(Operators);
    #endregion

    #region Key
    [Column(Field = "VendorId", Required = true)]
    public int VendorId { get; set; }
    #endregion    

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Operators GetById(object Id)
    {
        return (Operators)DataAccess.GetSingleRowByQuery(0, $"Select * from Operators where Id = '{Id}'", new Operators());
    }
    public static Operators GetByUsername(object Username)
    {
        return (Operators)DataAccess.GetSingleRowByQuery(0, $"Select * from Operators where Username = '{Username}'", new Operators());
    }
    public static bool IsFieldExist(object FieldName, object FieldValue, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from Operators where {FieldName} = '{FieldValue}' AND Id <> '{Id}' ").ToBool();
    }
    public static bool IsUsernameExist(object Username)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from Users where Username = '{Username}' ").ToBool();
    }
    public static bool IsNameVendorExist(object Name, object VendorId, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from Operators where Name = '{Name}' and VendorId = '{VendorId}' AND Id <> '{Id}' ").ToBool();
    }
    public static List<object> GetByVendor(Object VendorId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from Operators where VendorId = '{VendorId}' and Active = 1 ", new Operators());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Operators", new Operators());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Operators where Active = 1", new Operators());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "OperatorsGetByCriteria", pList, out TotalRow, new Operators());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("VendorId", VendorId));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Address", Address));
        pList.Add(new clsParameter("Phone", Phone));
        pList.Add(new clsParameter("Email", Email));
        pList.Add(new clsParameter("Username", Username));
        pList.Add(new clsParameter("Password", Password));
        pList.Add(new clsParameter("Active", 1));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "OperatorsInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("VendorId", VendorId));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Address", Address));
        pList.Add(new clsParameter("Phone", Phone));
        pList.Add(new clsParameter("Email", Email));        
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "OperatorsUpdate", pList);
    }
    public string ResetPassword()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Operators Set Password = '{U.EncryptString(Username)}' where Id = {Id}");
    }
    public string ChangePassword()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Operators Set Password  = '{Password}' where Id = {Id} ");
    }
    #endregion

    #endregion
}