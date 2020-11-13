using eLib.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Logic
{
    public interface IAppService
    {
        void CreateAccount(string login, string mail, string password, string confirmPassword);
        void CreateBook(string name, string author, string genre, DateTime? releaseDate, string description, byte[] cover, byte[] book);
        Account Login(string login, string password);
        List<BookHeader> GetBooks();
        BookHeader GetBookById(int id);
        List<BookDetails> GetAllDetails();
    }
}
