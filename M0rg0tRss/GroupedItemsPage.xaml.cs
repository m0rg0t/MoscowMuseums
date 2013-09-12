using Callisto.Controls;
using M0rg0tRss.Controls;
using M0rg0tRss.Data;
using M0rg0tRss.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WhereIsPolicemanWin8.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы сгруппированных элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234231

namespace M0rg0tRss
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class GroupedItemsPage : M0rg0tRss.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="pageState">Словарь состояния, сохраненного данной страницей в ходе предыдущего
        /// сеанса. Это значение будет равно NULL при первом посещении страницы.</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            //получаем папку с именем Data в локальной папке приложения
            var localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync
               ("Data", CreationCollisionOption.OpenIfExists);

            await ViewModelLocator.MainStatic.LoadRss();
            if (NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel() != 
              NetworkConnectivityLevel.InternetAccess)
            {                  
                    zommedOutView.ItemsSource = groupedItemsViewSource.View.CollectionGroups;
                    OfflineMode.Visibility = Visibility.Visible;
                /*}
                else
                {
                    var msg = new MessageDialog("Для работы приложения необходимо к интернет подключение.");
                    await msg.ShowAsync();
                }*/
            }
            else
            { 
                OfflineMode.Visibility = Visibility.Collapsed;
                zommedOutView.ItemsSource = groupedItemsViewSource.View.CollectionGroups;
            }

            this.BestTile.DataContext = ViewModelLocator.MainStatic.BestItems.Items;
            this.RandomTile.DataContext = ViewModelLocator.MainStatic.RandomItems.Items;
            this.MuseumTile.DataContext = ViewModelLocator.MainStatic.MuseumItems.Items;
            this.ParksTile.DataContext = ViewModelLocator.MainStatic.ParksItems.Items;
            this.CinemaTile.DataContext = ViewModelLocator.MainStatic.CinemaItems.Items;
            this.TheatreTile.DataContext = ViewModelLocator.MainStatic.TheatreItems.Items;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
            base.OnNavigatedTo(e);
        }

        /*private void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var viewPrivacyPage = new SettingsCommand("", "Privacy Statement", cmd =>
            {
                Launcher.LaunchUriAsync(new Uri("http://m0rg0t.com/?p=61", UriKind.Absolute));
            });
            args.Request.ApplicationCommands.Add(viewPrivacyPage);
        }*/

        /// <summary>
        /// Вызывается при нажатии заголовка группы.
        /// </summary>
        /// <param name="sender">Объект Button, используемый в качестве заголовка выбранной группы.</param>
        /// <param name="e">Данные о событии, описывающие, каким образом было инициировано нажатие.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Определение группы, представляемой экземпляром Button
            var group = (sender as FrameworkElement).DataContext;

            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            if (((RssDataGroup)group).UniqueId == "MainNews")
            {
                try
                {
                    var itemId = ((RssDataGroup)group).Items.First().UniqueId;
                    this.Frame.Navigate(typeof(ItemDetailPage), itemId);
                }
                catch { };
            }
            else
            {
                try
                {
                    if (((RssDataGroup)group).UniqueId == "touristBestItems")
                    {
                        this.Frame.Navigate(typeof(GroupDetailPage), "touristBestItems");
                    }
                    else
                    {
                        this.Frame.Navigate(typeof(MapItemsPage), ((RssDataGroup)group).UniqueId);
                    };
                }
                catch { };
            };            
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри группы.
        /// </summary>
        /// <param name="sender">Объект GridView (или ListView, если приложение прикреплено),
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            var itemId = ((RssDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private async void MapButton_Click(object sender, RoutedEventArgs e)
        {
            //await WindowsMapsHelper.MapsHelper.SearchAsync("tourist", "Rybinsk, Yaroslavl', Russia", null);
            try
            {
                this.Frame.Navigate(typeof(MapPage));
            }
            catch { };
        }

        private async void WriteProblem1AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Popup anon = new Popup();
            anon.Child = new AnonimusWriteControl();
            anon.IsLightDismissEnabled = true;
            anon.IsOpen = true;
        }

        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            try
            {
                var viewAboutPage = new SettingsCommand("", "Об авторе", cmd =>
                {
                    //(Window.Current.Content as Frame).Navigate(typeof(AboutPage));
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new About();
                    settingsFlyout.HeaderText = "Об авторе";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutPage);

                var viewAboutMalukahPage = new SettingsCommand("", "Политика конфиденциальности", cmd =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new Privacy();
                    settingsFlyout.HeaderText = "Политика конфиденциальности";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutMalukahPage);

                var refreshDatabase = new SettingsCommand("", "Обновить базу данных", cmd =>
                {
                    ViewModelLocator.MainStatic.LoadTouristQuery();
                });
                args.Request.ApplicationCommands.Add(refreshDatabase);
            }
            catch { };
        }



        public SettingsCommand viewStreetAndTownPage;

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.MainStatic.LoadRandomFromDB();
        }

        private void RadHubTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                this.Frame.Navigate(typeof(MapPage));
            }
            catch { };
        }

        private void RandomTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.RandomItems != null)
                {
                    this.Frame.Navigate(typeof(MapItemsPage), "TouristRandom");
                }
                else
                {
                    MessageDialog item = new MessageDialog("Категория еще не загружена");
                    item.ShowAsync();
                };
            }
            catch { };
        }

        private void BestTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if ((ViewModelLocator.MainStatic.GetGroup("touristBestItems") != null) &&
                    (ViewModelLocator.MainStatic.GetGroup("touristBestItems").Items.Count>0))
                {
                    //this.Frame.Navigate(typeof(MapItemsPage), "touristBestItems");
                    this.Frame.Navigate(typeof(GroupDetailPage), "touristBestItems");
                }
                else
                {
                    MessageDialog item = new MessageDialog("Категория еще не загружена");
                    item.ShowAsync();
                };
            }
            catch { };
        }

        private void MuseumTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.MuseumItems != null)
                {
                    this.Frame.Navigate(typeof(MapItemsPage), "museumGroup");
                }
                else
                {
                    MessageDialog item = new MessageDialog("Категория еще не загружена");
                    item.ShowAsync();
                };
            }
            catch { };
        }

        private void ParksTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.ParksItems != null)
                {
                    this.Frame.Navigate(typeof(MapItemsPage), "parksGroup");
                }
                else
                {
                    MessageDialog item = new MessageDialog("Категория еще не загружена");
                    item.ShowAsync();
                };
            }
            catch { };
        }

        private void CinemaTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.CinemaItems != null)
                {
                    this.Frame.Navigate(typeof(MapItemsPage), "cinemaGroup");
                }
                else
                {
                    MessageDialog item = new MessageDialog("Категория еще не загружена");
                    item.ShowAsync();
                };
            }
            catch { };
        }

        private void TheatreTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.TheatreItems != null)
                {
                    this.Frame.Navigate(typeof(MapItemsPage), "theatreGroup");
                }
                else
                {
                    MessageDialog item = new MessageDialog("Категория еще не загружена");
                    item.ShowAsync();
                };
            }
            catch { };
        }

    }
}
