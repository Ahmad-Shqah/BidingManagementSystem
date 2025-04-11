

namespace Biding.Application.DTOs
{
    public class BidDocumentDTO
    {
        public int BidId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
