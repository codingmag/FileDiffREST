//-----------------------------------------------------------------------
// <copyright file="FileDiffControllerIntegrationTests.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

// *** IMPORTANT ***
// Please run the tests in this file from the test explorer by selecting "filediffcontrollerintegrationtests"
// ordered test set and clicking "run selected tests" as they should be run in certain order.

using System.Collections.Generic;

namespace FileDiffREST.Web.Tests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using ActionResults;
    using Business.DTO;
    using Business.Mapper;
    using Business.Services;
    using Data.DAL;
    using FileDiffREST.Tests.Core;
    using FileDiffREST.Tests.Core.Comparers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Web.Controllers;

    /// <summary>
    /// Contains integration tests to be run in a certain order.
    /// </summary>
    [TestClass]
    public class FileDiffControllerIntegrationTests
    {
        /// <summary>
        /// The context.
        /// </summary>
        private FileDiffContext context;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            AutoMapperServiceLayerConfiguration.Configure();
            this.context = new FileDiffContext();
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB is empty,
        /// When: GetDiff is requested,
        /// Then: It is expected that API responds with NotFound status.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task EmptyDb_GetDiff_NotFound()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());

            // Act
            var result = await controller.GetDiffAsync(1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The action result should be 404-NotFound!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB is empty,
        /// When: PutLeftFile is executed,
        /// Then: It is expected that API responds with Created status.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task EmptyDb_PutLeftFile_Created()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());
            var leftFile = new FileContentDTO() {Data = TestConstants.File1Base64Content};

            // Act
            var result = await controller.PutLeftFileAsync(1, leftFile);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedFileActionResult), "The action result should be 201-Created!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB record exists with left file, but right file is null,
        /// When: GetDiff requested,
        /// Then: It is expected that API responds with NotFound status.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithLeftFile_GetDiff_NotFound()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());

            // Act
            var result = await controller.GetDiffAsync(1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The action result should be 404-NotFound!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB record exists with left file, but right file is null,
        /// When: PutRightFile is executed,
        /// Then: It is expected that API responds with Crated status.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithLeftFile_PutRightFile_Created()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());
            var rightFile = new FileContentDTO() { Data = TestConstants.File1Base64Content };

            // Act
            var result = await controller.PutRightFileAsync(1, rightFile);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedFileActionResult), "The action result should be 201-Created!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB record exists with 2 identical files,
        /// When: GetDiff is requested,
        /// Then: It is expected that API responds with Ok status and a diff result with Equals message.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithSameFile_GetDiff_OkEquals()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());

            // Act
            var actionResult = await controller.GetDiffAsync(1);
            var contentResult = actionResult as OkNegotiatedContentResult<DiffResultDTO>;

            //Assert
            Assert.IsNotNull(contentResult, "The action result should not be null!");
            Assert.IsNotNull(contentResult.Content, "The content of the action result should not be null!");
            Assert.AreEqual(ComparisonResult.Equals.ToString(), contentResult.Content.DiffResultType, "The diff result should be Equals!");
            Assert.IsNull(contentResult.Content.Diffs, "The list of diffs should not exist in the results!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB record exists with 2 identical files,
        /// When: PutRightFile is executed,
        /// Then: It is expected that API responds with Created status.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithSameFile_PutRightFile_Created()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());
            var rightFile = new FileContentDTO() { Data = TestConstants.FileIdenticalSizeNonIdenticalContentBase64Content };

            // Act
            var result = await controller.PutRightFileAsync(1, rightFile);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedFileActionResult), "The action result should be 201-Created!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB record exists with same sized files with different content,
        /// When: GetDiff is requested,
        /// Then: It is expected that API responds with Ok status with a diff result with message 
        /// ContentDoNotMatch and a list of diffs in means of starting offset and length.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithSameSizeDifferentContentFiles_GetDiff_OkContentDoNotMatch()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());
            var diffs = new List<DifferenceDTO>()
            {
                new DifferenceDTO() { Offset = 0, Length = 1 },
                new DifferenceDTO() { Offset = 2, Length = 2 }
            };

            // Act
            var actionResult = await controller.GetDiffAsync(1);
            var contentResult = actionResult as OkNegotiatedContentResult<DiffResultDTO>;

            //Assert
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

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB record exists with same sized files with different content,
        /// When: PutLeftFile is executed,
        /// Then: It is expected that API responds with Created status.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithSameSizeDifferentContentFiles_PutLeftFile_Created()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());
            var leftFile = new FileContentDTO() { Data = TestConstants.FileDifferentSizeBase64Content };

            // Act
            var result = await controller.PutLeftFileAsync(1, leftFile);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedFileActionResult), "The action result should be 201-Created!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: DB record exists with different sized files,
        /// When: GetDiff is requested,
        /// Then: It is expected that API responds with Ok status with a diff message SizesDoNotMatch.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithDifferentSizeFiles_GetDiff_OkSizeDoNotMatch()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());

            // Act
            var actionResult = await controller.GetDiffAsync(1);
            var contentResult = actionResult as OkNegotiatedContentResult<DiffResultDTO>;

            //Assert
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
        /// Given: DB record exists with different sized files,
        /// When: PutLeftFile is executed with an empty file,
        /// Then: It is expected that API responds with BadRequest status.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ExistingDbWithDifferentSizeFile_PutLeftFileEmpty_BadRequest()
        {
            // Arrange
            var controller = await Task.Run(() => GetController());
            var leftFile = new FileContentDTO() { Data = string.Empty };

            // Act
            var result = await controller.PutLeftFileAsync(1, leftFile);

            //Assert
            Assert.IsNotNull(result, "The action result should not be null!");
            Assert.IsInstanceOfType(result, typeof(BadRequestResult), "The action result should be 400-BadRequest!");
        }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <returns>The controller instance.</returns>
        private FileDiffController GetController()
        {
            var retrieveService = new RetrieveService(this.context);
            var uploadService = new UploadService(this.context);
            var diffService = new DiffService(this.context);
            return new FileDiffController(diffService, retrieveService, uploadService);
        }
    }
}
