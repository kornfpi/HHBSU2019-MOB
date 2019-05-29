using System;

namespace TodoREST
{
    public class HomeIssue
    {
        public string IssueId { get; set; }
        public int ActionPlanId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
    }
}
