using Microsoft.EntityFrameworkCore;
using Back.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.DAL.Context
{
    public class UAppContext : DbContext
    {
        public UAppContext(DbContextOptions<UAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("__User__");
            });

            modelBuilder.Entity<Symbol>(entity =>
            {
                entity.ToTable("__Symbol__");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("__Item__");
            });

            modelBuilder.Entity<ParamValue>(entity =>
            {
                entity.ToTable("__ParamValue__");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("__Session__");
            });

            modelBuilder.Entity<UserOption>(entity =>
            {
                entity.ToTable("__UserOption__");
            });
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Symbol> Symbols { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ParamValue> ParamValues { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<UserOption> UserOptions { get; set; }
        
    }
}
