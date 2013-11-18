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
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Windows.Media.Imaging;

namespace My_Note
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            //var currentTheme = Application.Current.Resources["PhoneBackgroundColor"].ToString();
            //if (currentTheme == "#FF000000")
            //{
            //    Brush br = new ImageBrush();
            //    ImageBrush bg = new ImageBrush();
            //    bg.ImageSource = new BitmapImage(new Uri("Images/DarkBackGround.jpg", UriKind.Relative));
            //    br = bg;
            //    PanoramaView.Background = br;
            //}
            //else
            //{
            //    ImageBrush bg = new ImageBrush();
            //    bg.ImageSource = new BitmapImage(new Uri("Images/LightBackGround.jpg", UriKind.Relative));
            //    PanoramaView.Background = bg;
            //}
        }

        //页面载入事件
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            string state = "";
            if(settings.Contains("state"))
            {
                if(settings.TryGetValue<string>("state",out state))
                {
                    if(state == "edit")
                    {
                        NavigationService.Navigate(new Uri("/My_Note;component/ViewEdit.xaml",UriKind.Relative));
                    }
                    else if(state == "add")
                    {
                        NavigationService.Navigate(new Uri("/My_Note;component/Add.xaml",UriKind.Relative));
                    }
                    
                }
            }

            BindList();

        }

        #region AppBar Click Handler

        private void AppBar_Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Add.xaml", UriKind.Relative));
        }

        private void AppBar_Help_Click(object sender, EventArgs e)
        {
            HelpCanvas.Visibility = System.Windows.Visibility.Visible;
        }

        private void AppBar_Setting_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/Setting.xaml", UriKind.Relative));
        }

        #endregion

        #region Binding ListBox

        private void BindList()
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            string[] fileList = appStorage.GetFileNames("*.txt");

            List<Note> notes = new List<Note>();

            foreach (string file in fileList)
            {
                if (file != "__ApplicationSettings")
                {
                    string fileName = file;

                    //string title = GetValueOrDefault<string>(file.Substring(0, 20), "未命名");

                    BitmapImage image = new BitmapImage();
                    if (appStorage.FileExists(file + ".jpg"))
                    {
                        Stream imagestream = appStorage.OpenFile(file + ".jpg", FileMode.Open, FileAccess.Read);
                        image.SetSource(imagestream);
                    }
                    else
                    {
                        image = null;
                    }

                    int year = int.Parse(fileName.Substring(0, 4), CultureInfo.InvariantCulture);
                    int month = int.Parse(fileName.Substring(5, 2), CultureInfo.InvariantCulture);
                    int day = int.Parse(fileName.Substring(8, 2),CultureInfo.InvariantCulture);
                    int hour = int.Parse(fileName.Substring(11, 2), CultureInfo.InvariantCulture);
                    int minute = int.Parse(fileName.Substring(14, 2), CultureInfo.InvariantCulture);
                    int second = int.Parse(fileName.Substring(17, 2), CultureInfo.InvariantCulture);

                    DateTime DataCreated = new DateTime(year, month, day, hour, minute, second);

                    string title = fileName.Substring(20);
                    //location = location.Replace("_", ",");
                    //location = location.Replace("-", " ");
                    title = title.Substring(0, title.Length - 4);
                    DateTime data = appStorage.GetLastWriteTime(file).DateTime;

                    notes.Add(new Note(DataCreated, fileName,title,image,data));
                }
            }

            var source = from item in notes orderby item.Data descending select item;

            NoteListBox.ItemsSource = source;

        }

        #endregion

        //添加笔记按钮
        private void NoteLocation_Click(object sender, RoutedEventArgs e)
        { 
            HyperlinkButton hb = (HyperlinkButton)sender;
            string uri = hb.Tag.ToString();
            NavigationService.Navigate(new Uri("/My_Note;component/ViewEdit.xaml?id=" + uri, UriKind.Relative));
        }

        //帮助按钮
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpCanvas.Visibility = System.Windows.Visibility.Collapsed;
        }

        //重写返回按键
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            ResourceManager rm = new ResourceManager(typeof(My_Note.Lang.AppResources));
            if (MessageBox.Show(rm.GetString("ExitMessageText"), rm.GetString("ExitText"), MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                new Microsoft.Xna.Framework.Game().Exit(); 
            }
        }

        //获取独立存储区键值
        public valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            valueType value;

            if (setting.Contains(Key))
            {
                value = (valueType)setting[Key];
            }

            else
            {
                value = defaultValue;
            }

            return value;
        }


        #region ToolBox
        private void ToolBoxCalButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/ToolBox/Calculator.xaml", UriKind.Relative));
        }

        private void ToolBoxToolBoxRecVoiceButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/ToolBox/RecordVoice.xaml", UriKind.Relative));
        }

        #endregion

    }
}