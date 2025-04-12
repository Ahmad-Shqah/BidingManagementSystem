This project is a .net 8 core web API system for managing Tenders, Bids and thier evaluations, Users, and their corresponding documents. The solution follows (Domain Drevin Design) as a clean architecture approach, separating responsibilities into layers: Controllers, Repositories with DTOs, and Domain Models,Infrusrtucture layer (database tables manage and relations).
BidingmanagementSystem/
├── Controllers/
│   ├── TenderController
│   ├── BidController
│   └── UserController
├── Repositories/
│   ├── Interfaces/
│   │   ├── ITenderRepo
│   │   ├── IBidRepo
│   │   └── IUserRepo
│   └── Implementations/
│       ├── TenderRepo
│       ├── BidRepo
│       └── UserRepo
├── DTOs/
│   ├── TenderDTO
│   ├── TenderDocumentDTO
│   ├── BidDTO
│   ├── BidDocumentDTO
│   └── UserDTO
├── Domain/
│   ├── TenderDomain/Tendor,TendorDoc
│   ├── BidDomain/Bid,BidDoc
│   ├── User/
|   └──Evaluation
└── Infrustructure/
    ├── SystemDbContext
    ├── configs...
.
.
.
 How It Works
 1. User Management
Create/Get User: Managed via UserController and UserRepo, utilizing UserDTO.

Role Validation: Role-based access Controled,Roles are (e.g., ProcurementOfficer) is handled before performing actions like creating tenders or uploading documents.

2. Tender Management
Create Tender: Handled by TenderController using a TenderDTO object.

Upload Tender Document: File data is sent using TenderDocumentDTO, then saved and linked to the tender by its TenderId.

Tender is the owning entity: All TenderDocument records are owned by and associated with a Tender.

3. Bid Management
Create Bid: BidController receives a BidDTO and creates a bid record associated with a TenderId and UserId.

Upload Bid Document: BidDocumentDTO handles file metadata for uploaded documents.

Evaluate Bid: Bid status can be updated (e.g., "Accepted", "Rejected") based on custom logic.

Bid is owned by a Tender and User: Each bid must be linked to both a tender and a bidding user.



🧪 Technologies Used
.NET 8 (ASP.NET Core Web API)

Entity Framework Core

Microsoft SQL Server for database

Swagger API documentation and test

DTO Pattern for data abstraction

LINQ for queries

Clean Architecture Principles

🔐 Authorization Strategy
Each controller checks user roles before performing sensitive actions (e.g., creating a tender, uploading documents).

 roles are : ProcurementOfficer, Bidder,evaluator.

🧼 Clean Code Practices
Controllers only handle HTTP logic.

Repositories encapsulate all database operations.

DTOs are used for clean data transfer and validation.

No DbContext is injected into controllers — only repositories.

📁 File Upload
Uploaded files are saved to a specified folder (e.g., /uploads) and file metadata is stored in the database via the respective document entities (TenderDocument, BidDocument).
