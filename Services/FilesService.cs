using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


// EU sei que salvar as fotos no proprio programa não é uma boa escolha mas fds, isso aqui  é só pro projeto escolar
namespace ProjetoMultidiciplinar.Services
{
    public class FilesService
    {
        public async Task<(string FileName, string Url)> SaveFile(
            IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                throw new ArgumentException("Arquivo inválido.");

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
