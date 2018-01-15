using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SaphirCongesCore.Models
{
    public enum TypePour
    {
        CongeEmploye,
        JoursFeries
    }
    public class CongesDescription
    {
        [Key]
        public virtual int CongesDescriptionID { get; set; }

        [Display(Name = "Type de congés")]
        [Required]
        public string TypeConge { get; set; }

        [Display(Name = "Couleur du Type de congés")]
        [Required]
        public string CongesColor { get; set; }

        [Display(Name = "Type")]
        [Required]
        public virtual TypePour? TypePour { get; set; }
    }

   
}
