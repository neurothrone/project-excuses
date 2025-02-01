using System.ComponentModel.DataAnnotations;

namespace Excuses.Persistence.Shared.Models;

public class Excuse
{
    public int Id { get; set; }

    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
}