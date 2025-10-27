using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    /// <summary>
    /// The context class is an abstract representation of the database, thus you must specify in here any tables which you would like to create in your database,
    /// including any configurations which you want to be applied in the database, for example the auto-generation of GUID fields. This is the peak of code-first approach.
    /// If we apply IdentityDbContext, it will automatically create tables (which the specifications are hidden) that will manage user accounts (example: AspNetUsers and AspNetRoles).
    /// Do you want to use User Accounts? If yes, then you must inherit from IdentityDbContext.
    /// </summary>
    public class ShoppingCartDbContext : IdentityDbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        //TODO: Configure lazy loading and GUID auto-generation.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
}
}