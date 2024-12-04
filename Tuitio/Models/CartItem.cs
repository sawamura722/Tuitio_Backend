using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int CourseId { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
