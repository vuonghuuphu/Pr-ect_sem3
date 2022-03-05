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
    public class account_balanceController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: account_balance
        public ActionResult Index()
        {
            return View(db.account_Balances.ToList());
        }

        // GET: account_balance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account_balance account_balance = db.account_Balances.Find(id);
            if (account_balance == null)
            {
                return HttpNotFound();
            }
            return View(account_balance);
        }

        // GET: account_balance/Create
        public ActionResult Create()
        {
            HttpCookie cookie = Request.Cookies["Email"];
            var email = cookie.Values["emailuser"];
            var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
            ViewBag.id = data.Id;
            var bank = db.Banks.ToList();
            ViewBag.ListBank = bank;
            return View();
        }

        // POST: account_balance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_uer,Money")] account_balance account_balance)
        {
            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies["Email"];
                var email = cookie.Values["emailuser"];
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                var id = data.Id;
                var datapay = db.account_Balances.Where(s => s.Id_uer == id).FirstOrDefault();
                if (datapay == null) {
                    db.account_Balances.Add(account_balance);
                    db.SaveChanges();
                    return RedirectToAction("Pay_acount", "account_balance", new { id = account_balance.Id, mon = account_balance.Money });
                }
                else
                {
                    var mon = account_balance.Money;
                    account_balance.Money = account_balance.Money + datapay.Money;
                    account_balance.Id = datapay.Id;
                    account_balance.Id_uer = datapay.Id_uer;
                    db.account_Balances.Remove(datapay);
                    db.account_Balances.Add(account_balance);
                    db.SaveChanges();



                    return RedirectToAction("Pay_acount", "account_balance", new { id = account_balance.Id, mon = mon });
                }

            }

            return View();
        }



        // POST: account_balance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            account_balance account_balance = db.account_Balances.Find(id);
            db.account_Balances.Remove(account_balance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Pay_acount(int? id, int? mon) {

            var account_balance = db.account_Balances.Where(b => b.Id == id).FirstOrDefault();

            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl1"];
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"];
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];

            //Build URL for VNPAY
            VnPayLibrary pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (mon * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Nạp tiền vào ví tài khoản"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "Naptienvaovi"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", account_balance.Id.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult Pay_acount_failed(int vnp_Amount, int vnp_TransactionNo, int vnp_TransactionStatus)
        {
            if (vnp_TransactionNo == 0 || vnp_TransactionStatus == 02)
            {
                HttpCookie cookie = Request.Cookies["Email"];
                var email = cookie.Values["emailuser"];
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                var id = data.Id;
                var datapay = db.account_Balances.Where(s => s.Id_uer == id).FirstOrDefault();
                var c = vnp_Amount / 100;
                account_balance o = new account_balance();
                o.Id = datapay.Id;
                o.Id_uer = datapay.Id_uer;
                o.Money = datapay.Money - c;
                db.account_Balances.Remove(datapay);
                db.SaveChanges();
                var d = db.account_Balances.Find(datapay.Id_uer);
                if (d == null)
                {
                db.account_Balances.Add(o);
                db.SaveChanges();
                }
                ViewBag.Message = "Nạp tiền vào ví thất bại";
                return View();
            }
            else
            {

                return RedirectToAction("AccountManagement", "AuthUser");
            }
        }
    }
}
