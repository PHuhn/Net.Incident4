// ===========================================================================
using System.Xml.Linq;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class WeatherItem
    {
        public string DayName { get; set; }
        public DateTime Day { get; set; }
        public int Low { get; set; }
        public int High { get; set; }
        public string Forcast { get; set; }
        public string ImageCode { get; set; }
        public string ImageUrl { get; set; }
        public string LocalImageUrl { get; set; }
        protected string LocalImagePath = "../Images/Weather/";
        // 
        public WeatherItem()
        {
            DayName = "";
            Day = new DateTime(1980,1,1);
            Low = -400;
            High = -400;
            Forcast = "";
            ImageCode = "";
            ImageUrl = "";
            LocalImageUrl = "";
        }
        // 
        public WeatherItem(string localImagePath)
        {
            DayName = "";
            Day = new DateTime(1980, 1, 1);
            Low = -400;
            High = -400;
            Forcast = "";
            ImageCode = "";
            ImageUrl = "";
            LocalImageUrl = "";
            LocalImagePath = localImagePath;
        }
        // 
        // <yweather:forecast day="Sun" date="8 Jan 2012"
        // low="28" high="37" text="Partly Cloudy" code="30"/>
        // 
        public void YahooForcast(XElement forcastItem)
        {
            DayName = forcastItem.Attribute("day").Value;
            Day = DateTime.Parse(forcastItem.Attribute("date").Value);
            Low = System.Convert.ToInt32(forcastItem.Attribute("low").Value);
            High = System.Convert.ToInt32(forcastItem.Attribute("high").Value);
            Forcast = forcastItem.Attribute("text").Value;
            ImageCode = forcastItem.Attribute("code").Value;
            ImageUrl = "http://l.yimg.com/a/i/us/we/52/" + ImageCode + ".gif";
            LocalImageUrl = LocalImagePath + ImageCode + ".gif";
        }
    }
}
// ===========================================================================
