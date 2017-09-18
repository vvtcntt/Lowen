using LOWEN.Models;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LOWEN.Controllers.Display
{
    public class DefaultController : Controller
    {
        // GET: Default
        private LOWENContext db = new LOWENContext();

        public ActionResult Index()
        {
            var groupProduct = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < groupProduct.Count; i++)
            {
                if(groupProduct[i].MaxSize==true)
                {
                    result.Append("<div class=\"tear_1 max\">");
                }
                else
                 result.Append("<div class=\"tear_1\">");
                result.Append("<div class=\"contentTear1\">");
                result.Append("<div class=\"name\">");
                result.Append("<a href=\"/" + groupProduct[i].Tag + ".html\" title=\"" + groupProduct[i].Name + "\">" + groupProduct[i].Name + "</a>");
                result.Append("</div>");
                result.Append("<div class=\"img\">");
                result.Append("<a href=\"/" + groupProduct[i].Tag + ".html\" title=\"" + groupProduct[i].Name + "\"><img src=\"" + groupProduct[i].Images + "\" alt=\"" + groupProduct[i].Name + "\" /></a> ");
                result.Append("</div>");
                result.Append("</div>");
                result.Append("</div>");
            }
            ViewBag.result = result.ToString();
            return View();
        }

        public PartialViewResult partialBanner()
        {
            var tblconfig = db.tblConfigs.FirstOrDefault();
            return PartialView(tblconfig);
        }

        public PartialViewResult partialFooter()
        {
            var tblconfig = db.tblConfigs.FirstOrDefault();
            return PartialView(tblconfig);
        }

        public PartialViewResult partialSlide()
        {
            var listImages = db.tblImages.Where(p => p.idCate == 1 && p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < listImages.Count; i++)
            {
                result.Append(" <a href=\"" + listImages[i].Url + "\" title=\"" + listImages[i].Name + "\"><img src=\"" + listImages[i].Images + "\" alt=\"" + listImages[i].Name + "\" /></a>");
            }
            ViewBag.result = result.ToString();
            return PartialView();
        }
    }
}