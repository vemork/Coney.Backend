﻿using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Country
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    public ICollection<State>? States { get; set; }

    public int StatesCount => States == null ? 0 : States.Count;
}