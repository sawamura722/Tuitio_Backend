using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseTitle { get; set; } = null!;

    public string? CourseDescription { get; set; }

    public decimal Price { get; set; }

    public bool IsOnline { get; set; }

    public int TeacherId { get; set; }

    public string? Thumbnail { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Location { get; set; }

    public int? Limit { get; set; }

    public TimeOnly? StartTimeAt { get; set; }

    public TimeOnly? EndTimeAt { get; set; }

    public DateOnly? StartDateAt { get; set; }

    public DateOnly? EndDateAt { get; set; }

    public bool? Monday { get; set; }

    public bool? Tuesday { get; set; }

    public bool? Wednesday { get; set; }

    public bool? Thursday { get; set; }

    public bool? Friday { get; set; }

    public bool? Saturday { get; set; }

    public bool? Sunday { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User Teacher { get; set; } = null!;

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
