namespace KoneProject.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Author { get; set; }
    }
}
