// ChatGPT: https://chatgpt.com/share/673bc0ca-9444-8002-b979-3a298e68deb0
using Lab5_Library.Models;
using Lab5_Library.Services;

namespace Library_Test
{
    [TestClass]
    public class LibraryTest
    {
        // Read Book Tests
        [TestMethod]
        public void TestReadBooksSuccess()
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file
            string testBooks = "./TestBooks.csv";
            var testData = new[]
            {
                "1,Test Book 1,Test Author 1,Test ISBN 1",
                "2,Test Book 2,Test Author 2,Test ISBN 2"
            };
            File.WriteAllLines(testBooks, testData);

            // Act
            Lib.ReadBooks(testBooks);

            // Assert
            Assert.AreEqual(3, Lib.Books.Count);
            Assert.AreEqual("Test Book 1", Lib.Books[0].Title);
            Assert.AreEqual("Test Author 2", Lib.Books[1].Author);

            if (File.Exists(testBooks))
            {
                File.Delete(testBooks);
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestReadBooksFail()
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file with no data
            string testBooks = "./TestBooksInvalid.csv";

            // Act
            Lib.ReadBooks(testBooks);

            // Assert
            Assert.AreEqual(0, Lib.Books.Count);
        }


        // Add Book Tests
        [DataTestMethod]
        [DataRow("Test", "Test", "Test")]
        public void TestAddBookSuccess(string newTitle, string newAuthor, string newISBN)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testBooks = "./TestAddBooks.csv";

            // Act
            Lib.AddBook(newTitle, newAuthor, newISBN, testBooks);

            // Assert
            Assert.AreEqual(1, Lib.Books.Count());
            Assert.AreEqual("Test", Lib.Books[0].Title);
            Assert.AreEqual("Test", Lib.Books[0].Author);
            Assert.AreEqual("Test", Lib.Books[0].ISBN);
        }
        
        [DataTestMethod]
        [DataRow("TEST", "TEST", 1)]
        [ExpectedException(typeof (System.ArgumentException))]
        public void TestAddBookFail(string newTitle, string newAuthor, string newISBN)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testBooks = "./TestAddBooks.csv";

            int length = Lib.Books.Count;
            int testLength = length + 1;

            // Act
            Lib.AddBook(newTitle, newAuthor, newISBN, testBooks);

            // Assert
            Assert.AreEqual(testLength, Lib.Books.Count());
        }
        
        
        // Edit Book Tests
        [DataTestMethod]
        [DataRow("Unit Test Title", "Unit Test Author", "Unit Test ISBN")]
        public void TestEditBookSuccess(string newTitle, string newAuthor, string newISBN)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file
            string testBooks = "./TestBooks.csv";
            var testData = new[]
            {
                "1,Test Title 1,Test Author 1,Test ISBN 1",
                "2,Test Title 2,Test Author 2,Test ISBN 2"
            };
            File.WriteAllLines(testBooks, testData);

            Lib.ReadBooks(testBooks);

            Book bookToEdit = Lib.Books.FirstOrDefault(b => b.Id == 1);

            // Act
            Lib.EditBook(newTitle, newAuthor, newISBN, bookToEdit, testBooks);

            // Assert
            // Checks if the book had its variables changed to match the new ones.
            Assert.AreEqual(newTitle, bookToEdit.Title);
            Assert.AreEqual(newAuthor, bookToEdit.Author);
            Assert.AreEqual(newISBN, bookToEdit.ISBN);

            // Checks if the CSV file has changed to match with the new books.
            var csvFile = File.ReadAllLines(testBooks);
            Assert.AreEqual("1,Unit Test Title,Unit Test Author,Unit Test ISBN", csvFile[0]);
            Assert.AreEqual("2,Test Title 2,Test Author 2,Test ISBN 2", csvFile[1]);
        }

        [DataTestMethod]
        [DataRow("Unit Test Title", "Unit Test Author", 1)]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestEditBookFail(string newTitle, string newAuthor, string newISBN)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file
            string testBooks = "./TestBooks.csv";
            var testData = new[]
            {
                "1,Test Title 1,Test Author 1,Test ISBN 1",
                "2,Test Title 2,Test Author 2,Test ISBN 2"
            };
            File.WriteAllLines(testBooks, testData);

            Lib.ReadBooks(testBooks);

            Book bookToEdit = Lib.Books.FirstOrDefault(b => b.Id == 1);

            // Act
            Lib.EditBook(newTitle, newAuthor, newISBN, bookToEdit, testBooks);

            // Assert
            // Checks if the book had its variables changed to match the new ones.
            Assert.AreEqual(newTitle, bookToEdit.Title);
            Assert.AreEqual(newAuthor, bookToEdit.Author);
            Assert.AreEqual(newISBN, bookToEdit.ISBN);

            // Checks if the CSV file has changed to match with the new books.
            var csvFile = File.ReadAllLines(testBooks);
            Assert.AreEqual("1,Unit Test Title,Unit Test Author,Unit Test ISBN", csvFile[0]);
            Assert.AreEqual("2,Test Title 2,Test Author 2,Test ISBN 2", csvFile[1]);
        }


        // Delete Book Tests
        [DataTestMethod]
        [DataRow(1)]
        public void TestDeleteBookSuccess(int ID)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testBooksFile = "./TestDeleteBooks.csv";
            var testBooksData = new[]
            {
                "1,Test,Test,Test"
            };
            File.WriteAllLines(testBooksFile, testBooksData);

            // Act
            Lib.DeleteBook(ID, testBooksFile);

            // Assert
            Assert.AreEqual(0, Lib.Books.Count()); // Checks to see if the List was reduced to 0 books.
        }
        
        [DataTestMethod]
        [DataRow("1")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestDeleteBookFail(int ID)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testBooksFile = "./TestDeleteBooks.csv";
            var testBooksData = new[]
            {
                "1,Test,Test,Test"
            };
            File.WriteAllLines(testBooksFile, testBooksData);

            // Act
            Lib.DeleteBook(ID, testBooksFile);

            // Assert
            Assert.AreEqual(0, Lib.Books.Count()); // Checks to see if the List was reduced to 0 books.
        }


        // Read User Tests
        [TestMethod]
        public void TestReadUsersSuccess()
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file
            string testUsers = "./TestUsers.csv";
            var testData = new[]
            {
                "1,Test User 1,Test Email 1",
                "2,Test User 2,Test Email 2"
            };
            File.WriteAllLines(testUsers, testData);

            // Act
            Lib.ReadUsers(testUsers);

            // Assert
            Assert.AreEqual(2, Lib.Users.Count);
            Assert.AreEqual("Test User 1", Lib.Users[0].Name);
            Assert.AreEqual("Test Email 2", Lib.Users[1].Email);

            if (File.Exists(testUsers))
            {
                File.Delete(testUsers);
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestReadUsersFail()
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file with no data
            string testUsers = "./TestUsersInvalid.csv";

            // Act
            Lib.ReadUsers(testUsers);

            // Assert
            Assert.AreEqual(0, Lib.Users.Count);
        }


        // Add User Tests
        [DataTestMethod]
        [DataRow("Test", "Test")]
        public void TestAddUserSuccess(string newName, string newEmail)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testUsersFile = "./TestAddUsers.csv";

            // Act
            Lib.AddUser(newName, newEmail, testUsersFile);

            // Assert
            Assert.AreEqual(1, Lib.Users.Count()); // Makes sure the Users List was added to
            Assert.AreEqual("Test", Lib.Users[0].Name); // Makes sure the values are what they are supposed to be
            Assert.AreEqual("Test", Lib.Users[0].Email);
        }

        [DataTestMethod]
        [DataRow("TEST", 1)]
        [ExpectedException(typeof (System.ArgumentException))]
        public void TestAddUserFail(string newName, string newEmail)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testUsersFile = "./TestAddUsers.csv";

            // Act
            Lib.AddUser(newName, newEmail, testUsersFile);

            // Assert
            Assert.AreEqual(1, Lib.Users.Count());
        }


        // Edit Users Tests
        [DataTestMethod]
        [DataRow("Unit Test Name", "Unit Test Email")]
        public void TestEditUserSuccess(string newName, string newEmail)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file
            string testUsers = "./TestUsers.csv";
            var testData = new[]
            {
                "1,Test Name 1,Test Email 1",
                "2,Test Name 2,Test Email 2"
            };
            File.WriteAllLines(testUsers, testData);

            Lib.ReadUsers(testUsers);

            User userToEdit = Lib.Users.FirstOrDefault(u => u.Id == 1);

            // Act
            Lib.EditUser(newName, newEmail, userToEdit, testUsers);

            // Assert
            // Checks if the book had its variables changed to match the new ones.
            Assert.AreEqual(newName, userToEdit.Name);
            Assert.AreEqual(newEmail, userToEdit.Email);

            // Checks if the CSV file has changed to match with the new books.
            var csvFile = File.ReadAllLines(testUsers);
            Assert.AreEqual("1,Unit Test Name,Unit Test Email", csvFile[0]);
            Assert.AreEqual("2,Test Name 2,Test Email 2", csvFile[1]);
        }

        [DataTestMethod]
        [DataRow("Unit Test Name", 1)]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestEditUserFail(string newName, string newEmail)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            // Creates a test csv file
            string testUsers = "./TestUsers.csv";
            var testData = new[]
            {
                "1,Test Name 1,Test Email 1",
                "2,Test Name 2,Test Email 2"
            };
            File.WriteAllLines(testUsers, testData);

            Lib.ReadUsers(testUsers);

            User userToEdit = Lib.Users.FirstOrDefault(u => u.Id == 1);

            // Act
            Lib.EditUser(newName, newEmail, userToEdit, testUsers);

            // Assert
            // Checks if the book had its variables changed to match the new ones.
            Assert.AreEqual(newName, userToEdit.Name);
            Assert.AreEqual(newEmail, userToEdit.Email);

            // Checks if the CSV file has changed to match with the new books.
            var csvFile = File.ReadAllLines(testUsers);
            Assert.AreEqual("1,Unit Test Name,Unit Test Email", csvFile[0]);
            Assert.AreEqual("2,Test Name 2,Test Email 2", csvFile[1]);
        }


        // Delete Users Tests
        [DataTestMethod]
        [DataRow(1)]
        public void TestDeleteUserSuccess(int ID)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testUsersFile = "./TestDeleteUsers.csv";
            var testUsersData = new[]
            {
                "1,Test,Test,Test"
            };
            File.WriteAllLines(testUsersFile, testUsersData);

            // Act
            Lib.DeleteUser(ID, testUsersFile);

            // Assert
            Assert.AreEqual(0, Lib.Users.Count()); // Checks to see if the List was reduced to 0 books.
        }

        [DataTestMethod]
        [DataRow("1")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestDeleteUserFail(int ID)
        {
            // Arrange
            LibraryService Lib = new LibraryService();

            string testUsersFile = "./TestDeleteUsers.csv";
            var testUsersData = new[]
            {
                "1,Test,Test,Test"
            };
            File.WriteAllLines(testUsersFile, testUsersData);

            // Act
            Lib.DeleteUser(ID, testUsersFile);

            // Assert
            Assert.AreEqual(0, Lib.Users.Count()); // Checks to see if the List was reduced to 0 books.
        }
    }
}
