using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.Mapping
{
    public class TiledCachedRESTLayer : TiledCachedLayer
    {
        public TiledCachedRESTLayer()
        {
 
        }

        public override MapImage GetTileUrl(int indexX, int indexY, int level)
        {
            throw new NotImplementedException();
        }
    }
}
