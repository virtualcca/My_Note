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
using System.IO.IsolatedStorage;
using System.Resources;

namespace My_Note
{
    public partial class Setting : PhoneApplicationPage
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting.xaml?guid=" + Guid.NewGuid() , UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void ClearIsolatedStorageButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager rm = new ResourceManager(typeof(My_Note.Lang.AppResources));
            if (MessageBox.Show(rm.GetString("SettingPageClearButtonMessage"), rm.GetString("SettingPageClearButtonMessageTitle"), MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                //ommit
            }
            else
            {
                var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
                appStorage.Remove();
                var appSetting = IsolatedStorageSettings.ApplicationSettings;
                appSetting.Clear();
            }
            
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/MainPage.xaml",UriKind.Relative));
            base.OnBackKeyPress(e);
        }

        private void ListPicker_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count.ToString() == "0")
            {
                return;
            }
            NavigationService.Navigate(new Uri("/My_Note;component/Setting.xaml?guid=" + Guid.NewGuid(), UriKind.Relative));
        }
    }
}