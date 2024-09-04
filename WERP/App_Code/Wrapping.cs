using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using GemBox.Spreadsheet;
using Microsoft.Exchange.WebServices.Data;
using U = Utility;

public class Wrapping
{
    #region Standard Properties
    public object Data { get; set; }
    public List<object> ListData { get; set; } = new List<object>();    
    public object Result { get; set; }
    public string ErrorMessage { get; set; }

    private string _requiredValidation;
    public string RequiredValidation
    {
        get { return _requiredValidation; }
        set
        {
            _requiredValidation = value;
            U.SetRequiredMessage(this);
        }
    }
    private string _errorValidation;
    public string ErrorValidation
    {
        get { return _errorValidation; }
        set
        {
            _errorValidation = value;
            U.SetErrorValidation(this);
        }
    }
    #endregion

    #region Project Properties
    public int Id { get; set; }
    public int KeyId { get; set; }
    public int Seq { get; set; }
    public string Code { get; set; }
    public StringBuilder Sb { get; set; } = new StringBuilder();
    public string Field { get; set; }
    public object Value { get; set; }

    #region Excel
    public int Row { get; set; }
    public int Column { get; set; }
    public ExcelWorksheet Sheet { get; set; }
    public CellStyle CellStyle { get; set; }
    public bool IsNumber { get; set; }
    #endregion

    #region Email
    public object ProjectId { get; set; }
    public string Dear { get; set; }
    public string Subject { get; set; }
    public string Foreword { get; set; }
    public ExchangeService ES { get; set; }
    public EmailMessage Mail { get; set; }
    public string Path { get; set; }
    #endregion

    #region Methods
    public void SetExcel(object Value, bool IsNumber = false)
    {
        this.Value = Value;
        this.IsNumber = IsNumber;
        U.SetExcel(this);
    }
    #endregion

    #endregion
}