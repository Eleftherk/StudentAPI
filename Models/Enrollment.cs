using System.Text.Json.Serialization;

namespace StudentAPI.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        //public int StudentId { get; set; }
        //public int SubjectId { get; set; }
        public decimal Grade { get; set; }
        public bool Passed => Grade >= 5;
        public DateTime DateOfEnrollment { get; set; } = DateTime.Now;
        //[JsonIgnore]
        public Student Student { get; set; }
        //[JsonIgnore]
        public Subject Subject { get; set; }
        //[JsonIgnore]
        public DateTime? DateOfChoosingBook { get; set; } = null;
        public Book? Book { get; set; } = null;

    }
}
