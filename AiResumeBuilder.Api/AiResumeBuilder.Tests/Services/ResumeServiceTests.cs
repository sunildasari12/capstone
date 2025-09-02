using NUnit.Framework;
using Moq;
using AiResumeBuilder.Api.Models;
using AiResumeBuilder.Api.Repositories;
using AiResumeBuilder.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiResumeBuilder.Api.Tests.Services
{
    [TestFixture]
    public class ResumeServiceTests
    {
        private Mock<IResumeRepository> _mockRepo;
        private ResumeService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IResumeRepository>();
            _service = new ResumeService(_mockRepo.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldCallRepositoryCreate_AndReturnResume()
        {
            // Arrange
            var resume = new Resume
            {
                Id = 1,
                UserId = 42,
                Title = "Software Engineer",
                Summary = "Test summary"
            };

            _mockRepo.Setup(r => r.CreateAsync(resume))
                     .ReturnsAsync(resume);

            // Act
            var result = await _service.CreateAsync(resume);

            // Assert
            Assert.That(result, Is.EqualTo(resume));
            _mockRepo.Verify(r => r.CreateAsync(resume), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnResume_IfExists()
        {
            // Arrange
            var resume = new Resume
            {
                Id = 1,
                UserId = 42,
                Title = "Software Engineer",
                Summary = "Test summary"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(resume);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(resume));
            _mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task GetByUserIdAsync_ShouldReturnListOfResumes()
        {
            // Arrange
            var resumes = new List<Resume>
            {
                new Resume { Id = 1, UserId = 42, Title = "Software Engineer", Summary = "Test summary" }
            };

            _mockRepo.Setup(r => r.GetByUserIdAsync(42))
                     .ReturnsAsync(resumes);

            // Act
            var result = await _service.GetByUserIdAsync(42);

            // Assert
            Assert.That(result, Is.EqualTo(resumes));
            _mockRepo.Verify(r => r.GetByUserIdAsync(42), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var resume = new Resume
            {
                Id = 1,
                UserId = 42,
                Title = "Software Engineer",
                Summary = "Updated summary"
            };

            _mockRepo.Setup(r => r.UpdateAsync(resume))
                     .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(resume);

            // Assert
            _mockRepo.Verify(r => r.UpdateAsync(resume), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1))
                     .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AiResumeBuilder.Tests.Services
//{
//    internal class ResumeServiceTests
//    {
//    }
//}
