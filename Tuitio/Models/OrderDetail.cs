using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class OrderDetail
{
    public int OrderdetailId { get; set; }

    public int OrderId { get; set; }

    public int CourseId { get; set; }

    public decimal Price { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
