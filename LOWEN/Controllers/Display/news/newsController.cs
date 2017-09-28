using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LOWEN.Models;
using System.Text;
using PagedList;
using PagedList.Mvc;
namespace LOWEN.Controllers.Display.news
{
    public class newsController : Controller
    {
        // GET: news
        public ActionResult Index()
        {
            return View();
        }
        private LOWENContext db = new LOWENContext();
        string nUrl = "";
        public string UrlNews(int idCate)
        {
            var ListMenu = db.tblGroupNews.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <a href=\"/" + ListMenu[i].Tag + "\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
                }

            }
            return nUrl;
        }
        public PartialViewResult partialNewsHomes()
        {
            var newsHomes = db.tblNews.Where(p => p.Active == true && p.ViewHomes == true).OrderByDescending(p => p.DateCreate).Take(4).ToList();
            StringBuilder result = new StringBuilder();
            for(int i=0;i<newsHomes.Count;i++)
            {
                result.Append("<div class=\"newsTearHomes\">");
                result.Append("<a href=\"/news/"+newsHomes[i].Tag+ "\" title=\"" + newsHomes[i].Name + "\"><img src=\"" + newsHomes[i].Images + "\" alt=\"" + newsHomes[i].Name + "\"></a>");
                result.Append("<a class=\"name\" href=\"/news/" + newsHomes[i].Tag + "\" title=\"" + newsHomes[i].Name + "\">" + newsHomes[i].Name + "</a>");
                result.Append("<span>" + newsHomes[i].Name + "</span>");
                result.Append("<span class=\"times\">Ngày cập nhật : " + newsHomes[i].DateCreate + "</span>");
                result.Append("</div>");
            }
            ViewBag.result = result.ToString();
            var videoHomes = db.tblVideos.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultViewVideo = new StringBuilder();
            if(videoHomes.Count>0)
            {
                resultViewVideo.Append("<iframe width=\"100 %\" height=\"240\" src=\"https://www.youtube.com/embed/" + videoHomes[0].Code + "?rel=0&amp;controls=0&amp;showinfo=0\" frameborder=\"0\" allowfullscreen></iframe>");
                resultViewVideo.Append("<a class=\"name\" href=\"\" title=\"\">" + videoHomes[0].Name + "</a>");
                resultViewVideo.Append("<div id=\"listVideo\">");
                resultViewVideo.Append("<ul>");
                for (int i = 0; i < videoHomes.Count; i++)
                {
                    if (i > 0)
                    {
                        resultViewVideo.Append("<li>");
                        resultViewVideo.Append("<a href=\"\" title=\"" + videoHomes[i].Name + "\">");
                        resultViewVideo.Append("<i class=\"fa fa - video - camera\" aria-hidden=\"true\"></i> " + videoHomes[i].Name + "");
                        resultViewVideo.Append("</a>");
                        resultViewVideo.Append("</li>");
                    }

                }
                resultViewVideo.Append("</ul>");
                resultViewVideo.Append("</div>");
            }
            
            ViewBag.resultViewVideo = resultViewVideo.ToString();
            return PartialView();
        }
        public ActionResult newsDetail(string tag)
        {
            var tblnews = db.tblNews.First(p => p.Tag == tag);
            int idUser = int.Parse(tblnews.idUser.ToString());
            ViewBag.Username = db.tblUsers.Find(idUser).UserName;
            int idCate = int.Parse(tblnews.idCate.ToString());
            var groupnews = db.tblGroupNews.First(p => p.id == idCate);
            ViewBag.NameMenu = groupnews.Name;
            ViewBag.Title = "<title>" + tblnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnews.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblnews.Title + "\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + tblnews.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"/news/" + StringClass.NameToTag(tag) + "\" />";
            meta += "<meta itemprop=\"name\" content=\"" + tblnews.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"/" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblnews.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"/" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"/\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            int id = int.Parse(tblnews.id.ToString());
            if (tblnews.Keyword != null)
            {
                string Chuoi = tblnews.Keyword;
                string[] Mang = Chuoi.Split(',');
                List<int> araylist = new List<int>();
                for (int i = 0; i < Mang.Length; i++)
                {
                    string tabs = Mang[i].ToString();
                    var listnew = db.tblNews.Where(p => p.Tabs.Contains(tabs) && p.id != id && p.Active == true).ToList();
                    for (int j = 0; j < listnew.Count; j++)
                    {
                        araylist.Add(listnew[j].id);
                    }
                }
                var Lienquan = db.tblNews.Where(p => araylist.Contains(p.id) && p.Active == true && p.id != id).OrderByDescending(p => p.Ord).Take(3).ToList();
                string chuoinew = "";
                if (Lienquan.Count > 0)
                {
                    chuoinew += " <div class=\"Lienquan\">";
                    for (int i = 0; i > Lienquan.Count; i++)
                    {
                        chuoinew += "<a href=\"/news/" + Lienquan[i].Tag + "\" title=\"" + Lienquan[i].Name + "\"> " + Lienquan[i].Name + "</a>";
                    }
                    chuoinew += "</div>";
                }
                ViewBag.chuoinew = chuoinew;
            }
            string chuoinewnew = "";
            var NewsNew = db.tblNews.Where(p => p.Active == true && p.id != id).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            for (int i = 0; i < NewsNew.Count; i++)
            {
                chuoinewnew += "<li><a href=\"/news/" + NewsNew[i].Tag + "\" title=\"" + NewsNew[i].Name + "\" rel=\"nofollow\"><i class=\"fa fa-google - plus\" aria-hidden=\"true\"></i> " + NewsNew[i].Name + " <span>" + NewsNew[i].DateCreate + "</span></a></li>";
            }
            ViewBag.chuoinewnews = chuoinewnew;

            //load tag

            string chuoitag = "";
            if (tblnews.Keyword != null)
            {
                string Chuoi = tblnews.Keyword;
                string[] Mang = Chuoi.Split(',');
                List<int> araylist = new List<int>();
                for (int i = 0; i < Mang.Length; i++)
                {
                    chuoitag += "<a href=\"/TagNews/" + StringClass.NameToTag(Mang[i]) + "\" title=\"" + Mang[i] + "\">" + Mang[i] + "</a>";
                }
            }
            ViewBag.chuoitag = chuoitag;
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(idCate) + "</ol> ";

            return View(tblnews);
        }
        public ActionResult listNews(int? page, string tag)
        {

            tblGroupNew tblgroupnews = db.tblGroupNews.FirstOrDefault(p => p.Tag == tag);
            int idCate = tblgroupnews.id;
            var listnews = db.tblNews.Where(p => p.idCate == idCate && p.Active == true).OrderByDescending(p => p.Ord).ToList();
            const int pageSize = 10;
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
            ViewBag.Name = tblgroupnews.Name;
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(idCate) + "</ol> ";
            ViewBag.Title = "<title>" + tblgroupnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblgroupnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblgroupnews.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"/" + tblgroupnews.Tag + "\"/>";
            return View(listnews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult partialLeftNews()
        {
            return PartialView();
        }
        public ActionResult tagNews(int? page, string tag)
        {

            string[] Mang1 = StringClass.COnvertToUnSign1(tag).Split('-');
            string chuoitag = "";
            for (int i = 0; i < Mang1.Length; i++)
            {
                if (i == 0)
                    chuoitag += Mang1[i];
                else
                    chuoitag += " " + Mang1[i];
            }
            int dem = 1;
            string name = "";
            List<tblNew> ListNew = (from c in db.tblNews where c.Active == true select c).ToList();
            List<tblNew> listnews = ListNew.FindAll(delegate (tblNew math)
            {
              
                if ((math.Keyword != null && math.Keyword != "") && StringClass.COnvertToUnSign1(math.Keyword.ToUpper()).Contains(chuoitag.ToUpper()))
                {

                    string[] Manghienthi = math.Keyword.Split(',');
                    foreach (var item in Manghienthi)
                    {
                        if (dem == 1)
                        {
                            var kiemtra = StringClass.COnvertToUnSign1(item.ToUpper()).Contains(chuoitag.ToUpper());
                            if (kiemtra == true)
                            {
                                name = item;
                                dem = 0;
                            }
                        }
                    }

                    return true;
                }

                else
                    return false;
            }
            );
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
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

            ViewBag.Name = name;
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   › "+name+"</ol> ";
            ViewBag.Title = "<title>" + name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://lowen.vn/TagNews/" + StringClass.NameToTag(chuoitag) + "\" />"; ;
            return View(listnews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult introductions()
        {
            int idcate = db.tblNews.FirstOrDefault(p => p.Tag == "lien-he").id;
            var tblnews = db.tblNews.FirstOrDefault(p => p.idCate == idcate);
            ViewBag.Title = "<title>" + tblnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnews.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblnews.Title + "\" />";
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + tblnews.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"" + StringClass.NameToTag(tblnews.Tag) + "\" />";
            meta += "<meta itemprop=\"name\" content=\"" + tblnews.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"/" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblnews.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"/" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"/\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(idcate) + "</ol> ";

            return View(tblnews);
        }
    }
}