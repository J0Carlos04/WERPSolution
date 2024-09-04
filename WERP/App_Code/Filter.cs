using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class Filter
{
    #region Properties
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "Seq")]
    public int Seq { get; set; }
    [Column(Field = "LogicalOperator")]
    public string LogicalOperator { get; set; }
    [Column(Field = "Field")]
    public string Field { get; set; }
    [Column(Field = "SearchName")]
    public string SearchName { get; set; }
    [Column(Field = "Operator")]
    public string Operator { get; set; }
    [Column(Field = "Value")]
    public string Value { get; set; }
    [Column(Field = "BoolValue")]
    public string BoolValue { get; set; }
    [Column(Field = "StartValue")]
    public string StartValue { get; set; }
    [Column(Field = "EndValue")]
    public string EndValue { get; set; }
    [Column(Field = "Priority")]
    public int Priority { get; set; }

    [Column(Field = "CreatedBy")]
    public string CreatedBy { get; set; }
    [Column(Field = "Created")]
    public DateTime Created { get; set; }
    [Column(Field = "ModifiedBy")]
    public string ModifiedBy { get; set; }
    [Column(Field = "Modified")]
    public DateTime Modified { get; set; }
    #endregion
}