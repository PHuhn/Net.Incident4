// ===========================================================================
// File: helpers.cs, in the UI.ViewHelpers directory
using System;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
//
using NSG.NetIncident4.Core.UI.ViewModels;
//
namespace NSG.NetIncident4.Core.UI.ViewHelpers
{
    //
    /// <summary>
    /// Collection of static helper methods for views.
    /// </summary>
    public static partial class Helpers
    {
        //
        /// <summary>
        /// Static helper method that returns only the first n characters of
        /// a String.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string TruncateString(this string str, int maxLength)
        {
            return (str.Length > maxLength ? str.Substring(0, maxLength) + "..." : str);
        }
        //
        /// <summary>
        /// Static helper method that removes carriage returns and linefeeds
        /// characters from string.
        /// </summary>
        /// <example>In a view (like a chtml file):
        /// <code>
        /// @(item.Exception.StripCRLF())
        /// </code>
        /// </example>
        /// <param name="str">some string</param>
        /// <returns>modified string</returns>
        public static string StripCRLF(this string str)
        {
            // Return empty string if null.
            if (string.IsNullOrEmpty(str))
                return "";
            // Remove carriage returns and linefeeds.
            string _ret = str.Replace("\n", "").Replace("\r", "");
            return _ret;
        }
        //
        /// <summary>
        /// Format a string like:
        ///     Description (1-Short desc)
        /// </summary>
        /// <example>In a view (like a chtml file):
        /// <code>
        ///  @Html.DisplayFor(modelItem => Helpers.DescIdShortDesc(item.IncidentTypeDesc, item.IncidentTypeId, item.IncidentTypeShortDesc))
        /// </code>
        /// or:
        /// <code>
        ///  @Helpers.DescIdShortDesc(item.IncidentTypeDesc, item.IncidentTypeId, item.IncidentTypeShortDesc)
        /// </code>
        /// </example>
        /// <param name="desc">string full description</param>
        /// <param name="id">integer key/id</param>
        /// <param name="shortDesc">string short description</param>
        /// <returns>Formated string</returns>
        public static string DescIdShortDesc(string desc, int id, string shortDesc)
        {
            return string.Format("{0} ({1} - {2})", desc, id, shortDesc);
        }
        //
        /// <summary>
        /// Returns an array of SelectListItem, containing the 3 items that
        /// define the item as invoking a pre defined script to populate
        /// the note.
        /// </summary>
        /// <returns>Array of SelectListItem, containing the 3 script items</returns>
        public static SelectListItem[] GetClientScriptList()
        {
            return new SelectListItem[] {
                (new SelectListItem() { Text = "-none -", Value = "" }),
                (new SelectListItem() { Text = "Ping", Value = "ping" }),
                (new SelectListItem() { Text = "WhoIs", Value = "whois" }),
                (new SelectListItem() { Text = "Email ISP Report", Value = "email" })
            };
        }
        //
        /// <summary>
        /// Static helper method that get the rss feed, and assign it to the
        /// syndication feed.
        /// </summary>
        /// <remarks>
        /// https://khalidabuhakmeh.com/reading-rss-feeds-with-dotnet-core
        /// https://talkdotnet.wordpress.com/2018/02/12/reading-rss-feed-with-microsoft-syndicationfeed-readerwriter/
        /// </remarks>
        /// <returns>Mircosoft's SyndicationFeed</returns>
        /// <exception cref="Exception">possibly causing an exception</exception>
        public static SyndicationFeed GetSyndicationFeed(string rssUrlFeed)
        {
            //
            SyndicationFeed feed = new SyndicationFeed();
            try
            {
                using (var reader = XmlReader.Create(rssUrlFeed))
                {
                    feed = SyndicationFeed.Load(reader);
                }
            }
            catch (Exception _ex)
            {
                var text = $"GetSyndicationFeed: Sorry, no data is available at this time for {rssUrlFeed}.<br />{_ex.Message}<br />";
                throw new Exception(text);
            }
            //
            return feed;
        }
        //
        /// <summary>
        /// Static helper method that get feed's items and convert to News
        /// item list, including optional creator and details.
        /// </summary>
        /// <param name="feedUri">URL of the feed</param>
        /// <param name="max">maximum number of items</param>
        /// <returns>List of news items</returns>
        /// <exception cref="Exception">possibly causing an exception</exception>
        public static async Task<List<News>> GetNewsFeed(string feedUri, int max = 100)
        {
            //
            List<News> _feed = new List<News>();
            try
            {
                // https://feeds.npr.org/1001/rss.xml
                // https://www.nytimes.com/svc/collections/v1/publish/https://www.nytimes.com/section/world/rss.xml
                using (var xmlReader = XmlReader.Create(feedUri, new XmlReaderSettings() { Async = true }))
                {
                    int cnt = 0;
                    var feedReader = new RssFeedReader(xmlReader);
                    while (await feedReader.Read() && cnt < max)
                    {
                        if (feedReader.ElementType == SyndicationElementType.Item)
                        {
                            var content = await feedReader.ReadContent();
                            var _title = content.Fields.Where(f => f.Name == "title").Select(f => f.Value).FirstOrDefault();
                            if (_title != null)
                            {
                                var _link = content.Fields.Where(f => f.Name == "link").Select(f => f.Value).FirstOrDefault();
                                var _description = content.Fields.Where(f => f.Name == "description").Select(f => f.Value).FirstOrDefault();
                                var _published = content.Fields.Where(f => f.Name == "pubDate").Select(f => DateTime.Parse(f.Value)).FirstOrDefault();
                                var _creator = content.Fields.Where(f => f.Name == "creator").Select(f => f.Value).FirstOrDefault();
                                var _details = content.Fields.Where(f => f.Name == "encoded").Select(f => f.Value).FirstOrDefault();
                                _feed.Add(new News(_title, (_link != null ? _link : ""),
                                    (_description != null ? _description : ""), (_details != null ? _details : ""),
                                    (_published != null ? _published : new DateTime(1980, 1, 1)), (_creator != null ? _creator : "")));
                                cnt++;
                            }
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                var text = $"GetNewsFeed: Sorry, no data is available at this time for {feedUri}.<br />{_ex.Message}<br />";
                throw new Exception(text);
            }
            //
            return _feed;
        }
        //
    }
    //
}
// ===========================================================================
