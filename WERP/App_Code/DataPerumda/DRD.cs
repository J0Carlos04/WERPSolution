using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DRD
{
    #region Properties
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "thbl")]
    public string thbl { get; set; }
    [Column(Field = "nolg")]
    public string nolg { get; set; }
    [Column(Field = "nama")]
    public string nama { get; set; }
    [Column(Field = "almt")]
    public string almt { get; set; }
    [Column(Field = "trp")]
    public string trp { get; set; }
    [Column(Field = "namatrp")]
    public string namatrp { get; set; }
    [Column(Field = "kd_merkmeter")]
    public string kd_merkmeter { get; set; }
    [Column(Field = "nometer")]
    public string nometer { get; set; }
    [Column(Field = "nosegelmeter")]
    public string nosegelmeter { get; set; }
    [Column(Field = "ukr")]
    public string ukr { get; set; }
    [Column(Field = "ukmeter")]
    public string ukmeter { get; set; }
    [Column(Field = "tglpasangmeter")]
    public string tglpasangmeter { get; set; }
    [Column(Field = "tmss")]
    public string tmss { get; set; }
    [Column(Field = "ketcatat")]
    public string ketcatat { get; set; }
    [Column(Field = "stml")]
    public string stml { get; set; }
    [Column(Field = "stma")]
    public string stma { get; set; }
    [Column(Field = "pakai")]
    public string pakai { get; set; }
    [Column(Field = "taksasi")]
    public string taksasi { get; set; }
    [Column(Field = "jmlhargaair")]
    public string jmlhargaair { get; set; }

    [Column(Field = "airkotor")]
    public string airkotor { get; set; }
    [Column(Field = "beabeban")]
    public string beabeban { get; set; }
    [Column(Field = "beaadmin")]
    public string beaadmin { get; set; }
    [Column(Field = "tjtg")]
    public string tjtg { get; set; }
    [Column(Field = "jmlreknunggak")]
    public string jmlreknunggak { get; set; }
    [Column(Field = "tagnunggak")]
    public string tagnunggak { get; set; }

    [Column(Field = "koordinatlat")]
    public string koordinatlat { get; set; }
    [Column(Field = "koordinatlong")]
    public string koordinatlong { get; set; }
    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "DRDGetByCriteria", pList, out TotalRow, new DRD());
    }
    #endregion

    #region Change Data
    public string Merge()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("thbl", thbl));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("nama", nama));
        pList.Add(new clsParameter("almt", almt));
        pList.Add(new clsParameter("trp", trp));
        pList.Add(new clsParameter("namatrp", namatrp));
        pList.Add(new clsParameter("kd_merkmeter", kd_merkmeter));
        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("nosegelmeter", nosegelmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("ukmeter", ukmeter));
        pList.Add(new clsParameter("tglpasangmeter", tglpasangmeter));
        pList.Add(new clsParameter("tmss", tmss));
        pList.Add(new clsParameter("ketcatat", ketcatat));
        pList.Add(new clsParameter("stml", stml));
        pList.Add(new clsParameter("stma", stma));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("taksasi", taksasi));
        pList.Add(new clsParameter("jmlhargaair", jmlhargaair));
        pList.Add(new clsParameter("airkotor", airkotor));
        pList.Add(new clsParameter("beabeban", beabeban));
        pList.Add(new clsParameter("beaadmin", beaadmin));
        pList.Add(new clsParameter("tjtg", tjtg));
        pList.Add(new clsParameter("jmlreknunggak", jmlreknunggak));
        pList.Add(new clsParameter("tagnunggak", tagnunggak));
        pList.Add(new clsParameter("koordinatlat", koordinatlat));
        pList.Add(new clsParameter("koordinatlong", koordinatlong));
        pList.Add(new clsParameter("Modified", DateTime.Now.Date.ToSQLDate()));
        return DataAccess.Save(0, "MergeDRD", pList);
    }
    #endregion

    #endregion
}