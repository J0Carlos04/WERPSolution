using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Activity
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
    [Column(Field = "CustNo", SearchName = "a.CustNo", SortName = "a.CustNo")]
    public string CustNo { get; set; }
    [Column(Field = "RevenueLoss", SearchName = "a.RevenueLoss", SortName = "a.RevenueLoss")]
    public decimal RevenueLoss { get; set; }
    [Column(Field = "VolumeLoss", SearchName = "a.VolumeLoss", SortName = "a.VolumeLoss")]
    public decimal VolumeLoss { get; set; }
    [Column(Field = "Issue", SearchName = "a.Issue", SortName = "a.Issue")]
    public string Issue { get; set; }
    [Column(Field = "Action", SearchName = "a.Action", SortName = "a.Action")]
    public string Action { get; set; }
    [Column(Field = "Status", SearchName = "a.Status", SortName = "a.Status")]
    public string Status { get; set; }
    [Column(Field = "PreviousWorkOrder", SearchName = "a.PreviousWorkOrder", SortName = "a.PreviousWorkOrder")]
    public int PreviousWorkOrder { get; set; }
    [Column(Field = "IssuedStartDate", SearchName = "a.IssueStartDate", SortName = "a.IssueStartDate")]
    public DateTime IssuedStartDate { get; set; }
    #endregion

    #region Methods
    public static List<object> GetByByMonthYear(object Month, object Year)
    {
        return DataAccess.GetDataBySP(3, "ActivityGetByMonthYear", new List<clsParameter> { new clsParameter("Year", Year), new clsParameter("Month", Month) }, new Activity());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(3, "ActivityGetByCriteria", pList, out TotalRow, new Activity());
    }
    public static List<object> GetByYear(object Year)
    {
        return DataAccess.GetDataBySP(3, "ActivityGetByYear", new List<clsParameter> { new clsParameter("Year", Year) }, new Activity());
    }
    #endregion
}