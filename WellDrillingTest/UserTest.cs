using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using robert.Controllers;
using robert.Data;
using robert.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellDrillingTest
{
    public class UserTest
    {
        private readonly IConfiguration _configuration;

        public UserTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        [Fact]
        public void AdditionTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                UsersController uc = new UsersController(context, _configuration);

                var result = uc.Add("angelinabest@mail.ru", "qwerty123", "Ангелина", "Морозова", "Менеджер") as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualUser = okResult.Value as User;

                var expectedUser = new User
                {
                    Id = 1012,
                    Username = "angelinabest@mail.ru",
                    Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                    Role = "Менеджер",
                    FirstName = "Ангелина",
                    LastName = "Морозова",
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsBlocked = false,
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedUser.Id, actualUser.Id);
                Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
                Assert.Equal(expectedUser.LastName, actualUser.LastName);
                Assert.Equal(expectedUser.Role, actualUser.Role);
                Assert.Equal(expectedUser.Username, actualUser.Username);
            }
        }

        [Fact]
        public void AdditionNotAvailableEmailTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                UsersController uc = new UsersController(context, _configuration);

                var result = uc.Add("angelinabest@mail.ru", 
                    "qwerty123", 
                    "Ангелина", 
                    "Морозова", 
                    "Менеджер") as JsonResult;
                var actualResult = result.Value as NotFoundResult;

                Assert.Equal(404, actualResult.StatusCode);
            }
        }

        [Fact]
        public void EditingTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                UsersController uc = new UsersController(context, _configuration);

                var result = uc.Edit(1012, "Оператор", "Морозова", "Ангелина") as JsonResult;
                var actualUser = result.Value as User;

                var expectedUser = new User
                {
                    Id = 1012,
                    Username = "angelinabest@mail.ru",
                    Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                    Role = "Оператор",
                    FirstName = "Ангелина",
                    LastName = "Морозова",
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsBlocked = false,
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedUser.Id, actualUser.Id);
                Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
                Assert.Equal(expectedUser.LastName, actualUser.LastName);
                Assert.Equal(expectedUser.Role, actualUser.Role);
                Assert.Equal(expectedUser.Username, actualUser.Username);
            }
        }

        [Fact]
        public void EditingPasswordTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                UsersController uc = new UsersController(context, _configuration);

                var result = uc.EditPassword(1012, "qwerty") as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualUser = okResult.Value as User;

                var expectedUser = new User
                {
                    Id = 1012,
                    Username = "angelinabest@mail.ru",
                    Password = "65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5",
                    Role = "Оператор",
                    FirstName = "Ангелина",
                    LastName = "Морозова",
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsBlocked = false,
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedUser.Id, actualUser.Id);
                Assert.Equal(expectedUser.Password, actualUser.Password);
                Assert.Equal(expectedUser.Username, actualUser.Username);
            }
        }

        [Fact]
        public void BlockTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                UsersController uc = new UsersController(context, _configuration);

                var result = uc.Block(1012) as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualUser = okResult.Value as User;

                var expectedUser = new User
                {
                    Id = 1012,
                    Username = "angelinabest@mail.ru",
                    Password = "65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5",
                    Role = "Оператор",
                    FirstName = "Ангелина",
                    LastName = "Морозова",
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsBlocked = true,
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedUser.Id, actualUser.Id);
                Assert.Equal(expectedUser.Username, actualUser.Username);
                Assert.Equal(expectedUser.IsBlocked, actualUser.IsBlocked);
            }
        }

        [Fact]
        public void UnblockTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                UsersController uc = new UsersController(context, _configuration);

                var result = uc.Unblock(1012) as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualUser = okResult.Value as User;

                var expectedUser = new User
                {
                    Id = 1012,
                    Username = "angelinabest@mail.ru",
                    Password = "65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5",
                    Role = "Оператор",
                    FirstName = "Ангелина",
                    LastName = "Морозова",
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsBlocked = false,
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedUser.Id, actualUser.Id);
                Assert.Equal(expectedUser.Username, actualUser.Username);
                Assert.Equal(expectedUser.IsBlocked, actualUser.IsBlocked);
            }
        }

        [Fact]
        public void GetUserTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                UsersController uc = new UsersController(context, _configuration);

                var result = uc.GetUserById(1012) as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualUser = okResult.Value as User;

                var expectedUser = new User
                {
                    Id = 1012,
                    Username = "angelinabest@mail.ru",
                    Password = "65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5",
                    Role = "Оператор",
                    FirstName = "Ангелина",
                    LastName = "Морозова",
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsBlocked = false,
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedUser.Id, actualUser.Id);
            }
        }
    }
}
