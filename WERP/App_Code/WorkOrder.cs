using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Microsoft.Exchange.WebServices.Data;
using System.Drawing;
using System.Xml.Linq;
using U = Utility;

public class WorkOrder
{
    #region Properties

    #region Fields
    [Column(Field = "Code", SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "OrderDate", Title = "Order Date", SearchName = "a.OrderDate", SortName = "a.OrderDate")]
    public DateTime OrderDate { get; set; }
    [Column(Field = "EarliestStartDate", Title = "Earliest Start Date", SearchName = "EarliestStartDate", SortName = "EarliestStartDate")]
    public DateTime EarliestStartDate { get; set; }
    [Column(Field = "LatestStartDate", Title = "Latest Start Date", SearchName = "a.LatestStartDate", SortName = "a.LatestStartDate")]
    public DateTime LatestStartDate { get; set; }
    [Column(Field = "StartDate", Title = "Start Date", SearchName = "a.StartDate", SortName = "a.StartDate")]
    public DateTime StartDate { get; set; }
    [Column(Field = "OperatorType", Required = true, SearchName = "a.OperatorType", SortName = "a.OperatorType")]
    public string OperatorType { get; set; }
    [Column(Field = "Vendor", SearchName = "Vendor", SortName = "Vendor")]
    public string Vendor { get; set; }
    [Column(Field = "Operators", Title = "Operator", SearchName = "Operators", SortName = "Operators")]
    public string Operators { get; set; }
    [Column(Field = "OperatorUserName", Title = "User Name", SearchName = "OperatorUserName", SortName = "OperatorUserName")]
    public string OperatorUserName { get; set; }
    [Column(Field = "Status", SearchName = "a.Status", SortName = "a.Status")]
    public string Status { get; set; }
    [Column(Field = "Result", SearchName = "a.Result", SortName = "a.Result")]
    public string Result { get; set; }
    [Column(Field = "Remarks", SearchName = "a.Remarks", SortName = "a.Remarks")]
    public string Remarks { get; set; }
    [Column(Field = "CloseDate", Title = "Maintenance Close Date", SearchName = "a.CloseDate", SortName = "a.CloseDate")]
    public DateTime CloseDate { get; set; }

    [Column(Field = "SealMeterCondition", SearchName = "a.SealMeterCondition", SortName = "a.SealMeterCondition")]
    public string SealMeterCondition { get; set; }
    [Column(Field = "BodyMeterCondition", SearchName = "a.BodyMeterCondition", SortName = "a.BodyMeterCondition")]
    public string BodyMeterCondition { get; set; }
    [Column(Field = "CoverMeterCondition", SearchName = "a.CoverMeterCondition", SortName = "a.CoverMeterCondition")]
    public string CoverMeterCondition { get; set; }
    [Column(Field = "GlassMeterCondition", SearchName = "a.GlassMeterCondition", SortName = "a.GlassMeterCondition")]
    public string GlassMeterCondition { get; set; }
    [Column(Field = "MachineMeterCondition", SearchName = "a.MachineMeterCondition", SortName = "a.MachineMeterCondition")]
    public string MachineMeterCondition { get; set; }

    [Column(Field = "ActualAchievement")]
    public string ActualAchievement { get; set; }
    [Column(Field = "Achievement", SearchName = "a.Achievement", SortName = "a.Achievement")]
    public string Achievement { get; set; }
    [Column(Field = "Used")]
    public bool Used { get; set; }
    [Column(Field = "Active")]
    public bool Active { get; set; }
    #endregion

    #region Default    
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
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "StockOutId")]
    public int StockOutId { get; set; }
    [Column(Field = "ScheduleId")]
    public int ScheduleId { get; set; }
    [Column(Field = "ScheduleMaintenanceId")]
    public int ScheduleMaintenanceId { get; set; }
    [Column(Field = "HelpdeskId")]
    public int HelpdeskId { get; set; }
    [Column(Field = "AreaId")]
    public int AreaId { get; set; }
    [Column(Field = "LocationId")]
    public int LocationId { get; set; }
    [Column(Field = "VendorId")]
    public int VendorId { get; set; }
    [Column(Field = "OperatorsId")]
    public int OperatorsId { get; set; }
    [Column(Field = "OperatorUserId")]
    public int OperatorUserId { get; set; }
    [Column(Field = "CustomerId")]
    public int CustomerId { get; set; }
    #endregion

    #region Additional Fields
    [Column(Field = "Subject", SearchName = "Subject", SortName = "Subject")]
    public string Subject { get; set; }
    [Column(Field = "UseItem", SearchName = "UseItem", SortName = "UseItem")]
    public string UseItem { get; set; }
    [Column(Field = "WorkDescription", Title = "Work Description", SearchName = "a.WorkDescription", SortName = "a.WorkDescription")]
    public string WorkDescription { get; set; }
    [Column(Field = "Area", SearchName = "Area", SortName = "Area")]
    public string Area { get; set; }
    [Column(Field = "Location", SearchName = "Location", SortName = "Location")]
    public string Location { get; set; }
    [Column(Field = "Category", Title = "Category", SearchName = "Category", SortName = "Category")]
    public string Category { get; set; }
    [Column(Field = "ScheduleStartDate", Title = "Start Date Schedule", SearchName = "ScheduleStartDate", SortName = "ScheduleStartDate")]
    public DateTime ScheduleStartDate { get; set; }
    [Column(Field = "WorkDuration", Required = true, SearchName = "WorkDuration", SortName = "WorkDuration")]
    public int WorkDuration { get; set; }
    [Column(Field = "WorkDurationType", Required = true, SearchName = "WorkDurationType", SortName = "WorkDurationType")]
    public string WorkDurationType { get; set; }
    [Column(Field = "TargetCompletion", Required = true, SearchName = "TargetCompletion", SortName = "TargetCompletion")]
    public DateTime TargetCompletion { get; set; }
    [Column(Field = "Period", SearchName = "Period", SortName = "Period")]
    public int Period { get; set; }
    [Column(Field = "PeriodType", Title = "Period Type", SearchName = "PeriodType", SortName = "PeriodType")]
    public string PeriodType { get; set; }
    [Column(Field = "ScheduleEndDate", Title = "End Date Schedule", SearchName = "ScheduleEndDate", SortName = "ScheduleEndDate")]
    public DateTime ScheduleEndDate { get; set; }

    private string _conductedBy;
    [Column(Field = "ConductedBy")]
    public string ConductedBy
    {
        get { return OperatorsId == 0 ? OperatorUserName : Operators; }
        set { _conductedBy = value; }
    }
    [Column(Field = "SubjectId")]
    public int SubjectId { get; set; }
    [Column(Field = "ParentCode")]
    public string ParentCode { get; set; }
    public List<object> Items { get; set; } = new List<object>();
    #endregion
    #endregion

    #region Methods
    #region Get Data
    public static WorkOrder GetById(object Id)
    {
        return (WorkOrder)DataAccess.GetSingleRowBySP(0, "WorkOrderGetById", new List<clsParameter> { new clsParameter("Id", Id) }, new WorkOrder());
    }
    public static WorkOrder GetByHelpdeskId(object HelpdeskId)
    {
        return (WorkOrder)DataAccess.GetSingleRowByQuery(0, $"Select * from WorkOrder where HelpdeskId = '{HelpdeskId}'", new WorkOrder());
    }
    public static List<object> GetByHelpdesk(object HelpdeskId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from WorkOrder where HelpdeskId = '{HelpdeskId}'", new WorkOrder());
    }    
    public static List<object> GetByScheduleMaintenanceId(object ScheduleMaintenanceId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from WorkOrder where ScheduleMaintenanceId = '{ScheduleMaintenanceId}'", new WorkOrder());
    }    
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from WorkOrder", new WorkOrder());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "WorkOrderGetByCriteria", pList, out TotalRow, new WorkOrder());
    }
    public static List<object> LookupGetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "WorkOrderLookupGetByCriteria", pList, out TotalRow, new WorkOrder());
    }
    public static List<object> GetByStockOutId(object StockOutId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from WorkOrder where StockOutId = '{StockOutId}'", new WorkOrder());
    }
    public static int GetTotalWorkOrderUsedByHelpdeskId(object HelpdeskId)
    {
        return (int)DataAccess.GetSingleValueByQuery(0, $"select COUNT(Id) from WorkOrder where HelpdeskId = '{HelpdeskId}' and Used = 1");
    }
    public static string GetStatusByHeldesk(object HelpdeskId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"SELECT CASE WHEN COUNT(*) = SUM(CASE WHEN status = 'Completed' THEN 1 ELSE 0 END) THEN 'Completed' ELSE 'OnGoing' END AS Status FROM WorkOrder where HelpdeskId = '{HelpdeskId}'").ToText();
    }
    public static bool IsStockOut(object WorkOrderId)
    {
        return DataAccess.IsDataExist(0, $"Select Id from WorkOrder where ISNULL(StockOutId, 0) <> 0 and Id = '{WorkOrderId}'").ToBool();
    }
    #endregion
    #region Change Data
    public string InsertByScheduler()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("ScheduleMaintenanceId", ScheduleMaintenanceId));
        pList.Add(new clsParameter("ScheduleId", ScheduleId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("OrderDate", OrderDate));
        pList.Add(new clsParameter("EarliestStartDate", EarliestStartDate));
        pList.Add(new clsParameter("LatestStartDate", LatestStartDate));
        pList.Add(new clsParameter("OperatorType", OperatorType.ToTextDB()));
        pList.Add(new clsParameter("VendorId", VendorId.ToIntDB()));
        pList.Add(new clsParameter("OperatorsId", OperatorsId.ToIntDB()));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId.ToIntDB()));
        pList.Add(new clsParameter("Status", Status));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "SchedulerWorkOrderInsert", pList);
    }
    public string InsertByHelpdesk()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("HelpdeskId", HelpdeskId));
        pList.Add(new clsParameter("AreaId", AreaId));
        pList.Add(new clsParameter("LocationId", LocationId));
        pList.Add(new clsParameter("OperatorType", OperatorType.ToTextDB()));
        pList.Add(new clsParameter("VendorId", VendorId.ToIntDB()));
        pList.Add(new clsParameter("OperatorsId", OperatorsId.ToIntDB()));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId.ToIntDB()));
        pList.Add(new clsParameter("CustomerId", CustomerId.ToIntDB()));
        pList.Add(new clsParameter("Status", Status));
        if (WorkDuration == 0)
        {
            pList.Add(new clsParameter("WorkDuration", DBNull.Value));
            pList.Add(new clsParameter("WorkDurationType", DBNull.Value));            
        }
        else
        {
            pList.Add(new clsParameter("WorkDuration", WorkDuration));
            pList.Add(new clsParameter("WorkDurationType", WorkDurationType));            
        }        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "HelpdeskWorkOrderInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("OperatorType", OperatorType));
        pList.Add(new clsParameter("VendorId", VendorId));
        pList.Add(new clsParameter("OperatorsId", OperatorsId));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId));
        if (StartDate == DateTime.MinValue) pList.Add(new clsParameter("StartDate", DBNull.Value));
        else pList.Add(new clsParameter("StartDate", StartDate));
        pList.Add(new clsParameter("WorkDuration", WorkDuration));
        pList.Add(new clsParameter("WorkDurationType", WorkDurationType));
        if (TargetCompletion == DateTime.MinValue) pList.Add(new clsParameter("TargetCompletion", DBNull.Value));
        else pList.Add(new clsParameter("TargetCompletion", TargetCompletion));
        pList.Add(new clsParameter("Status", Status));
        pList.Add(new clsParameter("Result", Result));
        pList.Add(new clsParameter("Remarks", Remarks));
        if (CloseDate == DateTime.MinValue) pList.Add(new clsParameter("CloseDate", DBNull.Value));
        else pList.Add(new clsParameter("CloseDate", CloseDate.ToSQLDateTime()));
        pList.Add(new clsParameter("SealMeterCondition", SealMeterCondition));
        pList.Add(new clsParameter("BodyMeterCondition", BodyMeterCondition));
        pList.Add(new clsParameter("CoverMeterCondition", CoverMeterCondition));
        pList.Add(new clsParameter("GlassMeterCondition", GlassMeterCondition));
        pList.Add(new clsParameter("MachineMeterCondition", MachineMeterCondition));
        pList.Add(new clsParameter("ActualAchievement", ActualAchievement));
        pList.Add(new clsParameter("Achievement", Achievement));
        pList.Add(new clsParameter("Used", Used));
        pList.Add(new clsParameter("Active", Active));        
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "WorkOrderUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "WorkOrder");
    }
    public static string Delete(object ScheduleId)
    {
        return DataAccess.Delete(0, ScheduleId, "ScheduleId", "WorkOrder");
    }
    public static string UpdateStockOutId(object StockOutId, object WorkOrderId, string Status, int Used = 1)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update WorkOrder Set StockOutId = '{StockOutId}', Status = '{Status}', Used = {Used}, ModifiedBy = '{U.GetUsername()}', Modified = GETDATE() where Id = '{WorkOrderId}' ");
    }
    public static string SaveAchievement(string Achievement)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update WorkOrder Set Achievement = '{Achievement}', ModifiedBy = '{U.GetUsername()}', Modified = GETDATE() where Id = {U.Id}");
    }
    public static string UpdateStatus(object Id, string Status)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update WorkOrder Set Status = '{Status}' where Id = '{Id}' ");
    }
    #endregion
    #endregion
}