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
    public class GovWeatherCurrentWeather_V2
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
        /// </summary>
        public string? Visibility { get; set; } = null;
        /// <summary>
        /// Sustained Wind Speed
        ///
        /// WindSpeed,WindGusts,UnitsWind,WindDirection
        /// In general UnitsWind is in knots
        /// WindDirection is 0 through 360
        /// </summary>
        public string? WindSustainedSpeed { get; set; } = null;
        /// <summary>
        /// Sustained Wind Speed
        /// </summary>
        public string? WindGustSpeed { get; set; } = null;
        /// <summary>
        /// In general WindUnits is in knots
        /// </summary>
        public string? WindUnits { get; set; } = null;
        /// <summary>
        /// WindDirection is 0 through 360
        /// </summary>
        public string? WindDirection { get; set; } = null;
        /// <summary>
        /// Watches, Warnings, and Advisories aka hazard-conditions
        /// </summary>
        public bool Warning { get; set; } = false;
        /// <summary>
        /// hazard headline
        /// hazard Text URL
        /// </summary>
        public List<Warnings> WarningList { get; set; } = new List<Warnings>();
        //
        /// <summary>
        /// Zero parameter constructor
        /// </summary>
        public GovWeatherCurrentWeather_V2()
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
        /// <param name="windSustainedSpeed"></param>
        /// <param name="windGustSpeed"></param>
        /// <param name="windUnits"></param>
        /// <param name="windDirection"></param>
        public GovWeatherCurrentWeather_V2(string? location, string? lastUpdated,
            string? lat, string? lon, string? elevation,
            string? icon, string? temp, string? conditions, string? humidity,
            string? windSpeed, string? barometer, string? dewpoint, string? visibility,
            string? windSustainedSpeed, string? windGustSpeed, string? windUnits,
            string? windDirection)

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
            this.WindSustainedSpeed = windSustainedSpeed;
            this.WindGustSpeed = windGustSpeed;
            this.WindUnits = windUnits;
            this.WindDirection = windDirection;
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
            _return.AppendFormat("Visibility: {0}, ", Visibility == null ? "-" : Visibility);
            _return.AppendFormat("Wind speed: {0}, ", WindSustainedSpeed == null ? "-" : WindSustainedSpeed);
            _return.AppendFormat("Wind gusts: {0}, ", WindGustSpeed == null ? "-" : WindGustSpeed);
            _return.AppendFormat("Wind units: {0}, ", WindUnits == null ? "-" : WindUnits);
            _return.AppendFormat("Wind direction: {0}, ", WindDirection == null ? "-" : WindDirection);
            _return.AppendFormat("Warning: {0}, ", Warning);
            foreach( Warnings _item in WarningList)
            {
                _return.AppendFormat(_item.ToString() );
            }
            _return.AppendFormat("]");
            return _return.ToString();
            //
        }
        // $"{_windSpeed},{_windGusts},{_unitsWind},{_windDirection}"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="windSpeed"></param>
        /// <returns></returns>
        public (string display, double windSpeed) ConvertWindSpeed()
        {
            string _windSpeedDisplay = "-unknown-";
            double _windSpeedDbl = 0.0;
            if (this.WindSpeed != null)
            {
                // $"{_windSpeed},{_windGusts},{_unitsWind},{_windDirection}"
                if (this.WindSpeed == "0,NA,knots,0")
                {
                    _windSpeedDisplay = "calm";
                }
                else
                {
                    string[] _windSpeed = this.WindSpeed.Split(",");
                    string _windUnits = "";
                    int _windSpeedInt = 0;
                    string _direction = "-";
                    if (_windSpeed.Length == 4)
                    {
                        _windUnits = _windSpeed[2];
                        _windSpeedInt = int.Parse(_windSpeed[0]);
                        int _windGustsInt = _windSpeed[1] != "NA" ? int.Parse(_windSpeed[1]) : 0;

                        if (_windSpeed[2] == "knots")
                        {
                            string _windGustsString = "";
                            _windUnits = _windSpeed[2];
                            _windSpeedDbl = (int)ConvertKnots2Mph(_windSpeed[0]);
                            _windSpeedInt = (int)_windSpeedDbl;
                            int _dir360 = int.Parse(_windSpeed[3]);
                            if (_windSpeed[1] != "NA")
                            {
                                _windGustsInt = (int)ConvertKnots2Mph(_windSpeed[1]);
                                _windGustsString = $"gusts {_windGustsInt} ";
                            }
                            // N NE E SE S SW W NW
                            _direction = ConvertDirection(_windSpeed[3]);
                            _windSpeedDisplay = $"{_direction} {_windSpeedInt} {_windGustsString}mph";
                        }
                        else
                        {
                            _windSpeedDisplay = $"{_windSpeed[0]} {_windSpeed[2]}";
                        }
                    }
                }
            }
            return (_windSpeedDisplay, _windSpeedDbl);
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir360"></param>
        /// <returns></returns>
        public string ConvertDirection(string dir360)
        {
            // N NE E SE S SW W NW
            string _direction = "-";
            try
            {
                int _dir360 = int.Parse(dir360);
                if (_dir360 > -1 && _dir360 < 361)
                {
                    if (_dir360 > 22.5 && _dir360 < 67.5)
                    {
                        _direction = "NE";
                    }
                    else if (_dir360 > 67.5 && _dir360 < 112.5)
                    {
                        _direction = "E";
                    }
                    else if (_dir360 > 112.5 && _dir360 < 157.5)
                    {
                        _direction = "SE";
                    }
                    else if (_dir360 > 157.5 && _dir360 < 202.5)
                    {
                        _direction = "S";
                    }
                    else if (_dir360 > 202.5 && _dir360 < 247.5)
                    {
                        _direction = "SW";
                    }
                    else if (_dir360 > 247.5 && _dir360 < 292.5)
                    {
                        _direction = "W";
                    }
                    else if (_dir360 > 292.5 && _dir360 < 337.5)
                    {
                        _direction = "NW";
                    }
                    else if (_dir360 > 337.5 || _dir360 < 22.5)
                    {
                        _direction = "N";
                    }
                }
                else
                {
                    _direction = $"-{dir360}-";
                }
            }
            catch
            {
                _direction = $"-{dir360}-";
            }
            return _direction;
        }
        // 
        /// <summary>
        /// Convert Visibility from "8.00,statute miles"
        /// </summary>
        /// <returns></returns>
        public string ConvertVisibility()
        {
            string _visibilityDisplay = "";
            if (!string.IsNullOrEmpty(this.Visibility))
            {
                string[] _visibility = this.Visibility.Split(",");
                if (_visibility.Length > 1)
                {
                    if (_visibility[1].ToLower().Contains("miles"))
                    {
                        _visibilityDisplay = $"{_visibility[0]} mi";
                    }
                    else
                    {
                        _visibilityDisplay = $"{_visibility[0]} {_visibility[1]}";
                    }
                }
            }
            return _visibilityDisplay;
        }
        //
        /// <summary>
        /// Convert the string LastUpdated to standard date format
        /// </summary>
        /// <returns>Month day year hour minutes string</returns>
        public string ConvertLastUpdated()
        {
            string _lastUpdatedDisplay = "-unknown-";
            if (!string.IsNullOrEmpty(this.LastUpdated))
            {
                try
                {
                    DateTime _tempLastUpdated = DateTime.Parse(this.LastUpdated);
                    _lastUpdatedDisplay = _tempLastUpdated.ToString("MMM d, yyyy hh:mm tt (K)");
                }
                catch { }
            }
            return _lastUpdatedDisplay;
        }
        //
        /// <summary>
        /// Convert temperature from fahrenheit to celsius.
        /// </summary>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public string ConvertTemperatureFromF2C(string temperature)
        {
            string _temperatureC = "-";
            try
            {
                double _temperature = double.Parse(temperature);
                // − (double)32.0 ) *((double)5 / 9);
                _temperatureC = double.Round((_temperature - 32.0) * (5.0 / 9.0)).ToString();
            }
            catch { }
            return _temperatureC;
        }
        //
        /// <summary>
        /// Calculate the wind chill based on temperature in F and wind speed in MPH
        /// </summary>
        /// <param name="temperatureF"></param>
        /// <param name="windSpeedMph"></param>
        /// <returns></returns>
        public string CalculateWindChill(double temperatureF, double windSpeedMph)
        {
            string _windChillStr = "-";
            try
            {
                double _windPow16 = Math.Pow(windSpeedMph, 0.16);
                double _windChillDbl = 35.74 + 0.6215 * temperatureF - 35.75 * _windPow16 + 0.4275 * temperatureF * _windPow16;
                _windChillStr = double.Round(_windChillDbl).ToString();
            }
            catch { }
            return _windChillStr;
        }
        //
        /// <summary>
        /// Knots are slightly more than MPH, convert the string to double and
        /// multiply it by 1.15078
        /// </summary>
        /// <param name="knots"></param>
        /// <returns>double value of windspeed in MPH</returns>
        public double ConvertKnots2Mph(string knots)
        {
            double _mph = 0.0;
            try
            {
                double _knots = double.Parse(knots);
                _mph = Double.Round((_knots) * 1.15078);
            }
            catch { }
            return _mph;
        }
    }
    public class Warnings
    {
        /// <summary>
        /// hazard headline
        /// </summary>
        public string? WarningTitle { get; set; } = null;
        /// <summary>
        /// hazard Text URL
        /// </summary>
        public string? WarningUrl { get; set; } = null;
        //
        public Warnings(string? title, string? url)
        {
            this.WarningTitle = title;
            this.WarningUrl = url;
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("WarningTitle: {0}, ", WarningTitle == null ? "-" : WarningTitle);
            _return.AppendFormat("WarningUrl: {0}]", WarningUrl == null ? "-" : WarningUrl);
            return _return.ToString();
            //
        }
    }
}
