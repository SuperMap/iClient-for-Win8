using SuperMap.WinRT.Core;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMap.WinRT.Utilities;
using System.Collections;

namespace SuperMap.WinRT.Mapping
{
    internal class Executant : IExecutant<Tile>
    {
        private Tile _data;

        public Executant()
        {
            Status = ExecuteStatus.Free;
        }

        public ExecuteStatus Status
        {
            get;
            set;
        }

        public Tile Data
        {
            get { return _data; }
        }

        public Task Doing(Tile data)
        {
            Status = ExecuteStatus.Busy;
            _data = data;
            return Task.Run(async () =>
                {
                    Tile result = await HowToDo(data);
                    result.Status = DataStatus.Complated;
                    Status = ExecuteStatus.Free;
                    _data = null;
                    OnExecutionCompleted(result);
                });
        }

        public event ExecutionComplatedHandler<Tile> ExecutionCompleted;

        private void OnExecutionCompleted(Tile result)
        {
            if (ExecutionCompleted != null)
            {
                ExecutionCompleted(this, new ExecutionComplatedEventArgs<Tile>(result));
            }
        }

        public Func<Tile, Task<Tile>> HowToDo
        {
            get;
            set;
        }

    }

    internal class ExecutantQueue : ExecutantQueueBase<Tile>
    {
        public ExecutantQueue(int count, Func<Tile, Task<Tile>> howToDo)
            : base(count, howToDo)
        {
            _executants = new Executant[count];
            for (int i = 0; i < count; i++)
            {
                _executants[i] = new Executant();
                _executants[i].HowToDo = howToDo;
            }
            InitQueue();
        }
    }

    internal class TiledQueueSystem : QueueSystemBase<Tile>
    {
        public TiledQueueSystem(int executantCount, Func<Tile, Task<Tile>> howToDo)
            : base()
        {
            ExecutingQueue = new ExecutantQueue(executantCount, howToDo);
            InitManager();
        }

        public IList<Tile> NeedShowTiles
        {
            get;
            set;
        }

        public override async Task InputData(IList<Tile> datas)
        {
            NeedShowTiles = datas;

            lock (base.Queue)
            {
                List<Tile> needRemove = new List<Tile>();
                foreach (Tile tile in base.Queue)
                {
                    try
                    {
                        if (tile == null)
                        {
                            //不知道为什么会出现null，猜测可能是在Completed中修改状态，与此处冲突，导致取值异常。
                            //依靠状态控制的方式不是线程安全的，尤其是对于里面的值而言。
                            needRemove.Add(tile);
                        }
                        else
                        {
                            if (tile.Status != DataStatus.Free)
                            {
                                needRemove.Add(tile);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

                foreach (Tile item in needRemove)
                {
                    base.Queue.Remove(item);
                }
                foreach (Tile item in datas)
                {
                    if (!base.Queue.Contains(item) && !base.SuccessfulData.Contains(item))
                    {
                        base.Queue.Add(item);
                        item.Status = DataStatus.Waiting;
                    }
                }
                foreach (Tile tile in Queue)
                {
                    if (!DoNext())
                    {
                        break;
                    }
                }
            }
        }

        public void Cancel()
        {
            TileComparer comparer = new TileComparer();
            var list = SuccessfulData.Intersect(NeedShowTiles, comparer);
            InputData(list == null ? null : list.ToList());
        }


        public int GetProcess()
        {
            TileComparer comparer = new TileComparer();
            if (NeedShowTiles == null || NeedShowTiles.Count <= 0)
            {
                return 100;
            }
            int needTiles = SuccessfulData.Intersect(NeedShowTiles, comparer).ToList().Count();

            int progress = needTiles * 100 / NeedShowTiles.Count;
            if (progress == 100)
            {
                if (Queue.Count > 0)
                {
                    progress = 99;
                }
            }
            return progress;
        }

        public IList<Tile> ClearAfterComplated()
        {
            lock (SuccessfulData)
            {
                TileComparer comparer = new TileComparer();
                var list = SuccessfulData.Except(NeedShowTiles, comparer).ToList();
                list.Select(c => SuccessfulData.Remove(c)).ToList();
                return list.ToList();
            }
        }
    }

}
