using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class BankContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-Q6FQQ59\SQLEXPRESS;Initial Catalog = Bank;Integrated Security = true;");

        }
    }
}
