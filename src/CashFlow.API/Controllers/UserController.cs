using CashFlow.Application.UseCases.Users.GetProfile;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Application.UseCases.Users.UpdateProfile;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisteredUserJson request
        )
    {
        var response = await useCase.Execute(request);


        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]

    [Authorize]
    public async Task<IActionResult> UpdateProfile(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}
