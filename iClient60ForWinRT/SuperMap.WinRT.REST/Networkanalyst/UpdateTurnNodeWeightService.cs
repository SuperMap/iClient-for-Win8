using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.Networkanalyst
{
    /// <summary>
    /// <para>${iServerJava6R_UpdateTurnNodeWeightService_Title}</para>
    /// </summary>
    public class UpdateTurnNodeWeightService : ServiceBase
    {
        /// <summary>
        /// ${iServerJava6R_UpdateTurnNodeWeightService_constructor_D}
        /// </summary>
        public UpdateTurnNodeWeightService()
        {
        }
        /// <summary>${iServerJava6R_UpdateTurnNodeWeightService_constructor_String_D}</summary>
        /// <param name="url">${iServerJava6R_UpdateTurnNodeWeightService_constructor_param_url}</param>
        public UpdateTurnNodeWeightService(string url)
            : base(url)
        {
        }
        /// <summary>${iServerJava6R_UpdateTurnNodeWeightService_method_ProcessAsync_D}</summary>
        public async Task ProcessAsync(TurnNodeWeightParameters parameters)
        {
            //url=http://192.168.11.11:8090/iserver/services/components-rest/rest/networkanalyst/RoadNet@Changchun;
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            if (base.Url.EndsWith("/"))
            {
                base.Url = base.Url.TrimEnd('/');
            }

            base.Url += "/turnnodeweight/" + parameters.NodeID + "/fromedge/" + parameters.FromEdgeID + "/toedge/" + parameters.ToEdgeID + "/weightfield/" + parameters.WeightField + ".json?_method=PUT&debug=true";
            // base.SubmitRequest(base.Url, null, new EventHandler<RequestEventArgs>(request_Completed), false, true, false);
           await base.SubmitRequest(base.Url, parameters.Weight.ToString(), false, false);
        }
    }
}
