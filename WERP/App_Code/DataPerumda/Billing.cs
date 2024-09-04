using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

public class Billing
{
    #region Properties
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "YearMonth", SearchName = "YearMonth", SortName = "YearMonth")]
    public string thbl { get; set; }
    [Column(Field = "NoSR", SearchName = "NoSR", SortName = "NoSR")]
    public string nolg { get; set; }
    [Column(Field = "nama", SearchName = "nama", SortName = "nama")]
    public string nama { get; set; }
    [Column(Field = "almt", SearchName = "almt", SortName = "almt")]
    public string almt { get; set; }
    [Column(Field = "trp", SearchName = "trp", SortName = "trp")]
    public string trp { get; set; }
    [Column(Field = "namatrp", SearchName = "namatrp", SortName = "namatrp")]
    public string namatrp { get; set; }
    [Column(Field = "kd_merkmeter", SearchName = "kd_merkmeter", SortName = "kd_merkmeter")]
    public string kd_merkmeter { get; set; }
    [Column(Field = "nometer", SearchName = "nometer", SortName = "nometer")]
    public string nometer { get; set; }
    [Column(Field = "nosegelmeter", SearchName = "nosegelmeter", SortName = "nosegelmeter")]
    public string nosegelmeter { get; set; }
    [Column(Field = "ukr", SearchName = "ukr", SortName = "ukr")]
    public string ukr { get; set; }
    [Column(Field = "ukmeter", SearchName = "ukmeter", SortName = "ukmeter")]
    public string ukmeter { get; set; }
    [Column(Field = "tglpasangmeter", SearchName = "tglpasangmeter", SortName = "tglpasangmeter")]
    public string tglpasangmeter { get; set; }
    [Column(Field = "tmss", SearchName = "tmss", SortName = "tmss")]
    public string tmss { get; set; }
    [Column(Field = "ketcatat", SearchName = "ketcatat", SortName = "ketcatat")]
    public string ketcatat { get; set; }
    [Column(Field = "StanLalu", SearchName = "StanLalu", SortName = "StanLalu")]
    public decimal stml { get; set; }
    [Column(Field = "StanKini", SearchName = "StanKini", SortName = "StanKini")]
    public decimal stma { get; set; }
    [Column(Field = "pakai", SearchName = "pakai", SortName = "pakai")]
    public decimal pakai { get; set; }
    [Column(Field = "jmlhargaair", SearchName = "jmlhargaair", SortName = "jmlhargaair")]
    public decimal jmlhargaair { get; set; }
    [Column(Field = "koordinatlat", SearchName = "koordinatlat", SortName = "koordinatlat")]
    public string koordinatlat { get; set; }
   [Column(Field = "koordinatlong", SearchName = "koordinatlong", SortName = "koordinatlong")]
    public string koordinatlong { get; set; }
    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "PencatatMeterGetByCriteria", pList, out TotalRow, new Billing());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("YearMonth", thbl));
        pList.Add(new clsParameter("NoSR", nolg));
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
        pList.Add(new clsParameter("StanLalu", stml));
        pList.Add(new clsParameter("StanKini", stma));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("jmlhargaair", jmlhargaair));
        pList.Add(new clsParameter("koordinatlat", koordinatlat));
        pList.Add(new clsParameter("koordinatlong", koordinatlong));
        return DataAccess.Save(1, "BillingInsert", pList);
    }
    public string InsertGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("YearMonth", thbl));
        pList.Add(new clsParameter("NoSR", nolg));
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
        pList.Add(new clsParameter("StanLalu", stml));
        pList.Add(new clsParameter("StanKini", stma));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("jmlhargaair", jmlhargaair));
        pList.Add(new clsParameter("koordinatlat", koordinatlat));
        pList.Add(new clsParameter("koordinatlong", koordinatlong));
        return DataAccess.Save(1, "BillingInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("YearMonth", thbl));
        pList.Add(new clsParameter("NoSR", nolg));
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
        pList.Add(new clsParameter("StanLalu", stml));
        pList.Add(new clsParameter("StanKini", stma));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("jmlhargaair", jmlhargaair));
        pList.Add(new clsParameter("koordinatlat", koordinatlat));
        pList.Add(new clsParameter("koordinatlong", koordinatlong));
        return DataAccess.Save(1, "BillingUpdate", pList);
    }
    public string UpdateGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("YearMonth", thbl));
        pList.Add(new clsParameter("NoSR", nolg));
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
        pList.Add(new clsParameter("StanLalu", stml));
        pList.Add(new clsParameter("StanKini", stma));
        pList.Add(new clsParameter("pakai", pakai));
        pList.Add(new clsParameter("jmlhargaair", jmlhargaair));
        pList.Add(new clsParameter("koordinatlat", koordinatlat));
        pList.Add(new clsParameter("koordinatlong", koordinatlong));
        return DataAccess.Save(1, "BillingUpdate", pList);
    }
    #endregion

    #endregion
}

public class BillingWrapper
{
    public List<Billing> data { get; set; }
}
