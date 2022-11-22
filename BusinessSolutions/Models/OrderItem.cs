using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutions.Models;

[Table("OrderItem")]
public partial class OrderItem
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? Name { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal? Quantity { get; set; }

    public string? Unit { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;
}
