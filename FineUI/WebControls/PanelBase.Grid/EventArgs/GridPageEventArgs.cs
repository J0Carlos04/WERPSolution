
#region Comment

/*
 * Project：    FineUI
 * 
 * FileName:    GridPageEventArgs.cs
 * CreatedOn:   2008-06-25
 * CreatedBy:   30372245@qq.com
 * 
 * 
 * Description：
 *      ->
 *   
 * History：
 *      ->
 * 
 * 
 * 
 * 
 */

#endregion

using System;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Web.UI;


namespace FineUI
{
    /// <summary>
    /// 表格分页事件参数
    /// </summary>
    public class GridPageEventArgs : EventArgs
    {

        private int _newPageIndex;

        /// <summary>
        /// 新页面的索引
        /// </summary>
        public int NewPageIndex
        {
            get { return _newPageIndex; }
        }

        private int _oldPageIndex;

        /// <summary>
        /// 老页面的索引
        /// </summary>
        public int OldPageIndex
        {
            get { return _oldPageIndex; }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="newPageIndex">新页面的索引</param>
        public GridPageEventArgs(int newPageIndex)
        {
            _newPageIndex = newPageIndex;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="newPageIndex">新页面的索引</param>
        /// <param name="oldPageIndex">老页面的索引</param>
        public GridPageEventArgs(int newPageIndex, int oldPageIndex)
        {
            _newPageIndex = newPageIndex;
            _oldPageIndex = oldPageIndex;
        }

    }
}



