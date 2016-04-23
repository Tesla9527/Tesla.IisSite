using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tesla.IisSite.Models
{
    public class IisSite
    {
        public long Id { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }       
        public string Protocol { get; set; }
        public string Link { get; set; }
        public string State { get; set; }
        public string SitePath { get; set; }
        public string Memo { get; set; }

    }
}