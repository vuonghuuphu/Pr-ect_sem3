using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class feedback
    {
        public int id { set; get; }
        public string name { set; get; }
        public string email { set; get; }
        public string subject { set; get; }
        public string content { set; get; }
    }
}