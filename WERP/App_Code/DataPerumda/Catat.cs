using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

public class Catat
{
    #region Properties 
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "thbl", SearchName = "thbl", SortName = "thbl")]
    public string thbl { get; set; }
    [Column(Field = "nolg", SearchName = "nolg", SortName = "nolg")]
    public string nolg { get; set; }
    [Column(Field = "nama", SearchName = "nama", SortName = "nama")]
    public string nama { get; set; }
    [Column(Field = "almt", SearchName = "almt", SortName = "almt")]
    public string almt { get; set; }
    [Column(Field = "trp", SearchName = "trp", SortName = "trp")]
    public string trp { get; set; }
    [Column(Field = "namatrp", SearchName = "namatrp", SortName = "namatrp")]
    public string namatrp { get; set; }
    [Column(Field = "kd_ukmeter", SearchName = "kd_ukmeter", SortName = "kd_ukmeter")]
    public string kd_ukmeter { get; set; }
    [Column(Field = "ukr", SearchName = "ukr", SortName = "ukr")]
    public string ukr { get; set; }
    [Column(Field = "tglcatat", SearchName = "tglcatat", SortName = "tglcatat")]
    public string tglcatat { get; set; }
    [Column(Field = "pencatat", SearchName = "pencatat", SortName = "pencatat")]
    public string pencatat { get; set; }
    [Column(Field = "tmss", SearchName = "tmss", SortName = "tmss")]
    public string tmss { get; set; }
    [Column(Field = "ketcatat", SearchName = "ketcatat", SortName = "ketcatat")]
    public string ketcatat { get; set; }
    [Column(Field = "stml", SearchName = "stml", SortName = "stml")]
    public decimal stml { get; set; }
    [Column(Field = "standcatat", SearchName = "standcatat", SortName = "standcatat")]
    public decimal standcatat { get; set; }
    [Column(Field = "stma", SearchName = "stma", SortName = "stma")]
    public decimal stma { get; set; }
    [Column(Field = "taksasi", SearchName = "taksasi", SortName = "taksasi")]
    public decimal taksasi { get; set; }
    [Column(Field = "pakai", SearchName = "pakai", SortName = "pakai")]
    public decimal pakai { get; set; }
    [Column(Field = "lat_long", SearchName = "lat_long", SortName = "lat_long")]
    public string lat_long { get; set; }
    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetAll()
    {
        return DataAccess.GetDataByQuery(1, "Select * from PencatatanMeter", new Catat());
    }
    public static Catat GetByKey(string nolg, string tglcatat)
    {
        return (Catat)DataAccess.GetSingleRowByQuery(1, $"Select top 1 * from Catat where nolg = '{nolg}' and tglcatat = '{tglcatat}' ", new Catat());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "CatatGetByCriteria", pList, out TotalRow, new Catat());
    }
    #endregion

    #region Change Date
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("thbl", thbl));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("nama", nama));
        pList.Add(new clsParameter("almt", almt));
        pList.Add(new clsParameter("trp", trp));
        pList.Add(new clsParameter("namatrp", namatrp));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("tglcatat", tglcatat));
        pList.Add(new clsParameter("pencatat", pencatat));
        pList.Add(new clsParameter("tmss", tmss));
        pList.Add(new clsParameter("stml", stml));
        pList.Add(new clsParameter("standcatat", standcatat));
        pList.Add(new clsParameter("stma", stma));
        pList.Add(new clsParameter("taksasi", taksasi));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("lat_long", lat_long));
        return DataAccess.Save(1, "CatatInsert", pList);
    }
    public string InsertGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("thbl", thbl));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("nama", nama));
        pList.Add(new clsParameter("almt", almt));
        pList.Add(new clsParameter("trp", trp));
        pList.Add(new clsParameter("namatrp", namatrp));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("tglcatat", tglcatat));
        pList.Add(new clsParameter("pencatat", pencatat));
        pList.Add(new clsParameter("tmss", tmss));
        pList.Add(new clsParameter("stml", stml));
        pList.Add(new clsParameter("standcatat", standcatat));
        pList.Add(new clsParameter("stma", stma));
        pList.Add(new clsParameter("taksasi", taksasi));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("lat_long", lat_long));
        return DataAccess.Save(1, "CatatInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("thbl", thbl));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("nama", nama));
        pList.Add(new clsParameter("almt", almt));
        pList.Add(new clsParameter("trp", trp));
        pList.Add(new clsParameter("namatrp", namatrp));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("tglcatat", tglcatat));
        pList.Add(new clsParameter("pencatat", pencatat));
        pList.Add(new clsParameter("tmss", tmss));
        pList.Add(new clsParameter("stml", stml));
        pList.Add(new clsParameter("standcatat", standcatat));
        pList.Add(new clsParameter("stma", stma));
        pList.Add(new clsParameter("taksasi", taksasi));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("lat_long", lat_long));
        return DataAccess.Save(1, "CatatUpdate", pList);
    }
    public string UpdateGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("thbl", thbl));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("nama", nama));
        pList.Add(new clsParameter("almt", almt));
        pList.Add(new clsParameter("trp", trp));
        pList.Add(new clsParameter("namatrp", namatrp));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("tglcatat", tglcatat));
        pList.Add(new clsParameter("pencatat", pencatat));
        pList.Add(new clsParameter("tmss", tmss));
        pList.Add(new clsParameter("stml", stml));
        pList.Add(new clsParameter("standcatat", standcatat));
        pList.Add(new clsParameter("stma", stma));
        pList.Add(new clsParameter("taksasi", taksasi));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("lat_long", lat_long));
        return DataAccess.Save(1, "CatatUpdate", pList);
    }
    #endregion

    #endregion
}

public class CatatWrapper
{
    public List<Catat> data { get; set; }
}
