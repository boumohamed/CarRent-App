using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectGestionLocationVoiture.Models
{
    [Table("reservations")]
    public class Reservation
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Client")]
        public int CID { get; set; }
        public Client Client { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Voiture")]
        public int VID { get; set; }
        public Voiture Voiture { get; set; }


        [Required]
        [DisplayName("Type Location")]
        [MaxLength(100)]
        public string TypeLocation { get; set; }

        [Key, Column(Order = 3, TypeName = "date")]
        [DisplayName("Date Debut")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateDebutLocation { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Date Retour")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateFinLocation { get; set; }
    }
}