using System;

namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class News
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public DateTime Published { get; set; }
        public string Creator { get; set; }
        //
        /// <summary>
        /// No parameter constructor
        /// </summary>
        public News()
        {
            Title = "";
            Link = "";
            Description = "";
            Details = "";
            Published = new DateTime(1980, 1, 1);
            Creator = "";
        }
        //
        /// <summary>
        /// All properties parameter constructor
        /// </summary>
        public News(string title, string link, string description, string details, DateTime published, string creator )
        {
            Title = title;
            Link = link;
            Description = description;
            Details = details;
            Published = published;
            Creator = creator;
        }
        //
    }
}
