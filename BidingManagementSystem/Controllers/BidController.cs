using Biding.Application.DTOs;
using Biding.Application.IRepositories;
using Biding.Domain.BidDomain;
using Biding_management_System.Models;
using Microsoft.AspNetCore.Mvc;
namespace BidingManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITenderRepository _tenderRepo;

        public BidController(IBidRepository bidRepo, IUserRepository userRepo, ITenderRepository tenderRepo)
        {
            _bidRepo = bidRepo;
            _userRepo = userRepo;
            _tenderRepo = tenderRepo;
        }

        // Create Bid
        [HttpPost("create Bid")]
        public async Task<IActionResult> CreateBid([FromBody] BidDTO bidDto)
        {
            var user = _userRepo.GetUserById(bidDto.UserId);
            var tender = _tenderRepo.getTenderById(bidDto.TenderId);
            if (user == null || tender == null)
                return BadRequest("Invalid user or tender ID.");

            if (user.Role != UserRole.Bidder)
                return BadRequest("U dont have permetion to bid.");

            var bid = new Bid(
                bidDto.UserId,
                bidDto.TenderId,
                bidDto.ProposedAmount,
                bidDto.TechnicalProposal,
                bidDto.FinancialProposal,
                bidDto.SubmittedAt,
                bidDto.Status
            );

            var createdBid = await _bidRepo.CreateBidAsync(bid);
            return Ok(createdBid);
        }

        // Get Bid by TenderId and BidId
        [HttpGet("get Bid wth tender {tenderId}/bid/{bidId}")]
        public async Task<IActionResult> GetBidForTenderWithId(int tenderId, int bidId)
        {
            var bid = await _bidRepo.GetBidForTenderWithIdAsync(tenderId, bidId);
            if (bid == null)
                return NotFound("Bid not found.");
            return Ok(bid);
        }

        // Get All Bids for Tender
        [HttpGet("{tenderId}/all-bids")]
        public async Task<IActionResult> GetAllBidsForTender(int tenderId)
        {
            var bids = await _bidRepo.GetAllBidsForTenderAsync(tenderId);
            return Ok(bids);
        }

        // Evaluate Bid
        [HttpPut("evaluate/{bidId}")]
        public async Task<IActionResult> EvaluateBid(int bidId, [FromBody] decimal score)
        {
            var bid = await _bidRepo.EvaluateBidAsync(bidId, score);
            if (bid == null)
                return NotFound("Bid not found.");
            return Ok(bid);
        }

        // Upload Bid Document
        [HttpPost("uploadBidDocument")]
        public async Task<IActionResult> UploadBidDocument([FromBody] BidDocumentDTO docDto)
        {
            var bid = await _bidRepo.GetBidForTenderWithIdAsync(docDto.BidId, docDto.BidId);
            if (bid == null)
                return NotFound("Bid not found.");

            var document = new BidDocument(docDto.BidId, docDto.FileName, docDto.FilePath, docDto.UploadedAt);
            var result = await _bidRepo.UploadBidDocumentAsync(document);
            return Ok(new { message = "Document uploaded.", document = result });
        }
    }
}
