using DAL;
using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class Subject
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
    public static readonly Type MyType = typeof(Subject);
    #endregion

    #region Fields
    [Column(Field = "Code", Required = true, SearchName = "Code", SortName = "Code")]
    public string Code { get; set; }
    [Column(Field = "Name", Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
    [Column(Field = "NeedCustNo", Title = "Need Cust No", Required = true, SearchName = "NeedCustNo", SortName = "NeedCustNo")]
    public bool NeedCustNo { get; set; }
    [Column(Field = "UseItem", Title = "Use Item", Required = true, SearchName = "UseItem", SortName = "UseItem")]
    public string UseItem { get; set; }
    [Column(Field = "WorkDuration", Title = "Work Duration", SearchName = "WorkDuration", SortName = "WorkDuration")]
    public int WorkDuration { get; set; }
    [Column(Field = "WorkDurationType", Title = "Work Duration Type", SearchName = "WorkDurationType", SortName = "WorkDurationType")]
    public string WorkDurationType { get; set; }
    [Column(Field = "MeterCondition", Title = "Meter Condition", Required = true, SearchName = "MeterCondition", SortName = "MeterCondition")]
    public bool MeterCondition { get; set; }
    [Column(Field = "UseNRW", Title = "Use NRW", Required = true, SearchName = "UseNRW", SortName = "UseNRW")]
    public bool UseNRW { get; set; }

    [Column(Field = "UseDPD", Title = "Use DPD", Required = true, SearchName = "UseDPD", SortName = "UseDPD")]
    public bool UseDPD { get; set; }
    [Column(Field = "Leak", Required = true, SearchName = "Leak", SortName = "Leak")]
    public bool Leak { get; set; }
    [Column(Field = "NeedRestoration", Title = "Need Restoration", Required = true, SearchName = "NeedRestoration", SortName = "NeedRestoration")]
    public bool NeedRestoration { get; set; }
    [Column(Field = "NeedClosed", Title = "Need Closed", Required = true, SearchName = "NeedClosed", SortName = "NeedClosed")]
    public bool NeedClosed { get; set; }
    [Column(Field = "UseTimeLimit", Title = "Use Time Limit", Required = true, SearchName = "UseTimeLimit", SortName = "UseTimeLimit")]
    public bool UseTimeLimit { get; set; }    
    [Column(Field = "UsePhoto", Title = "Use Photo", Required = true, SearchName = "UsePhoto", SortName = "UsePhoto")]
    public bool UsePhoto { get; set; }
    [Column(Field = "UseSection", Title = "Use Section", Required = true, SearchName = "UseSection", SortName = "UseSection")]
    public bool UseSection { get; set; }
    [Column(Field = "UpdateGIS", Title = "Update GIS", Required = true, SearchName = "UpdateGIS", SortName = "UpdateGIS")]
    public bool UpdateGIS { get; set; }

    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    #endregion

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Subject GetById(object Id)
    {
        return (Subject)DataAccess.GetSingleRowByQuery(0, $"Select * from Subject where Id = '{Id}'", new Subject());
    }
    public static Subject GetByKey(object Code, object Name, object Id)
    {
        return (Subject)DataAccess.GetSingleRowByQuery(0, $"Select * from Subject where (Code = '{Code}' or Name = '{Name}') AND Id <> '{Id}'", new Subject());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Subject", new Subject());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Subject where Active = 1", new Subject());
    }
    public static List<object> GetNotUsedCust()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Subject where NeedCustNo = 0 and Active = 1", new Subject());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new Subject());
    }
    #endregion

    #region Change Data
    public string Insert()
    {        
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("NeedCustNo", NeedCustNo));
        pList.Add(new clsParameter("UseItem", UseItem));
        pList.Add(new clsParameter("WorkDuration", WorkDuration));
        pList.Add(new clsParameter("WorkDurationType", WorkDurationType));
        pList.Add(new clsParameter("MeterCondition", MeterCondition));
        pList.Add(new clsParameter("UseNRW", UseNRW));
        pList.Add(new clsParameter("UseDPD", UseDPD));
        pList.Add(new clsParameter("Leak", Leak));
        pList.Add(new clsParameter("NeedRestoration", NeedRestoration));
        pList.Add(new clsParameter("NeedClosed", NeedClosed));
        pList.Add(new clsParameter("UseTimeLimit", UseTimeLimit));        
        pList.Add(new clsParameter("UsePhoto", UsePhoto));
        pList.Add(new clsParameter("UseSection", UseSection));
        pList.Add(new clsParameter("UpdateGIS", UpdateGIS));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "SubjectInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("NeedCustNo", NeedCustNo));
        pList.Add(new clsParameter("UseItem", UseItem));
        pList.Add(new clsParameter("WorkDuration", WorkDuration));
        pList.Add(new clsParameter("WorkDurationType", WorkDurationType));
        pList.Add(new clsParameter("MeterCondition", MeterCondition));
        pList.Add(new clsParameter("UseNRW", UseNRW));
        pList.Add(new clsParameter("UseDPD", UseDPD));
        pList.Add(new clsParameter("Leak", Leak));
        pList.Add(new clsParameter("NeedRestoration", NeedRestoration));
        pList.Add(new clsParameter("NeedClosed", NeedClosed));
        pList.Add(new clsParameter("UseTimeLimit", UseTimeLimit));        
        pList.Add(new clsParameter("UsePhoto", UsePhoto));
        pList.Add(new clsParameter("UseSection", UseSection));
        pList.Add(new clsParameter("UpdateGIS", UpdateGIS));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "SubjectUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "Subject");
    }
    #endregion

    #endregion
}