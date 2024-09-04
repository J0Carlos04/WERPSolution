using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.IO;
using System.Web.Services.Description;
using DAL;

public partial class Pages_Inventory_StockOrderDetail : System.Web.UI.Page
{    
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) LoadFirstTime();        
    }        

    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }        
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInit();        
        SetInitEdit();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        ViewState[CNT.VS.PageName] = "StockOrder";
        if (!U.CreateAccess("StockOrder")) Response.Redirect($"../{CNT.Unauthorized}.aspx");        
    }    
    private void SetInitEdit()
    {        
        StockOrder o = StockOrder.GetById(U.Id);
        tbCode.Value = o.Code;
        tbDescription.Value = o.Description;
        tbProcurementType.Value = o.ProcurementType;
        pnlPO.Visible = false;
        if (o.ProcurementType == "Regular")
        {
            PO p = PO.GetById(o.POId);            
            tbPO.Value = p.PONo;
            pnlPO.Visible = true;
        }
        tbVendor.Value = $"{o.Vendor}";
        tbRequester.Value = $"{o.Requester}";
        tbApprover.Value = $"{o.Approver}";
        tbStatus.Value = o.Status;        

        List<object> siList = StockOrderItem.GetByParendId(o.Id);
        U.BindGrid(gvItems, siList);
        if (o.Status == "Rejected") gvItems.Columns[2].Visible = true;
    }    
    #endregion 
}