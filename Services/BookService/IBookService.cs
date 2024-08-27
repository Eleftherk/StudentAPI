using StudentAPI.DTOs;

namespace StudentAPI.Services.BookService
{
    public interface IBookService
    {
        Task<List<BookDTO>?> GetAllBooks();
        Task<BookDTO?> GetBookById(int id);
        Task<List<BookDTO>?> GetBookByTitle(string name);
        Task<BookDTO?> CreateBook(BookCreateDTO book);
        Task<BookDTO?> UpdateBook(int id, BookCreateDTO book);
        Task<List<BookDTO>?> DeleteBook(int id);
        Task<List<EnrollmentForBookDTO>?> EnrollmentsWithBookAfterADate(DateTime? date);


    }
}
