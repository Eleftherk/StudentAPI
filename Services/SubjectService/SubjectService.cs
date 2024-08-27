using Azure.Core;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.DTOs;
using StudentAPI.Migrations;
using StudentAPI.Models;

namespace StudentAPI.Services.SubjectService
{
    public class SubjectService : ISubjectService
    {
        private readonly DataContext _context;
        public SubjectService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SubjectReturnDTO>> CreateSubject(SubjectCreateDTO request)
        {
            var newSubject = new Subject()
            {
                Name = request.Name,
                ECTS = request.ECTS
            };
            var subjects = await _context.Subjects.Include(c => c.Books).Include(c=>c.Enrollments).ThenInclude(c=>c.Student).ToListAsync();
            if ( subjects.FirstOrDefault(x => x.Name == request.Name) == null)
            {
                _context.Subjects.Add(newSubject);
                await _context.SaveChangesAsync();
            }
            List<SubjectReturnDTO> response = new List<SubjectReturnDTO>(); 
            foreach (var subject in subjects)
            {
                List<EnrollmentForSubjectDTO> enrollmentForSubjectDTOs = new List<EnrollmentForSubjectDTO>();
                foreach (var enrollment in subject.Enrollments)
                {
                    enrollmentForSubjectDTOs.Add(new EnrollmentForSubjectDTO(enrollment.Student.FirstName,
                        enrollment.Student.LastName, enrollment.Student.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed));
                }
                List<BookDTO> books = new List<BookDTO>();
                foreach (var book in subject.Books)
                {
                    books.Add(new BookDTO()
                    {
                        Id = book.Id,
                        Author = book.Author,
                        Description = book.Description,
                        Title = book.Title,
                        SubjectName = book.Subject.Name
                    });
                    
                }
                response.Add(new SubjectReturnDTO()
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    ECTS = subject.ECTS,
                    Enrollments = enrollmentForSubjectDTOs,
                    Books = books
                });
            }

            return response;
        }

        public async Task<List<SubjectReturnDTO>?> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject is null)
            {
                return null;
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            var subjects = await _context.Subjects.Include(c=>c.Enrollments).ThenInclude(c=>c.Student).Include(c=>c.Books).ToListAsync();

            List<SubjectReturnDTO> response = new List<SubjectReturnDTO>();
            foreach (var sub in subjects)
            {
                List<EnrollmentForSubjectDTO> enrollmentForSubjectDTOs = new List<EnrollmentForSubjectDTO>();
                foreach (var enrollment in sub.Enrollments)
                {
                    enrollmentForSubjectDTOs.Add(new EnrollmentForSubjectDTO(enrollment.Student.FirstName,
                        enrollment.Student.LastName, enrollment.Student.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed));
                }
                List<BookDTO> books = new List<BookDTO>();
                foreach (var book in sub.Books)
                {
                    books.Add(new BookDTO()
                    {
                        Id = book.Id,
                        Author = book.Author,
                        Description = book.Description,
                        Title = book.Title,
                        SubjectName = book.Subject.Name
                    });

                }
                response.Add(new SubjectReturnDTO()
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    ECTS = sub.ECTS,
                    Enrollments = enrollmentForSubjectDTOs,
                    Books = books
                });
            }

            return response;
        }

        public async Task<List<SubjectReturnDTO>> GetAllSubjects()
        {
            var subjects = await _context.Subjects.Include(c => c.Enrollments).ThenInclude(c => c.Student).Include(c=>c.Books).ToListAsync();
            List<SubjectReturnDTO> result = new List<SubjectReturnDTO>();
            for (int i = 0; i < subjects.Count; i++)
            {
                List<EnrollmentForSubjectDTO> enrollmentForSubjectDTOs = new List<EnrollmentForSubjectDTO>();
                foreach (var enrollment in subjects[i].Enrollments){
                    enrollmentForSubjectDTOs.Add(new EnrollmentForSubjectDTO(enrollment.Student.FirstName,
                        enrollment.Student.LastName,enrollment.Student.Id,enrollment.EnrollmentId,enrollment.Grade,enrollment.Passed));

                }
                List<BookDTO> books = new List<BookDTO>();
                foreach (var book in subjects[i].Books)
                {
                    books.Add(new BookDTO()
                    {
                        Id = book.Id,
                        Author = book.Author,
                        Description = book.Description,
                        Title = book.Title,
                        SubjectName = book.Subject.Name
                    });

                }
                result.Add(new SubjectReturnDTO(){
                    Id = subjects[i].Id,
                    Name = subjects[i].Name,
                    ECTS = subjects[i].ECTS,
                    Books = books,
                    Enrollments = enrollmentForSubjectDTOs,
                });
            }
            
            return result;
        }

        public async Task<SubjectReturnDTO?> GetSpecificSubjectFromName(String Name)
        {
            var subject = await _context.Subjects.Include(c => c.Enrollments).ThenInclude(c => c.Student).Include(c=>c.Books).FirstOrDefaultAsync(x=>x.Name==Name);

            if (subject is null)
            {
                return null;
            }
            
            List<EnrollmentForSubjectDTO> enrollmentForSubjectDTOs = new List<EnrollmentForSubjectDTO>();
            foreach (var enrollment in subject.Enrollments)
            {
                enrollmentForSubjectDTOs.Add(new EnrollmentForSubjectDTO(enrollment.Student.FirstName,
                    enrollment.Student.LastName, enrollment.Student.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed));

            }
            List<BookDTO> books = new List<BookDTO>();
            foreach (var book in subject.Books)
            {
                books.Add(new BookDTO()
                {
                    Id = book.Id,
                    Author = book.Author,
                    Description = book.Description,
                    Title = book.Title,
                    SubjectName = book.Subject.Name
                });

            }
            
            var subjectDTO = new SubjectReturnDTO()
            {
                Id = subject.Id,
                Name = subject.Name,
                ECTS = subject.ECTS,
                Books = books,
                Enrollments = enrollmentForSubjectDTOs,
            };

            return subjectDTO;
        }

        public async Task<SubjectReturnDTO?> GetSpecificSubjectFromId(int id)
        {
            var subject = await _context.Subjects.Include(c => c.Enrollments).ThenInclude(c => c.Student).Include(c=>c.Books).FirstOrDefaultAsync(x => x.Id == id);

            if (subject is null)
            {
                return null;
            }
            List<EnrollmentForSubjectDTO> enrollmentForSubjectDTOs = new List<EnrollmentForSubjectDTO>();
            foreach (var enrollment in subject.Enrollments)
            {
                enrollmentForSubjectDTOs.Add(new EnrollmentForSubjectDTO(enrollment.Student.FirstName,
                    enrollment.Student.LastName, enrollment.Student.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed));

            }
            List<BookDTO> books = new List<BookDTO>();
            foreach (var book in subject.Books)
            {
                books.Add(new BookDTO()
                {
                    Id = book.Id,
                    Author = book.Author,
                    Description = book.Description,
                    Title = book.Title,
                    SubjectName = book.Subject.Name
                });

            }

            var subjectDTO = new SubjectReturnDTO()
            {
                Id = subject.Id,
                Name = subject.Name,
                ECTS = subject.ECTS,
                Books = books,
                Enrollments = enrollmentForSubjectDTOs,
            };

            return subjectDTO;
        }

        public async Task<List<SubjectReturnDTO>?> UpdateSubject(int id, SubjectCreateDTO request)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == id);
            if (subject is null)
            {
                return null;
            }
            var subjects = _context.Subjects;
            if (await subjects.FirstOrDefaultAsync(x => x.Name == request.Name && x.Id != id) == null)
            {
                subject.Name = request.Name;
                subject.ECTS = request.ECTS;
                await _context.SaveChangesAsync();
            }

            return await GetAllSubjects();
        }

        public async Task<List<Subject>?> DeleteSubjectByName(string name)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Name == name);
            if (subject is null)
            {
                return null;
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return await _context.Subjects.ToListAsync();
        }

        public async Task<List<Subject>?> SetECTS(int id, int ECTS)
        {

            var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == id);
            if (subject is null)
            {
                return null;
            }
            subject.ECTS = ECTS;
            await _context.SaveChangesAsync();
            return await _context.Subjects.ToListAsync();
        }
        public async Task<List<Subject>?> SetECTSFromName(String Name, int ECTS)
        {

            var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Name == Name);
            if (subject is null)
            {
                return null;
            }
            subject.ECTS = ECTS;
            await _context.SaveChangesAsync();
            return await _context.Subjects.ToListAsync();
        }

        public async Task<List<SubjectReturnDTO>> GetAllSubjectsWithoutEnrollments()
        {
            var subjects = await _context.Subjects.Include(c => c.Books).ToListAsync();
            List<SubjectReturnDTO> result = new List<SubjectReturnDTO>();
            for (int i = 0; i < subjects.Count; i++)
            {
                List<BookDTO> books = new List<BookDTO>();
                foreach (var book in subjects[i].Books)
                {
                    books.Add(new BookDTO()
                    {
                        Id = book.Id,
                        Author = book.Author,
                        Description = book.Description,
                        Title = book.Title,
                        SubjectName = book.Subject.Name
                    });

                }
                result.Add(new SubjectReturnDTO()
                {
                    Id = subjects[i].Id,
                    Name = subjects[i].Name,
                    ECTS = subjects[i].ECTS,
                    Books = books,
                });
            }

            return result;
        }
    
        public async Task<SubjectReturnDTO?> GetSpecificSubjectFromIdWithoutEnrollments(int id)
        {

            var subject = await _context.Subjects.Include(c => c.Books).FirstOrDefaultAsync(x => x.Id == id);

            if (subject is null)
            {
                return null;
            }
            List<BookDTO> books = new List<BookDTO>();
            foreach (var book in subject.Books)
            {
                books.Add(new BookDTO()
                {
                    Id = book.Id,
                    Author = book.Author,
                    Description = book.Description,
                    Title = book.Title,
                    SubjectName = book.Subject.Name
                });
            }
            var subjectDTO = new SubjectReturnDTO()
            {
                Id = subject.Id,
                Name = subject.Name,
                ECTS = subject.ECTS,
                Books = books,
            };

            return subjectDTO;

        }

        public async Task<SubjectReturnDTO?> GetSpecificSubjectFromNameWithoutEnrollments(string Name)
        {
            var subject = await _context.Subjects.Include(c => c.Books).FirstOrDefaultAsync(x => x.Name == Name);

            if (subject is null)
            {
                return null;
            }
            List<BookDTO> books = new List<BookDTO>();
            foreach (var book in subject.Books)
            {
                books.Add(new BookDTO()
                {
                    Id = book.Id,
                    Author = book.Author,
                    Description = book.Description,
                    Title = book.Title,
                    SubjectName = book.Subject.Name
                });

            }

            var subjectDTO = new SubjectReturnDTO()
            {
                Id = subject.Id,
                Name = subject.Name,
                ECTS = subject.ECTS,
                Books = books
            };

            return subjectDTO;
        }
    }
    
}
