using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Data.SqlTypes;

namespace DAL
{
    public static class DataAccess
    {
        public static string IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return "";
                }
                catch (SqlException ex)
                {
                    return ex.Message;
                }
            }
        }
        private static object Converter(string Type, object Value)
        {
            object Result = Value;

            if (Type == "System.String") Result = Value.ToString();
            else if (Type.Contains("System.Nullable`1[[System.Int32"))
            {
                if (string.Format("{0}", Value).Trim() == "") Result = null;
                else Result = Convert.ToInt32(Value);
            }
            else if (Type == "System.Double" || Type.Contains("System.Nullable`1[[System.Double")) Result = Convert.ToDouble(Value);
            else if (Type == "System.Decimal" || Type.Contains("System.Nullable`1[[System.Decimal")) Result = Convert.ToDecimal(Value);

            return Result;
        }

        public static object GetSingleRowByQuery(int CnIndex, string Query, object obj)
        {            
            PropertyInfo[] props = obj.GetType().GetProperties();
            SqlCommand cmd = new SqlCommand(Query, Connection.Open(CnIndex));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    obj = Activator.CreateInstance(obj.GetType());
                    foreach (PropertyInfo prop in props)
                    {
                        try
                        {
                            string field = ((ColumnAttribute)prop.GetCustomAttributes(false)[0]).Field;
                            if (dr[field] != DBNull.Value)
                            {
                                if (prop.ToString().ToLower().Contains("string")) prop.SetValue(obj, dr[field].ToString().Trim(), null);
                                else prop.SetValue(obj, dr[field], null);
                            }
                        }
                        catch (Exception ex) { }                       
                    }                       
                }
            }
            cmd.Connection.Close();
            return obj;
        }

        public static object GetSingleRowBySP(int CnIndex, string SPName, List<clsParameter> Parameters, object obj)
        {
            PropertyInfo[] props = obj.GetType().GetProperties();
            SqlCommand cmd = new SqlCommand(SPName, Connection.Open(CnIndex));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            #region Parameters
            foreach (clsParameter p in Parameters)
            {
                object objValue = p.Value;
                if (objValue.GetType().ToString().ToLower().Contains("string"))
                    objValue = objValue.ToString().Replace("'", "''").Trim();
                cmd.Parameters.AddWithValue(p.Name, objValue);
            }
            #endregion            

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    obj = Activator.CreateInstance(obj.GetType());
                    foreach (PropertyInfo prop in props)
                    {
                        try
                        {
                            string field = ((ColumnAttribute)prop.GetCustomAttributes(false)[0]).Field;
                            if (dr[field] != DBNull.Value)
                            {
                                if (prop.ToString().ToLower().Contains("string")) prop.SetValue(obj, dr[field].ToString().Trim(), null);
                                else prop.SetValue(obj, dr[field], null);
                            }
                        }
                        catch { }
                    }                    
                }
            }
            cmd.Connection.Close();
            return obj;
        }

        public static List<object> GetDataByQuery(object CnIndex, string Query, object obj)
        {
            List<object> oList = new List<object>();
            PropertyInfo[] props = obj.GetType().GetProperties();
            SqlCommand cmd = new SqlCommand(Query, Connection.Open(CnIndex));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            #region AccessDatabase
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                List<int?> iList = new List<int?>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    for (int j = 0; j < props.Length; j++)
                    {
                        if (props[j].GetCustomAttributes(false).Length == 0) continue;
                        string field = ((ColumnAttribute)props[j].GetCustomAttributes(false)[0]).Field;
                        if (dr.GetName(i).ToLower() == field.ToLower())
                        {
                            iList.Add(j);
                            break;
                        }
                    }
                    if (iList.Count != (i + 1)) iList.Add(null);
                }

                while (dr.Read())
                {                    
                    obj = Activator.CreateInstance(obj.GetType());
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        if (iList.Count <= i) break;
                        if (dr[i] == DBNull.Value) continue;
                        if (iList[i] == null) continue;
                        int idx = Convert.ToInt32(iList[i]);
                        SetPropertiesValue(obj, dr, props, idx, i);
                        //props[idx].SetValue(obj, Converter(props[idx].PropertyType.FullName, dr[i]), null);                        
                    }                   
                    oList.Add(obj);
                }
            }
            cmd.Connection.Close();
            #endregion

            return oList;
        }                
        
        public static List<object> GetDataBySP(int CnIndex, string SPName, List<clsParameter> Parameters, object obj)
        {            
            List<object> oList = new List<object>();
            PropertyInfo[] props = obj.GetType().GetProperties();
            SqlCommand cmd = new SqlCommand(SPName, Connection.Open(CnIndex));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            #region Parameters
            foreach (clsParameter p in Parameters)
            {
                object objValue = p.Value;
                if (objValue != null && objValue.GetType().ToString().ToLower().Contains("string"))
                    objValue = objValue.ToString().Replace("'", "''").Trim();
                cmd.Parameters.AddWithValue(p.Name, objValue);
            }
            #endregion            

            #region AccessDatabase
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                List<int?> iList = new List<int?>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    for (int j = 0; j < props.Length; j++)
                    {
                        if (props[j].GetCustomAttributes(false).Length == 0) continue;
                        string field = ((ColumnAttribute)props[j].GetCustomAttributes(false)[0]).Field;
                        if (dr.GetName(i).ToLower() == field.ToLower())
                        {
                            iList.Add(j);
                            break;
                        }
                    }
                    if (iList.Count != (i + 1)) iList.Add(null);
                }

                while (dr.Read())
                {
                    obj = Activator.CreateInstance(obj.GetType());
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        if (iList.Count <= i) break;
                        if (dr[i] == DBNull.Value) continue;
                        if (iList[i] == null) continue;
                        int idx = Convert.ToInt32(iList[i]);
                        SetPropertiesValue(obj, dr, props, idx, i);
                    }
                    oList.Add(obj);
                }
            }
            cmd.Connection.Close();
            #endregion
                        
            return oList;
        }

        public static List<object> GetDataBySPPaging(int CnIndex, string SPName, List<clsParameter> Parameters, out double TotalRow, object obj)
        {
            TotalRow = 0;
            List<object> oList = new List<object>();
            PropertyInfo[] props = obj.GetType().GetProperties();
            SqlCommand cmd = new SqlCommand(SPName, Connection.Open(CnIndex));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            #region Parameters
            foreach (clsParameter p in Parameters)
            {
                object objValue = p.Value;
                if (objValue != null && objValue.GetType().ToString().ToLower().Contains("string"))
                {
                    if (p.IsUseApostrophe == false)
                        objValue = objValue.ToString().Replace("'", "''").Trim();
                    else objValue = objValue.ToString().Trim();
                }
                if (objValue == null) cmd.Parameters.AddWithValue(p.Name, DBNull.Value);
                else cmd.Parameters.AddWithValue(p.Name, objValue);
            }
            #endregion            

            #region AccessDatabase
            bool IsFirstRow = true;            
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                List<int?> iList = new List<int?>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    for (int j = 0; j < props.Length; j++)
                    {
                        if (props[j].GetCustomAttributes(false).Length == 0) continue;
                        string field = ((ColumnAttribute)props[j].GetCustomAttributes(false)[0]).Field;
                        if (dr.GetName(i).ToLower() == field.ToLower())
                        {
                            iList.Add(j);
                            break;
                        }
                    }
                    if (iList.Count != (i + 1)) iList.Add(null);
                }

                while (dr.Read())
                {
                    if (IsFirstRow)
                    {
                        TotalRow = Convert.ToDouble(dr[dr.FieldCount - 1]);
                        IsFirstRow = false;
                    }
                    obj = Activator.CreateInstance(obj.GetType());
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        if (iList.Count <= i) break;
                        if (dr[i] == DBNull.Value) continue;
                        if (iList[i] == null) continue;
                        int idx = Convert.ToInt32(iList[i]);
                        SetPropertiesValue(obj, dr, props, idx, i);
                    }
                    oList.Add(obj);
                }
            }
            cmd.Connection.Close();
            #endregion

            return oList;
        }        

        public static object GetSingleValueByQuery(object CnIndex, string Query)
        {
            SqlCommand cmd = new SqlCommand(Query, Connection.Open(CnIndex));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            object obj = cmd.ExecuteScalar();
            if (obj == System.DBNull.Value) obj = null;
            cmd.Connection.Close();
            return obj;
        }

        public static string ExecNonReturnValueByQuery(object CnIndex, string Query)
        {
            string ErrorMessage = "";
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(Query, Connection.Open(CnIndex));
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("Error Message : {0}", ex.Message);
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == System.Data.ConnectionState.Open))
                    cmd.Connection.Dispose();
            }            
            return ErrorMessage;
        }
        public static string ExecNonReturnValueBySP(int CnIndex, string SPName, List<clsParameter> Parameters)
        {
            string ErrorMessage = "";
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(SPName, Connection.Open(CnIndex));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                #region Parameters
                foreach (clsParameter p in Parameters)
                {
                    object objValue = p.Value;
                    if (objValue != null && objValue.GetType().ToString().ToLower().Contains("string"))
                        objValue = objValue.ToString().Trim();
                    cmd.Parameters.AddWithValue(p.Name, objValue);
                }
                #endregion

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("Error Message : {0}", ex.Message);
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == System.Data.ConnectionState.Open))
                    cmd.Connection.Dispose();
            }
            return ErrorMessage;
        }

        public static string TruncateTable(int CnIndex, string TableName)
        {
            string ErrorMessage = "";
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(string.Format("Truncate Table {0}", TableName), Connection.Open(CnIndex));
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == System.Data.ConnectionState.Open))
                    cmd.Connection.Dispose();
            }
            return ErrorMessage;
        }

        public static string DropTable(int CnIndex, string TableName)
        {
            string ErrorMessage = "";
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(string.Format("Drop Table {0}", TableName), Connection.Open(CnIndex));
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == System.Data.ConnectionState.Open))
                    cmd.Connection.Dispose();
            }
            return ErrorMessage;
        }

        public static string Save(int CnIndex, string SpName, List<clsParameter> Parameters)
        {
            string Result = "";
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(SpName, Connection.Open(CnIndex));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                foreach (clsParameter p in Parameters)
                {
                    if (p.Value == null) cmd.Parameters.AddWithValue(p.Name, DBNull.Value);
                    else
                    {                        
                        cmd.Parameters.AddWithValue(p.Name, p.Value);                        
                    }
                }

                object obj = cmd.ExecuteScalar();
                if (obj != null) Result = obj.ToString();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                Result = string.Format("Error Message : {0}", ex.Message);
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == System.Data.ConnectionState.Open))
                    cmd.Connection.Dispose();
            }
            return Result;
        }

        public static string Delete(int CnIndex, object Value, string FieldName, string TableName)
        {
            string Result = "";
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("DeleteTable", Connection.Open(CnIndex));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                #region Parameters
                cmd.Parameters.AddWithValue("@Field", FieldName);
                cmd.Parameters.AddWithValue("@Table", TableName);
                cmd.Parameters.AddWithValue("@Value", Value);
                #endregion

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                Result = string.Format("Error Message : {0}", ex.Message);
            }
            finally
            {
                if ((cmd.Connection != null) && (cmd.Connection.State == System.Data.ConnectionState.Open))
                    cmd.Connection.Dispose();
            }
            return Result;

        }

        public static List<string> GetListFieldByQuery(int CnIndex, string Query, string Field)
        {
            SqlCommand cmd = new SqlCommand(Query, Connection.Open(CnIndex));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            List<string> IDList = new List<string>();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read()) IDList.Add(dr[Field].ToString());
            }
            cmd.Connection.Close();
            return IDList;
        }

        private static void SetPropertiesValue(object obj, SqlDataReader dr, PropertyInfo[] props, int idx, int i)
        {
            switch (props[idx].PropertyType.FullName)
            {
                case "System.String":
                    props[idx].SetValue(obj, dr[i].ToString().Trim(), null);
                    break;
                case "System.Nullable`1[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                case "System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":                
                case "System.Int32":
                    if (dr[i] != null && dr[i].ToString().Trim() != "")
                        props[idx].SetValue(obj, Convert.ToInt32(dr[i]), null);
                    break;
                case "System.Nullable`1[[System.Double, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                case "System.Double":
                    props[idx].SetValue(obj, Convert.ToDouble(dr[i]), null);
                    break;
                case "System.Decimal":
                    props[idx].SetValue(obj, Convert.ToDecimal(dr[i]), null);
                    break;
                case "System.Boolean":
                    props[idx].SetValue(obj, Convert.ToBoolean(dr[i]), null);
                    break;
                default:
                    props[idx].SetValue(obj, dr[i], null);
                    break;
            }
        }

		public static object GetSingleValueBySP(int CnIndex, string SPName, List<clsParameter> Parameters)
		{
			SqlCommand cmd = new SqlCommand(SPName, Connection.Open(CnIndex));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 0;

			#region Parameters
			foreach (clsParameter p in Parameters)
			{
				object objValue = p.Value;
				if (objValue != null && objValue.GetType().ToString().ToLower().Contains("string"))
					objValue = objValue.ToString().Replace("'", "''").Trim();
				cmd.Parameters.AddWithValue(p.Name, objValue);
			}
			#endregion

			object obj = cmd.ExecuteScalar();
			if (obj == System.DBNull.Value) obj = null;
			cmd.Connection.Close();
			return obj;
		}        

        public static bool IsTableExist(int CnIndex, string DBName, string TableName)
        {
            SqlCommand cmd = new SqlCommand($"USE [{DBName}] Select Count(*) from INFORMATION_SCHEMA.TABLES where TABLE_Name = '{TableName}' ", Connection.Open(CnIndex));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            object obj = cmd.ExecuteScalar();
            bool Result = false;
            if (Convert.ToInt32(obj) != 0) Result = true;
            return Result;
        }

        public static bool IsSpExist(int CnIndex, string DBName, string SPName)
        {
            SqlCommand cmd = new SqlCommand($"Use [{DBName}] SELECT Count(*) FROM sys.objects WHERE type = 'P' AND name = '{SPName}'", Connection.Open(CnIndex));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            object obj = cmd.ExecuteScalar();
            bool Result = false;
            if (Convert.ToInt32(obj) != 0) Result = true;
            return Result;
        }
    }
}
