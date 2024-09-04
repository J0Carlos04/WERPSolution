using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

/// <summary>
/// Summary description for Item
/// </summary>
public class Item
{
    #region Properties
    [Column(Field = "Seq")]
    public int Seq { get; set; }
    [Column(Field = "Key")]
    public string Key { get; set; }
    [Column(Field = "Value")]
    public string Value { get; set; }
    [Column(Field = "Text")]
    public string Text { get; set; }
    [Column(Field = "Total")]
    public string Total { get; set; }
    [Column(Field = "Atribut")]
    public string Atribut { get; set; }
    [Column(Field = "Description")]
    public string Description { get; set; }
    public bool IsChecked { get; set; }
    #endregion

    #region Methods
    public static List<Item> GetOperatorsText()
    {
        return new List<Item>
        {
            new Item { Text = "Equal (=)", Value = "=" },
            new Item { Text = "Not equal to (<>)", Value = "<>"  },
            new Item { Text = "Contains", Value = "Like"  },
            new Item { Text = "Not Contains", Value = "Not Like"  },
            new Item { Text = "IN (Separate with comma)", Value = "IN"  },
            new Item { Text = "Not IN (Separate with comma)", Value = "Not IN"  },
        };
    }
    public static List<Item> GetOperatorsFilter()
    {
        return new List<Item>
        {
            new Item { Text = "Equal (=)", Value = "=" },
            new Item { Text = "Not equal to (<>)", Value = "<>"  },
            new Item { Text = "Greater than (>)", Value = ">"  },
            new Item { Text = "Less than (<)", Value = "<"  },
            new Item { Text = "Greater than or equal to (>=))", Value = ">="  },
            new Item { Text = "Less than or equal to (<=)", Value = "<="  },
            new Item { Text = "Contains", Value = "Like"  },
            new Item { Text = "Not Contains", Value = "Not Like"  },
            new Item { Text = "IN (Separate with comma)", Value = "IN"  },
            new Item { Text = "Not IN (Separate with comma)", Value = "Not IN"  },
            new Item { Text = "Between", Value = "Between"  }
        };
    }
    public static List<Item> GetOperatorsDateTime()
    {
        return new List<Item>
        {
            new Item { Text = "Equal (=)", Value = "=" },
            new Item { Text = "Not equal to (<>)", Value = "<>"  },
            new Item { Text = "Greater than (>)", Value = ">"  },
            new Item { Text = "Less than (<)", Value = "<"  },
            new Item { Text = "Greater than or equal to (>=))", Value = ">="  },
            new Item { Text = "Less than or equal to (<=)", Value = "<="  },
            new Item { Text = "IN (Ex : 01/01/2022,02/02/2022)", Value = "IN"  },
            new Item { Text = "Not IN (Ex : 01/01/2022,02/02/2022)", Value = "Not IN"  },
            new Item { Text = "Between", Value = "Between"  }
        };
    }
    public static List<Item> GetOperatorsBool()
    {
        return new List<Item>
        {
            new Item { Text = "Equal (=)", Value = "=" },
            new Item { Text = "Not equal to (<>)", Value = "<>"  }
        };
    }
    public static List<Item> GetOperators()
    {
        return new List<Item>
        {
            new Item { Text = "Equal (=)", Value = "=" },
            new Item { Text = "Not equal to (<>)", Value = "<>"  },
            new Item { Text = "Greater than (>)", Value = ">"  },
            new Item { Text = "Less than (<)", Value = "<"  },
            new Item { Text = "Greater than or equal to (>=))", Value = ">="  },
            new Item { Text = "Less than or equal to (<=)", Value = "<="  },
            new Item { Text = "Contains", Value = "Like"  },
            new Item { Text = "Not Contains", Value = "Not Like"  },
            new Item { Text = "IN (Separate with comma)", Value = "IN"  },
            new Item { Text = "Not IN (Separate with comma)", Value = "Not IN"  },
            new Item { Text = "Between", Value = "Between"  }
        };
    }    
    public static List<object> GetActiveMasterData(string TableName, string Field = "Name")
    {
        return DataAccess.GetDataByQuery(0, $"Select Id Value, {Field} Text from {TableName} where Active = 1", new Item());
    }
    #endregion
}