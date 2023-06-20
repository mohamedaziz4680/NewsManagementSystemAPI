using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DataAccess.Repository.IRepository;
using NewsManagementSystem.Models;
using NewsManagementSystem.Models.ViewModels;

namespace NewsManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            return Ok(_unitOfWork.News.GetAll(null, includeProperties: "Author"));
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            var news =  _unitOfWork.News.GetFirstOrDefault(u => u.Id == id, includeProperties: "Author");

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        // POST: api/News
        [HttpPost]
        public async Task<IActionResult> CreateNews([FromForm] NewsViewModel news)
        {
            if (ModelState.IsValid)
            {
                // Validate publication date
                DateTime currentDate = DateTime.Today;
                DateTime oneWeekFromToday = currentDate.AddDays(7);

                if (news.PublicationDate < currentDate || news.PublicationDate > oneWeekFromToday)
                {
                    ModelState.AddModelError("PublicationDate", "Publication date must be between today and a week from today.");
                    return BadRequest(ModelState);
                }

                // Convert the image data to byte array
                byte[] imageData = await GetImageDataFromRequest(news.ImageData);

                // Create the news entity
                var newsToAdd = new News
                {
                    Title = news.Title,
                    AuthorId = news.AuthorId,
                    Content = news.Content,
                    ImageData = imageData,
                    PublicationDate = news.PublicationDate,
                    CreationDate = DateTime.UtcNow
                };

                // Save the news entity to the database
                _unitOfWork.News.Add(newsToAdd);
                _unitOfWork.Save();

                return CreatedAtAction(nameof(GetNews), new { id = newsToAdd.Id }, newsToAdd);
            }
            return BadRequest();
        }

        // PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] NewsViewModel news)
        {
            if (id != news.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Validate publication date
                DateTime currentDate = DateTime.Today;
                DateTime oneWeekFromToday = currentDate.AddDays(7);

                if (news.PublicationDate < currentDate || news.PublicationDate > oneWeekFromToday)
                {
                    ModelState.AddModelError("PublicationDate", "Publication date must be between today and a week from today.");
                    return BadRequest(ModelState);
                }

                var newsToUpdate =  _unitOfWork.News.GetFirstOrDefault(u=> u.Id==id);

                if (newsToUpdate == null)
                {
                    return NotFound();
                }

                // Update the news entity
                newsToUpdate.Title = news.Title;
                newsToUpdate.AuthorId = news.AuthorId;
                newsToUpdate.Content = news.Content;
                newsToUpdate.PublicationDate = news.PublicationDate;

                // Check if a new image was provided
                if (news.ImageData != null)
                {
                    // Convert the new image data to byte array
                    byte[] newImageData = await GetImageDataFromRequest(news.ImageData);
                    newsToUpdate.ImageData = newImageData;
                }

                // Save the updated news entity to the database
                _unitOfWork.News.Update(newsToUpdate);
                _unitOfWork.Save();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var news = _unitOfWork.News.GetFirstOrDefault(u => u.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            _unitOfWork.News.Remove(news);
            _unitOfWork.Save();

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            bool newsExists = false;
            var news = _unitOfWork.News.GetFirstOrDefault(u=>u.Id == id);
            if (news != null)
            {
                newsExists = true;
            }
            return newsExists;
        }

        private async Task<byte[]> GetImageDataFromRequest(IFormFile imageFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
