using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProjetoMultidiciplinar.DTOs;
using ProjetoMultidiciplinar.Models;
using ProjetoMultidiciplinar.Data;
using ProjetoMultidiciplinar.Services;
using Microsoft.EntityFrameworkCore;

namespace ProjetoMultidiciplinar.Controllers
{
    [Authorize]
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FilesService _fileService;

        public ProductsController(
            AppDbContext context,
            FilesService fileService)
        {
            _context = context;
            _fileService = fileService;
        }


        [HttpPost("announce")]
        public async Task<IActionResult> Announce([FromForm] AnnouceDto annouceData)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var annouce = new ProductsModel
            {
                ID = Guid.NewGuid(),
                AuthorId = Guid.Parse(userId!),
                Title = annouceData.Title,
                Location = annouceData.Location,
                Description = annouceData.Description,
                State = annouceData.State,
                Quantity = annouceData.Quantity,
                Category = annouceData.Category.ToString()
            };

            if (annouceData.Images != null)
            {
                foreach (var image in annouceData.Images)
                {
                    var photoData = await _fileService.SaveFile(image);

                    var photo = new PhotosModel
                    {
                        ID = Guid.NewGuid(),
                        Url = photoData.Url,
                        ProductId = annouce.ID
                    };

                    annouce.Photos.Add(photo);
                }
            }

            _context.Products.Add(annouce);

            await _context.SaveChangesAsync();

            return Ok(annouce);
        }


        [HttpGet("announce/user")]
        public async Task<IActionResult> GetAnnouncesFromUser()
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

            var products = await _context.Products
                .Where(p => p.AuthorId == userGuid)
                .ToListAsync();

            return Ok(products);
        }


        [HttpGet("announces")]
        public async Task<IActionResult> GetAnnounces()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }
    }
}