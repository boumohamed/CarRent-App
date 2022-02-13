using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectGestionLocationVoiture.Models
{
    [Table("admins")]
    public class Admin : User
    {
        [Key]
        public int AID { get; set; }
    }
}