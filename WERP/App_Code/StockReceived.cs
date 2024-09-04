using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class StockReceived
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
    [Column(Field = "StockOrderId")]
    public int StockOrderId { get; set; }
    [Column(Field = "ReceiverUserId")]
    public int ReceiverUserId { get; set; }
    #endregion

    #region Fields
    [Column(Field = "ReceivedDate")]
    public DateTime ReceivedDate { get; set; }
    [Column(Field = "InvoiceNo")]
    public string InvoiceNo { get; set; }
    [Column(Field = "InvoiceDate")]
    public DateTime InvoiceDate { get; set; }
    [Column(Field = "InvoiceFileName")]
    public string InvoiceFileName { get; set; }
    [Column(Field = "InvoiceFileData")]
    public byte[] InvoiceFileData { get; set; }
    [Column(Field = "BastFileName")]
    public string BastFileName { get; set; }
    [Column(Field = "BastFileData")]
    public byte[] BastFileData { get; set; }
    #endregion
    #endregion

    #region Methods
    #region Get Data
    public static StockReceived GetById(object Id)
    {
        return (StockReceived)DataAccess.GetSingleRowByQuery(0, $"Select * from StockReceived where Id = '{Id}'", new StockReceived());
    }
    public static bool IsReceivedExist(object StockOrderId)
    {
        int ReceivedCount = DataAccess.GetSingleValueByQuery(0, $"select Count(*) from StockReceived where StockOrderId = '{StockOrderId}'").ToInt();
        if (ReceivedCount == 0) return false;
        else return true;
    }
    public static bool IsReceivedItemInUsed(object Id)
    {
        return DataAccess.GetSingleValueByQuery(0, $"select top 1 1 from StockReceivedItem where StockReceivedId = '{Id}' and (Id In (Select StockReceivedItemId from StockReceivedRetur) or Id in (Select StockReceivedItemId from StockOutItem))").ToBool();
    }
    #endregion
    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("StockOrderId", StockOrderId));
        pList.Add(new clsParameter("ReceiverUserId", ReceiverUserId));
        pList.Add(new clsParameter("ReceivedDate", ReceivedDate.ToSQLDateTime()));
        pList.Add(new clsParameter("InvoiceNo", InvoiceNo));

        if (InvoiceDate == DateTime.MinValue || InvoiceDate.Date == new DateTime(1, 1, 1)) pList.Add(new clsParameter("InvoiceDate", DBNull.Value));
        else pList.Add(new clsParameter("InvoiceDate", InvoiceDate.ToSQLDate()));

        pList.Add(new clsParameter("InvoiceFileName", InvoiceFileName));
        pList.Add(new clsParameter { Name = "InvoiceFileData", Value = InvoiceFileData, IsVarBinary = true });
        pList.Add(new clsParameter("BastFileName", BastFileName));
        pList.Add(new clsParameter { Name = "BastFileData", Value = BastFileData, IsVarBinary = true });        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "StockReceivedInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("StockOrderId", StockOrderId));
        pList.Add(new clsParameter("ReceiverUserId", ReceiverUserId));
        pList.Add(new clsParameter("ReceivedDate", ReceivedDate.ToSQLDateTime()));
        pList.Add(new clsParameter("InvoiceNo", InvoiceNo));

        if (InvoiceDate == DateTime.MinValue || InvoiceDate.Date == new DateTime(1, 1, 1)) pList.Add(new clsParameter("InvoiceDate", DBNull.Value));
        else pList.Add(new clsParameter("InvoiceDate", InvoiceDate.ToSQLDate()));

        pList.Add(new clsParameter("InvoiceFileName", InvoiceFileName));
        pList.Add(new clsParameter { Name = "InvoiceFileData", Value = InvoiceFileData, IsVarBinary = true });
        pList.Add(new clsParameter("BastFileName", BastFileName));
        pList.Add(new clsParameter { Name = "BastFileData", Value = BastFileData, IsVarBinary = true });
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "StockReceivedUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "StockReceived");
    }
    #endregion
    #endregion
}