using Excuses.Persistence.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Excuses.Persistence.EFCore.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<Excuse> Excuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Excuse>().HasData(
            new Excuse { Id = 1, Text = "My computer exploded.", Category = "work" },
            new Excuse { Id = 2, Text = "I got stuck in an endless email loop.", Category = "work" },
            new Excuse { Id = 3, Text = "My pet python is shedding and needs supervision.", Category = "work" },
            new Excuse { Id = 4, Text = "Aliens abducted my homework.", Category = "school" },
            new Excuse { Id = 5, Text = "The dog ate my laptop.", Category = "school" },
            new Excuse { Id = 6, Text = "I got lost in the metaverse and couldn't escape.", Category = "school" },
            new Excuse { Id = 7, Text = "I accidentally set my alarm for PM instead of AM.", Category = "social" },
            new Excuse { Id = 8, Text = "I was practicing social distancing... from everyone.", Category = "social" },
            new Excuse { Id = 9, Text = "My grandma challenged me to a gaming tournament.", Category = "social" },
            new Excuse { Id = 10, Text = "My Wi-Fi ran out of data.", Category = "technology" },
            new Excuse { Id = 11, Text = "The internet was too slow to function.", Category = "technology" },
            new Excuse
            {
                Id = 12, Text = "I mistakenly set my phone to 'Airplane Mode' and it flew away.",
                Category = "technology"
            },
            new Excuse { Id = 13, Text = "My cat hid my car keys.", Category = "pets" },
            new Excuse { Id = 14, Text = "My dog locked me out.", Category = "pets" },
            new Excuse
            {
                Id = 15, Text = "My parrot changed my password and won’t tell me what it is.", Category = "pets"
            },
            new Excuse { Id = 16, Text = "Gravity stopped working for me temporarily.", Category = "general" },
            new Excuse { Id = 17, Text = "I got stuck in an existential crisis.", Category = "general" },
            new Excuse { Id = 18, Text = "I was time-traveling and lost track of reality.", Category = "general" }
        );
    }
}