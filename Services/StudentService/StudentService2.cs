using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.Data;
using StudentAPI.DTOs;
using StudentAPI.DTOs.StudentAPI.DTOs;
using StudentAPI.Models;
using System.Diagnostics;
using System.Linq;
using static Azure.Core.HttpHeader;

namespace StudentAPI.Services.StudentService
{
    public class StudentService2 : IStudentService
    {
        private readonly DataContext _context;
        public StudentService2(DataContext context)
        {
            _context = context;
        }


        public async Task<List<StudentReturnDTO>> GetAllStudents()
        {
            var students = await _context.Students.Include(x => x.Enrollments).ThenInclude(c=>c.Subject).
                Include(x=>x.Enrollments).ThenInclude(c=>c.Book).ThenInclude(c=>c.Subject).ToListAsync();
            List<StudentReturnDTO> result = new List<StudentReturnDTO>();
            foreach (var student in students)
            {
                List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
                foreach (var enrollment in student.Enrollments)
                {
                    if (enrollment.Book is not null)
                    {
                        enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed,new BookDTO()
                        {
                            Id= enrollment.Book.Id,
                            Title=enrollment.Book.Title,
                            Description=enrollment.Book.Description,
                            Author=enrollment.Book.Author,
                            SubjectName=enrollment.Book.Subject.Name,
                        }));
                        continue;
                    }
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed,null));

                }
                result.Add(new StudentReturnDTO()
                {
                    ID = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    CurrentSemester = student.CurrentSemester,
                    Tell = student.Tell,
                    Email = student.email,
                    DateOfRegistration = student.DateOfRegistration,
                    Enrollments = enrollmentForStudentDTOs,
                    
                });
            }
            return result;
        }
       
        public async Task<StudentReturnDTO?> GetSpecificStudent(int id)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id); 

            if (student is null)
            {
                return null;
            }
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in student.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
            }
            var result = new StudentReturnDTO()
            {
                ID = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Tell = student.Tell,
                Email = student.email,
                CurrentSemester = student.CurrentSemester,
                DateOfRegistration = student.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };

            return result;
        }

        public async Task<List<StudentReturnDTO>?> GetSpecificStudents(String firstName, String lastName)
        {
            var students = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).Include(x => x.Enrollments).
                ThenInclude(c => c.Book).ThenInclude(c => c.Subject).Where(x=>x.FirstName==firstName && x.LastName == lastName).ToListAsync();

            if (students.IsNullOrEmpty())
            {
                return null;
            }
            var result = new List<StudentReturnDTO>();
            foreach (var student in students)
            {
                List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
                foreach (var enrollment in student.Enrollments)
                {
                    if (enrollment.Book is not null)
                    {
                        enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                        {
                            Id = enrollment.Book.Id,
                            Title = enrollment.Book.Title,
                            Description = enrollment.Book.Description,
                            Author = enrollment.Book.Author,
                            SubjectName = enrollment.Book.Subject.Name,
                        }));
                        continue;
                    }
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
                }
                result.Add(new StudentReturnDTO()
                {
                    ID = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Tell = student.Tell,
                    Email = student.email,
                    CurrentSemester = student.CurrentSemester,
                    DateOfRegistration = student.DateOfRegistration,
                    Enrollments = enrollmentForStudentDTOs,
                });
            }
            return result;
        }

        public async Task<List<StudentReturnDTO>> CreateStudent(StudentCreateDTO request)
        {
            var newStudent = new Student()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                CurrentSemester = request.CurrntSemester,
                Tell = request.Tell,
                email=request.email
            };
            var sub = request.Subjects;
            List<String> subjectNames = new List<String>();
            for (int i = 0; i < sub.Count; i++)
            {
                subjectNames.Add(sub[i].Name);
            }
            var subjects = await _context.Subjects.Where(s => subjectNames.Contains(s.Name)).ToListAsync();
            for (int i = 0; i < subjects.Count;i++){
                newStudent.Enrollments.Add(new Enrollment
                {

                    Student = newStudent,

                    Subject = subjects[i],

                });
            }
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();
            var students = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).ToListAsync();
            List<StudentReturnDTO> result = new List<StudentReturnDTO>();
            foreach (var student in students)
            {
                List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
                foreach (var enrollment in student.Enrollments)
                {
                    if (enrollment.Book is not null)
                    {
                        enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                        {
                            Id = enrollment.Book.Id,
                            Title = enrollment.Book.Title,
                            Description = enrollment.Book.Description,
                            Author = enrollment.Book.Author,
                            SubjectName = enrollment.Book.Subject.Name,
                        }));
                        continue;
                    }
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
                }
                result.Add(new StudentReturnDTO()
                {
                    ID = student.Id,
                    FirstName= student.FirstName,
                    LastName= student.LastName,
                    CurrentSemester = student.CurrentSemester,
                    Tell = student.Tell,
                    Email = student.email,
                    DateOfRegistration = student.DateOfRegistration,
                    Enrollments = enrollmentForStudentDTOs,                    
                });;
            }
            return result;
        }
       
        public async Task<StudentReturnDTO?> AddSubjectToStudent(int studentId, List<String> subjectNames)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).Include(x => x.Enrollments).
                ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == studentId);
            if (student is null)
            {
                return null;
            }
            //student.FirstName = request.FirstName;
            //student.LastName = request.LastName;
            var subjects = await _context.Subjects.Where(s => subjectNames.Contains(s.Name)).ToListAsync();
            if (subjects.Count == 0 || subjects.Count != subjectNames.Count || subjects is null)
            {
                return null;
            }
            for ( var i = 0;i < subjects.Count; i++)
            {
                if ( student.Enrollments.FirstOrDefault(x => x.Subject.Id == subjects[i].Id) == null)
                {
                    student.Enrollments.Add(new Enrollment
                    {

                        Student = student,

                        Subject = subjects[i],

                    }); 
                }

                
            }
            await _context.SaveChangesAsync();

            var stud = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == studentId);
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = student.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };
            return result;
        }
        
        public async Task<StudentReturnDTO?> UpdateStudent(int id,StudentCreateDTO request)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);

            if (student is null)
            {
                return null;
            }
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.CurrentSemester = request.CurrntSemester;
            student.Tell = request.Tell;
            student.email = request.email;

            var sub = request.Subjects;
            List<String> subjectNames = new List<String>();
            for (int i = 0; i < sub.Count; i++)
            {
                subjectNames.Add(sub[i].Name);
            }
            var subjects = await _context.Subjects.Where(s => subjectNames.Contains(s.Name)).ToListAsync();
            student.Enrollments.Clear();
            for (int i = 0; i < subjects.Count; i++)
            {
                student.Enrollments.Add(new Enrollment
                {

                    Student = student,

                    Subject = subjects[i],

                });
            }
            await _context.SaveChangesAsync();
            var stud = await _context.Students.Include(c => c.Enrollments)
                .ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));

            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = stud.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };
            return result;

        }

        public async Task<List<Student>?> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student is null)
            {
                return null;
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return await _context.Students.ToListAsync();
        }

        public async Task<StudentReturnDTO?> DeleteSubjectsFromStudent(int id, List<String> names)
        {
            if (names is null)
            {
                return null;
            }
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (student is null)
            {
                return null;
            }
            for (int i = 0;i< names.Count;i++)
            {
                student.Enrollments.RemoveAll(x=> x.Subject.Name == names[i]);
            }
            await _context.SaveChangesAsync();
            var stud = await _context.Students.Include(c => c.Enrollments).FirstOrDefaultAsync(x => x.Id == id);
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));

            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = stud.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };
            return result;

        }

        public async Task<StudentReturnDTO?> AddSubjectToStudentById(int studentId, List<int> subjectIds)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == studentId);
            if (student is null)
            {
                return null;
            }
            //student.FirstName = request.FirstName;
            //student.LastName = request.LastName;
            var subjects = await _context.Subjects.Where(s => subjectIds.Contains(s.Id)).ToListAsync();
            if (subjects.Count == 0 || subjects.Count != subjectIds.Count || subjects is null)
            {
                return null;
            }
            for (var i = 0; i < subjects.Count; i++)
            {
                if (student.Enrollments.FirstOrDefault(x => x.Subject.Id == subjects[i].Id) == null)
                {
                    student.Enrollments.Add(new Enrollment
                    {

                        Student = student,

                        Subject = subjects[i],

                    });
                }


            }
            await _context.SaveChangesAsync();
            var stud = await _context.Students.Include(c => c.Enrollments)
                .ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == studentId);

            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = stud.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };
            return result;
        }

        public async Task<StudentReturnDTO?> DeleteSubjectsFromStudentById(int id, List<int> ids)
        {
            if (ids is null)
            {
                return null;
            }
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (student is null)
            {
                return null;
            }
            for (int i = 0; i < ids.Count; i++)
            {
                student.Enrollments.RemoveAll(x => x.Subject.Id == ids[i]);
            }
            await _context.SaveChangesAsync();
            var stud = await _context.Students.Include(c => c.Enrollments)
                .ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = stud.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };

            return result;

        }

        public async Task<int> TotalECTS(int id)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (student is null)
            {
                return 0;
            }
            int sum = 0;
            for(int i=0; i<student.Enrollments.Count; i++)
            {
                sum += student.Enrollments[i].Subject.ECTS;
            }
            return sum;
        }

        public async Task<List<StudentReturnDTO>> AddSemester()
        {
            var students = await _context.Students.Include(x => x.Enrollments).ThenInclude(c => c.Subject).Include(x => x.Enrollments).
                ThenInclude(c => c.Book).ThenInclude(c => c.Subject).ToListAsync();
            List<StudentReturnDTO> result = new List<StudentReturnDTO>();

            if (students is null)
            {
                return null;
            }
            foreach(var student in students)
            {
                student.CurrentSemester += 1;
            }
            await _context.SaveChangesAsync();
            foreach (var student in students)
            {
                List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
                foreach (var enrollment in student.Enrollments)
                {
                    if (enrollment.Book is not null)
                    {
                        enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                        {
                            Id = enrollment.Book.Id,
                            Title = enrollment.Book.Title,
                            Description = enrollment.Book.Description,
                            Author = enrollment.Book.Author,
                            SubjectName = enrollment.Book.Subject.Name,
                        }));
                        continue;
                    }
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                        enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
                }
                result.Add(new StudentReturnDTO()
                {
                    ID = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    CurrentSemester = student.CurrentSemester,
                    Tell = student.Tell,
                    Email = student.email,
                    DateOfRegistration = student.DateOfRegistration,
                    Enrollments = enrollmentForStudentDTOs,
                });
            }


            return result;
        }
        
        public async Task<String?> certification(int id)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                return null;
            }
            List<String> subjects = new List<String>();
            foreach (var enrollment in student.Enrollments.Where(x => x.Passed))
            {
                subjects.Add(enrollment.Subject.Name);
            }
            String response;
            if (subjects.IsNullOrEmpty()){
                response = $"The student with the name {student.LastName} {student.FirstName} and id {student.Id}, is on the " +
                $"{student.CurrentSemester} semester and he haven't passed any subject.";
                return response;
            }
            response = $"The student with the name {student.LastName} {student.FirstName} and id {student.Id}, is on the " +
                $"{student.CurrentSemester} semester and he passed the subjects {String.Join("' ",subjects)}.";
            return response;
        }
        
        public async Task<StudentReturnDTO?> ChangeGradeOnSubject(int StudentId, int SubjectId, decimal Grade)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).
                Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == StudentId);
            if (student == null || Grade > 10 || Grade < 0)
            {
                return null;
            }
            Enrollment enrollment = new Enrollment();
            enrollment = student.Enrollments.FirstOrDefault(x=>x.Subject.Id == SubjectId );
            if (enrollment == null)
            {
                return null;
            }
            enrollment.Grade = Grade;
            await _context.SaveChangesAsync();
            var stud = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == StudentId);
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollmentStud in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = stud.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };
            return result;
        }
        
        public async Task<decimal?> GetMeanGrade(int id)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (student is null)
            {
                return null;
            }
            decimal sum = 0;
            
            int counter = 0;
            if (!student.Enrollments.Any(x => x.Passed))
            {
                return 0;
            }

            for (int i = 0; i < student.Enrollments.Count; i++)
            {
                var enrollment = student.Enrollments[i];
                if (enrollment.Passed)
                {
                    sum += enrollment.Grade;
                    counter++;
                }
            }
            
            return sum/counter;
        }

        public async Task<StudentReturnDTO?> UpdateEmail(int id, string email)
        {

            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject)
                .Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);

            if (student is null)
            {
                return null;
            }
            student.email = email;
            await _context.SaveChangesAsync();
            var stud = await _context.Students.Include(c => c.Enrollments)
                .ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = stud.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };
            return result;

        }
        
        public async Task<StudentReturnDTO?> UpdateTell(int id, string tell)
        {

            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject)
                .Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);

            if (student is null)
            {
                return null;
            }
            student.Tell = tell;
            await _context.SaveChangesAsync();
            var stud = await _context.Students.Include(c => c.Enrollments)
                .ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enrollment in stud.Enrollments)
            {
                if (enrollment.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, new BookDTO()
                    {
                        Id = enrollment.Book.Id,
                        Title = enrollment.Book.Title,
                        Description = enrollment.Book.Description,
                        Author = enrollment.Book.Author,
                        SubjectName = enrollment.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enrollment.Subject.Name,
                    enrollment.Subject.Id, enrollment.EnrollmentId, enrollment.Grade, enrollment.Passed, null));
            }
            var result = new StudentReturnDTO()
            {
                ID = stud.Id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                CurrentSemester = stud.CurrentSemester,
                Tell = stud.Tell,
                Email = stud.email,
                DateOfRegistration = stud.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs,
            };
            return result;
        }

        public async Task <List<RequestDTO>?> NewRequest(int id, string requestDescription)
        {
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                return null;
            }
            var request = new Models.Request(){
                Student = student,
                Description = requestDescription
            };
            student.Requests.Add(request);
            await _context.SaveChangesAsync();
            student = await _context.Students.Include(c => c.Requests).FirstOrDefaultAsync(x => x.Id == id);
            var requests = student.Requests;
            List<RequestDTO> requestDTOs= new List<RequestDTO>();
            for(int i=0; i < requests.Count; i++)
            {
                requestDTOs.Add(new RequestDTO
                {
                    Id = requests[i].Id,
                    Description = requests[i].Description,
                    Status = requests[i].Status,
                    Return = requests[i].Return,
                    DateoOfRequest = requests[i].DateOfRequest,
                    DateOfTermination = requests[i].DateOfterminations,
                    StudentId = requests[i].Student.Id,
                    FirstName = requests[i].Student.FirstName,
                    LastName = requests[i].Student.LastName
                }
                    );
            }
            return requestDTOs;
        }

        public async Task<List<RequestDTO>?> GetStudentsRequests(int id)
        {
            var student = await _context.Students.Include(c => c.Requests).FirstOrDefaultAsync(x => x.Id == id);
            if (student is null)
            {
                return null;
            }
            var requests = student.Requests;
            List<RequestDTO> requestDTOs = new List<RequestDTO>();
            for (int i = 0; i < requests.Count; i++)
            {
                requestDTOs.Add(new RequestDTO
                {
                    Id = requests[i].Id,
                    Description = requests[i].Description,
                    Status = requests[i].Status,
                    Return = requests[i].Return,
                    DateoOfRequest = requests[i].DateOfRequest,
                    DateOfTermination = requests[i].DateOfterminations,
                    StudentId = requests[i].Student.Id,
                    FirstName = requests[i].Student.FirstName,
                    LastName = requests[i].Student.LastName
                }
                    );
            }
            return requestDTOs;
        }   

        public async Task<StudentReturnDTO?> AddBookToStuden(int enrollmentId, int bookId)
        {
            var enrollment = await _context.Enrollments.Include(x => x.Subject).Include(x=>x.Book).Include(x=>x.Student).FirstOrDefaultAsync(c=>c.EnrollmentId == enrollmentId);
            var book = await _context.Books.Include(x=>x.Subject).FirstOrDefaultAsync(c=>c.Id == bookId);
            if (book == null || enrollment== null || enrollment.Book is not null || enrollment.Subject != book.Subject)
            {
                return null;
            } 
            enrollment.DateOfChoosingBook = DateTime.Now;
            enrollment.Book = book;
            await _context.SaveChangesAsync();
            var student = await _context.Students.Include(c => c.Enrollments).ThenInclude(c => c.Subject)
                .Include(x => x.Enrollments).ThenInclude(c => c.Book).ThenInclude(c => c.Subject).FirstOrDefaultAsync(x => x.Id == enrollment.Student.Id);
            if (student == null)
            {
                return null;
            }
            List<EnrollmentForStudentDTO> enrollmentForStudentDTOs = new List<EnrollmentForStudentDTO>();
            foreach (var enroll in student.Enrollments)
            {
                if (enroll.Book is not null)
                {
                    enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enroll.Subject.Name,
                    enroll.Subject.Id, enroll.EnrollmentId, enroll.Grade, enroll.Passed, new BookDTO()
                    {
                        Id = enroll.Book.Id,
                        Title = enroll.Book.Title,
                        Description = enroll.Book.Description,
                        Author = enroll.Book.Author,
                        SubjectName = enroll.Book.Subject.Name,
                    }));
                    continue;
                }
                enrollmentForStudentDTOs.Add(new EnrollmentForStudentDTO(enroll.Subject.Name,
                    enroll.Subject.Id, enroll.EnrollmentId, enroll.Grade, enroll.Passed, null));
            }
            StudentReturnDTO stud = new StudentReturnDTO()
            {
                ID= student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                CurrentSemester = student.CurrentSemester,
                Tell = student.Tell,
                Email = student.email,
                DateOfRegistration = student.DateOfRegistration,
                Enrollments = enrollmentForStudentDTOs
            };

            return stud;
            

        } 
    }
}