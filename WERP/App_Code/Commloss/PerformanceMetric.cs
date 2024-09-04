using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

/// <summary>
/// Summary description for PerformanceMetric
/// </summary>
public class PerformanceMetric
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
    [Column(Field = "RevenueSaved", SearchName = "a.RevenueSaved", SortName = "a.RevenueSaved")]
    public decimal RevenueSaved { get; set; }
    [Column(Field = "VolumeSaved", SearchName = "a.VolumeSaved", SortName = "a.VolumeSaved")]
    public decimal VolumeSaved { get; set; }
    [Column(Field = "CommercialSaved", SearchName = "a.CommercialSaved", SortName = "a.CommercialSaved")]
    public decimal CommercialSaved { get; set; }
    [Column(Field = "RevenueLoss", SearchName = "a.RevenueLoss", SortName = "a.RevenueLoss")]
    public decimal RevenueLoss { get; set; }
    [Column(Field = "VolumeLoss", SearchName = "a.VolumeLoss", SortName = "a.VolumeLoss")]
    public decimal VolumeLoss { get; set; }
    [Column(Field = "OngoingIssued", SearchName = "a.OngoingIssued", SortName = "a.OngoingIssued")]
    public int OngoingIssued { get; set; }
    [Column(Field = "FinishedIssued", SearchName = "a.FinishedIssued", SortName = "a.FinishedIssued")]
    public int FinishedIssued { get; set; }
    #endregion

    #region Methods
    public static PerformanceMetric GetByMonthYear(object Month, object Year)
    {        
        return (PerformanceMetric)DataAccess.GetSingleRowByQuery(3, $"SELECT TOP 1 * FROM PerformanceMetric WHERE MONTH(IssuedDate) = '{Month}' AND YEAR(IssuedDate) = '{Year}' ORDER BY IssuedDate DESC", new PerformanceMetric());
    }
    public static List<object> GetByRevenueVolume(object FiedName, object Year)
    {
        return DataAccess.GetDataByQuery(3, $"WITH Temp AS ( SELECT MonthIssuedDate, {FiedName}, ROW_NUMBER() OVER (PARTITION BY MONTH(IssuedDate) ORDER BY IssuedDate DESC) AS No FROM PerformanceMetric WHERE YearIssuedDate = {Year}) SELECT MonthIssuedDate, {FiedName} FROM Temp WHERE No = 1 order by MonthIssuedDate asc", new PerformanceMetric());
    }
    #endregion
}