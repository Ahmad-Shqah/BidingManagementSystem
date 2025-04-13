using Biding.Domain.BidDomain;

namespace Biding.Application.IRepositories
{
    public interface IBidRepository :IRepository<Bid>
    {
        Task<Bid> CreateBidAsync(Bid bid);
         Task<Bid?> GetBidWithIdAsync(int bidId);
        Task<IEnumerable<Bid>> GetAllBidsForTenderAsync(int tenderId);
        Task<Bid?> EvaluateBidAsync(int bidId, Status status);

        Task<BidDocument> UploadBidDocumentAsync(BidDocument bidDocument);
        Task<IEnumerable<BidDocument>> GetDocumentsForBidAsync(int bidId);
    }
}
