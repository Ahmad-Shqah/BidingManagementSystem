
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

        public async Task<Bid> CreateBidAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task<Bid?> GetBidForTenderWithIdAsync(int tenderId, int bidId)
        {
            return await _context.Bids
                .FirstOrDefaultAsync(b => b.TenderId == tenderId && b.Id == bidId);
        }

        public async Task<IEnumerable<Bid>> GetAllBidsForTenderAsync(int tenderId)
        {
            return await _context.Bids
                .Where(b => b.TenderId == tenderId)
                .ToListAsync();
        }

        public async Task<Bid?> EvaluateBidAsync(int bidId,decimal score)
        {
            var bid = await _context.Bids.FindAsync(bidId);
            if (bid == null)
                return null;

            bid.Score = score;
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

