using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LOWEN.Models;
using System.Text;

namespace LOWEN.Controllers.Display.product
{
    public class SaleController : Controller
    {
        // GET: Sale
        public ActionResult Index()
        {
            return View();
        }
        private LOWENContext db = new LOWENContext();

        public ActionResult details(string tag)
        {
            var config = db.tblConfigs.FirstOrDefault();

            ViewBag.Title = "<title> " + config.TitleSale + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + config.TitleSale + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + config.TitleSale + "\" /> ";

            var listProductSalePriority = db.tblProducts.Where(p => p.Active == true && p.Priority == true && p.ProductSale == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultPriority = new StringBuilder();
            foreach (var item in listProductSalePriority)
            {
                resultPriority.Append("<div class=\"item\">");
                resultPriority.Append("<div class=\"contentItem\">");
                resultPriority.Append("<div class=\"img\">");
                resultPriority.Append("<a href=\"/" + item.Tag + "-pd\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultPriority.Append("</div>");
                resultPriority.Append("<a href=\"/" + item.Tag + "-pd\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultPriority.Append("<div class=\"buy\">");
                resultPriority.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultPriority.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultPriority.Append("<a href=\"/" + item.Tag + "-pd\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
                resultPriority.Append("</div>");
            }
            ViewBag.resultPriority = resultPriority.ToString();


            var listProductSyn = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            if (listProductSyn.Count > 0)
            {
                ViewBag.CheckSyn = "oke";
                StringBuilder resultSyn = new StringBuilder();
                foreach (var item in listProductSyn)
                {
                    resultSyn.Append(" <div class=\"item\">");
                    resultSyn.Append("<div class=\"contentItem\">");
                    resultSyn.Append("<div class=\"img\">");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkDetail + "\" title=\"" + item.Name + "\" /></a>");
                    resultSyn.Append(" </div>");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                    resultSyn.Append("<div class=\"buy\">");
                    resultSyn.Append("<span class=\"note\">Giá chỉ từ</span>");
                    resultSyn.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                    resultSyn.Append("<a href=\"/syn/" + item.Tag + "\" title=\"" + item.Name + "\" >Xem ngay  &raquo;</a>");
                    resultSyn.Append("</div>");
                    resultSyn.Append("</div>");
                    resultSyn.Append("</div>");
                }
                ViewBag.resultSyn = resultSyn.ToString();
            }

            var listProductSale = db.tblProducts.Where(p => p.Active == true && p.ProductSale == true).OrderBy(p => p.idCate).ToList();
            StringBuilder resultSale = new StringBuilder();
            foreach (var item in listProductSale)
            {
                resultSale.Append("<div class=\"item\">");
                resultSale.Append("<div class=\"contentItem\">");
                resultSale.Append("<div class=\"img\">");
                resultSale.Append("<a href=\"/" + item.Tag + "-pd\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" title=\"" + item.Name + "\" /></a>");
                resultSale.Append("</div>");
                resultSale.Append("<a href=\"/" + item.Tag + "-pd\" title=\"" + item.Name + "\" class=\"name\">" + item.Name + "</a>");
                resultSale.Append("<div class=\"buy\">");
                resultSale.Append("<span class=\"note\">Giá chỉ từ</span>");
                resultSale.Append("<span class=\"price\">" + string.Format("{0:#,#}", item.PriceSale) + "<samp>đ</samp></span>");
                resultSale.Append("<a href=\"/" + item.Tag + "-pd\" title=\"" + item.Name + "\">Xem ngay  &raquo;</a>");
                resultSale.Append("</div>");
                resultSale.Append("</div>");

                resultSale.Append("</div>");
            }
            ViewBag.resultSale = resultSale.ToString();
            return View(config);
        }
    }
}