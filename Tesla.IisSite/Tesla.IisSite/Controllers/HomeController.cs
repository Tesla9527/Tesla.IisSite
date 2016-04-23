using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Web.Administration;
using System.Linq;
using Newtonsoft.Json;

namespace Tesla.IisSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sites = GetList().OrderBy(r => r.Id).ToList();
            return View(sites);
        }

        private List<Models.IisSite> GetList()
        {
            List<Models.SiteSave> iisSiteMemo = ReadFromFile<Models.SiteSave>("SiteData.json");

            List<Models.IisSite> iisSite = new List<Models.IisSite>();
            var sm = ServerManager.OpenRemote("localhost");
            foreach (Site site in sm.Sites)
            {
                foreach (Binding binding in site.Bindings)
                {
                    string[] bindingInfo = binding.BindingInformation.Split(':');
                    switch ((binding.Protocol ?? "").ToLower())
                    {
                        case "http":
                        case "https":
                            iisSite.Add(new Models.IisSite()
                            {
                                Id = site.Id,
                                Port = bindingInfo[1],
                                Name = site.Name,
                                Ip = bindingInfo[0],
                                Protocol = binding.Protocol,
                                Link = binding.Protocol + "://" + bindingInfo[0] + ":" + bindingInfo[1],
                                State = site.State.ToString(),
                                SitePath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath,
                                Memo = GetMemo(bindingInfo[1],iisSiteMemo),
                            });
                            break;
                        case "ftp":
                            iisSite.Add(new Models.IisSite()
                            {
                                Id = site.Id,
                                Port = bindingInfo[1],
                                Name = site.Name,
                                Ip = bindingInfo[0],
                                Protocol = binding.Protocol,
                                Link = binding.Protocol + "://" + bindingInfo[0] + ":" + bindingInfo[1],
                                State = "NA",
                                SitePath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath,
                                Memo = GetMemo(bindingInfo[1], iisSiteMemo),
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
            return iisSite;
        }

        /// <summary>
        /// 读取Json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<T> ReadFromFile<T>(string fileName)
        {
            var path = Server.MapPath("~/") + "\\App_Data\\" + fileName;
            string oldStr = null;
            if (System.IO.File.Exists(path))
            {
                oldStr = System.IO.File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<T>>(oldStr);
            }
            return null;
        }

        /// <summary>
        /// 通过Port获取Memo数据
        /// </summary>
        /// <param name="port"></param>
        /// <param name="li"></param>
        /// <returns></returns>
        public string GetMemo(string port,List<Models.SiteSave> li )
        {
            if (li == null) return "";
            IEnumerable<Models.SiteSave> ies = li.Where(a => a.Port == port);
            if (ies.Count() == 0) return "";
            else return ies.First().Memo;
        }
    }
}