using System;
using System.Text;
using SalesFirst.Core.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaphirCongesCore.Models
{
    public class EmployeQuota
    {
        [Key]
        public virtual int EmployeQuotaID { get; set; }

        [Display(Name = " ID de l'employé")]
        public int EmployeID { get; set; }

        [ForeignKey("EmployeID")]
        public Employee Employe { get; set; }

        [Display(Name ="Solde de congés utilisé")] //TODO Revoir le nommage
        public double PaidQuota { get; set; }

        [Display(Name ="Solde de congés total")] //TODO Revoir le nommage
        public double NonPaidQuota { get; set; }

    }
}
