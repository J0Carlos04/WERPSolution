﻿
#region Comment

/*
 * Project：    FineUI
 * 
 * FileName:    ListItem.cs
 * CreatedOn:   2008-04-23
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
using System.Web.UI.Design.WebControls;

using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Collections;


namespace FineUI
{
    /// <summary>
    /// 列表项
    /// </summary>
    [ToolboxItem(false)]
    [ParseChildren(true, DefaultProperty = "Text")]
    [Description("列表项")]
    [PersistChildren(false)]
    [ControlBuilder(typeof(NotAllowWhitespaceLiteralsBuilder))]
    public class ListItem
    {
        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public ListItem()
        {

        }

        public ListItem(string text)
        {
            Text = text;
            Value = text;
        }

        public ListItem(string text, bool selected)
        {
            Text = text;
            Value = text;
            Selected = selected;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        public ListItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        /// <param name="selected">是否选中</param>
        public ListItem(string text, string value, bool selected)
        {
            Text = text;
            Value = value;
            Selected = selected;
        }

        #endregion

        #region Properties

        private bool _selected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [Description("是否选中")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

        private string _text = String.Empty;
        /// <summary>
        /// 显示的文本
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("显示的文本")]
        [NotifyParentProperty(true)]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value; //HttpUtility.HtmlEncode(value);
            }
        }

        private string _value = String.Empty;
        /// <summary>
        /// 值
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("值")]
        [NotifyParentProperty(true)]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private bool _enableSelect = true;
        /// <summary>
        /// 是否可选择
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(true)]
        [Description("是否可选择")]
        [NotifyParentProperty(true)]
        public bool EnableSelect
        {
            get
            {
                return _enableSelect;
            }
            set
            {
                _enableSelect = value;
            }
        }

        private int _simulateTreeLevel = 0;

        /// <summary>
        /// 模拟树的层次（从0开始为根节点）
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(0)]
        [Description("模拟树的层次（从0开始为根节点）")]
        [NotifyParentProperty(true)]
        public int SimulateTreeLevel
        {
            get
            {
                return _simulateTreeLevel;
            }
            set
            {
                _simulateTreeLevel = value;
            }
        }

        #endregion

    }
}
