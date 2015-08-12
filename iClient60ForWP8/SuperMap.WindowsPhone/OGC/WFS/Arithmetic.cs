using System.Xml.Linq;
using System.Collections.Generic;

namespace SuperMap.Web.OGC
{

    // Value属性和Expressions属性二者选其一，当设置了Expression时，Value不起作用。
    /// <summary>${mapping_Arithmetic_Tile}</summary>
    /// <example>
    /// 	<code title="C#" description="" id="1e5f32ef-aea6-4dc0-849f-87268423ecc6" lang="C#">
    /// 实现语句：SmArea / SmPerimeter，SmPerimeter和SmArea 是字段名称。
    /// Arithmetic arithmetic = new Arithmetic
    /// {
    ///     PropertyNames={"SmArea","SmPerimeter"},
    ///     Type=ArithmeticType.Div,
    /// }</code>
    /// </example>
    public class Arithmetic
    {
        /// <summary>${mapping_Arithmetic_constructor_D}</summary>
        public Arithmetic()
        { }
        /// <summary>${mapping_Arithmetic_attribute_Type_D}</summary>
        public ArithmeticType Type { get; set; }
        /// <summary>${mapping_Arithmetic_attribute_PropertyName_D}</summary>
        public List<string> PropertyNames
        {
            get { return names; }
            set { names = value; }
        }
        private List<string> names = new List<string>();

        /// <summary>${mapping_Arithmetic_attribute_Value_D}</summary>
        public string Value { get; set; }
        /// <summary>${mapping_Arithmetic_attribute_Expression_D}</summary>
        public List<Arithmetic> Expressions
        {
            get { return exprs; }
            set { exprs = value; }
        }
        private List<Arithmetic> exprs = new List<Arithmetic>();

        //如果Expressions.Count>0的话，就递归表达式
        internal XElement ToXML()
        {
            string ogc = "http://www.opengis.net/ogc";
            XElement root = new XElement("{" + ogc + "}" + this.Type.ToString());
            if (this.PropertyNames != null && this.PropertyNames.Count > 1)  //两个PropertyName
            {
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
            else if (this.PropertyNames != null && this.PropertyNames.Count == 1)  //一个Property的情况，这是需要考虑Expressions，
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
            else //PropertyNames为空或者PropertyNames.Count为0时
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
                    return root;
                }
                else if (this.Expressions != null && this.Expressions.Count == 1)
                {
                    //Expression
                    //Value
                    XElement xe = this.Expressions[0].ToXML();
                    if (xe != null)
                    {
                        root.Add(xe);
                    }
                    root.Add(new XElement("{" + ogc + "}Literal", this.Value));
                    return root;
                }
                else
                {
                    return new XElement("{" + ogc + "}Literal", this.Value);
                }
            }
        }
    }
    /// <summary>${mapping_ArithmeticType_Tile}</summary>
    public enum ArithmeticType
    {
        /// <summary>${mapping_ArithmeticType_attribute_Add_D}</summary>
        Add,
        /// <summary>${mapping_ArithmeticType_attribute_Sub_D}</summary>
        Sub,
        /// <summary>${mapping_ArithmeticType_attribute_Mul_D}</summary>
        Mul,
        /// <summary>${mapping_ArithmeticType_attribute_Div_D}</summary>
        Div
    }
}
