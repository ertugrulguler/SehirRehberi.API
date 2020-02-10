using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SehirRehberi.API.Models;
using SehirRehberi.API.Repositories.Entities;
using SehirRehberi.API.ViewModels;

namespace SehirRehberi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthRepository _authRepository;
        private IConfiguration _configuration;
        public AuthController(AuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel userForRegister)
        {
            if (await _authRepository.UserExist(userForRegister.UserName))
            {
                ModelState.AddModelError("UserName", "Username is already exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new User
            {
                Username = userForRegister.UserName
            };

            var createdUser = _authRepository.Register(userToCreate, userForRegister.Password);
            return StatusCode(201);
        }
    }
}