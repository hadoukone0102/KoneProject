using KoneProject.DTOs;
using KoneProject.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoneProject.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // Constructor
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookServices.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookServices.GetByIdAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBooksDto book)
        {
            var newBooks = await _bookServices.CreateAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = newBooks.Id }, newBooks);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookServices.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
