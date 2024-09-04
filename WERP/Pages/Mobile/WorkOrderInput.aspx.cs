using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;

public partial class Pages_Mobile_WorkOrderInput : System.Web.UI.Page
{
    #region Fields
    private const string SMTableName = "ScheduleMaintenanceAttachment";
    private const string HTableName = "HelpdeskAttachment";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnDownload":
                U.DownloadFile(btn, "lb_FileName");
                break;
        }
    }
    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_Seq = (Literal)e.Row.FindControl("ltrl_Seq");
            ltrl_Seq.Text = (e.Row.RowIndex + 1).ToString();

            Button btnDownload = (Button)e.Row.FindControl("btnDownload");
            LinkButton lb_FileName = (LinkButton)e.Row.FindControl("lb_FileName");
            lb_FileName.Attributes.Add("onclick", string.Format("ClientChanged('{0}');", btnDownload.ClientID));
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
        ViewState[CNT.VS.Username] = U.GetUsername();
        if ($"{ViewState[CNT.VS.Username]}" == "") Response.Redirect(@"~\Pages\default.aspx");
    }
    private void SetInitEdit()
    {
        WorkOrder wo = WorkOrder.GetById(U.Id);
        tbArea.Value = wo.Area;
        tbLocation.Value = wo.Location;
        tbCategory.Value = wo.Category;
        tbSubject.Value = wo.Subject;
        tbWorkDescription.Value = wo.WorkDescription;

        int ParentId = 0;
        string TableName = "";
        if (wo.HelpdeskId == 0)
        {
            //U.Display("dvScheduler");
            //lblParentTitle.Text = "Maintenance Schedule";
            lblTitleWorkDescription.InnerText = "Work Description";
            Schedule s = Schedule.GetById(wo.ScheduleId);
            s.OrderDate = wo.OrderDate;
            s.EarliestStartDate = wo.EarliestStartDate;
            s.LatestStartDate = wo.LatestStartDate;
            List<object> sList = new List<object> { s };
            U.BindGrid(gvSchedule, sList);
            TableName = SMTableName;
            ParentId = wo.ScheduleMaintenanceId;
        }
        else
        {
            //U.Hide("dvScheduler");
            //lblParentTitle.Text = "Helpdesk";
            lblTitleWorkDescription.InnerText = "Request Detail";
            TableName = HTableName;
            ParentId = wo.HelpdeskId;
        }

        #region Helpdesk / Schedule Maintenance Attachment
        List<object> aList = Attachment.GetByOwnerID(TableName, ParentId);
        if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
        foreach (Attachment att in aList)
            U.SaveAttachmentToTempFolder(att);
        U.BindGrid(gvScheduleAttachment, aList);
        #endregion

        #region Operator
        pnlVendor.Visible = false;
        if (wo.VendorId != 0)
        {
            pnlVendor.Visible = true;
            tbVendor.Value = wo.Vendor;
            tbOperator.Value = wo.Operators;
        }
        else
        {
            Users u = Users.GetById(wo.OperatorUserId);
            tbOperator.Value = u.Name;
        }
            
        #endregion

        tbCode.Value = wo.Code;
    }
    #endregion




    
}