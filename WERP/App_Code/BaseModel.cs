using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

/// <summary>
/// Summary description for BaseModel
/// </summary>
public class BaseModel
{
    #region Properties
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "CreatedBy", Native = true)]
    public string CreatedBy { get; set; }
    [Column(Field = "Created")]
    public DateTime Created { get; set; }
    [Column(Field = "ModifiedBy", Native = true)]
    public string ModifiedBy { get; set; }
    [Column(Field = "Modified")]
    public DateTime Modified { get; set; }
    [Column(Field = "No")]
    public double No { get; set; }
    public bool IsChecked { get; set; }
    public string Mode { get; set; }
    #endregion

    #region Methods

    public static string GetTableName(Type type) => ((TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute)))?.Name ?? type.Name;

    #region Get Data
    public static T GetById<T>(object Id) where T : BaseModel, new()
    {
        return (T)DataAccess.GetSingleRowByQuery(0, $"SELECT * FROM {GetTableName(typeof(T))} WHERE Id = '{Id}'", new T());
    }
    public static List<T> GetAll<T>() where T : BaseModel, new()
    {
        string query = $"SELECT * FROM {GetTableName(typeof(T))}";
        return DataAccess.GetDataByQuery(0, query, new T()).Cast<T>().ToList();
    }
    public static List<T> GetAllActive<T>() where T : BaseModel, new()
    {        
        string query = $"SELECT * FROM {GetTableName(typeof(T))} WHERE Active = 1";
        return DataAccess.GetDataByQuery(0, query, new T()).Cast<T>().ToList();
    }
    #endregion

    #region Change Data    
    public string Save()
    {        
        if (Id.IsZero()) return DataAccess.Insert(0, GetTableName(this.GetType()), this);
        else return DataAccess.Update(0, GetTableName(this.GetType()), this);
    }
    public string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", GetTableName(this.GetType()));
    }
    #endregion

    #endregion
}