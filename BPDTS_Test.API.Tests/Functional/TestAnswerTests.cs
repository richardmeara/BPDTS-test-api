using BPDTS_Test.API.Controllers;
using BPDTS_Test.API.Models;
using BPDTS_Test.API.Models.Interfaces;
using BPDTS_Test.API.Tests.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPDTS_Test.API.Tests
{
    [TestClass]
    public class TestAnswerTests
    {
        private IBPDTSTestAppService _service;
        private TestAnswerController _controller;

        public TestAnswerTests()
        {
        }

        [TestInitialize]
        public void Initalize()
        {
            var mockLogger = new Mock<ILogger<TestAnswerController>>();
            ILogger<TestAnswerController> logger = mockLogger.Object;

            _service = new BPDTSTestAppServiceFake();
            _controller = new TestAnswerController(logger: logger, mainService: _service);
        }

        [TestMethod]
        public void GetLondonUsersByCityName_ReturnsOkObjectResult()
        {
            var result = _controller.GetLondonUsersByCityName();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetLondonUsersByCityName_ReturnsCorrectCount()
        {
            var result = await _controller.GetLondonUsersByCityName();
            var okResult = result as OkObjectResult;
            var londonUsersByCityName = okResult.Value as List<User>;
            Assert.AreEqual(1, londonUsersByCityName.Count);
        }

        [TestMethod]
        public void GetLondonUsersByCityNameAndCoordinates_ReturnsOkObjectResult()
        {
            var result = _controller.GetLondonUsersByCityNameAndCoordinates();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetLondonUsersByCityNameAndCoordinates_ReturnsCorrectCount()
        {
            var result = await _controller.GetLondonUsersByCityNameAndCoordinates();
            var okResult = result as OkObjectResult;
            var londonUsersByCityName = okResult.Value as List<User>;
            Assert.AreEqual(2, londonUsersByCityName.Count);
        }

        [TestMethod]
        public void GetLondonUsersByCoordinates_ReturnsOkObjectResult()
        {
            var result = _controller.GetLondonUsersByCoordinates();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetLondonUsersByCoordinates_ReturnsCorrectCount()
        {
            var result = await _controller.GetLondonUsersByCoordinates();
            var okResult = result as OkObjectResult;
            var londonUsersByCityName = okResult.Value as List<User>;
            Assert.AreEqual(1, londonUsersByCityName.Count);
        }
    }
}