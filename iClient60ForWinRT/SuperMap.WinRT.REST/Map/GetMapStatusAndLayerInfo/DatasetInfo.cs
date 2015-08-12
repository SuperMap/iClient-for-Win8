using SuperMap.WinRT.Utilities;
using System;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_DatasetInfo_Title}</para>
    /// 	<para>${REST_DatasetInfo_Description}</para>
    /// </summary>
    public class DatasetInfo
    {
        internal DatasetInfo()
        { }
        /// <summary>${REST_DatasetInfo_attribute_Name_D}</summary>
        public string Name { get; private set; }
        /// <summary>${REST_DatasetInfo_attribute_Type_D}</summary>
        public DatasetType Type { get; private set; }
        /// <summary>${REST_DatasetInfo_attribute_DatasourceName_D}</summary>
        public string DatasourceName { get; private set; }
        /// <summary>${REST_DatasetInfo_attribute_bounds_D}</summary>
        public Rectangle2D Bounds { get; private set; }
        /// <summary>${REST_DatasetInfo_method_FromJson_D}</summary>
        /// <returns>${REST_DatasetInfo_method_FromJson_return}</returns>
        /// <param name="jsonObject">${REST_DatasetInfo_method_FromJson_param_jsonObject}</param>
        internal static DatasetInfo FromJson(JsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                return null;
            }
            DatasetInfo result = new DatasetInfo();
            result.Name = jsonObject["name"].GetStringEx();

            if (jsonObject["type"].ValueType !=JsonValueType.Null)
            {
                result.Type = (DatasetType)Enum.Parse(typeof(DatasetType), jsonObject["type"].GetStringEx(), true);
            }
            else
            {
            }
            result.DatasourceName = jsonObject["dataSourceName"].GetStringEx();
            if (jsonObject["bounds"].ValueType !=JsonValueType.Null)
            {
                result.Bounds = DatasetInfo.ToRectangle2D(jsonObject["bounds"].GetObjectEx());
            }
            else
            {  }
            return result;
        }

        internal static Rectangle2D ToRectangle2D(JsonObject jsonObject)
        {
            double mbMinX = (jsonObject["leftBottom"].GetObjectEx())["x"].GetNumberEx();
            double mbMinY = (jsonObject["leftBottom"].GetObjectEx())["y"].GetNumberEx();
            double mbMaxX = (jsonObject["rightTop"].GetObjectEx())["x"].GetNumberEx();
            double mbMaxY = (jsonObject["rightTop"].GetObjectEx())["y"].GetNumberEx();
            return new Rectangle2D(mbMinX, mbMinY, mbMaxX, mbMaxY);
        }
    }
}
