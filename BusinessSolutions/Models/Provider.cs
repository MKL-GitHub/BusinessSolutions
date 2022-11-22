using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutions.Models;

[Table("Provider")]
public partial class Provider
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [InverseProperty("Provider")]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
