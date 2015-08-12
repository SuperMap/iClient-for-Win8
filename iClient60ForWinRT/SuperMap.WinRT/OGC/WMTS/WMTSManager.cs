using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Resources;
using System;

namespace SuperMap.WinRT.OGC
{
    internal sealed class WMTSManager : GetWMTSCapabilities
    {
        public WMTSManager(TiledWMTSLayer layer)
            : base(layer.Url, layer.RequestEncoding)
        {
            WMTSLayer = layer;
            this.Version = layer.Version;
            this.RequestEncoding = layer.RequestEncoding;
        }

        public override GetWMTSCapabilitiesResult ParseCapabilities(XDocument document)
        {
            WMTSManagerResult result = new WMTSManagerResult();
            if (document != null && document.Root != null)
            {
                string nsName = document.Root.Name.NamespaceName;
                XNamespace ns = document.Root.Name.Namespace;
                if (document.Root.Attribute("version") != null)
                {
                    this.Version = document.Root.Attribute("version").Value;
                }

                try
                {
                    result.LayerInfo = (from xmlLayer in document.Descendants(XName.Get("Layer", nsName))
                                        where (xmlLayer.Element(((XElement)xmlLayer.FirstNode).Name.Namespace + "Identifier").Value == WMTSLayer.LayerName)
                                        select new WMTSLayerInfo
                                        {
                                            Name = xmlLayer.Element(((XElement)xmlLayer.FirstNode).Name.Namespace + "Identifier") == null ? string.Empty : xmlLayer.Element(((XElement)xmlLayer.FirstNode).Name.Namespace + "Identifier").Value,
                                            //Bounds值并不是WGS84BoundingBox的值
                                            ////哪个值是有效的？
                                            //Bounds = base.GetBounds(xmlLayer.Element(((XElement)xmlLayer.FirstNode).Name.Namespace + "BoundingBox")),
                                        }).First();

                    result.MatrixSetInfo = (from xmlTileMatrixSet in document.Root.Element(XName.Get("Contents", nsName)).Elements(XName.Get("TileMatrixSet", nsName))
                                            where (xmlTileMatrixSet.Element(((XElement)xmlTileMatrixSet.FirstNode).Name.Namespace + "Identifier").Value == WMTSLayer.TileMatrixSet)
                                            select new WMTSTileMatrixSetInfo
                                            {
                                                Name = xmlTileMatrixSet.Element(((XElement)xmlTileMatrixSet.FirstNode).Name.Namespace + "Identifier") == null ? string.Empty : xmlTileMatrixSet.Element(((XElement)xmlTileMatrixSet.FirstNode).Name.Namespace + "Identifier").Value,
                                                WellKnownScaleSet = xmlTileMatrixSet.Element(XName.Get("WellKnownScaleSet", nsName)) == null ? string.Empty : xmlTileMatrixSet.Element(XName.Get("WellKnownScaleSet", nsName)).Value,
                                                SupportedCRS = base.GetCRS(xmlTileMatrixSet.Element(((XElement)xmlTileMatrixSet.FirstNode).Name.Namespace + "SupportedCRS")),
                                                //获取级数
                                                TileMatrixs = (from xmlTileMatrix in xmlTileMatrixSet.Elements(XName.Get("TileMatrix", nsName))
                                                               select new TileMatrix
                                                               {
                                                                   Name = xmlTileMatrix.Element(((XElement)xmlTileMatrix.FirstNode).Name.Namespace + "Identifier").Value,
                                                                   ScaleDenominator = double.Parse(xmlTileMatrix.Element(XName.Get("ScaleDenominator", nsName)).Value, CultureInfo.InvariantCulture),
                                                                   MatrixWidth = int.Parse(xmlTileMatrix.Element(XName.Get("MatrixWidth", nsName)).Value, CultureInfo.InvariantCulture),
                                                                   TileWidth = int.Parse(xmlTileMatrix.Element(XName.Get("TileWidth", nsName)).Value, CultureInfo.InvariantCulture),
                                                                   MatrixHeight = int.Parse(xmlTileMatrix.Element(XName.Get("MatrixHeight", nsName)).Value, CultureInfo.InvariantCulture),
                                                                   TileHeight = int.Parse(xmlTileMatrix.Element(XName.Get("TileHeight", nsName)).Value, CultureInfo.InvariantCulture),
                                                               }).ToList(),
                                            }).First();
                }
                catch
                {
                    throw new Exception(ExceptionStrings.ParametersError);
                }
            }

            return result;
        }

        private TiledWMTSLayer WMTSLayer;
    }

    internal class WMTSManagerResult : GetWMTSCapabilitiesResult
    {
        public WMTSLayerInfo LayerInfo { get; set; }

        public WMTSTileMatrixSetInfo MatrixSetInfo { get; set; }
    }
}
