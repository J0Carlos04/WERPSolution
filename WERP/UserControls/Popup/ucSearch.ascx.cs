using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using F = FineUI;
using U = Utility;

public partial class UserControls_Common_ucSearch : System.Web.UI.UserControl
{    
    #region Handlers
    public event System.EventHandler Searching;
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
            case "btnAddFilter":
                Add();
                break;
            case "btnReset":
                U.BindGrid(gvFilter, new List<object> { new Filter { Seq = 1, Priority = 1 } });
                break;
        }
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddl_Field":
                SetFilterControl(new Filter(), (GridViewRow)ddl.Parent.Parent);
                break;
            case "ddl_Operator":
                U.OperatorChanged(ddl, $"{U.ClassName}");
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
            case "imbSearch":
                Search();
                break;
        }
    }    
    protected void gvFilter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            #region Init Control
            DropDownList ddl_Logicaloperator = (DropDownList)e.Row.FindControl("ddl_Logicaloperator");
            DropDownList ddl_Operator = (DropDownList)e.Row.FindControl("ddl_Operator");
            DropDownList ddl_Field = (DropDownList)e.Row.FindControl("ddl_Field");
            DropDownList ddl_LookupParent = (DropDownList)e.Row.FindControl("ddl_LookupParent");
            TextBox tb_Value = (TextBox)e.Row.FindControl("tb_Value");
            TextBox tb_StartValue = (TextBox)e.Row.FindControl("tb_StartValue");
            TextBox tb_EndValue = (TextBox)e.Row.FindControl("tb_EndValue");
            DropDownList ddl_BoolValue = (DropDownList)e.Row.FindControl("ddl_BoolValue");
            DropDownList ddl_Priority = (DropDownList)e.Row.FindControl("ddl_Priority");            
            #endregion

            List<object> oList = (List<object>)gvFilter.DataSource;
            Filter o = (Filter)oList[e.Row.RowIndex];

            ddl_Logicaloperator.Visible = false;
            if (e.Row.RowIndex != 0)
            {
                ddl_Logicaloperator.Visible = true;
                ddl_Logicaloperator.SelectedValue = $"{o.LogicalOperator}";
            }
            U.SetField(ddl_Field);
            if ($"{o.Field}" != "")
                ddl_Field.SelectedValue = o.Field;

            SetFilterControl(o, e.Row);

            for (int i = 0; i < oList.Count; i++)
                ddl_Priority.Items.Add($"{i + 1}");
            if (o.Priority != 0) ddl_Priority.SelectedValue = $"{o.Priority}";
        }
    }
    protected void gvFilter_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    #endregion

    #region Methods
    private void LoadFirstTime()
    {
        SetInitFilter();
    }
    private void SetInitFilter()
    {
        List<object> oList = new List<object> { new Filter { Seq = 1, Priority = 1 } };
        if (Request.QueryString["SearchData"] != null)
        {
            oList = new List<object>();
            string[] arrData = $"{Request.QueryString["SearchData"]}".Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string Data in arrData)
            {
                string[] arrField = Data.Split('|');
                oList.Add(new Filter { LogicalOperator = arrField[0], SearchName = arrField[1], Field = arrField[2], Operator = arrField[3], Value = arrField[4], BoolValue = arrField[5], StartValue = arrField[6], EndValue = arrField[7], Priority = arrField[8].ToInt() });
            }
        }
        U.BindGrid(gvFilter, oList);
    }
    private void Search()
    {
        List<object> oList = U.GetGridData(gvFilter, typeof(Filter)).ListData;
        StringBuilder sb = new StringBuilder();
        foreach (Filter o in oList)
            sb.AppendLine($"{o.LogicalOperator}|{o.SearchName}|{o.Field}|{o.Operator}|{o.Value}|{o.BoolValue}|{o.StartValue}|{o.EndValue}|{o.Priority}");
        string Criteria = U.GetFilter(oList, $"{U.ClassName}");
        Criteria = $"{Criteria}~|{sb}";
        Session[CNT.Session.Criteria] = Criteria;        
        F.PageContext.RegisterStartupScript(F.ActiveWindow.GetHidePostBackReference());
    }
    private void Add()
    {
        List<object> oList = U.GetGridData(gvFilter, typeof(Filter)).ListData;
        if (oList.Count == 0)
        {
            SetInitFilter();
            return;
        }
        int MaxSeq = oList.Max(a => ((Filter)a).Seq) + 1;
        oList.Add(new Filter { Seq = MaxSeq, LogicalOperator = "AND", Field = "Code", Operator = "Like", Priority = MaxSeq });
        U.BindGrid(gvFilter, oList);
    }
    private void Delete(ImageButton imb)
    {
        GridViewRow row = (GridViewRow)imb.Parent.Parent;
        List<object> oList = U.GetGridData(gvFilter, typeof(Filter)).ListData;
        oList.RemoveAt(row.RowIndex);
        if (oList.Count == 0) U.BindGrid(gvFilter, new List<object> { new Filter { Seq = 1, Priority = 1 } });
        else U.BindGrid(gvFilter, oList);
    }
    
    private void SetFilterControl(Filter f, GridViewRow Row)
    {
        #region Init Control
        DropDownList ddl_Field = (DropDownList)Row.FindControl("ddl_Field");
        Label lbl_SearchName = (Label)Row.FindControl("lbl_SearchName");
        DropDownList ddl_Operator = (DropDownList)Row.FindControl("ddl_Operator");
        TextBox tb_Value = (TextBox)Row.FindControl("tb_Value");
        TextBox tb_StartValue = (TextBox)Row.FindControl("tb_StartValue");
        TextBox tb_EndValue = (TextBox)Row.FindControl("tb_EndValue");
        DropDownList ddl_BoolValue = (DropDownList)Row.FindControl("ddl_BoolValue");
        HtmlGenericControl dvValue = (HtmlGenericControl)Row.FindControl("dvValue");
        HtmlGenericControl dvStartValue = (HtmlGenericControl)Row.FindControl("dvStartValue");
        HtmlGenericControl dvEndValue = (HtmlGenericControl)Row.FindControl("dvEndValue");
        #endregion

        SetVisibilityFilterValue(Row, false);
        PropertyInfo pi = U.GetType(U.ClassName).GetProperty(ddl_Field.SelectedValue);
        lbl_SearchName.Text = $"{((ColumnAttribute)pi.GetCustomAttributes(false)[0]).SearchName}";
        string fn = U.GetDataTypeName($"{U.ClassName}", ddl_Field.SelectedValue);
        if (fn.Contains("string"))
        {
            dvValue.Visible = true;
            tb_Value.Text = f.Value;
            Utility.SetOperators(ddl_Operator, Item.GetOperatorsText());
            ddl_Operator.SelectedValue = $"{f.Operator}" == "" ? "Like" : f.Operator;
            tb_Value.TextMode = TextBoxMode.SingleLine;

        }
        else if (fn.Contains("bool"))
        {
            ddl_BoolValue.Visible = true;
            ddl_BoolValue.SelectedValue = f.BoolValue;
            Utility.SetOperators(ddl_Operator, Item.GetOperatorsBool());
            ddl_Operator.SelectedValue = $"{f.Operator}" == "" ? "=" : f.Operator;
        }
        else
        {
            if (fn.Contains("int") || fn.Contains("decimal"))
            {
                Utility.SetOperators(ddl_Operator, Item.GetOperatorsFilter());
                tb_Value.TextMode = TextBoxMode.SingleLine;
                tb_StartValue.TextMode = TextBoxMode.Number;
                tb_EndValue.TextMode = TextBoxMode.Number;
            }
            else if (fn.Contains("datetime"))
            {
                Utility.SetOperators(ddl_Operator, Item.GetOperatorsDateTime());
                tb_Value.TextMode = TextBoxMode.Date;
                tb_StartValue.TextMode = TextBoxMode.Date;
                tb_EndValue.TextMode = TextBoxMode.Date;
            }
            ddl_Operator.SelectedValue = $"{f.Operator}" == "" ? "Between" : f.Operator;
            OperatorChanged(ddl_Operator, $"{U.ClassName}");
        }
    }
    private void SetVisibilityFilterValue(GridViewRow Row, bool Visible)
    {
        DropDownList ddl_BoolValue = (DropDownList)Row.FindControl("ddl_BoolValue");
        HtmlGenericControl dvValue = (HtmlGenericControl)Row.FindControl("dvValue");
        HtmlGenericControl dvStartValue = (HtmlGenericControl)Row.FindControl("dvStartValue");
        HtmlGenericControl dvEndValue = (HtmlGenericControl)Row.FindControl("dvEndValue");

        ddl_BoolValue.Visible = Visible;
        dvValue.Visible = Visible;
        dvStartValue.Visible = Visible;
        dvEndValue.Visible = Visible;
    }
    public void OperatorChanged(DropDownList ddlOperator, string ClassName)
    {
        #region Init Control
        GridViewRow row = (GridViewRow)ddlOperator.Parent.Parent;
        DropDownList ddl_Field = (DropDownList)row.FindControl("ddl_Field");
        TextBox tb_Value = (TextBox)row.FindControl("tb_Value");
        TextBox tb_StartValue = (TextBox)row.FindControl("tb_StartValue");
        TextBox tb_EndValue = (TextBox)row.FindControl("tb_EndValue");
        HtmlGenericControl dvValue = (HtmlGenericControl)row.FindControl("dvValue");
        HtmlGenericControl dvStartValue = (HtmlGenericControl)row.FindControl("dvStartValue");
        HtmlGenericControl dvEndValue = (HtmlGenericControl)row.FindControl("dvEndValue");
        #endregion

        string fn = U.GetDataTypeName(ClassName, ddl_Field.SelectedValue);
        if (fn.Contains("string") || fn.Contains("bool")) return;

        SetVisibilityFilterValue(row, false);
        if (ddlOperator.SelectedValue == "Between")
        {
            dvStartValue.Visible = true;
            dvEndValue.Visible = true;
        }
        else
            dvValue.Visible = true;
    }
    #endregion    
}