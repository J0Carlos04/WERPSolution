using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class ScheduleMaintenance
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
    [Column(Field = "SubjectId")]
    public int SubjectId { get; set; }
    [Column(Field = "AreaId")]
    public int AreaId { get; set; }
    [Column(Field = "LocationId")]
    public int LocationId { get; set; }
    [Column(Field = "WorkOrderCategoryId")]
    public int WorkOrderCategoryId { get; set; }
    [Column(Field = "VendorId")]
    public int VendorId { get; set; }
    [Column(Field = "OperatorsId")]
    public int OperatorsId { get; set; }
    [Column(Field = "OperatorUserId")]
    public int OperatorUserId { get; set; }
    #endregion

    #region Fields
    [Column(Field = "Code", SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "Subject", SearchName = "f.Name", SortName = "f.Name")]
    public string Subject { get; set; }    
    [Column(Field = "Area", SearchName = "c.Name", SortName = "c.Name")]
    public string Area { get; set; }
    [Column(Field = "Location", SearchName = "d.Name", SortName = "d.Name")]
    public string Location { get; set; }
    [Column(Field = "WorkOrderCategory", Title = "Work Order Category", SearchName = "e.Name", SortName = "e.Name")]
    public string WorkOrderCategory { get; set; }
    [Column(Field = "WorkDescription", SearchName = "a.WorkDescription", SortName = "a.WorkDescription")]
    public string WorkDescription { get; set; }
    [Column(Field = "OperatorType", Required = true, SearchName = "a.OperatorType", SortName = "a.OperatorType")]
    public string OperatorType { get; set; }

    [Column(Field = "StartDate", SearchName = "b.StartDate", SortName = "b.StartDate")]
    public DateTime StartDate { get; set; }
    [Column(Field = "Period", SearchName = "b.Period", SortName = "b.Period")]
    public int Period { get; set; }
    [Column(Field = "PeriodType", SearchName = "b.PeriodType", SortName = "b.PeriodType")]
    public string PeriodType { get; set; }    
    [Column(Field = "EndDate", SearchName = "b.EndDate", SortName = "b.EndDate")]
    public DateTime EndDate { get; set; } 
    
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static ScheduleMaintenance GetById(object Id)
    {
        return (ScheduleMaintenance)DataAccess.GetSingleRowByQuery(0, $"Select a.*, b.Name Location from ScheduleMaintenance a Left Join Location b On b.Id = a.LocationId where a.Id = '{Id}'", new ScheduleMaintenance());
    }    
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from ScheduleMaintenance", new ScheduleMaintenance());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "ScheduleMaintenanceGetByCriteria", pList, out TotalRow, new ScheduleMaintenance());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("AreaId", AreaId));
        pList.Add(new clsParameter("LocationId", LocationId));
        pList.Add(new clsParameter("WorkOrderCategoryId", WorkOrderCategoryId));
        pList.Add(new clsParameter("SubjectId", SubjectId));
        pList.Add(new clsParameter("WorkDescription", WorkDescription));
        pList.Add(new clsParameter("OperatorType", OperatorType.ToTextDB()));
        pList.Add(new clsParameter("VendorId", VendorId.ToIntDB()));
        pList.Add(new clsParameter("OperatorsId", OperatorsId.ToIntDB()));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId.ToIntDB()));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ScheduleMaintenanceInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("AreaId", AreaId));
        pList.Add(new clsParameter("LocationId", LocationId));
        pList.Add(new clsParameter("WorkOrderCategoryId", WorkOrderCategoryId));
        pList.Add(new clsParameter("SubjectId", SubjectId));
        pList.Add(new clsParameter("WorkDescription", WorkDescription));
        pList.Add(new clsParameter("OperatorType", OperatorType.ToTextDB()));
        pList.Add(new clsParameter("VendorId", VendorId.ToIntDB()));
        pList.Add(new clsParameter("OperatorsId", OperatorsId.ToIntDB()));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId.ToIntDB()));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ScheduleMaintenanceUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.ExecNonReturnValueBySP(0, "DeleteScheduleMaintenance", new List<clsParameter> { new clsParameter("Id", Id) });    
    }
    public string DeleteWorkOrder()
    {
        return DataAccess.ExecNonReturnValueBySP(0, "WorkOrderDeleteByScheduleMaintenance", new List<clsParameter> { new clsParameter("Id", Id) });
    }
    #endregion

    #endregion
}