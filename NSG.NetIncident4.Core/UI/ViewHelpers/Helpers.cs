// ===========================================================================
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Mvc.Rendering;
//
namespace NSG.NetIncident4.Core.UI.ViewHelpers
{
    //
    /// <summary>
    /// static helper methods
    /// </summary>
    public static partial class Helpers
    {
        //
        /// <summary>
        /// Returns only the first n characters of a String.
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
        /// Remove carriage returns and linefeeds characters from string.
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
        /// <returns></returns>
        public static string DescIdShortDesc(string desc, int id, string shortDesc)
        {
            return string.Format("{0} ({1} - {2})", desc, id, shortDesc);
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// Get the rss feed, and assign it to the syndication feed.
        /// </summary>
        /// <remarks>
        /// https://khalidabuhakmeh.com/reading-rss-feeds-with-dotnet-core
        /// https://talkdotnet.wordpress.com/2018/02/12/reading-rss-feed-with-microsoft-syndicationfeed-readerwriter/
        /// </remarks>
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
                var text = $"Sorry, no news is available at this time for {rssUrlFeed}.<br />{_ex.Message}<br />";
                throw new Exception(text);
            }
            //
            return feed;
        }
    }
    //
}
// ===========================================================================
