using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using System.IO;
using Windows.Storage;
using System.Xml.Linq;
using Windows.UI.Xaml;

// 此文件定义的数据模型可充当在添加、移除或修改成员时
// 支持通知的强类型模型的代表性示例。所选
// 属性名称与标准项模板中的数据绑定一致。
//
// 应用程序可以使用此模型作为起始点并以它为基础构建，或完全放弃它并
// 替换为适合其需求的其他内容。

namespace REST_SampleCode.Data
{
    /// <summary>
    /// <see cref="SampleDataItem"/> 和 <see cref="SampleDataGroup"/> 的基类，
    /// 定义对两者通用的属性。
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : REST_SampleCode.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imageItemsPath, String imageGroupsPath, String description, ResourceDictionary res, String brushPath)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imageItemsPath = imageItemsPath;
            this._imageGroupsPath = imageGroupsPath;
            this._resources = res;
            this._brushPath = brushPath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _itemsImage = null;
        private String _imageItemsPath = null;
        public ImageSource ItemsImage
        {
            get
            {
                if (this._itemsImage == null && this._imageItemsPath != null)
                {
                    this._itemsImage = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imageItemsPath));
                }
                return this._itemsImage;
            }

            set
            {
                this._imageItemsPath = null;
                this.SetProperty(ref this._itemsImage, value);
            }
        }

        public void SetItemsImage(String path)
        {
            this._itemsImage = null;
            this._imageItemsPath = path;
            this.OnPropertyChanged("Image");
        }

        private ImageSource _groupsImage = null;
        private String _imageGroupsPath = null;
        public ImageSource GroupsImage
        {
            get
            {
                if (this._groupsImage == null && this._imageGroupsPath != null)
                {
                    this._groupsImage = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imageGroupsPath));
                }
                return this._groupsImage;
            }

            set
            {
                this._imageGroupsPath = null;
                this.SetProperty(ref this._groupsImage, value);
            }
        }

        public void SetGroupsImage(String path)
        {
            this._groupsImage = null;
            this._imageGroupsPath = path;
            this.OnPropertyChanged("Image");
        }

        private LinearGradientBrush _brush = null;
        private String _brushPath = null;
        private ResourceDictionary _resources;
        public LinearGradientBrush Brush
        {
            get
            {
                if (this._brush == null && this._brushPath != null)
                {
                    this._brush = this._resources[_brushPath] as LinearGradientBrush;
                }
                return this._brush;
            }

            set
            {
                this._imageGroupsPath = null;
                this.SetProperty(ref this._brush, value);
            }
        }

        public void setBrushPath(String path)
        {
            this._brush = null;
            this._brushPath = path;
            this.OnPropertyChanged("Brush");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// 泛型项数据模型。
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imageItemsPath, String imageGroupsPath, String description, String content, SampleDataGroup group, string csCode, string xamlCode, ResourceDictionary res, String brushPath)
            : base(uniqueId, title, subtitle, imageItemsPath, imageGroupsPath, description, res, brushPath)
        {
            this._content = content;
            this._group = group;
            this.CSCode = csCode;
            this.XamlCode = xamlCode;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }

        public string CSCode
        {
            get;
            set;
        }

        public string XamlCode
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 泛型组数据模型。
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imageItemsPath, String imageGroupsPath, String description, ResourceDictionary res, String brushPath)
            : base(uniqueId, title, subtitle, imageItemsPath, imageGroupsPath, description, res, brushPath)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // 由于两个原因提供要从 GroupedItemsPage 绑定到的完整
            // 项集合的子集: GridView 不会虚拟化大型项集合，并且它
            // 可在浏览包含大量项的组时改进用户
            // 体验。
            //
            // 最多显示 4 项，因为无论显示 1、2、3、4 还是 4 行，
            // 它都生成填充网格列

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 4)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        if (TopItems.Count > 4)
                        {
                            TopItems.RemoveAt(4);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 4 && e.NewStartingIndex < 4)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 4)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 4)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(4);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 4)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 4)
                        {
                            TopItems.Add(Items[3]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 4)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 4)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get { return this._topItem; }
        }
    }

    /// <summary>
    /// 创建包含硬编码内容的组和项的集合。
    /// 
    /// SampleDataSource 用占位符数据而不是实时生产数据
    /// 初始化，因此在设计时和运行时均需提供示例数据。
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource;

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();

        private static ResourceDictionary resources;

        public static ResourceDictionary Resources
        {
            get { return SampleDataSource.resources; }
            set
            {
                SampleDataSource.resources = value;
                _sampleDataSource = new SampleDataSource();
            }
        }
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // 对于小型数据集可接受简单线性搜索
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // 对于小型数据集可接受简单线性搜索
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async void ReadData()
        {
            XDocument document = XDocument.Load("SDKConfig.xml");
            foreach (var group in document.Root.Elements("Category"))
            {
                string groupName = (string)group.Element("name");
                string groupStyle = (string)group.Element("style");

                SampleDataGroup g = new SampleDataGroup(groupName, groupName, groupName, "", "", groupName, resources, groupStyle);
                foreach (var o in group.Elements("items").Elements<XElement>("item"))
                {
                    string name = (string)o.Element("id");
                    string xaml = (string)o.Element("xaml");
                    string source = (string)o.Element("source");
                    string code = (string)o.Element("code");
                    string imageItemsPath = (string)o.Element("imageItemsPath");
                    string imageGroupsPath = (string)o.Element("imageGroupsPath");
                    string explain = string.Empty;
                    try
                    {
                        explain = await PathIO.ReadTextAsync(string.Format("ms-appx:///{0}", (string)o.Element("explain")));
                    }
                    catch { }
                    SampleDataItem i = new SampleDataItem(name, name, name, imageItemsPath, imageGroupsPath, explain, xaml, g, code, source, resources, groupStyle);
                    g.Items.Add(i);
                }
                AllGroups.Add(g);
            }
        }

        public SampleDataSource()
        {
            ReadData();
        }
    }

    public sealed class ItemShow
    {
        public string Xaml
        {
            get;
            set;
        }

        public string CS
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public SampleDataItem ItemData
        {
            get;
            set;
        }
        public string HelpContent { get; set; }
    }
}
