using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class Totalizer
{
    #region Properties

    #region Fields
    [Column(Field = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    [Column(Field = "FunctionLogger", SearchName = "b.Name", SortName = "b.Name")]
    public string FunctionLogger { get; set; }
    [Column(Field = "Parent", SearchName = "c.Name", SortName = "c.Name")]
    public string Parent { get; set; }
    #endregion

    #region Keys
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "FunctionLoggerId")]
    public int FunctionLoggerId { get; set; }
    [Column(Field = "ParentId")]
    public int ParentId { get; set; }

    #endregion

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

    #region Additional
    [Column(Field = "Level")]
    public int Level { get; set; }
    #endregion

    #endregion

    #region Methods

    #region Get Data
    public static Totalizer GetById(object Id)
    {
        return (Totalizer)DataAccess.GetSingleRowByQuery(0, $"Select * from Totalizer where Id = '{Id}'", new Totalizer());
    }
    public static Totalizer GetByParentId(object ParentId)
    {
        return (Totalizer)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Name Parent from Totalizer a Left Join Totalizer b on b.Id = a.ParentId where a.ParentId = '{ParentId}'", new Totalizer());
    }
    public static Totalizer GetByKey(object Name, object Id)
    {
        return (Totalizer)DataAccess.GetSingleRowByQuery(0, $"Select * from Totalizer where Name = '{Name}' AND Id <> '{Id}'", new Totalizer());
    }
    public static List<object> GetALL(string OrderBy = "a.Name")
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Name FunctionLogger, b.[Level] from Totalizer a Left Join FunctionLogger b On b.Id = a.FunctionLoggerId Order By {OrderBy}", new Totalizer());
    }
    public static List<object> GetALLActive(string OrderBy = "b.[Level]")
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Name FunctionLogger, b.[Level] from Totalizer a Left Join FunctionLogger b On b.Id = a.FunctionLoggerId where a.Active = 1 Order By {OrderBy}", new Totalizer());
    }
    public static List<object> GetALLWithNoParent(string OrderBy = "b.[Level]")
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Name FunctionLogger, b.[Level] from Totalizer a Left Join FunctionLogger b On b.Id = a.FunctionLoggerId where a.ParentId IS NULL Order By {OrderBy}", new Totalizer());
    }
    public static List<object> GetGetALLActiveByFunctionLogger(object FunctionLoggerId, string OrderBy = "b.[Level]")
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Name FunctionLogger, b.[Level] from Totalizer a Left Join FunctionLogger b On b.Id = a.FunctionLoggerId where a.Active = 1 and FunctionLoggerId = '{FunctionLoggerId}' Order By {OrderBy}", new Totalizer());
    }
    public static List<object> GetParentTotalizer(object Id)
    {
        return DataAccess.GetDataBySP(0, "GetParentTotalizer", new List<clsParameter> { new clsParameter("Id", Id) }, new Totalizer());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "TotalizerGetByCriteria", pList, out TotalRow, new Totalizer());
    }
    public static List<object> ParentGetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "TotalizerParentGetByCriteria", pList, out TotalRow, new Totalizer());
    }
    #endregion

    #region Save Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("FunctionLoggerId", FunctionLoggerId));        
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Active", Active));
        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "TotalizerInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("FunctionLoggerId", FunctionLoggerId));        
        pList.Add(new clsParameter("Name", Name));        
        
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "TotalizerUpdate", pList);
    }
    public string UpdateParentId(object Id, object ParentId, object ModifiedBy)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Totalizer Set ParentId = '{ParentId}', ModifiedBy = '{ModifiedBy}', Modified = GETDATE() where Id = '{Id}' ");
    }
    public string DeleteMapping(object Id, object ModifiedBy)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Totalizer Set ParentId = null, ModifiedBy = '{ModifiedBy}', Modified = GETDATE() where Id = '{Id}' ");
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "Totalizer");
    }
    #endregion

    #endregion            
}