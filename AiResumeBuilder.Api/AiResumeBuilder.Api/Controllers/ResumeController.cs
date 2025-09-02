using AiResumeBuilder.Api.Data;
using AiResumeBuilder.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Security.Claims;

namespace AiResumeBuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    public class ResumesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ResumesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ Get all resumes for current user
        [HttpGet("my")]
        public async Task<IActionResult> GetMyResumes()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var resumes = await _db.Resumes
                .Where(r => r.UserId == userId)
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .ToListAsync();

            return Ok(resumes);
        }

        // ✅ Create a new resume
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromBody] Resume resume)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            resume.UserId = userId;
            resume.CreatedAt = DateTime.UtcNow;
            resume.UpdatedAt = DateTime.UtcNow;

            _db.Resumes.Add(resume);
            await _db.SaveChangesAsync();

            return Ok(resume);
        }

        // ✅ Update resume
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResume(int id, [FromBody] Resume updated)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // Load existing resume with relations
            var resume = await _db.Resumes
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (resume == null)
                return NotFound("Resume not found or unauthorized.");

            // ✅ Update basic fields
            resume.Title = updated.Title;
            resume.Summary = updated.Summary;
            resume.UpdatedAt = DateTime.UtcNow;

            // ✅ Update Education
            if (updated.Educations != null)
            {
                // Remove old educations
                _db.Educations.RemoveRange(resume.Educations);

                // Add new ones
                foreach (var edu in updated.Educations)
                {
                    edu.Id = 0; // Ensure EF treats this as new
                    edu.ResumeId = resume.Id;
                    _db.Educations.Add(edu);
                }
            }

            // ✅ Update Experience
            if (updated.Experiences != null)
            {
                // Remove old experiences
                _db.Experiences.RemoveRange(resume.Experiences);

                // Add new ones
                foreach (var exp in updated.Experiences)
                {
                    exp.Id = 0; // Ensure EF treats this as new
                    exp.ResumeId = resume.Id;
                    _db.Experiences.Add(exp);
                }
            }

            await _db.SaveChangesAsync();
            return Ok(resume);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateResume(int id, [FromBody] Resume updated)
        //{
        //    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        //    var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        //    if (resume == null) return NotFound("Resume not found or unauthorized.");

        //    resume.Title = updated.Title;
        //    resume.Summary = updated.Summary;
        //    resume.UpdatedAt = DateTime.UtcNow;

        //    await _db.SaveChangesAsync();
        //    return Ok(resume);
        //}

        // ✅ Add Education
        [HttpPost("{id}/education")]
        public async Task<IActionResult> AddEducation(int id, [FromBody] Education edu)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (resume == null) return NotFound("Resume not found or unauthorized.");

            edu.ResumeId = id;
            _db.Educations.Add(edu);
            await _db.SaveChangesAsync();

            return Ok(edu);
        }

        // ✅ Add Experience
        [HttpPost("{id}/experience")]
        public async Task<IActionResult> AddExperience(int id, [FromBody] Experience exp)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (resume == null) return NotFound("Resume not found or unauthorized.");

            exp.ResumeId = id;
            _db.Experiences.Add(exp);
            await _db.SaveChangesAsync();

            return Ok(exp);
        }

        // ✅ Download Resume as PDF
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadResume(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var resume = await _db.Resumes
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (resume == null) return NotFound("Resume not found or unauthorized.");

            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    page.Header()
                        .Text(resume.Title)
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Summary: {resume.Summary}");

                        // Education
                        if (resume.Educations.Any())
                        {
                            col.Item().Text("Education").FontSize(16).Bold().Underline();
                            foreach (var edu in resume.Educations)
                            {
                                col.Item().Text($"{edu.Degree} in {edu.FieldOfStudy} - {edu.Institution} ({edu.StartYear} - {edu.EndYear})");
                            }
                        }

                        // Experience
                        if (resume.Experiences.Any())
                        {
                            col.Item().Text("Experience").FontSize(16).Bold().Underline();
                            foreach (var exp in resume.Experiences)
                            {
                                col.Item().Text($"{exp.Title} at {exp.Company} ({exp.StartYear} - {exp.EndYear})");
                                col.Item().Text(exp.Description).FontSize(11).Italic();
                            }
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Generated with QuestPDF")
                        .FontSize(10);
                });
            });

            var pdf = document.GeneratePdf();
            return File(pdf, "application/pdf", $"{resume.Title}.pdf");
        }
    }
}

//using AiResumeBuilder.Api.Data;
//using AiResumeBuilder.Api.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;
//using System.Security.Claims;

//namespace AiResumeBuilder.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize(Roles = "User,Admin")]
//    public class ResumesController : ControllerBase
//    {
//        private readonly ApplicationDbContext _db;

//        public ResumesController(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        // ✅ Get all resumes for current user
//        [HttpGet("my")]
//        public async Task<IActionResult> GetMyResumes()
//        {
//            var userId = int.Parse(User.FindFirst("id")!.Value);

//            var resumes = await _db.Resumes
//                .Where(r => r.UserId == userId)
//                .Include(r => r.Educations)
//                .Include(r => r.Experiences)
//                .ToListAsync();

//            return Ok(resumes);
//        }

//        // ✅ Create a new resume
//        [HttpPost]
//        public async Task<IActionResult> CreateResume([FromBody] Resume resume)
//        {
//            var userId = int.Parse(User.FindFirst("id")!.Value);

//            resume.UserId = userId;
//            resume.CreatedAt = DateTime.UtcNow;
//            resume.UpdatedAt = DateTime.UtcNow;

//            _db.Resumes.Add(resume);
//            await _db.SaveChangesAsync();

//            return Ok(resume);
//        }

//        // ✅ Update resume
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateResume(int id, [FromBody] Resume updated)
//        {
//            var userId = int.Parse(User.FindFirst("id")!.Value);

//            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
//            if (resume == null) return NotFound("Resume not found or unauthorized.");

//            resume.Title = updated.Title;
//            resume.Summary = updated.Summary;
//            resume.UpdatedAt = DateTime.UtcNow;

//            await _db.SaveChangesAsync();
//            return Ok(resume);
//        }

//        // ✅ Add Education
//        [HttpPost("{id}/education")]
//        public async Task<IActionResult> AddEducation(int id, [FromBody] Education edu)
//        {
//            var userId = int.Parse(User.FindFirst("id")!.Value);

//            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
//            if (resume == null) return NotFound("Resume not found or unauthorized.");

//            edu.ResumeId = id;
//            _db.Educations.Add(edu);
//            await _db.SaveChangesAsync();

//            return Ok(edu);
//        }

//        // ✅ Add Experience
//        [HttpPost("{id}/experience")]
//        public async Task<IActionResult> AddExperience(int id, [FromBody] Experience exp)
//        {
//            var userId = int.Parse(User.FindFirst("id")!.Value);

//            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
//            if (resume == null) return NotFound("Resume not found or unauthorized.");

//            exp.ResumeId = id;
//            _db.Experiences.Add(exp);
//            await _db.SaveChangesAsync();

//            return Ok(exp);
//        }

//        // ✅ Download Resume as PDF
//        [HttpGet("{id}/download")]
//        public async Task<IActionResult> DownloadResume(int id)
//        {
//            //var userId = int.Parse(User.FindFirst("id")!.Value);
//            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

//            var resume = await _db.Resumes
//                .Include(r => r.Educations)
//                .Include(r => r.Experiences)
//                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

//            if (resume == null) return NotFound("Resume not found or unauthorized.");

//            QuestPDF.Settings.License = LicenseType.Community;

//            var document = Document.Create(container =>
//            {
//                container.Page(page =>
//                {
//                    page.Size(PageSizes.A4);
//                    page.Margin(40);

//                    page.Header()
//                        .Text(resume.Title)
//                        .FontSize(20)
//                        .Bold()
//                        .AlignCenter();

//                    page.Content().Column(col =>
//                    {
//                        col.Spacing(10);

//                        col.Item().Text($"Summary: {resume.Summary}");

//                        // Education
//                        if (resume.Educations.Any())
//                        {
//                            col.Item().Text("Education").FontSize(16).Bold().Underline();
//                            foreach (var edu in resume.Educations)
//                            {
//                                col.Item().Text($"{edu.Degree} in {edu.FieldOfStudy} - {edu.Institution} ({edu.StartYear} - {edu.EndYear})");
//                            }
//                        }

//                        // Experience
//                        if (resume.Experiences.Any())
//                        {
//                            col.Item().Text("Experience").FontSize(16).Bold().Underline();
//                            foreach (var exp in resume.Experiences)
//                            {
//                                col.Item().Text($"{exp.Title} at {exp.Company} ({exp.StartYear} - {exp.EndYear})");
//                                col.Item().Text(exp.Description).FontSize(11).Italic();
//                            }
//                        }
//                    });

//                    page.Footer()
//                        .AlignCenter()
//                        .Text("Generated with QuestPDF")
//                        .FontSize(10);
//                });
//            });

//            var pdf = document.GeneratePdf();
//            return File(pdf, "application/pdf", $"{resume.Title}.pdf");
//        }
//    }
//}

////using AiResumeBuilder.Api.Data;
////using AiResumeBuilder.Api.Models;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using QuestPDF.Fluent;
////using QuestPDF.Helpers;
////using QuestPDF.Infrastructure;

////namespace AiResumeBuilder.Api.Controllers
////{
////    [ApiController]
////    [Route("api/[controller]")]
////    [Authorize(Roles = "User,Admin")]
////    public class ResumesController : ControllerBase
////    {
////        private readonly ApplicationDbContext _db;

////        public ResumesController(ApplicationDbContext db)
////        {
////            _db = db;
////        }

////        // ✅ Get all resumes for current user
////        [HttpGet("my")]
////        public async Task<IActionResult> GetMyResumes()
////        {
////            var userId = int.Parse(User.FindFirst("id")!.Value);

////            var resumes = await _db.Resumes
////                .Where(r => r.UserId == userId)
////                .Include(r => r.Educations)
////                .Include(r => r.Experiences)
////                .ToListAsync();

////            return Ok(resumes);
////        }

////        // ✅ Create a new resume
////        [HttpPost]
////        public async Task<IActionResult> CreateResume([FromBody] Resume resume)
////        {
////            resume.UserId = int.Parse(User.FindFirst("id")!.Value);
////            resume.CreatedAt = DateTime.UtcNow;
////            resume.UpdatedAt = DateTime.UtcNow;

////            _db.Resumes.Add(resume);
////            await _db.SaveChangesAsync();

////            return Ok(resume);
////        }

////        // ✅ Update resume
////        [HttpPut("{id}")]
////        public async Task<IActionResult> UpdateResume(int id, [FromBody] Resume updated)
////        {
////            var resume = await _db.Resumes.FindAsync(id);
////            if (resume == null) return NotFound();

////            resume.Title = updated.Title;
////            resume.Summary = updated.Summary;
////            resume.UpdatedAt = DateTime.UtcNow;

////            await _db.SaveChangesAsync();
////            return Ok(resume);
////        }

////        // ✅ Add Education
////        [HttpPost("{id}/education")]
////        public async Task<IActionResult> AddEducation(int id, [FromBody] Education edu)
////        {
////            var resume = await _db.Resumes.FindAsync(id);
////            if (resume == null) return NotFound();

////            edu.ResumeId = id;
////            _db.Educations.Add(edu);
////            await _db.SaveChangesAsync();

////            return Ok(edu);
////        }

////        // ✅ Add Experience
////        [HttpPost("{id}/experience")]
////        public async Task<IActionResult> AddExperience(int id, [FromBody] Experience exp)
////        {
////            var resume = await _db.Resumes.FindAsync(id);
////            if (resume == null) return NotFound();

////            exp.ResumeId = id;
////            _db.Experiences.Add(exp);
////            await _db.SaveChangesAsync();

////            return Ok(exp);
////        }

////        // ✅ Download Resume as PDF
////        [HttpGet("{id}/download")]
////        public async Task<IActionResult> DownloadResume(int id)
////        {
////            var resume = await _db.Resumes
////                .Include(r => r.Educations)
////                .Include(r => r.Experiences)
////                .FirstOrDefaultAsync(r => r.Id == id);

////            if (resume == null) return NotFound();

////            QuestPDF.Settings.License = LicenseType.Community;

////            var document = Document.Create(container =>
////            {
////                container.Page(page =>
////                {
////                    page.Size(PageSizes.A4);
////                    page.Margin(40);

////                    page.Header()
////                        .Text(resume.Title)
////                        .FontSize(20)
////                        .Bold()
////                        .AlignCenter();

////                    page.Content().Column(col =>
////                    {
////                        col.Spacing(10);

////                        col.Item().Text($"Summary: {resume.Summary}");

////                        // Education
////                        if (resume.Educations.Any())
////                        {
////                            col.Item().Text("Education").FontSize(16).Bold().Underline();
////                            foreach (var edu in resume.Educations)
////                            {
////                                col.Item().Text($"{edu.Degree} in {edu.FieldOfStudy} - {edu.Institution} ({edu.StartYear} - {edu.EndYear})");
////                            }
////                        }

////                        // Experience
////                        if (resume.Experiences.Any())
////                        {
////                            col.Item().Text("Experience").FontSize(16).Bold().Underline();
////                            foreach (var exp in resume.Experiences)
////                            {
////                                col.Item().Text($"{exp.Title} at {exp.Company} ({exp.StartYear} - {exp.EndYear})");
////                                col.Item().Text(exp.Description).FontSize(11).Italic();
////                            }
////                        }
////                    });

////                    page.Footer()
////                        .AlignCenter()
////                        .Text("Generated with QuestPDF")
////                        .FontSize(10);
////                });
////            });

////            var pdf = document.GeneratePdf();
////            return File(pdf, "application/pdf", $"{resume.Title}.pdf");
////        }
////    }
////}

//////using AiResumeBuilder.Api.Data;
//////using AiResumeBuilder.Api.Models;
//////using Microsoft.AspNetCore.Authorization;
//////using Microsoft.AspNetCore.Mvc;
//////using QuestPDF.Fluent;
//////using QuestPDF.Helpers;
//////using QuestPDF.Infrastructure;

//////[ApiController]
//////[Route("api/[controller]")]
//////[Authorize(Roles = "User,Admin")]
//////public class ResumesController : ControllerBase
//////{
//////    private readonly ApplicationDbContext _db;
//////    public ResumesController(ApplicationDbContext db) => _db = db;

//////    // Create resume
//////    [HttpPost]
//////    public async Task<IActionResult> CreateResume([FromBody] Resume resume)
//////    {
//////        _db.Resumes.Add(resume);
//////        await _db.SaveChangesAsync();
//////        return Ok(resume);
//////    }

//////    // Edit resume
//////    [HttpPut("{id}")]
//////    public async Task<IActionResult> EditResume(int id, [FromBody] Resume updated)
//////    {
//////        var resume = await _db.Resumes.FindAsync(id);
//////        if (resume == null) return NotFound();

//////        resume.Title = updated.Title;
//////        resume.Summary = updated.Summary;
//////        resume.UpdatedAt = DateTime.UtcNow;
//////        await _db.SaveChangesAsync();

//////        return Ok(resume);
//////    }

//////    // ✅ Generate and download resume as PDF
//////    [HttpGet("{id}/download")]
//////    public async Task<IActionResult> DownloadResume(int id)
//////    {
//////        var resume = await _db.Resumes.FindAsync(id);
//////        if (resume == null) return NotFound();

//////        var pdf = Document.Create(container =>
//////        {
//////            container.Page(page =>
//////            {
//////                page.Margin(40);

//////                page.Header()
//////                    .Text($"Resume - {resume.Title}")
//////                    .FontSize(20)
//////                    .Bold();

//////                page.Content().Column(col =>
//////                {
//////                    col.Item().Text($"User ID: {resume.UserId}");
//////                    col.Item().Text($"Title: {resume.Title}");
//////                    col.Item().Text($"Summary: {resume.Summary}");
//////                    col.Item().Text($"Created: {resume.CreatedAt}");
//////                    col.Item().Text($"Updated: {resume.UpdatedAt}");
//////                });

//////                page.Footer()
//////                    .AlignCenter()
//////                    .Text(x => x.Span("Generated with QuestPDF").FontSize(10));
//////            });
//////        }).GeneratePdf();

//////        return File(pdf, "application/pdf", $"{resume.Title}.pdf");
//////    }
//////}

//////using AiResumeBuilder.Api.DTOs;
//////using AiResumeBuilder.Api.Models;
//////using AiResumeBuilder.Api.Services;
//////using Microsoft.AspNetCore.Authorization;
//////using Microsoft.AspNetCore.Mvc;

//////namespace AiResumeBuilder.Api.Controllers
//////{
//////    [ApiController]
//////    [Route("api/[controller]")]
//////    [Authorize(Roles = "User,Admin")]
//////    public class ResumesController : ControllerBase
//////    {
//////        private readonly IResumeService _service;
//////        public ResumesController(IResumeService service) => _service = service;

//////        [HttpPost]
//////        public async Task<IActionResult> Create(ResumeCreateDto dto)
//////        {
//////            var model = new Resume
//////            {
//////                UserId = dto.UserId,
//////                Title = dto.Title,
//////                Summary = dto.Summary
//////            };
//////            var created = await _service.CreateAsync(model);
//////            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//////        }

//////        [HttpGet("{id}")]
//////        [AllowAnonymous]
//////        public async Task<IActionResult> GetById(int id)
//////        {
//////            var r = await _service.GetByIdAsync(id);
//////            if (r == null) return NotFound();
//////            return Ok(r);
//////        }

//////        [HttpGet("user/{userId}")]
//////        public async Task<IActionResult> GetByUser(int userId)
//////        {
//////            var list = await _service.GetByUserIdAsync(userId);
//////            return Ok(list);
//////        }

//////        [HttpPut("{id}")]
//////        public async Task<IActionResult> Update(int id, ResumeUpdateDto dto)
//////        {
//////            var r = await _service.GetByIdAsync(id);
//////            if (r == null) return NotFound();

//////            r.Title = dto.Title;
//////            r.Summary = dto.Summary;
//////            r.UpdatedAt = DateTime.UtcNow;
//////            await _service.UpdateAsync(r);

//////            return NoContent();
//////        }

//////        [HttpDelete("{id}")]
//////        public async Task<IActionResult> Delete(int id)
//////        {
//////            await _service.DeleteAsync(id);
//////            return NoContent();
//////        }
//////    }
//////}

//////using AiResumeBuilder.Api.Data;
//////using AiResumeBuilder.Api.Models;
//////using Microsoft.AspNetCore.Authorization;
//////using Microsoft.AspNetCore.Mvc;

//////[ApiController]
//////[Route("api/[controller]")]
//////[Authorize(Roles = "User,Admin")] // ✅ Both User & Admin allowed
//////public class ResumesController : ControllerBase
//////{
//////    private readonly ApplicationDbContext _db;
//////    public ResumesController(ApplicationDbContext db) => _db = db;

//////    // Create resume
//////    [HttpPost]
//////    public async Task<IActionResult> CreateResume([FromBody] Resume resume)
//////    {
//////        _db.Resumes.Add(resume);
//////        await _db.SaveChangesAsync();
//////        return Ok(resume);
//////    }

//////    // Edit resume
//////    [HttpPut("{id}")]
//////    public async Task<IActionResult> EditResume(int id, [FromBody] Resume updated)
//////    {
//////        var resume = await _db.Resumes.FindAsync(id);
//////        if (resume == null) return NotFound();

//////        resume.Title = updated.Title;
//////        resume.Summary = updated.Summary;
//////        resume.UpdatedAt = DateTime.UtcNow;
//////        await _db.SaveChangesAsync();

//////        return Ok(resume);
//////    }

//////    // Download as PDF
//////    [HttpGet("{id}/download")]
//////    public async Task<IActionResult> DownloadResume(int id)
//////    {
//////        var resume = await _db.Resumes.FindAsync(id);
//////        if (resume == null) return NotFound();

//////        // Stub PDF generation
//////        var fileBytes = System.Text.Encoding.UTF8.GetBytes($"Resume: {resume.Title}");
//////        return File(fileBytes, "application/pdf", $"{resume.Title}.pdf");
//////    }
//////}

