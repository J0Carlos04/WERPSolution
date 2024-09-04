using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

public class PK
{
    #region Properties
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "no_mohonan_pk", SearchName = "no_mohonan_pk", SortName = "no_mohonan_pk")]
    public string no_mohonan_pk { get; set; }
    [Column(Field = "tgl_pasanglagi", SearchName = "tgl_pasanglagi", SortName = "tgl_pasanglagi")]
    public DateTime tgl_pasanglagi { get; set; }
    [Column(Field = "nolangganan", SearchName = "nolangganan", SortName = "nolangganan")]
    public string nolangganan { get; set; }
    [Column(Field = "kd_merkmeter", SearchName = "kd_merkmeter", SortName = "kd_merkmeter")]
    public string kd_merkmeter { get; set; }
    [Column(Field = "nometer", SearchName = "nometer", SortName = "nometer")]
    public string nometer { get; set; }
    [Column(Field = "nosegelmeter", SearchName = "nosegelmeter", SortName = "nosegelmeter")]
    public string nosegelmeter { get; set; }
    [Column(Field = "kd_ukmeter", SearchName = "kd_ukmeter", SortName = "kd_ukmeter")]
    public string kd_ukmeter { get; set; }
    [Column(Field = "ukr", SearchName = "ukr", SortName = "ukr")]
    public string ukr { get; set; }
    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetAll()
    {
        return DataAccess.GetDataByQuery(1, "Select * from BukaKembali", new PK());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "GetByCriteria", pList, out TotalRow, new PK());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("no_mohonan_pk", no_mohonan_pk));
        pList.Add(new clsParameter("tgl_pasanglagi", tgl_pasanglagi));
        pList.Add(new clsParameter("nolangganan", nolangganan));
        pList.Add(new clsParameter("kd_merkmeter", kd_merkmeter));
        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("nosegelmeter", nosegelmeter));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        return DataAccess.Save(1, "PKInsert", pList);
    }
    public string InsertGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("no_mohonan_pk", no_mohonan_pk));
        pList.Add(new clsParameter("tgl_pasanglagi", tgl_pasanglagi));
        pList.Add(new clsParameter("nolangganan", nolangganan));
        pList.Add(new clsParameter("kd_merkmeter", kd_merkmeter));
        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("nosegelmeter", nosegelmeter));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));
        return DataAccess.Save(1, "PKInsert", pList);
    }
    #endregion

    #endregion
}

public class PKWrapper
{
    public List<PK> data { get; set; }
}
