using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LOWEN.Models;
using System.Text;

namespace LOWEN.Controllers.Display.product
{
    public class ProductSynController : Controller
    {
        // GET: ProductSyn
        public ActionResult Index()
        {
            return View();
        }
        private LOWENContext db = new LOWENContext();
  
        public ActionResult listProductSyn()
        {
           
            ViewBag.Title = "<title>Danh sách sản phẩm LOWEN đồng bộ theo gói</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"Danh sách sản phẩm LOWEN đồng bộ theo gói\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"Danh sách sản phẩm LOWEN đồng bộ theo gói\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Danh sách sản phẩm LOWEN đồng bộ theo gói\" /> ";
            StringBuilder meta = new StringBuilder();
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://lowen.vn/san-pham-dong-bo\" />";
            
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›Sản phẩm đồng bộ Lowen</ol> ";
             StringBuilder result = new StringBuilder();
            
                result.Append("<div class=\"tearProductList\">");
                result.Append("<div class=\"nvar\">");
                result.Append("<h1> <a href=\"/san-pham-dong-bo\" title=\"Sản phẩm Lowen đồng bộ\">Sản phẩm Lowen đồng bộ</a></h1>");
                result.Append("</div>");
                result.Append("<div class=\"contentProductList\">");
                var listProduct = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    result.Append("<div class=\"tearPd\">");
                    if (listProduct[j].New == true)
                    {
                        result.Append(" <div class=\"news\">");
                        result.Append("<span>New</span>");
                        result.Append("</div>");
                    }

                    result.Append("<div class=\"contentTearPd\">");
                    result.Append("<div class=\"img\">");
                    result.Append("<a href=\"/syn/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                    result.Append("</div>");
                    result.Append("<h3>");
                    result.Append("<a href=\"/syn/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a>");
                    result.Append("</h3>");
                    result.Append("<div class=\"boxPrice\">");
                    result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span><a href=\"/Order/OrderAdd?id=" + listProduct[j].id + "&Ord=1\" title=\"Giỏ hàng\"><i class=\"fa fa-heart-o\" aria-hidden=\"true\"></i> Đặt hàng</a>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append("</div>");
                }
                result.Append("</div>");
                result.Append(" </div>");
           
            ViewBag.result = result.ToString();
            return View();
        }

        public ActionResult ProductSyn_Detail(string tag)
        {
            var tblproductSyn = db.tblProductSyns.First(p => p.Tag == tag);
            int id = int.Parse(tblproductSyn.id.ToString());
            string chuoi = "Khách hàng vui lòng kích vào chi tiết từng sản phẩm ở trên để xem thông thông số kỹ thuật !";
            ViewBag.Title = "<title>" + tblproductSyn.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblproductSyn.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblproductSyn.Keyword + "\" /> ";
            //Load Images Liên Quan
             
            int idsyn = int.Parse(tblproductSyn.id.ToString());
            if (tblproductSyn.Visit.ToString() != null && tblproductSyn.Visit.ToString() != "")
            {
                int visit = int.Parse(tblproductSyn.Visit.ToString());
                if (visit > 0)
                {
                    tblproductSyn.Visit = tblproductSyn.Visit + 1;
                    db.SaveChanges();
                }
                else
                {
                    tblproductSyn.Visit = tblproductSyn.Visit + 1;
                    db.SaveChanges();
                }
            }

            var Product = db.ProductConnects.Where(p => p.idSyn == idsyn).ToList();
            string chuoipr = "";
            string chuoisosanh = "";
            float tonggia = 0;
            if (Product.Count > 0)
            {
                chuoipr += "<div id=\"Content_spdb\">";
                chuoipr += "<span class=\"tinhnang\">&diams; Danh sách sản phẩm có trong " + tblproductSyn.Name + "</span>";
                chuoisosanh += "<div id=\"equa\">";
                chuoisosanh += "<div class=\"nVar_Equa\"><span>Bảng so sánh giá mua lẻ và mua theo bộ</span></div>";
                chuoisosanh += "<div class=\"Clear\"></div>";
                chuoisosanh += "<table width=\"200\" border=\"1\">";
                chuoisosanh += "<tr style=\"color:#333; text-transform:uppercase; line-height:25px; text-align:center\">";
                chuoisosanh += "<td style=\"width:5%;text-align:center\">STT</td>";
                chuoisosanh += "<td style=\"width:40%\">Tên Sản phẩm</td>";
                chuoisosanh += "<td style=\"width:10%;text-align:center\">Số lượng</td>";
                chuoisosanh += "<td style=\"width:20%;text-align:center\">Đơn Giá</td>";
                chuoisosanh += "<td style=\"text-align:center; width:20%\">Thành Tiền</td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "</div>";
                for (int i = 0; i < Product.Count; i++)
                {
                    string codepd = Product[i].idpd;

                    var Productdetail = db.tblProducts.Where(p => p.Code == codepd && p.Active == true).Take(1).ToList();
                    if (Productdetail.Count > 0)
                    {
                        int idCate = int.Parse(Productdetail[0].idCate.ToString());
                        var ListGroup = db.tblGroupProducts.Find(idCate);
                        chuoipr += "<div class=\"Tear_syn\">";
                        chuoipr += "<div class=\"img_syn\">";
                        chuoipr += "<div class=\"nvar_Syn\">";
                        chuoipr += "<h2><a href=\"/" + ListGroup.Tag + "" + Productdetail[0].Tag + "_" + Productdetail[0].id + ".html\" title=\"" + Productdetail[0].Name + "\">" + Productdetail[0].Name + "</a></h2>";
                        chuoipr += "</div>";
                        chuoipr += "<div class=\"img_syn\">";
                        chuoipr += "<a href=\"/" + ListGroup.Tag + "" + Productdetail[0].Tag + "_" + Productdetail[0].id + ".html\" title=\"" + Productdetail[0].Name + "\"><img src=\"" + Productdetail[0].ImageLinkThumb + "\" alt=\"" + Productdetail[0].Name + "\" /></a>";
                        chuoipr += "</div>";
                        chuoipr += "</div>";
                        chuoipr += "</div>";
                        chuoisosanh += "<tr style=\"line-height:20px\">";
                        chuoisosanh += "<td style=\"width:5%;text-align:center\">" + (i + 1) + "</td>";
                        chuoisosanh += "<td style=\"width:40%; text-indent:7px\">" + Productdetail[0].Name + "</td>";
                        chuoisosanh += "<td style=\"width:10%;text-align:center\"> 1 </td>";
                        chuoisosanh += "<td style=\"width:20%;text-align:center\">" + string.Format("{0:#,#}", Productdetail[0].PriceSale) + "</td>";
                        chuoisosanh += "<td style=\"text-align:center; width:20%\">" + string.Format("{0:#,#}", Productdetail[0].PriceSale) + "</td>";
                        chuoisosanh += " </tr>";
                        tonggia = tonggia + float.Parse(Productdetail[0].PriceSale.ToString());
                    }

                }
                chuoipr += "</div>";
                chuoisosanh += "<tr style=\"line-height:25px \">";
                chuoisosanh += "<td colspan=\"4\"><span style=\"text-align:center; margin-right:5px; font-weight:bold; display:block\">TỔNG GIÁ MUA LẺ</span></td>";
                chuoisosanh += "<td style=\"font-weight:bold; font-size:16px; text-align:center\">" + string.Format("{0:#,#}", Convert.ToInt32(tonggia)) + " đ</td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "<tr>";
                int sodu = Convert.ToInt32(tonggia) - int.Parse(tblproductSyn.PriceSale.ToString());

                chuoisosanh += "<td colspan=\"4\"><span style=\"text-align:center; margin-right:5px; font-weight:bold; display:block; color:#ff5400\">GIÁ MUA THEO BỘ</span></td>";
                chuoisosanh += "<td style=\"font-weight:bold; color:#ff5400; font-size:18px; font-family:UTMSwiss; text-align:center\">" + string.Format("{0:#,#}", tblproductSyn.PriceSale) + "đ<span style=\"font-style:italic; color:#333; font-size:12px; font-family:Arial, Helvetica, sans-serif; margin:5px; display:block; font-weight:normal\">Bạn đã tiết kiệm : " + string.Format("{0:#,#}", sodu) + "đ khi mua theo bộ</span></td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "</table>";
            }

            ViewBag.chuoi = chuoi;
            ViewBag.chuoisosanh = chuoisosanh;
            ViewBag.chuoipr = chuoipr;
            return View(tblproductSyn);
        }
    }
}