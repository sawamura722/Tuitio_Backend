﻿using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
