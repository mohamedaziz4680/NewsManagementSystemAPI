using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DataAccess.Repository.IRepository;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return Ok(_unitOfWork.Author.GetAll());
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author =  _unitOfWork.Author.GetFirstOrDefault(u => u.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            _unitOfWork.Author.Add(author);
             _unitOfWork.Save();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }
            var authorToUpdate =  _unitOfWork.Author.GetFirstOrDefault(u=> u.Id == id);
            if (authorToUpdate != null) 
            {
                authorToUpdate.Name = author.Name;
            }

            _unitOfWork.Author.Update(authorToUpdate);

            try
            {
                 _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = _unitOfWork.Author.GetFirstOrDefault(u => u.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            _unitOfWork.Author.Remove(author);
            _unitOfWork.Save();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            bool authorExists = false;
            var author = _unitOfWork.Author.GetFirstOrDefault(u=>u.Id == id);
            if (author != null)
            {
                authorExists = true;
            }
            return authorExists;
        }
    }
}
