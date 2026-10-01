using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Api.DTOs.PointsOfInterest;
using SmartCity.Api.Models;
using SmartCity.Api.Services;
using System.Security.Claims;

namespace SmartCity.Api.Controllers;

[ApiController]
[Route("api/points-of-interest")]
[Authorize]
public class PointsOfInterestController : ControllerBase
{
    private readonly PointOfInterestService _pointOfInterestService;

    public PointsOfInterestController(PointOfInterestService pointOfInterestService)
    {
        _pointOfInterestService = pointOfInterestService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var pointsOfInterest = await _pointOfInterestService.GetActiveAsync();

        return Ok(pointsOfInterest.Select(ToResponse));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(string id)
    {
        var pointOfInterest = await _pointOfInterestService.GetByIdAsync(id);
        if (pointOfInterest is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(pointOfInterest));
    }

    [HttpGet("category/{categoryId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategoryId(string categoryId)
    {
        var pointsOfInterest = await _pointOfInterestService.GetByCategoryIdAsync(categoryId);

        return Ok(pointsOfInterest.Select(ToResponse));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePointOfInterestRequest request)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(adminUserId))
        {
            return Unauthorized();
        }

        try
        {
            var pointOfInterest = await _pointOfInterestService.CreateAsync(
                adminUserId,
                request.CategoryId,
                request.Name,
                request.Description,
                request.Longitude,
                request.Latitude,
                request.Address,
                request.ImageUrl,
                request.PhoneNumber,
                request.WebsiteUrl,
                request.OpeningHours,
                request.ValidFrom,
                request.ValidUntil,
                request.IsTemporary);

            return Created(string.Empty, ToResponse(pointOfInterest));
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

    private static object ToResponse(PointOfInterest pointOfInterest)
    {
        return new
        {
            id = pointOfInterest.Id,
            category_id = pointOfInterest.CategoryId,
            name = pointOfInterest.Name,
            description = pointOfInterest.Description,
            location = new
            {
                longitude = pointOfInterest.Location?.Coordinates.Longitude,
                latitude = pointOfInterest.Location?.Coordinates.Latitude
            },
            address = pointOfInterest.Address,
            image_url = pointOfInterest.ImageUrl,
            phone_number = pointOfInterest.PhoneNumber,
            website_url = pointOfInterest.WebsiteUrl,
            opening_hours = pointOfInterest.OpeningHours,
            valid_from = pointOfInterest.ValidFrom,
            valid_until = pointOfInterest.ValidUntil,
            is_temporary = pointOfInterest.IsTemporary,
            is_active = pointOfInterest.IsActive,
            inserted_at = pointOfInterest.Audit.InsertedAt
        };
    }
}
