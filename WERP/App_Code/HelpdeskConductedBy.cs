using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class HelpdeskConductedBy : BaseModel
{
    #region Properties 
    [Column(Field = "Seq")]
    public int Seq { get; set; }
    [Column(Field = "HelpdeskId")]
    public int HelpdeskId { get; set; }
    [Column(Field = "OperatorType")]
    public string OperatorType { get; set; }
    [Column(Field = "VendorId")]
    public int VendorId { get; set; }
    [Column(Field = "OperatorsId")]
    public int OperatorsId { get; set; }
    [Column(Field = "OperatorUserId")]
    public int OperatorUserId { get; set; }
    #endregion

    #region Methods
    public static List<object> GetByHelpdesk(object HelpdeskId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from HelpdeskConductedBy where HelpdeskId = '{HelpdeskId}'", new HelpdeskConductedBy());
    }
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("HelpdeskId", HelpdeskId));
        pList.Add(new clsParameter("OperatorType", OperatorType));
        pList.Add(new clsParameter("VendorId", VendorId));
        pList.Add(new clsParameter("OperatorsId", OperatorsId));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "HelpdeskConductedByInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("HelpdeskId", HelpdeskId));
        pList.Add(new clsParameter("OperatorType", OperatorType));
        pList.Add(new clsParameter("VendorId", VendorId));
        pList.Add(new clsParameter("OperatorsId", OperatorsId));
        pList.Add(new clsParameter("OperatorUserId", OperatorUserId));
        pList.Add(new clsParameter("Author", ModifiedBy));
        return DataAccess.Save(0, "HelpdeskConductedByUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "HelpdeskConductedBy");
    }
    #endregion
}