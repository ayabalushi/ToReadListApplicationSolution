namespace ToReadListApplication.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Rate { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly PublishDate { get; set; }

    }
}
