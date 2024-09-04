using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DAL
{
    public static class Key
    {
        static string _domain;
        public static string Domain 
        {
            get { return ConfigurationManager.AppSettings["Domain"].ToString(); }
            set { _domain = value; }
        }
    }
}
