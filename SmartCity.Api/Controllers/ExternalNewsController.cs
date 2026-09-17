using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Api.DTOs.ExternalNews;
using SmartCity.Api.Models;
using SmartCity.Api.Services;
using System.Security.Claims;

namespace SmartCity.Api.Controllers;

[ApiController]
[Route("api/external-news")]
[Authorize]
public class ExternalNewsController : ControllerBase
{
    private readonly ExternalNewsService _externalNewsService;

    public ExternalNewsController(ExternalNewsService externalNewsService)
    {
        _externalNewsService = externalNewsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetVisible()
    {
        var news = await _externalNewsService.GetVisibleAsync();

        return Ok(news.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var news = await _externalNewsService.GetByIdAsync(id);
        if (news is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(news));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateExternalNewsRequest request)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(adminUserId))
        {
            return Unauthorized();
        }

        try
        {
            var news = await _externalNewsService.CreateAsync(
                adminUserId,
                request.ExternalId,
                request.Title,
                request.Summary,
                request.ImageUrl,
                request.SourceName,
                request.SourceUrl,
                request.Category,
                request.PublishedAt,
                request.ExpiresAt);

            return Created(string.Empty, ToResponse(news));
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }

    private static object ToResponse(ExternalNews news)
    {
        return new
        {
            id = news.Id,
            external_id = news.ExternalId,
            title = news.Title,
            summary = news.Summary,
            image_url = news.ImageUrl,
            source_name = news.SourceName,
            source_url = news.SourceUrl,
            category = news.Category,
            published_at = news.PublishedAt,
            expires_at = news.ExpiresAt,
            is_visible = news.IsVisible,
            inserted_at = news.Audit.InsertedAt
        };
    }
}
