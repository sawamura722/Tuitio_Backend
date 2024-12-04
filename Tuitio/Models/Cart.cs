using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int StudentId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User Student { get; set; } = null!;
}
