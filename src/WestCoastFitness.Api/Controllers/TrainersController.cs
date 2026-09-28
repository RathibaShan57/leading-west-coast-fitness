using Microsoft.AspNetCore.Mvc;
using WestCoastFitness.Application.Catalog;

namespace WestCoastFitness.Api.Controllers;

[ApiController]
[Route("api/trainers")]
public sealed class TrainersController : ControllerBase
{
    private readonly CatalogQueries _catalog;

    public TrainersController(CatalogQueries catalog) => _catalog = catalog;

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await _catalog.TrainersAsync(cancellationToken));
}
