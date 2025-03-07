﻿using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Route("api/users")]
public class AuthenticationController : BaseApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public ActionResult<AuthenticationTokensDto> RegisterTourist([FromBody] AccountRegistrationDto account)
    {
        var result = _authenticationService.RegisterTourist(account);
        return CreateResponse(result);
    }

    [HttpPost("login")]
    public ActionResult<AuthenticationTokensDto> Login([FromBody] CredentialsDto credentials)
    {
        var result = _authenticationService.Login(credentials);
        return CreateResponse(result);
    }

    [HttpGet("username/{userId:int}")]
    public ActionResult<CredentialsDto> GetUsername([FromRoute] int userId)
    {
        var result = _authenticationService.GetUsername(userId);
        return CreateResponse(result);
    }
}