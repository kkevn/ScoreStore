using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ScoreStore.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace ScoreStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>//, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ScoreStore.Models.Game> Game { get; set; }
        public DbSet<ScoreStore.Models.Scores> Scores { get; set; }

        //public DbSet<DataProtectionKey> DataProtectionKeys => throw new NotImplementedException();
    }
}
