using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.DataAccess.Repository.IRepository;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
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
            var news = _unitOfWork.News.GetFirstOrDefault(u => u.Id == id, includeProperties: "Author");

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }
    }
}
