// using BookProject.Models;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Data;

// namespace BookProject.Data
// {
//     public class BookDbContext : DbContext
//     {

//         public BookDbContext (DbContextOptions<BookDbContext> options) : base(options)
//         {
//         }
        
//         public DbSet<User> Users { get; set; }
//         public DbSet<Role> Roles { get; set; }
//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

            
//             modelBuilder.Entity<User>()
//                 .HasIndex(u => u.UserId)
//                 .IsUnique();
//         }
//     }
// }
