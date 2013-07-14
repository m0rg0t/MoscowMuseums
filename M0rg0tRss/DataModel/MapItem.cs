using Bing.Maps;
using GalaSoft.MvvmLight;
using M0rg0tRss.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace M0rg0tRss.DataModel
{
    public class MapItem: RssDataItem 
    {
    /*: RssDataItem
    {
        public MapItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, RssDataGroup group, double lat = 0, double lon = 0)
            : base(uniqueId, title, subtitle, imagePath, description, content, group)
        {
            this.Lat = lat;
            this.Lon = lon;
        }*/
        public MapItem()
        {
        }

        private double _lat;
        public double Lat
        {
            get
            {
                return _lat;
            }
            set
            {
                if (_lat!=value)
                {
                    _lat = value;
                };
            }
        }

        private double _object_rate = 0;
        /// <summary>
        /// Рейтинг объекта
        /// </summary>
        public double Object_rate
        {
            get
            {
                return _object_rate;
            }
            set
            {
                if (_object_rate != value)
                {
                    _object_rate = value;
                };
            }
        }

        private string _object_audio_urls = "";
        public string Object_audio_urls
        {
            get
            {
                return _object_audio_urls;
            }
            set
            {
                _object_audio_urls = value;
                try
                {
                    JObject o = JObject.Parse(_object_audio_urls);
                    ObservableCollection<string> items = new ObservableCollection<string>();
                    foreach (var item in o)
                    {
                        items.Add(item.ToString());
                    };
                    Audio_urls = items;
                }
                catch { };
            }
        }

        private ObservableCollection<string> _audio_urls = new ObservableCollection<string>();
        [SQLite.Ignore]
        public ObservableCollection<string> Audio_urls
        {
            get
            {
                return _audio_urls;
            }
            set
            {
                _audio_urls = value;
            }
        }

        [SQLite.Ignore]
        public string RatingText
        {
            get
            {
                return "Рейтинг: " + Object_rate.ToString();
            }
            private set
            {
            }
        }

        private double _object_address;
        /// <summary>
        /// Адрес объекта
        /// </summary>
        public double Object_address
        {
            get
            {
                return _object_address;
            }
            set
            {
                if (_object_address != value)
                {
                    _object_address = value;
                };
            }
        }

        private double _lon;
        public double Lon
        {
            get
            {
                return _lon;
            }
            set
            {
                if (_lon != value)
                {
                    _lon = value;
                };
            }
        }

        [SQLite.Ignore]
        public Bing.Maps.Location Location {
            private set { 

            }
            get {
                return new Location(this.Lat, this.Lon); ;
            }
        }
    }
}
