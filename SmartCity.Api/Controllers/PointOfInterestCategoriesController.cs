using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Controllers;

[ApiController]
[Route("api/point-of-interest-categories")]
[Authorize]
public class PointOfInterestCategoriesController : ControllerBase
{
    private readonly PointOfInterestCategoryRepository _pointOfInterestCategoryRepository;

    public PointOfInterestCategoriesController(PointOfInterestCategoryRepository pointOfInterestCategoryRepository)
    {
        _pointOfInterestCategoryRepository = pointOfInterestCategoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var categories = await _pointOfInterestCategoryRepository.GetActiveAsync();

        return Ok(categories.Select(ToResponse));
    }

    private static object ToResponse(PointOfInterestCategory category)
    {
        return new
        {
            id = category.Id,
            name = category.Name,
            description = category.Description,
            icon = category.Icon,
            marker_color = category.MarkerColor,
            display_order = category.DisplayOrder
        };
    }
}
