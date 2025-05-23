﻿
using Biding.Domain.BidDomain;

namespace Biding.Application.DTOs
{
    public class BidDTO
    {
        public int TenderId { get; set; }
        public decimal ProposedAmount { get; set; }
        public string? TechnicalProposal { get; set; }
        public string? FinancialProposal { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
