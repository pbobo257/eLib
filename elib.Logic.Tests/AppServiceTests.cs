using eLib.Entities;
using eLib.Entities.Exceptions;
using eLib.Infrastructure;
using eLib.Infrastructure.Abstractions;
using eLib.Infrastructure.Repositories;
using eLib.Logic;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace elib.Logic.Tests
{
    public class AppServiceTests
    {
        [Fact]
        public void CreateAccount_FieldsNotFilled_ThrowsNotAllFieldsFilledException()
        {

            var service = new AppService(null,null,null);

            Assert.Throws<NotAllFieldsFilledException>(() => service.CreateAccount("", "", "", ""));
        }

        [Fact]
        public void CreateAccount_EmailNotValid_ThrowsEmailNotValidException()
        {

            var service = new AppService(null, null, null);

            Assert.Throws<EmailNotValidException>(() => service.CreateAccount("user", "sadf", "13", "13"));
        }

        [Fact]
        public void CreateAccount_PasswordsNotMatch_ThrowsPasswordsNotMatchException()
        {

            var service = new AppService(null, null, null);

            Assert.Throws<PasswordsNotMatchException>(() => service.CreateAccount("user", "sadf@a", "13", "12"));
        }

        [Fact]
        public void CreateBook_FieldsNotFilled_ThrowsNotAllFieldsFilledException()
        {

            var service = new AppService(null, null, null);

            Assert.Throws<NotAllFieldsFilledException>(() => service.CreateBook("","","",new DateTime(),"",new byte[1],new byte[1]));
        }

        [Fact]
        public void CreateBook_CoverNotChosen_ThrowsCoverNotChosenException()
        {

            var service = new AppService(null, null, null);

            Assert.Throws<CoverNotChosenException>(() => service.CreateBook("1", "2", "3", new DateTime(), "4", null, new byte[1]));
        }

        [Fact]
        public void CreateBook_BookNotChosen_ThrowsBookNotChosenException()
        {

            var service = new AppService(null, null, null);

            Assert.Throws<BookNotChosenException>(() => service.CreateBook("1", "2", "3", new DateTime(), "4", new byte[1], null));
        }

        [Fact]
        public void GetAllDetails_Success_ReturnListOfBookDetails()
        {
            IEnumerable<BookDetails> books = new List<BookDetails>();

            var bookDetailsRepositoryMock = new Mock<IBookDetailsRepository>();
            bookDetailsRepositoryMock.Setup(x => x.GetAll()).Returns(books);

            var service = new AppService(null, null, bookDetailsRepositoryMock.Object);

            var result = service.GetAllDetails();

            Assert.NotNull(result);
        }

        [Fact]
        public void GetBookById_Success_ReturnBookHeader()
        {
            var book = new BookHeader();

            var bookHeaderRepositoryMock = new Mock<IBookHeaderRepository>();
            bookHeaderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(book);

            var service = new AppService(null, bookHeaderRepositoryMock.Object, null);

            var result = service.GetBookById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetBooks_Success_ReturnListOfBookHeaders()
        {
            var books = new List<BookHeader>();

            var bookHeaderRepositoryMock = new Mock<IBookHeaderRepository>();
            bookHeaderRepositoryMock.Setup(x => x.GetAll()).Returns(books);

            var service = new AppService(null, bookHeaderRepositoryMock.Object, null);

            var result = service.GetBooks();

            Assert.NotNull(result);
        }

        [Fact]
        public void Login_WrongLogin_ThrowsWrongLoginOrPasswordException()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetByLogin(It.IsAny<string>())).Returns<Account>(null);

            var service = new AppService(accountRepositoryMock.Object, null, null);

            Assert.Throws<WrongLoginOrPasswordException>(() => service.Login("a","b"));
        }
        
        [Fact]
        public void Login_WrongPassword_ThrowsWrongLoginOrPasswordException()
        {
            var account = new Account { Login = "User", Password = "pass" };
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetByLogin(It.IsAny<string>())).Returns(account);

            var service = new AppService(accountRepositoryMock.Object, null, null);

            Assert.Throws<WrongLoginOrPasswordException>(() => service.Login("User", "password"));
        }
        
        [Fact]
        public void Login_Success_ReturnAccount()
        {
            var account = new Account { Login = "User", Password = "pass" };
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetByLogin(It.IsAny<string>())).Returns(account);

            var service = new AppService(accountRepositoryMock.Object, null, null);

            var result = service.Login(account.Login,account.Password);

            Assert.NotNull(result);
        }

    }
}
