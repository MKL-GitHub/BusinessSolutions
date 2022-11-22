using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutions.Models;

[Table("Order")]
public partial class Order
{
    [Key]
    public int Id { get; set; }

    public string? Number { get; set; }

    public DateTime? Date { get; set; }

    public int ProviderId { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    [ForeignKey("ProviderId")]
    [InverseProperty("Orders")]
    public virtual Provider Provider { get; set; } = null!;
}
