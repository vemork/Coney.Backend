using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Ticket
{
    public int Id { get; set; }

    [MaxLength(10)]
    [Required]
    public string Code { get; set; } = null!;

    public bool WasPaid { get; set; } = false;

    public Riffle? Riffle { get; set; }

    [Required]
    public int RiffleId { get; set; }
}