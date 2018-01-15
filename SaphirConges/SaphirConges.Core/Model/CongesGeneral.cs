using System;
using SaphirCongesCore.Validation;
using System.ComponentModel.DataAnnotations;


namespace SaphirCongesCore.Models
{
    public class CongesGeneral
    {
        [Key]
        public virtual int CongesGeneralID { get; set; }

        [Required]
        public string Nom { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de début")]
        public virtual DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [CongesValidation("StartDate")]
        [Display(Name = "Date de fin")]
        public virtual DateTime EndDate { get; set; }

        [Range(1, 500)]
        [Display(Name = "Nombre de jours")]
        public virtual float NoOfDays
        {
            get { return (float)EndDate.Subtract(StartDate).TotalDays + 1; }
            set { }
        }

        [Required]
        [Display(Name = "Fréquence")]
        public virtual Frequence? Frequency { get; set; }

    }
    public enum Frequence
    {
        Annuel,
        AnnuelSpecifique
    }
}
