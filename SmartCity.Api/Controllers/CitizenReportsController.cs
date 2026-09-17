using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Api.DTOs.CitizenReports;
using SmartCity.Api.Models;
using SmartCity.Api.Services;
using System.Security.Claims;

namespace SmartCity.Api.Controllers;

[ApiController]
[Route("api/citizen-reports")]
[Authorize]
public class CitizenReportsController : ControllerBase
{
    private readonly CitizenReportService _citizenReportService;
    private readonly ReportConfirmationService _reportConfirmationService;
    private readonly ReportStatusHistoryService _reportStatusHistoryService;

    public CitizenReportsController(
        CitizenReportService citizenReportService,
        ReportConfirmationService reportConfirmationService,
        ReportStatusHistoryService reportStatusHistoryService)
    {
        _citizenReportService = citizenReportService;
        _reportConfirmationService = reportConfirmationService;
        _reportStatusHistoryService = reportStatusHistoryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCitizenReportRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var report = await _citizenReportService.CreateAsync(
            userId,
            request.CategoryId,
            request.Title,
            request.Description,
            request.Longitude,
            request.Latitude,
            request.Address,
            request.District);

        var response = ToResponse(report);

        return Created(string.Empty, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? moderationStatus,
        [FromQuery] string? resolutionStatus,
        [FromQuery] string? priority,
        [FromQuery] string? categoryId,
        [FromQuery] string? district)
    {
        try
        {
            var reports = await _citizenReportService.GetFilteredAsync(
                moderationStatus,
                resolutionStatus,
                priority,
                categoryId,
                district,
                null);

            return Ok(reports.Select(ToResponse));
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy(
        [FromQuery] string? moderationStatus,
        [FromQuery] string? resolutionStatus,
        [FromQuery] string? priority,
        [FromQuery] string? categoryId,
        [FromQuery] string? district)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            var reports = await _citizenReportService.GetFilteredAsync(
                moderationStatus,
                resolutionStatus,
                priority,
                categoryId,
                district,
                userId);

            return Ok(reports.Select(ToResponse));
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var report = await _citizenReportService.GetByIdAsync(id);
        if (report is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(report));
    }

    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> Confirm(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            await _reportConfirmationService.ConfirmAsync(id, userId);
            return Ok(new { message = "Report confirmed successfully." });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}/confirm")]
    public async Task<IActionResult> Unconfirm(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            await _reportConfirmationService.UnconfirmAsync(id, userId);
            return Ok(new { message = "Report confirmation removed successfully." });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateReportStatusRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            await _reportStatusHistoryService.UpdateStatusAsync(id, userId, request.ResolutionStatus, request.Note);
            return Ok(new { message = "Report status updated successfully." });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }

    [HttpGet("{id}/status-history")]
    public async Task<IActionResult> GetStatusHistory(string id)
    {
        try
        {
            var history = await _reportStatusHistoryService.GetHistoryAsync(id);
            return Ok(history.Select(ToHistoryResponse));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/moderation")]
    public async Task<IActionResult> Moderate(string id, [FromBody] ModerateCitizenReportRequest request)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(adminUserId))
        {
            return Unauthorized();
        }

        try
        {
            await _citizenReportService.ModerateAsync(id, adminUserId, request.ModerationStatus);
            return Ok(new { message = "Report moderation updated successfully." });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }

    private static object ToHistoryResponse(ReportStatusHistory history)
    {
        return new
        {
            id = history.Id,
            report_id = history.ReportId,
            previous_status = history.PreviousStatus,
            new_status = history.NewStatus,
            changed_by = history.ChangedBy,
            comment = history.Comment,
            inserted_at = history.Audit.InsertedAt
        };
    }

    private static object ToResponse(CitizenReport report)
    {
        return new
        {
            id = report.Id,
            user_id = report.UserId,
            category_id = report.CategoryId,
            title = report.Title,
            description = report.Description,
            location = new
            {
                longitude = report.Location?.Coordinates.Longitude,
                latitude = report.Location?.Coordinates.Latitude
            },
            address = report.Address,
            district = report.District,
            moderation_status = report.ModerationStatus,
            resolution_status = report.ResolutionStatus,
            priority = report.Priority,
            is_public = report.IsPublic,
            confirmation_count = report.ConfirmationCount,
            inserted_at = report.Audit.InsertedAt
        };
    }
}
