
namespace SuperMap.WinRT.Core
{
    /// <summary>${core_Unit_Title}</summary>
    public enum Unit
    {
        /// <summary>${core_Unit_Meter}</summary>
        Meter = 0x2710,    //10,000 //默认是米   0.1毫米
        /// <summary>${core_Unit_Centimeter}</summary>
        Centimeter = 100,  //100
        /// <summary>${core_Unit_Decimeter}</summary>
        Decimeter = 0x3e8, //1000
        /// <summary>${core_Unit_Foot}</summary>
        Foot = 0xbe8,   //1000
        /// <summary>${core_Unit_Inch}</summary>
        Inch = 0xfe,  //254
        /// <summary>${core_Unit_Kilometer}</summary>
        Kilometer = 0x989680,  //1000,000
        /// <summary>${core_Unit_Mile}</summary>
        Mile = 0xf59100,   //16090000
        /// <summary>${core_Unit_Millimeter}</summary>
        Millimeter = 10,   //10
        /// <summary>${core_Unit_Yard}</summary>
        Yard = 0x23b8,  //9144
        /// <summary>${core_Unit_Undefined}</summary>
        Undefined = -1,
        //NauticalMile = 0x11a97c0, //200海里
        /// <summary>${core_Unit_DecimalDegree}</summary>
        DecimalDegree = 0,
        /// <summary>${core_Unit_Degree}</summary>
        Degree = 1001745329,
        /// <summary>${core_Unit_Minute}</summary>
        Minute = 1000029089,
        /// <summary>${core_Unit_Second}</summary>
        Second = 1000000485,
        /// <summary>${core_Unit_Radian}</summary>
        Radian = 1100000000
    }
}
