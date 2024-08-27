using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.DTOs;
using StudentAPI.Models;

namespace StudentAPI.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly DataContext _context;
        public BookService(DataContext context)
        {
            _context = context;
        }
        public async Task<BookDTO?> CreateBook(BookCreateDTO book)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Name == book.SubjectName);
            if (subject == null)
            {
                return null;
            }
            var NewBook = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Subject = subject
            };
            var existedBook = await _context.Books.Include(c=>c.Subject).FirstOrDefaultAsync(c => c.Author == NewBook.Author && 
            c.Description == NewBook.Description && c.Subject ==  NewBook.Subject );
            if (existedBook != null ) {
                var retBook = new BookDTO()
                {
                    Id=existedBook.Id,
                    Title = existedBook.Title,
                    Description = existedBook.Description,
                    Author = existedBook.Author,
                    SubjectName = existedBook.Subject.Name
                };

                return retBook;
            }
            await _context.AddAsync(NewBook);
            await _context.SaveChangesAsync();
            Book Book = await _context.Books.FirstOrDefaultAsync(c => c.Title == book.Title && c.Author == book.Author && c.Description == book.Description && c.Subject.Name == book.SubjectName);
            if (Book == null)
            {
                return null;
            }
            BookDTO returnBook = new BookDTO()
            {
                Id = Book.Id,
                Title = Book.Title,
                Description = Book.Description,
                Author = Book.Author,
                SubjectName = Book.Subject.Name
            };
            return returnBook;

        }

        public async Task<List<BookDTO>?> DeleteBook(int id)
        {
            var book = _context.Books.Include(c => c.Subject).FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                return null;
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            List<BookDTO> bookList = new List<BookDTO>();
            foreach (var book1 in await _context.Books.Include(c => c.Subject).ToListAsync())
            {
                bookList.Add(new BookDTO()
                {
                    Id = book1.Id,
                    Title = book1.Title,
                    Description = book1.Description,
                    Author = book1.Author,
                    SubjectName = book1.Subject.Name
                });
            }
            return bookList;
        }

        public async Task<List<BookDTO>?> GetAllBooks()
        {
            var books = await _context.Books.Include(c => c.Subject).ToListAsync();
            if (books == null)
            {
                return null;
            }
            List<BookDTO> booklist = new List<BookDTO>();
            foreach (var book in books)
            {
                booklist.Add(new BookDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Author = book.Author,
                    SubjectName = book.Subject.Name
                });
            }
            return booklist;
        }

        public async Task<BookDTO?> GetBookById(int id)
        {
            var book = await _context.Books.Include(c => c.Subject).FirstOrDefaultAsync(c => c.Id == id);
            if (book == null)
            {
                return null;
            }
            BookDTO returnbook = new BookDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                SubjectName = book.Subject.Name
            };
            return returnbook;
        }

        public async Task<List<BookDTO>?> GetBookByTitle(string title)
        {
            var bookList = await _context.Books.Include(c => c.Subject).Where(c => c.Title == title).ToListAsync();
            if (bookList == null)
            {
                return null;
            }
            List<BookDTO> response = new List<BookDTO>();
            foreach (var book in bookList) {
                response.Add(new BookDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Author = book.Author,
                    SubjectName = book.Subject.Name
                });
            }
            return response;
        }

        public async Task<BookDTO?> UpdateBook(int id, BookCreateDTO book)
        {
            var book1 = await _context.Books.Include(c => c.Subject).FirstOrDefaultAsync(c => c.Id == id);
            if (book1 == null)
            {
                return null;
            }
            book1.Author = book.Author;
            book1.Description = book.Description;
            book1.Title = book.Title;
            book1.Subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Name == book.SubjectName);
            await _context.SaveChangesAsync();
            book1 = await _context.Books.FirstOrDefaultAsync(c => c.Id == id);

            BookDTO returnbook = new BookDTO()
            {
                Id = book1.Id,
                Title = book1.Title,
                Description = book1.Description,
                Author = book1.Author,
                SubjectName = book1.Subject.Name
            };
            return returnbook;

        }
        public async Task<List<EnrollmentForBookDTO>?> EnrollmentsWithBookAfterADate(DateTime? date)
        {
            var enrollments = await _context.Enrollments.Include(x=>x.Student).Include(x=>x.Book).Where(c => c.DateOfChoosingBook != null && c.DateOfChoosingBook>date).ToListAsync();
            if (enrollments == null)
            {
                return null;
            }          
            List<EnrollmentForBookDTO> response = new List<EnrollmentForBookDTO>();
            foreach (var enrollment in enrollments)
            {
                response.Add(new EnrollmentForBookDTO()
                {
                    enrolmentId = enrollment.EnrollmentId,
                    student = enrollment.Student,
                    dateOfChoosingBook = enrollment.DateOfChoosingBook,
                    book = enrollment.Book
                });
            }
            return response;
        }
    }
}
