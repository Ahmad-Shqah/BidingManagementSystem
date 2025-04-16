using Biding.Application.DTOs;
using Biding.Application.IRepositories;
using Biding.Domain.BidDomain;
using Biding_management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace BidingManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITenderRepository _tenderRepo;
        private readonly IConfiguration _configuration;

        public BidController(IBidRepository bidRepo, IUserRepository userRepo, ITenderRepository tenderRepo, IConfiguration configuration)
        {
            _bidRepo = bidRepo;
            _userRepo = userRepo;
            _tenderRepo = tenderRepo;
            _configuration=configuration;
        }

        // Create Bid
        [HttpPost("create Bid")]
        public async Task<IActionResult> CreateBid([FromBody] BidDTO bidDto,string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false
            }, out _);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepo.GetUserById(int.Parse(userId));
            // user authorization 
            if (user == null || (user.Role != UserRole.Bidder))
            {
                return Unauthorized("You do not have permission to create a Bid.");
            }

            var bid = new Bid(
                int.Parse(userId),
                bidDto.TenderId,
                bidDto.ProposedAmount,
                bidDto.TechnicalProposal,
                bidDto.FinancialProposal,
                bidDto.SubmittedAt
            );

            var createdBid = await _bidRepo.CreateBidAsync(bid);
            return Ok(createdBid);
        }

        // Get Bid BidId
        [HttpGet("get Bid wth BidId")]
        public async Task<IActionResult> GetBidForTenderWithId(int bidId)
        {
            var bid = await _bidRepo.GetByIdAsync(bidId);
            if (bid == null)
                return NotFound("Bid not found.");
            return Ok(bid);
        }

        // Get All Bids for Tender
        [HttpGet("get all-bids For tender with TenderId")]
        public async Task<IActionResult> GetAllBidsForTender(int tenderId)
        {
            var bids = await _bidRepo.GetAllBidsForTenderAsync(tenderId);
            return Ok(bids);
        }

        // Evaluate Bid: status values are : 0 for accepted,1 for refuesd, 2 for bending.
        [HttpPut("evaluate")]
        public async Task<IActionResult> EvaluateBid([FromBody] EvaluateDTO evaluateDto, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false
            }, out _);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepo.GetUserById(int.Parse(userId));
            // user authorization 
            if (user == null || (user.Role != UserRole.Evaluator))
            {
                return Unauthorized("You do not have permission to evaluate.");
            }



            var bid = await _bidRepo.EvaluateBidAsync(evaluateDto.BidId, evaluateDto.status);
            if (bid == null)
                return NotFound("Bid not found.");

            return Ok(bid);
        }

        // Upload Bid Document
        [HttpPost("uploadBidDocument")]
        public async Task<IActionResult> UploadBidDocument([FromBody] BidDocumentDTO docDto,string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false
            }, out _);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepo.GetUserById(int.Parse(userId));
            // user authorization 
            if (user == null || (user.Role != UserRole.Bidder))
            {
                return Unauthorized("You do not have permission to add a BidDoc.");
            }


            var bid = await _bidRepo.GetBidWithIdAsync(docDto.BidId);
            if (bid == null)
                return NotFound("Bid not found.");

            var document = new BidDocument(docDto.BidId, docDto.FileName, docDto.FilePath, docDto.UploadedAt);
            var result = await _bidRepo.UploadBidDocumentAsync(document);
            return Ok(new { message = "Document uploaded.", document = result });
        }
    }
}
