using KoneProject.Models;
using Microsoft.EntityFrameworkCore;

namespace KoneProject.Datas
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }
        // DbSet for UserModel
        public DbSet<UserModel> Users { get; set; }

    }
}
