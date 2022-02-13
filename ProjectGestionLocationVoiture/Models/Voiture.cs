using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectGestionLocationVoiture.Models
{
    [Table("cars")]
    public class Voiture
    {
        [Key]
        public int VID { get; set; }

        [ForeignKey("Modele")]
        [DisplayName("Modele de voiture")]
        public int MID { get; set; }
        public Modele Modele { get; set; }

        [Required]
        [MaxLength(20)]
        [Index(IsUnique = true)]
        public string Immatriculation { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Date de mise en Circulation")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCirculation { get; set; }

        [Required]
        [MaxLength(100)]
        public string Carburant { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DisplayName("Prix journalier")]
        public double Prixournalier { get; set; }

        
        [MaxLength(100)]
        public string ImagePrincipale { get; set; }

        [NotMapped]
        [DisplayName("Image Principale")]
        public HttpPostedFileBase ImagePrincipaleFile { get; set; }


        [MaxLength(100)]
        public string ImageSecondaire { get; set; }

        [NotMapped]
        [DisplayName("Image Secondaire")]
        public HttpPostedFileBase ImageSecondaireFile { get; set; }

        public bool Dsiponible { get; set; }

        [Required]
        public string Categorie { get; set; }

    }
}