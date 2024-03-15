namespace AnticaPizza.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class History
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int MenuID { get; set; }

        public decimal Prezzo { get; set; }

        public int Quantita { get; set; }

        public int NumeroOrdine { get; set; }

        public bool Stato { get; set; }

        public DateTime DataOrdine { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual User User { get; set; }
    }
}
