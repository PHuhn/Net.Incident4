using NUnit.Framework;
//
using System.ServiceModel.Syndication;
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
            // Assert.AreEqual(str, actual);
            Assert.That(actual, Is.EqualTo(str));
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
            Assert.That(actual, Is.EqualTo("123456789..."));
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
            Assert.That(actual, Is.EqualTo("12"));
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
            Assert.That(actual, Is.EqualTo("Description (1 - Desc)"));
        }
        //
        [Test]
        public void GetClientScriptList_Test()
        {
            // given
            // when
            SelectListItem[] actual = Core.UI.ViewHelpers.ViewHelpers.GetClientScriptList();
            // then
            Assert.That(actual[0].Value, Is.EqualTo(" "));
            Assert.That(actual[1].Value, Is.EqualTo("ping"));
            Assert.That(actual[2].Value, Is.EqualTo("whois"));
            Assert.That(actual[3].Value, Is.EqualTo("email"));
        }
        //
        [Test]
        public void GetSyndicationFeed_Test()
        {
            // given
            string feedUrl = @"https://raw.githubusercontent.com/PHuhn/Net.Incident4/main/NSG.NetIncident4.Core_Tests/Data/AccuWeather.xml";
            // when
            SyndicationFeed actual = Core.UI.ViewHelpers.ViewHelpers.GetSyndicationFeed(feedUrl);
            Assert.That(actual, Is.Not.Null);
            var _items = actual.Items.ToArray();
            Assert.That(_items.Length, Is.EqualTo(4));
            Assert.That(_items[0].Summary.Text.Substring(0,50), Is.EqualTo("Currently in Ann Arbor, MI: 44 °F and Showers <img"));
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
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(7));
            News news = actual[0];
            Assert.That(news.Title, Is.EqualTo("Kinder Eggs, other chocolate products recalled due to salmonella outbreak in Europe"));
            Assert.That(news.Description, Is.Not.Empty);
            Assert.That(news.Details, Is.Not.Empty);
            Assert.That(news.Creator, Is.EqualTo("Deepa Shivaram"));
            Assert.That(news.Published, Is.EqualTo(new DateTime(2022, 4, 7, 11, 51, 36)));
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
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(5));
        }
        //
    }
}