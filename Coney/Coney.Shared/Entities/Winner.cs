using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Winner
{
    public int Id { get; set; }

    [MaxLength(255)]
    [Required]
    public string Observations { get; set; } = null!;

    public bool WasDelivered { get; set; } = false;

    public Prize? Prize { get; set; }

    [Required]
    public int PrizeId { get; set; }

    public User? User { get; set; }

    [Required]
    public int UserId { get; set; }

    public Riffle? Riffle { get; set; }

    [Required]
    public int RiffleId { get; set; }
}