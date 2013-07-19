using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TouristMoscowWP.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private ObservableCollection<MapItem> _randomItems = new ObservableCollection<MapItem>();
        public ObservableCollection<MapItem> RandomItems {
            get {
                return _randomItems;
            }
            set {
                _randomItems = value;
            }
        }

        private ObservableCollection<MapItem> _bestItems = new ObservableCollection<MapItem>();
        public ObservableCollection<MapItem> BestItems
        {
            get
            {
                return _bestItems;
            }
            set
            {
                _bestItems = value;
            }
        }

        /*public async Task<bool> LoadRandomFromDB()
        {
            try
            {
                await prepareData();
            }
            catch { };

            var dbPath = "Data/places.db";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);

            try
            {
                var SomeItems = await conn.QueryAsync<MapItem>("SELECT * FROM MapItem ORDER BY RANDOM() LIMIT 0,200");
                foreach (var item in SomeItems)
                {
                    RandomItems.Add(item);
                };
                RaisePropertyChanged("RandomItems");
            }
            catch { };
            return true;
        }*/

        private ObservableCollection<MapItem> _items = new ObservableCollection<MapItem>();
        public ObservableCollection<MapItem> Items {
            get {
                return _items;
            }
            set {
                if (_items!=value) {
                    _items = value;
                    RaisePropertyChanged("Items");
                };
            }
        }

        private async Task<bool> prepareData()
        {
            /*var localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync
("Data", CreationCollisionOption.OpenIfExists);
            //�������� ������ ������ � ����� Data
            var files = await localFolder.GetFilesAsync();
            //�������� ������ ���� ������, ��� ������� config.xml
            var config = from file in files
                         where file.Name.Equals("places.db")
                         select file;
            var configEntries = config as StorageFile[] ?? config.ToArray();
            if (!configEntries.Any())
                await App.CopyConfigToLocalFolder();*/
            return true;
        }

        public async Task<string> MakeWebRequest(string url = "")
        {
            try
            {
                HttpClient http = new System.Net.Http.HttpClient();
                HttpResponseMessage response = await http.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
            catch {
                return "";
            };
        }

        public async void LoadSearchQuery(string query="") {
            try
            {
                if (query != "")
                {

                    ObservableCollection<MapItem> tempitems = new ObservableCollection<MapItem>();
                    Items = new ObservableCollection<MapItem>();

                    string response = await MakeWebRequest("http://api.pub.emp.msk.ru:8081/json/v10.0/tourstore/objects/search?token=" + App.TOKEN + "&query=" + query);
                    JObject o = JObject.Parse(response.ToString());
                    /*
                    var tourist = new RssDataGroup(category,
                        "Достопримечательности", "Достопримечательности", "", "");
                    tourist.Order = 7;
                    this._allGroups.Add(tourist);
                    RaisePropertyChanged("AllGroups");*/
                    Loading = true;
                    var i = 0;
                    try
                    {
                        foreach (var item in o["result"]["objects"])
                        {
                            if (i < 20)
                            {
                                try
                                {
<<<<<<< HEAD
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
                                    //await conn.QueryAsync<MapItem>("DELETE FROM MapItem WHERE UniqueId='" + currentMapItem.UniqueId + "'");
                                    //await conn.InsertAsync(currentMapItem);
                                    tempitems.Add(currentMapItem);
                                    i++;
                                }
                                catch { };
                            };
                        };
                    }
                    catch { };
                    Items = tempitems;
                    RaisePropertyChanged("Items");
                    Loading = false;
                }
            }
            catch { };
=======
                                    if (i < 20)
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

                                            currentMapItem.Object_audio_urls = item["object_audio_urls"].ToString();
                                            //await conn.QueryAsync<MapItem>("DELETE FROM MapItem WHERE UniqueId='" + currentMapItem.UniqueId + "'");
                                            //await conn.InsertAsync(currentMapItem);
                                            tempitems.Add(currentMapItem);
                                            i++;
                                        }
                                        catch { };
                                    };
                                };
                            } catch {};
                            Items = tempitems;
                            RaisePropertyChanged("Items");
                            Loading = false;
                        }
>>>>>>> 22956bc956503b3515068830dc6bc8b9c760200d
            }

        private bool _loading = false;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }
        
        private MapItem _currentItem = new MapItem();
        public MapItem CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                _currentItem = value;
                RaisePropertyChanged("CurrentItem");
            }
        }

        /*public async Task<bool> LoadBestFromDB()
        {
            try
            {
                await prepareData();
            }
            catch { };
            BestItems = new ObservableCollection<MapItem>();
            var dbPath = "Data/places.db";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);

            try
            {
                var SomeItems = await conn.QueryAsync<MapItem>("SELECT * FROM MapItem ORDER BY Object_rate DESC LIMIT 0,200");
                foreach (var item in SomeItems)
                {
                    BestItems.Add(item);
                };
                RaisePropertyChanged("AllGroups");
            }
            catch { };
            return true;
        }*/


    }
}