﻿#pragma checksum "G:\Project\My_Note\My_Note\Setting\Setting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7465C10C41BFC6EF616C16ECB93DBD86"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18033
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace My_Note.Setting {
    
    
    public partial class Setting : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.HyperlinkButton FontChange;
        
        internal System.Windows.Controls.HyperlinkButton ThemeChange;
        
        internal System.Windows.Controls.HyperlinkButton CacheClear;
        
        internal System.Windows.Controls.HyperlinkButton HelpPage;
        
        internal System.Windows.Controls.HyperlinkButton AboutPage;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/My_Note;component/Setting/Setting.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.FontChange = ((System.Windows.Controls.HyperlinkButton)(this.FindName("FontChange")));
            this.ThemeChange = ((System.Windows.Controls.HyperlinkButton)(this.FindName("ThemeChange")));
            this.CacheClear = ((System.Windows.Controls.HyperlinkButton)(this.FindName("CacheClear")));
            this.HelpPage = ((System.Windows.Controls.HyperlinkButton)(this.FindName("HelpPage")));
            this.AboutPage = ((System.Windows.Controls.HyperlinkButton)(this.FindName("AboutPage")));
        }
    }
}

