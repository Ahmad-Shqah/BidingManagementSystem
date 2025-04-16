using Biding.Application.DTOs;
using Biding.Application.IRepositories;
using Biding.Application.Repositories;
using Biding.Domain.TenderDomain;
using Biding_management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BidingManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderRepository _tenderRepo;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;

        public TenderController(ITenderRepository tenderRepo,IUserRepository userRepo, IConfiguration configuration)
        {
            _tenderRepo = tenderRepo;
            _userRepo =userRepo;
            _configuration = configuration;
        }

        // Create a new tender
        [HttpPost("create Tender")]
        public async Task<IActionResult> CreateTender([FromBody] TenderDTO tenderDTO,string token)
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
            if (user == null || (user.Role != UserRole.ProcurementOfficer))
            {
                return Unauthorized("You do not have permission to create a tender.");
            }

            var tender = new Tender(
                tenderDTO.Title,
                tenderDTO.ReferenceNumber,
                tenderDTO.Description,
                user.Id.ToString(),
                tenderDTO.IssueDate,
                tenderDTO.ClosingDate,
                tenderDTO.Type,
                tenderDTO.Category,
                tenderDTO.BudgetRange,
                tenderDTO.EligibilityCriteria,
                user.Id,
                tenderDTO.Location
            );

            var createdTender = await _tenderRepo.CreateTenderAsync(tender);
            return CreatedAtAction(nameof(GetTenderById), new { id = createdTender.Id }, createdTender);
        }

        // Upload a document for a tender
        [HttpPost("uploadTenderDocument")]
        public async Task<IActionResult> UploadTenderDocument([FromBody] TenderDocumentDTO tenderDocDto,string token)
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
            if (user == null || (user.Role != UserRole.ProcurementOfficer))
            {
                return Unauthorized("You do not have permission to add a tenderDoc.");
            }



            var tender = await _tenderRepo.GetTenderByIdAsync(tenderDocDto.TenderId);
            if (tender == null)
            {
                return NotFound("Tender not found.");
            }

            //  create TenderDocument
            var tenderDocument = new TenderDocument(tenderDocDto.TenderId, tenderDocDto.FileName, tenderDocDto.FilePath, DateTime.Now);
            var uploadedDocument = await _tenderRepo.UploadTenderDocumentAsync(tenderDocument);

            return Ok(new { message = "Document uploaded successfully.", document = uploadedDocument });
        }

        // Get a tender by ID
        [HttpGet("GetTenderById/{id}")]
        public async Task<IActionResult> GetTenderById(int id)
        {
            var tender = await _tenderRepo.GetTenderByIdAsync(id);
            if (tender == null)
            {
                return NotFound("Tender not found.");
            }

            return Ok(tender);
        }

        // Get all tenders created by the current user
        [HttpGet("GetAlltenders/{userId}")]
        public async Task<IActionResult> GetAllTendersByUser(int userId)
        {
            var tenders = await _tenderRepo.GetAllTendersByUserAsync(userId);
            if (tenders == null || !tenders.Any())
            {
                return NotFound("No tenders found for this user.");
            }

            return Ok(tenders);
        }
    }
}
