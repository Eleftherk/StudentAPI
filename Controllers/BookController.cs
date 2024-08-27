using Microsoft.AspNetCore.Mvc;
using StudentAPI.DTOs;
using StudentAPI.Services.BookService;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly BookService _bookService;
        public BookController(IBookService bookService) {
            _bookService = (BookService?)bookService;
        }

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<List<BookDTO>>> GetAllBooks()
        {
            var result = await _bookService.GetAllBooks();
            return Ok(result);
        }

        [HttpGet("GetSpesificBookById/{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var result = await _bookService.GetBookById(id);
            if (result is null)
            {
                return NotFound("There is no book with such an Id.");
            }
            return Ok(result);
        }

        [HttpGet("GetSpesificBookByTItle/{title}")]
        public async Task<ActionResult<BookDTO>> GetBookByTitle(string title)
        {
            var result = await _bookService.GetBookByTitle(title);
            if (result is null)
            {
                return NotFound("There is no book with such an Id.");
            }
            return Ok(result);
        }

        [HttpPost("CreateBook")]
        public async Task<ActionResult<BookDTO>> CreateStudent(BookCreateDTO request)
        {
            var result = await _bookService.CreateBook(request);
            return Ok(result);
        }

        [HttpPut("UpdateBook/{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBook(int id, BookCreateDTO request)
        {
            var result = await _bookService.UpdateBook(id, request);
            if (result is null)
            {
                return NotFound("There is no book with such an Id.");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteBook/{id}")]
        public async Task<ActionResult<List<BookDTO>>> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBook(id);
            if (result is null)
            {
                return NotFound("There is no book with such an id.");
            }
            return Ok(result);
        }

    }
}
