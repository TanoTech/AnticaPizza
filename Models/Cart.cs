namespace AnticaPizza.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cart
    {
        public Cart()
        {
            Stato = true;
            Note = "aggiungi nota";
        }
        public int ID { get; set; }

        public int UserID { get; set; }

        public int MenuID { get; set; }

        public int Quantita { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        public bool Stato { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual User User { get; set; }
    }
}
