using DatingApp.API.model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) : base (option)  {       
        }

        public DbSet<Value> values { get; set; }

        public DbSet<User> users { get; set; }
        
    }
}