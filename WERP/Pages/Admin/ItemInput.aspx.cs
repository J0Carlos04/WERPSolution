using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.Web.Services.Description;
using System.IO;
using DAL;

public partial class Pages_ItemInput : PageBase
{   
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadFirstTime();
        }       
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
        if (!U.ViewAccess(Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath))) Response.Redirect($"../{CNT.Unauthorized}.aspx");        
    }
    private void SetInitControl()
    {
        U.SetDropDownMasterData(ddlCategory, "Category");
        U.SetDropDownMasterData(ddlGroup, "ItemGroup");
        U.SetDropDownMasterData(ddlBrand, "Brand");
        U.SetDropDownMasterData(ddlModel, "Model");
        U.SetDropDownMasterData(ddlMaterial, "Material");
        U.SetDropDownMasterData(ddlSpecs, "Specs");
        U.SetDropDownMasterData(ddlUOM, "UOM");
        if (U.Id == null)
        {
            ddlActive.SelectedValue = "True";
            ddlActive.Enabled = false;
        }
        btnCancel.OnClientClick = "parent.removeActiveTab();";
    }
    private void SetInitEdit()
    {
        if (U.Id.IsNull()) return;
        Goods g = Goods.GetById(U.Id);
        tbCode.Value = g.Code;
        tbName.Value = g.Name;
        tbDescription.Value = g.Description;
        ddlCategory.SelectedValue = $"{g.CategoryId}";
        if (ddlCategory.SelectedValue.IsEmpty()) SetInactiveValueDropDown(ddlCategory, "Category", g.CategoryId);
        ddlGroup.SelectedValue = $"{g.ItemGroupId}";
        if (ddlGroup.SelectedValue.IsEmpty()) SetInactiveValueDropDown(ddlGroup, "ItemGroup", g.ItemGroupId);
        ddlBrand.SelectedValue = $"{g.BrandId}";
        if (ddlBrand.SelectedValue.IsEmpty()) SetInactiveValueDropDown(ddlBrand, "Brand", g.BrandId);
        ddlModel.SelectedValue = $"{g.ModelId}";
        if (ddlModel.SelectedValue.IsEmpty()) SetInactiveValueDropDown(ddlModel, "Model", g.ModelId);
        ddlMaterial.SelectedValue = $"{g.MaterialId}";
        if (ddlMaterial.SelectedValue.IsEmpty()) SetInactiveValueDropDown(ddlMaterial, "Material", g.MaterialId);
        ddlSpecs.SelectedValue = $"{g.SpecsId}";
        if (ddlSpecs.SelectedValue.IsEmpty()) SetInactiveValueDropDown(ddlMaterial, "Specs", g.SpecsId);
        ddlUOM.SelectedValue = $"{g.UOMId}";
        if (ddlUOM.SelectedValue.IsEmpty()) SetInactiveValueDropDown(ddlUOM, "UOM", g.UOMId);
        tbSize.Value = g.Size;
        tbThreshold.Value = $"{g.Threshold}";
        cbUseSKU.Checked = g.UseSKU;
        ddlActive.SelectedValue = $"{g.Active}";
    }
    private void SetInactiveValueDropDown(DropDownList ddl, string Table, object Id)
    {
        MasterData md = MasterData.GetById(Id, Table);
        if (md.Id.IsNotZero())
        {
            ddl.Items.Add(new ListItem($"{md.Name}", $"{md.Id}"));
            ddl.SetValue(md.Id);
        }        
    }

    private void Submit()
    {
        string Result = Validation(new Wrapping());
        if (Result != "") U.ShowMessage(Result);
        else
        {
            Result = Save();
            if ($"{Result}".Contains("Error Message"))
            {
                U.ShowMessage(Result);
                return;
            }            
            F.Alert.Show("Data has been Save Successfully", String.Empty, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + Request.QueryString["parenttabid"] + "', '" + "Search" + "');");

        }
    }
    private string Validation(Wrapping w)
    {
        if (tbCode.Value.IsEmpty()) w.RequiredValidation = "Code";
        if (tbName.Value.IsEmpty()) w.RequiredValidation = "Name";
        if (ddlCategory.SelectedValue.IsEmpty()) w.RequiredValidation = "Category";

        if (ddlGroup.SelectedValue.IsEmpty()) w.RequiredValidation = "Group";
        if (ddlBrand.SelectedValue.IsEmpty()) w.RequiredValidation = "Brand";
        if (ddlModel.SelectedValue.IsEmpty()) w.RequiredValidation = "Model";
        if (ddlMaterial.SelectedValue.IsEmpty()) w.RequiredValidation = "Material";
        if (ddlSpecs.SelectedValue.IsEmpty()) w.RequiredValidation = "Specs";
        if (ddlUOM.SelectedValue.IsEmpty()) w.RequiredValidation = "UOM";
        if (tbSize.Value.IsEmpty()) w.RequiredValidation = "Size";
        if (tbThreshold.Value.IsEmpty()) w.RequiredValidation = "Threshold";
        return $"{w.Sb}";
    }
    private string Save()
    {
        Goods g = new Goods();
        g.Code = tbCode.Value;
        g.Name = tbName.Value;
        g.Description = tbDescription.Value;
        g.CategoryId = ddlCategory.SelectedValue.ToInt();
        g.CategoryId = ddlCategory.SelectedValue.ToInt();
        g.ItemGroupId = ddlGroup.SelectedValue.ToInt();
        g.BrandId = ddlBrand.SelectedValue.ToInt();
        g.ModelId = ddlModel.SelectedValue.ToInt();
        g.MaterialId = ddlMaterial.SelectedValue.ToInt();
        g.SpecsId = ddlSpecs.SelectedValue.ToInt();
        g.UOMId = ddlUOM.SelectedValue.ToInt();
        g.Size = tbSize.Value;
        g.Threshold = tbThreshold.Value.ToInt();
        g.UseSKU = cbUseSKU.Checked;
        g.Active = ddlActive.SelectedValue.ToBool();
        string Result = "";
        if (U.Id == null)
        {
            g.CreatedBy = $"{ViewState[CNT.VS.Username]}";
            Result = g.Insert();
            if (Result.ContainErrorMessage()) return Result;
            g.Id = Result.ToInt();
        }
        else
        {
            g.Id = U.Id.ToInt();
            g.ModifiedBy = $"{ViewState[CNT.VS.Username]}";
            Result = g.Update();
            if (Result.ContainErrorMessage()) return Result;
        }

        return "";
    }
    private void Delete()
    {
        string Result = DataAccess.IsDataExist(0, "ItemId", U.Id);
        if (!Result.IsEmpty())
        {
            U.ShowMessage($"Delete Failed, {Environment.NewLine}Data used in {Result}, please delete those data first");
            return;
        }
        Result = DataAccess.Delete(0, U.Id, "Id", "Item");
        if (Result.Contains("Error Message"))
        {
            U.ShowMessage(string.Format("Delete Failed, \\n{0}", Result));
            return;
        }
        U.CloseUpdate("Data has been Deleted Successfully");
    }
    #endregion
}