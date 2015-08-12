using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SuperMap.WinRT.Core
{
    internal sealed class ObservableDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IDictionary, ICollection, IEnumerable, INotifyPropertyChanged
    {
        private Dictionary<string, object> storage;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableDictionary()
        {
            this.storage = new Dictionary<string, object>();
        }

        #region IDictionary<string,object> Members

        public void Add(string key, object value)
        {
            this.storage.Add(key, value);

            this.OnValueChanged(NotifyCollectionChangedAction.Add, key, null, value);
        }

        public bool ContainsKey(string key)
        {
            return this.storage.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.storage.Keys; }
        }

        public bool Remove(string key)
        {
            if (!this.storage.ContainsKey(key))
            {
                return false;
            }
            object oldValue = this.storage[key];
            bool flag = this.storage.Remove(key);
            if (flag)
            {
                this.OnValueChanged(NotifyCollectionChangedAction.Remove, key, oldValue, null);
            }
            return flag;
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.storage.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return this.storage.Values; }
        }

        public object this[string key]
        {
            get
            {
                if (this.storage.ContainsKey(key))
                {
                    return this.storage[key];
                }
                return null;
            }
            set
            {
                if (!this.storage.ContainsKey(key))
                {
                    this.Add(key, value);
                }
                else
                {
                    object oldValue = this.storage[key];
                    this.storage[key] = value;
                    this.OnValueChanged(NotifyCollectionChangedAction.Replace, key, oldValue, value);
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string,object>> Members

        public void Add(KeyValuePair<string, object> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.OnValueChanged(NotifyCollectionChangedAction.Reset, null, null, null);
            this.storage.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return (this.storage.ContainsKey(item.Key) && this.storage[item.Key] == item.Value);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.storage.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            if (!this.Contains(item))
            {
                return false;
            }
            return this.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> Members

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.storage.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.storage.GetEnumerator();
        }

        #endregion

        #region IDictionary Members

        public void Add(object key, object value)
        {
            this.storage.Add(key.ToString(), value);
        }

        public bool Contains(object key)
        {
            return this.storage.ContainsKey(key as string);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.storage.GetEnumerator();
        }

        public bool IsFixedSize
        {
            get { return ((IDictionary)this.storage).IsFixedSize; }
        }

        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)this.storage).Keys; }
        }

        public void Remove(object key)
        {
            if (!(key is string))
            {
                throw new ArgumentException("key is string.");
            }
            this.Remove(key as string);
        }

        ICollection IDictionary.Values
        {
            get { return ((IDictionary)this.storage).Values; }
        }


        public object this[object key]
        {
            get
            {
                return ((IDictionary)this.storage)[key];
            }
            set
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key is string.");
                }
                this.storage[key as string] = this.storage;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(System.Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)this.storage).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)this.storage).SyncRoot; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region ValueChanged
        public event EventHandler<DictionaryChangedEventArgs> ValueChanged;
        private void OnValueChanged(NotifyCollectionChangedAction action, string key, object oldValue, object newValue)
        {
            EventHandler<DictionaryChangedEventArgs> valueChanged = this.ValueChanged;
            if (valueChanged != null)
            {
                valueChanged(this, new DictionaryChangedEventArgs(action, key, oldValue, newValue));
            }
            this.RaisePropertyChanged("Item[]");
            this.RaiseCollectionChanged(action);
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        #endregion

    }
}
