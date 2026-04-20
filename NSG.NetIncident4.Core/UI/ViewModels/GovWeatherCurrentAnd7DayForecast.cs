using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NSG.NetIncident4.Core.UI.ViewModels
{
    //
    /// <summary>
    /// View for GovWeather.chtml
    /// </summary>
    public class GovWeatherCurrentAnd7DayForecast
    {
        // Current weather
        /// <summary>
        /// The details of the current weather at a specific zip code
        /// </summary>
        public GovWeatherCurrentWeather_V2? Current { get; set; } = null;
        // Weather forecast
        /// <summary>
        /// Location of the weather forecast
        /// </summary>
        public string? Location { get; set; } = null;
        /// <summary>
        /// Date time that the class is created
        /// </summary>
        public List<GovWeather7DayForecast>? Forecast { get; set; } = null;
        //
        /// <summary>
        /// Zero parameter constructor
        /// </summary>
        public GovWeatherCurrentAnd7DayForecast()
        {
            this.Current = new GovWeatherCurrentWeather_V2();
            this.Location = "";
            this.Forecast = new List<GovWeather7DayForecast>();
        }
        //
        /// <summary>
        /// View for GovWeather.chtml
        /// Current weather and 7 day forecast
        /// </summary>
        /// <param name="current"></param>
        /// <param name="location"></param>
        /// <param name="forecast"></param>
        public GovWeatherCurrentAnd7DayForecast(GovWeatherCurrentWeather_V2? current, string? location, List<GovWeather7DayForecast>? forecast)
        {
            this.Current = current;
            this.Location = location;
            this.Forecast = forecast;
        }
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("Location: {0}, ", Location);
            _return.AppendFormat("{0} ,", Current != null ? Current.ToString(): "-");
            if (Forecast != null)
            {
                foreach (var _item in Forecast)
                {
                    _return.AppendLine(_item.ToString());
                }
            }
            return _return.ToString();
            //
        }


    }
}
