using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebPageGrabber
{
    class Utilities
    {
        public static string GetFolderSelection()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            return string.Empty;
        }
        public static bool IsUrlValid(string url, UriKind urlKind = UriKind.Absolute)
        {
            Uri myUri;
            if (Uri.TryCreate(url, urlKind, out myUri))
            {
                if (urlKind == UriKind.RelativeOrAbsolute || urlKind == UriKind.Relative)
                {
                    return true;
                }
                if (myUri.IsWellFormedOriginalString())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// remove illegal characters from file name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string RemoveIllegalCharactersFromFileName(string filename)
        {
            if (filename.Length > 220)
            {
                filename = filename.Substring(0, 220);
            }

            filename = string.Join("_", filename.Split(System.IO.Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));

            //# % & * { } \ : < > ? / + 
            char[] moreIllegalChars = new char[] { '#', '%', '&', '*', '{', '}', '\\', ':', '<', '>', '?', '/', '+', '=' };

            filename = string.Join("_", filename.Split(moreIllegalChars, StringSplitOptions.RemoveEmptyEntries));
            return filename;
        }

        public static bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }
        public static string GetAbsolutePath(string currentURL, string relativePath)
        {
            return new System.Uri(new Uri(currentURL), relativePath).AbsoluteUri;

        }

    }
}
