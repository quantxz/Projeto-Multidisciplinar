using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoMultidiciplinar.Services
{
    public class PhotosService
    {
        public async Task<(string FileName, string Url)> SavePhoto(
            IFormFile photo)
        {
            var uploadsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            Directory.CreateDirectory(uploadsPath);

            var extension = Path.GetExtension(photo.FileName);

            var fileName = Guid.NewGuid() + extension;

            var filePath = Path.Combine(
                uploadsPath,
                fileName
            );

            await using var stream = new FileStream(
                filePath,
                FileMode.Create
            );

            await photo.CopyToAsync(stream);
            
            return (
                fileName,
                $"/uploads/{fileName}"
            );
        }
    }
}
