using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class Captch
    {
        public int phone { set; get; }
        public int idb { set; get; }
        public int idp { set; get; }
        public string captcha { set; get; }
        public string captcheck { set; get; }
    }
}