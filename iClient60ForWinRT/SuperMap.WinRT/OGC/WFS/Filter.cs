
using System.Xml.Linq;
namespace SuperMap.WinRT.OGC
{
    /// <summary> ${mapping_Filter_Tile} </summary>
    public abstract class Filter
    {
        /// <summary> ${mapping_Filter_constructor_D} </summary>
        protected Filter()
        { }

        internal abstract XElement ToXML();
    }
}
