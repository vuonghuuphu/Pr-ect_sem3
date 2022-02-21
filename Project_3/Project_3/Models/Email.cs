using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class Email
    {
        public int phone { set; get; }
        public int idb { set; get; }
        public int idp { set; get; }
        public int captcha { set; get; }
        public int captcheck { set; get; }
        public string to { set; get; }
        public string subject = "Xác nhận tài khoản";
        public string body = "da gui";
        public string email = "mobileservice8988@gmail.com";
        public string password = "Codau8tuoi";
    }
}