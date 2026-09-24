using Microsoft.EntityFrameworkCore;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Context;

public class CommentContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1995;Database=MultiShopCommentDb;User Id=sa;Password=Furkan12*;TrustServerCertificate=True");
    }
    
    public DbSet<UserComment> UserComments { get; set; }
}