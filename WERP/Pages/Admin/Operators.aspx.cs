using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;
using GemBox.Spreadsheet;
using System.IO;
using DAL;
using System.Text;

public partial class Pages_Admin_Operators : PageBase
{   
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadFirstTime();
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnAdd":
                SetInitAdd();
                break;
            case "btnAddSave":
                SaveAdd();
                break;

            case "btnEdit":
                SetInitEdit();
                break;
            case "btnEditSave":
                SaveEdit();
                break;

            case "btnAddCancel":
            case "btnEditCancel":
                Cancel();
                break;

            case "btnDelete":
                Delete();
                break;

            case "btnResetPassword":
                ResetPassword();
                break;

            case "btnExportExcel":
                ExportExcel();
                break;
        }
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, gvData.ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox cb_IsChecked = (CheckBox)e.Row.FindControl("cb_IsChecked");
            DropDownList ddl_VendorId = (DropDownList)e.Row.FindControl("ddl_VendorId");
            DropDownList ddl_Active = (DropDownList)e.Row.FindControl("ddl_Active");

            List<object> oList = (List<object>)gvData.DataSource;
            Operators o = (Operators)oList[e.Row.RowIndex];

            U.SetDropDownMasterData(ddl_VendorId, "Vendor");
            ddl_VendorId.SetValue(o.VendorId);

            cb_IsChecked.Enabled = oList.Exists(a => $"{((Operators)a).Mode}" != "") ? false : true;
            ddl_Active.SetValue(o.Active);
        }
    }
    protected void ucPaging_OpChanged(object sender, EventArgs e)
    {
        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("DataType", "data"));
        #endregion

        double CurrentPage = 0;
        if (sender.ToString().Contains("LinkButton"))
            CurrentPage = Convert.ToDouble(((DropDownList)((LinkButton)sender).Parent.FindControl("ddlPage")).SelectedValue);
        else CurrentPage = Convert.ToDouble(((DropDownList)sender).SelectedValue);

        double StartNo = (CurrentPage * Convert.ToInt32(ups.SelectedValue)) - (Convert.ToInt32(ups.SelectedValue) - 1);
        double EndNo = (StartNo + (Convert.ToInt32(ups.SelectedValue) - 1));

        pList.Add(new clsParameter("StartNo", StartNo));
        pList.Add(new clsParameter("EndNo", EndNo));

        List<object> oList = oList = Operators.GetByCriteria(pList, out double TotalRow);
        U.BindGrid(gvData, oList);
        lblRowCount.Text = string.Format("Page {0} of {1}, Total Record : {2}", CurrentPage, (double)ViewState[CNT.VS.TotalPage], (double)ViewState[CNT.VS.TotalRow]);

        ViewState[CNT.VS.StartNo] = StartNo;
    }
    protected void ups_Changed(object sender, EventArgs e)
    {
        ViewState[CNT.VS.StartNo] = null;
        SetInitControl();
        BindData();
    }
    protected void wSearch_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = Session[CNT.Session.Criteria].ToText().Split(new string[] { "~|" }, StringSplitOptions.None);
        Session[CNT.Session.Criteria] = null;
        ViewState[CNT.VS.Criteria] = arrArgument[0];
        ViewState[CNT.VS.SearchData] = arrArgument[1];
        btnSearch.OnClientClick = wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Operators&SearchData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSearch.OnClientClick += "return false;";
        Search();
    }
    protected void wSort_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrArgument = e.CloseArgument.Split(new string[] { "~|" }, StringSplitOptions.None);
        ViewState[CNT.VS.SortBy] = arrArgument[0];
        ViewState[CNT.VS.SortData] = arrArgument[1];
        btnSorting.OnClientClick = wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Operators&SortData={arrArgument[1].Replace(Environment.NewLine, "~")}");
        btnSorting.OnClientClick += "return false;";
        BindData();
    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInit();
        SetInitControl();
        BindData();
    }
    private void SetInit()
    {
        ViewState.Clear();
        ViewState[CNT.VS.Username] = U.GetUsername();
        if (ViewState[CNT.VS.Username].IsEmpty()) Response.Redirect(@"~\Pages\default.aspx");
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");
        btnSearch.OnClientClick = $"{wSearch.GetShowReference($"~/Pages/Search.aspx?ClassName=Operators")}return false;";
        btnSorting.OnClientClick = $"{wSort.GetShowReference($"~/Pages/Sort.aspx?ClassName=Operators")}return false;";        
    }
    private void SetInitControl()
    {
        btnSearch.Hidden = false;
        btnSorting.Hidden = false;

        btnAdd.Hidden = false;
        btnEdit.Hidden = false;
        btnDelete.Hidden = false;

        btnAddSave.Hidden = true;
        btnAddCancel.Hidden = true;
        btnEditSave.Hidden = true;
        btnEditCancel.Hidden = true;

        btnResetPassword.Hidden = false;
        btnExportExcel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = true;
    }

    private void Search()
    {
        #region ViewState        
        ViewState[CNT.VS.StartNo] = null;
        ViewState[CNT.VS.EndNo] = null;
        ViewState[CNT.VS.TotalRow] = null;
        ViewState[CNT.VS.TotalPage] = null;        
        #endregion

        SetInitControl();

        BindData();
    }
    private void BindData()
    {
        double StartNo = ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo];
        double EndNo = 0;
        double TotalRow = 0;

        #region GetCiteria
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("StartNo", StartNo));
        if (ups.SelectedValue != "ALL")
        {
            EndNo = StartNo + (Convert.ToInt32(ups.SelectedValue) - 1);
            pList.Add(new clsParameter("EndNo", EndNo));
            pList.Add(new clsParameter("DataType", "data"));
        }
        else pList.Add(new clsParameter("DataType", "all"));
        #endregion

        List<object> oList = Operators.GetByCriteria(pList, out TotalRow);

        bool IsDataExist = true;
        if (oList.Count == 0) IsDataExist = PrepareEmptyData(oList);
        else ups.Enabled = true;

        U.BindGrid(gvData, oList);

        #region SetPagingControl
        if (TotalRow == 0) TotalRow = 1;
        double TotalPage = 1;
        if (ups.SelectedValue != "ALL") TotalPage = Convert.ToDouble(Math.Ceiling(Convert.ToDouble(TotalRow) / (Convert.ToInt32(ups.SelectedValue))));

        string sCurrentPage = ((DropDownList)ucPaging.FindControl("ddlPage")).SelectedValue;
        int CurrentPage = Convert.ToInt32(sCurrentPage == "" ? "1" : sCurrentPage);
        ucPaging.SetTotalRow(TotalPage, CurrentPage);
        lblRowCount.Text = string.Format("Page {0} of {1}, Total Record : {2}", CurrentPage, TotalPage, IsDataExist ? TotalRow : 0);
        #endregion

        ViewState[CNT.VS.StartNo] = StartNo;
        ViewState[CNT.VS.EndNo] = EndNo;
        ViewState[CNT.VS.TotalRow] = TotalRow;
        ViewState[CNT.VS.TotalPage] = TotalPage;
    }
    private bool PrepareEmptyData(List<object> oList)
    {
        oList.Add(new Operators() { No = -1, Name = CNT.DataNotAvailable });
        btnDelete.Hidden = true;
        btnEdit.Hidden = true;
        btnExportExcel.Hidden = true;
        ups.Enabled = false;
        ucPaging.Visible = false;
        return false;
    }
    private void SetInitAdd()
    {
        #region Set Visibility Control
        btnSearch.Hidden = true;
        btnSorting.Hidden = true;
        btnAdd.Hidden = true;
        btnEdit.Hidden = true;
        btnDelete.Hidden = true;
        btnEditSave.Hidden = true;
        btnEditCancel.Hidden = true;
        btnResetPassword.Hidden = true;
        btnExportExcel.Hidden = true;
        ups.Enabled = false;
        ucPaging.Visible = false;

        btnAddSave.Hidden = false;
        btnAddCancel.Hidden = false;
        #endregion

        #region Add Empty Row        
        List<object> oList = U.GetGridData(gvData, Operators.MyType).ListData.FindAll(a => ((Operators)a).Name != CNT.DataNotAvailable);
        oList.ForEach(a => { ((Operators)a).IsChecked = false; ((Operators)a).Mode = ""; });
        oList.Insert(0, new Operators() { No = 0, IsChecked = true, Mode = "add" });
        U.BindGrid(gvData, oList);
        ((CheckBox)gvData.HeaderRow.Cells[0].FindControl("cbCheckAll")).Enabled = false;
        #endregion        
    }
    private void SetInitEdit()
    {
        List<object> oList = U.GetGridData(gvData, Operators.MyType).ListData;
        if (!oList.Exists(a => ((Operators)a).IsChecked))
        {
            U.ShowMessage("Please select row that you want to edit");
            return;
        }

        #region Set Visibility Control
        btnSearch.Hidden = true;
        btnSorting.Hidden = true;
        btnAdd.Hidden = true;
        btnAddSave.Hidden = true;
        btnAddCancel.Hidden = true;
        btnEdit.Hidden = true;
        btnDelete.Hidden = true;
        btnResetPassword.Hidden = true;
        btnExportExcel.Hidden = true;

        btnEditSave.Hidden = false;
        btnEditCancel.Hidden = false;
        ups.Enabled = false;
        ucPaging.Visible = false;
        #endregion                       

        oList.ForEach(a => { ((Operators)a).Mode = ((Operators)a).IsChecked ? "edit" : ""; });
        U.BindGrid(gvData, oList);
        ((CheckBox)gvData.HeaderRow.Cells[0].FindControl("cbCheckAll")).Enabled = false;
    }
    private void Cancel()
    {
        SetInitControl();
        BindData();
    }
    private void SaveAdd()
    {
        try
        {
            string Result = "";
            List<object> oList = U.GetGridData(gvData, typeof(Operators)).ListData;
            Operators o = (Operators)oList[0];            

            #region Validation	            
            Result = U.SingleRequiredValidation(o);
            if (Result != "")
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{Result}");
                return;
            }

            StringBuilder sb = new StringBuilder();
            int Seq = 0;
            if (Operators.IsFieldExist("Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. Operator with Name : {o.Name} already exist");
            if (Operators.IsNameVendorExist(o.Name, o.VendorId, o.Id)) sb.AppendLine($"{Seq.Inc()}. Operator with Name : {o.Name} and Vendor with Name : {o.Vendor} already exist");
            if (!U.IsValidEmail(o.Email)) sb.AppendLine("Invalid Email Address");
            if (Operators.IsFieldExist("Username", o.Username, o.Id)) sb.AppendLine($"{Seq.Inc()}. Operator with Username : {o.Username} already exist");
            if (DataAccess.IsFieldExist("Users", "Username", o.Username, 0)) sb.AppendLine($"{Seq.Inc()}. User with Username : {o.Username} already exist");
            if (Users.IsUsernameExist(o.Username)) sb.AppendLine($"Username : {o.Username} already exist in Users");

            if (!sb.ToText().IsEmpty())
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{sb.ToText()}");
                return;
            }
            #endregion

            o.Password = U.EncryptString(o.Username);
            o.CreatedBy = $"{ViewState[CNT.VS.Username]}";
            o.Active = true;
            Result = o.Insert();
            if (Result.Contains("Error Message"))
            {
                U.ShowMessage($"Save Failed, {Environment.NewLine}{Result}");
                return;
            }
            o.Id = Convert.ToInt32(Result);

            SetInitControl();
            BindData();
        }
        catch (Exception ex) { U.ShowMessage($"Save Failed, {Environment.NewLine}{ex.Message}"); }
    }
    private void SaveEdit()
    {
        string Result = "";
        List<object> oList = U.GetGridData(gvData, typeof(Operators)).ListData;
        Result = U.MultipleRequiredValidation(oList);
        if (!Result.IsEmpty())
        {
            U.ShowMessage($"Save Failed,{Environment.NewLine} {Result}");
            return;
        }

        int Seq = 0;
        StringBuilder sb = new StringBuilder();
        foreach (Operators o in oList)
        {
            if (!o.IsChecked) continue;
            #region Validation            
            if (Operators.IsFieldExist("Name", o.Name, o.Id)) sb.AppendLine($"{Seq.Inc()}. Operator with Name : {o.Name} already exist");
            if (Operators.IsNameVendorExist(o.Name, o.VendorId, o.Id)) sb.AppendLine($"{Seq.Inc()}. Operator with Name : {o.Name} and Vendor with Name : {o.Vendor} already exist");
            if (DataAccess.IsFieldExist("Users", "Username", o.Username, 0)) sb.AppendLine($"{Seq.Inc()}. Operator with Name : {o.Username} already exist");
            if (!U.IsValidEmail(o.Email)) sb.AppendLine($"{Seq.Inc()}. {o.Email} is Invalid Email Address");
            if (!sb.ToText().IsEmpty()) continue;
            #endregion
            
            o.ModifiedBy = $"{ViewState[CNT.VS.Username]}";
            Result = o.Update();
            if (Result.Contains("Error Message"))
            {
                U.ShowMessage($"Save Failed,{Environment.NewLine} {Result}");
                return;
            }
            o.Mode = "";
            o.IsChecked = false;
        }
        if (!sb.ToText().IsEmpty())
        {
            U.ShowMessage($"Save Failed, {Environment.NewLine}{sb.ToText()}");
            U.BindGrid(gvData, oList);
            return;
        }

        U.BindGrid(gvData, oList);
        SetInitControl();
    }
    private void Delete()
    {
        try
        {
            string Result = "";
            List<object> oList = U.GetGridData(gvData, typeof(Operators)).ListData;
            if (oList.FindAll(a => ((Operators)a).IsChecked).Count == 0)
            {
                U.ShowMessage("Please select row that you want to delete");
                return;
            }

            for (int i = 0; i < oList.Count; i++)
            {
                Operators o = (Operators)oList[i];
                if (!o.IsChecked) continue;

                Result = DataAccess.IsDataExist(0, "OperatorsId", o.Id);
                if (!Result.IsEmpty())
                {
                    U.ShowMessage($"Delete Failed, {Environment.NewLine}Data used in {Result}, please delete those data first");
                    return;
                }

                Result = DataAccess.Delete(0, o.Id, "Id", "Operators");
                if (Result.Contains("Error Message"))
                {
                    U.ShowMessage(string.Format("Delete Failed, \\n{0}", Result));
                    return;
                }
                oList.Remove(o);
                i -= 1;
            }

            double StartNo = (double)ViewState[CNT.VS.StartNo];
            if (oList.Count == 0 && StartNo != 1)
            {
                ViewState[CNT.VS.StartNo] = (StartNo - 10);
                DropDownList ddlPage = ((DropDownList)ucPaging.FindControl("ddlPage"));
                int value = Convert.ToInt32(ddlPage.SelectedValue);
                ((DropDownList)ucPaging.FindControl("ddlPage")).SelectedValue = (value - 1).ToString();
            }
            Search();
        }
        catch (Exception ex) { U.ShowMessage($"Delete Failed, {Environment.NewLine}{ex.Message}"); }
    }
    private void ResetPassword()
    {
        List<object> oList = U.GetGridData(gvData, Operators.MyType).ListData;
        if (!oList.Exists(a => ((Operators)a).IsChecked))
        {
            U.ShowMessage("Please select Operator you want to reset the password");
            return;
        }
        foreach (Operators o in oList.FindAll(a => ((Operators)a).IsChecked))
        {
            string Result = o.ResetPassword();
            if (Result.ContainErrorMessage())
            {
                U.ShowMessage($"Reset password for Operator : {o.Name} failed, {Result}");
                return;
            }
        }
        BindData();
        U.ShowMessage("Password has been Reset Successfully");
    }
    private void ExportExcel()
    {
        List<clsParameter> pList = U.GetParameter(ViewState[CNT.VS.Criteria], ViewState[CNT.VS.SortBy]);
        pList.Add(new clsParameter("StartNo", ViewState[CNT.VS.StartNo] == null ? 1 : (double)ViewState[CNT.VS.StartNo]));
        pList.Add(new clsParameter("DataType", "all"));
        List<object> oList = Operators.GetByCriteria(pList, out double TotalRow);

        U.ExportToExcel(oList, "Operators");        
    }
    #endregion
}