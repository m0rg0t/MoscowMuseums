using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.IO;
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

        public async Task<bool> LoadRandomFromDB()
        {
            try
            {
                await prepareData();
            }
            catch { };

            var dbPath = "Data/places.db";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);

            /*var tourist = new RssDataGroup("TouristRandom",
                "Случайные достопримечательности", "Достопримечательности", "", "");
            tourist.Order = 15;*/

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
        }

        private async Task<bool> prepareData()
        {
            /*var localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync
("Data", CreationCollisionOption.OpenIfExists);
            //получаем список файлов в папке Data
            var files = await localFolder.GetFilesAsync();
            //получаем список всех файлов, имя которых config.xml
            var config = from file in files
                         where file.Name.Equals("places.db")
                         select file;
            var configEntries = config as StorageFile[] ?? config.ToArray();
            if (!configEntries.Any())
                await App.CopyConfigToLocalFolder();*/
            return true;
        }

        public async Task<bool> LoadBestFromDB()
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
        }


    }
}