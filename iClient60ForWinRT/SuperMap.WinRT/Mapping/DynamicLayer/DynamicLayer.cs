using SuperMap.WinRT.Core;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// ${mapping_DynamicImageLayer_Title}<br/>
    /// ${mapping_DynamicImageLayer_Description}
    /// </summary>
    public abstract class DynamicLayer : ImageLayer
    {
        private int requestID;

        private CancellationTokenSource _cts;
        SynchronizationContext _sync;
        private object _thisLock;
        DispatcherTimer _timer;

        /// <summary>${mapping_DynamicImageLayer_constructor_None_D}</summary>
        protected DynamicLayer()
        {
            this.requestID = 0;
            base.progressWeight = 1.0;
            _sync = SynchronizationContext.Current;
            _thisLock = new object();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += _timer_Tick;
        }

        internal override void InitContainer()
        {
            Container = new DynamicLayerContainer(this);
        }

        internal override bool CheckBeforeDraw(UpdateParameter updateParameter)
        {
            if ((Map.MapManipulator == MapManipulator.None && Map.MapStatus == MapStatus.Still) ||
                Map.MapStatus == MapStatus.PanCompleted ||
                Map.MapStatus == MapStatus.ZoomCompleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void _timer_Tick(object sender, object e)
        {
            _timer.Stop();
            ImageInfo image = new ImageInfo();
            image.ViewBounds = this.ViewBounds;
            StartDownloadImage(image);
        }

        /// <summary>
        /// ${mapping_DynamicImageLayer_method_GetImage_D}
        /// </summary>
        /// <param name="cancel">${mapping_DynamicImageLayer_method_GetImage_param_cancel_D}</param>
        /// <returns>${mapping_DynamicImageLayer_method_GetImage_returns_D}</returns>
        protected abstract MapImage GetImage(CancellationToken cancel);

        void StartDownloadImage(ImageInfo image)
        {
            CancelRequest();
            _cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await DownloadImage(image, _cts.Token);
            });
        }

        private async Task DownloadImage(ImageInfo image, CancellationToken token)
        {
            try
            {
                MapImage mapImage = GetImage(token);
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                if (mapImage.MapImageType == MapImageType.Data)
                {
                    image.Datas = mapImage.Data;
                }
                else
                {
                    HttpWebRequest request = HttpWebRequest.CreateHttp(mapImage.Url);
                    token.Register(() =>
                        {
                            try
                            {
                                request.Abort();
                            }
                            catch
                            {

                            }
                        });

                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        using (MemoryStream memoryStream = new MemoryStream(0x10000))
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                byte[] temp = new byte[0x1000];
                                int bytes;
                                while ((bytes = responseStream.Read(temp, 0, temp.Length)) > 0)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        token.ThrowIfCancellationRequested();
                                    }
                                    memoryStream.Write(temp, 0, bytes);
                                }
                            }
                            image.Datas = memoryStream.ToArray();
                        }
                    }
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    if (image.Datas != null && image.Datas.Length > 0)
                    {
                        await Completed(image);
                    }
                }
            }
            catch (Exception ex)
            {
                //lock (_thisLock)
                //{
                //    if (_cts != null)
                //    {
                //        _cts.Dispose();
                //        _cts = null;
                //    }
                //}
            }

        }

        async Task Completed(ImageInfo imageInfo)
        {
            Action<object> action = async (obj) =>
                {
                    Container.Children.Clear();
                    if (imageInfo.Datas != null && imageInfo.ViewBounds == ViewBounds)
                    {
                        Image image = await DataToImage(imageInfo);
                        Container.Children.Add(image);
                    }
                    OnProgress(100);
                    //lock (_thisLock)
                    //{
                    //    if (_cts != null)
                    //    {
                    //        _cts.Dispose();
                    //        _cts = null;
                    //    }
                    //}
                };
            SendOrPostCallback callback = new SendOrPostCallback(action);
            _sync.Post(callback, null);
        }

        private async Task<Image> DataToImage(ImageInfo imageInfo)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(imageInfo.Datas);
                    await writer.StoreAsync();

                    bitmapImage.SetSource(randomAccessStream);
                }
            }
            Image image = new Image()
            {
                Opacity = 1.0,
                IsHitTestVisible = false,
                Stretch = Stretch.Fill,
                Source = bitmapImage
            };

            LayerContainer.SetBounds(image, imageInfo.ViewBounds);
            return image;
        }


        /// <summary>${mapping_DynamicImageLayer_method_refresh_D}</summary>
        public override void Refresh()
        {
            base.Container.Children.Clear();
            base.OnLayerChanged();
        }

        private void CancelRequest()
        {
            lock (_thisLock)
            {
                if (_cts != null)
                {
                    if (!_cts.IsCancellationRequested)
                    {
                        _cts.Cancel();
                    }
                    _cts.Dispose();
                    _cts = null;
                }
            }
        }

        /// <summary>${mapping_DynamicImageLayer_method_cancel_D}</summary>
        protected override void Cancel()
        {
            CancelRequest();
            base.Cancel();
            this.requestID++;
        }

        internal override void Draw(UpdateParameter updateParameter)
        {
            base.Draw(updateParameter);
            if ((!Rectangle2D.IsNullOrEmpty(ViewBounds)) && (base.IsVisible))
            {
                this.OnProgress(0);
                //if (_timer.IsEnabled)
                //{
                //    _timer.Stop();
                //}
                //_timer.Start();
                ImageInfo image = new ImageInfo();
                image.ViewBounds = this.ViewBounds;
                StartDownloadImage(image);
            }
        }

        class ImageInfo
        {
            public Rectangle2D ViewBounds { get; set; }

            public byte[] Datas { get; set; }
        }
    }
}
