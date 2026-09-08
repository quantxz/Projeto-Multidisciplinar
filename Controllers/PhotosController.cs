using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoMultidiciplinar.Controllers
{
    [Authorize]
    [ApiController]
    [Route("images")]
    public class PhotosController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {

            if (Request.ContentLength == null || Request.ContentLength == 0)
            {
                return BadRequest("Nenhuma imagem foi enviada.");
            }

            var uploadsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            Directory.CreateDirectory(uploadsPath);

            var fileName = Guid.NewGuid() + ".jpg";

            var filePath = Path.Combine(
                uploadsPath,
                fileName
            );

            await using var stream = new FileStream(
                filePath,
                FileMode.Create
            );

            await Request.Body.CopyToAsync(stream);

            return Ok(new
            {
                fileName,
                url = $"/uploads/{fileName}"
            });
        }
    }
}