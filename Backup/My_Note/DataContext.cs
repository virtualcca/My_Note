using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace My_Note
{
    public class Note
    {
        public string DataCreate { get; private set; }
        public string FileName { get; private set; }
        public string Title { get; private set; }
        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }
        public BitmapImage NoteImage { get; private set; }
        public DateTime Data { get; private set; }

        public Note(DateTime datacreate, string filename,string title,BitmapImage noteimage,DateTime data)
        {
            DataCreate = datacreate.ToString("F",CultureInfo.CurrentUICulture);
            FileName = filename;
            Title = title;
            Data = data;

            if (noteimage != null)
            {
                NoteImage = noteimage;
                if (NoteImage.PixelWidth > NoteImage.PixelHeight)
                {
                    ImageWidth = 360;
                    ImageHeight = 270;
                }
                else
                {
                    ImageHeight = 360;
                    ImageWidth = 270;
                }
            }
            else
            {
                NoteImage = new BitmapImage(new Uri("My_Note;component/Images/TransParent.png", UriKind.Relative));
                ImageWidth = 1;
                ImageHeight = 1;
            }
        }

    }

    public class Voice
    {
        public string FileName { get; private set; }

        public Voice(string filename)
        {
            FileName = filename;
        }
    }
}
