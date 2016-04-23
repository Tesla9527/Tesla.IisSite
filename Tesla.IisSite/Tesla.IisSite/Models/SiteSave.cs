using System.Web.Mvc;

namespace Tesla.IisSite.Models
{
    public class SiteSave
    {
        public string Port { get; set; }
        [AllowHtml]
        public string Memo { get; set; }
    }
}