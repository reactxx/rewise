using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace bank.model
{
    public class Context : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Facts> Factss { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Integrated Security=true;Initial Catalog=bank");
        }
    }
}
