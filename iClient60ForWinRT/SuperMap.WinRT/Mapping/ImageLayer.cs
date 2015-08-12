
namespace SuperMap.WinRT.Mapping
{
    /// <summary>${mapping_ImageLayer_Title}</summary>
    public abstract class ImageLayer : Layer
    {
        /// <summary>${mapping_ImageLayer_constructor_None_D}</summary>
        protected ImageLayer()
        {
            ImageFormat = "png";
        }

        internal override void Draw(UpdateParameter updateParameter)
        {

        }

        /// <summary>${mapping_ImageLayer_attribute_transparent_D}</summary>
        public bool Transparent { get; set; }
        /// <summary>${mapping_ImageLayer_attribute_ImageFormat_D}</summary>
        public string ImageFormat { get; set; }
    }
}
