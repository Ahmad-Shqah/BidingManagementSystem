
namespace Biding.Domain.BidDomain
{
    public class BidDocument
    {
        public int Id { get; set; }

        public int BidId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public BidDocument(int bidId, string fileName, string filePath, DateTime uploadedAt)
        {
            BidId=bidId;
            FileName=fileName;
            FilePath=filePath;
            UploadedAt=uploadedAt;
        }
    }

}
