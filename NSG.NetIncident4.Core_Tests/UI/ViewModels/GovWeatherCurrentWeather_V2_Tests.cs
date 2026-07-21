//
using NSG.NetIncident4.Core.UI.ViewModels;
using NUnit.Framework;
using System.Security.Policy;
using System.ServiceModel.Syndication;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
//
namespace NSG.NetIncident4.Core_Tests.UI.ViewModels
{
    public class GovWeatherCurrentWeather_V2_Tests
    {
        //
        // Date: 7/13/2026 9:08:21 AM, Location: Ann Arbor, Ann Arbor Municipal Airport, MI,
        // Latitude: 42.27, Longitude: -83.73, Elevation: 837, LastUpdated: 2026-07-13T12:57:39-00:00, Icon: NULL,
        // Temperature: NA, Conditions: , Humidity: NA, Wind speed: NA,NA,knots,NA, Barometer: NA, Dewpoint: NA, Visibility: NA,
        //statute miles, Wind speed: NA, Wind gusts: NA, Wind units: knots, Wind direction: NA, Warning: True,
        // record:[WarningTitle: Extreme Heat Watch, WarningUrl: https://forecast.weather.gov/showsigwx.php?warnzone=MIZ075&warncounty=MIC161&firewxzone=MIZ075&local_place1=Ann+Arbor+MI&product1=Extreme+Heat+Watch
        GovWeatherCurrentWeather_V2 _badCurrentWeather = new GovWeatherCurrentWeather_V2(
            "Ann Arbor, Ann Arbor Municipal Airport, MI",
            "2026-07-13T12:57:39-00:00", "42.27", "-83.73", "837", null,
            "NA", "", "NA", "NA,NA,knots,NA", "NA", "NA", "NA",
            "NA", "NA", "knots", "NA");
        //
        // Date: 7/13/2026 10:53:41 AM, Location: Detroit, Willow Run Airport, MI,
        // Latitude: 42.24, Longitude: -83.61, Elevation: 715, LastUpdated: 2026-07-13T09:53:00-04:00, Icon: https://forecast.weather.gov/newimages/medium/sct.png,
        // Temperature: 78, Conditions: Fair, Humidity: 54, Wind speed: 3,NA,knots,999, Barometer: 30.19, Dewpoint: 60, Visibility: 10.00,
        //statute miles, Wind speed: 3, Wind gusts: NA, Wind units: knots, Wind direction: 999, Warning: True,
        // record:[WarningTitle: Extreme Heat Watch, WarningUrl: https://forecast.weather.gov/showsigwx.php?warnzone=MIZ075&warncounty=MIC161&firewxzone=MIZ075&local_place1=Ypsilanti+MI&product1=Extreme+Heat+Watch
        GovWeatherCurrentWeather_V2 _goodCurrentWeather = new GovWeatherCurrentWeather_V2(
            "Detroit, Willow Run Airport, MI",
            "2026-07-13T09:53:00-04:00", "42.27", "-83.73", "837", null,
            "78", "Fair", "54", "3,NA,knots,999", "30.19", "60", "10.00",
            "3", "NA", "knots", "999");
        //
        [SetUp]
        public void Setup()
        {
        }
        //
        [Test]
        public void ConvertWindSpeed_Bad_Test()
        {
            // given / when
            var windSpeed = _badCurrentWeather.ConvertWindSpeed();
            // then
            Assert.That(windSpeed.display, Is.EqualTo("NA"));
            Assert.That(windSpeed.windSpeed, Is.EqualTo(0.0));
        }
        //
        [Test]
        public void ConvertWindSpeed_Good_Test()
        {
            // given / when
            var windSpeed = _goodCurrentWeather.ConvertWindSpeed();
            // then
            Assert.That(windSpeed.windSpeed, Is.EqualTo(3.0));
            Assert.That(windSpeed.display, Is.EqualTo("-999- 3 mph"));
        }
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
        [TestCase("999", "-999-")]
        public void Directions_Tests(string dir360, string assert)
        {
            // N NE E SE S SW W NW
            // given
            var _current = new GovWeatherCurrentWeather_V2();
            // when
            string _direction = _current.ConvertDirection(dir360);
            // then
            Assert.That(_direction, Is.EqualTo(assert));
            return;
        }
        //
        [Test]
        public void ConvertVisibility_Bad_Test()
        {
            // given / when
            var visibility = _badCurrentWeather.ConvertVisibility();
            // then
            Assert.That(visibility, Is.EqualTo("NA"));
        }
        //
        [Test]
        public void ConvertVisibility_Good_Test()
        {
            // given / when
            var visibility = _goodCurrentWeather.ConvertVisibility();
            // then
            Assert.That(visibility, Is.EqualTo("10.00"));
        }
        //
        [Test]
        public void ConvertLastUpdated_Bad_Test()
        {
            // given / when
            var lastUpdated = _badCurrentWeather.ConvertLastUpdated();
            // then
            Assert.That(lastUpdated, Is.EqualTo("*** Not a current observation ***"));
        }
        //
        [Test]
        public void ConvertLastUpdated_Good_Test()
        {
            // given / when
            var lastUpdated = _goodCurrentWeather.ConvertLastUpdated();
            // then
            Assert.That(lastUpdated, Is.EqualTo("Jul 13, 2026 09:53 AM (-04:00)"));
        }
        //
        [Test]
        public void ConvertTemperatureFromF2C_Bad_Test()
        {
            // given / when
            var temperatureC = _badCurrentWeather.ConvertTemperatureFromF2C(_badCurrentWeather.CurrentTemp);
            // then
            Assert.That(temperatureC, Is.EqualTo("NA"));
        }
        //
        [Test]
        public void ConvertTemperatureFromF2C_Good_Test()
        {
            // given / when
            var temperatureC = _goodCurrentWeather.ConvertTemperatureFromF2C(_goodCurrentWeather.CurrentTemp);
            // then
            Assert.That(temperatureC, Is.EqualTo("26"));
        }
        //
        [Test]
        public void CalculateWindChill_Bad_Test()
        {
            // given / when
            var temperatureC = _badCurrentWeather.CalculateWindChill(0.0, 0.0);
            // then
            Assert.That(temperatureC, Is.EqualTo(""));
        }
        //
        [Test]
        public void CalculateWindChill_Good_Test()
        {
            // given / when
            var temperatureC = _goodCurrentWeather.CalculateWindChill(32.0, 8.0);
            // then
            Assert.That(temperatureC, Is.EqualTo("25"));
        }
        //
        [Test]
        [TestCase(79.00, 41.00, "")]
        [TestCase(81.00, 39.00, "")]
        [TestCase(80.00, 40.00, "80")]
        [TestCase(90.00, 66.00, "103")]
        public void CalculateHeatIndex_Tests(double temperatureF, double relativeHumidityPer, string assert)
        {
            // given / when
            var heatIndexF = _goodCurrentWeather.CalculateHeatIndex(temperatureF, relativeHumidityPer);
            // then
            Assert.That(heatIndexF, Is.EqualTo(assert));
        }
        //
        [Test]
        public void ConvertKnots2Mph_Bad_Test()
        {
            // given / when
            var windSpeedMph = _badCurrentWeather.ConvertKnots2Mph(_badCurrentWeather.WindSustainedSpeed);
            // then
            Assert.That(windSpeedMph, Is.EqualTo(0.0));
        }
        //
        [Test]
        public void ConvertKnots2Mph_Good_Test()
        {
            // given / when
            var windSpeedMph = _goodCurrentWeather.ConvertKnots2Mph(_goodCurrentWeather.WindSustainedSpeed);
            // then
            Assert.That(windSpeedMph, Is.EqualTo(3.0));
        }
        //
    }
}
