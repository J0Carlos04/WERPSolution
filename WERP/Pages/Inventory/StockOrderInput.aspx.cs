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

public partial class Pages_Inventory_StockOrderInput : PageBase
{
    #region Fields
    private const string vsUserName = "Username";
    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            LoadFirstTime();
        string Script = "InitSelect2StockOrderInput();";       
        F.PageContext.RegisterStartupScript(Script);
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {            
            case "btnSubmit":
                Submit();
                break;
            case "btnDelete":
                Delete();
                break;
        }
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {
            case "btnAdd":
                AddItem();
                break;
        }
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddlProcurementType":
                ProcurementTypeChanged();
                break;
        }
    }
    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {
            case "imbDelete":
                DeleteItem(imb);
                break;
        }
    }

    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            Button btnCodeLookup = (Button)e.Row.FindControl("btnCodeLookup");            

            btnCodeLookup.OnClientClick = wItem.GetShowReference($"~/Pages/Inventory/SelectItem.aspx?RowIndex={e.Row.RowIndex}");
            btnCodeLookup.OnClientClick += "return false;";                  
        }
    }
    protected void wPO_Close(object sender, F.WindowCloseEventArgs e)
    {
        PO o = PO.GetById(e.CloseArgument);
        lblPOId.Text = $"{o.Id}";
        tbPONo.Value = o.PONo;
    }
    protected void wItem_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrResult = e.CloseArgument.Split('|');
        Goods g = Goods.GetById(arrResult[1]);

        List<object> oList = U.GetGridData(gvItems, typeof(StockOrderItem)).ListData;
        if (oList.Exists(a => ((StockOrderItem)a).ItemId == g.Id))
        {
            U.ShowMessage("Item already exist");
            return;
        }

        foreach (GridViewRow row in gvItems.Rows)
        {
            if (row.RowIndex != arrResult[0].ToInt()) continue;
            Literal ltrl_ItemId = (Literal)row.FindControl("ltrl_ItemId");
            TextBox tb_Code = (TextBox)row.FindControl("tb_Code");
            Literal ltrl_Name = (Literal)row.FindControl("ltrl_Name");
            ltrl_ItemId.Text = g.Id.ToString();
            tb_Code.Text = g.Code;
            ltrl_Name.Text = g.Name;
        }
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInit();                
        SetInitControl();
        SetInitEdit();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        ViewState[CNT.VS.PageName] = "StockOrder";
        if (!U.CreateAccess("StockOrder")) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSelectPO.OnClientClick = wPO.GetShowReference($"~/Pages/Inventory/SelectPO.aspx");
        btnSelectPO.OnClientClick += "return false;";
    }
    private void SetInitControl()
    {
        U.SetDropDownMasterData(ddlRequester, "Users");
        U.SetDropDownMasterData(ddlVendor, "Vendor");
        U.SetApproval(ddlApprover);
        tbStatus.Value = "Submitted";
        U.BindGrid(gvItems, new List<object> { new StockOrderItem() });                
    }
    private void SetInitEdit()
    {
        if (U.Id.IsNull())
        {
            btnDelete.Hidden = true;            
            return;
        }
        btnDelete.Hidden = !U.DeleteAccess(ViewState[CNT.VS.PageName]);
        btnSubmit.ConfirmText = "All Approval data wil be reset, Are your sure want to submit this Stock Order ?";
        StockOrder o = StockOrder.GetById(U.Id);
        tbCode.Value = o.Code;
        tbDescription.Value = o.Description;
        ddlProcurementType.SelectedValue = o.ProcurementType;
        pnlPO.Visible = false;
        if (o.ProcurementType == "Regular")
        {
            PO p = PO.GetById(o.POId);
            lblPOId.Text = $"{p.Id}";
            tbPONo.Value = p.PONo;
            pnlPO.Visible = true;
        }
        ddlVendor.SelectedValue = $"{o.VendorId}";
        ddlRequester.SelectedValue = $"{o.RequesterUserId}";
        tbStatus.Value = o.Status;
        ddlApprover.SelectedValue = $"{o.ApproverUserId}";       

        List<object> siList = StockOrderItem.GetByParendId(o.Id);
        U.BindGrid(gvItems, siList);
        if (o.Status == "Rejected") gvItems.Columns[2].Visible = true;
    }    

    private void ProcurementTypeChanged()
    {
        pnlPO.Visible = false;
        if (ddlProcurementType.SelectedValue == "Regular") pnlPO.Visible = true;
    }

    private void AddItem()
    {
        Wrapping w = new Wrapping();
        ItemValidation(w);
        if ($"{w.Sb}" != "")
        {
            U.ShowMessage($"{w.Sb}");
            return;
        }

        w.ListData.Add(new StockOrderItem { });
        U.BindGrid(gvItems, w.ListData);
    }
    private void DeleteItem(ImageButton imb)
    {
        GridViewRow row = (GridViewRow)imb.Parent.Parent;
        List<object> oList = U.GetGridData(gvItems, typeof(StockOrderItem)).ListData;
        oList.RemoveAt(row.RowIndex);
        if (oList.Count == 0) U.BindGrid(gvItems, new List<object> { new StockOrderItem { } });
        else U.BindGrid(gvItems, oList);
    }

    private void Submit()
    {
        Wrapping w = new Wrapping();
        Validation(w);
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
    private void Validation(Wrapping w)
    {
        if (tbCode.Value.IsEmpty()) w.RequiredValidation = "Code";
        else if(DataAccess.IsFieldExist("StockOrder", "Code", tbCode.Value, U.Id)) w.ErrorValidation = $"Stock Order with code : {tbCode.Value} already exist";
        if (ddlRequester.SelectedValue.IsEmpty()) w.RequiredValidation = "Requester";

        if (ddlProcurementType.SelectedValue == "Regular")
        {
            if (tbPONo.Value.IsEmpty()) w.RequiredValidation = "PO No";
        }
        if (ddlVendor.SelectedValue.IsEmpty()) w.RequiredValidation = "Vendor";

        if (ddlApprover.SelectedValue.IsEmpty()) w.RequiredValidation = "Approver";
        ItemValidation(w);
    }
    private void ItemValidation(Wrapping w)
    {
        w.ListData = U.GetGridData(gvItems, typeof(StockOrderItem)).ListData;
        int idx = 0;
        foreach (StockOrderItem o in w.ListData)
        {
            idx += 1;
            if (o.ItemId == 0) w.ErrorValidation = $"Item is required at row {idx}";            
            if (o.Qty == 0) w.ErrorValidation = $"Qty is required at row {idx}";
            if (o.UnitPrice == 0) w.ErrorValidation = $"Unit Price is required at row {idx}";
        }
    }
    private string Save(Wrapping w)
    {
        #region Stock Order
        StockOrder o = new StockOrder();
        o.Code = tbCode.Value;
        o.Description = tbDescription.Value;
        o.RequesterUserId = ddlRequester.SelectedValue.ToInt();
        o.ProcurementType = ddlProcurementType.SelectedValue;
        if (o.ProcurementType == "Regular")
            o.POId = lblPOId.Text.ToInt();
        o.VendorId = ddlVendor.SelectedValue.ToInt();
        o.Status = tbStatus.Value;
        o.ApproverUserId = ddlApprover.SelectedValue.ToInt();
        o.Status = "Submitted";        
        string Result = "";
        if (U.Id == null)
        {            
            o.CreatedBy = $"{ViewState[vsUserName]}";
            Result = o.Insert();
            if (Result.ContainErrorMessage()) return Result;
            o.Id = Result.ToInt();
        }
        else
        {            
            o.Id = U.Id.ToInt();
            o.ModifiedBy = $"{ViewState[vsUserName]}";
            Result = o.Update();
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion

        #region Stock Order Item
        List<object> siDBList = StockOrderItem.GetByParendId(o.Id);
        foreach (StockOrderItem si in siDBList)
        {
            if (!w.ListData.Exists(a => ((StockOrderItem)a).Id == si.Id))
            {
                Result = si.Delete();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        foreach (StockOrderItem si in w.ListData)
        {
            si.StockOrderId = o.Id;
            si.RejectReason = "";
            if (si.Id == 0)
            {
                si.CreatedBy = $"{ViewState[vsUserName]}";
                Result = si.Insert();
                if (Result.ContainErrorMessage()) return Result;
            }
            else
            {
                si.ModifiedBy = $"{ViewState[vsUserName]}";
                Result = si.Update();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        #endregion

        return "";
    }
    private void Delete()
    {
        string Result = StockOrder.Delete(U.Id);
        if (Result.ContainErrorMessage()) U.ShowMessage($"Delete Failed : {Result}");
        else U.CloseUpdate("Data has been Deleted Successfully");
    }
    #endregion            
}