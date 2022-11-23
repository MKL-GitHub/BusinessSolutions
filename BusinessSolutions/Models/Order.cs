using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutions.Models;

[Table("Order")]
public partial class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DisplayName("Номер")]
    public string? Number { get; set; }

    [Required]
    [DisplayName("Дата")]
    public DateTime? Date { get; set; }

    [Required]
    [DisplayName("Поставщик")]
    public int ProviderId { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    [ForeignKey("ProviderId")]
    [InverseProperty("Orders")]
    public virtual Provider Provider { get; set; } = null!;
}
