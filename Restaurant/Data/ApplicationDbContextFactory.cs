using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Restaurant.Data
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var conectionString = "Server=localhost;Database=resta;Uid=root;Pwd=;";

            optionsBuilder.UseMySql(
               conectionString,
                ServerVersion.AutoDetect(
                    conectionString
                )
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}