using EntityFrameworkConsoleApp.Constants;
using EntityFrameworkConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ShowMenu();
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowAllLibraries();
                        break;
                    case "2":
                        ShowAllBooks();
                        break;
                    case "3":
                        ShowAllAuthors();
                        break;
                    case "4":
                        ShowBooksByLibrary();
                        break;
                    case "5":
                        ShowBooksByAuthor();
                        break;
                    case "6":
                        ShowAuthorsByBook();
                        break;
                    case "7":
                        ShowAllBooksWithAuthors();
                        break;
                    case "8":
                        ShowAllLibrariesWithBooks();
                        break;
                    case "9":
                        AddLibrary();
                        break;
                    case "10":
                        AddBook();
                        break;
                    case "11":
                        AddAuthor();
                        break;
                    case "12":
                        AssignAuthorToBook();
                        break;
                    case "13":
                        FindBookByTitle();
                        break;
                    case "14":
                        FindAuthorByName();
                        break;
                    case "15":
                        FindBooksByGenre();
                        break;
                    case "0":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("""
                              0. Exit
                              1. Show All Libraries
                              2. Show All Books
                              3. Show All Authors
                              4. Show Books by Library
                              5. Show Books by Author
                              6. Show Authors by Books
                              7. Show All Books with Authors
                              8. Show All Libraries with Books
                              9. Add Library
                              10. Add Book
                              11. Add Author
                              12. Assign Author to Book
                              13. Find Book by Title
                              14. Find Author by Name
                              15. Find Books By Genre
                              """);
            Console.WriteLine();
            Console.Write("Enter menu number : ");
        }

        private static string GetInput(string inputMessage)
        {
            Console.Write(inputMessage);
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input can't be empty");
                Console.ResetColor();
                return GetInput(inputMessage);
            }
            return input;
        }

        private static int GetValidInput(string inputMessage)
        {
            Console.Write(inputMessage);

            string input = Console.ReadLine();
            bool isValid = int.TryParse(input, out int result);

            if (!isValid || result < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return GetValidInput(inputMessage);
            }
            return result;
        }

        static void ShowAllLibraries()
        {
            AppDbContext context = new();
            context.Libraries.ToList().ForEach(Console.WriteLine);
        }

        static void ShowAllBooks()
        {
            AppDbContext context = new();
            context.Books.ToList().ForEach(Console.WriteLine);
        }

        static void ShowAllAuthors()
        {
            AppDbContext context = new();
            context.Authors.ToList().ForEach(Console.WriteLine);
        }

        static void ShowBooksByLibrary()
        {
            AppDbContext context = new();

            int libraryId = GetValidInput("Enter library id: ");

            bool isExist = context.Libraries.Any(l => l.Id == libraryId);
            if (!isExist)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Library not found");
                Console.ResetColor();
                return;
            }

            var libraryBooks = context.Books
                            .Where(b => b.LibraryId == libraryId)
                            .Include(b => b.Library)
                            .ToList();
            if (libraryBooks != null)
                libraryBooks.ForEach(Console.WriteLine);
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No books found");
                Console.ResetColor();
            }
        }

        static void ShowBooksByAuthor()
        {
            AppDbContext context = new();

            int authorId = GetValidInput("Enter author id: ");

            bool isExist = context.Authors.Any(a => a.Id == authorId);
            if (!isExist)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Author not found");
                Console.ResetColor();
                return;
            }

            var authorBooks = context.BookAuthors
                            .Where(ab => ab.AuthorId == authorId)
                            .Include(ab => ab.Book)
                            .Include(ab => ab.Author)
                            .ToList();
            if (authorBooks.Count>0)
                authorBooks.ForEach(ab => Console.WriteLine(ab.Book));
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No books found");
                Console.ResetColor();
            }
        }

        static void ShowAuthorsByBook()
        {
            AppDbContext context = new();

            int bookId = GetValidInput("Enter book id: ");

            bool isExist = context.Books.Any(b => b.Id == bookId);
            if (!isExist)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book not found");
                Console.ResetColor();
                return;
            }

            var bookAuthors = context.BookAuthors
                            .Where(ab => ab.BookId == bookId)
                            .Include(ab => ab.Book)
                            .Include(ab => ab.Author)
                            .ToList();
            if (bookAuthors.Count>0)
                bookAuthors.ForEach(ab => Console.WriteLine(ab.Author));
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No authors found");
                Console.ResetColor();
            }
        }

        static void ShowAllBooksWithAuthors()
        {
            AppDbContext context = new();

            var books = context.Books
                        .Include(b => b.BookAuthors)
                        .ThenInclude(ab => ab.Author)
                        .ToList();

            if (books.Count > 0)
            {
                books.ForEach(b =>
                {
                    Console.WriteLine(new string('-',20));
                    Console.WriteLine(b);
                    b.BookAuthors.ForEach(ab => Console.WriteLine(ab.Author));
                    Console.WriteLine(new string('-',20));
                });
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No books found");
                Console.ResetColor();
            }
        }

        static void ShowAllLibrariesWithBooks()
        {
            AppDbContext context = new();

            var libraries = context.Libraries
                                   .Include(l => l.Books)
                                   .ToList();

            if (libraries.Count>0)
            {
                libraries.ForEach(l =>
                {
                    Console.WriteLine(new string('-', 20));
                    Console.WriteLine(l);
                    l.Books.ForEach(b => Console.WriteLine(b));
                    Console.WriteLine(new string('-', 20));
                });
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No libraries found");
                Console.ResetColor();
            }
        }

        static void AddLibrary()
        {
            AppDbContext context = new();

            string libraryName = GetInput("Enter library name: ");

            Library library = new() { Name = libraryName };

            context.Libraries.Add(library);
            context.SaveChanges();
        }

        static void AddBook()
        {
            AppDbContext context = new();

            string bookTitle = GetInput("Enter book title: ");
            int libraryId = GetValidInput("Enter library id: ");
            int genre = GetValidInput("Enter genre (0. Fiction, 1. NonFiction, 2. Mystery, 3.Romance, 4.Drama): ");
            DateTime publishYear = DateTime.UtcNow;

            Book book = new()
            {
                Title = bookTitle,
                LibraryId = libraryId,
                Genre = (Genre)genre,
                PublishYear = publishYear
            };

            context.Books.Add(book);
            context.SaveChanges();
        }

        static void AddAuthor()
        {
            AppDbContext context = new();

            string authorName = GetInput("Enter author name: ");
            int nationality = GetValidInput("Enter nationality (0. American, 1. British, 2. French, 3.German, 4.Russian, 6.Italian): ");

            Author author = new()
            {
                Name = authorName,
                Nationality = (Nationality)nationality
            };

            context.Authors.Add(author);
            context.SaveChanges();
        }

        static void AssignAuthorToBook()
        {
            AppDbContext context = new();

            int authorId = GetValidInput("Enter author id: ");
            int bookId = GetValidInput("Enter book id: ");

            BookAuthors bookAuthors = new()
            {
                AuthorId = authorId,
                BookId = bookId
            };

            context.BookAuthors.Add(bookAuthors);
            context.SaveChanges();
        }

        static void FindBookByTitle()
        {
            AppDbContext context = new();

            string title = GetInput("Enter book title: ");

            var book = context.Books.FirstOrDefault(b => b.Title.Trim().ToLower().Contains(title.Trim().ToLower()));

            if (book != null)
                Console.WriteLine(book);
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book not found");
                Console.ResetColor();
            }
        }

        static void FindAuthorByName()
        {
            AppDbContext context = new();

            string name = GetInput("Enter author name: ");

            var author = context.Authors
                        .FirstOrDefault(a => a.Name.Trim().ToLower().Contains(name.Trim().ToLower()));

            if (author != null)
                Console.WriteLine(author);
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Author not found");
                Console.ResetColor();
            }
        }

        static void FindBooksByGenre()
        {
            AppDbContext context = new();

            int genre = GetValidInput("Enter genre (0. Fiction, 1. NonFiction, 2. Mystery, 3.Romance, 4.Drama): ");

            var books = context.Books
                        .Where(b => b.Genre == (Genre)genre)
                        .ToList();

            if (books.Count > 0)
                books.ForEach(Console.WriteLine);
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No books found");
                Console.ResetColor();
            }
        }
    }
}
