using imageKeeper.Data;
using imageKeeper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace imageKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly ApplicationDbContext _context;

        public ImagesController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {
            var user = await _userManager.GetUserAsync(User);

            if (imageFile != null && imageFile.Length > 0)
            {
                // Генерируем уникальное имя файла
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                // Путь к папке для сохранения изображений, предварительно определенный в appsettings.json
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Полный путь к файлу изображения
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Создаем запись об изображении в базе данных
                var image = new Image
                {
                    FilePath = uniqueFileName,
                    UserId = user.Id
                };

                // Сохраняем изображение в базе данных
                _context.Images.Add(image);
                _context.SaveChanges();

                return Ok("Image uploaded successfully");
            }
            else
            {
                return BadRequest("Image file is empty or invalid");
            }
        }
    }
}
