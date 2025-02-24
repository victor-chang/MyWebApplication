using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public string? ProductId { get; set; }

    public int? Quantity { get; set; }

    public string? CustomerName { get; set; }

    public string? Address { get; set; }
}
