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
        public Forecast()
        {
            Header = "";
            Title = "";
            Description = "";
            Image = "";
        }
        //
        public Forecast(string header, string title, DateTime published, string description, string image)
        {
            Header = header;
            Title = title;
            Published = published;
            Description = description;
            Image = image;
        }
    }
}
