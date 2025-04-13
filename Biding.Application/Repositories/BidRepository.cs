
using Biding.Domain.BidDomain;
using Microsoft.EntityFrameworkCore;
using Biding_management_System.Data;
using Biding.Application.IRepositories;

namespace Biding.Application.Repositories
{
    public class BidRepository :Repository<Bid> , IBidRepository
    {
        private readonly SystemDbContext _context;


        public BidRepository(SystemDbContext context) : base(context)
        {
            _context = context;
        }

        // Big code to auto score bids
        public async Task<Bid> CreateBidAsync(Bid bid)
        {
            // Add the new bid
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            // Get all bids for the same TenderId
            var allBids = await _context.Bids
                .Where(b => b.TenderId == bid.TenderId)
                .ToListAsync();

            // Find the lowest ProposedAmount
            decimal minAmount = allBids.Min(b => b.ProposedAmount);

            // Rescore all bids
            foreach (var b in allBids)
            {
                b.Score = (minAmount / b.ProposedAmount) * 100;
            }

            // Save the updated scores
            await _context.SaveChangesAsync();

            return bid;
        }


        public async Task<Bid?> GetBidWithIdAsync( int bidId)
        {
            return await _context.Bids
                .FirstOrDefaultAsync( b => b.Id == bidId);
        }

        public async Task<IEnumerable<Bid>> GetAllBidsForTenderAsync(int tenderId)
        {
            return await _context.Bids
                .Where(b => b.TenderId == tenderId)
                .OrderBy(b => b.ProposedAmount)
                .ToListAsync();
        }

        //evaluate bid (to choose thhe winning bid)
        public async Task<Bid?> EvaluateBidAsync(int bidId, Status status)
        {
            var bid = await _context.Bids.FindAsync(bidId);
            if (bid == null)
                return null;

            bid.Status = status;
            // Get all bids for the same TenderId
            var allBids = await _context.Bids
                .Where(b => b.TenderId == bid.TenderId)
                .ToListAsync();
            foreach (var b in allBids)
            {//When a bid is set to be accepted for that tender, the other bids must be set refused
                if (b.Status != Status.Accepted)
                    b.Status = Status.Refused;
            }
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task<BidDocument> UploadBidDocumentAsync(BidDocument bidDocument)
        {
            _context.BidDocuments.Add(bidDocument);
            await _context.SaveChangesAsync();
            return bidDocument;
        }

        public async Task<IEnumerable<BidDocument>> GetDocumentsForBidAsync(int bidId)
        {
            return await _context.BidDocuments
                .Where(d => d.BidId == bidId)
                .ToListAsync();
        }
    }
}

