
#region Comment

/*
 * Project：    FineUI
 * 
 * FileName:    ThemeType.cs
 * CreatedOn:   2008-08-20
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
using System.Text;

namespace FineUI
{
    /// <summary>
    /// 主题
    /// </summary>
    public enum Theme
    {
        /// <summary>
        /// 蓝色经典主题
        /// </summary>
        Blue,
        /// <summary>
        /// 灰色主题
        /// </summary>
        Gray,
        /// <summary>
        /// 海王星主题（默认值）
        /// </summary>
        Neptune,
        /// <summary>
        /// 海卫一主题
        /// </summary>
        Triton,
        /// <summary>
        /// 小清新主题
        /// </summary>
        Crisp
    }

    /// <summary>
    /// 主题的类型名称
    /// </summary>
    internal static class ThemeHelper
    {
        public static string GetName(Theme type)
        {
            string result = String.Empty;

            switch (type)
            {
                case Theme.Blue:
                    result = "classic";
                    break;
                case Theme.Gray:
                    result = "gray";
                    break;
                case Theme.Triton:
                    result = "triton";
                    break;
                case Theme.Neptune:
                    result = "neptune";
                    break;
                case Theme.Crisp:
                    result = "crisp";
                    break;
            }

            return result;
        }
    }
}