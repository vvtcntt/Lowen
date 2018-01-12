using LOWEN.Models;
using PagedList;
using PagedList.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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
        private string nUrl = "";

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

        private List<string> Mangphantu = new List<string>();

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
            var groupProduct = db.tblGroupProducts.FirstOrDefault(p => p.Tag == tag);
            int id = groupProduct.id;
            ViewBag.Title = "<title>" + groupProduct.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + groupProduct.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupProduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupProduct.Keyword + "\" /> ";
            StringBuilder meta = new StringBuilder();
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://lowen.vn/" + StringClass.NameToTag(tag) + ".html\" />";
            meta.Append("<meta itemprop=\"name\" content=\"" + groupProduct.Name + "\" />");
            meta.Append("<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta itemprop=\"description\" content=\"" + groupProduct.Description + "\" />");
            meta.Append("<meta itemprop=\"image\" content=\"\" />");
            meta.Append("<meta property=\"og:title\" content=\"" + groupProduct.Title + "\" />");
            meta.Append("<meta property=\"og:type\" content=\"product\" />");
            meta.Append("<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta property=\"og:image\" content=\"\" />");
            meta.Append("<meta property=\"og:site_name\" content=\"http://lowen.vn\" />");
            meta.Append("<meta property=\"og:description\" content=\"" + groupProduct.Description + "\" />");
            meta.Append("<meta property=\"fb:admins\" content=\"\" />");
            ViewBag.Meta = meta;
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlProduct(groupProduct.id) + "</ol> ";
            var groupProductChild = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == id).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            if (groupProductChild.Count > 0)
            {
                result.Append("<h1 class=\"name\">" + groupProduct.Name + "</h1>");

                for (int i = 0; i < groupProductChild.Count; i++)
                {
                    result.Append("<div class=\"tearProductList\">");
                    result.Append("<div class=\"nvar\">");
                    result.Append("<h2><a href=\"/" + groupProductChild[i].Tag + ".html\" title=\"" + groupProductChild[i].Name + "\">" + groupProductChild[i].Name + "</a></h2>");
                    result.Append("</div>");
                    result.Append("<div class=\"contentProductList\">");
                    int idCate = groupProductChild[i].id;
                    var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate).OrderBy(p => p.Ord).Take(10).ToList();
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
                        result.Append("<a href=\"/" + listProduct[j].Tag + ".htm\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                        result.Append("</div>");
                        result.Append("<h3>");
                        result.Append("<a href=\"/" + listProduct[j].Tag + ".htm\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a>");
                        result.Append("</h3>");
                        result.Append("<div class=\"boxPrice\">");
                        result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span><a href=\"/gio-hang\" title=\"Giỏ hàng\"><i class=\"fa fa-heart-o\" aria-hidden=\"true\"></i> Đặt hàng</a>");
                        result.Append("</div>");
                        result.Append("</div>");
                        result.Append("</div>");
                    }
                    result.Append("</div>");
                    result.Append(" </div>");
                }
            }
            else
            {
                result.Append("<div class=\"tearProductList\">");
                result.Append("<div class=\"nvar\">");
                result.Append("<h1><a href=\"/" + groupProduct.Tag + ".html\" title=\"" + groupProduct.Name + "\">" + groupProduct.Name + "</a></h1>");
                result.Append("</div>");
                result.Append("<div class=\"contentProductList\">");
                var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == id).OrderBy(p => p.Ord).ToList();
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
                    result.Append("<a href=\"/" + listProduct[j].Tag + ".htm\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                    result.Append("</div>");
                    result.Append("<h3>");
                    result.Append("<a href=\"/" + listProduct[j].Tag + ".htm\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a>");
                    result.Append("</h3>");
                    result.Append("<div class=\"boxPrice\">");
                    result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span><a href=\"/gio-hang\" title=\"Giỏ hàng\"><i class=\"fa fa-heart-o\" aria-hidden=\"true\"></i> Đặt hàng</a>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append("</div>");
                }
                result.Append("</div>");
                result.Append(" </div>");
            }
            ViewBag.result = result.ToString();
            return View();
        }

        public ActionResult productDetail(string tag)
        {
            var product = db.tblProducts.FirstOrDefault(p => p.Tag == tag);
            ViewBag.Title = "<title>" + product.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + product.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + product.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + product.Keyword + "\" /> ";
            StringBuilder meta = new StringBuilder();
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://lowen.vn/" + StringClass.NameToTag(tag) + ".htm\" />";
            meta.Append("<meta itemprop=\"name\" content=\"" + product.Name + "\" />");
            meta.Append("<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta itemprop=\"description\" content=\"" + product.Description + "\" />");
            meta.Append("<meta itemprop=\"image\" content=\"\"" + product.ImageLinkThumb + " />");
            meta.Append("<meta property=\"og:title\" content=\"" + product.Title + "\" />");
            meta.Append("<meta property=\"og:type\" content=\"product\" />");
            meta.Append("<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta property=\"og:image\" content=\"\" />");
            meta.Append("<meta property=\"og:site_name\" content=\"http://lowen.vn\" />");
            meta.Append("<meta property=\"og:description\" content=\"" + product.Description + "\" />");
            meta.Append("<meta property=\"fb:admins\" content=\"\" />");
            ViewBag.Meta = meta;
            int idCate = int.Parse(product.idCate.ToString());
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlProduct(idCate) + "</ol> ";
            int id = product.id;
            StringBuilder resultImages = new StringBuilder();
            var listImages = db.tblImageProducts.Where(p => p.idProduct == id).ToList();
            if(listImages.Count>0)
            {
                resultImages.Append("<li class=\"getImg" + product.id + "\"><a href=\"javascript:;\" onclick=\"javascript:return getImage('" + product.ImageLinkDetail + "', 'getImg" + product.id + "')\" title=\"" + product.Name + "\"><img src=\"" + product.ImageLinkDetail + "\" alt=\"" + product.Name + "\" /></a></li>");

                for (int i = 0; i < listImages.Count; i++)
                {
                    resultImages.Append("<li class=\"getImg" + listImages[i].id + "\"><a href=\"javascript:;\" onclick=\"javascript:return getImage('" + listImages[i].Images + "', 'getImg" + listImages[i].id + "')\" title=\"" + product.Name + "\"><img src=\"" + listImages[i].Images + "\" alt=\"" + product.Name + "\" /></a></li>");
                }
            }
            
            ViewBag.resultImages = resultImages.ToString();
            var ListGroupCri = db.tblGroupCriterias.Where(p => p.idCate == idCate).ToList();
            List<int> Mang1 = new List<int>();
            for (int i = 0; i < ListGroupCri.Count; i++)
            {
                Mang1.Add(int.Parse(ListGroupCri[i].idCri.ToString()));
            }
            var ListCri = db.tblCriterias.Where(p => Mang1.Contains(p.id) && p.Active == true).ToList();
            StringBuilder resultCriteria = new StringBuilder();
            for (int i = 0; i < ListCri.Count; i++)
            {
                int idCre = int.Parse(ListCri[i].id.ToString());
                var ListCr = (from a in db.tblConnectCriterias
                              join b in db.tblInfoCriterias on a.idCre equals b.id
                              where a.idpd == id && b.idCri == idCre && b.Active == true
                              select new
                              {
                                  b.Name,
                                  b.Url,
                                  b.Ord
                              }).OrderBy(p => p.Ord).ToList();
                if (ListCr.Count > 0)
                {
                    resultCriteria.Append("<tr>");
                    resultCriteria.Append("<td>" + ListCri[i].Name + "</td>");
                    resultCriteria.Append("<td>");
                    int dem = 0;
                    string num = "";
                    if (ListCr.Count > 1)
                        num = "⊹ ";
                    foreach (var item in ListCr)
                        if (item.Url != null && item.Url != "")
                        {
                            resultCriteria.Append("<a href=\"" + item.Url + "\" title=\"" + item.Name + "\">");
                            if (dem == 1)
                                resultCriteria.Append(num + item.Name);
                            else
                                resultCriteria.Append(num + item.Name);
                            dem = 1;
                            resultCriteria.Append("</a>");
                        }
                        else
                        {
                            if (dem == 1)
                                resultCriteria.Append(num + item.Name + "</br> ");
                            else
                                resultCriteria.Append(num + item.Name + "</br> ");
                            dem = 1;
                        }
                    resultCriteria.Append("</td>");
                    resultCriteria.Append(" </tr>");
                }
            }
            ViewBag.resultCriteria = resultCriteria.ToString();
            //tag
            var listTag = db.tblProductTags.Where(p => p.idp == id).ToList();
            StringBuilder resultTag = new StringBuilder();
            for (int i = 0; i < listTag.Count; i++)
            {
                resultTag.Append(" <li><a href=\"/tag/" + listTag[i].Tag + "\" title=\"" + listTag[i].Name + "\">" + listTag[i].Name + "</a></li>");
            }
            ViewBag.resultTag = resultTag.ToString();

            //Sản phẩm liên quan
            StringBuilder result = new StringBuilder();
            result.Append("<div class=\"contentProductList\">");
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate && p.id != id).OrderBy(p => p.Ord).Take(10).ToList();
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
                result.Append("<a href=\"/" + listProduct[j].Tag + ".htm\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                result.Append("</div>");
                result.Append("<h3>");
                result.Append("<a href=\"/" + listProduct[j].Tag + ".htm\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a>");
                result.Append("</h3>");
                result.Append("<div class=\"boxPrice\">");
                result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span><a href=\"/gio-hang\" title=\"Giỏ hàng\"><i class=\"fa fa-heart-o\" aria-hidden=\"true\"></i> Đặt hàng</a>");
                result.Append("</div>");
                result.Append("</div>");
                result.Append("</div>");
            }
            result.Append("</div>");
            ViewBag.result = result;
            return View(product);
        }

        public ActionResult productTag(string tag, int? page)
        {
            var kiemtra = db.tblProductTags.Where(p => p.Tag == tag).Select(p => p.idp).ToList();
            var listProduct = db.tblProducts.Take(0).ToList();
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;
            if (kiemtra.Count > 0)
            {
                listProduct = db.tblProducts.Where(p => kiemtra.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
                string name = db.tblProductTags.FirstOrDefault(p => p.Tag == tag).Name;
                ViewBag.Name = name;
                ViewBag.Title = "<title>" + name + "</title>";
                ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + name + "\" />";
                ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
                //ViewBag.canonical = "<link rel=\"canonical\" href=\"http://lowen.vn/Tag/"+StringClass.NameToTag(chuoitag)+"\" />");
                StringBuilder meta = new StringBuilder();
                meta.Append("<meta itemprop=\"name\" content=\"" + name + "\" />");
                meta.Append("<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />");
                meta.Append("<meta itemprop=\"description\" content=\"" + name + "\" />");
                meta.Append("<meta itemprop=\"image\" content=\"\" />");
                meta.Append("<meta property=\"og:title\" content=\"" + name + "\" />");
                meta.Append("<meta property=\"og:type\" content=\"product\" />");
                meta.Append("<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />");
                meta.Append("<meta property=\"og:image\" content=\"\" />");
                meta.Append("<meta property=\"og:site_name\" content=\"http://LOWEN.vn\" />");
                meta.Append("<meta property=\"og:description\" content=\"" + name + "\" />");
                meta.Append("<meta property=\"fb:admins\" content=\"\" />");
                ViewBag.Meta = meta;
                StringBuilder result = new StringBuilder();
                result.Append("<div class=\"tearProductList\">");
                result.Append("<div class=\"nvar\">");
                result.Append("<h1><a href=\"#\" title=\"" + name + "\">" + name + "</a></h1>");
                result.Append("</div>");
                result.Append("<div class=\"contentProductList\">");
                var listProducts = listProduct.ToPagedList(pageNumber, pageSize);
                for (int j = 0; j < listProducts.Count; j++)
                {
                    result.Append("<div class=\"tearPd\">");
                    if (listProducts[j].New == true)
                    {
                        result.Append(" <div class=\"news\">");
                        result.Append("<span>New</span>");
                        result.Append("</div>");
                    }

                    result.Append("<div class=\"contentTearPd\">");
                    result.Append("<div class=\"img\">");
                    result.Append("<a href=\"/" + listProducts[j].Tag + ".htm\" title=\"" + listProducts[j].Name + "\"><img src=\"" + listProducts[j].ImageLinkThumb + "\" alt=\"" + listProducts[j].Name + "\" /></a>");
                    result.Append("</div>");
                    result.Append("<h3>");
                    result.Append("<a href=\"/" + listProducts[j].Tag + ".htm\" title=\"" + listProducts[j].Name + "\">" + listProducts[j].Name + "</a>");
                    result.Append("</h3>");
                    result.Append("<div class=\"boxPrice\">");
                    result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProducts[j].Price) + "đ</span><a href=\"/gio-hang\" title=\"Giỏ hàng\"><i class=\"fa fa-heart-o\" aria-hidden=\"true\"></i> Đặt hàng</a>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append("</div>");
                }
                result.Append("</div>");
                result.Append(" </div>");
                ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> " + name + " </ol> ";
                ViewBag.result = result.ToString();
                return View(listProduct.ToPagedList(pageNumber, pageSize));
            }
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult search(string tag, int? page)
        {
            if (Session["Search"]!=null && Session["Search"]!="")
            tag = Session["Search"].ToString();
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.Name.Contains(tag)).ToList();
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;

            ViewBag.Name = tag;
            ViewBag.Title = "<title>" + tag + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tag + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tag + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tag + "\" /> ";
             StringBuilder meta = new StringBuilder();
            meta.Append("<meta itemprop=\"name\" content=\"" + tag + "\" />");
            meta.Append("<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta itemprop=\"description\" content=\"" + tag + "\" />");
            meta.Append("<meta itemprop=\"image\" content=\"\" />");
            meta.Append("<meta property=\"og:title\" content=\"" + tag + "\" />");
            meta.Append("<meta property=\"og:type\" content=\"product\" />");
            meta.Append("<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta property=\"og:image\" content=\"\" />");
            meta.Append("<meta property=\"og:site_name\" content=\"http://LOWEN.vn\" />");
            meta.Append("<meta property=\"og:description\" content=\"" + tag + "\" />");
            meta.Append("<meta property=\"fb:admins\" content=\"\" />");
            ViewBag.Meta = meta;
            StringBuilder result = new StringBuilder();
            result.Append("<div class=\"tearProductList\">");
            result.Append("<div class=\"nvar\">");
            result.Append("<h1><a href=\"/tag/"+tag+"\" title=\"" + tag + "\">" + tag + "</a></h1>");
            result.Append("</div>");
            result.Append("<div class=\"contentProductList\">");
            var listProducts = listProduct.ToPagedList(pageNumber, pageSize);
            for (int j = 0; j < listProducts.Count; j++)
            {
                result.Append("<div class=\"tearPd\">");
                if (listProducts[j].New == true)
                {
                    result.Append(" <div class=\"news\">");
                    result.Append("<span>New</span>");
                    result.Append("</div>");
                }

                result.Append("<div class=\"contentTearPd\">");
                result.Append("<div class=\"img\">");
                result.Append("<a href=\"/" + listProducts[j].Tag + ".htm\" title=\"" + listProducts[j].Name + "\"><img src=\"" + listProducts[j].ImageLinkThumb + "\" alt=\"" + listProducts[j].Name + "\" /></a>");
                result.Append("</div>");
                result.Append("<h3>");
                result.Append("<a href=\"/" + listProducts[j].Tag + ".htm\" title=\"" + listProducts[j].Name + "\">" + listProducts[j].Name + "</a>");
                result.Append("</h3>");
                result.Append("<div class=\"boxPrice\">");
                result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProducts[j].Price) + "đ</span><a href=\"/gio-hang\" title=\"Giỏ hàng\"><i class=\"fa fa-heart-o\" aria-hidden=\"true\"></i> Đặt hàng</a>");
                result.Append("</div>");
                result.Append("</div>");
                result.Append("</div>");
            }
            result.Append("</div>");
            result.Append(" </div>");
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li> " + tag + " </ol> ";
            ViewBag.result = result.ToString();
            Session["Search"] = "";
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }
    }
}