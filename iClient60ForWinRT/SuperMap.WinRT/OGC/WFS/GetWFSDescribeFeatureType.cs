﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Net;
using System.IO;
using SuperMap.WinRT.Resources;
using System.Threading.Tasks;

namespace SuperMap.WinRT.OGC
{
    /// <summary>
    /// 	<para>${mapping_GetWFSDescribeFeatureType_Tile}</para>
    /// 	<para>${mapping_GetWFSDescribeFeatureType_Description}</para>
    /// </summary>
    public class GetWFSDescribeFeatureType : WFSServiceBase
    {
        private Dictionary<string, string> nameDic = new Dictionary<string, string>();
        private Dictionary<string, string> imports;

        /// <summary>${mapping_GetWFSDescribeFeatureType_constructor_D}</summary>
        public GetWFSDescribeFeatureType()
        { }
        /// <summary>${mapping_GetWFSDescribeFeatureType_constructor_string_D}</summary>
        public GetWFSDescribeFeatureType(string url)
        {
            this.Url = url;
        }
        /// <summary>${mapping_GetWFSDescribeFeatureType_method_ProcessAsync_D}</summary>
        public async Task<Dictionary<string, List<WFSFeatureDescription>>> ProcessAsync()
        {
            string url = GetFinalUrl();

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(ExceptionStrings.ParametersError);
            }

            HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(url + "&t=" + DateTime.Now.Ticks));
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync().ConfigureAwait(false);
                reader.Dispose();
                XDocument document = XDocument.Parse(result);
                this.ThrowResultExecption(document);
                return await ParseSuccessResult(document);
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                var message = reader.ReadToEnd();
                throw new WebException(message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        internal override string GetFinalUrl()
        {
            if (string.IsNullOrEmpty(Url))
            {
                return string.Empty;
            }

            string url = this.Url;
            if (!string.IsNullOrEmpty(url) && url.EndsWith("/"))
            {
                url = url.TrimEnd('/');
            }

            if (TypeNames.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (var subName in this.TypeNames)
                {
                    list.Add(subName);
                }
                return url += "?SERVICE=WFS" + "&VERSION=" + this.Version + "&REQUEST=DescribeFeatureType" +
                    "&OUTPUTFORMAT=XMLSCHEMA" + "&TYPENAME=" + string.Join(",", list);
            }
            else
            {
                return url += "?SERVICE=WFS" + "&VERSION=" + this.Version + "&REQUEST=DescribeFeatureType" + "&OUTPUTFORMAT=XMLSCHEMA";
            }
        }

        private List<string> featureNames = new List<string>();
        /// <summary>${mapping_GetWFSDescribeFeatureType_attribute_TypeName_D}</summary>
        public List<string> TypeNames
        {
            get { return featureNames; }
            private set { featureNames = value; }
        }

        private string GetSimpleName(string complexName)
        {
            if (!string.IsNullOrEmpty(complexName))
            {
                return complexName.Substring(complexName.LastIndexOf(':') + 1);
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task<Dictionary<string, List<WFSFeatureDescription>>> ParseSuccessResult(XDocument document)
        {
            if (document != null && document.Root != null)
            {
                //首先解析一下Import节点吧
                string xsd = "http://www.w3.org/2001/XMLSchema";
                XAttribute xsdAttr = document.Root.Attribute(XName.Get("xsd"));
                if (xsdAttr != null)
                {
                    xsd = xsdAttr.Value;
                }

                XAttribute targetNamespace = document.Root.Attribute(XName.Get("targetNamespace"));

                if (targetNamespace == null)
                {
                    imports = new Dictionary<string, string>();
                    foreach (var importNode in document.Root.Elements(XName.Get("import", xsd)))
                    {
                        XAttribute nameSpace = importNode.Attribute(XName.Get("namespace"));
                        XAttribute schemaLocation = importNode.Attribute(XName.Get("schemaLocation"));

                        if (nameSpace != null && schemaLocation != null)
                        {
                            try
                            {
                                HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(schemaLocation.Value, UriKind.RelativeOrAbsolute));
                                request.Method = "GET";
                                imports.Add(nameSpace.Value, schemaLocation.Value);
                                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                                StreamReader reader = new StreamReader(response.GetResponseStream());
                                string result = await reader.ReadToEndAsync();
                                reader.Dispose();
                                XDocument doc = XDocument.Parse(result);
                                await ParseSuccessResult(doc);
                            }
                            catch (WebException ex)
                            {
                                var response = ex.Response as HttpWebResponse;
                                StreamReader reader = new StreamReader(response.GetResponseStream());
                                var message = reader.ReadToEnd();
                                throw new WebException(message, ex.InnerException);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message, ex.InnerException);
                            }
                        }
                    }
                }
                else
                {
                    return ParseImportResult(document, targetNamespace.Value);
                }
            }
            return null;
        }

        private Dictionary<string, List<WFSFeatureDescription>> ParseImportResult(XDocument doc, string targetNS)
        {
            List<WFSFeatureDescription> types = new List<WFSFeatureDescription>();
            Dictionary<string, List<WFSFeatureDescription>> typesDic = new Dictionary<string, List<WFSFeatureDescription>>();
            string namesp = doc.Root.Name.NamespaceName;

            foreach (var ele in doc.Root.Elements(XName.Get("element", namesp)))
            {
                string type = string.Empty;
                string typeValue = string.Empty;
                XAttribute eleNameAttribute = ele.Attribute("name");
                XAttribute eleTypeAttribute = ele.Attribute("type");
                if (eleNameAttribute == null || eleTypeAttribute == null)
                {
                    continue;
                }

                if (this.TypeNames == null || (this.TypeNames != null && this.TypeNames.Count <= 0))
                {
                    GetHeaderName(eleNameAttribute.Value, eleTypeAttribute.Value, out type, out typeValue);
                    nameDic.Add(type, typeValue);
                }
                else
                {
                    foreach (var item in this.TypeNames)
                    {
                        if (!string.IsNullOrEmpty(eleNameAttribute.Value) && (eleNameAttribute.Value == GetSimpleName(item)))
                        {
                            nameDic.Add(item, eleTypeAttribute.Value);
                        }
                    }
                }
            }

            IEnumerable<XElement> coplexTypes = doc.Root.Elements(XName.Get("complexType", namesp));
            foreach (var type in coplexTypes)
            {
                WFSFeatureDescription typeObject = new WFSFeatureDescription();
                foreach (var namepair in nameDic)
                {
                    XAttribute typeNameAttribute = type.Attribute(XName.Get("name"));
                    if (typeNameAttribute != null && GetSimpleName(namepair.Value) == typeNameAttribute.Value)
                    {
                        typeObject.TypeName = namepair.Key;
                    }
                }

                if (string.IsNullOrEmpty(typeObject.TypeName))
                {
                    continue;
                }
                foreach (var element in type.Descendants(XName.Get("element", namesp)))
                {
                    XAttribute nameAttribute = element.Attribute(XName.Get("name"));
                    XAttribute typeAttribute = element.Attribute(XName.Get("type"));
                    if (nameAttribute != null)
                    {
                        if (typeAttribute != null && !string.IsNullOrEmpty(typeAttribute.Value) && typeAttribute.Value.Contains("gml:"))
                        {
                            typeObject.SpatialProperty = nameAttribute.Value;
                        }
                        else
                        {
                            typeObject.Properties.Add(nameAttribute.Value);
                        }
                    }
                }

                types.Add(typeObject);
            }

            typesDic.Add(targetNS, types);
            return typesDic;
        }

        private void GetHeaderName(string name, string type, out string typename, out string typeValue)
        {
            string[] strs;
            strs = !string.IsNullOrEmpty(type) ? type.Trim().Split(':') : null;
            if (strs != null && strs.Length > 1)
            {
                typename = strs[0] + ":" + name;
                typeValue = strs[1];
            }
            else
            {
                typename = string.Empty;
                typeValue = string.Empty;
            }
        }
    }
}
