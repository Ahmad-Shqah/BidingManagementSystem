
namespace Biding.Domain.BidDomain
{
    public class Bid
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TenderId { get; set; }

        public decimal ProposedAmount { get; set; }

        public string? TechnicalProposal { get; set; }

        public string? FinancialProposal { get; set; }

        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; }

        public decimal Score {  get; set; }

        public Bid(int userId, int tenderId, decimal proposedAmount, string? technicalProposal, string? financialProposal, DateTime submittedAt, string status)
        {
            UserId=userId;
            TenderId=tenderId;
            ProposedAmount=proposedAmount;
            TechnicalProposal=technicalProposal;
            FinancialProposal=financialProposal;
            SubmittedAt=submittedAt;
            Status=status;
            Score=0;//initial value
        }
    }


}
