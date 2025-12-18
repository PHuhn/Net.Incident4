//
using System.Xml.XPath;
using System.Xml.Linq;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class GovWeather
    {
        //
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns>string array</returns>
        public async Task<String[]> ReadDataWithUrlToArrayAsync(string urlString)
        {
            String[] data = [];
            var http = new HttpClient();
            // Supply the same header as chrome
            // <?xml version="1.0" encoding="ISO-8859-1"?>
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36");
            var response = await http.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                data = String.Join("", result).Split('\n');
            }
            return data;
        }
        //
        /// <summary>
        /// Get the latitude and longitude data.
        /// <example>
        /// Call of:
        /// https://graphical.weather.gov/xml/sample_products/browser_interface/ndfdXMLclient.php?listZipCodeList=48104
        /// returns the following:
        ///  <dwml xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" version="1.0" xsi:noNamespaceSchemaLocation="http://localhost/xml/schema/DWML.xsd">
        ///	  <latLonList>42.2754,-83.7308</latLonList>
        ///  </dwml>
        /// </example>
        /// </summary>
        /// <param name="xmlString">To is the actual xml data as a single string</param>
        /// <returns>tuple of latitude and longitude</returns>
        /// <exception cref="InvalidDataException"></exception>
        public (string lat, string lon) GovLatLonData(string xmlString)
        {
            // xpath:
            // https://www.w3schools.com/xml/xpath_syntax.asp
            String[] latlon = [];
            if (!String.IsNullOrEmpty(xmlString))
            {
                try
                {
                    XElement latlonRoot = XElement.Parse(xmlString);
                    XElement latlonElem = latlonRoot.XPathSelectElement("//latLonList");
                    if (latlonElem != null)
                    {
                        latlon = latlonElem.Value.Split(',');
                        if (latlon.Length > 1)
                        {
                            return (latlon[0], latlon[1]);
                        }
                        else
                        {
                            throw new InvalidDataException("GovLatLonData: Invalid Lat and Lon data.");
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("GovLatLonData: Lat and Lon not found.");
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine(_ex.Message);
                    throw;
                }
            }
            else
            {
                throw new InvalidDataException("GovLatLonData: Invalid parameter data.");
            }
            return ("", "");
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GovWeatherCurrentWeather GovWeatherCurrentWeatherData(string xmlString)
        {
            var _ret = new GovWeatherCurrentWeather("", "", "", "", "", "", "", "", "", "", "", "", "");
            if (!String.IsNullOrEmpty(xmlString))
            {
                try
                {
                    XElement root = XElement.Parse(xmlString);
                    XElement? _current = root.XPathSelectElement("/data[@type='current observations']");
                    if (_current == null)
                        throw new Exception("GovWeatherCurrentWeatherData: ");
                    // <location>
                    // <area-description>Ann Arbor, Ann Arbor Municipal Airport, MI</area-description>
                    _ret.Location = _current.XPathSelectElement("//location[area-description]").Element("area-description").Value;
                    // <data type=""current observations"">
                    //	<location>
                    //		<location-key>point1</location-key>
                    //		<point latitude=""42.22"" longitude=""-83.74""/>
                    //		<area-description>Ann Arbor, Ann Arbor Municipal Airport, MI</area-description>
                    //		<height datum=""mean sea level"" height-units=""feet"">837</height>
                    //	</location>
                    _ret.Lat = _current.XPathSelectElement("//location//point").Attribute("latitude").Value;
                    _ret.Lon = _current.XPathSelectElement("//location//point").Attribute("longitude").Value;
                    _ret.Elevation = _current.XPathSelectElement("/data[@type='current observations']//location").Element("height").Value;
                    // <time-layout time-coordinate="local">
                    // <start-valid-time period-name="current">2025-11-22T16:53:00-05:00</start-valid-time>
                    _ret.LastUpdated = _current.XPathSelectElement("/data[@type='current observations']//time-layout[@time-coordinate='local']").Element("start-valid-time").Value;
                    //	<conditions-icon type="forecast-NWS" time-layout="k-p1h-n1-1">
                    //		<name>Conditions Icon</name>
                    //		<icon-link>https://forecast.weather.gov/newimages/medium/sct.png</icon-link>
                    //	</conditions-icon>
                    _ret.WeatherIcon = _current.XPathSelectElement("/data[@type='current observations']//conditions-icon[@type='forecast-NWS']").Element("icon-link").Value;
                    //	<parameters applicable-location="point1">
                    //		<temperature type="apparent" units="Fahrenheit" time-layout="k-p1h-n1-1">
                    //			<value>40</value>
                    //		</temperature>
                    _ret.CurrentTemp = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//temperature[@type='apparent']").Value;
                    //	<parameters applicable-location="point1">
                    //	    <weather time-layout="k-p1h-n1-1">
                    //		    <weather-conditions weather-summary="Fair"/>
                    _ret.WeatherSummary = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//weather//weather-conditions").Attribute("weather-summary").Value;
                    //	<parameters applicable-location="point1">
                    //		<humidity type="relative" time-layout="k-p1h-n1-1">
                    //			<value>55</value>
                    //		</humidity>
                    _ret.Humidity = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//humidity[@type='relative']").Value;
                    //	<parameters applicable-location="point1">
                    //	    <wind-speed type="sustained" units="knots" time-layout="k-p1h-n1-1">
                    //		    <value>5</value>
                    //	    </wind-speed>
                    string _windSpeed = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//wind-speed[@type='sustained']").Value;
                    string _unitsWind = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//wind-speed[@type='sustained']").Attribute("units").Value;
                    //	<parameters applicable-location="point1">
                    //	    <wind-speed type="gust" units="knots" time-layout="k-p1h-n1-1">
                    //		    <value>NA</value>
                    //	    </wind-speed>
                    string _windGusts = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//wind-speed[@type='gust']").Value;
                    //	<parameters applicable-location="point1">
                    //	    <direction type="wind" units="degrees true" time-layout="k-p1h-n1-1">
                    //		    <value>250</value>
                    //	    </direction>
                    string _windDirection = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//direction[@type='wind']").Value;
                    _ret.WindSpeed = $"{_windSpeed},{_windGusts},{_unitsWind},{_windDirection}";
                    //	<parameters applicable-location="point1">
                    //	    <pressure type="barometer" units="inches of mercury" time-layout="k-p1h-n1-1">
                    //		    <value>30.03</value>
                    //	    </pressure>
                    _ret.Barometer = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//pressure[@type='barometer']").Value;
                    //	<parameters applicable-location="point1">
                    //		<temperature type="dew point" units="Fahrenheit" time-layout="k-p1h-n1-1">
                    //			<value>25</value>
                    //		</temperature>
                    _ret.Dewpoint = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//temperature[@type='dew point']").Value;
                    //	<parameters applicable-location="point1">
                    //	    <weather time-layout="k-p1h-n1-1">
                    //		    <weather-conditions weather-summary="Fair"/>
                    //		    <weather-conditions>
                    //			    <value>
                    //				    <visibility units="statute miles">10.00</visibility>
                    //	    		</value>
                    //		    </weather-conditions>
                    //	    </weather>
                    string _visibility = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//weather//weather-conditions//value//visibility").Value;
                    string _unitsVis = _current.XPathSelectElement("/data[@type='current observations']//parameters[@applicable-location='point1']//weather//weather-conditions//value//visibility").Attribute("units").Value;
                    _ret.Visibility = $"{_visibility},{_unitsVis}";
                    Console.WriteLine(_ret.ToString());
                }
                catch (Exception _ex)
                {
                    var _msg = $"{_ex.Message}\nGovWeatherCurrentWeatherData:\n{_ret.ToString()}";
                    Console.WriteLine(_msg);
                    throw new Exception(_msg);
                }
            }
            else
            {
                throw new Exception("GovWeatherCurrentWeatherData: Empty xml string passed.");
            }
            return _ret;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns>tuple of location and GovWeatherForecast list</returns>
        public GovWeatherCurrentAnd7DayForecast GovWeatherForecastData(string xmlData)
        {
            // xpath:
            // https://www.w3schools.com/xml/xpath_syntax.asp
            //
            List<GovWeather7DayForecast> forecastItems = new List<GovWeather7DayForecast>();
            string forecastLocation = "- unknown -";
            //
            if (!String.IsNullOrEmpty(xmlData))
            {
                try
                {
                    XElement root = XElement.Parse(xmlData);
                    XElement? forecast = root.XPathSelectElement("/data[@type='forecast']");
                    if (forecast != null)
                    {
                        XElement? location = forecast.XPathSelectElement("//location[description]");
                        if (location != null)
                        {
                            forecastLocation = location.Element("description").Value;
                        }
                        // <start-valid-time period-name="Tonight">2025-11-22T18:00:00-05:00</start-valid-time>
                        IEnumerable<XElement> _period14 = forecast.XPathSelectElements("//time-layout//start-valid-time");
                        if (_period14 == null)
                            throw new Exception("GovWeatherForecastData: //time-layout//start-valid-time not found");
                        IEnumerator<XElement> _periodEnumerator14 = _period14.GetEnumerator();
                        if (_periodEnumerator14 == null)
                            throw new Exception("GovWeatherForecastData: periodEnumerator is null");
                        IEnumerable<XElement> _wordedText14 = forecast.XPathSelectElements("//wordedForecast//text");
                        // <temperature type="minimum" units="Fahrenheit" time-layout="k-p24h-n7-1">
                        // <value>34</value>
                        IEnumerable<XElement> _lowTempEnumerable7 = forecast.XPathSelectElements("//temperature[@type='minimum']//value");
                        if (_lowTempEnumerable7 == null)
                            throw new Exception(": xpath low temperature values not found.");
                        IEnumerator<XElement> _lowTempEnumerator7 = _lowTempEnumerable7.GetEnumerator();
                        // <temperature type="maximum" units="Fahrenheit" time-layout="k-p24h-n7-2">
                        // <value>50</value>
                        IEnumerable<XElement> _highTempEnumerable7 = forecast.XPathSelectElements("//temperature[@type='maximum']//value");
                        if (_highTempEnumerable7 == null)
                            throw new Exception(": xpath high temperature values not found.");
                        IEnumerator<XElement> _highTempEnumerator7 = _highTempEnumerable7.GetEnumerator();
                        // <probability-of-precipitation type="12 hour" units="percent" time-layout="k-p12h-n14-1">
                        // <value xsi:nil="true"/>
                        // <value>70</value>
                        IEnumerable<XElement> _precipitationEnumerable14 = forecast.XPathSelectElements("//probability-of-precipitation//value");
                        if (_precipitationEnumerable14 == null)
                            throw new Exception(": xpath precipitation values not found.");
                        IEnumerator<XElement> _precipitationEnumerator14 = _precipitationEnumerable14.GetEnumerator();
                        // <weather time-layout="k-p12h-n14-1">
                        // <weather-conditions weather-summary="Mostly Cloudy"/>
                        IEnumerable<XElement> _conditionsEnumerable14 = forecast.XPathSelectElements("//weather//weather-conditions");
                        if (_conditionsEnumerable14 == null)
                            throw new Exception(": xpath weather=conditions values not found.");
                        IEnumerator<XElement> _conditionsEnumerator14 = _conditionsEnumerable14.GetEnumerator();
                        // <conditions-icon type="forecast-NWS" time-layout="k-p12h-n14-1">
                        // <icon-link>https://forecast.weather.gov/newimages/medium/nbkn.png</icon-link>
                        IEnumerable<XElement> _iconsEnumerable14 = forecast.XPathSelectElements("//conditions-icon//icon-link");
                        if (_iconsEnumerable14 == null)
                            throw new Exception(": xpath weather=icons values not found.");
                        IEnumerator<XElement> _iconsEnumerator14 = _iconsEnumerable14.GetEnumerator();
                        //
                        int _day = 0;
                        int _cnt = 0;
                        string _lowTemp = "";
                        string _highTemp = "";
                        //
                        foreach (var _txt in _wordedText14)
                        {
                            if (_periodEnumerator14.MoveNext())
                            {
                                string _govForeDate = _periodEnumerator14.Current.Value;
                                DateTime _dt = DateTime.Parse(_govForeDate);
                                string _periodName = "";
                                try
                                {
                                    _periodName = _periodEnumerator14.Current.Attribute("period-name").Value;
                                }
                                catch
                                {
                                    _periodName = "-";
                                }
                                if (_dt.Day != _day)
                                {
                                    _day = _dt.Day;
                                    _lowTempEnumerator7.MoveNext();
                                    _lowTemp = _lowTempEnumerator7.Current.Value;
                                    if( !(_cnt == 0 & _dt.Hour > 17 ) )
                                    {
                                        _highTempEnumerator7.MoveNext();
                                        _highTemp = _highTempEnumerator7.Current.Value;
                                    }
                                    ++_cnt;
                                }
                                _precipitationEnumerator14.MoveNext();
                                string? _precipitation = _precipitationEnumerator14.Current.Value;
                                _conditionsEnumerator14.MoveNext();
                                string? _condition = _conditionsEnumerator14.Current.Attribute("weather-summary").Value; ;
                                _iconsEnumerator14.MoveNext();
                                string? _icon = _iconsEnumerator14.Current.Value;
                                GovWeather7DayForecast _item = new GovWeather7DayForecast(_govForeDate, _periodName, _lowTemp, _highTemp, _precipitation, _condition, _icon, _txt.Value);
                                forecastItems.Add(_item);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("xpath: /data[@type='forecast'] not found");
                    }
                }
                catch (System.Exception _ex)
                {
                    Console.WriteLine(_ex.Message);
                    throw;
                }
            }
            return (new GovWeatherCurrentAnd7DayForecast(null, forecastLocation, forecastItems));
        }
        //
    }
}
