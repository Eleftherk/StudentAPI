using System.Text.Json.Serialization;

namespace StudentAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CurrentSemester { get; set; }
        public string Tell { get; set; }
        public string email { get; set; }
        public DateTime DateOfRegistration { get; set; } = DateTime.Now;
        [JsonIgnore]
        public List<Request?> Requests { get; set; } =new List<Request?>();

        [JsonIgnore]
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        
        
    }
}
