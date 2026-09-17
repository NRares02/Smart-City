using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Api.DTOs.OfficialAnnouncements;
using SmartCity.Api.Models;
using SmartCity.Api.Services;
using System.Security.Claims;

namespace SmartCity.Api.Controllers;

[ApiController]
[Route("api/official-announcements")]
[Authorize]
public class OfficialAnnouncementsController : ControllerBase
{
    private readonly OfficialAnnouncementService _officialAnnouncementService;

    public OfficialAnnouncementsController(OfficialAnnouncementService officialAnnouncementService)
    {
        _officialAnnouncementService = officialAnnouncementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublished()
    {
        var announcements = await _officialAnnouncementService.GetPublishedAsync();

        return Ok(announcements.Select(ToResponse));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateOfficialAnnouncementRequest request)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(adminUserId))
        {
            return Unauthorized();
        }

        var announcement = await _officialAnnouncementService.CreateAsync(
            adminUserId,
            request.Title,
            request.Summary,
            request.Content,
            request.Category,
            request.ImageUrl,
            request.IsPinned,
            request.ExpiresAt);

        return Created(string.Empty, ToResponse(announcement));
    }

    [HttpPatch("{id}/published")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetPublished(string id, [FromBody] UpdateOfficialAnnouncementPublicationRequest request)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(adminUserId))
        {
            return Unauthorized();
        }

        try
        {
            await _officialAnnouncementService.SetPublishedAsync(id, adminUserId, request.IsPublished);
            return Ok(new { message = "Announcement publication status updated successfully." });
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

    private static object ToResponse(OfficialAnnouncement announcement)
    {
        return new
        {
            id = announcement.Id,
            title = announcement.Title,
            summary = announcement.Summary,
            content = announcement.Content,
            category = announcement.Category,
            image_url = announcement.ImageUrl,
            author_id = announcement.AuthorId,
            is_published = announcement.IsPublished,
            is_pinned = announcement.IsPinned,
            expires_at = announcement.ExpiresAt,
            inserted_at = announcement.Audit.InsertedAt
        };
    }
}
