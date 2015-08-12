using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.iServerJava6R.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${iServerJava6R_NAResultMapImage_Title}</para>
    /// 	<para>${iServerJava6R_NAResultMapImage_Description}</para>
    /// </summary>
    public class NAResultMapImage
    {
        /// <summary>${iServerJava6R_NAResultMapImage_constructor_D}</summary>
        internal NAResultMapImage()
        { }
        /// <summary>${iServerJava6R_NAResultMapImage_attribute_imageData_D}</summary>
        public List<byte> ImageData { get; private set; }
        /// <summary>${iServerJava6R_NAResultMapImage_attribute_imageURL_D}</summary>
        public string ImageURL { get; private set; }
        /// <summary>${iServerJava6R_NAResultMapImage_attribute_mapParameter_D}</summary>
        public NAResultMapParameter MapParameter { get; private set; }

        /// <summary>${iServerJava6R_NAResultMapImage_method_fromJson_D}</summary>
        /// <returns>${iServerJava6R_NAResultMapImage_method_fromJson_return}</returns>
        /// <param name="json">${iServerJava6R_NAResultMapImage_method_fromJson_param_jsonObject}</param>
        public static NAResultMapImage FromJson(JsonObject json)
        {
            if (json != null)
            {
                NAResultMapImage result = new NAResultMapImage();
                result.ImageURL = json["imageURL"].GetStringEx();
                result.MapParameter = NAResultMapParameter.FromJson(json["mapParameter"].GetObjectEx());
                if (json["imageData"].ValueType !=JsonValueType.Null)
                {
                    result.ImageData = new List<byte>();
                    for (int i = 0; i < json["imageData"].GetArray().Count; i++)
                    {
                        result.ImageData.Add((byte)json["imageData"].GetArray()[i].GetNumberEx());
                    }
                }
                return result;
            }

            return null;
        }
    }
}
