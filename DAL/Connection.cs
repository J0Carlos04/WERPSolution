using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public static class Connection
    {
        public static SqlConnection Open(object cn)
        {
            if (IsNumericType(cn))
                return (OpenIndex((int)cn));
            else return OpenCN($"{cn}");
        }
        private static SqlConnection OpenIndex(int Index)
        {            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Index].ToString());
            conn.Open();

            return conn;
        }

        private static SqlConnection OpenCN(string cn)
        {
            SqlConnection conn = new SqlConnection(cn);
            conn.Open();
           
            return conn;
        }
        private static bool IsNumericType(object o)
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
    }    
}
