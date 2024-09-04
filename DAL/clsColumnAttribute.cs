using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAL
{
    /// <summary>
    /// Attribute for decorating fields,
    /// so they can be matched to DB columns
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ColumnAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the System.Data.DbType
        /// that will be used for the object for Parameters
        /// </summary>
        //public DbType DbType
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets or sets the Name of the object
        /// </summary>
        public string Field { get; set; }
        public bool Required { get; set; }
        public bool Unique { get; set; }
        public string SearchName { get; set; }
        public string SortName { get; set; }
        public int GridColumnSeq { get; set; }
        public string Title { get; set; }
        public bool ViewAccess { get; set; }
    }
}
