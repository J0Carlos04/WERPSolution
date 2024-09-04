using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class MasterData
{
    #region Properties

    #region Fields    
    [Column(Field = "Name", Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
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
    public static Type MyType { get; set; } = typeof(MasterData);
    #endregion
    
    #endregion

    #region Methods
    #region Get Data
    public static MasterData GetById(object Id, string Table)
    {
        return (MasterData)DataAccess.GetSingleRowByQuery(0, $"Select * from {Table} where Id = '{Id}'", new MasterData());
    }
    public static MasterData GetByKey(object Name, object Id, string Table)
    {
        return (MasterData)DataAccess.GetSingleRowByQuery(0, $"Select * from {Table} where Name = '{Name}' AND Id <> '{Id}'", new MasterData());
    }
    public static MasterData GetByName(object Name, string Table)
    {
        return (MasterData)DataAccess.GetSingleRowByQuery(0, $"Select * from {Table} where Name = '{Name}'", new MasterData());
    }
    public static List<object> GetALL(string Table)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from {Table}", new MasterData());
    }
    public static List<object> GetALLActive(string Table)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from {Table} where Active = 1", new MasterData());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new MasterData());
    }

    public static bool IsFieldExist(object TableName, object FieldName, object FieldValue, object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select 1 from {TableName} where {FieldName} = '{FieldValue}' AND Id <> '{Id}' ").ToBool();
    }
    #endregion
    #region Change Data
    public string Insert(string Table)
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Table", Table));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "InsertTable", pList);
    }
    public string Update(string Table)
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Table", Table));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "UpdateTable", pList);
    }
    #endregion
    #endregion
}