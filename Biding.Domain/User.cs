using Biding.Domain.BidDomain;
using Biding.Domain.TenderDomain;

namespace Biding_management_System.Models
{
        public enum UserRole    //for Role  Base Access Control :)
    {
        Admin,
        ProcurementOfficer,
        Bidder,
        Evaluator
    }
    public class User
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

        public UserRole Role { get; set; }

        public string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Tender>? CreatedTenders { get; set; }
    public ICollection<Bid>? Bids { get; set; }

        public User() { }

        public User(string fullName, string email, UserRole role, string passwordHash, DateTime createdAt, ICollection<Tender>? createdTenders, ICollection<Bid>? bids)
        {
            FullName=fullName;
            Email=email;
            Role=role;
            PasswordHash=passwordHash;
            CreatedAt = DateTime.UtcNow; 
            CreatedTenders = createdTenders ?? new List<Tender>(); 
            Bids = bids ?? new List<Bid>(); 
        }

        public User(string fullName, string email, UserRole role, string passwordHash)
        {
            FullName=fullName;
            Email=email;
            Role=role;
            PasswordHash=passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

     
    }


}
