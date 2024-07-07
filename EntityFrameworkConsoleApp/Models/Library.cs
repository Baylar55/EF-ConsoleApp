namespace EntityFrameworkConsoleApp.Models;

public class Library
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; }

    public override string ToString() => $"Id: {Id}, Name: {Name}";
}
