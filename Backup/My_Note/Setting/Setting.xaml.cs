using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Resources;
using System.IO.IsolatedStorage;

namespace My_Note.Setting
{
    public partial class Setting : PhoneApplicationPage
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void FontChange_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/FontChange.xaml", UriKind.Relative));
        }

        private void ThemeChange_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/ThemeChange.xaml", UriKind.Relative));
        }

        private void CacheClear_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/ClearCache.xaml", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/MainPage.xaml", UriKind.Relative));
            base.OnBackKeyPress(e);
        }

        private void HelpPage_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/Help.xaml", UriKind.Relative));
        }

        private void AboutPage_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/About.xaml", UriKind.Relative));
        }
    }
}