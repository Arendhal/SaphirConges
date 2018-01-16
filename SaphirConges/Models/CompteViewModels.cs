using System;
using System.Web;
using System.Linq;
using SalesFirst.Core.Model;
using SaphirCongesCore.Models;
using System.ComponentModel.DataAnnotations;


namespace SaphirConges.Models
{
    public class LoginViewModel
    {
        
        /*[Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }*/
        [Required]
        [Display(Name = "Employé")]
        public Employee employeeUsername { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string password { get; set; }

        [Display(Name = "Se souvenir de moi")]
        public bool rememberMe { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("password", ErrorMessage = " Le mot de passe et la confirmation ne sont pas identiques.")]
        public string confirmPassword { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string oldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("newPassword", ErrorMessage = " Le mot de passe et la confirmation ne sont pas identiques.")]
        public string confirmPassword { get; set; }
    }
}