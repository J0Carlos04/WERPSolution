using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DAL;
using U = Utility;

[Serializable]
public class ShowColumn
{
    #region Properties
    #region Default
    [Column(Field = "Id")]
    public int Id { get; set; }    
    [Column(Field = "No")]
    public double No { get; set; }
    public bool IsChecked { get; set; }
    public string Mode { get; set; }
    #endregion

    #region Fields    
    [Column(Field = "Seq", Required = true, SearchName = "Seq")]
    public int Seq { get; set; }
    [Column(Field = "UserName", Required = true, SearchName = "UserName")]
    public string UserName { get; set; }
    [Column(Field = "ModuleName", Required = true, SearchName = "ModuleName")]
    public string ModuleName { get; set; }
    [Column(Field = "ColumnName", Required = true, SearchName = "UserName")]
    public string ColumnName { get; set; }
    [Column(Field = "Visible", Required = true, SearchName = "Visible")]
    public bool Visible { get; set; }
    #endregion   
    #endregion

    #region Methods

    #region Get Data
    public static ShowColumn GetById(object Id)
    {
        return (ShowColumn)DataAccess.GetSingleRowByQuery(0, $"Select * from ShowColumn where Id = '{Id}'", new ShowColumn());
    } 
    public static List<object> GetByUserName(object UserName, object ModuleName)
    {
        return DataAccess.GetDataByQuery(0, $"Select * from ShowColumn where UserName = '{UserName}' and ModuleName = '{ModuleName}' Order By Seq", new ShowColumn());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from ShowColumn", new ShowColumn());
    }        
    public static List<object> GetInitColumn(object ClassName, bool ViewAccess)
    {
        List<object> oList = new List<object>();
        Type t = Type.GetType($"{ClassName}");
        PropertyInfo[] props = t.GetProperties();
        foreach (PropertyInfo pi in props)
        {
            try
            {
                int GridColumnSeq = ((ColumnAttribute)pi.GetCustomAttributes(false)[0]).GridColumnSeq;
                if (GridColumnSeq == 0) continue;
                string Title = $"{U.GetAtributValue($"{ClassName}", pi.Name, "Title")}";
                object View = U.GetAtributValue($"{ClassName}", pi.Name, "ViewAccess");
                if (View != null && Convert.ToBoolean(View))
                {
                    if (ViewAccess)
                        oList.Add(new ShowColumn { Seq = GridColumnSeq, ColumnName = Title == "" ? pi.Name : Title, IsChecked = true });
                }
                else
                    oList.Add(new ShowColumn { Seq = GridColumnSeq, ColumnName = Title == "" ? pi.Name : Title, IsChecked = true });
            }
            catch (Exception ex) { }
        }
        return oList;
    }
    public static List<object> GetInitColumn(object ClassName, string Search, bool ViewAccess)
    {
        List<object> oList = new List<object>();
        Type t = Type.GetType($"{ClassName}");
        PropertyInfo[] props = t.GetProperties();
        foreach (PropertyInfo pi in props)
        {
            try
            {
                int GridColumnSeq = ((ColumnAttribute)pi.GetCustomAttributes(false)[0]).GridColumnSeq;
                if (GridColumnSeq == 0) continue;
                if (pi.Name.ToLower().Contains(Search.ToLower()))
                {
                    string Title = $"{U.GetAtributValue($"{ClassName}", pi.Name, "Title")}";
                    object View = U.GetAtributValue($"{ClassName}", pi.Name, "ViewAccess");
                    if (View != null && Convert.ToBoolean(View))
                    {
                        if (ViewAccess)
                            oList.Add(new ShowColumn { Seq = GridColumnSeq, ColumnName = Title == "" ? pi.Name : Title, IsChecked = true });
                    }
                    else
                        oList.Add(new ShowColumn { Seq = GridColumnSeq, ColumnName = Title == "" ? pi.Name : Title, IsChecked = true });
                }
            }
            catch (Exception ex) { }
        }
        return oList;
    }
    #endregion

    #region Change Data
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Seq", Seq));
        pList.Add(new clsParameter("UserName", UserName));
        pList.Add(new clsParameter("ModuleName", ModuleName));
        pList.Add(new clsParameter("ColumnName", ColumnName));
        pList.Add(new clsParameter("Visible", Visible));
        return DataAccess.Save(0, "ShowColumnInsert", pList);
    } 
    public static string DeleteByUserNameModule(object UserName, object ModuleName)
    {
        return DataAccess.ExecNonReturnValueByQuery(0, $"Delete ShowColumn where UserName = '{UserName}' and ModuleName = '{ModuleName}'");
    }
    #endregion

    #endregion
}