using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using System.Globalization;
using System.Resources;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using My_Note.Lang;
using System.Text.RegularExpressions;

namespace My_Note
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<Note> Notes;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            GetBindList();
        }

        #region 生成Appbar -BuildLocalizedApplicationBar()
        /// <summary>
        /// 生成Appbar
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 0.78;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton addAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.add.rest.png", UriKind.Relative));
            addAppBarButton.Text = AppResources.MainPageAppbarButtonAdd;
            addAppBarButton.Click += AppBar_Add_Click;
            ApplicationBar.Buttons.Add(addAppBarButton);

            // 使用 AppResources 中的本地化字符串创建新菜单项。
            ApplicationBarMenuItem SettingAppBarMenuItem = new ApplicationBarMenuItem(AppResources.MainPageAppbarMenuSetting);
            SettingAppBarMenuItem.Click += AppBar_Setting_Click;
            ApplicationBar.MenuItems.Add(SettingAppBarMenuItem);
        } 
        #endregion

        #region 页面载入事件 -PhoneApplicationPage_Loaded
        /// <summary>
        /// 页面载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            #region 墓碑事件
            string state = "";
            if (settings.Contains("state"))
            {
                if (settings.TryGetValue<string>("state", out state))
                {
                    if (state == "edit")
                    {
                        NavigationService.Navigate(new Uri("/My_Note;component/ViewEdit.xaml", UriKind.Relative));
                    }
                    else if (state == "add")
                    {
                        NavigationService.Navigate(new Uri("/My_Note;component/Add.xaml", UriKind.Relative));
                    }

                }
            }
            #endregion

            //Dispatcher.BeginInvoke(new Action(() => { GetBindList(); }), null);

        } 
        #endregion

        #region Appbar按钮事件 
        /// <summary>
        /// 增加笔记按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBar_Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Add.xaml", UriKind.Relative));
        }
        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBar_Setting_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/Setting/Setting.xaml", UriKind.Relative));
        }

        #endregion

        #region 获取绑定的List<>对象 -GetBindList
        /// <summary>
        /// 获取绑定的List<>对象
        /// </summary>
        private void GetBindList()
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            string[] fileList = appStorage.GetFileNames("*.txt");
            //fileList = fileList.OrderByDescending(s => int.Parse(Regex.Match(s.Replace('_','\0'), @"\d+").Value)).ToArray();

            Notes = new List<Note>();

            foreach (string file in fileList)
            {
                if (file != "__ApplicationSettings")
                {
                    string fileName = file;

                    BitmapImage image;
                    if (appStorage.FileExists(file + ".jpg"))
                    {
                        image = new BitmapImage();
                        Stream imagestream = appStorage.OpenFile(file + ".jpg", FileMode.Open, FileAccess.Read);
                        image.SetSource(imagestream);
                    }
                    else
                    {
                        image = null;
                    }

                    int year = int.Parse(fileName.Substring(0, 4), CultureInfo.InvariantCulture);
                    int month = int.Parse(fileName.Substring(5, 2), CultureInfo.InvariantCulture);
                    int day = int.Parse(fileName.Substring(8, 2), CultureInfo.InvariantCulture);
                    int hour = int.Parse(fileName.Substring(11, 2), CultureInfo.InvariantCulture);
                    int minute = int.Parse(fileName.Substring(14, 2), CultureInfo.InvariantCulture);
                    int second = int.Parse(fileName.Substring(17, 2), CultureInfo.InvariantCulture);

                    DateTime DataCreated = new DateTime(year, month, day, hour, minute, second);

                    string title = fileName.Substring(20);
                    title = title.Substring(0, title.Length - 4);
                    DateTime data = appStorage.GetLastWriteTime(file).DateTime;

                    Notes.Add(new Note(DataCreated, fileName, title, image, data));
                }
            }

            appStorage.Dispose();
            Notes.Sort((x, y) => y.Data.CompareTo(x.Data));
        } 
        #endregion

        #region 绑定笔记列表 -BindList
        /// <summary>
        /// 绑定笔记列表
        /// </summary>
        private void BindList()
        {
            //var source = from item in ListSource where(NoteListBox.Items.Count<ListSource.Count()) select item;

            if (NoteListBox.Items.Count == 0&&Notes.Count!=0)
            {
                foreach (var item in Notes)
                {
                    NoteListBox.Items.Add(item);
                }
            }

            //NoteListBox.ItemsSource = ListSource;
        }

        #endregion

        #region 笔记点击事件 -NoteClick_Click
        /// <summary>
        /// 笔记点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteClick_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hb = (HyperlinkButton)sender;
            string uri = hb.Tag.ToString();
            NavigationService.Navigate(new Uri("/My_Note;component/ViewEdit.xaml?id=" + uri, UriKind.Relative));
        } 
        #endregion

        #region 重写返回事件 -OnBackKeyPress
        /// <summary>
        /// 重写返回事件
        /// </summary>
        /// <param name="e"></param>
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
        #endregion

        #region 重写界面更新事件 -Page_LayoutUpdated
        /// <summary>
        /// 重写界面更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_LayoutUpdated(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => { BindList(); }), null);
        } 
        #endregion

        #region 工具箱按钮
        /// <summary>
        /// 计算器工具按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBoxCalButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/ToolBox/Calculator.xaml", UriKind.Relative));
        }

        /// <summary>
        /// 录音工具按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBoxToolBoxRecVoiceButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/ToolBox/RecordVoice.xaml", UriKind.Relative));
        }

        #endregion

    }
}