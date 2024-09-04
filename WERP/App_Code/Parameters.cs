using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.Xml.Linq;

public class Parameters
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
    #endregion    

    #region Fields    
    [Column(Field = "Key")]
    public string Key { get; set; }
    [Column(Field = "Text")]
    public string Text { get; set; }
    [Column(Field = "Value")]
    public string Value { get; set; }
    [Column(Field = "Attribute")]
    public string Attribute { get; set; }
    #endregion
    #endregion

    #region Methods
    #region Get Data
    public static Parameters GetByKey(object Key)
    {
        return (Parameters)DataAccess.GetSingleRowByQuery(0, $"Select * from Parameters where [Key] = '{Key}' ", new Parameters());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Parameters", new Parameters());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Key", Key));
        pList.Add(new clsParameter("Text", Text));
        pList.Add(new clsParameter("Value", Value));
        pList.Add(new clsParameter("Attribute", Attribute));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ParametersInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Key", Key));
        pList.Add(new clsParameter("Text", Text));
        pList.Add(new clsParameter("Value", Value));
        pList.Add(new clsParameter("Attribute", Attribute));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "ParametersUpdate", pList);
    }
    #endregion
    #endregion
}