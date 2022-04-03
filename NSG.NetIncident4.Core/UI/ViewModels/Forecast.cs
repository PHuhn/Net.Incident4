// ===========================================================================
using System.Text.RegularExpressions;
using System.ServiceModel.Syndication;
using System.Globalization;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class Forecast
    {
        public string Header { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        //
        /// <summary>
        /// No parameter constructor
        /// </summary>
        public Forecast()
        {
            Header = "";
            Title = "";
            Description = "";
            Image = "";
        }
        //
        /// <summary>
        /// All properties parameters constructor
        /// </summary>
        public Forecast(string header, string title, DateTime published, string description, string image)
        {
            Header = header;
            Title = title;
            Published = published;
            Description = description;
            Image = image;
        }
        //
        public static List<Forecast> ToAccuForecast(string zipCode)
        {
            Regex _regex = new Regex("\"(.*?)\"");
            SyndicationFeed _feed = NSG.NetIncident4.Core.UI.ViewHelpers.Helpers.GetSyndicationFeed("https://rss.accuweather.com/rss/liveweather_rss.asp?locCode=" + zipCode);
            List<SyndicationItem> _posts = _feed.Items.ToList();
            List<Forecast> _forecasts = new List<Forecast>();
            if (_posts.Count() > 2)
            {
                // Convert rss to forecasts
                for (var _lp = 0; _lp < 3; _lp++)
                {
                    var _post = _posts[_lp];
                    string[] _summary = _post.Summary.Text.Split("<");
                    string[] _forecast = _summary[0].Split(":");
                    var _matches = _regex.Matches(_summary[1]);
                    string _header = "";
                    DateTime _published = _post.PublishDate.LocalDateTime;
                    switch (_lp)
                    {
                        case 0: // zeroth is current temperature
                            _header = $"{_forecast[0]} as of {_published.ToShortTimeString()}";
                            _summary[0] = _forecast[1];
                            break;
                        case 1: // first is today's forecast
                            var dt = DateTime.Now;
                            _header = $"Today's  {dt.ToString("MMM", CultureInfo.InvariantCulture)} {dt.Day} forecast";
                            break;
                        case 2: // is tomorrow's forecast
                            var dt2 = DateTime.Now.AddDays(1);
                            _header = $"Tomorrow's  {dt2.ToString("MMM", CultureInfo.InvariantCulture)} {dt2.Day} forecast";
                            break;
                    }
                    string _title = _post.Title.Text;
                    string _description = _summary[0];
                    string _image = "";
                    if (_matches.Count > 0)
                    {
                        _image = _matches[0].Groups[1].ToString();
                    }
                    _forecasts.Add(new Forecast(_header, _title, _published, _description, _image));
                }
            }
            return _forecasts;
        }
    }
}
// ===========================================================================
