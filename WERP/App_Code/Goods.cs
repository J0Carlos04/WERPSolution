using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

[Serializable]
public class Goods
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
    [Column(Field = "CategoryId")]
    public int CategoryId { get; set; }
    [Column(Field = "ItemGroupId")]
    public int ItemGroupId { get; set; }
    [Column(Field = "BrandId")]
    public int BrandId { get; set; }
    [Column(Field = "ModelId")]
    public int ModelId { get; set; }
    [Column(Field = "MaterialId")]
    public int MaterialId { get; set; }
    [Column(Field = "SpecsId")]
    public int SpecsId { get; set; }
    [Column(Field = "UOMId")]
    public int UOMId { get; set; }
    #endregion

    #region Fields    
    [Column(Field = "Code", Required = true, SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "Name", Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Description", Required = true, SearchName = "a.Description", SortName = "a.Description")]
    public string Description { get; set; }

    [Column(Field = "Category", Title = "Category", SearchName = "b.Name", SortName = "b.Name")]
    public string Category { get; set; }
    [Column(Field = "ItemGroup", Title = "Group", SearchName = "c.Name", SortName = "c.Name")]
    public string ItemGroup { get; set; }
    [Column(Field = "Brand", Title = "Brand", SearchName = "d.Name", SortName = "d.Name")]
    public string Brand { get; set; }
    [Column(Field = "Model", Title = "Model", SearchName = "e.Name", SortName = "e.Name")]
    public string Model { get; set; }
    [Column(Field = "Material", Title = "Material", SearchName = "f.Name", SortName = "f.Name")]
    public string Material { get; set; }
    [Column(Field = "Specs", Title = "Specs", SearchName = "g.Name", SortName = "g.Name")]
    public string Specs { get; set; }
    [Column(Field = "UOM", Title = "UOM", SearchName = "g.Name", SortName = "g.Name")]
    public string UOM { get; set; }

    [Column(Field = "Size", Required = true, SearchName = "Size", SortName = "Size")]
    public string Size { get; set; }
    [Column(Field = "Threshold", Required = true, SearchName = "Threshold", SortName = "Threshold")]
    public int Threshold { get; set; }
    [Column(Field = "StockQty", SearchName = "i.Qty", SortName = "i.Qty")]
    public int StockQty { get; set; }
    [Column(Field = "UseSKU", Title = "Use SKU", SearchName = "a.UseSKU", SortName = "a.UseSKU")]
    public bool UseSKU { get; set; }
    [Column(Field = "Active", Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public bool Active { get; set; }
    #endregion    
    #endregion

    #region Methods

    #region Get Data
    public static Goods GetById(object Id)
    {
        return (Goods)DataAccess.GetSingleRowBySP(0, "ItemGetById", new List<clsParameter> { new clsParameter("Id", Id) }, new Goods());
    }
    public static Goods GetByCode(object Code)
    {
        return (Goods)DataAccess.GetSingleRowByQuery(0, $"Select * from Item where Code = '{Code}'", new Goods());
    }
    public static Goods GetByCodeName(object Code, object Name)
    {
        return (Goods)DataAccess.GetSingleRowByQuery(0, $"Select * from Item where Code = '{Code}' and Name = '{Name}'", new Goods());
    }
    public static Goods GetByKey(object Name, object Id)
    {
        return (Goods)DataAccess.GetSingleRowByQuery(0, $"Select * from Item where Name = '{Name}' AND Id <> '{Id}'", new Goods());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Item", new Goods());
    }
    public static List<object> GetALLActive()
    {
        return DataAccess.GetDataByQuery(0, "Select * from Item where Active = 1", new Goods());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "ItemGetByCriteria", pList, out TotalRow, new Goods());
    }
    public static List<object> GetByScrolling(List<clsParameter> pList)
    {
        return DataAccess.GetDataBySP(0, "ItemGetByScrolling", pList, new Goods());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("CategoryId", CategoryId));
        pList.Add(new clsParameter("ItemGroupId", ItemGroupId));
        pList.Add(new clsParameter("BrandId", BrandId));
        pList.Add(new clsParameter("ModelId", ModelId));
        pList.Add(new clsParameter("MaterialId", MaterialId));
        pList.Add(new clsParameter("SpecsId", SpecsId));
        pList.Add(new clsParameter("UOMId", UOMId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Description", Description));
        pList.Add(new clsParameter("Size", Size));
        pList.Add(new clsParameter("Threshold", Threshold));
        pList.Add(new clsParameter("UseSKU", UseSKU));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ItemInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("CategoryId", CategoryId));
        pList.Add(new clsParameter("ItemGroupId", ItemGroupId));
        pList.Add(new clsParameter("BrandId", BrandId));
        pList.Add(new clsParameter("ModelId", ModelId));
        pList.Add(new clsParameter("MaterialId", MaterialId));
        pList.Add(new clsParameter("SpecsId", SpecsId));
        pList.Add(new clsParameter("UOMId", UOMId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Name", Name));
        pList.Add(new clsParameter("Description", Description));
        pList.Add(new clsParameter("Size", Size));
        pList.Add(new clsParameter("Threshold", Threshold));
        pList.Add(new clsParameter("UseSKU", UseSKU));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "ItemUpdate", pList);
    }    
    #endregion

    #endregion
}