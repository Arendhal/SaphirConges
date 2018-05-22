using System;
using Microsoft.AspNet.Identity.EntityFramework;


namespace SaphirConges.Models
{

    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public System.Data.Entity.DbSet<SaphirCongesCore.Models.Conges> Conges { get; set; }

        //public System.Data.Entity.DbSet<SaphirCongesCore.Models.EmployeHierarchie> EmployeHierarchies { get; set; }

        //public System.Data.Entity.DbSet<SaphirCongesCore.Models.EmployeHierarchie> EmployeHierarchies { get; set; }
    }
}