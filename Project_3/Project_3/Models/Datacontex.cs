using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Project_3.Models
{
    public class Datacontex : DbContext
    {
        public Datacontex() : base("OnlineMobileservices")
        {
        }
        public DbSet<Brand> brands { set; get; }

        public System.Data.Entity.DbSet<Project_3.Models.Bank> Banks { get; set; }

        public System.Data.Entity.DbSet<Project_3.Models.Oder> Oders { get; set; }

        public System.Data.Entity.DbSet<Project_3.Models.Product> products { get; set; }

        public System.Data.Entity.DbSet<Project_3.Models.feedback> feedbacks { get; set; }

        public System.Data.Entity.DbSet<Project_3.Models.Categorie> categories { get; set; }

        public System.Data.Entity.DbSet<Project_3.Models.account_balance> account_Balances { get; set; }

        public System.Data.Entity.DbSet<Project_3.Models.User> Users { get; set; }
    }
}