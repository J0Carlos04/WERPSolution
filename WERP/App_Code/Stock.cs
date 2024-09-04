using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using U = Utility;

public class Stock
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

    #region Key
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }    
    #endregion

    #region Fields
    [Column(Field = "Code", SearchName = "b.Code", SortName = "b.Code")]
    public string Code { get; set; }
    [Column(Field = "Name", SearchName = "b.Name", SortName = "b.Name")]
    public string Name { get; set; }
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }
    [Column(Field = "UsedQty", SearchName = "a.UsedQty", SortName = "a.UsedQty")]
    public int UsedQty { get; set; }
    [Column(Field = "Description", Required = true, SearchName = "b.Description", SortName = "b.Description")]
    public string Description { get; set; }
    [Column(Field = "Category", Title = "Category", SearchName = "c.Name", SortName = "c.Name")]
    public string Category { get; set; }
    [Column(Field = "ItemGroup", Title = "Group", SearchName = "d.Name", SortName = "d.Name")]
    public string ItemGroup { get; set; }
    [Column(Field = "Brand", Title = "Brand", SearchName = "e.Name", SortName = "e.Name")]
    public string Brand { get; set; }
    [Column(Field = "Model", Title = "Model", SearchName = "f.Name", SortName = "f.Name")]
    public string Model { get; set; }
    [Column(Field = "Material", Title = "Material", SearchName = "g.Name", SortName = "g.Name")]
    public string Material { get; set; }
    [Column(Field = "Specs", Title = "Specs", SearchName = "h.Name", SortName = "h.Name")]
    public string Specs { get; set; }
    [Column(Field = "UOM", Title = "UOM", SearchName = "i.Name", SortName = "i.Name")]
    public string UOM { get; set; }
    [Column(Field = "Size", Required = true, SearchName = "b.Size", SortName = "b.Size")]
    public string Size { get; set; }
    [Column(Field = "Threshold", Required = true, SearchName = "Threshold", SortName = "Threshold")]
    public int Threshold { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "b.Active", SortName = "b.Active")]
    public bool Active { get; set; }
    #endregion

    #region Additional    
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Stock GetById(object Id)
    {
        return (Stock)DataAccess.GetSingleRowByQuery(0, $"Select * from Stock where Id = '{Id}'", new Stock());
    }
    public static Stock GetByItemId(object ItemId)
    {
        return (Stock)DataAccess.GetSingleRowByQuery(0, $"Select * from Stock where ItemId = '{ItemId}'", new Stock());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Stock", new Stock());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockGetByCriteria", pList, out TotalRow, new Stock());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockUpdate", pList);
    }
    public string UpdateRetur()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Stock Set Qty = '{Qty}' where Id = '{Id}' ");
    }
    public string UpdateQty()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Stock Set Qty = '{Qty}', UsedQty = '{UsedQty}', ModifiedBy = '{U.GetUsername()}', Modified = GETDATE() where Id = '{Id}' ");
    }
    #endregion

    #endregion
}