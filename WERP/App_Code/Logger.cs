using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAL;

public class Logger
{
    #region Properties
    [Column(Field = "numericInformation_id")]
    public string NumericInformationId { get; set; }    
    [Column(Field = "Function")]
    public string Function { get; set; }
    [Column(Field = "Totalizer")]
    public string Totalizer { get; set; }
    [Column(Field = "StartMeter")]
    public double StartMeter { get; set; }
    [Column(Field = "EndMeter")]
    public double EndMeter { get; set; }

    [Column(Field = "TotalStartMeter")]
    public double TotalStartMeter { get; set; }
    [Column(Field = "TotalEndMeter")]
    public double TotalEndMeter { get; set; }

    #region Cubication
    private double _cubication;
    [Column(Field = "Cubication")]
    public double Cubication
    {
        get { return EndMeter - StartMeter; }
        set { _cubication = value; }
    }
    #endregion

    #region Total Cubication Anggota
    private double _totalCubicationAnggota;
    [Column(Field = "TotalCubicationAnggota")]
    public double TotalCubicationAnggota
    {
        get { return TotalEndMeter - TotalStartMeter; }
        set { _totalCubicationAnggota = value; }
    }
    #endregion

    [Column(Field = "Unit")]
    public string Unit { get; set; }    
    public int FunctionLoggerId { get; set; }
    public int Id { get; set; }
    public int ParentId { get; set; }
    public int Level { get; set; }
    #endregion

    #region Methods
    public static List<object> GetTotalizerPCWin(DateTime dtStart, DateTime dtEnd)
    {
        //Select REPLACE(a.[label], 'Totalizer', '') Totalizer, a.numericInformation_id, a.unit, 
        //    (Select Top 1 value from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] >= '2022-05-31 05:30:00' order by [date] asc) StartMeter,	
        // (Select Top 1 date from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] >= '2022-05-31 05:30:00' order by [date] asc) StartDateTaken,	
        //    (Select Top 1 value from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] <= '2022-05-31 06:30:00' order by [date] desc) EndMeter, 
        // (Select Top 1 date from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] <= '2022-05-31 06:30:00' order by [date] desc) EndDateTaken 
        //From [NumericInformations] a 
        //where a.[label] like '%Totalizer%' and a.[label] not like '%Reverse%'

        //StringBuilder sb = new StringBuilder();
        //sb.AppendLine("Select REPLACE(a.[label], 'Totalizer', '') Totalizer, a.numericInformation_id, a.unit, ");
        //sb.AppendLine($"    (Select Top 1 value from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] >= '{dtStart.ToSQLDateTime()}' order by [date] asc) StartMeter,	");
        //sb.AppendLine($"    (Select Top 1 value from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] <= '{dtEnd.ToSQLDateTime()}' order by [date] desc) EndMeter ");	
        //sb.AppendLine("From [NumericInformations] a ");
        //sb.AppendLine("where a.[label] like '%Totalizer%' and a.[label] not like '%Reverse%' ");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Select REPLACE(a.[label], 'Totalizer', '') Totalizer, a.numericInformation_id, a.unit, ");
        sb.AppendLine($"    (Select Top 1 value from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] >= '{dtStart.ToSQLDateTime()}' and [date] <= '{dtEnd.ToSQLDateTime()}' and value <> 0 order by [date] asc) StartMeter,	");
        sb.AppendLine($"    (Select Top 1 value from ScadaNetDbArchive_1.dbo.ArchivedNumericInformations where numericInformation_id = a.numericInformation_id and [date] >= '{dtStart.ToSQLDateTime()}' and [date] <= '{dtEnd.ToSQLDateTime()}' and value <> 0 order by [date] desc) EndMeter ");
        sb.AppendLine("From [NumericInformations] a ");
        sb.AppendLine("where a.[label] like '%Totalizer%' and a.[label] not like '%Reverse%' ");
        return DataAccess.GetDataByQuery(2, $"{sb}", new Logger());
    }
    
    #endregion
}