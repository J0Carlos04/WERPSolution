using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.Xml.Linq;

public class Schedule
{
    #region Properties
    #region Default
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "CreatedBy")]
    public string CreatedBy { get; set; }
    [Column(Field = "Created")]
    public DateTime Created { get; set; }
    [Column(Field = "ModifiedBy")]
    public string ModifiedBy { get; set; }
    [Column(Field = "Modified")]
    public DateTime Modified { get; set; }
    [Column(Field = "No")]
    public double No { get; set; }
    public bool IsChecked { get; set; }
    public string Mode { get; set; }
    #endregion

    #region Key
    [Column(Field = "ScheduleMaintenanceId")]
    public int ScheduleMaintenanceId { get; set; }
    #endregion

    #region Fields    
    [Column(Field = "StartDate")]
    public DateTime StartDate { get; set; }
    [Column(Field = "Period")]
    public int Period { get; set; }
    [Column(Field = "PeriodType")]
    public string PeriodType { get; set; }
    [Column(Field = "EndDate")]
    public DateTime EndDate { get; set; }
    [Column(Field = "Active")]
    public bool Active { get; set; }
    #endregion

    #region Additional Fields
    [Column(Field = "OrderDate")]
    public DateTime OrderDate { get; set; }
    [Column(Field = "EarliestStartDate")]
    public DateTime EarliestStartDate { get; set; }
    [Column(Field = "LatestStartDate")]
    public DateTime LatestStartDate { get; set; }
    #endregion

    #endregion

    #region Methods

    #region Get Data
    public static Schedule GetById(object Id)
    {
        return (Schedule)DataAccess.GetSingleRowByQuery(0, $"Select * from Schedule where Id = '{Id}'", new Schedule());
    }
    public static List<object> GetByScheduleMaintenanceId(object ScheduleMaintenanceId)
    {
        return DataAccess.GetDataByQuery(0, $"Select row_number() over (order by Id) as No, * from Schedule where ScheduleMaintenanceId = '{ScheduleMaintenanceId}'", new Schedule());
    }
    #endregion

    #region Change Data
    public static string InactiveByScheduleMaintenanceId(object ScheduleMaintenanceId)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update Schedule Set Active = 0 where ScheduleMaintenanceId = '{ScheduleMaintenanceId}' ");
    }
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("ScheduleMaintenanceId", ScheduleMaintenanceId));
        pList.Add(new clsParameter("Period", Period));
        pList.Add(new clsParameter("PeriodType", PeriodType));
        pList.Add(new clsParameter("StartDate", StartDate.ToSQLDate()));
        pList.Add(new clsParameter("EndDate", EndDate.ToSQLDate()));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ScheduleInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("ScheduleMaintenanceId", ScheduleMaintenanceId));
        pList.Add(new clsParameter("Period", Period));
        pList.Add(new clsParameter("PeriodType", PeriodType));
        pList.Add(new clsParameter("StartDate", StartDate.ToSQLDate()));
        pList.Add(new clsParameter("EndDate", EndDate.ToSQLDate()));
        pList.Add(new clsParameter("Active", Active));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ScheduleUpdate", pList);
    }
    #endregion

    #endregion
}