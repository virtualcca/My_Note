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
    public partial class ClearCache : PhoneApplicationPage
    {
        public ClearCache()
        {
            
            InitializeComponent();
        }

        private void ClearAllCacheButton_Click(object sender, RoutedEventArgs e)
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
                appStorage.Dispose();
            }
        }

        private void ClearAllNote_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager rm = new ResourceManager(typeof(My_Note.Lang.AppResources));
            if (MessageBox.Show(rm.GetString("SettingPageClearAllNoteMessage"), rm.GetString("SettingPageClearButtonMessageTitle"), MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                //ommit
            }
            else
            {
                using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string[] filelist = appStorage.GetFileNames("*.txt");

                    foreach (string file in filelist)
                    {
                        appStorage.DeleteFile(file);
                    }

                    string[] jpglist = appStorage.GetFileNames("*.jpg");

                    foreach (string file in jpglist)
                    {
                        appStorage.DeleteFile(file);
                    }
                }
            }
        }

        private void ClearAllVoiceButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager rm = new ResourceManager(typeof(My_Note.Lang.AppResources));
            if (MessageBox.Show(rm.GetString("SettingPageClearAllVoiceMessage"), rm.GetString("SettingPageClearButtonMessageTitle"), MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                //ommit
            }
            else
            {
                using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string[] filelist = appStorage.GetFileNames("*.wav");

                    foreach (string file in filelist)
                    {
                        appStorage.DeleteFile(file);
                    }
                }
            }
        }

    }
}