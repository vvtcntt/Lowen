using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LOWEN.Models;
namespace LOWEN.Controllers.Display.product
{
    public class productController : Controller
    {
        // GET: product
        public ActionResult Index()
        {
            return View();
        }
        private LOWENContext db = new LOWENContext();
        string nUrl = "";
        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/" + ListMenu[i].Tag + ".html\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"" + (ListMenu[i].Level + 2) + "\" /> </li> › " + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }
        List<string> Mangphantu = new List<string>();
        public List<string> Arrayid(int idParent)
        {

            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idParent).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {
                Mangphantu.Add(ListMenu[i].id.ToString());
                int id = int.Parse(ListMenu[i].id.ToString());
                Arrayid(id);

            }

            return Mangphantu;
        }
        public ActionResult listProduct(string tag)
        {
            return View();
        }
    }
}