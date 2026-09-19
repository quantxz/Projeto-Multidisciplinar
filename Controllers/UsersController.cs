using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoMultidiciplinar.Data;
using ProjetoMultidiciplinar.DTOs;
using ProjetoMultidiciplinar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetoMultidiciplinar.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProjetoMultidiciplinar.Controllers
{

    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly MessagesService _messagesService;
        private readonly FilesService _fileService;
        public UsersController(AppDbContext context, JwtService jwtService, MessagesService messagesService, FilesService fileService)
        {
            _jwtService = jwtService;
            _context = context;
            _messagesService = messagesService;
            _fileService = fileService;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ID == userGuid);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(user);
        }

        [Authorize]
        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userId, out var userGuid))
            {
                return Forbid();
            }

            var conversations =
                await _messagesService.GetConversations(userGuid);

            return Ok(conversations);
        }

        [HttpPost("register")]
        async public Task<IActionResult> RegisterUser(UserDto User)
        {
            var user = new UsersModel
            {
                ID = Guid.NewGuid(),
                Name = User.Name,
                Email = User.Email,
                Rating = "0"
            };

            var passwordHasher = new PasswordHasher<UsersModel>();

            user.Password = passwordHasher.HashPassword(
                user,
                User.Password
            );

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token = token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var passwordHasher = new PasswordHasher<UsersModel>();

            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                loginDto.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token
            });
        }

        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> updateUser([FromForm] UserUpdateProflieDto userDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Console.WriteLine(userDto);
            if (!Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ID == userGuid);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            if (userDto.Name != null)
            {
                user.Name = userDto.Name;
            }

            if (userDto.Bio != null)
            {
                user.Bio = userDto.Bio;
            }

            if (userDto.Locale != null)
            {
                user.Locale = userDto.Locale;
            }

            if (userDto.PhotoUrl != null)
            {
                var photoData = await _fileService.SaveFile(userDto.PhotoUrl);

                user.PhotoUrl = photoData.Url;
                Console.WriteLine(photoData);
            }
            Console.WriteLine(user.PhotoUrl);


            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
