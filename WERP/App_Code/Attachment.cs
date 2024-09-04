using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class Attachment
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
    #endregion

    #region Key
    [Column(Field = "OwnerId")]
    public int OwnerId { get; set; }
    #endregion

    #region Fields    
    [Column(Field = "Seq")]
    public int Seq { get; set; }
    [Column(Field = "FileName")]
    public string FileName { get; set; }    
    [Column(Field = "Data")]
    public Byte[] Data { get; set; }
    #endregion

    #region Additional 
    [Column(Field = "Latitude")]
    public decimal Latitude { get; set; }
    [Column(Field = "Longitude")]
    public decimal Longitude { get; set; }
    public string Table { get; set; }
    public string FileNameUniq { get; set; }
    public string Mode { get; set; }
    #endregion
    #endregion

    #region Methods

    #region Get Data
    public static Attachment GetByID(string Table, object ID)
    {
        return (Attachment)DataAccess.GetSingleRowByQuery(0, string.Format("Select * from {0} where ID = '{1}'", Table, ID), new Attachment());
    }
    public static List<object> GetByOwnerID(string Table, object OwnerID)
    {
        return DataAccess.GetDataByQuery(0, string.Format("select * from {0} where OwnerID = {1}", Table, OwnerID), new Attachment());
    }
    #endregion

    #region Change Data
    public string Insert(Attachment o)
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("OwnerID", o.OwnerId));
        pList.Add(new clsParameter("Seq", o.Seq));
        pList.Add(new clsParameter("FileName", o.FileName));        
        pList.Add(new clsParameter("Data", o.Data));
        if (o.Table == "WorkOrderWorkUpdateAttachment")
        {
            pList.Add(new clsParameter("Latitude", o.Latitude));
            pList.Add(new clsParameter("Longitude", o.Longitude));
        }
        pList.Add(new clsParameter("Author", o.CreatedBy));
        return DataAccess.Save(0, string.Format("{0}Insert", o.Table), pList);
    }
    public static string Delete(object ID, string Table)
    {
        return DataAccess.Delete(0, ID, "ID", Table);
    }
    public static string DeleteByOwnerId(object OwnerID, string Table)
    {
        return DataAccess.Delete(0, OwnerID, "OwnerID", Table);
    }
    #endregion

    #endregion
}