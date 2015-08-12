using System;
using System.Collections.Specialized;
namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_DictionaryChangedEventArgs_Title}</para>
    /// 	<para>${WP_core_DictionaryChangedEventArgs_Description}</para>
    /// </summary>
    public sealed class DictionaryChangedEventArgs : EventArgs
    {
        internal DictionaryChangedEventArgs(NotifyCollectionChangedAction action, string key, object oldValue, object newValue)
        {
            this.Action = action;
            this.Key = key;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>${WP_core_DictionaryChangedEventArgs_attribute_Action_D}</summary>
        public NotifyCollectionChangedAction Action { get; private set; }
        /// <summary>${WP_core_DictionaryChangedEventArgs_attribute_Key_D}</summary>
        public string Key { get; private set; }
        /// <summary>${WP_core_DictionaryChangedEventArgs_attribute_NewValue_D}</summary>
        public object NewValue { get; private set; }
        /// <summary>${WP_core_DictionaryChangedEventArgs_attribute_OldValue_D}</summary>
        public object OldValue { get; private set; }
    }
}
