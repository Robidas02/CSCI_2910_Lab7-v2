using Lab5_Library.Components.Pages;
using Lab5_Library.Models;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace Lab5_Library.Services
{
    public class LibraryService : ILibraryService
    {
        public List<Book> Books { get; set; } = new List<Book>();

        public List<User> Users { get; set; } = new List<User>();

        public Dictionary<User, List<Book>> BorrowedBooks { get; set; } = new Dictionary<User, List<Book>>();

        public string Message { get; set; }

        public bool BookFound { get; set; }

        public bool UserFound { get; set; }

        // Book methods
        public void ReadBooks(string url)
        {
            // This was ripped straight from the starter code.
            foreach (var line in File.ReadLines(url))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 4)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim()
                        };

                        Books.Add(book);
                    }
                }
        }

        public void AddBook(string title, string author, string isbn, string url)
        {
            Message = string.Empty;

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(isbn))
            {
                int id = Books.Any() ? Books.Max(b => b.Id) + 1 : 1;
                Book newBook = new Book { Id = id, Title = title, Author = author, ISBN = isbn };
                Books.Add(newBook);

                // Use StreamWriter to append to the csv file
                // These two lines came straight from ChatGPT. I forgot how to use the StreamWriter.
                var writer = new StreamWriter(url, append: true);
                    try
                    {
                        writer.WriteLine($"{newBook.Id},{newBook.Title},{newBook.Author},{newBook.ISBN}");
                    }
                    finally
                    {
                        writer.Dispose();
                    }

                Message = $"{title} added successfully.";

            }
            else
            {
                Message = "Please fill in all input fields.";
            }
        }

        public void EditBook(string newTitle, string newAuthor, string newISBN, Book bookToEdit, string url)
        {
            Message = string.Empty;

            // Only changes the items if the field isn't left blank
            if (!string.IsNullOrEmpty(newTitle))
            {
                bookToEdit.Title = newTitle;
            }
            if (!string.IsNullOrEmpty(newAuthor))
            {
                bookToEdit.Author = newAuthor;
            }
            if (!string.IsNullOrEmpty(newISBN))
            {
                bookToEdit.ISBN = newISBN;
            }

            // Overwrites each book in the CSV, so the updated book is saved.
            // Using the overwrite function to rewrite the whole file came from ChatGPT.
            var writer = new StreamWriter(url, append: false);
                try
                {
                    foreach (Book book in Books)
                    {
                        writer.WriteLine($"{book.Id},{book.Title},{book.Author},{book.ISBN}");
                    }
                }
                finally
                {
                    writer.Dispose();
                }

            BookFound = false;

            Message = "Book edited successfully!";
        }

        public void DeleteBook(int bookID, string url)
        {
            Message = string.Empty;

            Book bookToDelete = Books.FirstOrDefault(b => b.Id == bookID);
            if (bookToDelete != null)
            {
                Books.Remove(bookToDelete);

                var writer = new StreamWriter(url, append: false);
                    try
                    {
                        foreach (Book book in Books)
                        {
                            writer.WriteLine($"{book.Id},{book.Title},{book.Author},{book.ISBN}");
                        }
                    }
                    finally
                    {
                        writer.Dispose();
                    }
                

                Message = $"{bookToDelete.Title} was deleted.";
            }
            else
            {
                Message = "Book not found.";
            }
        }

        // User methods
        public void ReadUsers(string url)
        {
            // This was ripped straight from the starter code.
            foreach (var line in File.ReadLines(url))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3) // Ensure there are enough fields
                    {
                        var user = new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };

                        Users.Add(user);
                    }
                }
        }

        public void AddUser(string name, string email, string url)
        {
            Message = string.Empty;

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                int id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
                User newUser = new User { Id = id, Name = name, Email = email };
                Users.Add(newUser);

                // Use StreamWriter to append to the csv file
                // These two lines came straight from ChatGPT. I forgot how to use the StreamWriter.
                using (var writer = new StreamWriter(url, append: true))
                {
                    writer.WriteLine($"{newUser.Id},{newUser.Name},{newUser.Email}");
                }

                Message = $"{name} added successfully.";

            }
            else
            {
                Message = "Please fill in all input fields.";
            }
        }

        public void EditUser(string newName, string newEmail, User userToEdit, string url)
        {
            Message = string.Empty;

            // Only changes the items if the field isn't left blank
            if (!string.IsNullOrEmpty(newName))
            {
                userToEdit.Name = newName;
            }
            if (!string.IsNullOrEmpty(newEmail))
            {
                userToEdit.Email = newEmail;
            }

            // Overwrites each book in the CSV, so the updated book is saved.
            // Using the overwrite function to rewrite the whole file came from ChatGPT.
            using (var writer = new StreamWriter(url, append: false))
            {
                foreach (User user in Users)
                {
                    writer.WriteLine($"{user.Id},{user.Name},{user.Email}");
                }
            }

            UserFound = false;

            Message = "User edited successfully!";
        }

        public void DeleteUser(int userID, string url)
        {
            Message = string.Empty;

            User userToDelete = Users.FirstOrDefault(u => u.Id == userID);
            if (userToDelete != null)
            {
                Users.Remove(userToDelete);

                var writer = new StreamWriter(url, append: false);
                try
                {
                    foreach (User user in Users)
                    {
                        writer.WriteLine($"{user.Id},{user.Name},{user.Email}");
                    }
                }
                finally
                {
                    writer.Dispose();
                }


                Message = $"{userToDelete.Name} was deleted.";
            }
            else
            {
                Message = "User not found.";
            }
        }

        // Borrow Method
        public void BorrowBook(int bookId, int userId)
        {
            Message = string.Empty;

            Book book = Books.FirstOrDefault(b => b.Id == bookId);

            // This if-statement, and all that's in it, was ripped straight from the starter code.
            if (book != null && Books.Count(b => b.Id == bookId) > 0)
            {
                User user = Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    if (!BorrowedBooks.ContainsKey(user))
                    {
                        BorrowedBooks[user] = new List<Book>();
                    }
                        BorrowedBooks[user].Add(book);
                        Books.Remove(book);
                        Message = "Book borrowed successfully!";
                }
                else
                {
                    Message = "User not found.";
                }
            }
            else
            {
                Message = "Book not found";
            }
        }

        public void ReturnBook(int userId, int bookId)
        {
            Message = string.Empty;

            User user = Users.FirstOrDefault(u => u.Id == userId);

            if (user != null && BorrowedBooks.ContainsKey(user) && BorrowedBooks[user].Count > 0)
            {
                if(bookId >= 1 && bookId <= BorrowedBooks[user].Count)
                {
                    Book bookToReturn = BorrowedBooks[user][bookId - 1];

                    BorrowedBooks[user].RemoveAt(bookId - 1);
                    Books.Add(bookToReturn);

                    Message = "Book returned successfully!";
                }
                else
                {
                    Message = "Invalid input.";
                }
            }
            else
            {
                Message = "User not found or user has no borrowed books.";
            }
        }
    }
}