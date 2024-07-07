using EntityFrameworkConsoleApp.Constants;

namespace EntityFrameworkConsoleApp.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Nationality Nationality { get; set; }
    public List<BookAuthors> BookAuthors { get; set; }

    public override string ToString() => $"Id: {Id}, Name: {Name}, Nationality: {Nationality}";

}
