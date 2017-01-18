//-----------------------------------------------------------------------
// <copyright file="UploadServiceTests.cs" company=".">
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
    using Moq;
    using Services;

    [TestClass]
    public class UploadServiceTests
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
        /// Given: Left file is null,
        /// When: Left file is uploaded,
        /// Then: It is expected that the method throws an exception.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task LeftFileNull_Upload_ThrowException()
        {
            // Arrange
            const int id = 1;
            var data = new List<FileComparison>().AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(()=> MockContextHelper.SetupMockContext(mockSet));
            var uploadService = new UploadService(mockContext.Object);

            // Act
            var exceptionThrown = false;
            try
            {
                await uploadService.UploadLeftAsync(id, null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "ArgumentNullException was not thrown!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: Right file is null,
        /// When: Right file is uploaded,
        /// Then: It is expected that the method throws an exception.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task RightFileNull_Upload_ThrowException()
        {
            // Arrange
            const int id = 1;
            var data = new List<FileComparison>().AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var uploadService = new UploadService(mockContext.Object);

            // Act
            var exceptionThrown = false;
            try
            {
                await uploadService.UploadRightAsync(id, null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "ArgumentNullException was not thrown!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record does not exist in the context,
        /// When: Left file is uploaded,
        /// Then: It is expected that a new record is created.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task RecordNotExists_UploadLeft_Success()
        {
            // Arrange
            const int id = 1;
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var data = new List<FileComparison>().AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var uploadService = new UploadService(mockContext.Object);

            // Act
            await uploadService.UploadLeftAsync(id, leftFile);

            // Assert
            var recordAddedToDbSet = true;
            var changesSaved = true;

            try
            {
                mockSet.Verify(m => m.Add(It.IsAny<FileComparison>()), Times.Once());
            }
            catch (MockException)
            {
                recordAddedToDbSet = false;
            }

            try
            {
                mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
            }
            catch (MockException)
            {
                changesSaved = false;
            }

            Assert.IsTrue(recordAddedToDbSet, "Record was not added!");
            Assert.IsTrue(changesSaved, "Changes were not saved to context!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record does not exist in the context,
        /// When: Right file is uploaded,
        /// Then: It is expected that a new record is created.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task RecordNotExists_UploadRight_Success()
        {
            // Arrange
            const int id = 1;
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var data = new List<FileComparison>().AsQueryable();
            var mockSet = await Task.Run(() => MockContextHelper.SetupMockSet(data));
            var mockContext = await Task.Run(() => MockContextHelper.SetupMockContext(mockSet));
            var uploadService = new UploadService(mockContext.Object);

            // Act
            await uploadService.UploadRightAsync(id, rightFile);

            // Assert
            var recordAddedToDbSet = true;
            var changesSaved = true;

            try
            {
                mockSet.Verify(m => m.Add(It.IsAny<FileComparison>()), Times.Once());
            }
            catch (MockException)
            {
                recordAddedToDbSet = false;
            }

            try
            {
                mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
            }
            catch (MockException)
            {
                changesSaved = false;
            }

            Assert.IsTrue(recordAddedToDbSet, "Record was not added!");
            Assert.IsTrue(changesSaved, "Changes were not saved to context!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record does exists in the context,
        /// When: Left file is uploaded,
        /// Then: It is expected that record is updated.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task RecordExists_UploadLeft_Success()
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
            var uploadService = new UploadService(mockContext.Object);

            // Act
            await uploadService.UploadLeftAsync(id, rightFile);

            // Assert
            var recordAddedToDbSet = true;
            var changesSaved = true;

            try
            {
                mockSet.Verify(m => m.Add(It.IsAny<FileComparison>()), Times.Once());
            }
            catch (MockException)
            {
                recordAddedToDbSet = false;
            }

            try
            {
                mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
            }
            catch (MockException)
            {
                changesSaved = false;
            }

            Assert.IsFalse(recordAddedToDbSet, "Existing record should be updated instead of adding a new one!");
            Assert.IsTrue(changesSaved, "Changes were not saved to context!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File comparison record exists in the context,
        /// When: Right file is uploaded,
        /// Then: It is expected that record is updated.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task RecordExists_UploadRight_Success()
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
            var uploadService = new UploadService(mockContext.Object);

            // Act
            await uploadService.UploadRightAsync(id, rightFile);

            // Assert
            var recordAddedToDbSet = true;
            var changesSaved = true;

            try
            {
                mockSet.Verify(m => m.Add(It.IsAny<FileComparison>()), Times.Once());
            }
            catch (MockException)
            {
                recordAddedToDbSet = false;
            }

            try
            {
                mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
            }
            catch (MockException)
            {
                changesSaved = false;
            }

            Assert.IsFalse(recordAddedToDbSet, "Existing record should be updated instead of adding a new one!");
            Assert.IsTrue(changesSaved, "Changes were not saved to context!");
        }
    }
}
