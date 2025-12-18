//
using System.Text;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class GovWeather7DayForecast
    {
        public string Date { get; set; } = String.Empty;
        public string? TimeOfDay { get; set; } = String.Empty;
        public string? LowTemp { get; set; } = null;
        public string? HighTemp { get; set; } = null;
        public string? Precipitation { get; set; } = null;
        public string? WeatherSummary { get; set; } = null; // cloudy
        public string? WeatherIcon { get; set; } = null;  // url to icon
        public string? WeatherDetails { get; set; } = String.Empty; // detail summary
        /// <summary>
        /// 3 parameter constructor
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="summary"></param>
        public GovWeather7DayForecast(string date, string? timeOfDay, string? details)
        {
            this.Date = date;
            this.TimeOfDay = timeOfDay;
            this.WeatherDetails = details;
        }
        /// <summary>
        /// 7 parameter constructor
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="precipitation"></param>
        /// <param name="conditions"></param>
        /// <param name="icon"></param>
        /// <param name="summary"></param>
        public GovWeather7DayForecast(string date, string? timeOfDay, string? low, string? high, string? precipitation, string? conditions, string? icon, string? details)
        {
            this.Date = date;
            this.TimeOfDay = timeOfDay;
            this.LowTemp = low;
            this.HighTemp = high;
            this.Precipitation = precipitation;
            this.WeatherSummary = conditions; // cloudy
            this.WeatherIcon = icon;  // url to icon
            this.WeatherDetails = details;
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("Date: {0}, ", Date);
            _return.AppendFormat("TimeOfDay: {0}, ", TimeOfDay);
            if ( LowTemp != null ) _return.AppendFormat("Low: {0}, ", LowTemp);
            if ( HighTemp != null ) _return.AppendFormat("High: {0}, ", HighTemp);
            if ( Precipitation != null) _return.AppendFormat("Precipitation: {0}, ", Precipitation);
            if (WeatherSummary != null) _return.AppendFormat("Conditions: {0}, ", WeatherSummary);
            if (WeatherIcon != null) _return.AppendFormat("Icon: {0}, ", WeatherIcon);
            _return.AppendFormat("Details: {0}]", WeatherDetails);
            return _return.ToString();
            //
        }
    }
}
