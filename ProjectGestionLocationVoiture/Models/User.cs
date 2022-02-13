using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectGestionLocationVoiture.Models
{
    [Table("users")]
    public class User
    {

        [Key]
        public int UID { get; set; }

        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        [EmailAddress]
        [DisplayName("Email")]
        public string AdresseMail { get; set; }

        [MaxLength(100)]
        public string TypeUser { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Mot de passe")]
        public string MotDePasse { get; set; }

    }
}