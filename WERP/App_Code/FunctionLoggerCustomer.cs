using DAL;
using System;
using System.Collections.Generic;
using U = Utility;

[Serializable]
public class FunctionLoggerCustomer
{
    #region Properties
    [Column(Field = "Id")]
    public int Id { get; set; }    
    [Column(Field = "StationId", Required = true)]
    public int StationId { get; set; }
    [Column(Field = "FunctionLoggerId", Required = true)]
    public int FunctionLoggerId { get; set; }
    [Column(Field = "CustomerId", Required = true)]
    public int CustomerId { get; set; }
    [Column(Field = "Station", SearchName = "b.Name", SortName = "b.Name")]
    public string Station { get; set; }
    [Column(Field = "FunctionLogger", SearchName = "c.Name", SortName = "c.Name")]
    public string FunctionLogger { get; set; }
    [Column(Field = "Customer", SearchName = "d.Name", SortName = "d.Name")]
    public string Customer { get; set; }

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
    #endregion
    #endregion

    #region Methods
    public static FunctionLoggerCustomer GetById(object Id)
    {
        return (FunctionLoggerCustomer)DataAccess.GetSingleRowByQuery(0, $"Select * from FunctionLoggerCustomer where Id = '{Id}'", new FunctionLoggerCustomer());
    }    
    public static FunctionLoggerCustomer GetByKey(object StationId, object FunctionLoggerId, object CustomerId, object Id)
    {
        return (FunctionLoggerCustomer)DataAccess.GetSingleRowByQuery(0, $"Select * from FunctionLoggerCustomer where StationId = '{StationId}' and FunctionLoggerId = '{FunctionLoggerId}' and CustomerId = '{CustomerId}' AND Id <> '{Id}'", new FunctionLoggerCustomer());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "FunctionLoggerCustomerGetByCriteria", pList, out TotalRow, new FunctionLoggerCustomer());
    }

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("StationId", StationId));
        pList.Add(new clsParameter("FunctionLoggerId", FunctionLoggerId));
        pList.Add(new clsParameter("CustomerId", CustomerId));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "FunctionLoggerCustomerInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("StationId", StationId));
        pList.Add(new clsParameter("FunctionLoggerId", FunctionLoggerId));
        pList.Add(new clsParameter("CustomerId", CustomerId));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "FunctionLoggerCustomerUpdate", pList);
    }
    #endregion
    #endregion
}