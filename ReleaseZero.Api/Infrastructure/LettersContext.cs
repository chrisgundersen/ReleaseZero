using Microsoft.EntityFrameworkCore;
using ReleaseZero.Api.Models;

namespace ReleaseZero.Api.Infrastructure
{
    public class FooContext : DbContext
    {
        public FooContext(DbContextOptions<FooContext> options) : base(options) { }

        public DbSet<Foo> Foos { get; set; }
    }
}
