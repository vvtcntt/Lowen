using LOWEN.Models;
using System;
using System.Linq;
using System.Net.Mail;
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
                if (groupProduct[i].MaxSize == true)
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
            var config = db.tblConfigs.FirstOrDefault();
            ViewBag.Title = "<title>" + config.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + config.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + config.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + config.Keywords + "\" /> ";
            StringBuilder meta = new StringBuilder();
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://lowen.vn\" />";
            meta.Append("<meta itemprop=\"name\" content=\"" + config.Name + "\" />");
            meta.Append("<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta itemprop=\"description\" content=\"" + config.Description + "\" />");
            meta.Append("<meta itemprop=\"image\" content=\"\"" + config.Logo + " />");
            meta.Append("<meta property=\"og:title\" content=\"" + config.Title + "\" />");
            meta.Append("<meta property=\"og:type\" content=\"product\" />");
            meta.Append("<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />");
            meta.Append("<meta property=\"og:image\" content=\"\" />");
            meta.Append("<meta property=\"og:site_name\" content=\"http://lowen.vn\" />");
            meta.Append("<meta property=\"og:description\" content=\"" + config.Description + "\" />");
            meta.Append("<meta property=\"fb:admins\" content=\"\" />");
            ViewBag.Meta = meta;
            return View();
        }

        public PartialViewResult partialBanner()
        {
            var tblconfig = db.tblConfigs.FirstOrDefault();
            var listMenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == null).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for(int i=0;i<listMenu.Count;i++)
            {
                result.Append("<li>");
                result.Append("<a href=\"/"+listMenu[i].Tag+".html\" title=\""+listMenu[i].Name+ "\">" + listMenu[i].Name + "</a>");
                int idParent = listMenu[i].id;
                var listMenuChild= db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idParent).OrderBy(p => p.Ord).ToList();
                if(listMenuChild.Count>0)
                {
                    result.Append("<ul class=\"ul3\">");
                    for(int j=0;j<listMenuChild.Count;j++)
                    {
                        result.Append("<li><a href=\"/"+listMenuChild[j].Tag+ ".html\" title=\"" + listMenuChild[j].Name + "\">" + listMenuChild[j].Name + "</a></li>");
                    }
                   
                    result.Append("</ul>");
                }
               
                result.Append("</li>");
            }
            ViewBag.result = result.ToString();
            return PartialView(tblconfig);
        }

        public PartialViewResult partialFooter()
        {
            var tblconfig = db.tblConfigs.FirstOrDefault();
            return PartialView(tblconfig);
        }

        public ActionResult CommandSearch(FormCollection collection)
        {
            Session["Search"] = collection["txtSearch"];
            return Redirect("/Product/Search");
        }

        public PartialViewResult partialSlide()
        {
            var listImages = db.tblImages.Where(p => p.idCate == 1 && p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < listImages.Count; i++)
            {
                result.Append("<a href=\"" + listImages[i].Url + "\" title=\"" + listImages[i].Name + "\"><img src=\"" + listImages[i].Images + "\" alt=\"" + listImages[i].Name + "\" /></a>");
            }
            ViewBag.result = result.ToString();
            return PartialView();
        }
        public PartialViewResult partialIntroduction()
        {
           
            return PartialView(db.tblConfigs.FirstOrDefault());
        }
        public PartialViewResult partialdefault()
        {
            Session["Color"] = db.tblConfigs.First().Color;

            StringBuilder chuoi = new StringBuilder();
            chuoi.Append("<style>");
            var listMenu = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listMenu.Count; i++)
            {
                chuoi.Append(" .item_li" + listMenu[i].id + ":hover{background:#" + listMenu[i].Color + "}");
                chuoi.Append(" .item_li" + listMenu[i].id + ":hover a{color:#FFF !important}");
            }

            chuoi.Append(" </style>");
            ViewBag.chuoi = chuoi;
            return PartialView(db.tblConfigs.First());
        }
        public ActionResult CommandCall(string phone, string content)
        {
            string result = "";
            if (phone != null && phone != "")
            {

                var config = db.tblConfigs.First();
                var fromAddress = config.UserEmail;
                string fromPassword = config.PassEmail;
                var toAddress = config.Email;
                MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
                mailMessage.Subject = "Bạn nhận yêu cầu gọi điện thiết bị vệ sinh Lowen lúc " + DateTime.Now + "";
                mailMessage.Body = "Số điện thoại " + phone + ", nội dung " + content + "";
                //try
                //{

                SmtpClient smtpClient = new SmtpClient();
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = fromAddress,
                    Password = fromPassword
                };
                //smtpClient.UseDefaultCredentials = false;
                smtpClient.Send(mailMessage);
                result = "Bạn đã yêu cầu gọi điện thành công, bạn vui lòng cầm điện thoại trong khoảng 2-5 phút, chúng tôi sẽ liên hệ với bạn ngay !";
            }

            //}
            //catch(Exception ex)
            //{
            //    result = "Rất tiếc hiện chúng tôi không thể gọi cho bạn được, bạn có thể liên hệ qua hotline ở trên !" + ex;
            //}


            return Json(new { result = result });

        }

    }
}