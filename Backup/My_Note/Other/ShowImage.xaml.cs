using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Windows.Input;

namespace My_Note.Other
{
    public partial class ShowImage : PhoneApplicationPage
    {
        string filename;
        PageOrientation lastOrientation;
        double InitialSize;
        double InitialSizeX;
        double InitialSizeY;

        public ShowImage()
        {
            InitializeComponent();

            this.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(MainPage_OrientationChanged);
            lastOrientation = this.Orientation;
            this.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(MainPage_ManipulationDelta);
        }


        #region 平移图片 -MainPage_MainpulationDelta
        /// <summary>
        /// 页面设备更改期间时间 此处用于平移图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            //if (e.DeltaManipulation.Scale.X > 0 && e.DeltaManipulation.Scale.Y > 0)
            //{
            //    this.imgScale.ScaleX *= e.DeltaManipulation.Scale.X;
            //    this.imgScale.ScaleY *= e.DeltaManipulation.Scale.Y;
            //}

            this.imgTrans.X += e.DeltaManipulation.Translation.X;
            this.imgTrans.Y += e.DeltaManipulation.Translation.Y;
        } 
        #endregion

        #region 多点触控缩放 -Touch_FrameReported
        /// <summary>
        /// 触摸页面报告 用于多点触控缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {

            if (e.GetTouchPoints(this.ShowImageControler).Count > 1)
            {
                if (e.GetTouchPoints(this.ShowImageControler)[0].Action == TouchAction.Down || e.GetTouchPoints(this.ShowImageControler)[1].Action == TouchAction.Down)
                {
                    InitialSize = CalLenth(e.GetTouchPoints(this.ShowImageControler)[0].Position, e.GetTouchPoints(this.ShowImageControler)[1].Position);
                    InitialSizeX = imgScale.ScaleX;
                    InitialSizeY = imgScale.ScaleY;
                }
                if (InitialSize > 0)
                {
                    imgScale.ScaleX = InitialSizeX * CalLenth(e.GetTouchPoints(this.ShowImageControler)[0].Position, e.GetTouchPoints(this.ShowImageControler)[1].Position) / InitialSize;
                    imgScale.ScaleY = InitialSizeY * CalLenth(e.GetTouchPoints(this.ShowImageControler)[0].Position, e.GetTouchPoints(this.ShowImageControler)[1].Position) / InitialSize;

                }

            }
        } 
        #endregion

        #region 计算两触摸点之间的距离 -CalLenth
        /// <summary>
        /// 计算两触摸点之间的距离
        /// </summary>
        /// <param name="p1">触摸坐标点1</param>
        /// <param name="p2">触摸坐标点2</param>
        /// <returns></returns>
        private double CalLenth(Point p1, Point p2)
        {
            double x = System.Math.Abs(p1.X - p2.X);
            double y = System.Math.Abs(p1.Y - p2.Y);
            return System.Math.Sqrt(x * x + y * y);
        }
        #endregion

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            filename = NavigationContext.QueryString["id"];
            System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();

            IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication();
            using (IsolatedStorageFileStream filestream = appStore.OpenFile(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                bitmap.SetSource(filestream);
            }
            ShowImageControler.Source = bitmap;
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/My_Note;component/ViewEdit.xaml?id=" +  filename.Substring(0,filename.Length - 4),UriKind.Relative));
            base.OnBackKeyPress(e);
        }

        #region 响应重力更改 -MainPage_OrientationChanged
        /// <summary>
        /// 页面重力更改处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            PageOrientation newOrientation = e.Orientation;

            RotateTransition transitionElement = new RotateTransition();

            switch (newOrientation)
            {
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeRight:
                    if (lastOrientation == PageOrientation.PortraitUp)
                        transitionElement.Mode = RotateTransitionMode.In90Counterclockwise;
                    else
                        transitionElement.Mode = RotateTransitionMode.In180Clockwise;
                    break;
                case PageOrientation.LandscapeLeft:
                    if (lastOrientation == PageOrientation.LandscapeRight)
                        transitionElement.Mode = RotateTransitionMode.In180Counterclockwise;
                    else
                        transitionElement.Mode = RotateTransitionMode.In90Clockwise;
                    break;
                case PageOrientation.Portrait:
                case PageOrientation.PortraitUp:
                    if (lastOrientation == PageOrientation.LandscapeLeft)
                        transitionElement.Mode = RotateTransitionMode.In90Counterclockwise;
                    else
                        transitionElement.Mode = RotateTransitionMode.In90Clockwise;
                    break;
                default:
                    break;
            }

            PhoneApplicationPage phoneApplicationPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;
            ITransition transition = transitionElement.GetTransition(phoneApplicationPage);
            transition.Completed += delegate
            {
                transition.Stop();
            };
            transition.Begin();

            lastOrientation = newOrientation;

        } 
        #endregion

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Touch.FrameReported -= new TouchFrameEventHandler(Touch_FrameReported);
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            GC.SuppressFinalize(this);
            base.OnNavigatedTo(e);
        }
    }
}