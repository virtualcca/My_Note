using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using My_Note.Lang;

namespace My_Note
{
    public partial class ViewEdit : PhoneApplicationPage
    {
        string fileName;
        string titleName;
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public ViewEdit()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        #region 生成Appbar -BuildLocalizedApplicationBar
        //生成AppBar
        private void BuildLocalizedApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 0.78;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton SaveAddAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.save.rest.png", UriKind.Relative));
            SaveAddAppBarButton.Text = AppResources.ViewEditPageAppbarButtonSave;
            SaveAddAppBarButton.Click += AppBar_Save_Click;
            ApplicationBar.Buttons.Add(SaveAddAppBarButton);

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton EditAddAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.edit.rest.png", UriKind.Relative));
            EditAddAppBarButton.Text = AppResources.ViewEditPageAppbarButtonEdit;
            EditAddAppBarButton.Click += AppBar_Edit_Click;
            ApplicationBar.Buttons.Add(EditAddAppBarButton);

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton DeleteAddAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.delete.rest.png", UriKind.Relative));
            DeleteAddAppBarButton.Text = AppResources.ViewEditPageAppbarButtonDelete;
            DeleteAddAppBarButton.Click += Appbar_Delete_Click;
            ApplicationBar.Buttons.Add(DeleteAddAppBarButton);


        } 
        #endregion

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            string state = "";
            if (settings.Contains("state"))
            {
                if (settings.TryGetValue<string>("state",out state))
                {
                    if (state == "edit")
                    {
                        string value = "";
                        string title = "";
                        if (settings.Contains("value"))
                        {
                            if (settings.Contains("filename"))
                            {
                                if (settings.TryGetValue<string>("filename", out value))
                                {
                                    fileName = value;
                                }
                            }
                            if (settings.Contains("title"))
                            {
                                if (settings.TryGetValue<string>("title", out title))
                                {
                                    titleName = title;
                                }
                            }
                            if (settings.TryGetValue<string>("value", out value))
                            {
                                BindEdit(value);

                                var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
                                if (appStorage.FileExists(fileName + ".jpg"))
                                {
                                    System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                                    Image image = new Image();
                                    using (IsolatedStorageFileStream filestream = appStorage.OpenFile(fileName + ".jpg", FileMode.Open, FileAccess.Read))
                                    {
                                        bitmap.SetSource(filestream);
                                        image.Source = bitmap;
                                        ShowImage.Source = bitmap;
                                    }
                                }
                                else
                                {
                                    ShowImage.Visibility = System.Windows.Visibility.Collapsed;
                                }
                            }

                        }
                    }
                    else
                    {
                        BindView();
                    }
                }
                else
                {
                    BindView();
                }
            }
            else
            {
                BindView();
            }

        }

        #region 浏览模式 -BindView
        //浏览模式
        private void BindView()
        {
            fileName = NavigationContext.QueryString["id"];
            titleName = fileName.Substring(20);
            titleName = titleName.Substring(0, titleName.Length - 4);
            TitleTextBlock.Text = titleName;

            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            using (var file = appStorage.OpenFile(fileName, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    DisplayTextBlock.Text = sr.ReadToEnd();
                }
            }

            if (appStorage.FileExists(fileName + ".jpg"))
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                Image image = new Image();
                using (IsolatedStorageFileStream filestream = appStorage.OpenFile(fileName + ".jpg", FileMode.Open, FileAccess.Read))
                {
                    bitmap.SetSource(filestream);
                    image.Source = bitmap;
                    ShowImage.Source = bitmap;
                }
            }
            else
            {
                ShowImage.Visibility = System.Windows.Visibility.Collapsed;
            }
        } 
        #endregion

        #region 编辑模式 -BindEdit
        //编辑模式
        private void BindEdit(string content)
        {
            DisplayTextBox.Text = content;
            DisplayTextBlock.Visibility = System.Windows.Visibility.Collapsed;
            DisplayTextBox.Visibility = System.Windows.Visibility.Visible;
            TitleTextBox.Text = titleName;
            TitleTextBlock.Visibility = System.Windows.Visibility.Collapsed;
            TitleTextBox.Visibility = System.Windows.Visibility.Visible;

            My_Note.Other.StaticMethod.AddOrUpdata("state", "edit");
            My_Note.Other.StaticMethod.AddOrUpdata("value", DisplayTextBox.Text);
            My_Note.Other.StaticMethod.AddOrUpdata("filename", fileName);
            My_Note.Other.StaticMethod.AddOrUpdata("title", titleName);
            //settings["state"] = "edit";
            //settings["value"] = DisplayTextBox.Text;
            //settings["filename"] = fileName;
            //settings["title"] = titleName;
        } 
        #endregion

        #region AppBar点击事件处理

        /// <summary>
        /// 保存更改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBar_Save_Click(object sender, EventArgs e)
        {
            if (DisplayTextBox.Visibility == System.Windows.Visibility.Visible)
            {
                DisplayTextBlock.Visibility = System.Windows.Visibility.Visible;
                DisplayTextBox.Visibility = System.Windows.Visibility.Collapsed;
                SaveNote();
            }
        }

        /// <summary>
        /// 进入编辑模式按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBar_Edit_Click(object sender, EventArgs e)
        {
            if (DisplayTextBlock.Visibility == System.Windows.Visibility.Visible)
            {

                BindEdit(DisplayTextBlock.Text);
            }
        }

        private void Appbar_Delete_Click(object sender, EventArgs e)
        {
            DeleteDialog.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region 保存笔记 -SaveNote
        //保存笔记
        private void SaveNote()
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            string newfilename = fileName.Substring(0, 20) + TitleTextBox.Text + ".txt";

            if (newfilename == fileName)    //如果文件名没有变更的话
            {
                using (var file = appStorage.OpenFile(newfilename, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        sw.WriteLine(DisplayTextBox.Text);
                    }
                }
            }
            else   //文件名如果变更了的话
            {
                using (var file = appStorage.OpenFile(newfilename, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        sw.WriteLine(DisplayTextBox.Text);
                    }
                }
                appStorage.DeleteFile(fileName);

                if (appStorage.FileExists(fileName + ".jpg"))
                {
                    appStorage.CopyFile(fileName + ".jpg", newfilename + ".jpg");
                    appStorage.DeleteFile(fileName + ".jpg");
                }
            }
            NavigateToMainPage();
        } 
        #endregion

        #region 笔记删除菜单
        /// <summary>
        /// 取消删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteDialog.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 删除笔记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            appStorage.DeleteFile(fileName);
            appStorage.DeleteFile(fileName + ".jpg");

            NavigateToMainPage();
        } 
        #endregion

        #region 显示图片大图 -ShowImage_Click
        /// <summary>
        /// 显示图片大图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowImage_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string uri = fileName + ".jpg";
            NavigationService.Navigate(new Uri("/My_Note;component/Other/ShowImage.xaml?id=" + uri, UriKind.Relative));
        } 
        #endregion

        #region 导航到主页 -NavigateToMainPage
        /// <summary>
        /// 导航到主页
        /// </summary>
        private void NavigateToMainPage()
        {
            settings["filename"] = "";
            settings["state"] = "";
            settings["edit"] = "";
            settings["value"] = "";
            settings["title"] = "";
            NavigationService.Navigate(new Uri("/My_Note;component/MainPage.xaml", UriKind.Relative));
        } 
        #endregion

        #region 重写系统事件
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigateToMainPage();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (My_Note.Other.StaticMethod.GetValueOrDefault<string>("state", "") != "")
            {
                My_Note.Other.StaticMethod.AddOrUpdata("filename", fileName);
                My_Note.Other.StaticMethod.AddOrUpdata("title", TitleTextBox.Text);
                My_Note.Other.StaticMethod.AddOrUpdata("value", DisplayTextBox.Text);
            }
            base.OnNavigatedFrom(e);
        }
        #endregion

        private void DisplayTextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            My_Note.Other.StaticMethod.AddOrUpdata("value", DisplayTextBox.Text);
        }

        private void TitleTextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            My_Note.Other.StaticMethod.AddOrUpdata("title", TitleTextBox.Text);
        }
    }
}