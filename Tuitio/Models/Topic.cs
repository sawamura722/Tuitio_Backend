using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class Topic
{
    public int TopicId { get; set; }

    public int CourseId { get; set; }

    public string TopicTitle { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
