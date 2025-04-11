
namespace Biding_management_System.Models
{
    public class Evaluation
    {
        public int Id { get; set; }

        public int BidId { get; set; }

        public int EvaluatorId { get; set; }

 
        public double Score { get; set; }

 
        public string? Comment { get; set; }

        public DateTime EvaluationDate { get; set; } = DateTime.UtcNow;

        public Evaluation(int bidId, int evaluatorId, double score, string? comment, DateTime evaluationDate)
        {
            BidId=bidId;
            EvaluatorId=evaluatorId;
            Score=score;
            Comment=comment;
            EvaluationDate=evaluationDate;
        }
    }

}
