using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

public class KetCatat
{
    #region Properties
    [Column(Field = "No")]
    public double No { get; set; }
    [Column(Field = "kd_ketcatat", SearchName = "kd_ketcatat", SortName = "kd_ketcatat")]
    public string kd_ketcatat { get; set; }
    [Column(Field = "Nama", SearchName = "Nama", SortName = "Nama")]
    public string Nama { get; set; }

    #endregion

    #region Methods

    #region Get Data
    public static List<object> GetAll()
    {
        return DataAccess.GetDataByQuery(1, "Select * from KetCatat", new KetCatat());
    }
    public static List<object> GetByCode(string Code)
    {
        return DataAccess.GetDataByQuery(1, $"Select * from KetCatat where kd_ketcatat = '{Code}'", new KetCatat());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(1, "GetByCriteria", pList, out TotalRow, new KetCatat());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("kd_ketcatat", kd_ketcatat));
        pList.Add(new clsParameter("Nama", Nama));
        return DataAccess.Save(1, "KetCatatInsert", pList);
    }
    public string InsertGCP()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("kd_ketcatat", kd_ketcatat));
        pList.Add(new clsParameter("Nama", Nama));
        return DataAccess.Save(1, "KetCatatInsert", pList);
    }
    #endregion

    #endregion

}
public class KetCatatWrapper
{
    public List<KetCatat> data { get; set; }
}
