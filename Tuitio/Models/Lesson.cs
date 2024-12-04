using System;
using System.Collections.Generic;

namespace Tuitio.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public string LessonTitle { get; set; } = null!;

    public string? LessonContent { get; set; }

    public string? VideoUrl { get; set; }

    public string? Thumbnail { get; set; }

    public int? TopicId { get; set; }

    public TimeOnly? Duration { get; set; }

    public virtual Topic? Topic { get; set; }
}
