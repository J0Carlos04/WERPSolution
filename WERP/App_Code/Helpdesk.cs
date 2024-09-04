using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DAL;
using Microsoft.Exchange.WebServices.Data;

public class Helpdesk
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
    [Column(Field = "WorkOrderTypeId")]
    public int WorkOrderTypeId { get; set; }
    [Column(Field = "HelpdeskCategoryId")]
    public int HelpdeskCategoryId { get; set; }
    [Column(Field = "SubjectId")]
    public int SubjectId { get; set; }
    [Column(Field = "AreaId")]
    public int AreaId { get; set; }
    [Column(Field = "LocationId")]
    public int LocationId { get; set; }
    [Column(Field = "RequestTypeId")]
    public int RequestTypeId { get; set; }
    [Column(Field = "RequestSourceId")]
    public int RequestSourceId { get; set; }    
    [Column(Field = "SocialMediaId")]
    public int SocialMediaId { get; set; }    
    [Column(Field = "CustomerId")]
    public int CustomerId { get; set; }
    [Column(Field = "VendorId")]
    public int VendorId { get; set; }
    [Column(Field = "OperatorsId")]
    public int OperatorsId { get; set; }
    [Column(Field = "OperatorUserId")]
    public int OperatorUserId { get; set; }
    #endregion

    #region Fields    
    [Column(Field = "Code", Required = true, SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "WorkOrderType", Required = true, SearchName = "h.Name", SortName = "h.Name")]
    public string WorkOrderType { get; set; }
    [Column(Field = "HelpdeskCategory", Required = true, SearchName = "g.Name", SortName = "g.Name")]
    public string HelpdeskCategory { get; set; }
    [Column(Field = "Subject", Required = true, SearchName = "f.Name", SortName = "f.Name")]
    public string Subject { get; set; }
    [Column(Field = "Area", Required = true, SearchName = "d.Name", SortName = "d.Name")]
    public string Area { get; set; }
    [Column(Field = "Location", Required = true, SearchName = "e.Name", SortName = "e.Name")]
    public string Location { get; set; }
    [Column(Field = "OperatorType", Required = true, SearchName = "a.OperatorType", SortName = "a.OperatorType")]
    public string OperatorType { get; set; }
    [Column(Field = "Vendor", SearchName = "Vendor", SortName = "Vendor")]
    public string Vendor { get; set; }
    [Column(Field = "Operators", Title = "Operator", SearchName = "Operators", SortName = "Operators")]
    public string Operators { get; set; }
    [Column(Field = "OperatorUserName", Title = "User Name", SearchName = "OperatorUserName", SortName = "OperatorUserName")]
    public string OperatorUserName { get; set; }
    [Column(Field = "RequestType", Required = true, SearchName = "c.Name", SortName = "c.Name")]
    public string RequestType { get; set; }
    [Column(Field = "RequestSource", Required = true, SearchName = "b.Name", SortName = "b.Name")]
    public string RequestSource { get; set; }
    [Column(Field = "RequesterName", Required = true, SearchName = "RequesterName", SortName = "RequesterName")]
    public string RequesterName { get; set; }
    [Column(Field = "RequesterEmail", Required = true, SearchName = "RequesterEmail", SortName = "RequesterEmail")]
    public string RequesterEmail { get; set; }
    [Column(Field = "RequesterPhone", Required = true, SearchName = "RequesterPhone", SortName = "RequesterPhone")]
    public string RequesterPhone { get; set; }
    [Column(Field = "SocialMedia", Required = true, SearchName = "i.Name", SortName = "i.Name")]
    public string SocialMedia { get; set; }

    [Column(Field = "RequestDetail", Required = true, SearchName = "RequestDetail", SortName = "RequestDetail")]
    public string RequestDetail { get; set; }
    [Column(Field = "Requested", Required = true, SearchName = "Requested", SortName = "Requested")]
    public DateTime Requested { get; set; }     
            
    [Column(Field = "User", Required = true, SearchName = "User", SortName = "User")]
    public string User { get; set; }

    [Column(Field = "Response", Required = true, SearchName = "Response", SortName = "Response")]
    public DateTime Response { get; set; }
    [Column(Field = "ResponseStatus", Required = true, SearchName = "ResponseStatus", SortName = "ResponseStatus")]
    public string ResponseStatus { get; set; }
    [Column(Field = "Status", Required = true, SearchName = "Status", SortName = "Status")]
    public string Status { get; set; }        
    [Column(Field = "Completion", Required = true, SearchName = "Completion", SortName = "Completion")]
    public DateTime Completion { get; set; }
    [Column(Field = "TargetCompletionStatus", Required = true, SearchName = "TargetCompletionStatus", SortName = "TargetCompletionStatus")]
    public string TargetCompletionStatus { get; set; }
    
    [Column(Field = "CustNo", Required = true, SearchName = "j.CustNo", SortName = "j.CustNo")]
    public string CustNo { get; set; }
    [Column(Field = "CustName", Title = "Cust Name", Required = true, SearchName = "j.Name", SortName = "j.Name")]
    public string CustName { get; set; }
    [Column(Field = "CustAddress", Title = "Cust Address", Required = true, SearchName = "j.Address", SortName = "j.Address")]
    public string CustAddress { get; set; }
    [Column(Field = "CustEachOperator")]
    public int CustEachOperator { get; set; }
    #endregion

    #region Additional 
    public List<object> Customers { get; set; } = new List<object>();
    public List<object> Items { get; set; } = new List<object>();
    #endregion
    #endregion

    #region Methods

    #region GetData
    public static Helpdesk GetById(object Id)
    {
        return (Helpdesk)DataAccess.GetSingleRowBySP(0, "HelpdeskGetById", new List<clsParameter> { new clsParameter("Id", Id) }, new Helpdesk());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "HelpdeskGetByCriteria", pList, out TotalRow, new Helpdesk());
    }
    #endregion

    #region ChangeData
    public string Insert()
    {                        
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("WorkOrderTypeId", WorkOrderTypeId));
        pList.Add(new clsParameter("HelpdeskCategoryId", HelpdeskCategoryId));
        pList.Add(new clsParameter("SubjectId", SubjectId));
        pList.Add(new clsParameter("AreaId", AreaId));
        pList.Add(new clsParameter("LocationId", LocationId));
        pList.Add(new clsParameter("RequestTypeId", RequestTypeId));
        pList.Add(new clsParameter("RequestSourceId", RequestSourceId));        
        pList.Add(new clsParameter("SocialMediaId", SocialMediaId));        
        pList.Add(new clsParameter("CustomerId", CustomerId));
        pList.Add(new clsParameter("OperatorType", OperatorType.ToTextDB()));
        pList.Add(new clsParameter("VendorId", VendorId.ToIntDB()));
        pList.Add(new clsParameter("OperatorsId", OperatorsId.ToIntDB()));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId.ToIntDB()));
        pList.Add(new clsParameter("RequesterName", RequesterName));
        pList.Add(new clsParameter("RequesterEmail", RequesterEmail));
        pList.Add(new clsParameter("RequesterPhone", RequesterPhone));
        pList.Add(new clsParameter("RequestDetail", RequestDetail));
        pList.Add(new clsParameter("Requested", Requested));        
        
        pList.Add(new clsParameter("Response", Response));
        pList.Add(new clsParameter("ResponseStatus", ResponseStatus));
        pList.Add(new clsParameter("Status", Status));
        if (Completion == DateTime.MinValue) pList.Add(new clsParameter("Completion", DBNull.Value));
        else pList.Add(new clsParameter("Completion", Completion));
        pList.Add(new clsParameter("TargetCompletionStatus", TargetCompletionStatus));

        pList.Add(new clsParameter("CustEachOperator", CustEachOperator));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "HelpdeskInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("WorkOrderTypeId", WorkOrderTypeId));
        pList.Add(new clsParameter("HelpdeskCategoryId", HelpdeskCategoryId));
        pList.Add(new clsParameter("SubjectId", SubjectId));
        pList.Add(new clsParameter("AreaId", AreaId));
        pList.Add(new clsParameter("LocationId", LocationId));
        pList.Add(new clsParameter("RequestTypeId", RequestTypeId));
        pList.Add(new clsParameter("RequestSourceId", RequestSourceId));
        pList.Add(new clsParameter("SocialMediaId", SocialMediaId));
        pList.Add(new clsParameter("CustomerId", CustomerId));
        pList.Add(new clsParameter("OperatorType", OperatorType.ToTextDB()));
        pList.Add(new clsParameter("VendorId", VendorId.ToIntDB()));
        pList.Add(new clsParameter("OperatorsId", OperatorsId.ToIntDB()));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId.ToIntDB()));
        pList.Add(new clsParameter("RequesterName", RequesterName));
        pList.Add(new clsParameter("RequesterEmail", RequesterEmail));
        pList.Add(new clsParameter("RequesterPhone", RequesterPhone));
        pList.Add(new clsParameter("RequestDetail", RequestDetail));
        pList.Add(new clsParameter("Requested", Requested));
        pList.Add(new clsParameter("Response", Response));
        pList.Add(new clsParameter("ResponseStatus", ResponseStatus));
        pList.Add(new clsParameter("Status", Status));
        if (Completion == DateTime.MinValue) pList.Add(new clsParameter("Completion", DBNull.Value));
        else pList.Add(new clsParameter("Completion", Completion));
        pList.Add(new clsParameter("TargetCompletionStatus", TargetCompletionStatus));

        pList.Add(new clsParameter("CustEachOperator", CustEachOperator));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "HelpdeskUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.ExecNonReturnValueBySP(0, "DeleteHelpdesk", new List<clsParameter> { new clsParameter("Id", Id) });
    }
    public string DeleteWorkOrder()
    {
        return DataAccess.ExecNonReturnValueBySP(0, "WorkOrderDeleteByHelpdesk", new List<clsParameter> { new clsParameter("Id", Id) });
    }
    #endregion

    #endregion
}