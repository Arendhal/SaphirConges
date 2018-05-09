using System;
using SalesFirst.Core.Model;
using SaphirCongesCore.Validation;
using System.ComponentModel.DataAnnotations;


namespace SaphirCongesCore.Models
{
    public class Conges
    {
        [Key]
        public virtual int CongesID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = false)]
        [CurrentDateCheck]
        [Display(Name ="Date de début")]
        public virtual DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = false)]
        [CongesValidation("StartDate")]
        [Display(Name = "Date de fin")]
        public virtual DateTime EndDate { get; set; }

        [Required]
        [Range(0.5, 100)]
       
        [Display(Name = "Nombre de jours")]
        public float NoOfDays { get; set; }

        [Display(Name = "Demi-journée:")]
        public virtual String HalfDay { get; set; }

        [Display(Name = "Demi-journée:")]
        public virtual String HalfDayEnd { get; set; }

        [Display(Name = "Statut")]
        public virtual String Statut { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Posé le")]
        public virtual DateTime BookingDate { get; set; }

        [Display(Name = "Posé par")]
        public virtual String BookedBy { get; set; }


        public virtual Employee Employe { get; set; }

        [Required]
        [Display(Name = "Type de congés")]
        public virtual String TypeConges { get; set; }

        [Display(Name = "Description")]
        public virtual String CongesDescription { get; set; }

    }
}
