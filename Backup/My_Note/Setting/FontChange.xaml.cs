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
    public partial class FontChange : PhoneApplicationPage
    {
        private const string phoneTextFontSize = "PhoneTextFontSize";
        private const string phoneTextFontFamily = "PhoneTextFontFamily";

        public FontChange()
        {
            InitializeComponent();
            
            FontFamilyListBox.SelectedIndex = My_Note.Other.StaticMethod.GetValueOrDefault<int>(phoneTextFontFamily, 0);
            TextFontSizeSlider.Value = My_Note.Other.StaticMethod.GetValueOrDefault<int>(phoneTextFontSize, 24);
            //FontFamilyListBox.SelectedIndex = 
        }

        private void TextFontSizeSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ShowFontSize != null)
            {
                ShowFontSize.FontSize = e.NewValue;
            }
        }

        private void FontFamilyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShowFontSize != null)
            {
                switch (FontFamilyListBox.SelectedIndex)
                {
                    case 0:
                        ShowFontSize.FontFamily = new System.Windows.Media.FontFamily("Segoe WP");
                        break;
                    case 1:
                        ShowFontSize.FontFamily = new System.Windows.Media.FontFamily("Arial");
                        break;
                    case 2:
                        ShowFontSize.FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS");
                        break;
                    default:
                        ShowFontSize.FontFamily = new System.Windows.Media.FontFamily("Segoe WP");
                        break;
                }
                My_Note.Other.StaticMethod.AddOrUpdata(phoneTextFontFamily, FontFamilyListBox.SelectedIndex);
            }
        }


    }
}