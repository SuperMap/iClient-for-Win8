using System;
using System.Collections.ObjectModel;

using System.Collections.Generic;
namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_FeatureCollection_title}</para>
    /// 	<para>${core_FeatureCollection_Description}</para>
    /// </summary>
    public class FeatureCollection : ObservableCollection<Feature>
    {
        internal event EventHandler CollectionClearing;
        /// <summary>${core_FeatureCollection_method_ClearItems_D}</summary>
        protected override void ClearItems()
        {
            if (this.CollectionClearing != null)
            {
                this.CollectionClearing(this, EventArgs.Empty);
            }
            base.ClearItems();
        }
        /// <summary>${core_FeatureCollection_constructor_None_D}</summary>
        public FeatureCollection()
        {
        }
        /// <summary>${core_FeatureCollection_constructor_D}</summary>
        /// <param name="collection">${core_FeatureCollection_constructor_param_collection_D}</param>
        public FeatureCollection(IEnumerable<Feature> collection)
            : base(collection)
        { 
        }
    }
}

