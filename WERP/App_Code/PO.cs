using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

/// <summary>
/// Summary description for PO
/// </summary>
public class PO
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

    #region Foreign Key    
    #endregion

    #region Fields    
    [Column(Field = "PONo", Title = "PO No.", Required = true, SearchName = "a.PONo", SortName = "a.PONo")]
    public string PONo { get; set; }
    [Column(Field = "PODate", Required = true, SearchName = "a.PODate", SortName = "a.PODate")]
    public DateTime PODate { get; set; }
    [Column(Field = "PRNo", Title = "PR No.", Required = true, SearchName = "a.PRNo", SortName = "a.PRNo")]
    public string PRNo { get; set; }
    [Column(Field = "QuotNo", Title = "Quot No.", Required = true, SearchName = "a.QuotNo", SortName = "a.QuotNo")]
    public string QuotNo { get; set; }
    [Column(Field = "Detail", SearchName = "a.Detail", SortName = "a.Detail")]
    public string Detail { get; set; }    
    [Column(Field = "FileName", SearchName = "a.FileName", SortName = "a.FileName")]
    public string FileName { get; set; }
    [Column(Field = "FileData")]
    public byte[] FileData { get; set; }
    #endregion

    #region Additional  
    [Column(Field = "DataText")]
    public string DataText { get; set; }
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static PO GetById(object Id)
    {
        return (PO)DataAccess.GetSingleRowByQuery(0, $"Select * from PO a where a.Id = '{Id}'", new PO());
    }
    public static PO GetByKey(object PONo, object Id)
    {
        return (PO)DataAccess.GetSingleRowByQuery(0, $"Select * from PO where PONo = '{PONo}' AND Id <> '{Id}'", new PO());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from PO", new PO());
    }   
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "POGetByCriteria", pList, out TotalRow, new PO());
    }
    #endregion

    #region Change Data
    public string Insert()
    { 
        List<clsParameter> pList = new List<clsParameter>();        
        pList.Add(new clsParameter("PONo", PONo));
        pList.Add(new clsParameter("PODate", PODate));
        pList.Add(new clsParameter("PRNo", PRNo));
        pList.Add(new clsParameter("QuotNo", QuotNo));
        pList.Add(new clsParameter("Detail", Detail));
        pList.Add(new clsParameter("FileName", FileName));
        pList.Add(new clsParameter("FileData", FileData));                
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "POInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));        
        pList.Add(new clsParameter("PONo", PONo));
        pList.Add(new clsParameter("PODate", PODate));
        pList.Add(new clsParameter("PRNo", PRNo));
        pList.Add(new clsParameter("QuotNo", QuotNo));
        pList.Add(new clsParameter("Detail", Detail));
        pList.Add(new clsParameter("FileName", FileName));
        pList.Add(new clsParameter("FileData", FileData));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "POUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "PO");
    }
    #endregion

    #endregion
}