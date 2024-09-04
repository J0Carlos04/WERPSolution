using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.Collections;

public partial class Pages_Inventory_StockOrderApproval : PageBase
{
    #region Fields
    private const string vsUserName = "Username";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) LoadFirstTime();         
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnApproveAll":
                ApprovedAll();
                break;
            case "btnRejectAll":
                RejectAll();
                break;
            case "btnSubmitApproval":
                Submit();
                break;

        }        
    }            
    protected void cb_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        switch (cb.ID)
        {
            case "cbCheckAllApprove":
                CheckAllChanged("Approve", cb.Checked);
                break;
            case "cbCheckAllReject":
                CheckAllChanged("Reject", cb.Checked);
                break;
            case "cb_IsCheckedApprove":
                CheckChange(cb, "Approve");
                break;
            case "cb_IsCheckedReject":
                CheckChange(cb, "Reject");
                break;
        }        
    }

    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAllApprove = (CheckBox)e.Row.FindControl("cbCheckAllApprove");
            CheckBox cbCheckAllReject = (CheckBox)e.Row.FindControl("cbCheckAllReject");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }

    protected string GetItemUrl(object id)
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", "ViewItem");
        joBuilder.AddProperty("title", "View Item");
        joBuilder.AddProperty("iframeUrl", $"getItemWindowUrl('{id}')", true);
        joBuilder.AddProperty("refreshWhenExist", true);
        joBuilder.AddProperty("iconFont", "pencil");
        return String.Format("parent.addExampleTab({0});", joBuilder);
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        ViewState[vsUserName] = U.GetUsername();
        if ($"{ViewState[vsUserName]}" == "") Response.Redirect(@"~\Pages\default.aspx");
        btnCancel.OnClientClick = "parent.removeActiveTab();";        
        SetInitEdit();
    }    
    private void SetInitEdit()
    {
        if (U.Id.IsNull()) return;
        StockOrder o = StockOrder.GetById(U.Id);
        tbCode.Value = o.Code;
        tbDescription.Value = o.Description;
        if (o.Description.IsEmpty()) pnlDescription.Visible = false;
        tbRequester.Value = o.Requester;
        tbProcurementType.Value = o.ProcurementType;
        pnlPO.Visible = false;
        if (o.ProcurementType == "Regular")
        {
            pnlPO.Visible = true;
            tbPO.Value = o.PONo;
        }
        tbVendor.Value = o.Vendor;
        tbApprover.Value = o.Approver;
        tbStatus.Value = o.Status;

        List<object> siList = StockOrderItem.GetByParendId(o.Id);
        U.BindGrid(gvItems, siList);
    }
    private void CheckAllChanged(string Name, bool Checked)
    {
        string OppositeName = Name == "Approve" ? "Reject" : "Approve";
        foreach (GridViewRow Row in gvItems.Rows)
        {
            CheckBox cb = (CheckBox)Row.FindControl($"cb_IsChecked{Name}");
            TextBox tb_RejectReason = (TextBox)Row.FindControl("tb_RejectReason");
            tb_RejectReason.Visible = false;
            cb.Checked = Checked;

            CheckBox cbOpposite = (CheckBox)Row.FindControl($"cb_IsChecked{OppositeName}");
            cbOpposite.Checked = !Checked;

            if ((Name == "Reject" && Checked == true) || (Name == "Approve" && Checked == false))
                tb_RejectReason.Visible = true;
        }
        CheckBox cbAll = (CheckBox)gvItems.HeaderRow.FindControl($"CbCheckAll{OppositeName}");
        cbAll.Checked = !Checked;
    }
    private void CheckChange(CheckBox cb, string Name)
    {
        string OppositeName = Name == "Approve" ? "Reject" : "Approve";
        GridViewRow CurrentRow = (GridViewRow)cb.Parent.Parent;
        CheckBox cbOpposite = (CheckBox)CurrentRow.FindControl($"cb_IsChecked{OppositeName}");
        TextBox tbRejectReason = (TextBox)CurrentRow.FindControl("tb_RejectReason");
        tbRejectReason.Visible = false;
        cbOpposite.Checked = !cb.Checked;
        if (Name == "Reject" && cb.Checked == true)
            tbRejectReason.Visible = true;


        bool Checked = true;
        bool OppositeChecked = true;
        foreach (GridViewRow Row in gvItems.Rows)
        {
            CheckBox cb_IsChecked = (CheckBox)Row.FindControl($"cb_IsChecked{Name}");
            CheckBox cb_IsCheckedOpposite = (CheckBox)Row.FindControl($"cb_IsChecked{OppositeName}");
           
            

            if (!cb_IsChecked.Checked) Checked = false;
            if (!cb_IsCheckedOpposite.Checked) OppositeChecked = false;            
        }

        CheckBox cbAll = (CheckBox)gvItems.HeaderRow.FindControl($"CbCheckAll{Name}");
        CheckBox cbAllOpposite = (CheckBox)gvItems.HeaderRow.FindControl($"CbCheckAll{OppositeName}");
        cbAll.Checked = Checked;
        cbAllOpposite.Checked = OppositeChecked;
    }

    private void ApprovedAll()
    {
        CheckAllChanged("Approve", true);
        List<object> oList = U.GetGridData(gvItems, typeof(StockOrderItem)).ListData;
        Wrapping w = new Wrapping();
        int idx = 0;
        foreach (StockOrderItem o in oList)
        {
            idx += 1;
            if (o.ApprovedQty == 0) w.ErrorValidation = $"Approved Qty is required at row {idx}";
            if (o.ApprovedQty > o.Qty) w.ErrorValidation = $"Approved Qty can't be greater then Qty at row {idx}";
            if (o.ApprovedQty < o.ReceivedQty) w.ErrorValidation = $"Approved Qty can't be less then Received Qty at row {idx}";
        }
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }
        string Result = "";
        foreach (StockOrderItem o in oList)
        {
            o.ModifiedBy = $"{ViewState[vsUserName]}";
            o.PendingQty += o.ApprovedQty;
            if (o.ApprovedQty == o.Qty) o.Status = "Approved";
            else o.Status = "Partial Approved";            
            Result = o.Approved();
            if (Result.ContainErrorMessage())
            {
                U.ShowMessage($"Approved Failed, {Environment.NewLine}{Result}");
                return;
            }
        }
        StockOrder si = new StockOrder();
        si.Id = U.Id.ToInt();
        if (oList.Exists(a => ((StockOrderItem)a).Status == "Partial Approved")) si.Status = "Partial Approved";
        else si.Status = "Approved";
        si.ModifiedBy = $"{ViewState[vsUserName]}";
        Result = si.UpdateApproval();
        if (Result.ContainErrorMessage())
        {
            U.ShowMessage($"Approved Failed, {Environment.NewLine}{Result}");
            return;
        }
        F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
    }
    private void RejectAll()
    {
        CheckAllChanged("Reject", true);
        List<object> oList = U.GetGridData(gvItems, typeof(StockOrderItem)).ListData;
        Wrapping w = new Wrapping();
        int idx = 0;
        foreach (StockOrderItem o in oList)
        {
            idx += 1;
            if (o.RejectReason.IsEmpty()) w.ErrorValidation = $"Reject Reason is required at row {idx}";
            if (o.ReceivedQty > 0) w.ErrorValidation = $"Can't reject an item that has been received at row {idx}";
        }
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }
        string Result = "";
        foreach (StockOrderItem o in oList)
        {
            o.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = o.Rejected();
            if (Result.ContainErrorMessage())
            {
                U.ShowMessage($"Approved Failed, {Environment.NewLine}{Result}");
                return;
            }
        }
        StockOrder si = new StockOrder();
        si.Id = U.Id.ToInt();
        si.Status = "Rejected";
        si.ModifiedBy = $"{ViewState[vsUserName]}";
        Result = si.UpdateApproval();
        if (Result.ContainErrorMessage())
        {
            U.ShowMessage($"Approved Failed, {Environment.NewLine}{Result}");
            return;
        }
        F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");
    }
    private void Submit()
    {
        Wrapping w = new Wrapping();
        ItemValidation(w);
        if ($"{w.Sb}" != "") U.ShowMessage($"{w.Sb}");
        else
        {
            string Result = Save(w);
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }
            F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");

        }
    }
    private void ItemValidation(Wrapping w)
    {
        w.ListData = U.GetGridData(gvItems, typeof(StockOrderItem)).ListData;
        if (w.ListData.Count(a => ((StockOrderItem)a).IsCheckedApprove == false && ((StockOrderItem)a).IsCheckedReject == false) == w.ListData.Count)
            w.ErrorValidation = "Please check the box for each item you want to approve";
        else
        {
            int idx = 0;
            foreach (StockOrderItem o in w.ListData)
            {
                idx += 1;
                if (o.IsCheckedApprove == false && o.IsCheckedReject == false) w.ErrorValidation = $"Approval at row {idx} is required";
                if (o.IsCheckedReject == true && o.RejectReason == "") w.ErrorValidation = $"Reject Reason is required at row {idx}";
                if (o.IsCheckedApprove && o.ApprovedQty == 0) w.ErrorValidation = $"Approved Qty is required at row {idx}";
                if (o.ApprovedQty > o.Qty) w.ErrorValidation = $"Approved Qty can't bigger then Qty at row {idx}";
                if (o.ApprovedQty < o.ReceivedQty) w.ErrorValidation = $"Approved Qty can't be less then Received Qty at row {idx}";
                if (o.IsCheckedReject == true && o.ReceivedQty > 0) w.ErrorValidation = $"Can't reject an item that has been received at row {idx}";
            }
        }
    }
    private string Save(Wrapping w)
    {
        string Result = "";
        foreach (StockOrderItem o in w.ListData)
        {
            o.PendingQty += o.ApprovedQty;
            if (o.IsCheckedReject) o.Status = "Rejected";
            else if (o.ApprovedQty == o.Qty) o.Status = "Approved";
            else o.Status = "Partial Approved";
            if (o.IsCheckedApprove)
                Result = o.Approved();
            else
                Result = o.Rejected();
            if (Result.ContainErrorMessage()) return Result;
        }

        StockOrder si = new StockOrder();
        si.Id = U.Id.ToInt();
        if (w.ListData.Exists(a => ((StockOrderItem)a).Status == "Partial Approved")) si.Status = "Partial Approved";
        else if (w.ListData.Count(a => ((StockOrderItem)a).IsCheckedApprove) == w.ListData.Count)
            si.Status = "Approved";
        else if (w.ListData.Count(a => ((StockOrderItem)a).IsCheckedReject) == w.ListData.Count)
            si.Status = "Rejected";
        else si.Status = "Partial Approved";

        Result = si.UpdateApproval();

        return Result;
    }
    #endregion            
}