using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using robert.Controllers;
using robert.Data;
using robert.Models;

namespace WellDrillingTest
{
    public class ClientTest
    {
        private readonly IConfiguration _configuration;

        public ClientTest()
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
                ClientsController cc = new ClientsController(context);

                var result = cc.Add("Рябинин", "Захар", "+7 (950) 928-0902", "ryabininsergey@gmail.com", "РТ, Казань, Новаторов, 20, 92", 0) as JsonResult;
                var actualClient = result.Value as Client;

                var expectedClient = new Client
                {
                    Id = 8,
                    FirstName = "Захар",
                    LastName = "Рябинин",
                    ContactNumber = "+7 (950) 928-0902",
                    Email = "ryabininsergey@gmail.com",
                    Address = "РТ, Казань, Новаторов, 20, 92",
                    ClientType = ClientType.Физическое,
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsDeleted = false,
                    WorkOrders = new List<WorkOrder>()
                };

                Assert.Equal(expectedClient.Id, actualClient.Id);
                Assert.Equal(expectedClient.FirstName, actualClient.FirstName);
                Assert.Equal(expectedClient.LastName, actualClient.LastName);
                Assert.Equal(expectedClient.ContactNumber, actualClient.ContactNumber);
                Assert.Equal(expectedClient.Email, actualClient.Email);
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
                ClientsController cc = new ClientsController(context);

                var result = cc.Edit(8, "Рябинин", "Захар", "+7 (950) 928-0920", "ryabininsergey@yandex.ru", "РТ, Казань, Новаторов, 20, 92", 0) as JsonResult;
                var actualClient = result.Value as Client;

                var expectedClient = new Client
                {
                    Id = 8,
                    FirstName = "Захар",
                    LastName = "Рябинин",
                    ContactNumber = "+7 (950) 928-0920",
                    Email = "ryabininsergey@yandex.ru",
                    Address = "РТ, Казань, Новаторов, 20, 92",
                    ClientType = ClientType.Физическое,
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsDeleted = false,
                    WorkOrders = new List<WorkOrder>()
                };

                Assert.Equal(expectedClient.Id, actualClient.Id);
                Assert.Equal(expectedClient.FirstName, actualClient.FirstName);
                Assert.Equal(expectedClient.LastName, actualClient.LastName);
                Assert.Equal(expectedClient.ContactNumber, actualClient.ContactNumber);
                Assert.Equal(expectedClient.Email, actualClient.Email);
            }
        }

        [Fact]
        public void EditingUndefinedTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                ClientsController cc = new ClientsController(context);

                var result = cc.Edit(100, "Рябинин", 
                    "Захар", 
                    "+7 (950) 928-0920", 
                    "ryabininsergey@yandex.ru", 
                    "РТ, Казань, Новаторов, 20, 92", 
                    0) as JsonResult;
                var actualClient = result.Value as Client;

                Client? expectedClient = null;

                Assert.Equal(expectedClient, actualClient);
            }
        }

        [Fact]
        public void IsDeletedFalseTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                ClientsController cc = new ClientsController(context);

                var result = cc.IsDeletedFalse(5) as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualClient = okResult.Value as Client;

                var expectedClient = new Client
                {
                    Id = 5,
                    FirstName = "Надежда",
                    LastName = "Беловаs",
                    ContactNumber = "+79870453651",
                    Email = "nadezhdabelowa@yandex.ru",
                    Address = "не важно",
                    ClientType = ClientType.Физическое,
                    RegistrationDate = DateTime.ParseExact("2024-03-16 19:22:38.9746549", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsDeleted = false,
                    WorkOrders = new List<WorkOrder>()
                };

                Assert.Equal(expectedClient.Id, actualClient.Id);
                Assert.Equal(expectedClient.IsDeleted, actualClient.IsDeleted);
            }
        }

        [Fact]
        public void IsDeletedTrueTest()
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                ClientsController cc = new ClientsController(context);

                var result = cc.IsDeletedTrue(4) as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualClient = okResult.Value as Client;

                var expectedClient = new Client
                {
                    Id = 4,
                    FirstName = "Иван",
                    LastName = "Иванов",
                    ContactNumber = "+79412687753",
                    Email = "ivanivan@mail.ru",
                    Address = "РТ, Казань, Пушкина, 3, 3",
                    ClientType = ClientType.Юридическое,
                    RegistrationDate = DateTime.ParseExact("2024-03-15 22:01:12.6703485", "yyyy-MM-dd HH:mm:ss.fffffff", null),
                    IsDeleted = true,
                    WorkOrders = new List<WorkOrder>()
                };

                Assert.Equal(expectedClient.Id, actualClient.Id);
                Assert.Equal(expectedClient.IsDeleted, actualClient.IsDeleted);
            }
        }
    }
}