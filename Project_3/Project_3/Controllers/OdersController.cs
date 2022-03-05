using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_3.Models;
using System.Configuration;

namespace Project_3.Controllers
{
    public class OdersController : Controller
    {

        private Datacontex db = new Datacontex();

        // GET: Oders
        public ActionResult Index(int? p, int? d)
        {
            ViewBag.ListProduct = p;
            ViewBag.ListBrand = d;
            return View();
        }


        // GET: Oders/Create
        public ActionResult Create(int? c, int? br, int? pro, int? pho)
        {
            if (br == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var brand = db.brands.Where(p => p.Id == br).ToList();
            var product = db.products.Where(p => p.Id == pro).ToList();
            var bank = db.Banks.ToList();
            if (brand == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ListProduct = product.First().Name;
                ViewBag.ListProductid = product.First().Id;
                ViewBag.ListBrand = brand.First().Name;
                ViewBag.ListBrandid = brand.First().Id;
                ViewBag.ListPrice = product.First().Price;
                ViewBag.ListBank = bank;
                ViewBag.Phone = pho;
            }
            Oder model = new Oder();
            model.Id_Brand = (int)br;
            model.Id_Product = (int)pro;
            model.Phone_number = pho.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Phone_number,Id_Brand,Id_Product,Price,Card_Id,Create_At")] Oder oder)
        {
            oder.Create_At = DateTime.Now;
            oder.Card_Id = 12345678;
            if (ModelState.IsValid)
            {
                db.Oders.Add(oder);
                db.SaveChanges();
                HttpCookie cookie = new HttpCookie("Id_oder");
                cookie.Values["id_oder"] = oder.Id.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);
                return RedirectToAction("Payment", "Oders", new { p = oder.Id});
            }

            return RedirectToAction("Create", "Oders", new { br = oder.Id_Brand, pro = oder.Id_Product, pho = oder.Phone_number }) ;
        }


        public ActionResult Bill(int? p)
        {
            var bill = db.Oders.Where(b => b.Id == p).ToList();
            ViewBag.ListOder = bill;
            var idbrand = bill.First().Id_Brand;
            var bra = db.brands.Where(b => b.Id == idbrand).ToList();
            ViewBag.ListBrand = bra;
            var idpro = bill.First().Id_Product;
            var pro = db.products.Where(b => b.Id == idpro).ToList();
            ViewBag.ListProduct = pro;
            return View();
        }

        public ActionResult Oder_account( int? br, int? pro)
        {
            HttpCookie cookie = Request.Cookies["Email"];
            if (cookie == null) { 
            return RedirectToAction("Login", "AuthUser");
            }
            var email = cookie.Values["emailuser"];
            if (email == "")
            {
                return RedirectToAction("Login", "AuthUser");
            }

            else {
                if (br == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                var id = data.Id;
                var datapay = db.account_Balances.Where(s => s.Id_uer == id).FirstOrDefault();
                var brand = db.brands.Where(p => p.Id == br).ToList();
                var product = db.products.Where(p => p.Id == pro).ToList();
                if (datapay == null) {
                    return RedirectToAction("Create", "account_balance");
                }
                else { 
                if (datapay.Money < product.First().Price) {
                    return RedirectToAction("AccountManagement", "AuthUser");
                }
                if (brand == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    ViewBag.Money = datapay.Money;
                    ViewBag.ListProduct = product.First().Name;
                    ViewBag.ListProductid = product.First().Id;
                    ViewBag.ListBrand = brand.First().Name;
                    ViewBag.ListBrandid = brand.First().Id;
                    ViewBag.ListPrice = product.First().Price;
                }
                }
                Oder model = new Oder();
                model.Id_Brand = (int)br;
                model.Id_Product = (int)pro;
            return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Oder_account([Bind(Include = "Id,Phone_number,Id_Brand,Id_Product,Price,Card_Id,Create_At")] Oder oder)
        {
            oder.Create_At = DateTime.Now;
            oder.Card_Id = 0;
            if (ModelState.IsValid)
            {

                HttpCookie cookie = Request.Cookies["Email"];
                var email = cookie.Values["emailuser"];
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                var id = data.Id;
                var datapay = db.account_Balances.Where(s => s.Id_uer == id).FirstOrDefault();

                account_balance a = new account_balance();
                a.Id = datapay.Id;
                a.Id_uer = datapay.Id_uer;
                a.Money = datapay.Money - oder.Price;

                db.account_Balances.Remove(datapay);
                db.account_Balances.Add(a);
                db.SaveChanges();

                db.Oders.Add(oder);
                db.SaveChanges();

                return RedirectToAction("Bill_Oder_account", "Oders", new { id = oder.Id,pho = oder.Phone_number});
            }

            return RedirectToAction("Create", "Oders", new { br = oder.Id_Brand, pro = oder.Id_Product,  });
        }

        public ActionResult Bill_Oder_account(int? id)
        {
            var bill = db.Oders.Where(b => b.Id == id).ToList();
            ViewBag.ListOder = bill;
            var idbrand = bill.First().Id_Brand;
            var bra = db.brands.Where(b => b.Id == idbrand).ToList();
            ViewBag.ListBrand = bra;
            var idpro = bill.First().Id_Product;
            var pro = db.products.Where(b => b.Id == idpro).ToList();
            ViewBag.ListProduct = pro;
            return View();
        }


        public ActionResult oder_admin()
        {
            ViewBag.ListBill = db.Oders.ToList();
            ViewBag.ListBill2 = "1";
            return View();
        }
        [HttpPost]
        public ActionResult oder_admin(Day day)
        {
            if (day.Date != null)
            {
                int result = Int32.Parse(day.Date);
                if (result < 10)
                {
                    day.Date = "0" + day.Date;
                }
            }
            if (day.Date == null && day.Month == null && day.Year == 0)
            {
                ViewBag.ListBill1 = day.Month;
                ViewBag.ListBill = db.Oders.ToList();
            }
            else if (day.Date != null && day.Month == null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '____"+day.Date+"_____%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Date + "%" + day.Year + "%__%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month != null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Date + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month != null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Date + "%" + day.Year + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month != null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%__%" + day.Year + "%__%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month != null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Year + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Year + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            ViewBag.ListBill1 = day.Month;

            return View();
        }

        public ActionResult Payment(int? p)
        {
            var bill = db.Oders.Where(b => b.Id == p).FirstOrDefault();

            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"];
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"];
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];

            OrderInfo order = new OrderInfo();

            order.OrderId = (long)bill.Id; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = bill.Price ; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            order.OrderDesc = "ok";
            order.CreatedDate = DateTime.Now;

            //Build URL for VNPAY
            VnPayLibrary pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (bill.Price*100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "Nạp tiền điện thoại"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", bill.Id.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm(string vnp_TxnRef,int vnp_TransactionNo,int vnp_TransactionStatus)
        {
                var random = new Random();
                string s = string.Empty;
                for (int i = 0; i < 13; i++)
                s = String.Concat(s, random.Next(10).ToString());

            if (vnp_TransactionNo == 0 || vnp_TransactionStatus == 02) {
                return RedirectToAction("pay_failed", "Oders", new { id = vnp_TxnRef});
            }
            ViewBag.Message = s;
            ViewBag.Message1 = vnp_TxnRef;
            return View();
        }

        public ActionResult pay_failed(string id)
        {
            var bill = db.Oders.Where(b => b.Id.ToString() == id).FirstOrDefault();
            if (bill != null)
            {
                db.Oders.Remove(bill);
                db.SaveChanges();
                ViewBag.Message = "Thanh toán thất bại";
                return View();
            }
            else {
                ViewBag.Message = "Thanh toán thất bại";
                return View();
            }
        }
    }
}
