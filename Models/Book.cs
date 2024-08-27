using System.Text.Json.Serialization;

namespace StudentAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        //[JsonIgnore]
        public Subject Subject { get; set; }
        //[JsonIgnore]
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
