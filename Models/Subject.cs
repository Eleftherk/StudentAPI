using System.Text.Json.Serialization;

namespace StudentAPI.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ECTS { get; set; }

        //public List<Student> Students { get; set; }
        [JsonIgnore]
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        //[JsonIgnore]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
