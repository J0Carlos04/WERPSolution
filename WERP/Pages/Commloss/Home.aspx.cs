using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using U = Utility;
using F = FineUI;
using System.Web.UI.HtmlControls;

public partial class Pages_Commloss_Home : System.Web.UI.Page
{
    #region Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Initialize();
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        switch (ddl.ID)
        {
            case "ddlMonth":
                MonthYearChanged();
                ChartTypeChange();
                BindStationMetric();
                break;
            case "ddlChartType":
                ChartTypeChange();
                break;
        }
    }
    protected void Fbtn_Click(object sender, EventArgs e)
    {
        F.Button btn = (F.Button)sender;
        switch (btn.ID)
        {
            case "btnRevenueSaved":
                U.OpenNewTab($"Performance", $"Performance", $"getPerformanceUrl('{ddlMonth.SelectedValue}', '{tbYear.Text}')");
                break;
            case "btnActivity":
                U.OpenNewTab($"Activity", $"Activity", $"getActivityUrl('{ddlMonth.SelectedValue}', '{tbYear.Text}')");
                break;
            case "btnActivityPreviousMonth":
                U.OpenNewTab($"Activity", $"Activity", $"getActivityUrl('{ddlMonth.SelectedValue.ToInt() - 1}', '{tbYear.Text}')");
                break;
            case "btnActivityPerStation":                
                U.OpenNewTab($"Activity", $"Activity", $"getActivityPerStationUrl('{ddlMonth.SelectedValue}', '{tbYear.Text}', '{btn.Text}')");
                break;
        }
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        switch (btn.ID)
        {            
            case "btnActivityPerStation":
                U.OpenNewTab($"Activity", $"Activity", $"getActivityUrl('{ddlMonth.SelectedValue}', '{tbYear.Text}')");
                break;
        }
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal ltrlSiteNumber = (Literal)e.Row.FindControl("ltrlSiteNumber");
            HtmlGenericControl dvStation = (HtmlGenericControl)e.Row.FindControl("dvStation");
            F.Button btnActivityPerStation = (F.Button)e.Row.FindControl("btnActivityPerStation");
            btnActivityPerStation.Text = ltrlSiteNumber.Text;
            dvStation.Attributes.Add("onclick", $"ClientChanged('{btnActivityPerStation.ClientID}');");
        }
    }
    #endregion
    #region Methods
    private void Initialize()
    {
        InitControl();
    }
    private void InitControl()
    {
        ddlMonth.SetValue(DateTime.Now.Month);
        tbYear.Text = DateTime.Now.Year.ToString();
        ltrlMonthYear.Text = $"{U.GetShortMonth(DateTime.Now.Month)} {DateTime.Now.Year}";
        MonthYearChanged();
        ChartTypeChange();
        BindStationMetric();
    }
    private void MonthYearChanged()
    {
        PerformanceMetric pm = PerformanceMetric.GetByMonthYear(ddlMonth.SelectedValue, tbYear.Text);
        ltrlRevenueSaved.Text = pm.RevenueSaved.ToString("n0");
        ltrlVolumeSaved.Text = pm.VolumeSaved.ToString("n0");
        ltrl_CommercialSaved.Text = pm.CommercialSaved.ToString("n0");
        ltrlMonthlyRevenueLoss.Text = pm.RevenueLoss.ToString("n0");
        ltrlMonthlyOngoingIssue.Text = pm.OngoingIssued.ToString("n0");
        ltrlMonthlyVolumeLoss.Text = pm.VolumeLoss.ToString("n0");
        ltrlMonthlyFinishedIssued.Text = pm.FinishedIssued.ToString("n0");

    }
    private void ChartTypeChange()
    {
        #region Chart Configuration         
        cLosses.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;        
        Series s = new Series();
        s.ChartType = SeriesChartType.Line;
        s.Color = System.Drawing.ColorTranslator.FromHtml("#40669f");
        s.MarkerSize = 5;
        s.MarkerStyle = MarkerStyle.Circle;
        cLosses.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new System.Drawing.Font("Poppins", 6f);
        cLosses.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Poppins", 6f);
        cLosses.ChartAreas["ChartArea1"].AxisX.Minimum = 1; // Minimum value
        cLosses.ChartAreas["ChartArea1"].AxisX.Maximum = 12; // Maximum value

        for (int i = 1; i <= 12; i++)
            cLosses.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(i - 0.5, i + 0.5, U.GetShortMonth(i));        
        #endregion

        List<object> pmList = PerformanceMetric.GetByRevenueVolume(ddlChartType.SelectedValue, tbYear.Text);
        for (int i = 1; i <= 12; i++)
        {
            PerformanceMetric pm = (PerformanceMetric)pmList.Find(a => ((PerformanceMetric)a).MonthIssuedDate.Is(i));

            decimal TotalLoss = pm == null ? 0 : (ddlChartType.SelectedValue.Is("VolumeLoss") ? pm.VolumeLoss : pm.RevenueLoss);

            DataPoint dp = new DataPoint();
            dp.Label = $"{TotalLoss.ToString("n0")}";
            dp.Font = new System.Drawing.Font("Poppins", 6f);
            dp.XValue = i;
            dp.AxisLabel = U.GetShortMonth(i);
            dp.YValues[0] = TotalLoss.ToDouble();            

            // Add the data point only if the value is non-zero
            if (TotalLoss != 0)
            {                
                //VerticalLineAnnotation connectorLine = new VerticalLineAnnotation();
                //connectorLine.AxisX = cLosses.ChartAreas["ChartArea1"].AxisX;
                //connectorLine.AxisY = cLosses.ChartAreas["ChartArea1"].AxisY;
                //connectorLine.LineColor = Color.Gray; // Set the color of the line
                //connectorLine.LineWidth = 1; // Set the width of the line
                //connectorLine.AnchorX = i; // Set the X-value where the line should be anchored
                //connectorLine.AnchorY = TotalLoss.ToDouble(); // Set the Y-value where the line should be anchored
                //connectorLine.IsInfinitive = false; // Set to true for an infinite line
                //connectorLine.ClipToChartArea = "ChartArea1"; // Set the chart area to clip the line
                //connectorLine.LineDashStyle = ChartDashStyle.Dash; // Set the line style to Dash (dotted)
                s.Points.Add(dp);
                //cLosses.Annotations.Add(connectorLine); // Add the connector line to the chart
            }
            else
            {
                // Add an empty data point with zero value to maintain the X-axis labels
                s.Points.AddXY(i, double.NaN);
            }            
        }        
        cLosses.Series.Add(s);
    }
    private void BindStationMetric()
    {
        List<object> smList = Activity.GetByByMonthYear(ddlMonth.SelectedValue, tbYear.Text);
        U.BindGrid(gvData, smList);
    }
    #endregion
}