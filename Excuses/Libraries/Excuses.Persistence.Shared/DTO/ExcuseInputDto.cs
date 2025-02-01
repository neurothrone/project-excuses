using System.ComponentModel;

namespace Excuses.Persistence.Shared.DTO;

public record ExcuseInputDto
{
    [DefaultValue("Aliens abducted me and I lost track of time.")]
    public required string Text { get; set; }

    [DefaultValue("work")]
    public required string Category { get; set; }
}