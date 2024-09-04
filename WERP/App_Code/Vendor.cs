using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class Vendor
{
    #region Properties
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
    public static readonly Type MyType = typeof(Vendor);
    #endregion

    #region Fields    
    [Column(Field = "Code", Required = true, SearchName = "Code", SortName = "Code")]
    public string Code { get; set; }
    [Column(Field = "Name", Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
    [Column(Field = "Address", Required = true, SearchName = "Address", SortName = "Address")]
    public string Address { get; set; }
    [Column(Field = "Phone", Required = true, SearchName = "Phone", SortName = "Phone")]
    public string Phone { get; set; }
    [Column(Field = "Email", Required = true, SearchName = "Email", SortName = "Email")]
    public string Email { get; set; }
    [Column(Field = "ContactPerson", Required = true, SearchName = "ContactPerson", SortName = "ContactPerson")]
    public string ContactPerson { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    #endregion

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Vendor GetById(object Id)
    {
        return (Vendor)DataAccess.GetSingleRowByQuery(0, $"Select * from Vendor where Id = '{Id}'", new Vendor());
    }
    public static Vendor GetByKey(object Name, object Id)
    {
        return (Vendor)DataAccess.GetSingleRowByQuery(0, $"Select * from Vendor where Name = '{Name}' AND Id <> '{Id}'", new Vendor());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Vendor", new Vendor());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Vendor where Active = 1", new Vendor());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new Vendor());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Address", Address));
        pList.Add(new clsParameter("Phone", Phone));
        pList.Add(new clsParameter("Email", Email));
        pList.Add(new clsParameter("ContactPerson", ContactPerson));
        pList.Add(new clsParameter("Active", 1));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "VendorInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Address", Address));
        pList.Add(new clsParameter("Phone", Phone));
        pList.Add(new clsParameter("Email", Email));
        pList.Add(new clsParameter("ContactPerson", ContactPerson));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "VendorUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "Vendor");
    }
    #endregion

    #endregion
}