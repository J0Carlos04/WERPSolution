using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

public class PB
{
    #region Properties
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "noreg", SearchName = "noreg", SortName = "noreg")]
    public string noreg { get; set; }
    [Column(Field = "tgl_pasang", SearchName = "tgl_pasang", SortName = "tgl_pasang")]
    public string tgl_pasang { get; set; }
    [Column(Field = "nolg", SearchName = "nolg", SortName = "nolg")]
    public string nolg { get; set; }
    [Column(Field = "alamat", SearchName = "alamat", SortName = "alamat")]
    public string alamat { get; set; }
    [Column(Field = "kd_merkmeter", SearchName = "kd_merekmeter", SortName = "kd_merekmeter")]
    public string kd_merkmeter { get; set; }
    [Column(Field = "nometer", SearchName = "nometer", SortName = "nometer")]
    public string nometer { get; set; }
    [Column(Field = "kd_ukmeter", SearchName = "kd_ukmeter", SortName = "kd_ukmeter")]
    public string kd_ukmeter { get; set; }
    [Column(Field = "ukr", SearchName = "ukr", SortName = "ukr")]
    public string ukr { get; set; }
    [Column(Field = "nosegelmeter", SearchName = "nosegelmeter", SortName = "nosegelmeter")]
    public string nosegelmeter { get; set; }
    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetAll()
    {
        return DataAccess.GetDataByQuery(1, "Select * from SambunganBaru", new PB());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "GetByCriteria", pList, out TotalRow, new PB());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("noreg", noreg));
        pList.Add(new clsParameter("tgl_pasang", tgl_pasang));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("alamat", alamat));
        pList.Add(new clsParameter("kd_merkmeter", kd_merkmeter));
        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("nosegelmeter", nosegelmeter));
        return DataAccess.Save(1, "PBInsert", pList);
    }
    public string InsertGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("noreg", noreg));
        pList.Add(new clsParameter("tgl_pasang", tgl_pasang));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("alamat", alamat));
        pList.Add(new clsParameter("kd_merkmeter", kd_merkmeter));
        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("nosegelmeter", nosegelmeter));
        return DataAccess.Save(1, "PBInsert", pList);
    }
    #endregion

    #endregion
}

public class PBWrapper
{
    public List<PB> data { get; set; }
}
