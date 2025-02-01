using Excuses.Persistence.Shared.Models;

namespace Excuses.Persistence.InMemory.Data;

public class InMemoryDataStore
{
    public readonly List<Excuse> Excuses =
    [
        new() { Id = 1, Text = "My computer exploded.", Category = "work" },
        new() { Id = 2, Text = "I got stuck in an endless email loop.", Category = "work" },
        new() { Id = 3, Text = "My pet python is shedding and needs supervision.", Category = "work" },
        new() { Id = 4, Text = "Aliens abducted my homework.", Category = "school" },
        new() { Id = 5, Text = "The dog ate my laptop.", Category = "school" },
        new() { Id = 6, Text = "I got lost in the metaverse and couldn't escape.", Category = "school" },
        new() { Id = 7, Text = "I accidentally set my alarm for PM instead of AM.", Category = "social" },
        new() { Id = 8, Text = "I was practicing social distancing... from everyone.", Category = "social" },
        new() { Id = 9, Text = "My grandma challenged me to a gaming tournament.", Category = "social" },
        new() { Id = 10, Text = "My Wi-Fi ran out of data.", Category = "technology" },
        new() { Id = 11, Text = "The internet was too slow to function.", Category = "technology" },
        new()
        {
            Id = 12, Text = "I mistakenly set my phone to 'Airplane Mode' and it flew away.", Category = "technology"
        },
        new() { Id = 13, Text = "My cat hid my car keys.", Category = "pets" },
        new() { Id = 14, Text = "My dog locked me out.", Category = "pets" },
        new() { Id = 15, Text = "My parrot changed my password and won’t tell me what it is.", Category = "pets" },
        new() { Id = 16, Text = "Gravity stopped working for me temporarily.", Category = "general" },
        new() { Id = 17, Text = "I got stuck in an existential crisis.", Category = "general" },
        new() { Id = 18, Text = "I was time-traveling and lost track of reality.", Category = "general" }
    ];
}