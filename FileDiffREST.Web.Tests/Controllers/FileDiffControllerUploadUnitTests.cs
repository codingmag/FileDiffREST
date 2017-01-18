//-----------------------------------------------------------------------
// <copyright file="FileDiffControllerUploadUnitTests.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Web.Tests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using ActionResults;
    using Business.DTO;
    using Business.Interfaces;
    using FileDiffREST.Tests.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Web.Controllers;

    /// <summary>
    /// Unit tests for upload related actions in FileDiffController.
    /// </summary>
    [TestClass]
    public class FileDiffControllerUploadUnitTests
    {
        /// <summary>
        /// Tests the following criteria:
        /// Given: Service returns FileComparisonDTO after uploading,
        /// When: Left file is uploaded,
        /// Then: It is expected that the controller returns 201-Created.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task DtoReturned_PutLeftFile_Created()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            mockUploadService.Setup(u => u.UploadLeftAsync(id, leftFile))
                .Returns(Task.FromResult(new FileComparisonDTO()
                    {
                        Id = id,
                        Left = leftFile,
                        Right = rightFile
                    }));
            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var result = await controller.PutLeftFileAsync(id, new FileContentDTO() { Data = TestConstants.File1Base64Content });

            // Assert
            Assert.IsNotNull(result, "The action result should not be null!");
            Assert.IsInstanceOfType(result, typeof(CreatedFileActionResult), "The action result should be 201-Created!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Service returns FileComparisonDTO after uploading,
        /// When: Right file is uploaded,
        /// Then: It is expected that the controller returns 201-Created.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task DtoReturned_PutRightFile_Created()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            mockUploadService.Setup(u => u.UploadLeftAsync(id, leftFile))
                .Returns(Task.FromResult(new FileComparisonDTO()
                {
                    Id = id,
                    Left = leftFile,
                    Right = rightFile
                }));
            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var result = await controller.PutRightFileAsync(id, new FileContentDTO() { Data = TestConstants.File1Base64Content });

            // Assert
            Assert.IsNotNull(result, "The action result should not be null!");
            Assert.IsInstanceOfType(result, typeof(CreatedFileActionResult), "The action result should be 201-Created!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Left file content is empty,
        /// When: Left file is uploaded,
        /// Then: It is expected that the controller returns 400-BadRequest.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task EmptyFile_PutLeftFile_BadRequest()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            var leftFile = new FileContentDTO() { Data = string.Empty };

            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var result = await controller.PutLeftFileAsync(id, leftFile);

            // Assert
            Assert.IsNotNull(result, "The action result should not be null!");
            Assert.IsInstanceOfType(result, typeof(BadRequestResult), "The action result should be 400-BadRequest!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Right file content is empty,
        /// When: Right file is uploaded,
        /// Then: It is expected that the controller returns 400-BadRequest.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task EmptyFile_PutRightFile_BadRequest()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            var rightFile = new FileContentDTO() { Data = string.Empty };

            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var result = await controller.PutRightFileAsync(id, rightFile);

            // Assert
            Assert.IsNotNull(result, "The action result should not be null!");
            Assert.IsInstanceOfType(result, typeof(BadRequestResult), "The action result should be 400-BadRequest!");
        }
    }
}
