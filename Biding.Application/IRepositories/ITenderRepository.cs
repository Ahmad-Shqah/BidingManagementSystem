

using Biding.Domain.TenderDomain;

namespace Biding.Application.IRepositories
{
    public interface ITenderRepository : IRepository<Tender>
    {
        //for generating tender logic
        Task<Tender> CreateTenderAsync(Tender tender);
        //for getting tender logic
        Task<Tender> GetTenderByIdAsync(int id);
        Task<IEnumerable<Tender>> GetAllTendersByUserAsync(int userId);

        //uplode doc (create Tender Document)
        Task<TenderDocument> UploadTenderDocumentAsync(TenderDocument tenderDocument);
        public Tender getTenderById(int id);

    }
}
