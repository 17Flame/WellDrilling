using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using robert.Controllers;
using robert.Data;
using robert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellDrillingTest
{
    public class WellTest
    {
        private readonly IConfiguration _configuration;

        public WellTest()
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
                WellsController wc = new WellsController(context);

                var result = wc.Add("Тестовая скважина 4", 
                    55.764841, 
                    49.414373, 5, 5, "Вращательное", "Торф") as JsonResult;
                var okResult = result.Value as OkObjectResult;
                var actualWell = okResult.Value as Well;

                var expectedWell = new Well
                {
                    Id = 1005,
                    Name = "Тестовая скважина 4",
                    Latitude = 55.764841,
                    Longitude = 49.414373,
                    Depth = 5,
                    Diameter = 5,
                    DrillingMethod = "Вращательное",
                    SoilType = "Торф",
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedWell.Id, actualWell.Id);
                Assert.Equal(expectedWell.Name, actualWell.Name);
                Assert.Equal(expectedWell.Latitude, actualWell.Latitude);
                Assert.Equal(expectedWell.Longitude, actualWell.Longitude);
                Assert.Equal(expectedWell.Depth, actualWell.Depth);
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
                WellsController wc = new WellsController(context);

                var result = wc.Edit(1005, "Тестовая скважина 4",
                    55.764841,
                    49.414373, 10, 3, 
                    "Вращательное", "Суглинок") as JsonResult;
                var actualWell = result.Value as Well;

                var expectedWell = new Well
                {
                    Id = 1005,
                    Name = "Тестовая скважина 4",
                    Latitude = 55.764841,
                    Longitude = 49.414373,
                    Depth = 10,
                    Diameter = 3,
                    DrillingMethod = "Вращательное",
                    SoilType = "Суглинок",
                    WellUsers = new List<WellUser>()
                };

                Assert.Equal(expectedWell.Id, actualWell.Id);
                Assert.Equal(expectedWell.Name, actualWell.Name);
                Assert.Equal(expectedWell.Latitude, actualWell.Latitude);
                Assert.Equal(expectedWell.Longitude, actualWell.Longitude);
                Assert.Equal(expectedWell.Depth, actualWell.Depth);
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
                WellsController wc = new WellsController(context);

                var result = wc.Edit(100, "Тестовая скважина 4",
                    55.764841,
                    49.414373, 10, 3,
                    "Вращательное", "Суглинок") as JsonResult;
                var actualWell = result.Value as Well;

                Well expectedWell = null;

                Assert.Equal(expectedWell, actualWell);
            }
        }
    }
}
