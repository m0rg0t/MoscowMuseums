using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TouristMoscowWP.ViewModel
{
    public class MapItem: ViewModelBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");
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

        protected bool SetProperty<T>(ref T storage, T value, String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set {
                this.SetProperty(ref this._uniqueId, value, "UniqueId"); 
            }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value, "Title"); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value, "Subtitle"); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value, "Description"); }
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value, "Content"); }
        }

        private ImageSource _image = null;

        private string _imagePath = string.Empty;
        public string ImagePath
        {
            get { return this._imagePath; }
            set { this.SetProperty(ref this._imagePath, value, "ImagePath"); }
        }

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this.ImagePath != null)
                {
                    this._image = new BitmapImage(new Uri(this.ImagePath));
                }
                return this._image;
            }

            set
            {
                this.ImagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this.ImagePath = path;
            RaisePropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
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
                if (_lat != value)
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

    }
}
