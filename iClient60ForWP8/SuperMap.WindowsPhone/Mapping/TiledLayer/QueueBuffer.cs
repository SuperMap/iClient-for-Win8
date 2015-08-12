using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.Mapping
{

    /// <summary>
    /// Tiles的缓冲区，用来存放待请求的Tiles。结构类似于二维数组，
    /// 第一维是一个Dictionary，Key为图层id；第二维是一个双向链表，存放这个图层中的Tile。
    /// 在弹出Tile时，取第一个长度大于0的链表的最后一个。
    /// </summary>
    internal class QueueBuffer
    {
        private Dictionary<string, LinkedList<Tile>> _items;
        private object _thisLock = new object();

        public QueueBuffer()
        {
            _items = new Dictionary<string, LinkedList<Tile>>();
        }
        /// <summary>
        /// 根据ID插入一批新的索引。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="items"></param>
        public void InsertItemsById(string id, IEnumerable<Tile> items)
        {
            lock (_thisLock)
            {
                if (!_items.ContainsKey(id))
                {
                    return;
                }
                if (items != null && items.Count() > 0)
                {
                    foreach (Tile tile in items)
                    {
                        _items[id].AddLast(tile);
                    }
                }
            }
        }

        /// <summary>
        /// 根据ID清除对应的索引。
        /// </summary>
        /// <param name="id"></param>
        public void Clear(string id)
        {
            lock (_thisLock)
            {
                if (!_items.ContainsKey(id))
                {
                    return;
                }
                else
                {
                    _items[id].Clear();
                }
            }
        }

        /// <summary>
        /// 向缓冲区中注册一个新的成员。
        /// </summary>
        /// <param name="id"></param>
        public void Register(string id)
        {
            lock (_thisLock)
            {
                if (!_items.ContainsKey(id))
                {
                    _items.Add(id, new LinkedList<Tile>());
                }
            }
        }

        /// <summary>
        /// 从缓冲区中注销一个已有的成员。
        /// </summary>
        /// <param name="id"></param>
        public void Unregiest(string id)
        {
            lock (_thisLock)
            {
                if (_items.ContainsKey(id))
                {
                    _items[id].Clear();
                    _items.Remove(id);
                }
            }
        }

        /// <summary>
        /// 确定Tile是否在链表中。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool Contains(string id, Tile tile)
        {
            lock (_thisLock)
            {
                if (!_items.ContainsKey(id))
                {
                    return false;
                }
                else
                {
                    return _items[id].Contains(tile);
                }
            }
        }

        /// <summary>
        /// 弹出一个Tile，同时将其从链表中移除掉。
        /// 查找方式是：第一个长度大于0的链表的第一个值。
        /// </summary>
        /// <returns></returns>
        public Tile Pop()
        {
            Tile tile = null;
            lock (_thisLock)
            {
                foreach (LinkedList<Tile> list in _items.Values)
                {
                    if (list.Count > 0)
                    {
                        tile = list.First.Value;
                        list.RemoveFirst();
                        break;
                    }
                }
            }
            return tile;
        }

        /// <summary>
        /// 向链表最后面插入一个Tile。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tile"></param>
        public void AddLast(string id, Tile tile)
        {
            lock (_thisLock)
            {
                if (_items.ContainsKey(id))
                {
                    _items[id].AddLast(tile);
                }
            }
        }

        /// <summary>
        /// 缓冲区中的Tiles数量。
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                lock (_thisLock)
                {
                    foreach (LinkedList<Tile> list in _items.Values)
                    {
                        count += list.Count;
                    }
                }
                return count;
            }
        }
    }

}
