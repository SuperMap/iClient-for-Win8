using System;
using System.ComponentModel;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_CRS_Title}</para>
    /// 	<para>${core_CRS_Description}</para>
    /// </summary>
    public class CoordinateReferenceSystem : IEquatable<CoordinateReferenceSystem>
    {
        /// <overloads>${core_CRS_constructor_overloads}</overloads>
        /// <summary>${core_CRS_constructor_None_D}</summary>
        public CoordinateReferenceSystem()
            : this(0)
        {
        }

        /// <summary>${core_CRS_constructor_Int_D}</summary>
        /// <param name="wkid">${core_CRS_constructor_Int_param_WKID}</param>
        public CoordinateReferenceSystem(int wkid)
            : this(wkid, Unit.Undefined)
        {
        }
        /// <summary>${core_CRS_constructor_Int_Unit_D}</summary>
        /// <param name="wkid">${core_CRS_constructor_Int_param_WKID}</param>
        /// <param name="unit">${core_CRS_constructor_Int_Unit_param_unit}</param>
        public CoordinateReferenceSystem(int wkid, Unit unit)
            : this(wkid, unit, 6378137)
        {

        }
        /// <summary>
        /// ${core_CRS_constructor_Int_Unit_double_D}
        /// </summary>
        /// <param name="wkid">${core_CRS_constructor_Int_param_WKID}</param>
        /// <param name="unit">${core_CRS_constructor_Int_Unit_param_unit}</param>
        /// <param name="datumAxis">${core_CRS_constructor_Int_Unit_param_datumAxis}</param>
        public CoordinateReferenceSystem(int wkid, Unit unit, double datumAxis)
        {
            WKID = wkid;
            Unit = unit;
            DatumAxis = datumAxis;
        }

        /// <summary>${core_CRS_method_Clone_D}</summary>
        /// <returns>${core_CRS_method_Clone_return}</returns>
        /// <remarks>${core_CRS_method_Clone_remarks}</remarks>
        public CoordinateReferenceSystem Clone()
        {
            CoordinateReferenceSystem reference = base.MemberwiseClone() as CoordinateReferenceSystem;
            reference.WKID = WKID;
            reference.Unit = Unit;
            reference.DatumAxis = DatumAxis;
            return reference;
        }

        /// <summary>${core_CRS_method_equals_CRS_D}</summary>
        /// <overloads>${core_CRS_method_equals_overloads}</overloads>
        /// <returns>${core_CRS_method_equals_CRS_return}</returns>
        /// <param name="crs1">${core_CRS_method_equals_CRS_param_crs}</param>
        /// <param name="crs2">${core_CRS_method_equals_CRS_param_crs}</param>
        /// <param name="ignoreNull">${core_CRS_method_equals_CRS_param_ignoreNull}</param>
        public static bool Equals(CoordinateReferenceSystem crs1, CoordinateReferenceSystem crs2, bool ignoreNull)
        {
            if (crs1 == null || crs2 == null)
            {
                return ignoreNull;
            }
            if (IsWebMercatorWKID(crs1.WKID) && IsWebMercatorWKID(crs2.WKID))
            {
                return true;
            }
            return (crs1.WKID == crs2.WKID);
        }


        internal static bool IsWebMercatorWKID(int wkid)
        {
            return (wkid == 3857) || (wkid == 900913) || (wkid == 102113) || (wkid == 102100);
        }

        /// <summary>${core_CRS_method_equals_D}</summary>
        /// <returns>${core_CRS_method_equals_return}</returns>
        /// <param name="other">${core_CRS_method_equals_param_other}</param>
        public bool Equals(CoordinateReferenceSystem other)
        {
            return Equals(this, other, false);
        }//忽略单位

        /// <summary>${core_CRS_attribute_WKID_D}</summary>
        public int WKID { get; set; }
        /// <summary>${core_CRS_attribute_uint_D}</summary>
        public Unit Unit { get; set; }

        public double DatumAxis { get; set; }
    }
}
