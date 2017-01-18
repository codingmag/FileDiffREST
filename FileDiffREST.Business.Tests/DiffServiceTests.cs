//-----------------------------------------------------------------------
// <copyright file="DiffServiceTests.cs" company=".">
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
    using DTO;
    using Data.DAL;
    using Data.Models;
    using FileDiffREST.Tests.Core;
    using FileDiffREST.Tests.Core.Comparers;
    using Helper;
    using Mapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services;

    /// <summary>
    /// Class that contains unit tests for DiffService
    /// </summary>
    [TestClass]
    public class DiffServiceTests
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
        /// When: Files are compared,
        /// Then: It is expected that the method throws an exception.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task LeftFileNull_Compare_ThrowException()
        {
            // Arrange
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            const int id = 1;

            var mockContext = await Task.Run(()=> GetMockContext(id, null, rightFile));;
            var diffService = new DiffService(mockContext.Object);

            // Act
            var exceptionThrown = false;
            try
            {
                await diffService.GetDiffAsync(id);
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
        /// When: Files are compared,
        /// Then: It is expected that the method throws an exception.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task RightFileNull_Compare_ThrowException()
        {
            // Arrange
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            const int id = 1;
            var mockContext = await Task.Run(() => GetMockContext(id, leftFile, null));
            var diffService = new DiffService(mockContext.Object);

            // Act
            var exceptionThrown = false;
            try
            {
                await diffService.GetDiffAsync(id);
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
        /// Given: File sizes do not match,
        /// When: Files are compared,
        /// Then: It is expected that the method returns a result indicating that the sizes do not match 
        /// and diff object should be null.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task NonIdenticalSizes_Compare_ReturnSizesDoNotMatch()
        {
            // Arrange
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var rightFile = Convert.FromBase64String(TestConstants.FileDifferentSizeBase64Content);
            const int id = 1;

            var mockContext = await Task.Run(()=> GetMockContext(id, leftFile, rightFile));
            var diffService = new DiffService(mockContext.Object);

            // Act
            var result = await diffService.GetDiffAsync(id);

            // Assert
            Assert.IsNotNull(result, "The comparison result of the files is null!");
            Assert.AreEqual(ComparisonResult.SizeDoNotMatch.ToString(), 
                result.DiffResultType,
                "The result type should be SizeDoNotMatch!");
            Assert.IsNull(result.Diffs, "The list of differences should be null!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: File sizes are the same but the content of the files are not identical,
        /// When: Files are compared,
        /// Then: It is expected that the method returns a result indicating that the sizes are identical
        /// but the content is different and the diff object should be listing the differences in terms of
        /// starting offset and length.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task IdenticalSizesNonIdenticalContent_Compare_ReturnContentDiff()
        {
            // Arrange
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var rightFile = Convert.FromBase64String(TestConstants.FileIdenticalSizeNonIdenticalContentBase64Content);
            const int id = 1;

            var mockContext = await Task.Run(() => GetMockContext(id, leftFile, rightFile));
            var diffService = new DiffService(mockContext.Object);

            var expectedDiff = new List<DifferenceDTO>()
            {
                new DifferenceDTO() { Offset = 0, Length = 1 },
                new DifferenceDTO() { Offset = 2, Length = 2 }
            };

            // Act
            var result = await diffService.GetDiffAsync(id);

            // Assert
            Assert.IsNotNull(result, "The comparison result of the files is null!");
            Assert.AreEqual(ComparisonResult.ContentDoNotMatch.ToString(), 
                result.DiffResultType,
                "The result type should be ContentDoNotMatch!");
            Assert.IsNotNull(result.Diffs, "The list of differences is null!");
            CollectionAssert.AreEqual(expectedDiff, 
                result.Diffs, 
                new DifferenceDTOComparer(),
                "The list of diffs is not matching with the expected result!");
        }

        /// <summary>
        /// Tests the following criteria:
        /// Given: The file sizes and their content is the same,
        /// When: Files are compared,
        /// Then: It is expected that the method returns a result indcating the files are equal
        /// and the diff object should be null.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        [TestMethod]
        public async Task IdenticalSizesIdenticalContent_Compare_ReturnEquals()
        {
            // Arrange
            var leftFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            var rightFile = Convert.FromBase64String(TestConstants.File1Base64Content);
            const int id = 1;

            var mockContext = await Task.Run(() => GetMockContext(id, leftFile, rightFile));
            var diffService = new DiffService(mockContext.Object);

            // Act
            var result = await diffService.GetDiffAsync(id);

            // Assert
            Assert.IsNotNull(result, "The comparison result of the files is null!");
            Assert.AreEqual(ComparisonResult.Equals.ToString(), 
                result.DiffResultType,
                "The result type should be Equals!");
            Assert.IsNull(result.Diffs, "The list of differences should be null!");
        }

        /// <summary>
        /// Sets up and returns the mock context.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <param name="leftFile">The left file.</param>
        /// <param name="rightFile">The right file.</param>
        /// <returns>Mock <see cref="FileDiffContext"/> object.</returns>
        private static Mock<FileDiffContext> GetMockContext(int id, byte[] leftFile, byte[] rightFile)
        {
            var data = new List<FileComparison>
            {
                new FileComparison() { Id = id, Left = leftFile, Right = rightFile}
            }.AsQueryable();

            var mockSet = MockContextHelper.SetupMockSet(data);
            return MockContextHelper.SetupMockContext(mockSet);
        }
    }
}
