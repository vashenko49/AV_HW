using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace WebPageGrabber
{
    class WebPageGrabber
    {

        Dictionary<string, string> VisitedLinks;

        SettingsObject Settings;
        public WebPageGrabber(SettingsObject _settings)
        {
            Settings = _settings;
            VisitedLinks = new Dictionary<string, string>();
        }



        public void StartGrab()
        {
            List<string> linksInWebBage = new List<string>();
            string pathToDownlaodSubLinks = string.Empty;

            GrabAndStoreWebPage(Settings.URL, Settings.DestinationFolder, ref linksInWebBage, ref pathToDownlaodSubLinks, true);

            if (Settings.Depth > 1)
            {


                if (!(System.IO.Directory.Exists(pathToDownlaodSubLinks)))
                    System.IO.Directory.CreateDirectory(pathToDownlaodSubLinks);

                //Step 2: Recursively download links to the specified level
                RecursivelyDownloadUrl(linksInWebBage, pathToDownlaodSubLinks, Settings.Depth - 1, true);

            }

            ReenableGrabButton();

        }

        private void RecursivelyDownloadUrl(List<string> links, string pathToDownload, int depth, bool calledFromMain = false)
        {
            if (depth == 0) return;

            if (!(System.IO.Directory.Exists(pathToDownload)))
                System.IO.Directory.CreateDirectory(pathToDownload);


            List<string> subLinks = new List<string>();

            for (int i = 0; i < links.Count; i++)
            {

                string link = links[i];
                string pathtoDownloadSybLinks = string.Empty;
                GrabAndStoreWebPage(link, pathToDownload, ref subLinks, ref pathtoDownloadSybLinks);

                //if this is null means there was an error visiting the link
                if (pathtoDownloadSybLinks.Trim() != string.Empty)
                {
                    RecursivelyDownloadUrl(subLinks, pathtoDownloadSybLinks, depth - 1);
                }

            }
        }




        public void GrabAndStoreWebPage(string url, string path, ref List<string> linksInWebBage, ref string pathToDownlaodSubLinks, bool mainPage = false)
        {
            //First clean the URL
            char[] illegalEnds = new char[] { '/', '#', '?', '&' };
            while (illegalEnds.Contains(url[url.Length - 1]))
            {
                url = url.Substring(0, url.Length - 1);
            }

            //already visited. why again?
            if (VisitedLinks.ContainsKey(url))
            {
                pathToDownlaodSubLinks = VisitedLinks[url];
                return;
            }




            StringBuilder htmlContent = new StringBuilder();
            using (var client = new WebClient())
            {
                try
                {
                    htmlContent.Append(client.DownloadString(url));
                }
                catch (WebException ex)
                {
                    if (mainPage)
                    {
                        ShowMessageBox(ex.Message);
                    }

                    return;
                }
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent.ToString());

            ResolveRelativePathsForLinksInTag("//a", "href", url, ref doc, linksInWebBage);
            ResolveRelativePathsForLinksInTag("//script", "src", url, ref doc, null);
            ResolveRelativePathsForLinksInTag("//img", "src", url, ref doc, null);
            ResolveRelativePathsForLinksInTag("//link", "href", url, ref doc, null);



            string htmlFileName = string.Empty;
            var htmlnode = doc.DocumentNode.Descendants("title").SingleOrDefault();
            if (htmlnode != null)
            {
                htmlFileName = Utilities.RemoveIllegalCharactersFromFileName(htmlnode.InnerText);
            }
            if (htmlFileName.Length < 1)
            {
                int lastIndex = url.LastIndexOf('/');
                htmlFileName = Utilities.RemoveIllegalCharactersFromFileName(url.Substring(lastIndex + 1));
            }
            htmlFileName = htmlFileName.Length > 250 ? htmlFileName.Substring(0, 245) : htmlFileName;

            string pathToSavedoc = path + @"\" + htmlFileName + "_" + ((new Random()).Next(1, 99999)) + "_Grabbed.html";

            doc.Save(pathToSavedoc);

            pathToDownlaodSubLinks = path + @"\" + @htmlFileName;

            //add to visited list
            VisitedLinks.Add(url, pathToDownlaodSubLinks);


            //System.IO.File.WriteAllText(Settings.DestinationFolder + @"\" + htmlFileName, doc.HtmlDocument);
        }


        private void ResolveRelativePathsForLinksInTag(string xPath, string attribute, string url, ref HtmlAgilityPack.HtmlDocument doc, List<string> storeLinks)
        {
            var node = doc.DocumentNode;
            if (node == null) return;

            var nodes = node.SelectNodes(xPath);
            if (nodes == null) return;

            foreach (var item in nodes)
            {
                if (item.Attributes[attribute] != null && item.Attributes[attribute].Value != null)
                {
                    string link = item.Attributes[attribute].Value;

                    if (!(Utilities.IsUrlValid(link, UriKind.RelativeOrAbsolute)))
                        continue;

                    if (!Utilities.IsAbsoluteUrl(link))
                    {
                        link = Utilities.GetAbsolutePath(url, link);
                        if (Utilities.IsUrlValid(link))
                        {
                            item.Attributes[attribute].Value = link;
                        }
                    }

                    if (item.Attributes[attribute].Value.StartsWith(@"//"))
                    {
                        item.Attributes[attribute].Value = "http:" + item.Attributes[attribute].Value;
                    }
                    if (storeLinks != null)
                    {
                        storeLinks.Add(link);
                    }
                }
            }
        }

        private void ShowMessageBox(string msg)
        {

            Settings.MainFormInstance.Invoke((MethodInvoker)delegate ()
            {
                MessageBox.Show(msg);
            }
            );
        }

        private void ReenableGrabButton()
        {
            Settings.MainFormInstance.Invoke((MethodInvoker)delegate
            {
                Settings.MainFormInstance.ReEnableGrabButton();
            });
        }

    }
}
