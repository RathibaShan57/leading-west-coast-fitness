using Microsoft.AspNetCore.Mvc;
using WestCoastFitness.Application.Catalog;

namespace WestCoastFitness.Api.Controllers;

[ApiController]
[Route("api/sessions")]
public sealed class SessionsController : ControllerBase
{
    private readonly CatalogQueries _catalog;

    public SessionsController(CatalogQueries catalog) => _catalog = catalog;

    [HttpGet]
    public async Task<IActionResult> Upcoming(CancellationToken cancellationToken) =>
        Ok(await _catalog.UpcomingAsync(cancellationToken));
}
