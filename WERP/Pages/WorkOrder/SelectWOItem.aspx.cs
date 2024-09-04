using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;

public partial class Pages_WorkOrder_SelectWOItem : System.Web.UI.Page
{
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) Initialize();
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Wrapping w = new Wrapping();
        List<object> woiList = U.GetGridData(gvData, typeof(WorkOrderWorkUpdateItem)).ListData;
        if (!woiList.Exists(a => ((WorkOrderWorkUpdateItem)a).IsChecked))
        {
            U.ShowMessage($"Please select the Item you want {U.QSWorkType}");
            return;
        }

        int idx = 0;        
        foreach (WorkOrderWorkUpdateItem woi in woiList)
        {
            idx += 1;
            if (!woi.IsChecked) continue;
            if (woi.Qty == 0) w.ErrorValidation = $"Qty is required in Row {idx}";
            if (woi.Qty > woi.UsedQty)
                w.ErrorValidation = $"{U.QSWorkType} Qty cant be bigger then Used Qty in Row {idx}";   
            woi.ReferenceId = woi.Id;
            woi.Id = 0;
            if (ViewState[CNT.VS.WorkType].Is("Retur")) woi.ReturQty = woi.Qty;
            else woi.DisposalQty = woi.Qty;
            woi.Qty = 0;
        }                
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }
        Session[CNT.Session.WorkUpdateItem] = woiList.FindAll(a => ((WorkOrderWorkUpdateItem)a).IsChecked);
        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference(U.QSSeq));
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvData.ClientID.Replace("_", "$")));
        }
        //else if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
        //    ltrl_No.Text = (e.Row.RowIndex + 1).ToString();
        //}
    }
    #endregion

    #region Methods
    private void Initialize()
    {
        BindData();
    }
    private void BindData()
    {
        if (Session[CNT.Session.WorkUpdate] == null)
        {
            U.ShowMessage("Work Update is required");
            return;
        }

        List<object> wowuList = (List<object>)Session[CNT.Session.WorkUpdate];
        WorkOrderWorkUpdate CurrentWowu = (WorkOrderWorkUpdate)wowuList[U.QSSeq.ToInt()];
        ViewState[CNT.VS.WorkType] = CurrentWowu.WorkType;
        List<WorkOrderWorkUpdateItem> wowuiList = new List<WorkOrderWorkUpdateItem>();
        foreach (WorkOrderWorkUpdate wowu in wowuList)
        {
            foreach (WorkOrderWorkUpdateItem wowui in wowu.Items)
                wowuiList.Add(wowui);
        }

        List<object> Items = new List<object>();        
        var wuList = WorkOrderWorkUpdate.GetByWorkOrderId(U.QSWorkOrderId);
        int No = 0;
        foreach (WorkOrderWorkUpdate wu in wuList)
        {
            var wuiList = WorkOrderWorkUpdateItem.GetByWorkOrderWorkUpdateId(wu.Id);
            foreach (WorkOrderWorkUpdateItem wui in wuiList)
            {
                int UsedQty = wowuiList.Where(a => a.ReferenceId.Is(wui.Id)).Sum(b => CurrentWowu.WorkType.Is("Retur") ? b.ReturQty : b.DisposalQty);
                wui.Qty = wui.Qty - wui.ReturQty - wui.DisposalQty;
                if (UsedQty >= wui.Qty) continue;
                No += 1;
                wui.No = No;
                wui.WorkDetail = wu.WorkDetail;
                wui.WorkUpdateDate = wu.Date;                
                wui.UsedQty = wui.Qty - UsedQty;                               
                wui.Qty = 0;
                Items.Add(wui);
            }
        }

        U.BindGrid(gvData, Items);
                
        if (Items.Count.IsNotZero() && (U.QSWorkType.Is(CNT.Retur) || U.QSWorkType.Is(CNT.Disposal)))
        {
            Literal ltrlQty = (Literal)gvData.HeaderRow.FindControl("ltrlQty");
            ltrlQty.Text = $"{U.QSWorkType} Qty";            
        }
    }
    #endregion

}