
using Biding.Application.IRepositories;
using Biding.Domain.TenderDomain;
using Biding_management_System.Data;
using Biding_management_System.Models;
using Microsoft.EntityFrameworkCore;


namespace Biding.Application.Repositories
{
    public class TenderRepository : Repository<Tender>, ITenderRepository
    {
        private readonly SystemDbContext _context;

        //inject my dbContext class
        public TenderRepository(SystemDbContext context) : base(context)
        {
            _context = context;
        }

        //create new tender 
        public async Task<Tender> CreateTenderAsync(Tender tender)
        {
            _context.Tenders.Add(tender);
            await _context.SaveChangesAsync();
            return tender;
        }

        //get an exsiting tender
        public async Task<Tender> GetTenderByIdAsync(int id)
        {
            return await _context.Tenders.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        //get all tenders available (u made as a User :) )
        public async Task<IEnumerable<Tender>> GetAllTendersByUserAsync(int userId)
        {
            return await _context.Tenders
                .Where(t => t.CreatedByUserId == userId)
                .ToListAsync();
        }

        //Uplode Doc for  tender
        public async Task<TenderDocument> UploadTenderDocumentAsync(TenderDocument tenderDocument)
        {
            var tender =  _context.Tenders.Where(t => t.Id == tenderDocument.TenderId).FirstOrDefault();
            if (tender == null) { return null; }
            _context.TenderDocuments.Add(tenderDocument);
            await _context.SaveChangesAsync();
            return tenderDocument;
        }
        public Tender getTenderById(int id)
        {
            var tender = (from t in _context.Tenders
                          where t.Id == id
                          select t).FirstOrDefault();

            return tender;
        }

   
    }
}

