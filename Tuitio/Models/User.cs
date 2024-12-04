using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string? ProfileImage { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;
}
