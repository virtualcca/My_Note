using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Windows.Media;
using System.Resources;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using System.Windows.Media.Animation;

namespace AppSetting
{
    public class AppSetting
    {
        IsolatedStorageSettings setting;

        private const string phoneTextFontSize = "PhoneTextFontSize";
        private const string phoneTextFontFamily = "PhoneTextFontFamily";
        private const string phoneThemeBackGround = "PhoneThemeBackGround";

        public AppSetting()
        {
            try
            {
                setting = IsolatedStorageSettings.ApplicationSettings;
            }
            catch
            {
                //ommit
            }
        }

        public bool AddOrUpdata(string key, object value)
        {
            bool valueChanged = false;

            if (setting.Contains(key))
            {
                if (setting[key] != value)
                {
                    setting[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                setting.Add(key, value);
            }

            return valueChanged;
        }

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

        public void Save()
        {
            setting.Save();
        }

        //手机字体大小
        public int PhoneTextSize
        {
            get
            {
                return GetValueOrDefault<int>(phoneTextFontSize, 26);
            }
            set
            {
                AddOrUpdata(phoneTextFontSize, value);
                Save();
            }
        }

        //更改手机字体列表 辅助属性
        public int PhoneTextFontFamilyIndex
        {
            get
            {
                return GetValueOrDefault<int>(phoneTextFontFamily, 0);
            }
            set
            {
                AddOrUpdata(phoneTextFontFamily, value);
                Save();
            }
        }

        //手机字体
        public FontFamily PhoneTextFontFamily
        {
            get
            {
                switch (PhoneTextFontFamilyIndex)
                {
                    case 0:
                        return new FontFamily("Segoe WP");
                    case 1:
                        return new FontFamily("Arial");
                    case 2:
                        return new FontFamily("Comic Sans MS");
                    default:
                        return new FontFamily("Segoe WP");
                }
            }
        }

        //主题背景
        public int PhoneThemeBackGroundIndex
        {
            get
            {
                return GetValueOrDefault<int>(phoneThemeBackGround, 0);
            }
            set
            {
                AddOrUpdata(phoneThemeBackGround, value);
                Save();
            }
        }



    }
}
