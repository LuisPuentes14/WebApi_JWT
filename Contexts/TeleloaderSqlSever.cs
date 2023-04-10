using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Contexts
{
    public class TeleloaderSqlSever : DbContext
    {
        public TeleloaderSqlSever(DbContextOptions<TeleloaderSqlSever> option) : base(option)
        {
            //
        }

        public DbSet<Usuario> usuario { get; set; }
    }
}
