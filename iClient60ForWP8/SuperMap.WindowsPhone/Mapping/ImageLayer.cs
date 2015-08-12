
namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>${WP_mapping_ImageLayer_Title}</summary>
    public abstract class ImageLayer : Layer
    {
        /// <summary>${WP_mapping_ImageLayer_constructor_None_D}</summary>
        protected ImageLayer()
        {
            ImageFormat = "png";
        }

        internal override void Draw(DrawParameter drawParameter)
        {
            base.Draw(drawParameter);
        }//子类重写一下

        /// <summary>${WP_mapping_ImageLayer_attribute_transparent_D}</summary>
        public bool Transparent { get; set; }
        /// <summary>${WP_mapping_ImageLayer_attribute_ImageFormat_D}</summary>
        public string ImageFormat { get; set; }
    }
}
