using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class StockOrder
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
    [Column(Field = "RequesterUserId")]
    public int RequesterUserId { get; set; }
    [Column(Field = "POId")]
    public int POId { get; set; }
    [Column(Field = "VendorId")]
    public int VendorId { get; set; }
    [Column(Field = "ApproverUserId")]
    public int ApproverUserId { get; set; }        
    #endregion

    #region Fields
    [Column(Field = "Code", SearchName = "Code", SortName = "Code")]
    public string Code { get; set; }
    [Column(Field = "Description", SearchName = "Description", SortName = "Description")]
    public string Description { get; set; }
    [Column(Field = "Requester", Title = "Requester", SearchName = "Requester", SortName = "Requester")]
    public string Requester { get; set; }
    [Column(Field = "ProcurementType", SearchName = "ProcurementType", SortName = "ProcurementType")]
    public string ProcurementType { get; set; }
    [Column(Field = "PONo", SearchName = "PONo", SortName = "PONo")]
    public string PONo { get; set; }
    [Column(Field = "PODate", SearchName = "PODate", SortName = "PODate")]
    public DateTime PODate { get; set; }
    [Column(Field = "Vendor", SearchName = "Vendor", SortName = "Vendor")]
    public string Vendor { get; set; }    
    [Column(Field = "Approver", Title = "Approver", SearchName = "Approver", SortName = "Approver")]
    public string Approver { get; set; }
    
    [Column(Field = "Status", SearchName = "Status", SortName = "Status")]
    public string Status { get; set; }
    [Column(Field = "StatusDate", SearchName = "StatusDate", SortName = "StatusDate")]
    public DateTime StatusDate { get; set; }
    #endregion

    #region Additional
    public List<object> Items { get; set; }
    [Column(Field = "ItemCode", Title = "Item Code", SearchName = "f.Code")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName", Title = "Item Name", SearchName = "f.Name")]
    public string ItemName { get; set; }
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static StockOrder GetById(object Id)
    {
        return (StockOrder)DataAccess.GetSingleRowBySP(0, "StockOrderGetById", new List<clsParameter> { new clsParameter("Id", Id) }, new StockOrder());
    }
    public static StockOrder GetByKey(object Code, object Id)
    {
        return (StockOrder)DataAccess.GetSingleRowByQuery(0, $"Select * from StockOrder where Code = '{Code}' AND Id <> '{Id}'", new StockOrder());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from StockOrder", new StockOrder());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockOrderGetByCriteria", pList, out TotalRow, new StockOrder());
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();       
        pList.Add(new clsParameter("RequesterUserId", RequesterUserId));
        pList.Add(new clsParameter("POId", POId));
        pList.Add(new clsParameter("VendorId", VendorId));
        pList.Add(new clsParameter("ApproverUserId", ApproverUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Description", Description));
        pList.Add(new clsParameter("ProcurementType", ProcurementType));
        pList.Add(new clsParameter("Status", Status));

        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockOrderInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("RequesterUserId", RequesterUserId));
        pList.Add(new clsParameter("POId", POId));
        pList.Add(new clsParameter("VendorId", VendorId));
        pList.Add(new clsParameter("ApproverUserId", ApproverUserId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Description", Description));
        pList.Add(new clsParameter("ProcurementType", ProcurementType));
        pList.Add(new clsParameter("Status", Status));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockOrderUpdate", pList);
    }
    public string UpdateApproval()
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Update StockOrder Set Status = '{Status}', StatusDate = GETDATE() Where Id = '{Id}' ");
    }
    public static string Delete(object Id)
    {
        return DataAccess.ExecNonReturnValueBySP(0, "StockOrderDelete", new List<clsParameter> { new clsParameter("StockOrderId", Id) });
    }
    #endregion

    #endregion
}