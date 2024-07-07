using EntityFrameworkConsoleApp.Constants;

namespace EntityFrameworkConsoleApp.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Genre Genre { get; set; }
    public DateTime PublishYear { get; set; }
    public int LibraryId { get; set; }
    public Library Library { get; set; }
    public List<BookAuthors> BookAuthors { get; set; }

    public override string ToString() => $"Id: {Id}, Title: {Title}, Genre: {Genre}, PublishYear: {PublishYear}";
}
