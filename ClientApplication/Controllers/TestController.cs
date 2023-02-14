﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System.Security.Claims;
using Models;

namespace ServiceApplication.Controllers
{
    public class TestController : ControllerBase
    {

        [Authorize]
        [HttpGet("getRoles")]
        public IActionResult GetRoles()
        {
            // Find all our role claims
            var claims = User.FindAll(ClaimTypes.Role);

            var items = new List<string>();

            foreach (var claim in claims)
            {
                items.Add($"Type: {claim.Type} Value: {claim.Value}");
            }

            // Return a list of all role claims
            return Ok(items);
        }


        [Authorize(Policy = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Admin only here");
        }
    }
}


