using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My_Note
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        { }

        private static My_Note.Lang.AppResources localizedResources = new Lang.AppResources();
        public Lang.AppResources LocalizedResources { get {return localizedResources;} }
    }
}
