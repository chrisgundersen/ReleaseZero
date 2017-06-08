using Microsoft.EntityFrameworkCore;
using ReleaseZero.Api.Models;

namespace ReleaseZero.Api.Infrastructure
{
    /// <summary>
    /// Letters context.
    /// </summary>
    public class LettersContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Infrastructure.LettersContext"/> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public LettersContext(DbContextOptions<LettersContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the letters.
        /// </summary>
        /// <value>The letters.</value>
        public DbSet<Letter> Letters { get; set; }
    }
}
