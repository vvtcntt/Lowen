using LOWEN.Models;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LOWEN.Controllers.Display.download
{
    public class downloadController : Controller
    {
        // GET: download
        public ActionResult Index()
        {
            return View();
        }

        private LOWENContext db = new LOWENContext();

        public ActionResult listDownload()
        {
            ViewBag.Title = "<title> Tài về thư viện thiết bị vệ sinh LOWEN</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Tải các tài liệu như Catalog, báo giá sản phẩm thiết bị vệ sinh LOWEN mới nhất năm 2018\"/>";
            StringBuilder result = new StringBuilder();
            var listFile = db.tblFiles.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            result.Append(" <h1>Tải về thư viện Lowen</h1>");
            result.Append("<div id=\"contentDownload\">");
            for (int i = 0; i < listFile.Count; i++)
            {
                result.Append("<div class=\"tearDownload\">");
                result.Append("<div class=\"contentTearDownload\">");
                result.Append("<div class=\"name\">");
                result.Append("<a href=\"/" + listFile[i].File + "\" title=\"" + listFile[i].Name + "\">" + listFile[i].Name + "</a>");
                result.Append("</div>");
                result.Append("<a href=\"/" + listFile[i].File + "\" title=\"" + listFile[i].Name + "\" class=\"img\"><img src=\"" + listFile[i].Image + "\" alt=\"" + listFile[i].Name + "\" /></a>");
                result.Append("</div>");
                result.Append("</div>");
            }
            result.Append("</div>");
            ViewBag.result = result.ToString();
            return View();
        }
    }
}