using System.Collections.Generic;
using System.Xml.Linq;

namespace SuperMap.Web.OGC
{
    /// <summary>${mapping_Comparison_Tile}</summary>
    /// <example>
    /// 	<code title="C#" description="" id="d9732c6a-9e7f-4f2a-90fb-fd694415edd0" lang="C#">
    /// 1、当Type属性设置为ComparisonType.Null时：
    ///  
    /// 实现语句：Capital = Null，Capital为字段名。
    ///  
    /// Comparison compare1 = new Comparison 
    /// { 
    ///    PropertyNames = { "Capital" }, 
    ///    Type = ComparisonType.Null 
    /// };
    ///  
    /// 2、当Type属性设置为ComparisonType.Between时：
    /// 实现语句：100 &lt; SmID &lt; 110，SmID为字段名。
    /// Comparison compare2= new Comparison
    /// {
    ///    Type = ComparisonType.Between,
    ///    PropertyNames = { "SmID" },
    ///    Expressions =
    ///    {
    ///       new Arithmetic{ Value="100"},
    ///       new Arithmetic{ Value="110"},
    ///    }
    /// };
    ///  
    /// 3、实现语句：Capital = “华盛顿”，Capital 为字段名。
    /// Comparison compare3= new Comparison
    /// {
    ///    PropertyNames = { "Capital" },
    ///    Type = ComparisonType.EqualTo,
    ///    Value = "华盛顿"
    /// };
    ///  
    /// 4、实现语句：(SQKM * SQMI) &gt; 20
    /// Comparison compare4 = new Comparison
    /// {
    ///    Type = ComparisonType.GreaterThan,
    ///    Expressions =
    ///    {
    ///       new Arithmetic
    ///      {
    ///         Type = ArithmeticType.Mul,
    ///         PropertyNames = {"SQKM","SQMI"},
    ///      }
    ///    },
    ///      Value = "20"
    /// }</code>
    /// </example>
    public class Comparison : Filter
    {
        Dictionary<string, string> typePair = new Dictionary<string, string>();
        /// <summary>${mapping_Comparison_constructor_D}</summary>
        public Comparison()
        {
            typePair.Add("EqualTo", "PropertyIsEqualTo");
            typePair.Add("NotEqualTo", "PropertyIsNotEqualTo");
            typePair.Add("LessThan", "PropertyIsLessThan");
            typePair.Add("GreaterThan", "PropertyIsGreaterThan");
            typePair.Add("LessThanOrEqualTo", "PropertyIsLessThanOrEqualTo");
            typePair.Add("GreaterThanOrEqualTo", "PropertyIsGreaterThanOrEqualTo");
            typePair.Add("Like", "PropertyIsLike");
            typePair.Add("Null", "PropertyIsNull");
            typePair.Add("Between", "PropertyIsBetween");
        }
        private List<string> propertynames = new List<string>();

        //type of the comparison
        /// <summary>${mapping_Comparison_attribute_Type_D}</summary>
        public ComparisonType Type { get; set; }

        //name of the context property to compare
        /// <summary>${mapping_Comparison_attribute_PropertyName_D}</summary>
        public List<string> PropertyNames
        {
            get { return names; }
            set { names = value; }
        }
        private List<string> names = new List<string>();

        //TODO:对通配符的字符进行验证。<Literal>20</Literal>
        /// <summary>${mapping_Comparison_attribute_Value_D}</summary>
        public string Value { get; set; }
        /// <summary>${mapping_Comparison_attribute_Expression_D}</summary>
        public List<Arithmetic> Expressions
        {
            get { return exprs; }
            set { exprs = value; }
        }
        private List<Arithmetic> exprs = new List<Arithmetic>();
        ////<Literal>20</Literal>
        ///// <summary>${mapping_Comparison_attribute_LowerBoundary_D}</summary>
        //public string LowerBoundary { get; set; }
        ///// <summary>${mapping_Comparison_attribute_UpperBoundary_D}</summary>
        //public string UpperBoundary { get; set; }

        internal override XElement ToXML()
        {
            string ogc = "http://www.opengis.net/ogc";
            XElement root = new XElement("{" + ogc + "}" + typePair[this.Type.ToString()]);

            switch (this.Type)
            {
                case ComparisonType.EqualTo:
                case ComparisonType.NotEqualTo:
                case ComparisonType.LessThan:
                case ComparisonType.GreaterThan:
                case ComparisonType.LessThanOrEqualTo:
                case ComparisonType.GreaterThanOrEqualTo:
                    if (this.PropertyNames != null && this.PropertyNames.Count > 1)
                    {
                        //PropertyName
                        //PropertyName
                        int nameCount = 0;
                        foreach (string name in this.PropertyNames)
                        {
                            if (nameCount > 2)
                                break;
                            root.Add(new XElement("{" + ogc + "}PropertyName", name));
                            nameCount++;
                        }
                        return root;
                    }
                    else if (this.propertynames != null && this.PropertyNames.Count == 1)
                    {
                        root.Add(new XElement("{" + ogc + "}PropertyName", this.PropertyNames[0]));
                        if (this.Expressions != null && this.Expressions.Count > 0)
                        {
                            //PropertyName
                            //Expression
                            XElement subValue = this.Expressions[0].ToXML();
                            if (subValue != null)
                            {
                                root.Add(subValue);
                            }
                        }
                        else
                        {
                            //PropertyName
                            //Value
                            root.Add(new XElement("{" + ogc + "}Literal", this.Value));
                        }
                        return root;
                    }
                    else
                    {
                        if (this.Expressions != null && this.Expressions.Count > 1)
                        {
                            //Expression
                            //Expression
                            int exprCount = 0;
                            foreach (var expr in this.Expressions)
                            {
                                if (exprCount > 2)
                                    break;
                                root.Add(expr.ToXML());
                                exprCount++;
                            }
                        }
                        else if (this.Expressions != null && this.Expressions.Count == 1)
                        {
                            //Expression
                            //Value
                            root.Add(this.Expressions[0].ToXML());
                            root.Add(new XElement("{" + ogc + "}Literal", this.Value));
                        }
                        return root;
                    }
                case ComparisonType.Like:
                    //TODO:这里需要处理通配符                   
                    root.SetAttributeValue("wildCard", "*");
                    root.SetAttributeValue("singleChar", ".");
                    root.SetAttributeValue("escape", "!");
                    if (this.PropertyNames != null && this.PropertyNames.Count > 0)
                    {
                        root.Add(new XElement("{" + ogc + "}PropertyName", this.PropertyNames[0]));
                        root.Add(new XElement("{" + ogc + "}Literal", this.Value));
                        return root;
                    }
                    return root;
                case ComparisonType.Null:
                    if (this.PropertyNames != null && this.PropertyNames.Count > 0)
                    {
                        root.Add(new XElement("{" + ogc + "}PropertyName", this.PropertyNames[0]));
                        return root;
                    }
                    break;
                case ComparisonType.Between:
                    //lower.Add(new XElement("{" + ogc + "}Literal", this.LowerBoundary));
                    if (this.PropertyNames != null && this.PropertyNames.Count > 0)
                    {
                        if (this.Expressions != null && this.Expressions.Count > 1)
                        {
                            XElement lower = new XElement("{" + ogc + "}LowerBoundary");
                            XElement sub1 = this.Expressions[0].ToXML();
                            if (sub1 != null)
                            {
                                lower.Add(sub1);
                            }
                            else
                            {
                                return null;
                            }
                            XElement upper = new XElement("{" + ogc + "}UpperBoundary");
                            XElement sub2 = this.Expressions[1].ToXML();
                            if (sub1 != null)
                            {
                                upper.Add(sub2);
                            }
                            else
                            {
                                return null;
                            }
                            root.Add(lower);
                            root.Add(upper);
                        }
                        root.Add(new XElement("{" + ogc + "}PropertyName", this.PropertyNames[0]));
                        return root;
                    }
                    else if (this.PropertyNames == null || (this.PropertyNames != null && this.PropertyNames.Count == 0))
                    {
                        if (this.Expressions != null && this.Expressions.Count > 2)
                        {
                            XElement expr1 = this.Expressions[0].ToXML();
                            if (expr1 != null)
                            {
                                root.Add(expr1);
                            }
                            else
                            {
                                return null;
                            }

                            XElement lower = new XElement("{" + ogc + "}LowerBoundary");
                            XElement sub1 = this.Expressions[1].ToXML();
                            if (sub1 != null)
                            {
                                lower.Add(sub1);
                            }
                            else
                            {
                                return null;
                            }
                            XElement upper = new XElement("{" + ogc + "}UpperBoundary");
                            XElement sub2 = this.Expressions[2].ToXML();
                            if (sub1 != null)
                            {
                                upper.Add(sub2);
                            }
                            else
                            {
                                return null;
                            }
                            root.Add(lower);
                            root.Add(upper);
                        }
                    }

                    return root;
            }

            //可以用作逻辑表达式的枚举值：
            //EqualTo,NotEqualTo,LessThan,GreaterThan,LessThanOrEqualTo,GreaterThanOrEqualTo 
            return null;
        }
    }
    /// <summary>${mapping_ComparisonType_Tile}</summary>
    public enum ComparisonType
    {
        /// <summary>${mapping_ComparisonType_attribute_EqualTo_D}</summary>
        EqualTo,                //value
        /// <summary>${mapping_ComparisonType_attribute_NotEqualTo_D}</summary>
        NotEqualTo,             //value
        /// <summary>${mapping_ComparisonType_attribute_LessThan_D}</summary>
        LessThan,               //value
        /// <summary>${mapping_ComparisonType_attribute_GreaterThan_D}</summary>
        GreaterThan,            //value
        /// <summary>${mapping_ComparisonType_attribute_LessThanOrEqualTo_D}</summary>
        LessThanOrEqualTo,      //value
        /// <summary>${mapping_ComparisonType_attribute_GreaterThanOrEqualTo_D}</summary>
        GreaterThanOrEqualTo,   //value
        /// <summary>${mapping_ComparisonType_attribute_Like_D}</summary>
        Like,                   //value
        /// <summary>${mapping_ComparisonType_attribute_Null_D}</summary>
        Null,
        /// <summary>${mapping_ComparisonType_attribute_Between_D}</summary>
        Between,                //LowerBoundary/UpperBoundary
    }
}
