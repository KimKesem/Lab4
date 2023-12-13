using System;
using System.Collections.Generic;

public class TaskModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Tags { get; set; }
}