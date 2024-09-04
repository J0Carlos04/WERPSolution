using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Microsoft.Exchange.WebServices.Data;

public class HelpdeskCustomer : BaseModel
{
    #region Properties
    [Column(Field = "Seq")]
    public int Seq { get; set; }
    [Column(Field = "HelpdeskId")]
    public int HelpdeskId { get; set; }
    [Column(Field = "CustomerId")]
    public int CustomerId { get; set; }
    #endregion

    #region Methods
    public static List<object> GetByHelpdesk(object HelpdeskId)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from HelpdeskCustomer where HelpdeskId = '{HelpdeskId}'", new HelpdeskCustomer());
    }
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("HelpdeskId", HelpdeskId));
        pList.Add(new clsParameter("CustomerId", CustomerId));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "HelpdeskCustomerInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("HelpdeskId", HelpdeskId));
        pList.Add(new clsParameter("CustomerId", CustomerId));
        pList.Add(new clsParameter("Author", CreatedBy));
        return DataAccess.Save(0, "HelpdeskCustomerUpdate", pList);
    }
    public string Delete()
    {
        return DataAccess.Delete(0, Id, "Id", "HelpdeskCustomer");
    }
    public static string DeleteByHelpdeskId(object HelpdeskId)
    {
        return DataAccess.Delete(0, HelpdeskId, "HelpdeskId", "HelpdeskCustomer");
    }
    #endregion
}