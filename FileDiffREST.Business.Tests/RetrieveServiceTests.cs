//-----------------------------------------------------------------------
// <copyright file="RetrieveServiceTests.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;
    using FileDiffREST.Tests.Core;
    using Helper;
    using Mapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services;

    /// <summary>
    /// Class that contains unit tests for RetrieveService
    /// </summary>
    [TestClass]
    public class RetrieveServiceTests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            AutoMapperServiceLayerConfiguration.Configure();
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record exits,
        /// When: File comparison record is retrieved,
        /// Then: It is expected that the method returns a record with left and right files.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ComparisonRecordExistsWithFiles_Retrieve_FilesAreRetrieved()
        {
            // Arrange
            const int id = 1;
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var data = new List<FileComparison>()
                {
                    new FileComparison() { Id = id, Right = rightFile, Left = leftFile}
                }.AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var retrieveService = new RetrieveService(mockContext.Object);

            // Act
            var files = await retrieveService.GetFilesAsync(id);

            // Assert
            Assert.IsNotNull(files, "Retrieved record should not be null!");
            Assert.IsNotNull(files.Left, "Left file should not be null!");
            Assert.IsNotNull(files.Right, "Right file should not be null!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record does not exist,
        /// When: File comparison record is retrieved,
        /// Then: It is expected that the method returns null.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ComparisonRecordNotExistsWithFiles_Retrieve_NullRetrieved()
        {
            // Arrange
            const int id = 1;
            var data = new List<FileComparison>().AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var retrieveService = new RetrieveService(mockContext.Object);

            // Act
            var files = await retrieveService.GetFilesAsync(id);

            // Assert
            Assert.IsNull(files, "Retrieved record should be null!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record exists but left file is null,
        /// When: It is checked whether left exists,
        /// Then: It is expected that the method returns false.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ComparisonRecordExistsLeftNull_LeftExists_False()
        {
            // Arrange
            const int id = 1;
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var data = new List<FileComparison>()
                {
                    new FileComparison() { Id = id, Right = rightFile, Left = null}
                }.AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var retrieveService = new RetrieveService(mockContext.Object);

            // Act
            var result = await retrieveService.LeftFileExistsAsync(id);

            // Assert
            Assert.IsFalse(result, "Retrieved result should be false!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record exists but right file is null,
        /// When: It is checked whether right exists,
        /// Then: It is expected that the method returns false.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ComparisonRecordExistsRightNull_RightExists_False()
        {
            // Arrange
            const int id = 1;
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var data = new List<FileComparison>()
                {
                    new FileComparison() { Id = id, Right = null, Left = leftFile}
                }.AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var retrieveService = new RetrieveService(mockContext.Object);

            // Act
            var result = await retrieveService.RightFileExistsAsync(id);

            // Assert
            Assert.IsFalse(result, "Retrieved result should be false!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record and right file exists,
        /// When: It is checked whether right exists,
        /// Then: It is expected that the method returns true.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ComparisonRecordExistsRightNotNull_RightExists_True()
        {
            // Arrange
            const int id = 1;
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var data = new List<FileComparison>()
                {
                    new FileComparison() { Id = id, Right = rightFile, Left = leftFile}
                }.AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var retrieveService = new RetrieveService(mockContext.Object);

            // Act
            var result = await retrieveService.RightFileExistsAsync(id);

            // Assert
            Assert.IsTrue(result, "Retrieved result should be true!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record and left file exists,
        /// When: It is checked whether left exists,
        /// Then: It is expected that the method returns true.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task ComparisonRecordExistsLeftNotNull_LeftExists_True()
        {
            // Arrange
            const int id = 1;
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var data = new List<FileComparison>()
                {
                    new FileComparison() { Id = id, Right = rightFile, Left = leftFile}
                }.AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var retrieveService = new RetrieveService(mockContext.Object);

            // Act
            var result = await retrieveService.LeftFileExistsAsync(id);

            // Assert
            Assert.IsTrue(result, "Retrieved result should be true!");
        }
    }
}
