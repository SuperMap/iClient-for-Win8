﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Xml.Linq;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Resources;
using System.IO;
using System.Threading.Tasks;

namespace SuperMap.WinRT.OGC
{
    /// <summary>
    /// 	<para>${mapping_GetWMTSCapabilities_Tile}</para>
    /// 	<para>${mapping_GetWMTSCapabilities_Description}</para>
    /// </summary>
    public class GetWMTSCapabilities
    {
        /// <summary>${mapping_GetWMTSCapabilities_constructor_D}</summary>
        public GetWMTSCapabilities()
        { }
        /// <summary>${mapping_GetWMTSCapabilities_constructor_string_D}</summary>
        public GetWMTSCapabilities(string url)
            : this()
        {
            this.Url = url;
        }

        /// <summary>${mapping_GetWMTSCapabilities_constructor_string_D}</summary>
        public GetWMTSCapabilities(string url, RequestEncoding encoding)
            : this()
        {
            this.Url = url;
            this.RequestEncoding = encoding;
        }
        /// <summary>${mapping_GetWMTSCapabilities_method_ProcessRequest_D}</summary>
        public async Task<GetWMTSCapabilitiesResult> ProcessAsync()
        {
            if (!string.IsNullOrEmpty(this.Url) && this.Url.EndsWith("/"))
            {
                this.Url = this.Url.TrimEnd('/');
            }

            if (RequestEncoding == RequestEncoding.KVP)
            {
                StringBuilder sb = new StringBuilder(this.Url);
                sb.Append("?SERVICE=WMTS");
                sb.Append("&VERSION=").Append(this.Version);
                sb.Append("&REQUEST=GetCapabilities");
                this.Url = sb.ToString();
            }
            else if (RequestEncoding == RequestEncoding.REST)
            {
                StringBuilder sb = new StringBuilder(this.Url);
                sb.Append("/").Append(this.Version);
                sb.Append("/WMTSCapabilities.xml");
                this.Url = sb.ToString();
            }

            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(this.Url, UriKind.RelativeOrAbsolute));
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync().ConfigureAwait(false);
                reader.Dispose();
                XDocument doc = XDocument.Parse(result);

                if (doc != null && doc.Document != null && doc.Document.Root != null && doc.Document.Root.Name.LocalName == "ExceptionReport")
                {
                    string code = string.Empty;
                    string locator = string.Empty;
                    string message = string.Empty;
                    XElement firstNode = doc.Document.Root.FirstNode as XElement;

                    if (firstNode != null)
                    {
                        XAttribute exceptionCodeAttribute = firstNode.Attribute("exceptionCode");
                        if (exceptionCodeAttribute != null)
                        {
                            code = exceptionCodeAttribute.Value;
                        }
                        XAttribute locatorAttribute = firstNode.Attribute("locator");
                        if (locatorAttribute != null)
                        {
                            locator = locatorAttribute.Value;
                        }
                        message = firstNode.Value;
                    }

                    throw new RequestException(code, locator, message);
                }
                else
                {
                    return this.ParseCapabilities(doc);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var response = ex.Response as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    var message = reader.ReadToEnd();
                    throw new WebException(message, ex.InnerException);
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>${mapping_GetWMTSCapabilities_method_ParseCapabilities_D}</summary>
        public virtual GetWMTSCapabilitiesResult ParseCapabilities(XDocument document)
        {
            GetWMTSCapabilitiesResult result = new GetWMTSCapabilitiesResult();
            if (document != null && document.Root != null)
            {
                string nsName = document.Root.Name.NamespaceName;
                XNamespace ns = document.Root.Name.Namespace;
                XAttribute versionAttr = document.Root.Attribute("version");
                if (versionAttr != null)
                {
                    result.Version = versionAttr.Value;
                }

                foreach (var layer in (from xmlLayer in document.Descendants(XName.Get("Layer", nsName))
                                       select new WMTSLayerInfo
                                       {
                                           Name = xmlLayer.Element(((XElement)xmlLayer.FirstNode).Name.Namespace + "Identifier") == null ? string.Empty : xmlLayer.Element(((XElement)xmlLayer.FirstNode).Name.Namespace + "Identifier").Value,
                                           ////哪个值是有效的？Bounds值并不是WGS84BoundingBox值
                                           //Bounds = GetBounds(xmlLayer.Element(((XElement)xmlLayer.FirstNode).Name.Namespace + "WGS84BoundingBox")),
                                           ImageFormat = GetSupportFormats(xmlLayer.Elements(XName.Get("Format", nsName)).ToList()),
                                           Style = GetSupportStyle(xmlLayer.Elements(XName.Get("Style", nsName)).ToList()),
                                           TileMatrixSetLinks = GetSupportLinks(xmlLayer.Elements(XName.Get("TileMatrixSetLink", nsName)).ToList())
                                       }))
                {
                    result.LayerInfos.Add(layer);
                };

                foreach (var tileMatrixSet in (from xmlTileMatrixSet in document.Root.Element(XName.Get("Contents", nsName)).Elements(XName.Get("TileMatrixSet", nsName))
                                               select new WMTSTileMatrixSetInfo
                                               {
                                                   Name = xmlTileMatrixSet.Element(((XElement)xmlTileMatrixSet.FirstNode).Name.Namespace + "Identifier") == null ? string.Empty : xmlTileMatrixSet.Element(((XElement)xmlTileMatrixSet.FirstNode).Name.Namespace + "Identifier").Value,
                                                   WellKnownScaleSet = xmlTileMatrixSet.Element(XName.Get("WellKnownScaleSet", nsName)) == null ? string.Empty : xmlTileMatrixSet.Element(XName.Get("WellKnownScaleSet", nsName)).Value,
                                                   SupportedCRS = GetCRS(xmlTileMatrixSet.Element(((XElement)xmlTileMatrixSet.FirstNode).Name.Namespace + "SupportedCRS")),
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
                                               }))
                {
                    result.WMTSTileMatrixSetInfo.Add(tileMatrixSet);
                };
            }

            return result;
        }

        /// <summary>${mapping_GetWMTSCapabilities_method_GetCRS_D}</summary>
        internal CoordinateReferenceSystem GetCRS(XElement elmt)
        {
            CoordinateReferenceSystem crs = new CoordinateReferenceSystem();
            if (elmt != null)
            {
                string value = elmt.Value;
                string[] temp = value.Trim().Split(':');
                if (temp != null)
                {
                    int wkid = int.Parse(temp.Last(), CultureInfo.InvariantCulture);
                    crs.WKID = wkid;
                }
            }
            return crs;
        }

        /// <summary>${mapping_GetWMTSCapabilities_method_GetSupportLinks_D}</summary>
        internal List<TileMatrixSetLink> GetSupportLinks(List<XElement> links)
        {
            //暂时不搞TileMatrixSetLimits
            List<TileMatrixSetLink> objectLinks = new List<TileMatrixSetLink>();
            if (links != null)
            {
                foreach (var item in links)
                {
                    objectLinks.Add(new TileMatrixSetLink
                    {
                        TileMatrixSet = item.Element(XName.Get("TileMatrixSet", item.Name.NamespaceName)).Value
                    });
                }
            }

            return objectLinks;
        }

        /// <summary>${mapping_GetWMTSCapabilities_method_GetSupportStyle_D}</summary>
        internal string GetSupportStyle(List<XElement> styles)
        {
            // 取isDefault=true的值
            string realStyle = string.Empty;
            if (styles != null)
            {
                foreach (var item in styles)
                {
                    if (item.Attribute("isDefault") != null && bool.Parse(item.Attribute("isDefault").Value) == true)
                    {
                        realStyle = item.Element(((XElement)item.FirstNode).Name.Namespace + "Identifier").Value;
                    }
                }
            }

            return realStyle;
        }

        /// <summary>${mapping_GetWMTSCapabilities_method_GetSupportFormats_D}</summary>
        internal List<string> GetSupportFormats(List<XElement> elmts)
        {
            List<string> objectElms = new List<string>();
            if (objectElms != null)
            {
                foreach (var elmt in elmts)
                {
                    objectElms.Add(elmt.Value);
                }
            }

            return objectElms;
        }

        /// <summary>${mapping_GetWMTSCapabilities_attribute_Url_D}</summary>
        public string Url { get; set; }
        ///// <summary>${mapping_GetWMTSCapabilities_attribute_Error_D}</summary>
        //public Exception Error { get; private set; }

        private string version = "1.0.0";
        /// <summary>${mapping_GetWMTSCapabilities_attribute_Version_D}</summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>${mapping_GetWMTSCapabilities_attribute_RequestEncoding_D}</summary>
        public RequestEncoding RequestEncoding { get; set; }
    }
}
