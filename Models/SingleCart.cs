using System.Collections.Generic;

namespace AnticaPizza.Models
{
    public class SingleCart
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
