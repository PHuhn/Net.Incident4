//
using System.Text;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    //
    /// <summary>
    /// Overcast, 27°F, -3°C
    /// Humidity    63%
    /// Wind Speed  SE 3 mph
    /// Barometer   30.22 in (1024.8 mb)
    /// Dewpoint	16°F(-9°C)
    /// Visibility	10.00 mi
    /// Last update	1 Dec 5:53 pm EST
    /// </summary>
    public class GovWeatherCurrentWeather
    {
        /// <summary>
        /// Date time that the class is created
        /// </summary>
        public string Date { get; set; } = string.Empty;
        /// <summary>
        /// Location of the current weather conditions
        /// </summary>
        public string? Location { get; set; } = null;
        /// <summary>
        /// Latitude of the location
        /// </summary>
        public string? Lat { get; set; } = null;
        /// <summary>
        /// Longitude of the location
        /// </summary>
        public string? Lon { get; set; } = null;
        /// <summary>
        /// Elevation at the location
        /// </summary>
        public string? Elevation { get; set; } = null;
        /// <summary>
        /// Current weather conditions as of date time
        /// </summary>
        public string? LastUpdated { get; set; } = null;
        /// <summary>
        /// URL of an image icon for the current weather conditions
        /// </summary>
        public string? WeatherIcon { get; set; } = null;  // url to icon
        /// <summary>
        /// Current temperature
        /// </summary>
        public string? CurrentTemp { get; set; } = null;
        /// <summary>
        /// Short description of weather
        /// </summary>
        public string? WeatherSummary { get; set; } = null; // cloudy
        /// <summary>
        /// Humidity is assumed to be percentage (%)
        /// </summary>
        public string? Humidity { get; set; } = null;
        /// <summary>
        /// Wind speed contains:
        /// wind speed,gusts speed,units,direction
        /// </summary>
        public string? WindSpeed { get; set; } = null;
        /// <summary>
        /// Barometer in inches
        /// </summary>
        public string? Barometer { get; set; } = null;
        /// <summary>
        /// Dewpoint in F
        /// </summary>
        public string? Dewpoint { get; set; } = null;
        /// <summary>
        /// Visibility
        /// WindSpeed,WindGusts,UnitsWind,WindDirection
        /// In general UnitsWind is in knots
        /// WindDirection is 0 through 360
        /// </summary>
        public string? Visibility { get; set; } = null;
        //
        /// <summary>
        /// Zero parameter constructor
        /// </summary>
        public GovWeatherCurrentWeather()
        {
            this.Date = DateTime.Now.ToString();
        }
        //
        /// <summary>
        /// Current weather
        /// </summary>
        /// <param name="location"></param>
        /// <param name="lastUpdated"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="elevation"></param>
        /// <param name="icon"></param>
        /// <param name="temp"></param>
        /// <param name="conditions"></param>
        /// <param name="humidity"></param>
        /// <param name="windSpeed"></param>
        /// <param name="barometer"></param>
        /// <param name="dewpoint"></param>
        /// <param name="visibility"></param>
        public GovWeatherCurrentWeather(string? location, string? lastUpdated,
            string? lat, string? lon, string? elevation,
            string? icon, string? temp, string? conditions, string? humidity,
            string? windSpeed, string? barometer, string? dewpoint, string? visibility)
        {
            this.Date = DateTime.Now.ToString();
            this.Lat = lat;
            this.Lon = lon;
            this.Elevation = elevation;
            this.Location = location;
            this.LastUpdated = lastUpdated;
            this.WeatherIcon = icon;  // url to icon
            this.CurrentTemp = temp;
            this.WeatherSummary = conditions; // cloudy
            this.Humidity = humidity;
            this.WindSpeed = windSpeed;
            this.Barometer = barometer;
            this.Dewpoint = dewpoint;
            this.Visibility = visibility;
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
            _return.AppendFormat("Location: {0}, ", Location == null ? "-" : Location);
            _return.AppendFormat("Latitude: {0}, ", Lat == null ? "-" : Lat);
            _return.AppendFormat("Longitude: {0}, ", Lon == null ? "-" : Lon);
            _return.AppendFormat("Elevation: {0}, ", Elevation == null ? "-" : Elevation);
            _return.AppendFormat("LastUpdated: {0}, ", LastUpdated == null ? "-" : LastUpdated);
            _return.AppendFormat("Icon: {0}, ", WeatherIcon == null? "-" : WeatherIcon);
            _return.AppendFormat("Temperature: {0}, ", CurrentTemp == null ? "-" : CurrentTemp);
            _return.AppendFormat("Conditions: {0}, ", WeatherSummary == null ? "-" : WeatherSummary);
            _return.AppendFormat("Humidity: {0}, ", Humidity == null ? "-" : Humidity);
            _return.AppendFormat("Wind speed: {0}, ", WindSpeed == null ? "-" : WindSpeed);
            _return.AppendFormat("Barometer: {0}, ", Barometer == null ? "-" : Barometer);
            _return.AppendFormat("Dewpoint: {0}, ", Dewpoint == null ? "-" : Dewpoint);
            _return.AppendFormat("Visibility: {0}]", Visibility == null ? "-" : Visibility);
            return _return.ToString();
            //
        }
    }
}
