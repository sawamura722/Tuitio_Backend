using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class School
{
    public int SchoolId { get; set; }

    public string SchoolTitle { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Telephone { get; set; } = null!;
}
