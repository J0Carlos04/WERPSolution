using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using FineUI;


/// <summary>
/// Summary description for StationMetric
/// </summary>
public class StationMetric
{
    #region Properties
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "IssuedDate", SearchName = "a.IssuedDate", SortName = "a.IssuedDate")]
    public DateTime IssuedDate { get; set; }
    [Column(Field = "MonthIssuedDate", SearchName = "a.MonthIssuedDate", SortName = "a.MonthIssuedDate")]
    public int MonthIssuedDate { get; set; }
    [Column(Field = "YearIssuedDate", SearchName = "a.YearIssuedDate", SortName = "a.YearIssuedDate")]
    public int YearIssuedDate { get; set; }
    [Column(Field = "SiteNumber", SearchName = "a.SiteNumber", SortName = "a.SiteNumber")]
    public int SiteNumber { get; set; }
    [Column(Field = "StationName", SearchName = "a.StationName", SortName = "a.StationName")]
    public string StationName { get; set; }
    [Column(Field = "RevenueSaved", SearchName = "a.RevenueSaved", SortName = "a.RevenueSaved")]
    public decimal RevenueSaved { get; set; }
    [Column(Field = "VolumeSaved", SearchName = "a.VolumeSaved", SortName = "a.VolumeSaved")]
    public decimal VolumeSaved { get; set; }
    [Column(Field = "FinishedIssued", SearchName = "a.FinishedIssued", SortName = "a.FinishedIssued")]
    public int FinishedIssued { get; set; }
    #endregion

    #region Methods
    public static List<object> GetByByMonthYear(object Month, object Year)
    {
        return DataAccess.GetDataBySP(3, "StationMetricGetByMonthYear", new List<clsParameter> { new clsParameter("Year", Year), new clsParameter("Month", Month) }, new StationMetric());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(3, "StationMetricGetByCriteria", pList, out TotalRow, new StationMetric());
    }
    public static List<object> GetByYear(object Year)
    {
        return DataAccess.GetDataBySP(3, "StationMetricGetByYear", new List<clsParameter> { new clsParameter("Year", Year) }, new StationMetric());
    }
    #endregion
}