using System;
using System.Xml.Linq;
using SuperMap.Web.Core;
using System.Collections.Generic;
using SuperMap.Web.Utilities;
using System.Windows.Browser;

namespace SuperMap.Web.OGC
{
    /// <summary>
    ///     <para>${mapping_WFSTransaction_Tile}</para>
    ///     <para>${mapping_WFSTransaction_Description}</para>
    /// </summary>
    public class WFSTransaction : WFSServiceBase
    {
        /// <summary>${mapping_WFSTransaction_constructor_None_D}</summary>
        /// <overloads>${mapping_WFSTransaction_constructor_overloads}</overloads>
        public WFSTransaction()
        { }

        /// <summary>${mapping_WFSTransaction_constructor_String_D}</summary>
        /// <param name="url">${mapping_WFSTransaction_constructor_String_param_url}</param>
        public WFSTransaction(string url)
        {
            this.Url = url;
        }

        internal override string GetFinalUrl()
        {
            if (string.IsNullOrEmpty(Url))
            {
                return string.Empty;
            }

            if (!Url.Contains("http://"))  //相对地址
            {
                var pageUrl = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var localUrl = pageUrl.AbsoluteUri.Substring(0, pageUrl.AbsoluteUri.IndexOf(pageUrl.AbsolutePath));
                this.Url = localUrl + Url;
            }

            string midurl = "?request=Transaction&version=" + this.Version;

            return this.Url + midurl;
        }

        internal override void ParseSuccessResult(XDocument document, string originResult, object userState)
        {
            //更新和删除没有InsertResult节点，只有TransactionResult节点。添加的话这两个节点都有            
            string wfs = "http://www.opengis.net/wfs";
            WFSTResult result = new WFSTResult();
            XElement root = document.Element(XName.Get("WFS_TransactionResponse", wfs));
            if (root != null)
            {
                result = WFSTResult.FromXML(root);
            }

            OnProcessComplated(result, originResult, userState);
        }

        //public string ReleaseAction { get { return "ALL"; } } //不支持啊

        private void Process(object parameters, object state)
        {
            string postbody = string.Empty;
            XDocument doc = ReturnTransactionRoot();

            doc.Root.SetAttributeValue(XName.Get("version"), this.Version);
            doc.Root.SetAttributeValue(XName.Get("service"), "WFS");

            List<WFSTParameters> paramList = parameters as List<WFSTParameters>;
            if ((paramList != null && paramList.Count > 0))
            {
                foreach (var item in paramList)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    if (item is WFSTInsertParams)
                    {
                        doc.Root.Add((item as WFSTInsertParams).ToXML(this.FeatureNS));
                    }
                    else if (item is WFSTDeleteParams)
                    {
                        doc.Root.Add((item as WFSTDeleteParams).ToXML(this.FeatureNS));
                    }
                    else if (item is WFSTUpdateParams)
                    {
                        doc.Root.Add((item as WFSTUpdateParams).ToXML(this.FeatureNS));
                    }
                }

                postbody = doc.ToString();
            }

            base.ProcessAsync(postbody, state);
        }

        internal XDocument ReturnTransactionRoot()
        {
            string xmlns = "http://www.w3.org/2000/xmlns/";
            string wfs = "http://www.opengis.net/wfs";
            string ogc = "http://www.opengis.net/ogc";
            string xsi = "http://www.w3.org/2001/XMLSchema-instance";

            if (string.IsNullOrEmpty(this.FeatureNS))
            {
                throw new ArgumentNullException("缺少参数");
            }
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

            XElement root = new XElement("{" + wfs + "}Transaction",
                new XAttribute("{" + xmlns + "}wfs", wfs),
                new XAttribute("{" + xmlns + "}ogc", ogc),
                new XAttribute("{" + xmlns + "}xsi", xsi));

            root.SetAttributeValue(XName.Get("{" + xsi + "}schemaLocation"), this.SchemaLocation);

            //添加默认的命名空间
            root.SetAttributeValue(XName.Get("xmlns"), this.FeatureNS);

            doc.AddFirst(root);
            return doc;
        }
        /// <summary>${mapping_WFSTransaction_method_processAsync_D}</summary>
        /// <overloads>${mapping_WFSTransaction_method_processAsync_overloads}</overloads>
        public void ProcessAsync(List<WFSTParameters> parameters)
        {
            ProcessAsync(parameters, null);
        }
        /// <summary>${mapping_WFSTransaction_method_processAsync_D}</summary>
        /// <param name="parameters">${mapping_WFSTransaction_method_processAsync_param_parameters}</param>
        /// <param name="state">${mapping_WFSTransaction_method_processAsync_param_state}</param>
        public void ProcessAsync(List<WFSTParameters> parameters, object state)
        {
            this.Process(parameters, state);
        }
        /// <summary>${mapping_WFSTransaction_event_processCompleted_D}</summary>
        [ScriptableMember]
        public event EventHandler<ResultEventArgs<WFSTResult>> ProcessCompleted;

        private void OnProcessComplated(WFSTResult result, string originRes, object userState)
        {
            if (ProcessCompleted != null)
            {
                ProcessCompleted(this, new ResultEventArgs<WFSTResult>(result, originRes, userState));
            }
        }
        /// <summary>${mapping_WFSTransaction_attribute_featureNS_D}</summary>
        public string FeatureNS { get; set; }
        /// <summary>${mapping_WFSTransaction_attribute_schemaLocation_D}</summary>
        public string SchemaLocation { get; set; }
    }
    /// <summary>
    /// 	<para>${mapping_WFSTParameters_Title}</para>
    /// </summary>
    abstract public class WFSTParameters
    {
        /// <summary>${mapping_WFSTParameters_attribute_typeName_D}</summary>
        public string TypeName { get; set; }

        internal abstract XElement ToXML(string featureNS);
    }
    /// <summary>
    /// 	<para>${mapping_WFSTDeleteParams_Title}</para>
    /// 	<para>${mapping_WFSTDeleteParams_Description}</para>
    /// </summary>
    public sealed class WFSTDeleteParams : WFSTParameters
    {
        /// <summary>${mapping_WFSTDeleteParams_constructor_D}</summary>
        public WFSTDeleteParams() { }
        /// <summary>${mapping_WFSTDeleteParams_attribute_featureIDs_D}</summary>
        public string[] FeatureIDs { get; set; }
        /// <summary>${mapping_WFSTDeleteParams_attribute_filter_D}</summary>
        public Filter Filter { get; set; }

        internal override XElement ToXML(string featureNS)
        {
            string wfs = "http://www.opengis.net/wfs";
            string ogc = "http://www.opengis.net/ogc";
            string localTypeName = string.Empty;

            XElement delete = new XElement("{" + wfs + "}Delete");

            #region 必设属性的判断
            if (string.IsNullOrEmpty(this.TypeName))
            {
                return delete;
            }
            #endregion
            //获取TypeName的本地名称。
            string[] splitNames = this.TypeName.Split(':');
            if (splitNames != null)
            {
                int totalCount = splitNames.Length;
                localTypeName = splitNames[totalCount - 1];
            }
            if (string.IsNullOrEmpty(localTypeName))
            {
                return delete;
            }

            delete.SetAttributeValue(XName.Get("typeName"), localTypeName);
            XElement filter = new XElement(XName.Get("{" + ogc + "}Filter"));

            if (FeatureIDs != null && FeatureIDs.Length > 0)
            {
                for (int i = 0; i < FeatureIDs.Length; i++)
                {
                    XElement featureID = new XElement(XName.Get("{" + ogc + "}FeatureId"), new XAttribute(XName.Get("fid"), FeatureIDs[i]));
                    filter.Add(featureID);
                }
            }
            else if (this.Filter != null)
            {
                XElement subFilterValue = this.Filter.ToXML();

                if (subFilterValue != null)
                {
                    filter.Add(subFilterValue);
                }
            }
            else
            {
                //啥也不干
            }
            delete.AddFirst(filter);
            return delete;
        }
    }
    /// <summary>
    /// 	<para>${mapping_WFSTUpdateParams_Title}</para>
    /// 	<para>${mapping_WFSTUpdateParams_Description}</para>
    /// </summary>
    public sealed class WFSTUpdateParams : WFSTParameters
    {
        /// <summary>${mapping_WFSTUpdateParams_constructor_D}</summary>
        public WFSTUpdateParams() { }
        /// <summary>${mapping_WFSTUpdateParams_attribute_filter_D}</summary>
        public Filter Filter { get; set; }
        /// <summary>${mapping_WFSTUpdateParams_attribute_feature_D}</summary>
        public Feature Feature { get; set; }
        /// <summary>${mapping_WFSTUpdateParams_attribute_spatialProperty_D}</summary>
        public string SpatialProperty { get; set; }
        /// <summary>${mapping_WFSTUpdateParams_attribute_featureIDs_D}</summary>
        public string[] FeatureIDs { get; set; }

        internal override XElement ToXML(string featureNS)
        {
            string wfs = "http://www.opengis.net/wfs";
            string ogc = "http://www.opengis.net/ogc";
            string localTypeName = string.Empty;
            XElement update = new XElement("{" + wfs + "}Update");

            #region 必设属性的判断
            if (this.Feature == null || string.IsNullOrEmpty(this.TypeName) || (this.Feature.Attributes.Count == 0 && this.Feature.Geometry == null))
            {
                return update;
            }
            #endregion

            //获取TypeName的本地名称。
            string[] splitNames = this.TypeName.Split(':');
            if (splitNames != null)
            {
                int totalCount = splitNames.Length;
                localTypeName = splitNames[totalCount - 1];
            }
            if (string.IsNullOrEmpty(localTypeName))
            {
                return update;
            }

            update.SetAttributeValue(XName.Get("typeName"), localTypeName);
            //Feature信息
            //存在Feature.Geometry的话就加空间属性
            if (this.Feature.Geometry != null && !string.IsNullOrEmpty(this.SpatialProperty))
            {
                update.Add(CreatePropertyXML(this.SpatialProperty, XMLHelper.FromGeometry(this.Feature.Geometry)));
            }

            //Feature.Attributes.Count>0的话就加普通属性
            if (this.Feature.Attributes.Count > 0)
            {
                foreach (var keyValue in this.Feature.Attributes)
                {
                    update.Add(CreatePropertyXML(keyValue.Key, keyValue.Value));
                }
            }

            //FeatureIDs和Filter部分
            XElement filter = new XElement(XName.Get("{" + ogc + "}Filter"));
            if (FeatureIDs != null && FeatureIDs.Length > 0)
            {
                for (int i = 0; i < FeatureIDs.Length; i++)
                {
                    XElement featureID = new XElement(XName.Get("{" + ogc + "}FeatureId"), new XAttribute(XName.Get("fid"), FeatureIDs[i]));
                    filter.Add(featureID);
                }
            }
            else if (this.Filter != null)
            {
                XElement subFilterValue = this.Filter.ToXML();

                if (subFilterValue != null)
                {
                    filter.Add(subFilterValue);
                }
            }
            else
            { }

            update.Add(filter);
            return update;
        }

        private XElement CreatePropertyXML(string name, object value)
        {
            string wfs = "http://www.opengis.net/wfs";
            XElement property = new XElement(XName.Get("Property", wfs), new XElement(XName.Get("Name", wfs), name));
            if (value is XElement)
            {
                property.Add(new XElement(XName.Get("Value", wfs), value as XElement));
            }
            else
            {
                property.Add(new XElement(XName.Get("Value", wfs), value));
            }

            return property;
        }
    }
    /// <summary>
    /// 	<para>${mapping_WFSTInsertParams_Title}</para>
    /// 	<para>${mapping_WFSTInsertParams_Description}</para>
    /// </summary>
    public sealed class WFSTInsertParams : WFSTParameters
    {
        /// <summary>${mapping_WFSTInsertParams_constructor_D}</summary>
        public WFSTInsertParams()
        {
            Features = new FeatureCollection();
        }
        /// <summary>${mapping_WFSTInsertParams_attribute_spatialProperty_D}</summary>
        public string SpatialProperty { get; set; }
        /// <summary>${mapping_WFSTInsertParams_attribute_features_D}</summary>
        public FeatureCollection Features { get; set; }

        internal override XElement ToXML(string featureNS)
        {
            string wfs = "http://www.opengis.net/wfs";
            string localTypeName = string.Empty;
            XElement insert = new XElement("{" + wfs + "}Insert");

            #region 必设属性的判断
            if (string.IsNullOrEmpty(this.SpatialProperty) || string.IsNullOrEmpty(this.TypeName))
            {
                return insert;
            }
            #endregion

            //获取TypeName的本地名称。
            string[] splitNames = this.TypeName.Split(':');
            if (splitNames != null)
            {
                int totalCount = splitNames.Length;
                localTypeName = splitNames[totalCount - 1];
            }
            if (string.IsNullOrEmpty(localTypeName))
            {
                return insert;
            }

            if (this.Features != null)
            {
                foreach (Feature feature in this.Features)
                {
                    XElement insertBody = new XElement(XName.Get(localTypeName, featureNS));
                    if (feature.Geometry != null)
                    {
                        XElement spatialBody = new XElement(XName.Get(this.SpatialProperty, featureNS));
                        spatialBody.Add(XMLHelper.FromGeometry(feature.Geometry));
                        insertBody.Add(spatialBody);
                    }
                    //处理属性
                    foreach (var attr in feature.Attributes)
                    {
                        string localPropName = string.Empty;
                        if (attr.Key.Contains(":"))
                        {
                            string[] properties = attr.Key.Split(':');

                            if (!string.IsNullOrEmpty(properties[1]))
                            {
                                localPropName = properties[1];
                            }
                        }
                        else
                        {
                            localPropName = attr.Key;
                        }
                        XElement propertyBody = new XElement(XName.Get(localPropName, featureNS), attr.Value.ToString());
                        insertBody.Add(propertyBody);
                    }

                    insert.Add(insertBody);
                }
            }
            return insert;
        }
    }
    /// <summary>
    /// 	<para>${mapping_WFSTResult_Title}</para>
    /// </summary>
    public class WFSTResult
    {
        /// <summary>${mapping_WFSTResult_constructor_D}</summary>
        public WFSTResult()
        {
            InsertResults = new List<InsertResult>();
        }
        /// <summary>${mapping_WFSTResult_attribute_transactionResult_D}</summary>
        public TransactionResult TransactionResult { get; private set; }
        /// <summary>${mapping_WFSTInsertParams_attribute_insertResults_D}</summary>
        public List<InsertResult> InsertResults { get; private set; }

        internal static WFSTResult FromXML(XElement TResult)
        {
            string wfs = "http://www.opengis.net/wfs";
            WFSTResult result = new WFSTResult();
            XElement transactionResult = TResult.Element(XName.Get("TransactionResult", wfs));

            result.TransactionResult = TransactionResult.FromXML(transactionResult);

            IEnumerable<XElement> insertResults = TResult.Elements(XName.Get("InsertResult", wfs));
            if (insertResults != null)
            {
                foreach (var item in insertResults)
                {
                    if (item != null)
                    {
                        result.InsertResults.Add(InsertResult.FromXML(item));
                    }
                }
            }
            return result;
        }
    }
    /// <summary>
    /// 	<para>${mapping_TransactionResult_Title}</para>
    /// </summary>
    public class TransactionResult
    {
        /// <summary>${mapping_TransactionResult_constructor_D}</summary>
        public TransactionResult() { }
        /// <summary>${mapping_TransactionResult_attribute_status_D}</summary>
        public WFSTStatus Status { get; private set; }
        /// <summary>${mapping_TransactionResult_attribute_locator_D}</summary>
        public string Locator { get; private set; }
        /// <summary>${mapping_TransactionResult_attribute_message_D}</summary>
        public string Message { get; private set; }

        internal static TransactionResult FromXML(XElement TResult)
        {
            string wfs = "http://www.opengis.net/wfs";
            TransactionResult result = new TransactionResult();
            XElement status = TResult.Element(XName.Get("Status", wfs));
            XElement locator = TResult.Element(XName.Get("Locator", wfs));
            XElement message = TResult.Element(XName.Get("Message", wfs));
            string statusStr = ((XElement)status.FirstNode).Name.LocalName;
            switch (statusStr)
            {
                case "SUCCESS":
                    result.Status = WFSTStatus.Success;
                    break;
                case "PARTIAL":
                    result.Status = WFSTStatus.Partial;
                    break;
                case "FAILED":
                    result.Status = WFSTStatus.Failed;
                    break;
            }
            result.Locator = locator != null ? locator.ToString() : string.Empty;
            result.Message = message != null ? message.ToString() : string.Empty;
            return result;
        }

        /// <summary>
        /// 	<para>${mapping_WFSTStatus_Title}</para>
        /// </summary>
        public enum WFSTStatus
        {
            /// <summary>${mapping_WFSTStatus_attribute_success_D}</summary>
            Success,
            /// <summary>${mapping_WFSTStatus_attribute_failed_D}</summary>
            Failed,
            /// <summary>${mapping_WFSTStatus_attribute_partial_D}</summary>
            Partial
        }
    }
    /// <summary>
    /// 	<para>${mapping_InsertResult_Title}</para>
    /// </summary>
    public class InsertResult
    {
        /// <summary>${mapping_InsertResult_constructor_D}</summary>
        public InsertResult()
        {
            FeatureIDs = new List<string>();
        }
        /// <summary>${mapping_InsertResult_attribute_featureIDS_D}</summary>
        public List<string> FeatureIDs { get; private set; }

        internal static InsertResult FromXML(XElement TResult)
        {
            InsertResult result = new InsertResult();
            string ogc = "http://www.opengis.net/ogc";
            IEnumerable<XElement> featureIDS = TResult.Elements(XName.Get("FeatureId", ogc));
            if (featureIDS != null)
            {
                foreach (XElement id in featureIDS)
                {
                    result.FeatureIDs.Add(id.Attribute(XName.Get("fid")).Value);
                }
            }

            return result;
        }
    }
}
