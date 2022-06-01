using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Task.Data;
using Task.Dtos;
using Task.Models;
using Task.Services;

namespace Task.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserService _service;
        public AuthController(IConfiguration configuration, DataContext context, UserService service)
        {
            _context = context;
            _configuration = configuration;
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<User>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<User>> Create(UserDto request)
        {
            var user = await _service.CreateUser(request);
            if (user == null)
                return BadRequest("User already exists");
            return Ok(user);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return BadRequest("User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User deleted");
        }

        [HttpPut]
        public async Task<ActionResult<User>> Update(UserDto request)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null)
                return BadRequest("User not found");
            user.Email = request.Email;
            user.Firstname = request.Firstname;
            user.Lastname = request.Lastname;
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var user = await _context.Users.FirstAsync(u=>u.Email == request.Email);
            if (user == null)
                return BadRequest("User not found");
            var isValid = _service.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isValid)
                return BadRequest("Wrong password");
            var token = _service.CreateToken(user);
            return Ok(token);
        }

        [HttpPost("check_token")]
        public async Task<ActionResult<User>> CheckToken(string token)
        {
            var email = _service.ValidateToken(token);
            if (email == null)
                return BadRequest("Invalid token");
            var user = await _context.Users.FirstAsync(d => d.Email == email);
            return Ok(user);
        }
        

        
    }
    
}
