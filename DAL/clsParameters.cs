using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class clsParameter
    {
        #region Fields
        string _name;
        object _value;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public object Value
        {
            get { return _value; }
            set 
            {
                if (value == null) _value = null;
                else _value = value.GetType().ToString() == "string" ? value.ToString().Trim() : value;
            }
        }
        public bool IsUseApostrophe { get; set; }

        public clsParameter() { }
        public clsParameter(string Name, object Value) 
        {
            _name = Name;
            _value = Value;
        }

        #endregion        
    }
    

}
