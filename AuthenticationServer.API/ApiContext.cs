﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models;

namespace AuthenticationServer.API
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "UsersDb");
        }
        public DbSet<User> Users { get; set; }

    }
}