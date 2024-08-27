namespace StudentAPI.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public String? Return { get; set; }
        public DateTime DateOfRequest { get; set; } = DateTime.Now;
        public DateTime? DateOfterminations { get; set; } = null;
        public Student Student { get; set; }

    }
}
