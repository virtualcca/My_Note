using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Globalization;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using My_Note.Lang;


namespace My_Note
{
    public partial class Add : PhoneApplicationPage
    {
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        PhotoChooserTask photoChooserTask;
        BitmapImage bitmap;


        public Add()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();

            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            photoChooserTask.ShowCamera = true;

        }

        #region 生成Appbar -BuildLocalizedApplicationBar
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
            ApplicationBarIconButton saveAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.save.rest.png", UriKind.Relative));
            saveAppBarButton.Text = AppResources.AddPageAppbarButtonSave;
            saveAppBarButton.Click += Appbar_Save_Click;
            ApplicationBar.Buttons.Add(saveAppBarButton);

            ApplicationBarIconButton cameraAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.feature.camera.rest.png", UriKind.Relative));
            cameraAppBarButton.Text = AppResources.AddPageAppbarButtonCamera;
            cameraAppBarButton.Click += Appbar_Camera_Click;
            ApplicationBar.Buttons.Add(cameraAppBarButton);

        } 
        #endregion

        #region 页面载入事件
        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            string state = "";
            if (settings.Contains("state"))
            {
                if (settings.TryGetValue<string>("state", out state))
                {
                    if (state == "add")
                    {
                        string value = "";
                        if (settings.TryGetValue<string>("value", out value))
                        {
                            EditTextBox.Text = value;
                        }
                        string titlevalue = "";
                        if (settings.TryGetValue<string>("title", out titlevalue))
                        {
                            NoteTitle.Text = titlevalue;
                        }
                        using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            if (appStorage.FileExists("My_Note_tempimage.jpg"))
                            {
                                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                                using (IsolatedStorageFileStream filestream = appStorage.OpenFile("My_Note_tempimage.jpg", FileMode.Open, FileAccess.Read))
                                {
                                    bitmap.SetSource(filestream);
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
            }

            settings["state"] = "add";
            settings["value"] = EditTextBox.Text;
        } 
        #endregion

        #region AppBar 点击事件
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appbar_Save_Click(object sender, EventArgs e)
        {
            if (NoteTitle.Text.Trim().Length == 0 || EditTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("标题或者内容不能为空");
                EditTextBox.Focus();
                return;
            }

            ////Save Method
            //if (location == null||location.Trim().Length == 0)
            //{
            //    location = "Unknown Location";
            //}

            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.Year);
            sb.Append("_");
            sb.Append(string.Format(CultureInfo.InvariantCulture,"{0:00}", DateTime.Now.Month));
            sb.Append("_");
            sb.Append(string.Format(CultureInfo.InvariantCulture,"{0:00}", DateTime.Now.Day));
            sb.Append("_");
            sb.Append(string.Format(CultureInfo.InvariantCulture,"{0:00}", DateTime.Now.Hour));
            sb.Append("_");
            sb.Append(string.Format(CultureInfo.InvariantCulture,"{0:00}", DateTime.Now.Minute));
            sb.Append("_");
            sb.Append(string.Format(CultureInfo.InvariantCulture,"{0:00}", DateTime.Now.Second));
            sb.Append("_");

            //增加标题的键值对
            //if (!AddOrUpdata(sb.ToString(), NoteTitle.Text))
            //{
            //    settings[sb.ToString()] = NoteTitle.Text;
            //}

            //location = location.Replace(" ", "-");
            //location = location.Replace(",", "_");
            sb.Append(NoteTitle.Text);
            sb.Append(".txt");

            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            using (var fileName = appStorage.OpenFile(sb.ToString(),FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine(EditTextBox.Text);
                }
            }

            if(appStorage.FileExists("My_Note_tempimage.jpg"))
            {
                appStorage.CopyFile("My_Note_tempimage.jpg", sb.ToString() + ".jpg");
                appStorage.DeleteFile("My_Note_tempimg.jpg");
            }

            settings["state"] = "";
            settings["value"] = "";
            MessageBox.Show("保存成功");

            appStorage.Dispose();

            NavigateBack();
        }

        /// <summary>
        /// 清理图片按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appbar_Menu_ClearImage_Click(object sender, EventArgs e)
        {
            bitmap = null;
            ShowImage.Source = null;
        }

        /// <summary>
        /// 拍照按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appbar_Camera_Click(object sender, EventArgs e)
        {
            photoChooserTask.Show();
        }

        /// <summary>
        /// 获取照片后的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                bitmap = bmp;
                ShowImage.Source = bmp;

                WriteableBitmap _wb;

                if (bitmap != null)
                {
                    _wb = new WriteableBitmap(bitmap);

                    var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                    if (myStore.FileExists("My_Note_tempimage.jpg"))
                        myStore.DeleteFile("My_Note_tempimage.jpg");
                    IsolatedStorageFileStream myFileStream = myStore.CreateFile("My_Note_tempimage.jpg");

                    // 将图片保存到独立存储的临时文件
                    _wb.SaveJpeg(myFileStream, _wb.PixelWidth, _wb.PixelHeight, 0, 95);
                    myFileStream.Close();
                    myStore.Dispose();

                    ShowImage.Visibility = System.Windows.Visibility.Visible;
                    ShowImage.Source = bitmap;
                }
            }
            //myImage.Source = bmp;

        }

        #endregion

        #region 保存成功返回主页 -NavigateBack
        /// <summary>
        /// 保存成功返回主页
        /// </summary>
        private void NavigateBack()
        {
            settings["state"] = "";
            settings["value"] = "";
            settings["title"] = "";

            using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (appStorage.FileExists("My_Note_tempimage.jpg"))
                {
                    appStorage.DeleteFile("My_Note_tempimage.jpg");
                }
            }

            NavigationService.Navigate(new Uri("/My_Note;component/MainPage.xaml", UriKind.Relative));
        } 
        #endregion

        private void NoteTitle_LostFocus_1(object sender, RoutedEventArgs e)
        {
            My_Note.Other.StaticMethod.AddOrUpdata("title", NoteTitle.Text);
        }

        private void EditTextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            My_Note.Other.StaticMethod.AddOrUpdata("value", EditTextBox.Text);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (My_Note.Other.StaticMethod.GetValueOrDefault<string>("state", "") != "")
            {
                My_Note.Other.StaticMethod.AddOrUpdata("title", NoteTitle.Text);
                My_Note.Other.StaticMethod.AddOrUpdata("value", EditTextBox.Text);
            }
            base.OnNavigatedFrom(e);
        }

    }
}