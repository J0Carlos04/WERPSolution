using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

public class Rate : BaseModel
{
    #region Properties
    #region Fields
    [Column(Field = "Code", Native = true, Required = true, SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }
    [Column(Field = "Name", Native = true, Required = true, SearchName = "a.Name", SortName = "a.Name")]
    public string Name { get; set; }
    [Column(Field = "Active", Native = true, Required = true, SearchName = "a.Active", SortName = "a.Active")]
    public string Active { get; set; }
    #endregion
    #endregion

    #region Methods
    #region Get Data

    #endregion
    #region Change Data

    #endregion
    #endregion
}