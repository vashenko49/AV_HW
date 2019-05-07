using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageGrabber
{
    public enum FileExtensions
    {
        HTML,
        PHP,
        ASPX,
        ASP,
        CFM,
        JPG,
        PNG,
        JSP
    }

    class SettingsObject
    {
        public SettingsObject(MainForm _form)
        {
            MainFormInstance = _form;
        }

        public MainForm MainFormInstance
        {
            get;
            set;
        }
        public int Depth
        {
            get;
            set;
        }
        public bool ResolveRelativePaths
        {
            get;
            set;
        }
        public string DestinationFolder
        {
            get;
            set;
        }
        public string URL
        {
            get;
            set;
        }

        public bool IsValid
        {
            get;
            set;
        }
    }
}
