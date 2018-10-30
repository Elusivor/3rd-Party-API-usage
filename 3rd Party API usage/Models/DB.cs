using System.Data.Entity;

namespace _3rd_Party_API_usage.Models
{
    public class DB : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
    }
}