using Lab5_Library.Models;

namespace Lab5_Library.Services
{
    public interface ILibraryService
    {
        public List<Book> Books { get; set; }

        public List<User> Users { get; set; }

        public Dictionary<User, List<Book>> BorrowedBooks {  get; set; } 

        public string Message { get; set; }

        public bool BookFound { get; set; }

        public bool UserFound { get; set; }
        

        // Book Methods
        public void ReadBooks(string url);

        public void AddBook(string title, string author, string isbn, string url);

        public void EditBook(string newTitle, string newAuthor, string newISBN, Book bookToEdit, string url);

        public void DeleteBook(int bookID, string url);

        // User Methods
        public void ReadUsers(string url);

        public void AddUser(string name, string email, string url);

        public void EditUser(string newName, string newEmail, User userToEdit, string url);

        public void DeleteUser(int userID, string url);

        // Borrow Method
        public void BorrowBook(int bookId, int userId);

        // Return Method
        public void ReturnBook(int userId, int bookId);
    }
}
