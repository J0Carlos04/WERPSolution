using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

public class Tutupan
{
    #region Properties
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "noreg", SearchName = "noreg", SortName = "noreg")]
    public string noreg { get; set; }
    [Column(Field = "tgl_tutup", SearchName = "tgl_tutup", SortName = "tgl_tutup")]
    public string tgl_tutup { get; set; }
    [Column(Field = "nolg", SearchName = "nolg", SortName = "nolg")]
    public string nolg { get; set; }
    [Column(Field = "kd_merekmeter", SearchName = "kd_merekmeter", SortName = "kd_merekmeter")]
    public string kd_merekmeter { get; set; }
    [Column(Field = "nometer", SearchName = "nometer", SortName = "nometer")]
    public string nometer { get; set; }
    [Column(Field = "kd_ukmeter", SearchName = "kd_ukmeter", SortName = "kd_ukmeter")]
    public string kd_ukmeter { get; set; }
    [Column(Field = "ukr", SearchName = "ukr", SortName = "ukr")]
    public string ukr { get; set; }
    [Column(Field = "stand_angkat", SearchName = "stand_angkat", SortName = "stand_angkat")]
    public string stand_angkat { get; set; }
    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "GetByCriteria", pList, out TotalRow, new Tutupan());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("noreg", noreg));
        pList.Add(new clsParameter("tgl_tutup", tgl_tutup));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("kd_merekmeter", kd_merekmeter));
        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("stand_angkat", stand_angkat));
        return DataAccess.Save(1, "TutupanInsert", pList);
    }
    public string InsertGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("noreg", noreg));
        pList.Add(new clsParameter("tgl_tutup", tgl_tutup));
        pList.Add(new clsParameter("nolg", nolg));
        pList.Add(new clsParameter("kd_merekmeter", kd_merekmeter));
        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        pList.Add(new clsParameter("stand_angkat", stand_angkat));
        return DataAccess.Save(1, "TutupanInsert", pList);
    }
    #endregion

    #endregion
}
public class TutupanWrapper
{
    public List<Tutupan> data { get; set; }
}
