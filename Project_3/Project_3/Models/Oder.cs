using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class Oder
    {
        public int Id { set; get; }
        public string Phone_number { set; get; }
        public int Id_Brand { set; get; }
        public int Id_Product { set; get; }
        public int Price { set; get; }
        public int Card_Id { set; get; }
        public DateTime Create_At { set; get; }
    }
}