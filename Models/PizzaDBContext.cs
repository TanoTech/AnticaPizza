using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AnticaPizza.Models
{
    public partial class PizzaDBContext : DbContext
    {
        public PizzaDBContext()
            : base("name=PizzaDBContext")
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .Property(e => e.Prezzo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Menu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.Histories)
                .WithRequired(e => e.Menu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Histories)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
