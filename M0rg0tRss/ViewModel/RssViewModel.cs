using GalaSoft.MvvmLight;
using M0rg0tRss.Data;
using M0rg0tRss.DataModel;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Syndication;

namespace M0rg0tRss.ViewModel
{
    public class RssViewModel : ViewModelBase
    {
        private bool _loading = false;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                if (_loading != value)
                {
                    _loading = value;
                    RaisePropertyChanged("Loading");
                };
            }
        }

        private bool _loaded = false;
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
            set
            {
                if (_loaded != value)
                {
                    _loaded = value;
                    RaisePropertyChanged("Loaded");
                };
            }
        }

        public async Task<bool> LoadCacheRss(StorageFile[] feedsEntries)
        {
            Loading = true;            

            foreach (var feed in feedsEntries)
            {
                //await ViewModelLocator.MainStatic.AddGroupForFeedAsync(feed);
            }

            try
            {
                //UpdateTile();
            }
            catch { };
            //RaisePropertyChanged("AllGroups");
            Loading = false;
            return true;
        }

        public async Task<bool> LoadRss()
        {
            if (Loaded == false)
            {
                Loading = true;
                
                //LoadTouristQuery();
                await LoadRandomFromDB();
                await LoadBestFromDB();
                await LoadCustomGroupFromDB("Музей", "museumGroup", 10, "Музеи Москвы");
                await LoadCustomGroupFromDB("Парк", "parksGroup", 10, "Парки Москвы");
                await LoadCustomGroupFromDB("кинотеатр", "cinemaGroup", 10, "Кинотеатры Москвы", true);
                await LoadCustomGroupFromDB("театр", "theatreGroup", 10, "Театры Москвы");

                BestItems = ViewModelLocator.MainStatic.GetGroup("touristBestItems");
                RandomItems = ViewModelLocator.MainStatic.GetGroup("TouristRandom");
                MuseumItems = ViewModelLocator.MainStatic.GetGroup("museumGroup");
                ParksItems = ViewModelLocator.MainStatic.GetGroup("parksGroup");
                CinemaItems = ViewModelLocator.MainStatic.GetGroup("cinemaGroup");
                TheatreItems = ViewModelLocator.MainStatic.GetGroup("theatreGroup");

                var feeds = await App.ReadSettings();

                foreach (var feed in feeds)
                {
                    //await ViewModelLocator.MainStatic.AddGroupForFeedAsync(feed.url, feed.id, feed.title);
                }
                try
                {
                    //UpdateTile();
                }
                catch { };
                RaisePropertyChanged("AllGroups");
                Loading = false;
                Loaded = true;
            };
            return true;
        }

        private RssDataGroup _TheatreItems;
        /// <summary>
        /// 
        /// </summary>
        public RssDataGroup TheatreItems
        {
            get { return _TheatreItems; }
            set { _TheatreItems = value; }
        }
        

        private RssDataGroup _cinemaItems;
        /// <summary>
        /// 
        /// </summary>
        public RssDataGroup CinemaItems
        {
            get { return _cinemaItems; }
            set { _cinemaItems = value; }
        }
        

        private RssDataGroup _ParksItems;
        /// <summary>
        /// 
        /// </summary>
        public RssDataGroup ParksItems
        {
            get { return _ParksItems; }
            set { _ParksItems = value; }
        }
        

        private RssDataGroup _museumItems;
        /// <summary>
        /// 
        /// </summary>
        public RssDataGroup MuseumItems
        {
            get { return _museumItems; }
            set { _museumItems = value; }
        }
        

        private RssDataGroup _bestItems;
        /// <summary>
        /// 
        /// </summary>
        public RssDataGroup BestItems
        {
            get { return _bestItems; }
            set { 
                _bestItems = value;
                RaisePropertyChanged("RandomItems");
            }
        }

        private RssDataGroup _randomItems;
        /// <summary>
        /// 
        /// </summary>
        public RssDataGroup RandomItems
        {
            get { return _randomItems; }
            set { 
                _randomItems = value;
                RaisePropertyChanged("RandomItems");
            }
        }
        
        

        private ObservableCollection<RssDataGroup> _allGroups = new ObservableCollection<RssDataGroup>();
        public ObservableCollection<RssDataGroup> AllGroups
        {
            get
            {
                /*ObservableCollection<RssDataGroup> tempGroups = new ObservableCollection<RssDataGroup>();
                var sorted = (from groupitem in _allGroups
                              orderby groupitem.Order descending
                              select groupitem).ToList();
                foreach(var item in sorted) {
                    tempGroups.Add(item);
                };
                return tempGroups;*/
                return _allGroups;
            }
            set
            {
                if (_allGroups!=value)
                {
                    _allGroups = value;
                    RaisePropertyChanged("AllGroups");
                }
            }
        }

        public async Task<bool> AddGroupForFeedAsync(StorageFile sf)
        {
            string clearedContent = String.Empty;

            if (GetGroup(sf.DisplayName) != null) return false;

            var feed = new SyndicationFeed();
            feed.LoadFromXml(await XmlDocument.LoadFromFileAsync(sf));

            var feedGroup = new RssDataGroup(
                uniqueId: sf.DisplayName.ToString().Replace(".rss", ""),
                title: "Новости", //feed.Title != null ? feed.Title.Text : null,
                subtitle: feed.Subtitle != null ? feed.Subtitle.Text : null,
                imagePath: feed.ImageUri != null ? feed.ImageUri.ToString() : null,
                description: null);

            foreach (var i in feed.Items)
            {
                string imagePath = GetImageFromPostContents(i);

                if (i.Summary != null)
                    clearedContent = Windows.Data.Html.HtmlUtilities.ConvertToText(i.Summary.Text);
                else
                    if (i.Content != null)
                        clearedContent = Windows.Data.Html.HtmlUtilities.ConvertToText(i.Content.Text);

                if (imagePath != null && feedGroup.Image == null)
                    feedGroup.SetImage(imagePath);

                if (imagePath == null) imagePath = "ms-appx:///Assets/DarkGray.png";

                feedGroup.Items.Add(new RssDataItem(
                    uniqueId: i.Id, title: i.Title.Text, subtitle: null, imagePath: imagePath,
                    description: null, content: clearedContent, @group: feedGroup));
            }

            _allGroups.Add(feedGroup);
            return true;
        }

        public async Task<bool> AddGroupForFeedAsync(string feedUrl, string ID="1", string titleRss="")
        {
            string clearedContent = String.Empty;

            if (GetGroup(feedUrl) != null) return false;

            var feed = await new SyndicationClient().RetrieveFeedAsync(new Uri(feedUrl));

            var localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync
                ("Data", CreationCollisionOption.OpenIfExists);
            //получаем/перезаписываем файл с именем "ID".rss
            var fileToSave = await localFolder.CreateFileAsync(ID + ".rss", CreationCollisionOption.ReplaceExisting);

            //сохраняем фид в этот файл
            await feed.GetXmlDocument(SyndicationFormat.Rss20).SaveToFileAsync(fileToSave);

            var feedGroup = new RssDataGroup(
                uniqueId: ID,
                title: titleRss,
                subtitle: feed.Subtitle != null ? feed.Subtitle.Text : null,
                imagePath: feed.ImageUri != null ? feed.ImageUri.ToString() : null,
                description: null);

            foreach (var i in feed.Items)
            {
                string imagePath = null;
                try
                {
                    imagePath = GetImageFromPostContents(i); ;
                }
                catch { };

                if (i.Summary != null)
                    clearedContent = Windows.Data.Html.HtmlUtilities.ConvertToText(i.Summary.Text);
                else
                    if (i.Content != null)
                        clearedContent = Windows.Data.Html.HtmlUtilities.ConvertToText(i.Content.Text);

                if (imagePath != null && feedGroup.Image == null)
                    feedGroup.SetImage(imagePath);

                if (imagePath == null) imagePath = "ms-appx:///Assets/DarkGray.png";

                try
                {
                    feedGroup.Items.Add(new RssDataItem(
                        uniqueId: i.Id, title: i.Title.Text, subtitle: null, imagePath: imagePath,
                        description: null, content: clearedContent, @group: feedGroup));
                }
                catch { };
            }

            _allGroups.Remove(_allGroups.FirstOrDefault(c=>c.UniqueId == feedGroup.UniqueId));
            _allGroups.Add(feedGroup);
            //AllGroups = SortItems();
            return true;
        }

        private ObservableCollection<RssDataGroup> SortItems()
        {
            ObservableCollection<RssDataGroup> tempGroups = new ObservableCollection<RssDataGroup>();
            var sorted = (from groupitem in _allGroups
                          orderby groupitem.Order descending
                          select groupitem).ToList();
            foreach (var item in sorted)
            {
                tempGroups.Add(item);
            };
            return tempGroups;
        }

        private static string GetImageFromPostContents(SyndicationItem item)
        {
            string text2search = "";

            if (item.Content != null) text2search += item.Content.Text;
            if (item.Summary != null) text2search += item.Summary.Text;

            return Regex.Matches(text2search,
                    @"(?<=<img\s+[^>]*?src=(?<q>['""]))(?<url>.+?)(?=\k<q>)",
                    RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Where(m =>
                {
                    Uri url;
                    if (Uri.TryCreate(m.Groups[0].Value, UriKind.Absolute, out url))
                    {
                        string ext = Path.GetExtension(url.AbsolutePath).ToLower();
                        if (ext == ".png" || ext == ".jpg" || ext == ".bmp") return true;
                    }
                    return false;
                })
                .Select(m => m.Groups[0].Value)
                .FirstOrDefault();
        }

        public IEnumerable<RssDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            return AllGroups;
        }

        public RssDataGroup GetGroup(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public RssDataItem GetItem(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() > 0) return matches.First();
            return null;
        }

        public RssViewModel()
        {            
        }

        private MapItem _currentTouristItem = null;
        public MapItem CurrentTouristItem
        {
            get
            {
                return _currentTouristItem;
            }
            set
            {
                if (_currentTouristItem!=value) {
                    _currentTouristItem = value;
                    RaisePropertyChanged("CurrentTouristItem");
                };
            }
        }

        public void UpdateTile()
        {
            var news = AllGroups.FirstOrDefault(c=>c.UniqueId=="1").Items.ToList();
            var xml = new XmlDocument();
            xml.LoadXml(
                string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<tile>
    <visual branding=""none"">
        <binding template=""TileSquarePeekImageAndText03"">
            <image id=""1"" src=""ms-appx:///Assets/Logo.png"" alt=""alt text""/>
            <text id=""1"">{0}</text>
            <text id=""2"">{1}</text>
            <text id=""3"">{2}</text>
            <text id=""4"">{3}</text>
        </binding>
        <binding template=""TileWidePeekImageAndText02"">                        
            <image id=""1"" src=""ms-appx:///Assets/WideLogo.png"" alt=""alt text""/>
            <text id=""1"">{0}</text>
            <text id=""2"">{1}</text>
            <text id=""3"">{2}</text>
            <text id=""4"">{3}</text>
        </binding>  
    </visual>
</tile>",
                    news.Count > 0 ? System.Net.WebUtility.HtmlEncode(news[0].Title) : "",
                    news.Count > 1 ? System.Net.WebUtility.HtmlEncode(news[1].Title) : "",
                    news.Count > 2 ? System.Net.WebUtility.HtmlEncode(news[2].Title) : "",
                    news.Count > 3 ? System.Net.WebUtility.HtmlEncode(news[3].Title) : ""));
            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(xml));
        }

        public async Task<string> MakeWebRequest(string url = "")
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }


        public async Task<bool> LoadRandomFromDB()
        {
            try
            {
                await prepareData();
            }
            catch { };

            var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Data/places.db");
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);

            var tourist = new RssDataGroup("TouristRandom",
                "Случайные достопримечательности", "Достопримечательности", "", "");
            tourist.Order = 15;

            try
            {
                var SomeItems = await conn.QueryAsync<MapItem>("SELECT * FROM MapItem ORDER BY RANDOM() LIMIT 0,200");
                foreach (var item in SomeItems)
                {
                    item.Group = tourist;
                    tourist.Items.Add(item);
                };
                try
                {
                    this._allGroups.Remove(this._allGroups.FirstOrDefault(c => c.UniqueId == "TouristRandom"));
                }
                catch { };
                tourist.ImagePath = tourist.Items.First().ImagePath;
                this._allGroups.Add(tourist);
                RaisePropertyChanged("AllGroups");
            }
            catch { };
            return true;
        }

        private async Task<bool> prepareData()
        {
            var localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync
("Data", CreationCollisionOption.OpenIfExists);
            //получаем список файлов в папке Data
            var files = await localFolder.GetFilesAsync();
            //получаем список всех файлов, имя которых config.xml
            var config = from file in files
                         where file.Name.Equals("places.db")
                         select file;
            var configEntries = config as StorageFile[] ?? config.ToArray();
            if (!configEntries.Any())
                await App.CopyConfigToLocalFolder();
            return true;
        }



        public async Task<bool> LoadBestFromDB()
        {
            try
            {
                await prepareData();
            }
            catch { };

            var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Data/places.db");
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);

            var tourist = new RssDataGroup("touristBestItems",
                "Лучшие по рейтингу", "Достопримечательности", "", "");
            tourist.Order = 15;

            try
            {
                var SomeItems = await conn.QueryAsync<MapItem>("SELECT * FROM MapItem ORDER BY Object_rate DESC LIMIT 0,200");
                foreach (var item in SomeItems)
                {
                    item.Group = tourist;
                    tourist.Items.Add(item);
                };
                try
                {
                    this._allGroups.Remove(this._allGroups.FirstOrDefault(c => c.UniqueId == "TouristBest"));
                    tourist.ImagePath = tourist.Items.First().ImagePath;
                }
                catch { };
                this._allGroups.Add(tourist);
                RaisePropertyChanged("AllGroups");
            }
            catch { };
            return true;
        }

        public async Task<bool> LoadCustomGroupFromDB(string search = "", string uniqueId = "", int order = 10, string GroupName="", bool searchInContent=false)
        {
            if (search != "")
            {
                try
                {
                    try
                    {
                        await prepareData();
                    }
                    catch { };

                    var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Data/places.db");
                    SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);

                    var tourist = new RssDataGroup(uniqueId,
                        GroupName, "Достопримечательности", "", "");
                    tourist.Order = order;

                    try
                    {
                        //OR Content LIKE '%" + search + "%'
                        string query = "SELECT * FROM MapItem WHERE (Title LIKE '%" + search + "%') OR (Description LIKE '%" + search + "%') OR (Content LIKE '%" + search + "%') ORDER BY Object_rate DESC LIMIT 0,200";
                        query = "SELECT * FROM MapItem ORDER BY Object_rate";
                        var SomeItems = await conn.QueryAsync<MapItem>(query);
                        var i = 0;
                        foreach (var item in SomeItems)
                        {
                            if (searchInContent) {
                                if (((item.Title != null && item.Title.Contains(search)) || (item.Content != null && item.Content.Contains(search))) && (i < 100)) //(item.Content != null && item.Content.Contains(search))
                                {
                                    i++;
                                    item.Group = tourist;
                                    tourist.Items.Add(item);
                                };
                            } else {
                                if ((item.Title != null && item.Title.Contains(search)) && (i < 100)) //(item.Content != null && item.Content.Contains(search))
                                {
                                    i++;
                                    item.Group = tourist;
                                    tourist.Items.Add(item);
                                };
                            };                            
                        };

                        if (i > 0)
                        {
                            try
                            {
                                this._allGroups.Remove(this._allGroups.FirstOrDefault(c => c.UniqueId == uniqueId));
                                tourist.ImagePath = tourist.Items.First().ImagePath;
                            }
                            catch { };
                            this._allGroups.Add(tourist);
                            RaisePropertyChanged("AllGroups");
                        };
                    }
                    catch { };
                }
                catch { };
            };
            return true;
        }


        public async Task<bool> LoadSearchFromDB(string search = "")
        {
            if (search != "")
            {
                try
                {
                    try
                    {
                        await prepareData();
                    }
                    catch { };

                    var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Data/places.db");
                    SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);

                    var tourist = new RssDataGroup("TouristSearch",
                        "Результаты поиска " + search.Trim(), "Достопримечательности", "", "");
                    tourist.Order = 554;

                    try
                    {
                        //OR Content LIKE '%" + search + "%'
                        string query = "SELECT * FROM MapItem WHERE (Title LIKE '%" + search + "%') OR (Description LIKE '%" + search + "%') OR (Content LIKE '%" + search + "%') ORDER BY Object_rate DESC LIMIT 0,200";
                        query = "SELECT * FROM MapItem ORDER BY Object_rate";
                        var SomeItems = await conn.QueryAsync<MapItem>(query);
                        var i = 0;
                        foreach (var item in SomeItems)
                        {
                            if ((item.Title != null && item.Title.Contains(search) ||
                                (item.Content != null && item.Content.Contains(search))) && (i<100))
                            {
                                i++;
                                item.Group = tourist;
                                tourist.Items.Add(item);
                            };
                        };
                        
                        if (i > 0)
                        {
                            try
                            {
                                this._allGroups.Remove(this._allGroups.FirstOrDefault(c => c.UniqueId == "TouristSearch"));
                                tourist.ImagePath = tourist.Items.First().ImagePath;
                            }
                            catch { };                            
                            this._allGroups.Add(tourist);
                            RaisePropertyChanged("AllGroups");
                        };
                    }
                    catch { };
                }
                catch { };
            };
            return true;
        }

        /// <summary>
        /// Обновление базы данных
        /// </summary>
        /// <param name="query"></param>
        /// <param name="category"></param>
        public async void LoadTouristQuery(string query = "", string category = "Tourist")
        {
            var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Data/places.db");
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            await conn.CreateTableAsync<MapItem>();

            string response = await MakeWebRequest("http://api.pub.emp.msk.ru:8081/json/v10.0/tourstore/objects/search?token=" + App.TOKEN + "&query=");
            JObject o = JObject.Parse(response.ToString());
            /*
            var tourist = new RssDataGroup(category,
                "Достопримечательности", "Достопримечательности", "", "");
            tourist.Order = 7;
            this._allGroups.Add(tourist);
            RaisePropertyChanged("AllGroups");*/
            Loading = true;
            foreach (var item in o["result"]["objects"])
            {
                try
                {
                    string obj_response = await MakeWebRequest("http://api.pub.emp.msk.ru:8081/json/v10.0/tourstore/objects/get?token=" + App.TOKEN + "&object_id=" + item["id"].ToString());
                    JObject obj = JObject.Parse(obj_response.ToString());

                    MapItem currentMapItem = new MapItem();
                    currentMapItem.UniqueId = obj["result"]["object_id"].ToString();
                    currentMapItem.Title = obj["result"]["object_name"].ToString();
                    currentMapItem.Subtitle = obj["result"]["object_address"].ToString();
                    currentMapItem.SetImage(obj["result"]["object_photo"].ToString());
                    currentMapItem.ImagePath = obj["result"]["object_photo"].ToString();
                    currentMapItem.Description = obj["result"]["object_description"].ToString();
                    currentMapItem.Content = obj["result"]["object_story"].ToString();
                    currentMapItem.Lat = item["object_geo"]["latitude"].Value<Double>();
                    currentMapItem.Lon = item["object_geo"]["longitude"].Value<Double>();
                    currentMapItem.Object_rate = item["object_rate"].Value<Double>();

                    try
                    {                        
                        currentMapItem.Object_audio_urls = obj["result"]["object_audio_urls"].ToString();
                        if (currentMapItem.Object_audio_urls != "" && currentMapItem.Object_audio_urls != null && currentMapItem.Object_audio_urls != "[]")
                        {
                            string id = obj["result"]["object_name"].ToString();
                        };
                       //currentMapItem.Object_audio_urls
                    }
                    catch {
                        currentMapItem.Object_audio_urls = "";
                    };

                    try
                    {
                        currentMapItem.Object_image_urls = obj["result"]["object_img_urls"].ToString();
                        //currentMapItem.Object_audio_urls
                    }
                    catch
                    {
                        currentMapItem.Object_audio_urls = "";
                    };

                    await conn.QueryAsync<MapItem>("DELETE FROM MapItem WHERE UniqueId='" + currentMapItem.UniqueId + "'");
                    await conn.InsertAsync(currentMapItem);
                }
                catch { };
            };
            Loading = false;
        }

    }
}
