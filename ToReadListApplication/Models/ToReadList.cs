namespace ToReadListApplication.Models
{
    public class ToReadList
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

    }
}
