using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class Area
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
    #endregion

    #region Fields    
    [Column(Field = "Name", Required = true, SearchName = "Name", SortName = "Name")]
    public string Name { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    #endregion

    #region Additional        
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Area GetById(object Id)
    {
        return (Area)DataAccess.GetSingleRowByQuery(0, $"Select * from Area where Id = '{Id}'", new Area());
    }
    public static Area GetByKey(object Name, object Id)
    {
        return (Area)DataAccess.GetSingleRowByQuery(0, $"Select * from Area where Name = '{Name}' AND Id <> '{Id}'", new Area());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Area", new Area());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Area where Active = 1", new Area());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "GetByCriteria", pList, out TotalRow, new Area());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Table", "Area"));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "InsertTable", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Table", "Area"));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "UpdateTable", pList);
    }
    #endregion

    #endregion
}