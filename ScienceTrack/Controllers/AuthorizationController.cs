﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScienceTrack.Services;
using ScienceTrack.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ScienceTrack.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private AuthorizationService authorizationService;
        public AuthorizationController(AuthorizationService authorizationService) 
        {
            this.authorizationService = authorizationService;
        }
        [HttpPost]
        public async Task<IActionResult> Authorize([FromForm] string userName, [FromForm] string password)
        {
            var user = await authorizationService.Authorize(HttpContext, userName, password);
            if (user == null) 
            {
                return Unauthorized("Wrong authorization");
            }
            return Ok(new UserDTO(user));
        }

        [HttpPost]
        public async Task<IActionResult> IsAuthorize()
        {
            var user = await authorizationService.IsAuthorize(HttpContext);
            if (user == null)
                return Unauthorized();
            return Ok(new UserDTO(user));
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] string userName, [FromForm] string officialName, [FromForm] string password)
        {
            var user = await authorizationService.Register(userName, officialName, password);
            if (user == null)
            {
                return BadRequest("User already exists in the system");
            }
            return Ok(new UserDTO(user));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (await authorizationService.Logout(HttpContext))
                return Ok("You are logged out");
            return Unauthorized();
        }
    }
}
