using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectGestionLocationVoiture.Models
{
    [Table("clients")]
    public class Client : User
    {

        // ViewBag pagemaitre layout
        // ViewData, ViewTemp vue et controller
        // ViewData => refresh
        // Viewtemp once used => deleted


        [Required]
        [MinLength(4), MaxLength(100)]
        [DisplayName("Nom Complet")]
        public string Nom { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telephone { get; set; }


        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Date de Naissance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateNaissance { get; set; }

        [MaxLength(100)]
        public string Cin { get; set; }

        [NotMapped]
        [DisplayName("CIN")]
        public HttpPostedFileBase CinFile { get; set; }

        [MaxLength(100)]
        public string PermisConduire { get; set; }

        [NotMapped]
        [DisplayName("Permis de Conduire")]
        public HttpPostedFileBase PermisConduireFile { get; set; }

    }
}