//-----------------------------------------------------------------------
// <copyright file="FileDiffControllerDiffUnitTests.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Web.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Business.DTO;
    using Business.Interfaces;
    using FileDiffREST.Tests.Core.Comparers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Web.Controllers;

    /// <summary>
    /// Unit tests for diff related actions in FileDiffController.
    /// </summary>
    [TestClass]
    public class FileDiffControllerDiffUnitTests
    {
        /// <summary>
        /// Tests the following criteria:
        /// Given: Service returns left file does not exist,
        /// When: Files are compared,
        /// Then: It is expected that the controller returns NotFound.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task LeftFileNull_GetDiff_NotFound()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            mockRetrieveService.Setup(r => r.LeftFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            mockRetrieveService.Setup(r => r.RightFileExistsAsync(id))
                .Returns(Task.FromResult(false));
            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var result = await controller.GetDiffAsync(id);

            // Assert
            Assert.IsNotNull(result, "The action result should not be null!");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The action result should be 404-NotFound!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Service returns right file does not exist,
        /// When: Files are compared,
        /// Then: It is expected that the controller returns NotFound.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task RightFileNull_GetDiff_NotFound()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            mockRetrieveService.Setup(r => r.LeftFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            mockRetrieveService.Setup(r => r.RightFileExistsAsync(id))
                .Returns(Task.FromResult(false));
            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var result = await controller.GetDiffAsync(id);

            // Assert
            Assert.IsNotNull(result, "The action result should not be null!");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The action result should be 404-NotFound!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Service returned Equals comparison result,
        /// When: Files are compared,
        /// Then: It is expected that the controller returns OK having diffResultType item set to Equals.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task FilesSame_GetDiff_Diff()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            mockDiffService.Setup(d => d.GetDiffAsync(id))
                .Returns(Task.FromResult(new DiffResultDTO() { DiffResultType = ComparisonResult.Equals.ToString()}));
            mockRetrieveService.Setup(r => r.LeftFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            mockRetrieveService.Setup(r => r.RightFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var actionResult = await controller.GetDiffAsync(id);
            var contentResult = actionResult as OkNegotiatedContentResult<DiffResultDTO>;

            // Assert
            Assert.IsNotNull(contentResult, "The action result should not be null!");
            Assert.IsNotNull(contentResult.Content, "The content of the action result should not be null!");
            Assert.AreEqual(ComparisonResult.Equals.ToString(), contentResult.Content.DiffResultType, "The diff result should be Equals!");
            Assert.IsNull(contentResult.Content.Diffs, "The list of diffs should not exist in the results!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Service returned SizeDoNotMatch comparison result,
        /// When: Files are compared,
        /// Then: It is expected that the controller returns OK having diffResultType item set to SizeDoNotMatch.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task FilesDifferentSize_GetDiff_Diff()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            mockDiffService.Setup(d => d.GetDiffAsync(id))
                .Returns(Task.FromResult(new DiffResultDTO() { DiffResultType = ComparisonResult.SizeDoNotMatch.ToString() }));
            mockRetrieveService.Setup(r => r.LeftFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            mockRetrieveService.Setup(r => r.RightFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);


            // Act
            var actionResult = await controller.GetDiffAsync(id);
            var contentResult = actionResult as OkNegotiatedContentResult<DiffResultDTO>;

            // Assert
            Assert.IsNotNull(contentResult, "The action result should not be null!");
            Assert.IsNotNull(
                contentResult.Content, 
                "The content of the action result should not be null!");
            Assert.AreEqual(
                ComparisonResult.SizeDoNotMatch.ToString(), contentResult.Content.DiffResultType, 
                "The diff result should be SizeDoNotMatch!");
            Assert.IsNull(contentResult.Content.Diffs, "The list of diffs should not exist in the results!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Service returned ContentDoNotMatch with list of diffs,
        /// When: Files are compared,
        /// Then: It is expected that the controller returns OK having diffResultType item set to ContentDoNotMatch
        /// having the list of diffs in the result in means of the starting offset and length.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task FilesSameSizeDifferentContent_GetDiff_Diff()
        {
            // Arrange
            const int id = 1;
            var mockDiffService = new Mock<IDiffService>();
            var mockRetrieveService = new Mock<IRetrieveService>();
            var mockUploadService = new Mock<IUploadService>();
            var diffs = new List<DifferenceDTO>()
            {
                new DifferenceDTO() { Offset = 0, Length = 1 },
                new DifferenceDTO() { Offset = 2, Length = 2 }
            };
            mockDiffService.Setup(d => d.GetDiffAsync(id))
                .Returns(Task.FromResult(new DiffResultDTO()
                {
                    DiffResultType = ComparisonResult.ContentDoNotMatch.ToString(),
                    Diffs = diffs
                }));
            mockRetrieveService.Setup(r => r.LeftFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            mockRetrieveService.Setup(r => r.RightFileExistsAsync(id))
                .Returns(Task.FromResult(true));
            var controller = new FileDiffController(mockDiffService.Object, mockRetrieveService.Object, mockUploadService.Object);

            // Act
            var actionResult = await controller.GetDiffAsync(id);
            var contentResult = actionResult as OkNegotiatedContentResult<DiffResultDTO>;

            // Assert
            Assert.IsNotNull(contentResult, "The action result should not be null!");
            Assert.IsNotNull(contentResult.Content, "The content of the action result should not be null!");
            Assert.AreEqual(
                ComparisonResult.ContentDoNotMatch.ToString(), 
                contentResult.Content.DiffResultType, 
                "The diff result should be ContentDoNotMatch!");
            Assert.IsNotNull(contentResult.Content.Diffs, "The list of diffs should exist in the results!");
            CollectionAssert.AreEqual(
                diffs, 
                contentResult.Content.Diffs, 
                new DifferenceDTOComparer(),
                "The list of diffs is not matching with the expected result!");
        }
    }
}
