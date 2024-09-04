using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;

public partial class Pages_Admin_ItemDetail : PageBase
{
    #region Fields

    #endregion

    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadFirstTime();
        }
    }    
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInitControl();
        SetInitEdit();
    }
    private void SetInitControl()
    {        
        btnCancel.OnClientClick = "parent.removeActiveTab();";
    }
    private void SetInitEdit()
    {
        if (U.Id.IsNull()) return;
        Goods g = Goods.GetById(U.Id);
        tbCode.Value = g.Code;
        tbName.Value = g.Name;
        tbDescription.Value = g.Description;
        tbCategory.Value = g.Category;
        tbGroup.Value = g.ItemGroup;
        tbBrand.Value = g.Brand;
        tbModel.Value = g.Model;
        tbMaterial.Value = g.Material;
        tbSpecs.Value = g.Specs;
        tbUOM.Value = g.UOM;
        tbSize.Value = $"{g.Size}";
        tbThreshold.Value = $"{g.Threshold}";
        tbActive.Value = $"{g.Active}";
    }    
    #endregion
}