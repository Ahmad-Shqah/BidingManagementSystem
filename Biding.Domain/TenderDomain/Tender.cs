
namespace Biding.Domain.TenderDomain
{
    public class Tender
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ReferenceNumber { get; set; }

        public string? Description { get; set; }


        public string IssuedBy { get; set; }


        public DateTime IssueDate { get; set; }


        public DateTime ClosingDate { get; set; }


        public string Type { get; set; }


        public string Category { get; set; }

        public string? BudgetRange { get; set; }

        public string? EligibilityCriteria { get; set; }

        public int CreatedByUserId { get; set; }

        public string? Location { get; set; }

        public Tender(string title, string referenceNumber, string? description, string issuedBy, DateTime issueDate, DateTime closingDate, string type, string category, string? budgetRange, string? eligibilityCriteria, int createdByUserId, string? location)
        {
            Title=title;
            ReferenceNumber=referenceNumber;
            Description=description;
            IssuedBy=issuedBy;
            IssueDate=issueDate;
            ClosingDate=closingDate;
            Type=type;
            Category=category;
            BudgetRange=budgetRange;
            EligibilityCriteria=eligibilityCriteria;
            CreatedByUserId=createdByUserId;
            Location=location;
        }
    }

}
