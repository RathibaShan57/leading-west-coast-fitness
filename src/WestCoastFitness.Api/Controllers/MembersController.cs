using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WestCoastFitness.Application.Members;

namespace WestCoastFitness.Api.Controllers;

[ApiController]
[Route("api/members")]
public sealed class MembersController : ControllerBase
{
    private readonly RegisterMemberHandler _handler;
    private readonly IValidator<RegisterMemberCommand> _validator;

    public MembersController(RegisterMemberHandler handler, IValidator<RegisterMemberCommand> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterMemberCommand command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var member = await _handler.HandleAsync(command, cancellationToken);
            return Created($"/api/members/{member.Id}", member);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
