using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

public class GantiMeter
{
    #region Properties
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "no_mohonan_ganmeter", SearchName = "no_mohonan_ganmeter", SortName = "no_mohonan_ganmeter")]
    public string no_mohonan_ganmeter { get; set; }
    [Column(Field = "nolangganan", SearchName = "nolangganan", SortName = "nolangganan")]
    public string nolangganan { get; set; }
    [Column(Field = "tgl_pasang", SearchName = "tgl_pasang", SortName = "tgl_pasang")]
    public string tgl_pasang { get; set; }

    [Column(Field = "nometer_lama", SearchName = "nometer_lama", SortName = "nometer_lama")]
    public string nometer_lama { get; set; }
    [Column(Field = "kd_merek_lama", SearchName = "kd_merek_lama", SortName = "kd_merek_lama")]
    public string kd_merek_lama { get; set; }

    [Column(Field = "ukr_lama", SearchName = "ukr_lama", SortName = "ukr_lama")]
    public string ukr_lama { get; set; }
    [Column(Field = "ukmeter_lama", SearchName = "ukmeter_lama", SortName = "ukmeter_lama")]
    public string ukmeter_lama { get; set; }

    [Column(Field = "nometer", SearchName = "nometer", SortName = "nometer")]
    public string nometer { get; set; }
    [Column(Field = "kd_merekmeter", SearchName = "kd_merekmeter", SortName = "kd_merekmeter")]
    public string kd_merekmeter { get; set; }

    [Column(Field = "kd_ukmeter", SearchName = "kd_ukmeter", SortName = "kd_ukmeter")]
    public string kd_ukmeter { get; set; }
    [Column(Field = "ukr", SearchName = "ukr", SortName = "ukr")]
    public string ukr { get; set; }
    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetAll()
    {
        return DataAccess.GetDataByQuery(1, "Select * from GantiMeter", new GantiMeter());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "GetByCriteria", pList, out TotalRow, new GantiMeter());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("no_mohonan_ganmeter", no_mohonan_ganmeter));
        pList.Add(new clsParameter("nolangganan", nolangganan));
        pList.Add(new clsParameter("tgl_pasang", tgl_pasang));

        pList.Add(new clsParameter("nometer_lama", nometer_lama));
        pList.Add(new clsParameter("kd_merek_lama", kd_merek_lama));
        pList.Add(new clsParameter("ukr_lama", ukr_lama));
        pList.Add(new clsParameter("ukmeter_lama", ukmeter_lama));

        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("kd_merekmeter", kd_merekmeter));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));

        return DataAccess.Save(1, "GantiMeterInsert", pList);
    }
    public string InsertGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("no_mohonan_ganmeter", no_mohonan_ganmeter));
        pList.Add(new clsParameter("nolangganan", nolangganan));
        pList.Add(new clsParameter("tgl_pasang", tgl_pasang));

        pList.Add(new clsParameter("nometer_lama", nometer_lama));
        pList.Add(new clsParameter("kd_merek_lama", kd_merek_lama));
        pList.Add(new clsParameter("ukr_lama", ukr_lama));
        pList.Add(new clsParameter("ukmeter_lama", ukmeter_lama));

        pList.Add(new clsParameter("nometer", nometer));
        pList.Add(new clsParameter("kd_merekmeter", kd_merekmeter));
        pList.Add(new clsParameter("kd_ukmeter", kd_ukmeter));
        pList.Add(new clsParameter("ukr", ukr));

        return DataAccess.Save(1, "GantiMeterInsert", pList);
    }
    #endregion

    #endregion
}
public class GantiMeterWrapper
{
    public List<GantiMeter> data { get; set; }
}
