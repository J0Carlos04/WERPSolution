using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using U = Utility;

public partial class Pages_Mobile_WorkOrder : System.Web.UI.Page
{
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnSelect":                
                Response.Redirect($@"~\Pages\Mobile\WorkOrderInput.aspx?Id={((Literal)btn.FindControl("ltrl_Id")).Text}");
                break;
            case "btnItem":
                Response.Redirect($@"~\Pages\Mobile\WorkOrderItem.aspx?Id={((Literal)btn.FindControl("ltrl_Id")).Text}");
                break;
            case "btnWorkUpdate":
                Response.Redirect($@"~\Pages\Mobile\WorkOrderWorkUpdate.aspx?Id={((Literal)btn.FindControl("ltrl_Id")).Text}");
                break;
        }
    }
    protected void lb_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        switch (lb.ID)
        {
            case "lb_Subject":
                Response.Redirect($@"~\Pages\Mobile\WorkOrderInput.aspx?Id={((Literal)lb.FindControl("ltrl_Id")).Text}");
                break;
        }
    }
    protected void gvWorkOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            WorkOrder wo = (WorkOrder)e.Row.DataItem;
            Button btnItem = (Button)e.Row.FindControl("btnItem");
            Button btnWorkUpdate = (Button)e.Row.FindControl("btnWorkUpdate");
            Button btnAttachment = (Button)e.Row.FindControl("btnAttachment");
            HtmlGenericControl dvFooter = (HtmlGenericControl)e.Row.FindControl("dvFooter");
            btnItem.Visible = false;
            btnWorkUpdate.Visible = false;
            btnAttachment.Visible = false;            

            if (wo.Status.Is(CNT.Status.Preparation)) 
            {
                if (wo.UseItem.Is("Yes") || wo.UseItem.Is("Both")) btnItem.Visible = true;
                else btnWorkUpdate.Visible = true;
            }
            if (wo.Status.Is(CNT.Status.ItemReceived) || wo.Status.Is(CNT.Status.Started) || wo.Status.Is(CNT.Status.Inprogress))
            {
                btnWorkUpdate.Visible = true;
                btnAttachment.Visible = true;
            }
            dvFooter.Visible = btnItem.Visible || btnWorkUpdate.Visible || btnAttachment.Visible;
        }
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInit();
        double StartNo = ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo];
        double EndNo = 0;
        double TotalRow = 0;

        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy], "", true, U.ViewAllDataAccess(ViewState[CNT.VS.PageName]));
        pList.Add(new clsParameter("StartNo", StartNo));
        pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = WorkOrder.GetByCriteria(pList, out TotalRow);
                
        U.BindGrid(gvWorkOrder, oList);
    }
    private void SetInit()
    {
        ViewState[CNT.VS.Username] = U.GetUsername();
        if ($"{ViewState[CNT.VS.Username]}" == "") Response.Redirect(@"~\Pages\default.aspx");
    }
    #endregion






    
}