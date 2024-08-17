namespace ToReadListApplication.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Book> Books = new HashSet<Book> { };
    }
}
