using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace LSScraper.LSapi
{
    public static class Api
    {
        public static IEnumerable<Post> ScrapeLS()
        {
            string html = GetHtml();
            return GetPosts(html);
        }

        private static string GetHtml()
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "amazon is trash");
                var url = $"https://www.thelakewoodscoop.com/";
                var html = client.GetStringAsync(url).Result;
                return html;
            }
        }

        private static IEnumerable<Post> GetPosts(string html)
        {
            var parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            var itemDivs = document.QuerySelectorAll(".post");
            List<Post> items = new List<Post>();
            foreach (var div in itemDivs)
            {
                Post post = new Post();
                var atag = div.QuerySelectorAll("a").First();
                post.Title = atag.TextContent;
                post.Url = atag.Attributes["href"].Value;

                var image = div.QuerySelector("img.aligncenter");
                if (image != null)
                {
                    post.ImageUrl = image.Attributes["src"].Value;
                }

                var date = div.QuerySelectorAll("div.postmetadata-top small").First();
                if (date != null && date.TextContent.Trim() != "")
                {
                    post.Date = DateTime.Parse(date.TextContent.Trim());
                }

                items.Add(post);
            }

            return items;
        }

    }
}
