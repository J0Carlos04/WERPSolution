using DAL;
using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
[Table("Customer")]
public class Customer : BaseModel
{
    #region Properties
    #region Fields    
    [Column(Field = "CustNo", Native = true, Title = "Customer No", Required = true, SearchName = "CustNo", SortName = "CustNo")]
    public string CustNo { get; set; }
    [Column(Field = "Name", Native = true, Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
    [Column(Field = "Address", Required = true, SearchName = "Address", SortName = "Address")]
    public string Address { get; set; }
    [Column(Field = "Phone", Native = true, Required = true, SearchName = "Phone", SortName = "Phone")]
    public string Phone { get; set; }
    [Column(Field = "Email", Native = true, Required = true, SearchName = "Email", SortName = "Email")]
    public string Email { get; set; }
    [Column(Field = "Rate", SearchName = "c.Code", SortName = "c.Code")]
    public string Rate { get; set; }
    [Column(Field = "RateName", Title = "Rate Name", SearchName = "c.Name", SortName = "c.Name")]
    public string RateName { get; set; }
    [Column(Field = "MeterSealNo", Native = true, SearchName = "MeterSealNo", SortName = "MeterSealNo")]
    public string MeterSealNo { get; set; }
    [Column(Field = "MeterInstallationDate", Native = true, Title = "Meter Installation Date", SearchName = "MeterInstallationDate", SortName = "MeterInstallationDate")]
    public DateTime MeterInstallationDate { get; set; }
    [Column(Field = "KodeUKR", Native = true, Title = "Size Meter Code", SearchName = "KodeUKR", SortName = "KodeUKR")]
    public string KodeUKR { get; set; }
    [Column(Field = "UKMeter", Native = true, Title = "Size Meter", SearchName = "UKMeter", SortName = "a.UKMeter")]
    public string UKMeter { get; set; }
    [Column(Field = "Status", Native = true, SearchName = "a.Status", SortName = "a.Status")]
    public string Status { get; set; }
    [Column(Field = "Active", Native = true, Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    #endregion

    #region Key
    [Column(Field = "LocationId", Native = true, Required = true)]
    public int LocationId { get; set; }
    [Column(Field = "RateId", Native = true)]
    public int RateId { get; set; }
    #endregion    

    #region Additional    
    [Column(Field = "Used")]
    public bool Used { get; set; }
    [Column(Field = "Location", SearchName = "b.Name", SortName = "b.Name")]
    public string Location { get; set; }
    [Column(Field = "RT", SearchName = "RT", SortName = "RT")]
    public string RT { get; set; }
    [Column(Field = "RW", SearchName = "RW", SortName = "RW")]
    public string RW { get; set; }
    [Column(Field = "Kelurahan", SearchName = "UrbanVilage", SortName = "UrbanVilage")]
    public string Kelurahan { get; set; }
    [Column(Field = "Kecamatan", SearchName = "SubDistrict", SortName = "SubDistrict")]
    public string Kecamatan { get; set; }
    [Column(Field = "Latitude", SearchName = "Latitude", SortName = "Latitude")]
    public string Latitude { get; set; }
    [Column(Field = "Longitude", SearchName = "Longitude", SortName = "Longitude")]
    public string Longitude { get; set; }

    public string OperatorType { get; set; }
    public int VendorId { get; set; }    
    public int OperatorsId { get; set; }    
    public int OperatorUserId { get; set; }
    public static readonly Type MyType = typeof(Customer);
    #endregion
    #endregion

    #region Methods

    #region Get Data    
    public static Customer GetByKey(object Name, object Id)
    {
        return (Customer)DataAccess.GetSingleRowByQuery(0, $"Select * from Customer where Name = '{Name}' AND Id <> '{Id}'", new Customer());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Customer", new Customer());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Customer where Active = 1", new Customer());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "CustomerGetByCriteria", pList, out TotalRow, new Customer());
    }
    public static List<object> CustomerLookupGetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "CustomerLookupGetByCriteria", pList, out TotalRow, new Customer());
    }
    public static List<object> GetByHelpdeskId(Object HelpdeskId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.Id, a.LocationId, a.CustNo, a.Name, b.Name Address, a.Phone, a.Email, 1 Used from Customer a Left Join Location b On b.Id = a.LocationId where a.Id in (Select CustomerId from HelpdeskCustomer where HelpdeskId = '{HelpdeskId}')", new Customer());
    }
    #endregion

    #region Change Data
       
    #endregion

    #endregion
}