using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class DataLogger
{
    [Column(Field = "Station", SearchName = "c.label", SortName = "c.label")]
    public string Station { get; set; }
    [Column(Field = "FunctionLogger", SearchName = "b.label", SortName = "b.label")]
    public string FunctionLogger { get; set; }
    [Column(Field = "date", SearchName = "a.date", SortName = "a.date")]
    public DateTime date { get; set; }
    [Column(Field = "value", SearchName = "a.value", SortName = "a.value")]
    public double value { get; set; }

    [Column(Field = "No")]
    public double No { get; set; }
    public bool IsChecked { get; set; }
    public string Mode { get; set; }

    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "DataLoggerGetNotSentLatestData", pList, out TotalRow, new DataLogger());
    }
}