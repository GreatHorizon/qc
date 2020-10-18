using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net;
using System.Text;

namespace BrokenLinks
{
    class BrokenLinksChecker
    {
        public BrokenLinksChecker(string link)
        {
            m_mainLink = link;  
        }
        private static bool IsValidStatusCode(HttpResponseMessage res)
        {
            int statusCode = (int)res.StatusCode;
            if (statusCode < 400 && statusCode > 199)
            {
                return true;
            }

            return false;
        }

        private static void GetLinks(string currentLink)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(currentLink);

            HtmlNode[] nodes = document.DocumentNode.SelectNodes("//a").ToArray();

            foreach (HtmlNode item in nodes)
            {
                string url = item.GetAttributeValue("href", null);
                if (!m_links.Contains(url) && Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                {
                    m_links.Add(url);
                    TryToGetLinks(m_mainLink + url);
                }
            }
        }

        public List<string> GetValidLinks()
        {
            return m_validLinks;
        }

        public List<string> GetInvalidLinks()
        {
            return m_invalidLinks;
        }

        private static void TryToGetLinks(string currentLinks)
        {
            try
            {
                GetLinks(currentLinks);
            }
            catch (System.ArgumentNullException)
            {
                return;
            }
        }

        public void CheckAllLinks()
        {
            TryToGetLinks(m_mainLink);

            string url;
            string message;
            foreach (var link in m_links)
            {
                url = link;
                if (!link.StartsWith("http://") && !link.StartsWith("https://") && !link.StartsWith("ftp://"))
                {
                    url = m_mainLink + link;
                }

                using (var response = client.GetAsync(url).Result)
                {
                    int code = (int)response.StatusCode;
                    message = $"{url} {code.ToString()} {response.StatusCode}";
                    if (IsValidStatusCode(response))
                    {
                        m_validLinks.Add(message);
                    }
                    else
                    {
                        m_invalidLinks.Add(message);
                    }

                }
            }
        }

        private static List<string> m_validLinks = new List<string>();
        private static List<string> m_invalidLinks = new List<string>();
        private static HttpClient client = new HttpClient();
        private static string m_mainLink;
        private static List<string> m_links = new List<string>();
    }
    class Program
    {
        private static string CheckArgs(string[] args)
        {
            if (args.Length != ARGS_COUNT)
            {
                throw new Exception("Invalid arguments count");
            }

            return args[0];
        }
        static void Main(string[] args)
        {
            try
            {
                string link = CheckArgs(args);
                BrokenLinksChecker checker = new BrokenLinksChecker(link);
                checker.CheckAllLinks();

                PutLinksIntoFile(m_validOut, checker.GetValidLinks());
                PutLinksIntoFile(m_invalidOut, checker.GetInvalidLinks());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            m_validOut.Close();
            m_invalidOut.Close();
        }

        private static void PutLinksIntoFile(StreamWriter stream, List<string> links)
        {
            foreach (var link in links)
            {
                stream.WriteLine(link);
            }

            stream.WriteLine("Links count " + links.Count());
            stream.WriteLine("Date " + DateTime.UtcNow);
        }

        private static int ARGS_COUNT = 1;
        private static StreamWriter m_validOut = new StreamWriter("../../../valid.txt", false, Encoding.UTF8);
        private static StreamWriter m_invalidOut = new StreamWriter("../../../invalid.txt", false, Encoding.UTF8);
    }
}
