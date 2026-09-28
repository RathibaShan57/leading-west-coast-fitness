using Microsoft.AspNetCore.Mvc;
using WestCoastFitness.Application.Catalog;

namespace WestCoastFitness.Api.Controllers;

[ApiController]
[Route("api/plans")]
public sealed class PlansController : ControllerBase
{
    private readonly CatalogQueries _catalog;

    public PlansController(CatalogQueries catalog) => _catalog = catalog;

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await _catalog.PlansAsync(cancellationToken));
}
