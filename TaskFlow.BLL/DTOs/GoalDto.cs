using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.BLL.DTOs
{
    public class GoalDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public DateTime TargetDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
