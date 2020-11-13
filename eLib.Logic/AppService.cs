using eLib.Entities;
using eLib.Entities.Exceptions;
using eLib.Infrastructure;
using eLib.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eLib.Logic
{
    public class AppService : IAppService
    {
        protected readonly IAccountRepository _accountRepository;
        protected readonly IBookHeaderRepository _bookHeaderRepository;
        protected readonly IBookDetailsRepository _bookDetailsRepository;

        public AppService(IAccountRepository accountRepository,
            IBookHeaderRepository bookHeaderRepository,
            IBookDetailsRepository bookDetailsRepository)
        {
            _accountRepository = accountRepository;
            _bookHeaderRepository = bookHeaderRepository;
            _bookDetailsRepository = bookDetailsRepository;
        }

        public void CreateAccount(string login, string mail, string password, string confirmPassword)
        {
            if (login == string.Empty || mail == string.Empty
               || password == string.Empty || confirmPassword == string.Empty)
            {
                throw new NotAllFieldsFilledException();
            }

            if (!new EmailAddressAttribute().IsValid(mail))
            {
                throw new EmailNotValidException();
            }

            if (password != confirmPassword)
            {
                throw new PasswordsNotMatchException();
            }

            var acc = new Account()
            {
                Login = login,
                Email = mail,
                Password = password,
                UserType = UserType.User
            };

            _accountRepository.Insert(acc);
            _accountRepository.SaveChanges();
        }

        public void CreateBook(string name, string author, string genre, DateTime? releaseDate, string description, byte[] cover, byte[] book)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author)
               || string.IsNullOrEmpty(genre) || releaseDate == null
               || string.IsNullOrEmpty(description))
            {
                throw new NotAllFieldsFilledException();
            }

            if (cover == null)
            {
                throw new CoverNotChosenException();
            }

            if (book == null)
            {
                throw new BookNotChosenException();
            }

            var header = new BookHeader
            {
                Cover = cover,
                Name = name,
                Details = new BookDetails
                {
                    Author = author,
                    Genre = genre,
                    Description = description,
                    ReleaseDate = (DateTime)releaseDate,
                    Book = book
                }
            };

            _accountRepository.SaveChanges();
        }

        public List<BookDetails> GetAllDetails()
        {
            return _bookDetailsRepository.GetAll().ToList();
        }


        public BookHeader GetBookById(int id)
        {
            return _bookHeaderRepository.GetById(id);
        }

        public List<BookHeader> GetBooks()
        {
            return _bookHeaderRepository.GetAll().ToList();
        }

        public Account Login(string login, string password)
        {
            var account = _accountRepository.GetByLogin(login);
            if(account == null || account.Password != password)
            {
                throw new WrongLoginOrPasswordException();
            }

            return account;
        }
    }
}
