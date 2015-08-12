using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// ${WP_mapping_DynamicImageLayer_Title}<br/>
    /// ${WP_mapping_DynamicImageLayer_Description}
    /// </summary>
    public abstract class DynamicLayer : ImageLayer
    {
        private int requestID;
        /// <summary>${WP_mapping_DynamicImageLayer_method_onImageSourceCompleted_D}</summary>
        /// <param name="imgSrc">${WP_mapping_DynamicImageLayer_method_onImageSourceCompleted_param_imgSrc}</param>
        public delegate void OnImageSourceCompleted(ImageSource imgSrc);

        /// <summary>${WP_mapping_DynamicImageLayer_constructor_None_D}</summary>
        protected DynamicLayer()
        {
            this.requestID = 0;
            base.progressWeight = 1.0;
        }

        /// <summary>${WP_mapping_DynamicImageLayer_method_refresh_D}</summary>
        public override void Refresh()
        {
            base.OnLayerChanged();
        }

        /// <summary>${WP_mapping_DynamicImageLayer_method_cancel_D}</summary>
        protected override void Cancel()
        {
            base.Cancel();
            this.requestID++;
        }

        /// <summary>${WP_mapping_DynamicImageLayer_method_getImageUrl_D}</summary>
        /// <returns>${WP_mapping_DynamicImageLayer_method_getImageUrl_return}</returns>
        protected abstract string GetImageUrl();

        internal override void Draw(DrawParameter drawParameter)
        {
            base.Draw(drawParameter);
            if ((!ViewBounds.IsEmpty) && (base.IsVisible))
            {
                this.OnProgress(0);
                this.GetImageSource(
                    delegate(ImageSource imgSrc)
                    {
                        this.GetSourceCompleted(imgSrc, drawParameter.UseTransitions, ++this.requestID);
                    }
                );
            }
        }

        /// <summary>${WP_mapping_DynamicImageLayer_method_GetImageSource_D}</summary>
        /// <param name="onCompleted">${WP_mapping_DynamicImageLayer_method_GetImageSource_param_onCompleted}</param>
        protected virtual void GetImageSource(OnImageSourceCompleted onCompleted)
        {
            string imgUrl = this.GetImageUrl();
            if (string.IsNullOrEmpty(imgUrl))
            {
                onCompleted(null);
            }
            else
            {
                BitmapImage imgSrc = null;
                if (imgUrl.Length > 0)
                {
                    imgSrc = new BitmapImage
                      {
                          UriSource = new Uri(imgUrl, UriKind.Absolute),
                      };
                }
                onCompleted(imgSrc);
            }
        }

        private void GetSourceCompleted(ImageSource imgSrc, bool useTransitions, int id)
        {
            if (id == this.requestID)
            {
                if (imgSrc == null || ViewBounds.IsEmpty)
                {
                    base.Container.Children.Clear();
                    this.OnProgress(100);
                }
                else
                {
                    Image img = new Image
                    {
                        IsHitTestVisible = false,
                        Opacity = 1.0,
                        DataContext = useTransitions,
                    };
                    img.SetValue(Image.StretchProperty, Stretch.Fill);//模糊吗?
                    LayerContainer.SetBounds(img, ViewBounds);

                    img.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(this.Img_ImageFailed);
                    bool flag = (imgSrc is BitmapImage) && ((imgSrc as BitmapImage).UriSource != null) && !string.IsNullOrEmpty((imgSrc as BitmapImage).UriSource.OriginalString);
                    if (flag)
                    {
                        EventHandler<DownloadProgressEventArgs> onProgressEventHandler = null;
                        onProgressEventHandler = delegate(object sender, DownloadProgressEventArgs e)
                        {
                            this.bitmap_DownloadProgress(sender, e, img, onProgressEventHandler, id);
                        };
                        (imgSrc as BitmapImage).DownloadProgress += onProgressEventHandler;
                    }
                    img.Source = imgSrc;
                    base.Container.Children.Add(img);
                    if (!flag)
                    {
                        this.ShowImage(img);
                        this.OnProgress(100);
                    }
                }
            }
        }

        private void bitmap_DownloadProgress(object sender, DownloadProgressEventArgs e, Image img, EventHandler<DownloadProgressEventArgs> onProgressEventHandler, int id)
        {
            if (id != this.requestID)
            {
                if (img.Parent != null)
                {
                    (img.Parent as Panel).Children.Remove(img);
                }
            }
            else
            {
                int progress = e.Progress;
                if ((sender as BitmapImage).UriSource.OriginalString != null)
                {
                    if (e.Progress == 100)
                    {
                        (sender as BitmapImage).DownloadProgress -= onProgressEventHandler;
                        this.ShowImage(img);
                    }
                    this.OnProgress(e.Progress);
                }
            }
        }

        private void ShowImage(Image img)
        {
            int num = 0;
            for (int i = base.Container.Children.Count - 1; i >= 0; i--)
            {
                FrameworkElement element = base.Container.Children[num] as FrameworkElement;
                if (img == element)
                {
                    num++;
                }
                else if (element.Parent != null)
                {
                    (element.Parent as Panel).Children.Remove(element);
                }
            }
            img.Opacity = 1.0;
            if ((bool)img.DataContext)
            {
                Storyboard storyboard = new Storyboard();
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = new double?(img.Opacity),
                    To = 1.0,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                animation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity", new object[0]));
                storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, img);
                storyboard.Begin();
            }
            else
            {
                img.Opacity = 1.0;
            }
        }

        private void Img_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.OnProgress(100);
        }
    }
}
