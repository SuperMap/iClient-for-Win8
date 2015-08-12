
using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.Resources;
using System.Collections.ObjectModel;
using SuperMap.WinRT.Service;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
using System.Threading.Tasks;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_MeasureService_Tile}</para>
    /// 	<para>${REST_MeasureService_Description}</para>
    /// </summary>
    public class MeasureService : ServiceBase
    {
        /// <summary>${REST_MeasureService_constructor_None_D}</summary>
        /// <overloads>${REST_MeasureService_constructor_overloads}</overloads>
        public MeasureService() { }

        /// <summary>${REST_MeasureService_constructor_String_D}</summary>
        /// <param name="url">${REST_MeasureService_constructor_String_param_url}</param>
        public MeasureService(string url) : base(url) { }

        /// <summary>${REST_MeasureService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_MeasureService_method_ProcessAsync_param_parameters}</param>
        /// <param name="state">${REST_MeasureService_method_processAsync_param_state}</param>
        public async Task<MeasureResult> ProcessAsync(MeasureParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            if (!this.Url.EndsWith("/"))
            {
                this.Url += '/';
            }

            //将错误抛给服务器，让其返回错误结果，出发我们的Failed事件；
            if (parameters.Geometry is GeoLine)
            {
                this.Url += "distance.json?debug=true&_method=GET&";
            }
            else if (parameters.Geometry is GeoRegion)
            {
                this.Url += "area.json?debug=true&_method=GET&";
            }
            else
            {
                this.Url += "distance.json?debug=true&_method=GET&";
            }
            
            var result = await base.SubmitRequest(base.Url, GetParameters(parameters),  true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return MeasureResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters(MeasureParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Point2DCollection ps = new Point2DCollection();
            ObservableCollection<Point2DCollection> og = new ObservableCollection<Point2DCollection>();
            if (parameters.Geometry is GeoLine)
            {
                og = (parameters.Geometry as GeoLine).Parts;
            }
            else if (parameters.Geometry is GeoRegion)
            {
                og = (parameters.Geometry as GeoRegion).Parts;
            }
            else
            {
                dictionary.Add("point2Ds", "[]");
                dictionary.Add("unit", parameters.Unit.ToString().ToUpper());
                return dictionary;
            }

            foreach (Point2DCollection g in og)
            {
                for (int i = 0; i < g.Count; i++)
                {
                    ps.Add(g[i]);
                }
            }
            dictionary.Add("point2Ds", JsonHelper.FromPoint2DCollection(ps));
            dictionary.Add("unit", parameters.Unit.ToString().ToUpper());
            return dictionary;
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    MeasureResult result = MeasureResult.FromJson(jsonObject);
        //    LastResult = result;
        //    MeasureEventArgs args = new MeasureEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(MeasureEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        //Application.Current.RootVisual.Dispatcher.BeginInvoke(ProcessCompleted, new object[] { this, args });
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_MeasureService_event_ProcessCompleted_D}</summary>
        //public event EventHandler<MeasureEventArgs> ProcessCompleted;

        //private MeasureResult lastResult;
        ///// <summary>${REST_MeasureService_attribute_lastResult_D}</summary>
        //public MeasureResult LastResult
        //{
        //    get
        //    {
        //        return lastResult;
        //    }
        //    private set
        //    {
        //        lastResult = value;
        //        base.OnPropertyChanged("LastResult");
        //    }
        //}
    }
}
