//
using NSG.NetIncident4.Core.UI.ViewModels;
using NUnit.Framework;
//
namespace NSG.NetIncident4.Core_Tests.UI.ViewModels
{
    public class GovWeather_Tests
    {
		//
		GovWeather _govWeather;

        [SetUp]
        public void Setup()
        {
            _govWeather = new GovWeather();
        }
        private static string GovZip2LatLonUrl = @"https://graphical.weather.gov/xml/sample_products/browser_interface/ndfdXMLclient.php?listZipCodeList=";
        private static string xmlMapClickString = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<dwml xmlns:xsd=""https://www.w3.org/2001/XMLSchema"" xmlns:xsi=""https://www.w3.org/2001/XMLSchema-instance"" version=""1.0"" xsi:noNamespaceSchemaLocation=""https://graphical.weather.gov/xml/DWMLgen/schema/DWML.xsd"">
	<head>
		<product concise-name=""dwmlByDay"" operational-mode=""developmental"" srsName=""WGS 1984"">
			<creation-date refresh-frequency=""PT1H"">2025-11-22T16:13:22-05:00</creation-date>
			<category>current observations and forecast</category>
		</product>
		<source>
			<production-center>Detroit/Pontiac, MI</production-center>
			<credit>https://www.weather.gov/dtx</credit>
			<more-information>https://www.nws.noaa.gov/forecasts/xml/</more-information>
		</source>
	</head>
	<data type=""forecast"">
		<location>
			<location-key>point1</location-key>
			<description>Ann Arbor, MI</description>
			<point latitude=""42.27"" longitude=""-83.73""/>
			<city state=""MI"">Ann Arbor</city>
			<height datum=""mean sea level"">856</height>
		</location>
		<moreWeatherInformation applicable-location=""point1"">https://forecast.weather.gov/MapClick.php?lat=42.27&amp;lon=-83.73</moreWeatherInformation>
		<time-layout time-coordinate=""local"" summarization=""12hourly"">
			<layout-key>k-p12h-n14-1</layout-key>
			<start-valid-time period-name=""Tonight"">2025-11-22T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Sunday"">2025-11-23T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Sunday Night"">2025-11-23T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Monday"">2025-11-24T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Monday Night"">2025-11-24T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Tuesday"">2025-11-25T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Tuesday Night"">2025-11-25T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Wednesday"">2025-11-26T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Wednesday Night"">2025-11-26T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Thanksgiving Day"">2025-11-27T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Thursday Night"">2025-11-27T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Friday"">2025-11-28T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Friday Night"">2025-11-28T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Saturday"">2025-11-29T06:00:00-05:00</start-valid-time>
		</time-layout>
		<time-layout time-coordinate=""local"" summarization=""12hourly"">
			<layout-key>k-p24h-n7-1</layout-key>
			<start-valid-time period-name=""Tonight"">2025-11-22T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Sunday Night"">2025-11-23T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Monday Night"">2025-11-24T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Tuesday Night"">2025-11-25T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Wednesday Night"">2025-11-26T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Thursday Night"">2025-11-27T18:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Friday Night"">2025-11-28T18:00:00-05:00</start-valid-time>
		</time-layout>
		<time-layout time-coordinate=""local"" summarization=""12hourly"">
			<layout-key>k-p24h-n7-2</layout-key>
			<start-valid-time period-name=""Sunday"">2025-11-23T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Monday"">2025-11-24T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Tuesday"">2025-11-25T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Wednesday"">2025-11-26T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Thanksgiving Day"">2025-11-27T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Friday"">2025-11-28T06:00:00-05:00</start-valid-time>
			<start-valid-time period-name=""Saturday"">2025-11-29T06:00:00-05:00</start-valid-time>
		</time-layout>
		<parameters applicable-location=""point1"">
			<temperature type=""minimum"" units=""Fahrenheit"" time-layout=""k-p24h-n7-1"">
				<name>Daily Minimum Temperature</name>
				<value>34</value>
				<value>27</value>
				<value>39</value>
				<value>38</value>
				<value>26</value>
				<value>22</value>
				<value>19</value>
			</temperature>
			<temperature type=""maximum"" units=""Fahrenheit"" time-layout=""k-p24h-n7-2"">
				<name>Daily Maximum Temperature</name>
				<value>50</value>
				<value>51</value>
				<value>54</value>
				<value>48</value>
				<value>34</value>
				<value>34</value>
				<value>35</value>
			</temperature>
			<probability-of-precipitation type=""12 hour"" units=""percent"" time-layout=""k-p12h-n14-1"">
				<name>12 Hourly Probability of Precipitation</name>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value>70</value>
				<value>80</value>
				<value>40</value>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
				<value xsi:nil=""true""/>
			</probability-of-precipitation>
			<weather time-layout=""k-p12h-n14-1"">
				<name>Weather Type, Coverage, Intensity</name>
				<weather-conditions weather-summary=""Mostly Cloudy""/>
				<weather-conditions weather-summary=""Decreasing Clouds""/>
				<weather-conditions weather-summary=""Mostly Clear""/>
				<weather-conditions weather-summary=""Mostly Sunny""/>
				<weather-conditions weather-summary=""Mostly Cloudy then Rain Likely""/>
				<weather-conditions weather-summary=""Rain""/>
				<weather-conditions weather-summary=""Mostly Cloudy then Chance Rain""/>
				<weather-conditions weather-summary=""Chance Rain and Breezy""/>
				<weather-conditions weather-summary=""Mostly Cloudy""/>
				<weather-conditions weather-summary=""Mostly Cloudy""/>
				<weather-conditions weather-summary=""Mostly Cloudy""/>
				<weather-conditions weather-summary=""Partly Sunny""/>
				<weather-conditions weather-summary=""Mostly Cloudy""/>
				<weather-conditions weather-summary=""Partly Sunny""/>
			</weather>
			<conditions-icon type=""forecast-NWS"" time-layout=""k-p12h-n14-1"">
				<name>Conditions Icon</name>
				<icon-link>https://forecast.weather.gov/newimages/medium/nbkn.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/bkn.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/nfew.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/sct.png</icon-link>
				<icon-link>https://forecast.weather.gov/DualImage.php?i=nbkn&amp;j=nra&amp;jp=70</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/ra80.png</icon-link>
				<icon-link>https://forecast.weather.gov/DualImage.php?i=nbkn&amp;j=nra&amp;jp=40</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/ra.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/nbkn.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/bkn.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/nbkn.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/bkn.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/nbkn.png</icon-link>
				<icon-link>https://forecast.weather.gov/newimages/medium/bkn.png</icon-link>
			</conditions-icon>
			<wordedForecast time-layout=""k-p12h-n14-1"" dataSource=""dtxNetcdf"" wordGenerator=""markMitchell"">
				<name>Text Forecast</name>
				<text>Mostly cloudy, with a low around 34. South southwest wind 3 to 8 mph. Winds could gust as high as 18 mph. </text>
				<text>Mostly cloudy, then gradually becoming sunny, with a high near 50. West northwest wind 10 to 13 mph, with gusts as high as 28 mph. </text>
				<text>Mostly clear, with a low around 27. West wind 5 to 7 mph becoming calm in the evening. </text>
				<text>Mostly sunny, with a high near 51. Calm wind becoming south 5 to 9 mph in the morning. </text>
				<text>Rain likely after 1am. Cloudy, with a low around 39. South wind around 6 mph. Chance of precipitation is 70%. New precipitation amounts between a tenth and quarter of an inch possible. </text>
				<text>Rain before 1pm. High near 54. Chance of precipitation is 80%.</text>
				<text>A chance of rain after 1am. Mostly cloudy, with a low around 38. Chance of precipitation is 40%.</text>
				<text>A chance of rain before 1pm. Mostly cloudy, with a high near 48. Breezy. </text>
				<text>Mostly cloudy, with a low around 26.</text>
				<text>Mostly cloudy, with a high near 34.</text>
				<text>Mostly cloudy, with a low around 22.</text>
				<text>Partly sunny, with a high near 34.</text>
				<text>Mostly cloudy, with a low around 19.</text>
				<text>Partly sunny, with a high near 35.</text>
			</wordedForecast>
		</parameters>
	</data>
	<data type=""current observations"">
		<location>
			<location-key>point1</location-key>
			<point latitude=""42.22"" longitude=""-83.74""/>
			<area-description>Ann Arbor, Ann Arbor Municipal Airport, MI</area-description>
			<height datum=""mean sea level"" height-units=""feet"">837</height>
		</location>
		<moreWeatherInformation applicable-location=""point1"">https://www.nws.noaa.gov/data/obhistory/KARB.html</moreWeatherInformation>
		<time-layout time-coordinate=""local"">
			<layout-key>k-p1h-n1-1</layout-key>
			<start-valid-time period-name=""current"">2025-11-22T16:53:00-05:00</start-valid-time>
		</time-layout>
		<parameters applicable-location=""point1"">
			<temperature type=""apparent"" units=""Fahrenheit"" time-layout=""k-p1h-n1-1"">
				<value>40</value>
			</temperature>
			<temperature type=""dew point"" units=""Fahrenheit"" time-layout=""k-p1h-n1-1"">
				<value>25</value>
			</temperature>
			<humidity type=""relative"" time-layout=""k-p1h-n1-1"">
				<value>55</value>
			</humidity>
			<weather time-layout=""k-p1h-n1-1"">
				<name>Weather Type, Coverage, Intensity</name>
				<weather-conditions weather-summary=""Fair""/>
				<weather-conditions>
					<value>
						<visibility units=""statute miles"">10.00</visibility>
					</value>
				</weather-conditions>
			</weather>
			<conditions-icon type=""forecast-NWS"" time-layout=""k-p1h-n1-1"">
				<name>Conditions Icon</name>
				<icon-link>https://forecast.weather.gov/newimages/medium/sct.png</icon-link>
			</conditions-icon>
			<direction type=""wind"" units=""degrees true"" time-layout=""k-p1h-n1-1"">
				<value>250</value>
			</direction>
			<wind-speed type=""gust"" units=""knots"" time-layout=""k-p1h-n1-1"">
				<value>NA</value>
			</wind-speed>
			<wind-speed type=""sustained"" units=""knots"" time-layout=""k-p1h-n1-1"">
				<value>5</value>
			</wind-speed>
			<pressure type=""barometer"" units=""inches of mercury"" time-layout=""k-p1h-n1-1"">
				<value>30.03</value>
			</pressure>
		</parameters>
	</data>
</dwml>";

        //
        [TestCase("1", "N")]
        [TestCase("359", "N")]
        [TestCase("23", "NE")]
        [TestCase("68", "E")]
        [TestCase("90", "E")]
        [TestCase("113", "SE")]
        [TestCase("135", "SE")]
        [TestCase("158", "S")]
        [TestCase("180", "S")]
        [TestCase("203", "SW")]
        [TestCase("225", "SW")]
        [TestCase("248", "W")]
        [TestCase("270", "W")]
        [TestCase("293", "NW")]
        [TestCase("315", "NW")]
        public void Directions_Tests(string dir360, string assert)
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
                    Assert.That(_direction, Is.EqualTo(assert));
                    return;
                }
                else
                {
                    Assert.Fail($"Directions_Tests parameter: {dir360}");
                }
            }
            catch
            {
                Assert.Fail($"Directions_Tests parameter: {dir360}");
            }
        }

        [Test]
        public async Task ReadDataWithUrlToArrayAsync_Test()
        {
			string _zipCode = "48104";
            try
            {
                string urlString = $"{GovZip2LatLonUrl}{_zipCode}";
                string[] latlonArrayData = await _govWeather.ReadDataWithUrlToArrayAsync(urlString);
				Assert.That(latlonArrayData[0], Is.EqualTo("<?xml version='1.0'?>"));
                Assert.That(latlonArrayData[1].Substring(0,5), Is.EqualTo("<dwml"));
                Assert.That(latlonArrayData[3], Is.EqualTo("</dwml>"));
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex.Message);
                Assert.Fail(_ex.Message);
            }
        }
        //
        [TestCase("48750", "Oscoda, MI")]
        [TestCase("48104", "Ann Arbor, MI")]
        [TestCase("49090", "South Haven, MI")]
        public async Task GovWeather7DayForecast_Tests(string usaZipCode, string assert)
        {
            if (!string.IsNullOrEmpty(usaZipCode))
            {
                try
                {
                    string urlString = $"{GovZip2LatLonUrl}{usaZipCode}";
                    string[] _latlonArrayData = await _govWeather.ReadDataWithUrlToArrayAsync(urlString);
                    var _latlon = _govWeather.GovLatLonData(String.Join("", _latlonArrayData));
                    string govWeatherUrlString = String.Format("https://forecast.weather.gov/MapClick.php?lat={0}&lon={1}&unit=0&lg=english&FcstType=dwml", _latlon.lat, _latlon.lon);
                    Console.WriteLine(govWeatherUrlString);
                    string[] forecastArrayData = await _govWeather.ReadDataWithUrlToArrayAsync(govWeatherUrlString);
                    Console.WriteLine("GovWeather7DayForecastData started ...");
                    var locationForecast = _govWeather.GovWeatherForecastData(String.Join("", forecastArrayData));
                    Assert.That(locationForecast.Location, Is.EqualTo(assert));
					if(locationForecast.Forecast != null)
						Assert.That(locationForecast.Forecast.Count(), Is.GreaterThan(12));
					else
						Assert.Fail("locationForecast.Forecast is null");
                }
                catch (Exception _ex)
                {
                    Console.WriteLine(_ex.Message);
                    Assert.Fail(_ex.Message);
                }
            }
            else
            {
                Assert.Fail($"GovWeather7DayForecast parameter: {usaZipCode}");
            }
        }
        //
        [Test]
        public void GovLatLonData_Test()
        {
            // given
            string xmlString = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<dwml xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.0"" xsi:noNamespaceSchemaLocation=""http://localhost/xml/schema/DWML.xsd"">
	<latLonList>42.2754,-83.7308</latLonList>
</dwml>";
            // when
            var latLon = _govWeather.GovLatLonData(xmlString);
            // then
            Assert.That(latLon.lat, Is.EqualTo("42.2754"));
            Assert.That(latLon.lon, Is.EqualTo("-83.7308"));
        }
        //
        [Test]
        public void GovWeather7DayForecastData_Test()
        {
            // given / when
            var forecast = _govWeather.GovWeatherForecastData(String.Join("\r", xmlMapClickString));
			// then
			if (forecast != null && forecast.Forecast != null)
			{
                Assert.That(forecast.Location, Is.EqualTo("Ann Arbor, MI"));
                Assert.That(forecast.Forecast.Count(), Is.EqualTo(14));
                foreach (var item in forecast.Forecast)
                    Console.WriteLine(item.ToString());
            }
			else
			{
				Assert.Fail("forecast or forecast.Forecast is null");
			}
        }
        //
        [Test]
        public void GovWeatherCurrentWeatherData_Test()
        {
            // given / when
            var current = _govWeather.GovWeatherCurrentWeatherData(String.Join("\r", xmlMapClickString));
            // then
            Assert.That(current.Location, Is.EqualTo("Ann Arbor, Ann Arbor Municipal Airport, MI"));
            Assert.That(current.LastUpdated, Is.EqualTo("2025-11-22T16:53:00-05:00"));
            Assert.That(current.WeatherIcon, Is.EqualTo("https://forecast.weather.gov/newimages/medium/sct.png"));
            Assert.That(current.CurrentTemp, Is.EqualTo("40"));
            Assert.That(current.Dewpoint, Is.EqualTo("25"));
            Assert.That(current.Barometer, Is.EqualTo("30.03"));
            Assert.That(current.WeatherSummary, Is.EqualTo("Fair"));
            Assert.That(current.Humidity, Is.EqualTo("55"));
            Assert.That(current.WindSpeed, Is.EqualTo("5,NA,knots,250"));
            Assert.That(current.Visibility, Is.EqualTo("10.00,statute miles"));
            // 
        }
    }
}
