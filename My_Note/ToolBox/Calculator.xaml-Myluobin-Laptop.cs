using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using System.IO.IsolatedStorage;
using System.Globalization;
using System.Resources;

namespace My_Note.ToolBox
{
    public partial class Calculator : PhoneApplicationPage
    {
        Number FirstNumber;
        Number SecondNumber;
        private bool IsFirstNum;
        private bool IsEqualPress;

        private enum Operators { None, Add, Sub, Multi, Div, Percent }
        Operators opera = Operators.None;

        public Calculator()
        {
            InitializeComponent();
            FirstNumber = new Number();
            SecondNumber = new Number();
            IsFirstNum = true;
            IsEqualPress = false;
        }

        //所有数字按钮（含小数点）
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (IsEqualPress)
            {
                FirstNumber.NumString.Clear();
                FirstNumber.NumString.Append("0");
                FirstNumber.Num = 0;
                IsEqualPress = false;
            }

            if (IsFirstNum)
            {
                FirstNumber = JudgeInput(FirstNumber, button);
                ShowResultTextBlock.Text = FirstNumber.NumString.ToString();
                if (ShowResultTextBlock.Text.Length > 8)
                    ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                else
                    ShowResultTextBlock.FontSize = 90;
            }
            else
            {
                SecondNumber = JudgeInput(SecondNumber, button);
                ShowResultTextBlock.Text = SecondNumber.NumString.ToString();
                if (ShowResultTextBlock.Text.Length > 8)
                    ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                else
                    ShowResultTextBlock.FontSize = 90;
            }

        }

        //所有双目操作符按钮
        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstNumber.NumString.ToString() == "")
            {
                return;
            }
            IsEqualPress = false;
            IsFirstNum = false;
            if (FirstNumber.NumString.ToString().IndexOf("0", FirstNumber.NumString.ToString().Length - 1) != -1)
            {
                FirstNumber.NumString.Clear();
                FirstNumber.NumString.Append("0");
            }
            FirstNumber.Num = double.Parse(FirstNumber.NumString.ToString(), CultureInfo.InvariantCulture);

            Button button = sender as Button;

            switch (button.Content.ToString())
            {
                case "+":
                    opera = Operators.Add;
                    break;
                case "-":
                    opera = Operators.Sub;
                    break;
                case "×":
                    opera = Operators.Multi;
                    break;
                case "÷":
                    opera = Operators.Div;
                    break;
                case "%":
                    opera = Operators.Percent;
                    break;
                default:
                    opera = Operators.None;
                    break;
            }
        }

        //等号按钮
        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            string result = "0";

            if (IsFirstNum)
            {
                ShowResultTextBlock.Text = FirstNumber.NumString.ToString();
                if (ShowResultTextBlock.Text.Length > 8)
                    ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                else
                    ShowResultTextBlock.FontSize = 90;
            }
            else if (SecondNumber.NumString.ToString() == "")
            {
                //ommit
            }
            else
            {

                IsEqualPress = true;

                if (opera == Operators.None)
                {
                    //ommit
                }
                else
                {
                    SecondNumber.Num = double.Parse(SecondNumber.NumString.ToString(), CultureInfo.InvariantCulture);
                    IsFirstNum = true;

                    switch (opera)
                    {
                        case Operators.Add:
                            result = (FirstNumber.Num + SecondNumber.Num).ToString(CultureInfo.InvariantCulture);
                            break;
                        case Operators.Sub:
                            result = (FirstNumber.Num - SecondNumber.Num).ToString(CultureInfo.InvariantCulture);
                            break;
                        case Operators.Multi:
                            result = (FirstNumber.Num * SecondNumber.Num).ToString(CultureInfo.InvariantCulture);
                            break;
                        case Operators.Div:
                            if (SecondNumber.Num == 0)
                            {
                                ResourceManager rm = new ResourceManager(typeof(My_Note.Lang.AppResources));
                                MessageBox.Show(rm.GetString("CalculateDivErro"));
                                CleanButton_Click(sender, e);
                                return;
                            }
                            else
                            {
                                result = (FirstNumber.Num / SecondNumber.Num).ToString(CultureInfo.InvariantCulture);
                            }
                            break;
                        case Operators.Percent:
                            result = (FirstNumber.Num % SecondNumber.Num).ToString(CultureInfo.InvariantCulture);
                            break;
                    }
                }
            }
            ShowResultTextBlock.Text = result;
            if (result.Length > 8)
                ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
            else
                ShowResultTextBlock.FontSize = 90;

            SecondNumber.NumString.Clear();
            SecondNumber.NumString.Append("0");

            FirstNumber.NumString.Clear();
            FirstNumber.NumString.Append(ShowResultTextBlock.Text);
            SecondNumber = new Number();
            opera = Operators.None;
        }

        //清除按钮
        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            FirstNumber = new Number();
            SecondNumber = new Number();
            ShowResultTextBlock.Text = "0";
            ShowResultTextBlock.FontSize = 90;
            IsFirstNum = true;
            IsEqualPress = false;
        }

        //正负号
        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowResultTextBlock.Text == "0")
            {
            }
            else
            {
                if (IsFirstNum)
                {
                    FirstNumber.Num = double.Parse(FirstNumber.NumString.ToString(), CultureInfo.InvariantCulture);
                    FirstNumber.Num *= -1;
                    FirstNumber.NumString.Clear();
                    FirstNumber.NumString.Append(FirstNumber.Num.ToString(CultureInfo.InvariantCulture));
                    ShowResultTextBlock.Text = FirstNumber.NumString.ToString();
                    if (ShowResultTextBlock.Text.Length > 8)
                        ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                    else
                        ShowResultTextBlock.FontSize = 90;
                }
                else
                {
                    SecondNumber.Num = double.Parse(SecondNumber.NumString.ToString(), CultureInfo.InvariantCulture);
                    SecondNumber.Num *= -1;
                    SecondNumber.NumString.Clear();
                    SecondNumber.NumString.Append(SecondNumber.Num.ToString(CultureInfo.InvariantCulture));
                    ShowResultTextBlock.Text = SecondNumber.NumString.ToString();
                    if (ShowResultTextBlock.Text.Length > 8)
                        ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                    else
                        ShowResultTextBlock.FontSize = 90;
                }
            }
        }

        //回退键
        private void BackSpaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFirstNum)
            {
                if (FirstNumber.NumString.Length >= 2)
                {
                    FirstNumber.NumString.Remove(FirstNumber.NumString.Length - 1, 1);
                    ShowResultTextBlock.Text = FirstNumber.NumString.ToString();
                    if (ShowResultTextBlock.Text.Length > 8)
                        ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                    else
                        ShowResultTextBlock.FontSize = 90;
                }
                else
                {
                    FirstNumber.NumString.Clear();
                    FirstNumber.NumString.Append("0");
                    ShowResultTextBlock.Text = FirstNumber.NumString.ToString();
                    if (ShowResultTextBlock.Text.Length > 8)
                        ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                    else
                        ShowResultTextBlock.FontSize = 90;
                }

            }
            else
            {
                if (SecondNumber.NumString.Length >= 2)
                {
                    SecondNumber.NumString.Remove(SecondNumber.NumString.Length - 1, 1);
                    ShowResultTextBlock.Text = SecondNumber.NumString.ToString();
                    if (ShowResultTextBlock.Text.Length > 8)
                        ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                    else
                        ShowResultTextBlock.FontSize = 90;
                }
                else
                {
                    SecondNumber.NumString.Clear();
                    SecondNumber.NumString.Append("0");
                    ShowResultTextBlock.Text = SecondNumber.NumString.ToString();
                    if (ShowResultTextBlock.Text.Length > 8)
                        ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                    else
                        ShowResultTextBlock.FontSize = 90;
                }
            }
        }

        //M+按钮
        private void MPlusButton_Click(object sender, RoutedEventArgs e)
        {
            var setting = IsolatedStorageSettings.ApplicationSettings;
            string result = "";
            if (setting.Contains("mnumberresult"))
            {
                if (setting.TryGetValue<string>("mnumberresult", out result))
                {
                    if (result != "0")
                    {
                        ShowResultTextBlock.Text = (double.Parse(result, CultureInfo.InvariantCulture) + double.Parse(ShowResultTextBlock.Text, CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture);
                        if (ShowResultTextBlock.Text.Length > 8)
                            ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                        else
                            ShowResultTextBlock.FontSize = 90;
                    }
                    else
                    {
                        setting["mnumberresult"] = ShowResultTextBlock.Text;
                    }
                }
            }
            else
            {
                setting["mnumberresult"] = ShowResultTextBlock.Text;
            }
        }

        //MC按钮
        private void MCleanButton_Click(object sender, RoutedEventArgs e)
        {
            var setting = IsolatedStorageSettings.ApplicationSettings;
            setting["mnumberresult"] = "0";
        }

        //MR按钮
        private void MRButton_Click(object sender, RoutedEventArgs e)
        {
            var setting = IsolatedStorageSettings.ApplicationSettings;
            string result = "";
            if (setting.Contains("mnumberresult"))
            {
                if (setting.TryGetValue<string>("mnumberresult", out result))
                {
                    ShowResultTextBlock.Text = result;
                    if (ShowResultTextBlock.Text.Length > 8)
                        ShowResultTextBlock.FontSize = 90 * 8 / ShowResultTextBlock.Text.Length;
                    else
                        ShowResultTextBlock.FontSize = 90;
                }
            }
            if (IsFirstNum)
            {
                FirstNumber.NumString = new StringBuilder(result);
            }
            else
            {
                SecondNumber.NumString = new StringBuilder(result);
            }
        }

        //判断输入的合法性检查
        private Number JudgeInput(Number n, Button b)
        {
            string buttonString = b.Content.ToString();
            string numberString = n.NumString.ToString();

            if (buttonString == "0")
            {
                if (numberString == "0")
                {
                    //ommit
                }
                else
                {
                    n.NumString.Append(buttonString);
                }
            }
            else if (buttonString == ".")
            {
                if (n.IsDecimal == true)
                {
                    //ommit
                }
                else if (IsEqualPress)
                {
                    //ommit    
                }
                else if (ShowResultTextBlock.Text == "0")
                {
                    n.NumString.Append("0.");
                    n.IsDecimal = true;
                }
                else
                {
                    n.NumString.Append(buttonString);
                    n.IsDecimal = true;
                }
            }
            else
            {
                if (n.NumString.ToString() == "0")
                {
                    n.NumString.Replace("0", buttonString);
                }
                else
                {
                    n.NumString.Append(buttonString);
                }
            }

            return n;
        }

        private class Number
        {
            public StringBuilder NumString;
            public double Num { get; set; }
            public bool IsDecimal { get; set; }

            public Number()
            {
                NumString = new StringBuilder();
                Num = 0;
                IsDecimal = false;
            }
        }
    }

}