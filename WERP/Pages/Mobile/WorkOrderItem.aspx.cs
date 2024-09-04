using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;

public partial class Pages_Mobile_WorkOrderItem : System.Web.UI.Page
{
    #region Fields
    private const string SMTableName = "ScheduleMaintenanceAttachment";
    private const string HTableName = "HelpdeskAttachment";
    private const string vsRowIndex = "RowIndex";
    #endregion

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
            case "btnDownload":
                U.DownloadFile(btn, "lb_FileName");
                break;
            case "btnCodeLookup":
                pnlItems.Visible = false;
                pnlLookupItems.Visible = true;
                ViewState[vsRowIndex] = ((GridViewRow)btn.Parent.Parent).RowIndex;
                break;
            case "btnAddItem":
                AddItem();
                break;
            case "btnDeleteItem":
                DeleteItem();
                break;
        }
    }
    protected void rb_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton rb = (RadioButton)sender;        
        Literal ltrlId = (Literal)rb.FindControl("ltrl_Id");
        Literal ltrlCode = (Literal)rb.FindControl("ltrl_Code");
        Literal ltrlName = (Literal)rb.FindControl("ltrl_Name");

        GridViewRow Row = gvItems.Rows[ViewState[vsRowIndex].ToInt()];
        Literal ltrl_ItemId = (Literal)Row.FindControl("ltrl_ItemId");
        TextBox tb_ItemCode = (TextBox)Row.FindControl("tb_ItemCode");
        Literal ltrl_ItemName = (Literal)Row.FindControl("ltrl_ItemName");
        ltrl_ItemId.Text = ltrlId.Text;
        tb_ItemCode.Text = ltrlCode.Text;
        ltrl_ItemName.Text = ltrlName.Text;

        pnlItems.Visible = true;
        pnlLookupItems.Visible = false;
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
    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvItems.ClientID.Replace("_", "$")));
        }
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
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

        List<object> oList = Goods.GetByScrolling(new List<clsParameter> { new clsParameter("PageIndex", 1), new clsParameter("PageSize", "10") });
        if (oList.Count > 0) oList.Add(new Goods { Code = CNT.DataNotAvailable });
        U.BindGrid(gvData, oList);
    }
    private void SetInitEdit()
    {
        WorkOrder wo = WorkOrder.GetById(U.Id);
        tbCode.Value = wo.Code;
        tbArea.Value = wo.Area;
        tbLocation.Value = wo.Location;
        tbCategory.Value = wo.Category;
        tbSubject.Value = wo.Subject;
        tbWorkDescription.Value = wo.WorkDescription;

        int ParentId = 0;
        string TableName = "";
        if (wo.HelpdeskId == 0)
        {
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
            lblTitleWorkDescription.InnerText = "Request Detail";
            gvSchedule.Visible = false;
            TableName = HTableName;
            ParentId = wo.HelpdeskId;
        }

        List<object> aList = Attachment.GetByOwnerID(TableName, ParentId);
        if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
        foreach (Attachment att in aList)
            U.SaveAttachmentToTempFolder(att);
        U.BindGrid(gvScheduleAttachment, aList);

        #region Work Order Item        
        List<object> woItemList = WorkOrderItem.GetByWorkOrderId(wo.Id);
        if (woItemList.Count == 0) woItemList.Add(new WorkOrderItem { });        
        U.BindGrid(gvItems, woItemList);
        #endregion
    }

    #region Items
    private void AddItem()
    {
        Wrapping w = new Wrapping();
        ItemValidation(w);
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}", btnAddItem);
            return;
        }

        w.ListData.Add(new WorkOrderItem { });
        U.BindGrid(gvItems, w.ListData);
    }
    private void DeleteItem()
    {
        List<object> oList = U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData;
        if (!oList.Exists(a => ((WorkOrderItem)a).IsChecked))
        {
            U.ShowMessage("Please select Item you want to delete", btnDeleteItem);
            return;
        }

        oList.RemoveAll(a => ((WorkOrderItem)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvItems, new List<object> { new WorkOrderItem { } });
        else U.BindGrid(gvItems, oList);
    }
    #endregion
    private void ItemValidation(Wrapping w)
    {
        w.ListData = U.GetGridData(gvItems, typeof(WorkOrderItem)).ListData;
        int idx = 0;
        foreach (WorkOrderItem o in w.ListData)
        {
            idx += 1;
            if (o.ItemId == 0) w.ErrorValidation = $"Item is required at row {idx}";
            if (o.Qty == 0) w.ErrorValidation = $"Qty is required at row {idx}";
        }
    }
    #endregion

}