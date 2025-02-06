using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Excuses.Persistence.Shared.DTO;

public record ExcuseInputDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    [DefaultValue("Aliens abducted me and I lost track of time.")]
    public required string Text { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    [DefaultValue("work")]
    public required string Category { get; set; }
}