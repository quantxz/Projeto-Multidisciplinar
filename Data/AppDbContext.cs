using Microsoft.EntityFrameworkCore;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UsersModel> Users { get; set; }
        
        public DbSet<MessagesModel> Messages { get; set; }

        public DbSet<ProductsModel> Products { get; set; }

        public DbSet<PhotosModel> Photos { get; set; }
        
        public DbSet<ConversationsModel> Conversations { get; set; }
    }
}