using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Comment
{
    public int Id { get; set; }

    [MaxLength(255)]
    [Required]
    public string Observations { get; set; } = null!;

    public DateTime DateComment { get; set; } = DateTime.Now;

    public User? User { get; set; }

    [Required]
    public int UserId { get; set; }

    public Riffle? Riffle { get; set; }

    [Required]
    public int RiffleId { get; set; }
}