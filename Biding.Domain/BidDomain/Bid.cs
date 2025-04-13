
namespace Biding.Domain.BidDomain
{
    public enum Status    //for bid status (at most 1 accepted and others refused ) :)
    {
        Accepted,
        Refused,
        Bending
    }
    public class Bid
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TenderId { get; set; }

        public decimal ProposedAmount { get; set; }

        public string? TechnicalProposal { get; set; }

        public string? FinancialProposal { get; set; }

        public DateTime SubmittedAt { get; set; }
        public Status Status { get; set; }

        public decimal Score {  get; set; }

        public Bid(int userId, int tenderId, decimal proposedAmount, string? technicalProposal, string? financialProposal, DateTime submittedAt)
        {
            UserId=userId;
            TenderId=tenderId;
            ProposedAmount=proposedAmount;
            TechnicalProposal=technicalProposal;
            FinancialProposal=financialProposal;
            SubmittedAt=submittedAt;
            Status=Status.Bending;
            Score=0;//initial value
        }
    }


}
