using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TouristMoscowWP.ViewModel;
using Coding4Fun.Toolkit.Controls;

namespace TouristMoscowWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = ViewModelLocator.MainStatic;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            /*if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }*/
            if (ViewModelLocator.MainStatic.Items.Count() == 0)
            {
                ViewModelLocator.MainStatic.LoadSearchQuery("музей");
            };            
        }

        private void FindDataButton_Click(object sender, EventArgs e)
        {
            ViewModelLocator.MainStatic.LoadSearchQuery(this.SearchText.Text);
            this.Focus();
        }

        private void FindItemsList_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentItem = (e.Item.Content as MapItem);
                NavigationService.Navigate(new Uri("/ItemPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void privacyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var messagePrompt = new MessagePrompt
                {
                    Title = "Политика конфиденциальности",
                    Body = new TextBlock { Text = "Данный текст описывает, как используется информация, предоставляемая приложению.\nПриложение не собирает никаких данных без вашего ведома.\nПриложение не собирает и не хранит информацию, которая связана с определенным именем.\nМы также делаем все возможное, чтобы обезопасить хранимые данные.\nПринимая условия, которые включают эту политику вы соглашаетесь с данной политикой конфиденциальности.\n\nКонтакты m0rg0t.Anton@gmail.com", MaxHeight = 500, TextWrapping = TextWrapping.Wrap },
                    IsAppBarVisible = false,
                    IsCancelVisible = false
                };
                messagePrompt.Show();
            }
            catch { };

        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModelLocator.MainStatic.LoadSearchQuery(this.SearchText.Text);
                this.Focus();
            };
        }
    }
}