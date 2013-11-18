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
using System.Collections;
using System.Windows.Media;

namespace My_Note.Setting
{
    public partial class ThemeChange : PhoneApplicationPage
    {
        private const string phoneThemeBackGround = "PhoneThemeBackGround";
        bool isInitial = false;
        public ThemeChange()
        {
            InitializeComponent();
            isInitial = true;
            SelectThemeList.SelectedIndex = My_Note.Other.StaticMethod.GetValueOrDefault<int>(phoneThemeBackGround, 0);
        }

        private void ListPicker_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count.ToString() == "0"||isInitial==false)
            {
                return;
            }
            My_Note.Other.StaticMethod.AddOrUpdata(phoneThemeBackGround, SelectThemeList.SelectedIndex);
            MergeCustomColors();
            //NavigationService.Navigate(new Uri("/My_Note;component/Setting/ThemeChange.xaml?date=" + DateTime.Now.ToString(), UriKind.Relative));
        }

        private void MergeCustomColors()
        {
            int value;
            IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
            if (setting.Contains("PhoneThemeBackGround"))
            {
                value = (int)setting["PhoneThemeBackGround"];
            }

            else
            {
                value = 0;
            }

            string source;

            if (value == 0)
                source = String.Format("/My_Note;component/Style/LightThemeResources.xaml");
            else
                source = String.Format("/My_Note;component/Style/DarkThemeResources.xaml");

            var dictionaries = new ResourceDictionary();
            var themeStyles = new ResourceDictionary { Source = new Uri(source, UriKind.Relative) };
            dictionaries.MergedDictionaries.Add(themeStyles);


            ResourceDictionary appResources = App.Current.Resources;
            foreach (DictionaryEntry entry in dictionaries.MergedDictionaries[0])
            {
                SolidColorBrush colorBrush = entry.Value as SolidColorBrush;
                SolidColorBrush existingBrush = appResources[entry.Key] as SolidColorBrush;
                if (existingBrush != null && colorBrush != null)
                {
                    existingBrush.Color = colorBrush.Color;
                }
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/Setting.xaml", UriKind.Relative));
            base.OnBackKeyPress(e);
        }
    }
}