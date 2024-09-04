using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using F = FineUI;
using U = Utility;

public partial class Pages_SuperAdmin_UserManagementInput : PageBase
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
            case "btnSubmit":
                Submit();
                break;
            case "btnDelete":
                Delete();
                break;
            case "btnDeleteUser":
                DeleteUsers();
                break;
            case "btnDeleteOperator":
                DeleteOperator();
                break;
            case "btnDeleteRole":
                DeleteRole();
                break;
            case "btnDeleteModule":
                DeleteModule();
                break;
        }
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch(ddl.ID)
        {
            case "ddlAllUsers":
                AllUsersChanged();
                break;
            case "ddlAllOperators":
                AllOperatorsChanged();
                break;
            case "ddlAllModules":
                AllModulesChanged();
                break;
        }
    }
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, ((GridView)sender).ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            ltrl_No.Text = $"{e.Row.RowIndex + 1}";
        }
    }
    protected void gvOperators_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, ((GridView)sender).ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            ltrl_No.Text = $"{e.Row.RowIndex + 1}";
        }
    }
    protected void gvRole_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, ((GridView)sender).ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            ltrl_No.Text = $"{e.Row.RowIndex + 1}";
        }
    }
    protected void gvModule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbCheckAll = (CheckBox)e.Row.FindControl("cbCheckAll");
            cbCheckAll.Attributes.Add("onclick", string.Format("SelectAll('{0}', '{1}');", cbCheckAll.ClientID, ((GridView)sender).ClientID.Replace("_", "$")));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrl_No = (Literal)e.Row.FindControl("ltrl_No");
            ltrl_No.Text = $"{e.Row.RowIndex + 1}";
        }
    }

    protected void wUsers_Close(object sender, FineUI.WindowCloseEventArgs e)
    {
        string[] arrResult = e.CloseArgument.Split(',');
        List<object> oList = U.GetGridData(gvUsers, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
        foreach (string Id in arrResult)
        {
            if (oList.Exists(a => ((UserManagementLookup)a).Id.ToText() == Id)) continue;
            UserManagementLookup uml = UserManagementLookup.GetById(Id, "Users");
            oList.Add(uml);
        }        
        U.BindGrid(gvUsers, oList);
    }
    protected void wOperators_Close(object sender, FineUI.WindowCloseEventArgs e)
    {
        string[] arrResult = e.CloseArgument.Split(',');
        List<object> oList = U.GetGridData(gvOperators, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
        foreach (string Id in arrResult)
        {
            if (oList.Exists(a => ((UserManagementLookup)a).Id.ToText() == Id)) continue;
            UserManagementLookup uml = UserManagementLookup.GetById(Id, "Operators");
            oList.Add(uml);
        }
        U.BindGrid(gvOperators, oList);
    }
    protected void wRole_Close(object sender, F.WindowCloseEventArgs e)
    {
        string[] arrResult = e.CloseArgument.Split(',');
        List<object> oList = U.GetGridData(gvRole, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
        foreach (string Id in arrResult)
        {
            if (oList.Exists(a => ((UserManagementLookup)a).Id.ToText() == Id)) continue;
            UserManagementLookup uml = UserManagementLookup.GetById(Id, "Role");
            oList.Add(uml);
        }
        U.BindGrid(gvRole, oList);
    }
    protected void wModule_Close(object sender, FineUI.WindowCloseEventArgs e)
    {
        string[] arrResult = e.CloseArgument.Split(',');
        List<object> oList = U.GetGridData(gvModule, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
        foreach (string Id in arrResult)
        {
            if (oList.Exists(a => ((UserManagementLookup)a).Id.ToText() == Id)) continue;
            UserManagementLookup uml = UserManagementLookup.GetById(Id, "Module");
            oList.Add(uml);
        }
        U.BindGrid(gvModule, oList);
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
        //if (!U.IsMember(CNT.SuperAdmin)) Response.Redirect($"../{CNT.Unauthorized}.aspx");                
    }
    private void SetInitControl()
    {
        btnSelectUser.OnClientClick = wUsers.GetShowReference($"~/Pages/SuperAdmin/LookupUserManagement.aspx?Tbl=Users");
        btnSelectOperator.OnClientClick = wOperators.GetShowReference($"~/Pages/SuperAdmin/LookupUserManagement.aspx?Tbl=Operators");
        btnSelectRole.OnClientClick = wRole.GetShowReference($"~/Pages/SuperAdmin/LookupUserManagement.aspx?Tbl=Role");
        btnSelectModule.OnClientClick = wModule.GetShowReference($"~/Pages/SuperAdmin/LookupUserManagement.aspx?Tbl=Module");        

        U.BindGrid(gvUsers, new List<object> { new UserManagementLookup() });
        U.BindGrid(gvOperators, new List<object> { new UserManagementLookup() });
        U.BindGrid(gvRole, new List<object> { new UserManagementLookup() });
        U.BindGrid(gvModule, new List<object> { new UserManagementLookup() });

    }
    private void SetInitEdit()
    {
        if (U.Id.IsEmpty()) return;
        UserManagement o = UserManagement.GetById(U.Id);
        ddlAllUsers.SetValue(o.AllUsers);
        AllUsersChanged();
        ddlAllOperators.SetValue(o.AllOperators);
        AllOperatorsChanged();
        ddlAllModules.SetValue(o.AllModules);
        AllModulesChanged();
        ddlCreate.SetValue(o.Create);
        ddlRead.SetValue(o.Read);
        ddlUpdate.SetValue(o.Update);
        ddlDelete.SetValue(o.Delete);
        ddlViewAllData.SetValue(o.ViewAllData);
        ddlDeviation.SetValue(o.Deviation);

        List<object> oList = new List<object>();
        if (!ddlAllUsers.SelectedValue.ToBool())
        {
            oList = UserManagementLookup.GetByParent(U.Id, "Users");
            if (oList.Count == 0) oList.Add(new UserManagementLookup());
            U.BindGrid(gvUsers, oList);
        }
           
        if (!ddlAllOperators.SelectedValue.ToBool())
        {
            oList = UserManagementLookup.GetByParent(U.Id, "Operators");
            if (oList.Count == 0) oList.Add(new UserManagementLookup());
            U.BindGrid(gvOperators, oList);
        }

        oList = UserManagementLookup.GetByParent(U.Id, "Role");
        if (oList.Count == 0) oList.Add(new UserManagementLookup());
        U.BindGrid(gvRole, oList);

        if (!ddlAllModules.SelectedValue.ToBool())
        {
            oList = UserManagementLookup.GetByParent(U.Id, "Module");
            if (oList.Count == 0) oList.Add(new UserManagementLookup());
            U.BindGrid(gvModule, oList);
        }
    }

    private void AllUsersChanged()
    {
        if (ddlAllUsers.SelectedValue.ToBool()) U.Hide("dvSelectedUsers");
        else U.Display("dvSelectedUsers");
    }
    private void AllOperatorsChanged()
    {
        if (ddlAllOperators.SelectedValue.ToBool()) U.Hide("dvSelectedOperators");
        else U.Display("dvSelectedOperators");
    }
    private void AllModulesChanged()
    {
        if (ddlAllModules.SelectedValue.ToBool()) U.Hide("dvSelectedModules");
        else U.Display("dvSelectedModules");
    }

    private void DeleteUsers()
    {
        List<object> oList = U.GetGridData(gvUsers, UserManagementLookup.MyType).ListData;
        if (!oList.Exists(a => ((UserManagementLookup)a).IsChecked))
        {
            U.ShowMessage("Please select the User you want to delete");
            return;
        }
        oList.RemoveAll(a => ((UserManagementLookup)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvUsers, new List<object> { new UserManagementLookup() });
        else U.BindGrid(gvUsers, oList);
    }
    private void DeleteOperator()
    {
        List<object> oList = U.GetGridData(gvOperators, UserManagementLookup.MyType).ListData;
        if (!oList.Exists(a => ((UserManagementLookup)a).IsChecked))
        {
            U.ShowMessage("Please select the Operator you want to delete");
            return;
        }
        oList.RemoveAll(a => ((UserManagementLookup)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvOperators, new List<object> { new UserManagementLookup() });
        else U.BindGrid(gvOperators, oList);
    }
    private void DeleteRole()
    {
        List<object> oList = U.GetGridData(gvRole, UserManagementLookup.MyType).ListData;
        if (!oList.Exists(a => ((UserManagementLookup)a).IsChecked))
        {
            U.ShowMessage("Please select the Role you want to delete");
            return;
        }
        oList.RemoveAll(a => ((UserManagementLookup)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvRole, new List<object> { new UserManagementLookup() });
        else U.BindGrid(gvRole, oList);
    }
    private void DeleteModule()
    {
        List<object> oList = U.GetGridData(gvModule, UserManagementLookup.MyType).ListData;
        if (!oList.Exists(a => ((UserManagementLookup)a).IsChecked))
        {
            U.ShowMessage("Please select the Module you want to delete");
            return;
        }
        oList.RemoveAll(a => ((UserManagementLookup)a).IsChecked);
        if (oList.Count == 0) U.BindGrid(gvModule, new List<object> { new UserManagementLookup() });
        else U.BindGrid(gvModule, oList);
    }

    private void Submit()
    {
        string Result = Validation(new Wrapping());
        if (Result != "") Utility.ShowMessage(Result);
        else
        {
            Result = Save();
            if ($"{Result}".Contains("Error Message"))
            {
                Utility.ShowMessage(Result);
                return;
            }
            U.CloseUpdate();
        }
    }
    private string Validation(Wrapping w)
    {
        List<object> Users = U.GetGridData(gvUsers, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());        
        List<object> Operators = U.GetGridData(gvOperators, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
        List<object> Roles = U.GetGridData(gvRole, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
        List<object> Modules = U.GetGridData(gvModule, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());

        if (!ddlAllUsers.SelectedValue.ToBool() && Users.Count == 0 && !ddlAllOperators.SelectedValue.ToBool() && Operators.Count == 0) w.RequiredValidation = "Users or Operators";
        if (Roles.Count == 0 && Modules.Count == 0) w.RequiredValidation = "Roles or Modules";

        if (Modules.Count != 0)
        {
            if (ddlCreate.SelectedValue == "") w.RequiredValidation = "Create";
            if (ddlRead.SelectedValue == "") w.RequiredValidation = "Read";
            if (ddlUpdate.SelectedValue == "") w.RequiredValidation = "Update";
            if (ddlDelete.SelectedValue == "") w.RequiredValidation = "Delete";
            if (ddlViewAllData.SelectedValue == "") w.RequiredValidation = "View All Data";
            if (ddlDeviation.SelectedValue == "") w.RequiredValidation = "Deviation";
        }        
        return $"{w.Sb}";
    }
    private string Save()
    {
        string Result = "";

        #region User Management
        UserManagement o = new UserManagement();
        if (!U.Id.IsEmpty()) o = UserManagement.GetById(U.Id);
        o.AllUsers = ddlAllUsers.SelectedValue.ToBool();
        o.AllOperators = ddlAllOperators.SelectedValue.ToBool();
        o.AllModules = ddlAllModules.SelectedValue.ToBool();
        o.Create = ddlCreate.SelectedValue.ToBool();
        o.Read = ddlRead.SelectedValue.ToBool();
        o.Update = ddlUpdate.SelectedValue.ToBool();
        o.Delete = ddlDelete.SelectedValue.ToBool();
        o.ViewAllData = ddlViewAllData.SelectedValue.ToBool();
        o.Deviation = ddlDeviation.SelectedValue.ToBool();

        if (o.Id == 0)
        {
            o.CreatedBy = $"{ViewState[CNT.VS.Username]}";
            Result = o.Insert();
            if (Result.ContainErrorMessage()) return Result;
            o.Id = Result.ToInt();
        }
        else
        {
            o.ModifiedBy = $"{ViewState[CNT.VS.Username]}";
            Result = o.Change();
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion

        #region Users
        Result = UserManagementLookup.DeleteByParent("UserManagementUsers", o.Id);
        if (Result.ContainErrorMessage()) return Result;
        if (!ddlAllUsers.SelectedValue.ToBool())
        {            
            List<object> uList = U.GetGridData(gvUsers, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
            foreach (UserManagementLookup u in uList)
            {
                UserManagementUsers umu = new UserManagementUsers { UsersId = u.Id, UserManagementId = o.Id, CreatedBy = U.GetUsername() };
                Result = umu.Insert();
                if (Result.ContainErrorMessage()) return Result;
            }
        }
        #endregion

        #region Operators
        Result = UserManagementLookup.DeleteByParent("UserManagementOperators", o.Id);
        if (Result.ContainErrorMessage()) return Result;
        if (!ddlAllOperators.SelectedValue.ToBool())
        {            
            List<object> opList = U.GetGridData(gvOperators, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
            foreach (UserManagementLookup u in opList)
            {
                UserManagementOperators umo = new UserManagementOperators { OperatorsId = u.Id, UserManagementId = o.Id, CreatedBy = U.GetUsername() };
                Result = umo.Insert();
                if (Result.ContainErrorMessage()) return Result;
            }
        }        
        #endregion

        #region Role
        Result = UserManagementLookup.DeleteByParent("UserManagementRole", o.Id);
        if (Result.ContainErrorMessage()) return Result;
        List<object> rList = U.GetGridData(gvRole, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
        foreach (UserManagementLookup u in rList)
        {
            UserManagementRole umr = new UserManagementRole { RoleId = u.Id, UserManagementId = o.Id, CreatedBy = U.GetUsername() };
            Result = umr.Insert();
            if (Result.ContainErrorMessage()) return Result;
        }
        #endregion

        #region Module
        Result = UserManagementLookup.DeleteByParent("UserManagementModule", o.Id);
        if (Result.ContainErrorMessage()) return Result;
        if (!ddlAllModules.SelectedValue.ToBool())
        {            
            List<object> mList = U.GetGridData(gvModule, UserManagementLookup.MyType).ListData.FindAll(a => !((UserManagementLookup)a).Name.IsEmpty());
            foreach (UserManagementLookup u in mList)
            {
                UserManagementModule umm = new UserManagementModule { ModuleId = u.Id, UserManagementId = o.Id, CreatedBy = U.GetUsername() };
                Result = umm.Insert();
                if (Result.ContainErrorMessage()) return Result;
            }
        }        
        #endregion

        return Result;
    }
    private void Delete()
    {
        List<object> oList = UserManagement.GetByRole(CNT.Role.SuperAdmin);
        oList.RemoveAll(a => ((UserManagement)a).Id == U.Id.ToInt());
        if (oList.IsEmptyList())
        {
            U.ShowMessage("Delete failed, User Management must have at least one SuperAdmin Role");
            return;
        }
        string Result = UserManagement.Remove(U.Id);
        if (Result.ContainErrorMessage() ) U.ShowMessage($"Delete Failed, {Result}");
        else U.CloseUpdate("Data has been Delete Successfully");
    }
    #endregion        

    
}