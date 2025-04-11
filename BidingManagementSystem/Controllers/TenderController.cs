using Biding.Application.DTOs;
using Biding.Application.IRepositories;
using Biding.Domain.TenderDomain;
using Biding_management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace BidingManagementSystem.Controllers
{
    public class TenderController : ControllerBase
    {
        private readonly ITenderRepository _tenderRepo;
        private readonly IUserRepository _userRepo;

        public TenderController(ITenderRepository tenderRepo,IUserRepository userRepo)
        {
            _tenderRepo = tenderRepo;
            _userRepo =userRepo;
        }

        // Create a new tender
        [HttpPost("create Tender")]
        public async Task<IActionResult> CreateTender([FromBody] TenderDTO tenderDTO)
        {
            // user authorization 
            var user = _userRepo.GetUserById(int.Parse(tenderDTO.IssuedBy));
            if (user == null || (user.Role != UserRole.ProcurementOfficer))
            {
                return Unauthorized("You do not have permission to create a tender.");
            }

            var tender = new Tender(
                tenderDTO.Title,
                tenderDTO.ReferenceNumber,
                tenderDTO.Description,
                tenderDTO.IssuedBy,
                tenderDTO.IssueDate,
                tenderDTO.ClosingDate,
                tenderDTO.Type,
                tenderDTO.Category,
                tenderDTO.BudgetRange,
                tenderDTO.EligibilityCriteria,
                int.Parse(tenderDTO.IssuedBy),
                tenderDTO.Location
            );

            var createdTender = await _tenderRepo.CreateTenderAsync(tender);
            return CreatedAtAction(nameof(GetTenderById), new { id = createdTender.Id }, createdTender);
        }

        // Upload a document for a tender
        [HttpPost("uploadTenderDocument/{tenderId}")]
        public async Task<IActionResult> UploadTenderDocument(TenderDocumentDTO tenderDocDto)
        {
            var tender = await _tenderRepo.GetTenderByIdAsync(tenderDocDto.TenderId);
            if (tender == null)
            {
                return NotFound("Tender not found.");
            }

            var user = _userRepo.GetUserById(int.Parse(tender.IssuedBy));
            if (user == null || (user.Role != UserRole.ProcurementOfficer))
            {
                return Unauthorized("You do not have permission to upload a document for this tender.");
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
