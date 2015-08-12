using System.Collections.Generic;
using System.Xml.Linq;

namespace SuperMap.WinRT.OGC
{
    /// <summary>${mapping_Logical_Tile}</summary>
    /// <example>
    /// 	<code title="C#" description="" id="17af6499-b796-4479-89ba-6e6f44cd7f58" lang="C#">
    /// 1.实现语句：(SQKM &lt; 10) And ((SQKM * SQMI) &gt; 20)，SQKM和SQMI为字段名称。
    /// new Logical
    /// {
    ///    Type = LogicalType.And,
    ///    Filters=
    ///    {
    ///       new Comparison
    ///      {
    ///         PropertyNames = {"SQKM"},
    ///         Type = ComparisonType.LessThan,
    ///         Value = "10"
    ///      },
    ///      new Comparison
    ///     {
    ///        Type = ComparisonType.GreaterThan,
    ///        Expressions =
    ///        {
    ///          new Arithmetic
    ///          {
    ///            Type = ArithmeticType.Mul,
    ///            PropertyNames = {"SQKM","SQMI"},
    ///          }
    ///        },
    ///        Value = "20"
    ///     } 
    ///   }
    /// }
    /// 2.实现语句：(SQKM &gt; 8000000) And (Not (Capital = Null)) ，SQKM和Capital为字段名称。
    /// Logical f2 = new Logical
    /// {
    ///    Type = LogicalType.And,
    ///    Filters =
    ///    {
    ///       new Comparison
    ///      {
    ///         Type=ComparisonType.GreaterThan,
    ///         Value="8000000",
    ///         PropertyNames={"SQKM"}
    ///      },
    ///      new Logical
    ///      {
    ///         Type=LogicalType.Not,
    ///         Filters=
    ///         {
    ///            new Comparison
    ///           {
    ///              Type=ComparisonType.Null,
    ///              PropertyNames={"Capital"}
    ///           },
    ///         }
    ///      },
    ///    }
    /// };</code>
    /// </example>
    public class Logical : Filter
    {
        /// <summary>${mapping_Logical_constructor_D}</summary>
        public Logical()
        {
            filters = new List<Filter>();
        }

        private List<Filter> filters;
        /// <summary>${mapping_Logical_attribute_Filters_D}</summary>
        public List<Filter> Filters
        {
            get { return filters; }
            set { filters = value; }
        }
        /// <summary>${mapping_Logical_attribute_Type_D}</summary>
        public LogicalType Type { get; set; }


        internal override XElement ToXML()
        {
            string ogc = "http://www.opengis.net/ogc";
            if (this.Filters != null)
            {
                XElement xe = new XElement("{" + ogc + "}" + this.Type.ToString());
                switch (this.Type)
                {
                    //二元运算符
                    case LogicalType.And:
                    case LogicalType.Or:
                        //这里只支持两个对象的逻辑运算。
                        //当用户传入多个时，只取前两个。
                        if (filters.Count >= 2)
                        {
                            int i = 0;
                            foreach (var item in this.filters)
                            {
                                if (i < 2)
                                {
                                    XElement value = item.ToXML();
                                    if (value != null)
                                    {
                                        xe.Add(value);
                                        i++;
                                    }
                                }
                            }
                        }
                        break;
                    //一元运算符
                    case LogicalType.Not:
                        foreach (var item in this.filters)
                        {
                            XElement value = item.ToXML();
                            if (value != null)
                            {
                                xe.Add(value);
                                break;
                            }
                        }
                        break;
                }

                return xe;
            }
            return null;
        }
    }
    /// <summary>${mapping_LogicalType_Tile}</summary>
    public enum LogicalType
    {
        /// <summary>${mapping_LogicalType_attribute_And_D}</summary>
        And,
        /// <summary>${mapping_LogicalType_attribute_Or_D}</summary>
        Or,
        /// <summary>${mapping_LogicalType_attribute_Not_D}</summary>
        Not
    }
}
