using System.Xml.Linq;
using SuperMap.Web.Core;

namespace SuperMap.Web.OGC
{
    /// <summary>
    /// 	<para>${mapping_SpatialRect_Tile}</para>
    /// 	<para>${mapping_SpatialRect_Description}</para>
    /// </summary>
    /// <example>
    /// 	<para></para>
    /// 	<code title="C#" description="" id="304d5a51-c38c-4b0b-a16d-6609e30d9601" lang="C#">
    /// 实现语句：与某一空间矩形区域内。
    /// SpatialRect spatial = new SpatialRect
    /// {
    ///     Type = SpatialType.BBOX,
    ///     PropertyName = "World",
    ///     Value = new Rectangle2D(100,-10,180,30)
    /// }</code>
    /// </example>
    public class SpatialRect : Spatial
    {
        /// <summary>${mapping_SpatialRect_constructor_D}</summary>
        public SpatialRect()
        { }
        /// <summary>${mapping_SpatialRect_attribute_Value_D}</summary>
        public Rectangle2D Value { get; set; }

        internal override XElement ToXML()
        {
            string ogc = "http://www.opengis.net/ogc";
            string gml = "http://www.opengis.net/gml";

            XElement xe = new XElement("{" + ogc + "}" + this.Type.ToString());
            xe.Add(new XElement("{" + ogc + "}PropertyName", this.PropertyName));

            string coorStr = string.Empty;
            if (!this.Value.IsEmpty)
            {
                coorStr = this.GetCoorStr(this.Value.BottomLeft, this.Value.TopRight);
            }

            XElement gmlCoor = new XElement("{" + gml + "}coordinates", coorStr);
            xe.Add(new XElement("{" + gml + "}Box", gmlCoor));

            return xe;
        }
    }
}
