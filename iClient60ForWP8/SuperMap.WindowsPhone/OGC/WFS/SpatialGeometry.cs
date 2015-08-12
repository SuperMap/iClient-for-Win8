using System.Collections.Generic;
using System.Xml.Linq;
using SuperMap.Web.Core;

namespace SuperMap.Web.OGC
{
    /// <summary>
    /// 	<para>${mapping_SpatialGeometry_Tile}</para>
    /// 	<para>${mapping_SpatialGeometry_Description}</para>
    /// </summary>
    /// <example>
    /// 	<para></para>
    /// 	<code title="C#" description="" id="da42fa57-5ec4-451e-9130-3d95468f00c5" lang="C#">
    /// 实现语句：与某一空间区域相离。
    /// SpatialGeometry spatial = new SpatialGeometry
    /// {
    ///    Type = SpatialType.Disjoint,
    ///    PropertyName = "World",
    ///    Value = new GeoRegion
    ///   {
    ///      Parts = 
    ///      {
    ///          new Point2DCollection
    ///          {
    ///             new Point2D(100,-10),
    ///             new Point2D(140,-10),
    ///             new Point2D(140,0),
    ///             new Point2D(100,0),
    ///             new Point2D(100,-10),
    ///          }
    ///      }
    ///   }
    /// }</code>
    /// </example>
    public class SpatialGeometry : Spatial
    {
        /// <summary>${mapping_SpatialGeometry_constructor_D}</summary>
        public SpatialGeometry()
        { }
        /// <summary>${mapping_SpatialGeometry_attribute_Value_D}</summary>
        public SuperMap.Web.Core.Geometry Value { get; set; }

        //缓冲距离
        /// <summary>${mapping_SpatialGeometry_attribute_Distance_D}</summary>
        public double Distance { get; set; }
        internal override XElement ToXML()
        {
            string ogc = "http://www.opengis.net/ogc";
            string gml = "http://www.opengis.net/gml";
            XElement xe = new XElement("{" + ogc + "}" + this.Type.ToString());
            if (this.Value != null)
            {
                xe.Add(new XElement("{" + ogc + "}PropertyName", this.PropertyName));

                if (this.Value is GeoPoint)
                {
                    GeoPoint point = this.Value as GeoPoint;
                    if (point != null)
                    {
                        XElement coordinates = new XElement("{" + gml + "}coordinates", this.GetCoorStr(point.Location));
                        xe.Add(new XElement("{" + gml + "}Point", coordinates));
                    }
                }
                else if (this.Value is GeoLine)
                {
                    GeoLine line = this.Value as GeoLine;
                    if (line != null)
                    {
                        //lineStringMembers
                        List<XElement> eles = new List<XElement>();
                        foreach (Point2DCollection item in line.Parts)
                        {
                            XElement coordinates = new XElement("{" + gml + "}coordinates", GetCoorStr(item));
                            XElement LineString = new XElement("{" + gml + "}LineString", coordinates);
                            eles.Add(new XElement("{" + gml + "}lineStringMember", LineString));
                        }

                        xe.Add(new XElement("{" + gml + "}MultiLineString", eles));
                    }
                }
                else if (this.Value is GeoRegion)
                {
                    GeoRegion region = this.Value as GeoRegion;
                    if (region != null)
                    {
                        //polygonMembers
                        List<XElement> eles = new List<XElement>();
                        foreach (Point2DCollection item in region.Parts)
                        {
                            XElement coordinates = new XElement("{" + gml + "}coordinates", this.GetCoorStr(item));
                            XElement linearRing = new XElement("{" + gml + "}LinearRing", coordinates);
                            XElement outerBoundaryIs = new XElement("{" + gml + "}outerBoundaryIs", linearRing);
                            XElement polygon = new XElement("{" + gml + "}Polygon", outerBoundaryIs);
                            eles.Add(new XElement("{" + gml + "}polygonMember", polygon));
                        }

                        xe.Add(new XElement("{" + gml + "}MultiPolygon", eles));
                    }
                }
                else
                {
                    //TODO://自定义的Geometry就不用管了，还是开个接口呢？
                }

                //再判断Type的类型，如果是DWithin或者Beyond的话，还要构建Distance值。   
                if (this.Type == SpatialType.Beyond || this.Type == SpatialType.DWithin)
                {
                    xe.Add(new XElement("{" + ogc + "}Distance", this.Distance.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                }
            }
            return xe;
        }
    }
}