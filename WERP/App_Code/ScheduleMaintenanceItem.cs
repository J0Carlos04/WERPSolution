using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Microsoft.Exchange.WebServices.Data;
using System.Drawing;
using System.Xml.Linq;

public class ScheduleMaintenanceItem
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
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }
    #endregion

    #region Fields
    [Column(Field = "Seq", SearchName = "a.Seq", SortName = "a.Seq")]
    public int Seq { get; set; }
    [Column(Field = "ItemCode", Title = "Item Code", SearchName = "b.Code", SortName = "b.Code")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName", Title = "Item Name", SearchName = "b.Name", SortName = "b.Name")]
    public string ItemName { get; set; }    
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }
    #endregion

    #region Additional

    #endregion
    #endregion

    #region Methods
    #region Get Data
    public static List<object> GetByScheduleMaintenanceId(object ScheduleMaintenanceId)
    {
        return DataAccess.GetDataByQuery(0, $"Select a.*, b.Code ItemCode, b.Name ItemName from ScheduleMaintenanceItem a Left Join Item b On b.Id = a.ItemId where a.ScheduleMaintenanceId = '{ScheduleMaintenanceId}'", new ScheduleMaintenanceItem());
    }
    #endregion
    #region Changed Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("ScheduleMaintenanceId", ScheduleMaintenanceId));
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));        
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ScheduleMaintenanceItemInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("ScheduleMaintenanceId", ScheduleMaintenanceId));
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "ScheduleMaintenanceItemUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "ScheduleMaintenanceItem");
    }
    #endregion
    #endregion
}