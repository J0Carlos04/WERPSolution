using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class RequestType
{
    #region Properties

    #region Fields        
    [Column(Field = "Name", Title = "Name", Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
    [Column(Field = "ResponseTime", Title = "Response Time", Required = true, SearchName = "ResponseTime", SortName = "ResponseTime")]
    public int ResponseTime { get; set; }
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
    public static readonly Type MyType = typeof(RequestType);
    #endregion    

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static RequestType GetById(object Id)
    {
        return (RequestType)DataAccess.GetSingleRowByQuery(0, $"Select * from RequestType where Id = '{Id}'", new RequestType());
    }
    public static RequestType GetByKey(object Name, object Id)
    {
        return (RequestType)DataAccess.GetSingleRowByQuery(0, $"Select * from RequestType where Name = '{Name}' AND Id <> '{Id}'", new RequestType());
    }
    public static bool IsFieldExist(object FieldName, object FieldValue, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from RequestType where {FieldName} = '{FieldValue}' AND Id <> '{Id}' ").ToBool();
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from RequestType", new RequestType());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from RequestType where Active = 1", new RequestType());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new RequestType());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();        
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("ResponseTime", ResponseTime));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "RequestTypeInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));        
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("ResponseTime", ResponseTime));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "RequestTypeUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "RequestType");
    }
    #endregion

    #endregion
}