using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int StudentId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User Student { get; set; } = null!;
}
