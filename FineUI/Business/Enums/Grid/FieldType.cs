﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FineUI
{
    /// <summary>
    /// 表格可编辑字段的类型
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// 自动（默认值）
        /// </summary>
        Auto,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 整型
        /// </summary>
        Int,
        /// <summary>
        /// 浮点数
        /// </summary>
        Float,
        /// <summary>
        /// 双精度浮点数（精度为15~16）
        /// </summary>
        Double,
        /// <summary>
        /// 布尔型
        /// </summary>
        Boolean,
        /// <summary>
        /// 日期
        /// </summary>
        Date
    }

    /// <summary>
    /// 表格可编辑字段的类型名称
    /// </summary>
    internal static class FieldTypeName
    {
        public static string GetName(FieldType type)
        {
            string result = String.Empty;

            switch (type)
            {
                case FieldType.Auto:
                    result = "auto";
                    break;
                case FieldType.String:
                    result = "string";
                    break;
                case FieldType.Int:
                    result = "int";
                    break;
                case FieldType.Float:
                    result = "float";
                    break;
                case FieldType.Boolean:
                    result = "boolean";
                    break;
                    // ExtJS v6.0不支持客户端指定 double 字符串，需要改为 float
                    // 客户端处理一样，这个设置只影响服务器端的解析
                case FieldType.Double:
                    result = "float";
                    break;
                case FieldType.Date:
                    result = "date";
                    break;
            }

            return result;
        }
    }
}