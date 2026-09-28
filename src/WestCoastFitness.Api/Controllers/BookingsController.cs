using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WestCoastFitness.Application.Bookings;

namespace WestCoastFitness.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public sealed class BookingsController : ControllerBase
{
    private readonly BookClassHandler _handler;
    private readonly IValidator<BookClassCommand> _validator;

    public BookingsController(BookClassHandler handler, IValidator<BookClassCommand> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Book([FromBody] BookClassCommand command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var booking = await _handler.HandleAsync(command, cancellationToken);
            return Created($"/api/bookings/{booking.Id}", booking);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
