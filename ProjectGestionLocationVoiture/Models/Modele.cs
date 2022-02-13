using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectGestionLocationVoiture.Models
{
    [Table("modeles")]
    public class Modele
    {

        [Key]
        public int MID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Marque { get; set; }

        [Required]
        [MaxLength(100)]
        public string Serie { get; set; }
    }
}