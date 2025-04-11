
namespace Biding.Domain.TenderDomain
{
    public class TenderDocument
    {
        public int Id { get; set; }

        public int TenderId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedAt { get; set; }

        public TenderDocument(int tenderId, string fileName, string filePath, DateTime uploadedAt)
        {
            TenderId=tenderId;
            FileName=fileName;
            FilePath=filePath;
            UploadedAt=uploadedAt;
        }
    }


}
