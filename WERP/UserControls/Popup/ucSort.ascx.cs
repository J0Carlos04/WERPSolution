using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.Reflection;
using DAL;
using System.Text;

public partial class UserControls_Common_ucSort : System.Web.UI.UserControl
{
    #region Fields
    public string ClassName { get; set; }
    #endregion

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
            case "btnSearch":
                Search();
                break;
            case "btnAdd":
                Add();
                break;
            case "btnReset":
                U.BindGrid(gvData, new List<object> { new Sort { SortField = "a.Modified", SortDirection = "desc" } });
                break;
        }
    }
    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {
            case "imbDelete":
                Delete(imb);
                break;
        }
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl_SortField = (DropDownList)e.Row.FindControl("ddl_SortField");
            DropDownList ddl_SortDirection = (DropDownList)e.Row.FindControl("ddl_SortDirection");

            SetSortField(ddl_SortField);

            Sort s = (Sort)((List<object>)gvData.DataSource)[e.Row.RowIndex];
            ddl_SortField.SelectedValue = s.SortField;
            ddl_SortDirection.SelectedValue = s.SortDirection;
        }
    }   
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInitSorting();
    }
    private void SetInitSorting()
    {
        List<object> oList = new List<object> { new Sort { SortField = "a.Modified", SortDirection = "desc" } };
        if (Request.QueryString["SortData"] != null)
        {
            oList = new List<object>();
            string[] arrData = $"{Request.QueryString["SortData"]}".Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string Data in arrData)
            {
                string[] arrField = Data.Split('|');
                oList.Add(new Sort { SortField = arrField[0], SortDirection = arrField[1] });
            }
        }
        U.BindGrid(gvData, oList);
    }
    private void Search()
    {
        string SortBy = "";
        StringBuilder sb = new StringBuilder();
        foreach (GridViewRow Row in gvData.Rows)
        {
            DropDownList ddl_SortField = (DropDownList)Row.FindControl("ddl_SortField");
            DropDownList ddl_SortDirection = (DropDownList)Row.FindControl("ddl_SortDirection");

            SortBy = $"{SortBy}, {ddl_SortField.SelectedValue} {ddl_SortDirection.SelectedValue}";
            sb.AppendLine($"{ddl_SortField.SelectedValue}|{ddl_SortDirection.SelectedValue}");
        }
        if (SortBy.Substring(0, 1) == ",")
            SortBy = SortBy.Remove(0, 1);

        SortBy = $"{SortBy}~|{sb}";
        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference(SortBy));
    }
    private void SetSortField(DropDownList ddl)
    {
        ddl.Items.Clear();
        PropertyInfo[] props = U.GetType($"{U.ClassName}").GetProperties();
        foreach (PropertyInfo pi in props)
        {
            try
            {
                string SortName = $"{((ColumnAttribute)pi.GetCustomAttributes(false)[0]).SortName}";
                string Title = "";
                try { Title = $"{((ColumnAttribute)pi.GetCustomAttributes(false)[0]).Title}"; }
                catch (Exception exTitle) { }
                if (SortName != "")
                {
                    if (Title != "") ddl.Items.Add(new ListItem(Title, SortName));
                    else ddl.Items.Add(new ListItem(pi.Name, SortName));
                }                
            }
            catch (Exception ex) { }
        }
        ddl.Items.Add(new ListItem("Created", "a.Created"));
        ddl.Items.Add(new ListItem("Modified", "a.Modified"));
    }
    private void Add()
    {
        List<object> oList = U.GetGridData(gvData, typeof(Sort)).ListData;
        if (oList.Count == 0)
        {
            SetInitSorting();
            return;
        }
        oList.Add(new Sort());
        U.BindGrid(gvData, oList);
    }
    private void Delete(ImageButton imb)
    {
        GridViewRow row = (GridViewRow)imb.Parent.Parent;
        List<object> oList = U.GetGridData(gvData, typeof(Sort)).ListData;
        oList.RemoveAt(row.RowIndex);
        if (oList.Count == 0) SetInitSorting();
        else U.BindGrid(gvData, oList);
    }
    #endregion
}