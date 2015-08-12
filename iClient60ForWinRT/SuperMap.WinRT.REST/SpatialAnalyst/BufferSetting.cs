
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_BufferSetting_Title}</para>
    /// 	<para>${REST_BufferSetting_Description}</para>
    /// </summary>
    public class BufferSetting
    {
        /// <summary>${REST_BufferSetting_constructor_D}</summary>
        public BufferSetting()
        { }
        /// <summary>${REST_BufferSetting_attribute_endType_D}</summary>
        public BufferEndType EndType
        {
            get;
            set;
        }
        /// <summary>${REST_BufferSetting_attribute_leftDistance_D}</summary>
        public BufferDistance LeftDistance
        {
            get;
            set;
        }
        /// <summary>${REST_BufferSetting_attribute_rightDistance_D}</summary>
        public BufferDistance RightDistance
        {
            get;
            set;
        }
        /// <summary>${REST_BufferSetting_attribute_semicircleLineSegment_D}</summary>
        public int SemicircleLineSegment
        {
            get;
            set;
        }

        internal static string ToJson(BufferSetting bufferSetting)
        {
            System.Text.StringBuilder json = new System.Text.StringBuilder("{");
            //System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            json.AppendFormat("\"endType\":\"{0}\",", bufferSetting.EndType);

            if (bufferSetting.LeftDistance != null)
            {
                json.AppendFormat("\"leftDistance\":{0},", BufferDistance.ToJson(bufferSetting.LeftDistance));
            }
            else
            {
                //json.Append("\"leftDistance\":null,");
                json.AppendFormat("\"leftDistance\":{0},", BufferDistance.ToJson(new BufferDistance()));
            }

            if (bufferSetting.RightDistance != null)
            {
                json.AppendFormat("\"rightDistance\":{0},", BufferDistance.ToJson(bufferSetting.RightDistance));
            }
            else
            {
                //json.Append("\"rightDistance\":null,");
                json.AppendFormat("\"rightDistance\":{0},", BufferDistance.ToJson(new BufferDistance()));
            }

            json.AppendFormat("\"semicircleLineSegment\":{0}", bufferSetting.SemicircleLineSegment);
            json.Append("}");
            return json.ToString();
        }

    }
}
