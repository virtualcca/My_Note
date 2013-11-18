using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media;

namespace My_Note.Other
{
    class StaticMethod
    {
        /// <summary>
        /// 增添或者更新键值
        /// </summary>
        /// <param name="key">键值名</param>
        /// <param name="value">增添或者更新的值</param>
        /// <returns></returns>
        public static bool AddOrUpdata(string key, object value)
        {
            IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;

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

        /// <summary>
        /// 获得键值对的值
        /// </summary>
        /// <typeparam name="valueType">键值的数据类型</typeparam>
        /// <param name="Key">键值对的名称</param>
        /// <param name="defaultValue">如果当前键值对不存在的时候使用的默认值</param>
        /// <returns></returns>
        public static valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
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

    }
}
