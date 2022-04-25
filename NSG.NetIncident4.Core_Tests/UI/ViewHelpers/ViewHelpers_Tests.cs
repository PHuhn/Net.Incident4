using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
//
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Microsoft.AspNetCore.Mvc.Rendering;
//
using Moq;
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.UI.ViewModels;
using NSG.NetIncident4.Core.UI.ViewHelpers;
//
namespace NSG.NetIncident4.Core_Tests.UI.ViewHelpers
{
    [TestFixture]
    public class ViewHelpers_Tests : UnitTestFixture
    {
        [SetUp]
        public void Setup()
        {
        }
        //
        [Test]
        public void TruncateString_NotTruncated_Test()
        {
            // given
            string str = "1234567890";
            int maxLength = 10;
            // when
            string actual = Core.UI.ViewHelpers.ViewHelpers.TruncateString(str, maxLength);
            // then
            Assert.AreEqual(str, actual);
        }
        //
        [Test]
        public void TruncateString_Truncated_Test()
        {
            // given
            string str = "1234567890";
            int maxLength = 9;
            // when
            string actual = Core.UI.ViewHelpers.ViewHelpers.TruncateString(str, maxLength);
            // then
            Assert.AreEqual("123456789...", actual);
        }
        //
        // StripCRLF(this string str)
        [Test]
        public void StripCRLF_Test()
        {
            // given
            string str = $"1\n\r2";
            // when
            string actual = Core.UI.ViewHelpers.ViewHelpers.StripCRLF(str);
            // then
            Assert.AreEqual("12", actual);
        }
        //
        [Test]
        public void DescIdShortDesc_Test()
        {
            // given
            string desc = "Description";
            int id = 1;
            string shortDesc = "Desc";
            // when
            string actual = Core.UI.ViewHelpers.ViewHelpers.DescIdShortDesc(desc, id, shortDesc);
            // then
            Assert.AreEqual("Description (1 - Desc)", actual);
        }
        //
        [Test]
        public void GetClientScriptList_Test()
        {
            // given
            // when
            SelectListItem[] actual = Core.UI.ViewHelpers.ViewHelpers.GetClientScriptList();
            // then
            Assert.AreEqual(" ", actual[0].Value);
            Assert.AreEqual("ping", actual[1].Value);
            Assert.AreEqual("whois", actual[2].Value);
            Assert.AreEqual("email", actual[3].Value);
        }
        //
        [Test]
        public void GetSyndicationFeed_Test()
        {
            // given
            string feedUrl = @"https://raw.githubusercontent.com/PHuhn/Net.Incident4/main/NSG.NetIncident4.Core_Tests/Data/AccuWeather.xml";
            // when
            SyndicationFeed actual = Core.UI.ViewHelpers.ViewHelpers.GetSyndicationFeed(feedUrl);
            Assert.IsNotNull(actual);
            var _items = actual.Items.ToArray();
            Assert.AreEqual(4, _items.Length);
            Assert.AreEqual("Currently in Ann Arbor, MI: 44 °F and Showers <img", _items[0].Summary.Text.Substring(0,50));
        }
        //
        [Test]
        public async Task GetNewsFeed_All_Test()
        {
            // given
            string feedUrl = "https://raw.githubusercontent.com/PHuhn/Net.Incident4/main/NSG.NetIncident4.Core_Tests/Data/npr.xml";
            // when
            List<News> actual = await Core.UI.ViewHelpers.ViewHelpers.GetNewsFeed(feedUrl);
            // then
            Assert.IsNotNull(actual);
            Assert.AreEqual(7, actual.Count);
            News news = actual[0];
            Assert.AreEqual("Kinder Eggs, other chocolate products recalled due to salmonella outbreak in Europe", news.Title);
            Assert.IsNotEmpty(news.Description);
            Assert.IsNotEmpty(news.Details);
            Assert.AreEqual("Deepa Shivaram", news.Creator);
            Assert.AreEqual(new DateTime(2022, 4, 7, 11, 51, 36), news.Published);
        }
        //
        [Test]
        public async Task GetNewsFeed_Only5_Test()
        {
            // given
            string feedUrl = "https://raw.githubusercontent.com/PHuhn/Net.Incident4/main/NSG.NetIncident4.Core_Tests/Data/npr.xml";
            // when
            List<News> actual = await Core.UI.ViewHelpers.ViewHelpers.GetNewsFeed(feedUrl, 5);
            // then
            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count);
        }
        //
    }
}