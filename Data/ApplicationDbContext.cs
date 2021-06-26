using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ScoreStore.Models;

namespace ScoreStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ScoreStore.Models.Player> PlayerModel { get; set; }
        public DbSet<ScoreStore.Models.Game> Game { get; set; }
    }
}
