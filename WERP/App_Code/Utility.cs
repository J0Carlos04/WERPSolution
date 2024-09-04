using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using F = FineUI;
using Microsoft.Exchange.WebServices.Data;
using System.Security.Cryptography;
using DAL;
using System.Collections;
using System.Net.Mail;

/// <summary>
/// Summary description for Utility
/// </summary>
public static class Utility
{
    #region Properties 
    public static object ClassName { get { return HttpContext.Current.Request.QueryString["ClassName"]; } }
    private static object _parentTabId;
    public static object ParentTabId { get { return HttpContext.Current.Request.QueryString["parenttabid"]; } set { _parentTabId = value; } }
    private static object _id;
    public static object Id { get { return HttpContext.Current.Request.QueryString["Id"]; } set { _id = value; } }
    //StockReceivedId
    private static object _stockOrderId;
    public static object StockOrderId { get { return HttpContext.Current.Request.QueryString["StockOrderId"]; } set { _stockOrderId = value; } }
    private static object _stockReceivedId;
    public static object StockReceivedId { get { return HttpContext.Current.Request.QueryString["StockReceivedId"]; } set { _stockReceivedId = value; } }

    private static object _workOrderId;
    public static object WorkOrderId { get { return HttpContext.Current.Request.QueryString["WorkOrderId"]; } set { _workOrderId = value; } }
    private static object _stockReceivedItemId;
    public static object StockReceivedItemId { get { return HttpContext.Current.Request.QueryString["StockReceivedItemId"]; } set { _stockReceivedItemId = value; } }
    private static object _view;
    public static object View { get { return HttpContext.Current.Request.QueryString["View"]; } set { _view = value; } }
    private static object _scheduleId;
    public static object ScheduleId { get { return HttpContext.Current.Request.QueryString["ScheduleId"]; } set { _scheduleId = value; } }
    private static object _scheduleMaintenanceId;
    public static object ScheduleMaintenanceId { get { return HttpContext.Current.Request.QueryString["ScheduleMaintenanceId"]; } set { _scheduleMaintenanceId = value; } }
    private static object _stockOutItemId;
    public static object StockOutItemId { get { return HttpContext.Current.Request.QueryString["StockOutItemId"]; } set { _stockOutItemId = value; } }
    private static object _helpdeskId;
    public static object HelpdeskId { get { return HttpContext.Current.Request.QueryString["HelpdeskId"]; } set { _helpdeskId = value; } }
    private static object _tbl;
    public static object Tbl { get { return HttpContext.Current.Request.QueryString["Tbl"]; } set { _tbl = value; } }
    public static string QSSeq { get { return $"{HttpContext.Current.Request.QueryString["Seq"]}"; } }
    public static string QSWorkOrderId { get { return $"{HttpContext.Current.Request.QueryString["WorkOrderId"]}"; } }
    public static string QSCode { get { return $"{HttpContext.Current.Request.QueryString["Code"]}"; } }
    public static string QSWorkType { get { return $"{HttpContext.Current.Request.QueryString["WorkType"]}"; } }
    public static string QSStockOutId { get { return $"{HttpContext.Current.Request.QueryString["StockOutId"]}"; } }
    public static string QSStockOutItemId { get { return $"{HttpContext.Current.Request.QueryString["StockOutItemId"]}"; } }
    public static string QSShowStock { get { return $"{HttpContext.Current.Request.QueryString["ShowStock"]}"; } }
    public static string QSRowIndex { get { return $"{HttpContext.Current.Request.QueryString["RowIndex"]}"; } }
    public static string QSSelect { get { return $"{HttpContext.Current.Request.QueryString["Select"]}"; } }
    public static string QSMonth { get { return $"{HttpContext.Current.Request.QueryString["Month"]}"; } }
    public static string QSYear { get { return $"{HttpContext.Current.Request.QueryString["Year"]}"; } }
    public static string QSSiteNumber { get { return $"{HttpContext.Current.Request.QueryString["SiteNumber"]}"; } }
    public static string QSLookupType { get { return $"{HttpContext.Current.Request.QueryString["LookupType"]}"; } }
    public static string PathTempFolder { get { return $@"{HttpContext.Current.Server.MapPath("~")}\TempFiles\"; } }
    #endregion

    #region Extension
    public static bool IsNumericType(this object o)
    {
        switch (Type.GetTypeCode(o.GetType()))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
    public static bool IsNull(this object o)
    {
        if (o == null) return true;
        else return false;
    }
    public static bool ContainErrorMessage(this string o)
    {
        return o.Contains("Error Message");
    }
    public static bool ContainErrorMessage(this string o, out string ResultErrorMessage)
    {
        ResultErrorMessage = o;
        return o.Contains("Error Message");
    }
    public static int ToInt(this object o)
    {
        Int32.TryParse($"{o}", NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int Result);        
        return Result;
    }
    public static double ToDouble(this object o)
    {
        double Result = 0;
        double.TryParse($"{o}", out Result);
        return Result;
    }
    public static decimal ToDecimal(this object o)
    {
        decimal Result = 0;
        decimal.TryParse($"{o}", out Result);
        return Result;
    }
    public static string ToText(this object o) => $"{o}".Trim();   
    public static decimal NormalizeDecimal(this decimal value)
    {
        return value / 1.000000000000000000000000000000000m;
    }
    public static bool ToBool(this object o)
    {
        bool Result = false;
        try
        {
            if (o.ToText().Is("1")) return true;
            else if (o.ToText().Is("0")) return false;
            Result = Convert.ToBoolean(o);
        }
        catch (Exception ex)
        {

        }
        return Result;
    }
    public static bool IsEmpty(this object o) => $"{o}" == "";
    public static bool IsNotEmpty(this object o) => $"{o}" != "";
    public static bool IsEmptyList(this List<object> o)
    {
        return o.Count == 0 ? true : false;
    }
    public static char ToChar(this string o)
    {
        Char.TryParse(o, out char Result);
        return Result;
    }
    public static bool Is(this object o, object Value) => $"{o}" == $"{Value}";
    public static bool Or(this object o, object Value)
    {
        string[] arrList = $"{Value}".Split(',');
        return arrList.Contains($"{o}");
    }
    public static bool IsNot(this object o, object Value) => $"{o}" != $"{Value}";
    public static bool IsZero(this object o) => $"{o}" == "0" || $"{o}" == "";
    public static bool IsNotZero(this object o) => $"{o}" != "0";
    public static string NoSpace(this object o)
    {
        return $"{o}".Replace(" ", "");
    }
    public static string ToSQLDate(this DateTime o)
    {
        return $"{o.Year}-{o.Month.ToString("00")}-{o.Day.ToString("00")}";
    }
    public static string ToSQLDateTime(this DateTime o)
    {
        return $"{o.Year}-{o.Month.ToString("00")}-{o.Day.ToString("00")} {o.Hour.ToString("00")}:{o.Minute.ToString("00")}:{o.Second.ToString("00")}";
    }
    public static DateTime ToHTML5Date(this string o)
    {
        o = o.Replace("T", " ");
        return new DateTime(o.Substring(0, 4).ToInt(), o.Substring(5, 2).ToInt(), o.Substring(8, 2).ToInt());
    }
    public static DateTime ToHTML5DateTime(this string o)
    {
        o = o.Replace("T", " ");
        return new DateTime(o.Substring(0, 4).ToInt(), o.Substring(5, 2).ToInt(), o.Substring(8, 2).ToInt(), o.Substring(11, 2).ToInt(), o.Substring(14, 2).ToInt(), 0);
    }
    public static void SetValue(this DropDownList ddl, object Value)
    {
        try
        {
            ddl.SelectedValue = $"{Value}";
        }
        catch
        {
            ddl.SelectedIndex = 0;
        }
    }
    public static int Inc(this ref int o, int Value = 1) => o += Value;
    public static object ToTextDB(this object o) => $"{o}".IsEmpty() ? DBNull.Value : o;
    public static object ToIntDB(this object o) => o.ToInt().IsZero() ? DBNull.Value : o;
    #endregion

    #region Generic Function
    public static T GetById<T>(object id = null, string Table = "") where T : BaseModel, new()
    {
        if (id == null) id = Id;
        if (Table.IsEmpty()) Table = typeof(T).Name;
        return (T)DataAccess.GetSingleRowByQuery(0, $"SELECT * FROM {Table} WHERE Id = '{id}'", new T());
    }
    #endregion

    #region Common Function   
    public static void ShowMessage(string ErrorMessage, Control c)
    {
        ScriptManager.RegisterStartupScript(c, typeof(Page), "alert", string.Format("alert('{0}');", ErrorMessage.Replace("\r\n", "").Replace("'", @"\'")), true);
    }
    public static void ShowMessage(object ErrorMessage, F.Icon Icon = F.Icon.Error, string Title = "Error")
    {
        F.Alert alert = new F.Alert();
        alert.CssClass = "myalert";
        alert.Message = $"{ErrorMessage}";
        alert.Icon = Icon;
        alert.Title = Title;
        alert.Show();
    }   
    public static void ShowMessageDelete(string ErrorMessage)
    {
        ShowMessage($"Delete Failed : {ErrorMessage}");
    }
    public static bool ShowError(this bool o, object ErrorMessage, F.Icon Icon = F.Icon.Error, string Title = "Error")
    {
        if (o) ShowMessage(ErrorMessage, Icon, Title);
        return o;
    }    
    public static decimal TextToDecimal(string Text)
    {
        decimal Result = 0;
        try
        {
            Result = Convert.ToDecimal(Text);
        }
        catch { }
        return Result;
    }
    public static bool In<T>(this T t, params T[] values)
    {
        foreach (T value in values)
        {
            if (t.Equals(value))
            {
                return true;
            }
        }
        return false;
    }
    public static string GetUsername()
    {
        string Result = $"{HttpContext.Current.Session[CNT.Username]}";
        if (Result == "" && HttpContext.Current.Request.Cookies[CNT.Username] != null) Result = $"{HttpContext.Current.Request.Cookies[CNT.Username].Value}";
        return Result;
    }
    public static bool IsUser()
    {
        string Result = $"{HttpContext.Current.Session["IsUser"]}";
        if (Result == "" && HttpContext.Current.Request.Cookies["IsUser"] != null) Result = $"{HttpContext.Current.Request.Cookies[CNT.Username].Value}";
        return Result.ToBool();
    }
    public static object GetAtributValue(string ClassName, string PropName, string AtributeName)
    {
        object obj = null;
        try
        {
            obj = Type.GetType(ClassName).GetProperty(PropName).GetCustomAttributesData()[0].NamedArguments.FirstOrDefault(a => a.MemberName == AtributeName).TypedValue.Value;
        }
        catch (Exception ex)
        {
           
        }
        return obj; 
    }
    public static Type GetType(object ClassName)
    {
        return Type.GetType($"{ClassName}");
    }
    public static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double rad(double angle) => angle * 0.017453292519943295769236907684886127d; // = angle * Math.Pi / 180.0d
        double havf(double diff) => Math.Pow(Math.Sin(rad(diff) / 2d), 2); // = sin²(diff / 2)
        return 12745.6 * Math.Asin(Math.Sqrt(havf(lat2 - lat1) + Math.Cos(rad(lat1)) * Math.Cos(rad(lat2)) * havf(lon2 - lon1))); // earth radius 6.372,8‬km x 2 = 12745.6
    }    
    #endregion

    #region File
    public static string GetContentType(string FileName)
    {
        string Result = "";
        switch (Path.GetExtension(FileName).ToLower())
        {
            case ".txt":
                Result = @"text/plain";
                break;
            case ".jpeg":
                Result = "image/jpeg";
                break;
            case ".jpg":
                Result = "image/jpg";
                break;
            case ".png":
                Result = @"image/png";
                break;
            case ".pdf":
                Result = @"application/pdf";
                break;
            case ".doc":
                Result = @"application/msword";
                break;
            case ".docx":
                Result = @"application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;
            case ".xls":
                Result = @"application/vnd.ms-excel";
                break;
            case ".xlsx":
                Result = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ".ppt":
                Result = @"application/vnd.ms-powerpoint";
                break;
            case ".pptx":
                Result = @"application/vnd.openxmlformats-officedocument.presentationml.presentation";
                break;
            case ".ico":
                Result = @"image/x-icon";
                break;
            case ".bmp":
                Result = @"image/bmp";
                break;
            case ".mp4":
                Result = "video/mp4";
                break;
            case ".flv":
                Result = "video/x-flv";
                break;
            case ".3gp":
                Result = "video/3gpp";
                break;
            case ".mov":
                Result = "video/quicktime";
                break;
            case ".avi":
                Result = "video/x-msvideo";
                break;
            case ".wmv":
                Result = "video/x-ms-wmv";
                break;
        }
        return Result;
    }
    public static void OpenFile(string FileName, string ContentType, Byte[] Data)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = false;
        HttpContext.Current.Response.ContentType = ContentType;
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", FileName));
        HttpContext.Current.Response.BinaryWrite(Data);
        HttpContext.Current.Response.End();

    }
    public static void Upload(FileUpload fu, Label lbl, LinkButton lb)
    {
        lbl.Text = Convert.ToBase64String(fu.FileBytes);
        lb.Text = fu.FileName;
    }
    public static void UploadSingleFile(FileUpload fu, LinkButton lb)
    {
        if (!fu.HasFile)
        {
            ShowMessage("File is required");
            return;
        }
        Attachment att = new Attachment { FileName = fu.FileName, FileNameUniq = $"{DateTime.Now.ToSQLDateTime().Replace(":", "-")}_{fu.FileName}" };
        string Path = $"{PathTempFolder}{att.FileNameUniq}";
        File.WriteAllBytes(Path, fu.FileBytes);
        lb.Text = att.FileName;
        lb.CommandName = att.FileNameUniq;
    }
    public static void DeleteSingleFile(LinkButton lb)
    {
        string Path = $"{PathTempFolder}{lb.CommandName}";
        if (File.Exists(Path))
        {
            File.SetAttributes(Path, FileAttributes.Normal);
            File.Delete(Path);
        }
        lb.Text = "";
        lb.CommandName = "";
    }
    public static byte[] GetFile(string FileName)
    {        
        string Path = $"{PathTempFolder}{FileName}";        
        return File.ReadAllBytes(Path);        
    }
    public static void SaveAttachmentToTempFolder(Attachment att)
    {
        if (att.FileName == CNT.DataNotAvailable) return;
        att.FileNameUniq = $"{DateTime.Now.ToSQLDateTime().Replace(":", "-")}_{att.FileName}";
        string Path = $"{PathTempFolder}{att.FileNameUniq}";
        File.WriteAllBytes(Path, att.Data);
    }
    public static void DownloadFile(Button btn, string lbFileName)
    {
        GridViewRow Row = (GridViewRow)btn.Parent.Parent;
        LinkButton lb = (LinkButton)Row.FindControl(lbFileName);
        Literal ltrl_FileNameUniq = (Literal)Row.FindControl("ltrl_FileNameUniq");
        if (ltrl_FileNameUniq.Text == "") return;
        byte[] Data = GetFile(ltrl_FileNameUniq.Text);
        OpenFile(lb.Text, GetContentType(lb.Text), Data);
    }
    public static void AddAttachment(FileUpload fu, GridView gv)
    {
        if (!fu.HasFile)
        {
            ShowMessage("File is required");
            return;
        }
        
        List<object> attList = GetGridData(gv, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).FileName != CNT.DataNotAvailable);
        Attachment att = new Attachment();
        att.Seq = attList.Count == 0 ? 1 : attList.Count + 1;
        att.FileName = fu.FileName;
        att.FileNameUniq = $"{DateTime.Now.ToSQLDateTime().Replace(":", "-")}_{fu.FileName}";
        attList.Add(att);
        string Path = $"{PathTempFolder}{att.FileNameUniq}";
        File.WriteAllBytes(Path, fu.FileBytes);
        Utility.BindGrid(gv, attList);
    }
    public static void DeleteAttachment(ImageButton imb)
    {
        try
        {
            GridViewRow row = (GridViewRow)imb.Parent.Parent;
            Literal ltrl_Seq = (Literal)row.FindControl("ltrl_Seq");
            GridView gv = (GridView)row.Parent.Parent;

            List<object> aList = GetGridData(gv, typeof(Attachment)).ListData;
            Attachment att = (Attachment)aList[row.RowIndex];
            string Path = $"{PathTempFolder}{att.FileNameUniq}";
            if (File.Exists(Path))
            {
                File.SetAttributes(Path, FileAttributes.Normal);
                File.Delete(Path);
            }
            aList.RemoveAll(a => ((Attachment)a).Seq.ToString() == ltrl_Seq.Text);
            if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
            Utility.BindGrid(gv, aList);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }
    public static void DeleteAttachment(GridView gv)
    {
        try
        {
            List<object> aList = GetGridData(gv, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).FileName != CNT.DataNotAvailable);
            if (!aList.Exists(a => ((Attachment)a).IsChecked))
            {
                ShowMessage("Please select the Attachment you want to delete");
                return;
            }
            foreach (Attachment att in aList)
            {
                if (!att.IsChecked) continue;
                string Path = $"{PathTempFolder}{att.FileNameUniq}";
                if (File.Exists(Path))
                {
                    File.SetAttributes(Path, FileAttributes.Normal);
                    File.Delete(Path);
                }                
            }
            aList.RemoveAll(a => ((Attachment)a).IsChecked);

            if (aList.Count == 0) aList.Add(new Attachment { Seq = 0, FileName = CNT.DataNotAvailable });
            Utility.BindGrid(gv, aList);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }
    #endregion    

    #region Text Manipulation
    public static string TitleCase(string Text)
    {
        string Result = "";
        Result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Text.ToLower());

        return Result;
    }
    public static string RemoveDecimalTrailingZero(decimal? d)
    {
        if (d == null) return "";
        char point = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
        string s = d.ToString();
        s = Convert.ToDecimal(d).ToString(string.Format("n{0}", s.SkipWhile(c => c != '.').Skip(1).Count()));
        if (!s.Contains(point))
            return s;
        return s.TrimEnd('0').TrimEnd(point);
    }
    public static string RemoveHTMLTag(string Text)
    {
        Text = HtmlRemoval.StripTagsRegex(Text);
        Text = HtmlRemoval.StripTagsRegexCompiled(Text);
        Text = HtmlRemoval.StripTagsCharArray(Text);
        Text = Text.Replace("\r\n", "");
        return Text;
    }
    public static string EncryptString(string Message)
    {
        string Passphrase = ConfigurationManager.AppSettings["phs"];
        byte[] Results;
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Step 1. We hash the passphrase using MD5
        // We use the MD5 hash generator as the result is a 128 bit byte array
        // which is a valid length for the TripleDES encoder we use below

        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Create a new TripleDESCryptoServiceProvider object
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Setup the encoder
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert the input string to a byte[]
        byte[] DataToEncrypt = UTF8.GetBytes(Message);

        // Step 5. Attempt to encrypt the string
        try
        {
            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
        }
        finally
        {
            // Clear the TripleDes and Hashprovider services of any sensitive information
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }

        // Step 6. Return the encrypted string as a base64 encoded string
        return Convert.ToBase64String(Results);
    }
    public static string DecryptString(string Message)
    {
        string Passphrase = ConfigurationManager.AppSettings["phs"];
        byte[] Results;
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Step 1. We hash the passphrase using MD5
        // We use the MD5 hash generator as the result is a 128 bit byte array
        // which is a valid length for the TripleDES encoder we use below

        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Create a new TripleDESCryptoServiceProvider object
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Setup the decoder
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert the input string to a byte[]
        byte[] DataToDecrypt = Convert.FromBase64String(Message);

        // Step 5. Attempt to decrypt the string
        try
        {
            ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
        }
        finally
        {
            // Clear the TripleDes and Hashprovider services of any sensitive information
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }

        // Step 6. Return the decrypted string in UTF8 format
        return UTF8.GetString(Results);
    }
    public static string SpelledOut(Decimal d)
    {
        string[] _satuan = new[] { "nol", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan" };
        string[] _belasan = new[] { "sepuluh", "sebelas", "dua belas", "tiga belas", "empat belas", "lima belas", "enam belas", "tujuh belas", "delapan belas", "sembilan belas" };
        string[] _puluhan = new[] { "", "", "dua puluh", "tiga puluh", "empat puluh", "lima puluh", "enam puluh", "tujuh puluh", "delapan puluh", "sembilan puluh" };
        string[] _ribuan = new[] { "", "ribu", "juta", "milyar", "triliyun" };

        string strResult = "";
        Decimal frac = d - Decimal.Truncate(d);
        if (Decimal.Compare(frac, 0.0m) != 0) strResult = SpelledOut(Decimal.Round(frac * 100)) + " sen";

        string strTemp = Decimal.Truncate(d).ToString();
        for (int i = strTemp.Length; i > 0; i--)
        {
            int nDigit = Convert.ToInt32(strTemp.Substring(i - 1, 1));
            int nPos = (strTemp.Length - i) + 1;
            switch (nPos % 3)
            {
                case 1:
                    bool bAllZeros = false;
                    string tmpBuff;
                    if (i == 1) tmpBuff = _satuan[nDigit] + " ";
                    else if (strTemp.Substring(i - 2, 1) == "1") tmpBuff = _belasan[nDigit] + " ";
                    else if (nDigit > 0) tmpBuff = _satuan[nDigit] + " ";
                    else
                    {
                        bAllZeros = true;
                        if (i > 1)
                        {
                            if (strTemp.Substring(i - 2, 1) != "0") bAllZeros = false;
                        }
                        if (i > 2)
                        {
                            if (strTemp.Substring(i - 3, 1) != "0") bAllZeros = false;
                        }
                        tmpBuff = "";
                    }

                    if ((!bAllZeros) && (nPos > 1))
                    {
                        if ((strTemp.Length == 4) && (strTemp.Substring(0, 1) == "1"))
                        {
                            tmpBuff = "se" + _ribuan[(int)Decimal.Round(nPos / 3m)] + " ";
                        }
                        else
                        {
                            tmpBuff += _ribuan[(int)Decimal.Round(nPos / 3)] + " ";
                        }
                    }
                    strResult = tmpBuff + strResult;
                    break;
                case 2:
                    if (nDigit > 0) strResult = _puluhan[nDigit] + " " + strResult;
                    break;
                case 0:
                    if (nDigit > 0)
                    {
                        if (nDigit == 1) strResult = "seratus " + strResult;
                        else strResult = _satuan[nDigit] + " ratus " + strResult;
                    }
                    break;
            }
        }

        strResult = strResult.Trim().ToLower();
        if (strResult.Length > 0) strResult = strResult.Substring(0, 1).ToUpper() + strResult.Substring(1, strResult.Length - 1);

        return strResult;
    }
    public static string ToWords(this decimal value, string CurrencyText)
    {
        string decimals = "";
        string input = Math.Round(value, 2).ToString();

        if (input.Contains("."))
        {
            decimals = input.Substring(input.IndexOf(".") + 1);
            // remove decimal part from input
            input = input.Remove(input.IndexOf("."));
        }

        // Convert input into words. save it into strWords
        string strWords = GetWords(input) + " " + CurrencyText;


        if (decimals.Length > 0)
        {
            // if there is any decimal part convert it to words and add it to strWords.
            strWords += " and " + GetWords(decimals) + " Cents";
        }

        return strWords;
    }
    private static string GetWords(string input)
    {
        // these are seperators for each 3 digit in numbers. you can add more if you want convert beigger numbers.
        string[] seperators = { "", " Thousand ", " Million ", " Billion " };

        // Counter is indexer for seperators. each 3 digit converted this will count.
        int i = 0;

        string strWords = "";

        while (input.Length > 0)
        {
            // get the 3 last numbers from input and store it. if there is not 3 numbers just use take it.
            string _3digits = input.Length < 3 ? input : input.Substring(input.Length - 3);
            // remove the 3 last digits from input. if there is not 3 numbers just remove it.
            input = input.Length < 3 ? "" : input.Remove(input.Length - 3);

            int no = int.Parse(_3digits);
            // Convert 3 digit number into words.
            _3digits = GetWord(no);

            // apply the seperator.
            _3digits += seperators[i];
            // since we are getting numbers from right to left then we must append resault to strWords like this.
            strWords = _3digits + strWords;

            // 3 digits converted. count and go for next 3 digits
            i++;
        }
        return strWords;
    }
    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }    
    private static string GetWord(int no)
    {
        string[] Ones =
        {
            "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
            "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen"
        };

        string[] Tens = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };

        string word = "";

        if (no > 99 && no < 1000)
        {
            int i = no / 100;
            word = word + Ones[i - 1] + " Hundred ";
            no = no % 100;
        }

        if (no > 19 && no < 100)
        {
            int i = no / 10;
            word = word + Tens[i - 1] + " ";
            no = no % 10;
        }

        if (no > 0 && no < 20)
        {
            word = word + Ones[no - 1];
        }

        return word;
    }
    #endregion

    #region Filter
    public static void SetOperators(DropDownList ddl, List<Item> Items)
    {
        ddl.Items.Clear();
        foreach (Item item in Items)
            ddl.Items.Add(new ListItem(item.Text, item.Value));
    }       
    public static void OperatorChanged(DropDownList ddlOperator, string ClassName)
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

        string fn = GetDataTypeName(ClassName, ddl_Field.SelectedValue);
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
    public static void SetField(DropDownList ddl)
    {
        ddl.Items.Clear();
        Type t = Type.GetType($"{ClassName}");
        PropertyInfo[] props = t.GetProperties();
        foreach (PropertyInfo pi in props.OrderBy(a => a.Name))
        {
            try
            {
                string SearchName = $"{((ColumnAttribute)pi.GetCustomAttributes(false)[0]).SearchName}";
                string Title = "";
                try { Title = $"{((ColumnAttribute)pi.GetCustomAttributes(false)[0]).Title}"; }
                catch (Exception exTitle) { }
                if (SearchName != "")
                {
                    if (!((ColumnAttribute)pi.GetCustomAttributes(false)[0]).ViewAccess)
                    {
                        if (Title != "") ddl.Items.Add(new ListItem(Title, pi.Name));
                        else ddl.Items.Add(new ListItem(pi.Name));
                    }
                }
            }
            catch (Exception ex) { }

        }
    }
    private static void SetVisibilityFilterValue(GridViewRow Row, bool Visible)
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
    public static string GetDataTypeName(string ClassName, string FieldName)
    {
        PropertyInfo pi = Type.GetType(ClassName).GetProperty(FieldName);
        return pi.PropertyType.FullName.ToLower();
    }
    public static string GetFilter(List<object> oList, string ClassName)
    {
        string Result = "";
        List<int> Priorities = new List<int>();
        foreach (Filter f in oList)
        {
            if (!Priorities.Exists(a => a == f.Priority))
                Priorities.Add(f.Priority);
        }
        foreach (int Priority in Priorities)
        {
            int idx = 0;
            int Total = oList.Count(a => ((Filter)a).Priority == Priority);
            foreach (Filter f in oList.FindAll(a => ((Filter)a).Priority == Priority).OrderBy(b => ((Filter)b).Seq))
            {
                idx += 1;
                Result = $"{Result} {f.LogicalOperator} ";
                if (idx == 1) Result = $"{Result}(";

                string PercentSymbol = f.Operator.Contains("Like") ? "%" : "";

                string fn = GetDataTypeName(ClassName, f.Field);
                #region String
                if (fn.Contains("string"))
                {
                    if (f.Operator.Contains("IN"))
                    {
                        string[] arrValue = f.Value.Split(',');
                        string Value = "";
                        foreach (string v in arrValue)
                            Value = $"{Value},'{v}'";
                        Value = Value.Remove(0, 1);
                        Result = $"{Result} {f.SearchName} {f.Operator} ({Value}) ";
                    }
                    else
                        Result = $"{Result} {f.SearchName} {f.Operator} '{PercentSymbol}{f.Value}{PercentSymbol}' ";
                }
                #endregion
                #region Numeric
                else if (fn.Contains("int") || fn.Contains("decimal"))
                {
                    if (f.Value == "") f.Value = "0";
                    if (f.Operator.Contains("IN"))
                    {
                        string[] arrValue = f.Value.Split(',');
                        string Value = "";
                        for (int i = 0; i < arrValue.Length; i++)
                        {
                            if (arrValue[i] == "") arrValue[i] = "0";
                            Value = $"{Value},'{arrValue[i]}'";
                        }
                        Value = Value.Remove(0, 1);
                        Result = $"{Result} {f.SearchName} {f.Operator} ({Value}) ";
                    }
                    else if (f.Operator == "Between")
                    {
                        if (f.StartValue == "") f.StartValue = "0";
                        if (f.EndValue == "") f.EndValue = "0";
                        Result = $"{Result} {f.SearchName} {f.Operator} '{f.StartValue}' AND '{f.EndValue}' ";
                    }
                    else
                        Result = $"{Result} {f.SearchName} {f.Operator} '{PercentSymbol}{f.Value}{PercentSymbol}' ";
                }
                #endregion
                #region Date
                else if (fn.Contains("datetime"))
                {
                    if (f.Operator.Contains("IN"))
                    {
                        string[] arrValue = f.Value.Split(',');
                        string Value = "";
                        foreach (string v in arrValue)
                        {
                            DateTime dt = Utility.GetDate(v);
                            string Date = $"{dt.Year}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}";
                            Value = $"{Value},'{Date}'";
                        }
                        Value = Value.Remove(0, 1);
                        Result = $"{Result} {f.SearchName} {f.Operator} ({Value}) ";
                    }
                    else if (f.Operator == "Between")
                    {
                        DateTime dtStart = Utility.GetDate(f.StartValue);
                        DateTime dtEnd = Utility.GetDate(f.EndValue);

                        string StartDate = $"{dtStart.Year}-{dtStart.Month.ToString("00")}-{dtStart.Day.ToString("00")}";
                        string EndDate = $"{dtEnd.Year}-{dtEnd.Month.ToString("00")}-{dtEnd.Day.ToString("00")}";

                        Result = $"{Result} {f.SearchName} {f.Operator} '{StartDate}' AND '{EndDate}' ";
                    }
                    else
                    {
                        DateTime dt = Utility.GetDate(f.Value);
                        string Date = $"{dt.Year}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}";
                        Result = $"{Result} {f.SearchName} {f.Operator} '{Date}' ";
                    }
                }
                #endregion
                #region Bool
                else if (fn.Contains("bool"))
                {
                    int Value = $"{f.BoolValue}".ToLower() == "true" ? 1 : 0;
                    Result = $"{Result} {f.SearchName} {f.Operator} {Value} ";
                }
                #endregion

                if (idx == Total) Result = $"{Result})";
            }
        }
        if (Result != "") Result = $" Where {Result}";
        return Result;
    }
    #endregion

    #region Converter                   
    public static DateTime GetDate(string Text)
    {
        DateTime dt = new DateTime();
        try { dt = DateTime.ParseExact(Text, "dd/MM/yyyy", CultureInfo.InvariantCulture); }
        catch
        {
            try { dt = DateTime.ParseExact(Text, "yyyy-MM-dd", CultureInfo.InvariantCulture); }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }            
        }
        return dt;
    }    
    #endregion

    #region Validation    
    public static string SingleRequiredValidation(object obj)
    {
        int Seq = 0;
        StringBuilder sb = new StringBuilder();

        #region Mandatory Validation
        PropertyInfo[] props = obj.GetType().GetProperties();
        foreach (PropertyInfo prop in props)
        {
            if (prop.GetCustomAttributes(false).Count() == 0) continue;
            if (((ColumnAttribute)prop.GetCustomAttributes(false)[0]).Required)
            {
                if ($"{prop.GetValue(obj, null)}" == "" || (($"{prop}".ToLower().Contains("int") || $"{prop}".ToLower().Contains("decimal")) && $"{prop.GetValue(obj, null)}" == "0"))
                {
                    Seq += 1;
                    sb.AppendFormat($"{Seq}. {prop.Name} is required{Environment.NewLine}");
                }
            }
        }
        #endregion

        return sb.ToString();
    }    
    public static string MultipleRequiredValidation(List<object> oList)
    {
        int Seq = 0;
        StringBuilder sb = new StringBuilder();
        foreach (object obj in oList)
        {
            Seq += 1;
            PropertyInfo[] props = obj.GetType().GetProperties();
            string ErrorMessage = $"Line {Seq} : ";
            bool IsErrorExist = false;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttributes(false).Count() == 0) continue;
                if (((ColumnAttribute)prop.GetCustomAttributes(false)[0]).Required)
                {
                    if ($"{prop.GetValue(obj, null)}" == "" || (($"{prop}".ToLower().Contains("int") || $"{prop}".ToLower().Contains("decimal")) && $"{prop.GetValue(obj, null)}" == "0"))
                    {
                        ErrorMessage = $"{ErrorMessage}{prop.Name}, ";
                        IsErrorExist = true;
                    }
                }
            }
            if (IsErrorExist)
            {
                ErrorMessage = $"{ErrorMessage.Remove(ErrorMessage.Length - 2, 2)} is required";
                sb.AppendLine(ErrorMessage);
            }
        }
        return sb.ToString();
    }
    public static void SetRequiredMessage(Wrapping w)
    {
        w.Seq += 1;
        w.Sb.AppendFormat("{0}. {1}{2}", w.Seq, $"{w.RequiredValidation} is required", Environment.NewLine);
    }
    public static void SetErrorValidation(Wrapping w)
    {
        w.Seq += 1;
        w.Sb.AppendFormat("{0}. {1}{2}", w.Seq, $"{w.ErrorValidation}", Environment.NewLine);
    }
    public static string AttachmentValidation(GridView gv, out List<object> oList)
    {
        oList = Utility.GetGridData(gv, typeof(Attachment)).ListData.FindAll(a => ((Attachment)a).FileName != CNT.DataNotAvailable);
        foreach (Attachment att in oList)
        {
            try { att.Data = GetFile(att.FileNameUniq); }
            catch { return "Error Message : Your page is expired please reload with klik Button Refresh"; }
        }
        return "";
    }
    #endregion

    #region Grid View
    public static void BindGrid(GridView gv, object obj)
    {
        gv.DataSource = obj;
        gv.DataBind();

        SetHeaderGrid(gv);
    }
    public static void BindGridF(F.Grid gv, object obj)
    {
        gv.DataSource = obj;
        gv.DataBind();
    }
    private static void SetHeaderGrid(GridView gv)
    {
        try
        {
            if (gv.DataSource == null) return;

            HttpCookie themeCookie = HttpContext.Current.Request.Cookies["Theme_v6"];
            if (themeCookie == null) return;

            switch ($"{themeCookie.Value}".ToLower())
            {
                case "blue":
                    gv.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e9fd");
                    gv.HeaderRow.ForeColor = System.Drawing.Color.Black;
                    break;
                case "gray":
                    gv.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d0d0d0");
                    gv.HeaderRow.ForeColor = System.Drawing.Color.Black;
                    break;
                case "neptune":
                    gv.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#157fcc");
                    gv.HeaderRow.ForeColor = System.Drawing.Color.White;
                    break;
                case "triton":
                    gv.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f6f6");
                    gv.HeaderRow.ForeColor = System.Drawing.Color.Black;
                    break;
                case "crisp":
                    gv.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#ececec");
                    gv.HeaderRow.ForeColor = System.Drawing.Color.Black;
                    break;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public static Wrapping GetGridData(GridView gv, Type t)
    {
        Wrapping w = new Wrapping();
        PropertyInfo[] props = t.GetProperties();
        foreach (GridViewRow row in gv.Rows)
        {
            try
            {
                w.Data = Activator.CreateInstance(t);
                foreach (TableCell cel in row.Cells)
                {
                    w.ErrorMessage = ((TemplateField)((DataControlFieldCell)(cel)).ContainingField).HeaderText;
                    foreach (Control c in cel.Controls)
                    {
                        try
                        {
                            if (c.GetType().Name == "HtmlGenericControl")
                            {
                                foreach (Control cItem in GetControlInDiv(c))
                                    w.Data = SetProperties(props, w.Data, cItem);
                            }
                            else if (c.GetType().Name == "HtmlTable")
                            {
                                foreach (Control cItem in GetControlInTable(c))
                                    w.Data = SetProperties(props, w.Data, cItem);
                            }
                            else if (c.GetType().Name == "UpdatePanel")
                            {
                                foreach (Control cItem in GetControlInUpdatePanel(c))
                                    w.Data = SetProperties(props, w.Data, cItem);
                            }
                            else if (c.ID == null || c.ID.Split('_').Length <= 1) continue;
                            else w.Data = SetProperties(props, w.Data, c);
                        }
                        catch (Exception ex1)
                        {
                            w.ErrorMessage = $"Error in {w.ErrorMessage} : {ex1.Message}";
                        }

                    }
                }
                w.ListData.Add(w.Data);
            }
            catch (Exception ex)
            {
                w.ErrorMessage = $"Error in {w.ErrorMessage} : {ex.Message}";
            }
        }

        return w;
    }
    public static Wrapping GetGridData(GridView gv, Type t, out List<object> oList)
    {
        Wrapping w = new Wrapping();
        oList = new List<object>();
        PropertyInfo[] props = t.GetProperties();
        foreach (GridViewRow row in gv.Rows)
        {
            try
            {
                w.Data = Activator.CreateInstance(t);
                foreach (TableCell cel in row.Cells)
                {
                    w.ErrorMessage = ((TemplateField)((DataControlFieldCell)(cel)).ContainingField).HeaderText;
                    foreach (Control c in cel.Controls)
                    {
                        try
                        {
                            if (c.GetType().Name == "HtmlGenericControl")
                            {
                                foreach (Control cItem in GetControlInDiv(c))
                                    w.Data = SetProperties(props, w.Data, cItem);
                            }
                            else if (c.GetType().Name == "HtmlTable")
                            {
                                foreach (Control cItem in GetControlInTable(c))
                                    w.Data = SetProperties(props, w.Data, cItem);
                            }
                            else if (c.GetType().Name == "UpdatePanel")
                            {
                                foreach (Control cItem in GetControlInUpdatePanel(c))
                                    w.Data = SetProperties(props, w.Data, cItem);
                            }
                            else if (c.ID == null || c.ID.Split('_').Length <= 1) continue;
                            else w.Data = SetProperties(props, w.Data, c);
                        }
                        catch (Exception ex1)
                        {
                            w.ErrorMessage = $"Error in {w.ErrorMessage} : {ex1.Message}";
                        }

                    }
                }
                oList.Add(w.Data);
                w.ListData.Add(w.Data);
            }
            catch (Exception ex)
            {
                w.ErrorMessage = $"Error in {w.ErrorMessage} : {ex.Message}";
            }
        }

        return w;
    }
    public static List<clsParameter> GetParameter(object Criteria, object SortBy, string Table = "", bool DataAccess = false, bool ViewAllData = true)
    {
        List<clsParameter> pList = new List<clsParameter>();
        if ($"{Criteria}" != "") pList.Add(new clsParameter { Name = "Criteria", Value = Criteria, IsUseApostrophe = true });
        else pList.Add(new clsParameter("Criteria", ""));
        if ($"{SortBy}" != "") pList.Add(new clsParameter("Sort", SortBy));
        if (Table != "") pList.Add(new clsParameter("Table", Table));
        if (DataAccess)
        {
            pList.Add(new clsParameter("Username", GetUsername()));
            pList.Add(new clsParameter("ViewAllData", ViewAllData));
        }
        

        return pList;
    }
    private static List<Control> GetControlInDiv(Control c)
    {
        List<Control> cList = new List<Control>();
        foreach (Control cItem in c.Controls)
        {
            if (cItem.ID != null && cItem.ID.Contains("_")) cList.Add(cItem);
        }

        return cList;
    }
    private static List<Control> GetControlInTable(Control c)
    {
        List<Control> cList = new List<Control>();
        foreach (Control cRow in c.Controls)
            foreach (Control cCell in cRow.Controls)
                foreach (Control cItem in cCell.Controls)
                {
                    if (cItem.GetType().Name == "Panel")
                    {
                        foreach (Control cItemPnl in cItem.Controls)
                            if (cItemPnl.ID != null && cItemPnl.ID.Contains("_")) cList.Add(cItemPnl);
                    }
                    if (cItem.ID != null && cItem.ID.Contains("_")) cList.Add(cItem);
                }
        return cList;
    }
    private static List<Control> GetControlInUpdatePanel(Control c)
    {
        List<Control> cList = new List<Control>();
        foreach (Control cItem in c.Controls)
        {
            foreach (Control cRow in cItem.Controls)
            {
                if (cRow.ID != null && cRow.ID.Contains("_")) cList.Add(cRow);
            }
        }
        return cList;
    }
    private static object SetProperties(PropertyInfo[] props, object obj, Control c)
    {
        foreach (PropertyInfo pi in props)
        {
            if (c == null) continue;
            if (pi.Name.ToLower() == c.ID.Split('_')[1].ToLower())
            {
                if (c == null) continue;
                object ControlValue = GetControlValue(c);
                if (pi.Name == "No" && $"{ControlValue}" == "New") ControlValue = "0";
                if (pi.ToString().ToLower().Contains("string"))
                {
                    pi.SetValue(obj, ControlValue, null);
                    break;
                }
                else if (pi.ToString().ToLower().Contains("double"))
                {
                    if ($"{ControlValue}" != "-" && $"{ControlValue}" != "") pi.SetValue(obj, Convert.ToDouble(ControlValue), null);
                    break;
                }
                else if (pi.ToString().ToLower().Contains("boolean"))
                {
                    if ($"{ControlValue}" != "") pi.SetValue(obj, Convert.ToBoolean(ControlValue), null);
                    break;
                }
                else if (pi.ToString().ToLower().Contains("decimal"))
                {
                    if ($"{ControlValue}" != "-" && $"{ControlValue}" != "") pi.SetValue(obj, Convert.ToDecimal(ControlValue), null);
                    break;
                }
                else if (pi.ToString().ToLower().Contains("datetime"))
                {
                    try
                    {
                        if ($"{ControlValue}" != "-" && $"{ControlValue}" != "") pi.SetValue(obj, Utility.GetDate($"{ControlValue}"), null);
                    }
                    catch (Exception ex) { }

                    break;
                }
                else if (pi.ToString().ToLower().Contains("int") && !pi.ToString().ToLower().Contains("boolean"))
                {                    
                    if ($"{ControlValue}" != "") pi.SetValue(obj, ControlValue.ToInt(), null);
                    break;
                }
                if (ControlValue.GetType().ToString() == "System.Web.UI.WebControls.ListItemCollection")
                {
                    List<Item> iList = new List<Item>();
                    foreach (ListItem li in (ListItemCollection)ControlValue)
                    {
                        if (!li.Selected) continue;
                        iList.Add(new Item() { Text = li.Text, Value = li.Value, IsChecked = li.Selected });
                    }
                    pi.SetValue(obj, iList);
                }
            }
        }
        return obj;
    }
    private static object GetControlValue(Control c)
    {
        object result = "";
        switch (c.GetType().Name)
        {
            case "HiddenField":
                result = ((HiddenField)c).Value.Trim();
                break;
            case "Literal":
                result = ((Literal)c).Text.Trim();
                break;
            case "Label":
                result = ((Label)c).Text.Trim();
                break;
            case "TextBox":
                result = ((TextBox)c).Text.Trim();
                break;
            case "HtmlInputGenericControl":
                result = ((HtmlInputGenericControl)c).Value.Trim();
                break;
            case "DropDownList":
                result = ((DropDownList)c).SelectedValue;
                break;
            case "CheckBox":
                result = ((CheckBox)c).Checked.ToString();
                break;
            case "RadioButtonList":
                result = ((RadioButtonList)c).SelectedValue;
                break;
            case "FileUpload":
                result = ((FileUpload)c).FileName;
                break;
            case "Image":
                result = ((Image)c).ImageUrl;
                break;
            case "LinkButton":
                result = ((LinkButton)c).Text;
                break;            
            case "GridView":
                //result = GetGridData((GridView)c, new Wrapping { Data = new Attachment() }).ListData;
                break;
        }
        return result;
    }
    #endregion

    #region DateTime
    public static string SetDate(DateTime Text)
    {
        return string.Format(@"{0}/{1}/{2}", Text.Day.ToString("00"), Text.Month.ToString("00"), Text.Year);
    }
    public static int GetMonth(string Text)
    {
        int Result = 0;
        switch (Text.ToLower())
        {
            case "jan":
            case "january":
                Result = 1;
                break;
            case "feb":
            case "february":
                Result = 2;
                break;
            case "mar":
            case "march":
                Result = 3;
                break;
            case "apr":
            case "april":
                Result = 4;
                break;
            case "may":
                Result = 5;
                break;
            case "jun":
            case "june":
                Result = 6;
                break;
            case "jul":
            case "july":
                Result = 7;
                break;
            case "aug":
            case "august":
                Result = 8;
                break;
            case "sep":
            case "september":
                Result = 9;
                break;
            case "oct":
            case "october":
                Result = 10;
                break;
            case "nov":
            case "november":
                Result = 11;
                break;
            case "dec":
            case "des":
            case "december":
                Result = 12;
                break;
        }
        return Result;


    }
    public static string GetMonth(int Month)
    {
        string Result = "";
        switch (Month)
        {
            case 1:
                Result = "January";
                break;
            case 2:
                Result = "February";
                break;
            case 3:
                Result = "March";
                break;
            case 4:
                Result = "April";
                break;
            case 5:
                Result = "May";
                break;
            case 6:
                Result = "June";
                break;
            case 7:
                Result = "July";
                break;
            case 8:
                Result = "August";
                break;
            case 9:
                Result = "September";
                break;
            case 10:
                Result = "October";
                break;
            case 11:
                Result = "November";
                break;
            case 12:
                Result = "December";
                break;
        }
        return Result;


    }
    public static string GetShortMonth(int Month)
    {
        string Result = "";
        switch (Month)
        {
            case 1:
                Result = "Jan";
                break;
            case 2:
                Result = "Feb";
                break;
            case 3:
                Result = "Mar";
                break;
            case 4:
                Result = "Apr";
                break;
            case 5:
                Result = "May";
                break;
            case 6:
                Result = "Jun";
                break;
            case 7:
                Result = "Jul";
                break;
            case 8:
                Result = "Aug";
                break;
            case 9:
                Result = "Sep";
                break;
            case 10:
                Result = "Oct";
                break;
            case 11:
                Result = "Nov";
                break;
            case 12:
                Result = "Dec";
                break;
        }
        return Result;


    }
    public static void SetMonthF(F.DropDownList ddl)
    {
        ddl.Items.Add(new F.ListItem("Select Month", "0"));
        for (int i = 1; i <= 12; i++)
        {
            ddl.Items.Add(new F.ListItem(GetMonth(i), $"{i}"));
        }
    }
    #endregion    

    #region Security
    public static bool CreateAccess(object PageName)
    {
        if (IsMember(CNT.SuperAdmin) || IsMember(CNT.Admin)) return true;
        return Access(PageName, CNT.Access.C);
    }
    public static bool ViewAccess(object PageName)
    {
        if (IsMember(CNT.SuperAdmin) || IsMember(CNT.Admin)) return true;        
        return Access(PageName, CNT.Access.R);
    }    
    public static bool UpdateAccess(object PageName)
    {
        if (IsMember(CNT.SuperAdmin) || IsMember(CNT.Admin)) return true;
        return Access(PageName, CNT.Access.U);
    }
    public static bool DeleteAccess(object PageName)
    {
        if (IsMember(CNT.SuperAdmin) || IsMember(CNT.Admin)) return true;
        return Access(PageName, CNT.Access.D);
    }
    public static bool ViewAllDataAccess(object PageName)
    {
        if (IsMember(CNT.SuperAdmin) || IsMember(CNT.Admin)) return true;
        return Access(PageName, CNT.Access.V);
    }
    public static bool DeviationAccess(object PageName)
    {
        if (IsMember(CNT.SuperAdmin)) return true;
        return Access(PageName, CNT.Access.DEV);
    }
    public static bool Access(object PageName, string Access)
    {
        List<object> oList = new List<object>();
        if (IsMember(CNT.User)) oList = UserManagement.UsersGetByPageName(PageName);        
        else oList = UserManagement.OperatorsGetByPageName(PageName);

        if (oList.Count == 0) return false;
        switch (Access)
        {
            case CNT.Access.C:
                if (oList.Exists(a => !((UserManagement)a).Create)) return false;
                else return true;
            case CNT.Access.R:
                if (oList.Exists(a => !((UserManagement)a).Read)) return false;
                else return true;
            case CNT.Access.U:
                if (oList.Exists(a => !((UserManagement)a).Update)) return false;
                else return true;
            case CNT.Access.D:
                if (oList.Exists(a => !((UserManagement)a).Delete)) return false;
                else return true;
            case CNT.Access.V:
                if (oList.Exists(a => !((UserManagement)a).ViewAllData)) return false;
                else return true;
            case CNT.Access.DEV:
                if (oList.Exists(a => !((UserManagement)a).Deviation)) return false;
                else return true;                
        }
        return false;
    }
    public static bool IsMember(string RoleName)
    {
        bool Result = false;
        if (HttpContext.Current.Session[RoleName] != null)
            Result = HttpContext.Current.Session[RoleName].ToBool();
        else if (HttpContext.Current.Request.Cookies[RoleName] != null)
            Result = HttpContext.Current.Request.Cookies[RoleName].Value.ToBool();
        return Result;
    }
    public static void SetPage(DropDownList ddl, string EmptyText = "", bool ShowEmptyText = true)
    {
        ddl.Items.Clear();
        if (!EmptyText.IsEmpty() || ShowEmptyText) ddl.Items.Add(new ListItem(EmptyText, ""));

        string sourceDirectory = HttpContext.Current.Server.MapPath(@"~/Pages/");
        DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);
        var aspxFiles = Directory.EnumerateFiles(sourceDirectory, "*.aspx", SearchOption.AllDirectories).Select(Path.GetFileName);

        foreach (string currentFile in aspxFiles.OrderBy(s => s))
            ddl.Items.Add(currentFile.Replace(".aspx", ""));
    }
    #endregion

    #region Excel
    public static void SetExcel(Wrapping w)
    {
        ExcelCell Cell = w.Sheet.Cells[w.Row, w.Column];
        Cell.Style = w.CellStyle;
        if (w.Value == null)
        {
            if (w.IsNumber) Cell.Value = 0;
            else Cell.Value = "";
        }
        else Cell.Value = w.Value;

        Cell.Style.HorizontalAlignment = w.IsNumber ? HorizontalAlignmentStyle.Right : HorizontalAlignmentStyle.Left;
        Cell.Style.WrapText = true;
        w.Column += 1;
    }
    public static void ExportToExcel(List<object> oList, string FileName)
    {
        #region Excel Style
        SpreadsheetInfo.SetLicense("E43Y-EJC3-221R-AA38");
        ExcelFile excel = new ExcelFile();
        ExcelWorksheet sheet = null;
        CellStyle HeaderStyle = new CellStyle(excel);
        CellStyle HeaderTagLineStyle = new CellStyle(excel);
        CellStyle ItemStyleOdd = new CellStyle(excel);
        CellStyle ItemStyleEven = new CellStyle(excel);

        HeaderStyle.Borders.SetBorders(MultipleBorders.Vertical, System.Drawing.Color.Black, LineStyle.Thin);
        HeaderStyle.Borders.SetBorders(MultipleBorders.Horizontal, System.Drawing.Color.Black, LineStyle.Thin);
        HeaderStyle.Font.Name = "Calibri";
        HeaderStyle.Font.Size = 220;
        HeaderStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        HeaderStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
        HeaderStyle.FillPattern.SetSolid(System.Drawing.Color.FromArgb(169, 219, 128));

        ItemStyleOdd.Borders.SetBorders(MultipleBorders.Vertical, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleOdd.Borders.SetBorders(MultipleBorders.Horizontal, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleOdd.Font.Name = "Calibri";
        ItemStyleOdd.Font.Size = 220;
        ItemStyleOdd.HorizontalAlignment = HorizontalAlignmentStyle.Left;
        ItemStyleOdd.VerticalAlignment = VerticalAlignmentStyle.Center;

        ItemStyleEven.Borders.SetBorders(MultipleBorders.Vertical, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleEven.Borders.SetBorders(MultipleBorders.Horizontal, System.Drawing.Color.LightGray, LineStyle.Thin);
        ItemStyleEven.Font.Name = "Calibri";
        ItemStyleEven.Font.Size = 220;
        ItemStyleEven.HorizontalAlignment = HorizontalAlignmentStyle.Left;
        ItemStyleEven.VerticalAlignment = VerticalAlignmentStyle.Center;
        #endregion

        sheet = excel.Worksheets.Add("Data");
        
        Wrapping w = new Wrapping { Sheet = sheet, CellStyle = HeaderStyle };
        PropertyInfo[] props = oList[0].GetType().GetProperties();

        w.SetExcel("No");        
        foreach (PropertyInfo pi in props)
        {
            try
            {
                string ExcelTitle = ((ColumnAttribute)pi.GetCustomAttributes(false)[0]).Title;
                if (ExcelTitle.IsEmpty()) continue;
                w.SetExcel(ExcelTitle);                
            }
            catch (Exception ex) { }
        }        

        #region DataExcel		
        w.Column = 0;
        w.CellStyle = ItemStyleOdd;
        foreach (var o in oList)
        {
            w.Row += 1;
            w.SetExcel(w.Row, true);
            foreach (PropertyInfo pi in props)
            {
                try
                {
                    string ExcelTitle = ((ColumnAttribute)pi.GetCustomAttributes(false)[0]).Title;
                    if (ExcelTitle.IsEmpty()) continue;                    
                    w.SetExcel(pi.GetValue(o, null), IsNumericType(pi));
                }
                catch (Exception ex) { }                
            }
            w.Column = 0;
        }        
        #endregion

        #region AutoFitText
        int columnCount = sheet.CalculateMaxUsedColumns();
        for (int j = 0; j < columnCount; j++)
        {
            sheet.Columns[j].AutoFitAdvanced(1.3f, sheet.Rows[0], sheet.Rows[sheet.Rows.Count - 1]);
            if (sheet.Columns[j].Width >= 30000) sheet.Columns[j].Width = 30000;
        }
        #endregion

        #region SaveExcel
        HttpResponse resp = HttpContext.Current.Response;
        resp.Clear();
        resp.Buffer = false;
        resp.AppendHeader("content-disposition", $"attachment; filename={FileName}.xlsx");
        resp.AppendHeader("Content-Transfer-Encoding", "binary");
        resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        byte[] buffer = null;
        string tempFilename = $"{PathTempFolder}{FileName}.xlsx";

        if (File.Exists(tempFilename)) File.Delete(tempFilename);
        excel.SaveXlsx(tempFilename);


        using (FileStream fs = new FileStream(tempFilename, FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
        }
        if (File.Exists(tempFilename))
            File.Delete(tempFilename);

        resp.BinaryWrite(buffer);
        resp.End();
        #endregion
    }
    #endregion

    #region FineUI
    public static void OpenNewTab(string Id, string Title, string IFrameURL)
    {
        F.JsObjectBuilder joBuilder = new F.JsObjectBuilder();
        joBuilder.AddProperty("id", Id);
        joBuilder.AddProperty("title", Title);
        joBuilder.AddProperty("iframeUrl", IFrameURL, true);
        joBuilder.AddProperty("refreshWhenExist", true);
        joBuilder.AddProperty("iconFont", "plus");
        F.PageContext.RegisterStartupScript($"parent.addExampleTab({joBuilder});");
    }
    public static void AddClassToClass(string SourceClass, string AddedClass)
    {
        F.PageContext.RegisterStartupScript($"AddClassToClass('.{SourceClass}', '{AddedClass}');");
    }
    public static void Hide(string ControlClientId)
    {
        F.PageContext.RegisterStartupScript($"document.getElementById(\"{ControlClientId}\").style.display = \"none\";");
    }
    public static void Display(string ControlClientId)
    {
        F.PageContext.RegisterStartupScript($"document.getElementById(\"{ControlClientId}\").style.display = \"\";");
    }    
    public static void Display(string ControlClientId, bool Visible)
    {
        if (Visible) F.PageContext.RegisterStartupScript($"document.getElementById(\"{ControlClientId}\").style.display = \"\";");
        else F.PageContext.RegisterStartupScript($"document.getElementById(\"{ControlClientId}\").style.display = \"none\";");
    }
    public static void ClosePopup(string Parameter)
    {
        F.PageContext.RegisterStartupScript("parent.activeTabAndUpdate('" + HttpContext.Current.Request.QueryString["parenttabid"] + "', '" + Parameter + "');");
    }
    public static void ExecClientScript(string Script)
    {
        F.PageContext.RegisterStartupScript(Script);
    }
    public static void ClearEventArgument()
    {
        F.PageContext.RegisterStartupScript("theForm.__EVENTARGUMENT.value = '';");
    }
    public static void CloseUpdate(string Message = "Data has been Save Successfully", string Title = "Information")
    {
        F.Alert.Show(Message, Title, F.Alert.DefaultMessageBoxIcon, "parent.activeTabAndUpdate('" + ParentTabId + "', '" + "Search" + "');");
    }
    #endregion
        
    #region WERP
    public static void SetApproval(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        if (UserManagement.GetByRole(CNT.Role.Approver).Exists(a => ((UserManagement)a).AllUsers))
        {
            foreach (Users o in Users.GetALLActive())
                ddl.Items.Add(new ListItem(o.Name, $"{o.Id}"));
        }
        else
        {
            foreach(Users o in Users.GetByRole(CNT.Role.Approver))
                ddl.Items.Add(new ListItem(o.Name, $"{o.Id}"));
        }
    }
    public static void SetFunctionLogger(DropDownList ddl, string EmptyText = "")
    {
        ddl.Items.Clear();
        if (!EmptyText.IsEmpty()) ddl.Items.Add(new ListItem(EmptyText, ""));
        List<object> oList = Function.GetALLActive();
        foreach (Function o in oList)
            ddl.Items.Add(new ListItem(o.Name, $"{o.Id}"));
    } 
    public static void SetTotalizer(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<object> oList = Totalizer.GetALL();
        foreach (Totalizer o in oList)
            ddl.Items.Add(new ListItem($"{o.Name} - {o.FunctionLogger}", $"{o.Id}"));
    }
    public static void SetTotalizerByFunction(DropDownList ddl, object FunctionId)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("Pilih Lokasi", ""));
        List<object> oList = Totalizer.GetGetALLActiveByFunctionLogger(FunctionId);
        foreach (Totalizer o in oList)
            ddl.Items.Add(new ListItem($"{o.Name} - {o.FunctionLogger}", $"{o.Id}"));
    }
    public static void SetNoParentTotalizer(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<object> oList = Totalizer.GetALLWithNoParent();
        foreach (Totalizer o in oList)
            ddl.Items.Add(new ListItem($"{o.Name} - {o.FunctionLogger}", $"{o.Id}"));
    }
    public static void SetParentTotalizer(DropDownList ddl, object TotalizerId)
    {
        ddl.Items.Clear();
        List<object> oList = Totalizer.GetParentTotalizer(TotalizerId);
        foreach (Totalizer o in oList)
            ddl.Items.Add(new ListItem($"{o.Name} ({o.FunctionLogger})", $"{o.Id}"));
    }
    public static void SetDropDownMasterData(DropDownList ddl, string TableName, string FieldName = "Name")
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<object> oList = Item.GetActiveMasterData(TableName, FieldName);
        foreach (Item o in oList)
            ddl.Items.Add(new ListItem($"{o.Text}", $"{o.Value}"));
    }
    public static void SetDropDownTypeLocation(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        ddl.Items.Add("Customer");
        foreach (Function o in Function.GetByUseLocation())
            ddl.Items.Add(o.Name);
    }
    public static void SetDropDownFunctionStation(DropDownList ddl, string FunctionName)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");        
        foreach (FunctionStation o in FunctionStation.GetByFunctionName(FunctionName))
            ddl.Items.Add(new ListItem(o.Station, o.Id.ToText()));
    }
    public static void SetDropDownFunctionLoggerByStation(DropDownList ddl, object StationId)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<object> oList = FunctionLogger.GetByStation(StationId);
        foreach (FunctionLogger o in oList)
            ddl.Items.Add(new ListItem($"{o.Name}", $"{o.Id}"));
    }
    public static void SetDropDownChildStation(DropDownList ddl, object ParentStationId)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        Station s = Station.LowestLevelGetById(ParentStationId);
        foreach (Station o in Station.GetLowerStation(s.Level))
            ddl.Items.Add(new ListItem($"{o.Name}", $"{o.Id}"));
    }
    public static void SetSubjectNotUsedCustomer(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        foreach (Subject s in Subject.GetNotUsedCust())
            ddl.Items.Add(new ListItem(s.Name, s.Id.ToText()));
    }
    public static void SetDropDownRack(DropDownList ddl, object WarehouseId)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<object> oList = Rack.GetALLActiveByWarehouseId(WarehouseId);
        foreach (Rack o in oList)
            ddl.Items.Add(new ListItem($"{o.Name}", $"{o.Id}"));
    }
    public static void SetLocationByArea(DropDownList ddl, object AreaId)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<object> oList = Location.GetByAreaId(AreaId);
        foreach (Location o in oList)
            ddl.Items.Add(new ListItem($"{o.Name}", $"{o.Id}"));
    }
    public static void SetOperatorsByVendor(DropDownList ddl, object VendorId)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<object> oList = Operators.GetByVendor(VendorId);
        foreach (Operators o in oList)
            ddl.Items.Add(new ListItem(o.Name, $"{o.Id}"));
    }
    public static void SetDropDownRate(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add("");
        List<Rate> oList = BaseModel.GetAllActive<Rate>();
        foreach (Rate o in oList)
            ddl.Items.Add(new ListItem($"{o.Code}-{o.Name}", $"{o.Id}"));
    }
    #endregion
}