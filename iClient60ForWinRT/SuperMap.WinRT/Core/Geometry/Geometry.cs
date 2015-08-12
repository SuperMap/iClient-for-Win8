using System;
using System.Runtime.Serialization;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// ${core_Geometry_Title}<br/>
    /// ${core_Geometry_Description}
    /// </summary>
    /// <remarks>${core_Geometry_Description_Remarks}</remarks>
    [DataContract, KnownType(typeof(GeoPoint)), KnownType(typeof(GeoRegion)), KnownType(typeof(GeoCircle)), KnownType(typeof(GeoLine))]
    public abstract class Geometry
    {
        internal event EventHandler GeometryChanged;

        internal Geometry()
        {
        }
        /// <summary>${core_Geometry_method_raiseGeometryChanged_D}</summary>
        protected internal void RaiseGeometryChanged()
        {
            if (this.GeometryChanged != null)
            {
                this.GeometryChanged(this, new EventArgs());
            }
        }
        /// <summary>${core_Geometry_method_clone_D}</summary>
        public abstract Geometry Clone();
        /// <summary>${core_Geometry_method_Offset_D}</summary>
        /// <param name="deltaX">${core_Geometry_method_Offset_param_deltaX}</param>
        /// <param name="deltaY">${core_Geometry_method_Offset_param_deltaX}</param>
        public abstract void Offset(double deltaX, double deltaY);
        /// <summary>${core_Geometry_attribute_bounds_D}</summary>
        public abstract Rectangle2D Bounds { get; }
    }
}
