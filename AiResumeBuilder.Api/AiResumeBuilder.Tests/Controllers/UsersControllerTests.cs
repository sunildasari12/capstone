using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AiResumeBuilder.Api.Controllers;
using AiResumeBuilder.Api.Models;
using AiResumeBuilder.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace AiResumeBuilder.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUserRepository> _mockRepo;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _controller = new UsersController(_mockRepo.Object);
        }

        [Test]
        public async Task Get_UserExists_ReturnsOkWithUser()
        {
            // Arrange
            var userId = 1;
            var fixedDate = new DateTime(2025, 01, 01, 12, 0, 0, DateTimeKind.Utc); // ✅ fixed CreatedAt for stable test

            var user = new AppUser
            {
                Id = userId,
                FullName = "John Doe",
                Email = "john@example.com",
                PasswordHash = "hashed-password",
                Role = UserRole.User,
                CreatedAt = fixedDate
            };

            _mockRepo.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync(user);

            // Act
            var result = await _controller.Get(userId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // Convert anonymous object → dictionary
            var json = JsonSerializer.Serialize(result.Value);
            var returnedUser = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            Assert.AreEqual(user.Id, returnedUser["Id"].GetInt32());
            Assert.AreEqual(user.FullName, returnedUser["FullName"].GetString());
            Assert.AreEqual(user.Email, returnedUser["Email"].GetString());

            // Role is serialized as number (enum underlying int)
            Assert.AreEqual((int)user.Role, returnedUser["Role"].GetInt32());

            // ✅ Compare CreatedAt as DateTime (not string)
            var createdAt = returnedUser["CreatedAt"].GetDateTime();
            Assert.That(createdAt, Is.EqualTo(user.CreatedAt).Within(TimeSpan.FromSeconds(1)));
        }


        //[Test]
        //public async Task Get_UserExists_ReturnsOkWithUser()
        //{
        //    // Arrange
        //    var userId = 1;
        //    var user = new AppUser
        //    {
        //        Id = userId,
        //        FullName = "John Doe",
        //        Email = "john@example.com",
        //        PasswordHash = "hashed-password",
        //        Role = UserRole.User,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _mockRepo.Setup(r => r.GetByIdAsync(userId))
        //             .ReturnsAsync(user);

        //    // Act
        //    var result = await _controller.Get(userId) as OkObjectResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(200, result.StatusCode);

        //    // Convert anonymous object → dictionary
        //    var json = JsonSerializer.Serialize(result.Value);
        //    var returnedUser = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        //    Assert.AreEqual(user.Id, returnedUser["Id"].GetInt32());
        //    Assert.AreEqual(user.FullName, returnedUser["FullName"].GetString());
        //    Assert.AreEqual(user.Email, returnedUser["Email"].GetString());

        //    // Role is serialized as number (enum underlying int)
        //    Assert.AreEqual((int)user.Role, returnedUser["Role"].GetInt32());

        //    // CreatedAt: compare within tolerance because of JSON precision
        //    var createdAt = returnedUser["CreatedAt"].GetDateTime();
        //    Assert.That(createdAt, Is.EqualTo(user.CreatedAt).Within(TimeSpan.FromSeconds(1)));
        //}

        [Test]
        public async Task Get_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var userId = 2;
            _mockRepo.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.Get(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}



//using System;
//using System.Collections.Generic;
//using System.Text.Json;
//using System.Threading.Tasks;
//using AiResumeBuilder.Api.Controllers;
//using AiResumeBuilder.Api.Models;
//using AiResumeBuilder.Api.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using NUnit.Framework;

//namespace AiResumeBuilder.Tests.Controllers
//{
//    [TestFixture]
//    public class UsersControllerTests
//    {
//        private Mock<IUserRepository> _mockRepo;
//        private UsersController _controller;

//        [SetUp]
//        public void Setup()
//        {
//            _mockRepo = new Mock<IUserRepository>();
//            _controller = new UsersController(_mockRepo.Object);
//        }

//        [Test]
//        public async Task Get_UserExists_ReturnsOkWithUser()
//        {
//            // Arrange
//            var userId = 1;
//            var user = new AppUser
//            {
//                Id = userId,
//                FullName = "John Doe",
//                Email = "john@example.com",
//                PasswordHash = "hashed-password",
//                Role = UserRole.User,
//                CreatedAt = DateTime.UtcNow
//            };

//            _mockRepo.Setup(r => r.GetByIdAsync(userId))
//                     .ReturnsAsync(user);

//            // Act
//            var result = await _controller.Get(userId) as OkObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(200, result.StatusCode);

//            // Convert anonymous object → dictionary
//            var json = JsonSerializer.Serialize(result.Value);
//            var returnedUser = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

//            Assert.AreEqual(user.Id.ToString(), returnedUser["Id"].ToString());
//            Assert.AreEqual(user.FullName, returnedUser["FullName"].ToString());
//            Assert.AreEqual(user.Email, returnedUser["Email"].ToString());
//            Assert.AreEqual(user.Role.ToString(), returnedUser["Role"].ToString());
//        }

//        [Test]
//        public async Task Get_UserDoesNotExist_ReturnsNotFound()
//        {
//            // Arrange
//            var userId = 2;
//            _mockRepo.Setup(r => r.GetByIdAsync(userId))
//                     .ReturnsAsync((AppUser)null);

//            // Act
//            var result = await _controller.Get(userId);

//            // Assert
//            Assert.IsInstanceOf<NotFoundResult>(result);
//        }
//    }
//}
