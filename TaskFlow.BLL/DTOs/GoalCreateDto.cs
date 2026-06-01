using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.BLL.DTOs
{
    public class GoalCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime TargetDate { get; set; }
        public int ProjectId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
