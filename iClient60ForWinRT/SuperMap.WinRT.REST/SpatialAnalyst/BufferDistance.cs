using System.Text;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_BufferDistance_Title}</para>
    /// 	<para>${REST_BufferDistance_Description}</para>
    /// </summary>
    public class BufferDistance
    {
        /// <summary>${REST_BufferDistance_constructor_D}</summary>
        public BufferDistance()
        {
            Value = 100;
        }
        /// <summary>${REST_BufferDistance_attribute_exp_D}</summary>
        public string Expression
        {
            get;
            set;
        }
        /// <summary>${REST_BufferDistance_attribute_value_D}</summary>
        public double Value
        {
            get;
            set;
        }

        internal static string ToJson(BufferDistance bufferDistance)
        {
            System.Text.StringBuilder json = new StringBuilder("{");
            //System.Collections.Generic.List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(bufferDistance.Expression) && !string.IsNullOrWhiteSpace(bufferDistance.Expression))
            {
                json.AppendFormat("\"exp\":\"{0}\",", bufferDistance.Expression);
            }
            else
            {
                json.Append("\"exp\":null,");
            }

            json.AppendFormat("\"value\":{0},", bufferDistance.Value);

            json.Append("}");

            return json.ToString();
        }
    }
}
