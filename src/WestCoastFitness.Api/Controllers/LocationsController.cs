using Microsoft.AspNetCore.Mvc;
using WestCoastFitness.Application.Catalog;

namespace WestCoastFitness.Api.Controllers;

[ApiController]
[Route("api/locations")]
public sealed class LocationsController : ControllerBase
{
    private readonly CatalogQueries _catalog;

    public LocationsController(CatalogQueries catalog) => _catalog = catalog;

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await _catalog.LocationsAsync(cancellationToken));
}
