
namespace Biding.Application.DTOs
{
    public class TenderDTO
    {
        public string Title { get; set; }
        public string ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string? BudgetRange { get; set; }
        public string? EligibilityCriteria { get; set; }
        public string? Location { get; set; }
    }
}
