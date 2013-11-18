using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using System.IO.IsolatedStorage;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace My_Note.ToolBox
{
    public partial class RecordVoice : PhoneApplicationPage
    {
        #region 全局变量定义
        private bool IsRecord = false;

        Microphone gMicrophone = Microphone.Default;
        byte[] gAudioBuffer;
        MemoryStream gStream = new MemoryStream();
        int gSpendTime = 0;

        DispatcherTimer timer; 
        #endregion

        //构造函数
        public RecordVoice()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(33);
            timer.Tick += delegate { FrameworkDispatcher.Update(); };
            timer.Start();

            gMicrophone.BufferReady += new EventHandler<EventArgs>(gMicrophone_BufferReady);
        }

        #region 麦克风缓冲 -gMicrophone_BufferReady
        /// <summary>
        /// 麦克风缓冲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gMicrophone_BufferReady(object sender, EventArgs e)
        {
            gSpendTime += 1;
            gMicrophone.GetData(gAudioBuffer);
            gStream.Write(gAudioBuffer, 0, gAudioBuffer.Length);

            if (gSpendTime == 7)
            {
                //自动结束
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    StopRecordVoice();
                    MessageBox.Show("时间太长，自动停止录音");
                }));
            }
        } 
        #endregion

        #region 录音按钮 -RecordButton_Click
        /// <summary>
        /// 录音按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRecord)
            {
                StartRecordVoice();
            }
            else
            {
                StopRecordVoice();
            }
        } 
        #endregion

        #region 开始录音 -StartRecordVoice
        /// <summary>
        /// 开始录音
        /// </summary>
        private void StartRecordVoice()
        {
            gSpendTime = 0;
            IsRecord = true;
            RecordButton.Content = "Recording...";
            RecordButton.Background = App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
            RecordButton.Foreground = App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;

            gMicrophone.BufferDuration = TimeSpan.FromMilliseconds(1000);
            gAudioBuffer = new byte[gMicrophone.GetSampleSizeInBytes(gMicrophone.BufferDuration)];
            gMicrophone.Start();
        } 
        #endregion

        #region 停止录音 -StopRecordVoice
        /// <summary>
        /// 停止录音
        /// </summary>
        private void StopRecordVoice()
        {
            IsRecord = false;
            RecordButton.Content = "Save...";
            RecordButton.Foreground = App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
            RecordButton.Background = App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
            gSpendTime = 0;

            if (gMicrophone.State == MicrophoneState.Started)
            {
                gMicrophone.Stop();
                gStream.Close();
            }

            MemoryStream m = new MemoryStream();
            int s = gMicrophone.SampleRate / 2;
            WavHeader.WriteWavHeader(m, s);
            byte[] b = NormalizeWaveData(gStream.ToArray());
            m.Write(b, 0, b.Length);
            m.Flush();
            WavHeader.UpdateWavHeader(m);

            using (var tStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string savepath = DateTime.Now.ToString();
                savepath = savepath.Replace('/', '-');
                savepath = savepath.Replace(':', ' ');
                using (var tFStream = new IsolatedStorageFileStream(savepath + ".wav", FileMode.Create, tStore))
                {
                    byte[] tByteInStream = m.ToArray();
                    tFStream.Write(tByteInStream, 0, tByteInStream.Length);
                    tFStream.Flush();
                }
            }
            gStream = new MemoryStream();

            UpdataListBox();
            RecordButton.Content = "Record";
        } 
        #endregion

        #region PCM录音数据转换 -NormalizeWaveData
        /// <summary>
        /// PCM数据16KHz-16bit*****8kHz-8bit转换方法
        /// </summary>
        /// <param name="sourceData">原始录音数据</param>
        /// <returns></returns>
        byte[] NormalizeWaveData(byte[] sourceData)
        {
            int len = (sourceData.Length / 2 / 2);
            using (MemoryStream ms = new MemoryStream(len))
            {
                for (int i = 0; i < len; i++)
                {
                    sbyte data = (sbyte)sourceData[i * 4 + 1];
                    ms.WriteByte((byte)(data + 128));
                    ms.Flush();
                }

                return ms.ToArray();
            }
        } 
        #endregion

        #region 页面载入事件 -PhoneApplicationPage_Loaded_1
        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            UpdataListBox();
        } 
        #endregion

        #region 更新列表数据 -UpdataListBox
        /// <summary>
        /// 更新列表
        /// </summary>
        private void UpdataListBox()
        {
            List<Voice> list = new List<Voice>();

            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            string[] filelist = appStorage.GetFileNames("*.wav");

            foreach (var file in filelist)
            {
                list.Add(new Voice(file));
            }

            appStorage.Dispose();

            VoiceListBox.ItemsSource = list;
        } 
        #endregion

        #region 播放声音 -PlayVoiceButton_Click
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayVoiceButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hb = (HyperlinkButton)sender;
            string path = hb.Content.ToString();

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = new IsolatedStorageFileStream(path, FileMode.Open, myIsolatedStorage))
                {
                    SoundEffect effect = SoundEffect.FromStream(stream);
                    FrameworkDispatcher.Update();
                    effect.Play();
                }
            }
        } 
        #endregion

        #region 重写页面离开事件 -OnNavigatedFrom
        /// <summary>
        /// 重写页面离开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (IsRecord)
            {
                StopRecordVoice();
            }
            timer.Stop();
            base.OnNavigatedFrom(e);
        } 
        #endregion

        #region 通过ContextMenu删除单项录音数据 -ListContextMenuItem_Click
        /// <summary>
        /// 单项录音删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Microsoft.Phone.Controls.MenuItem menuitem = (Microsoft.Phone.Controls.MenuItem)sender;
                appStorage.DeleteFile(menuitem.Tag.ToString());
                UpdataListBox();
            }
        } 
        #endregion

    }
}