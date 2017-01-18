//-----------------------------------------------------------------------
// <copyright file="FileDiffManagerTests.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTO;
    using FileDiffREST.Tests.Core;
    using FileDiffREST.Tests.Core.Comparers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Util;

    /// <summary>
    /// Class that contains unit tests for FileDiffManager
    /// </summary>
    [TestClass]
    public class FileDiffManagerTests
    {
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
            var fileDiffManager = new FileDiffManager()
            {
                LeftFile = null,
                RightFile = rightFile
            };

            // Act
            var exceptionThrown = false;
            try
            {
                await fileDiffManager.CompareAsync();
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
            var fileDiffManager = new FileDiffManager()
            {
                LeftFile = leftFile,
                RightFile = null
            };

            // Act
            var exceptionThrown = false;
            try
            {
                await fileDiffManager.CompareAsync();
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

            var fileDiffManager = new FileDiffManager()
            {
                LeftFile = leftFile,
                RightFile = rightFile
            };

            // Act
            var result = await fileDiffManager.CompareAsync();

            // Assert
            Assert.IsNotNull(result, "The comparison result of the files is null!");
            Assert.AreEqual(
                ComparisonResult.SizeDoNotMatch.ToString(), 
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

            var fileDiffManager = new FileDiffManager()
            {
                LeftFile = leftFile,
                RightFile = rightFile
            };

            var expectedDiff = new List<DifferenceDTO>()
            {
                new DifferenceDTO() { Offset = 0, Length = 1 },
                new DifferenceDTO() { Offset = 2, Length = 2 }
            };

            // Act
            var result = await fileDiffManager.CompareAsync();

            // Assert
            Assert.IsNotNull(result, "The comparison result of the files is null!");
            Assert.AreEqual(
                ComparisonResult.ContentDoNotMatch.ToString(), 
                result.DiffResultType, 
                "The result type should be ContentDoNotMatch!");
            Assert.IsNotNull(result.Diffs, "The list of differences is null!");
            CollectionAssert.AreEqual(
                expectedDiff, 
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

            var fileDiffManager = new FileDiffManager()
            {
                LeftFile = leftFile,
                RightFile = rightFile
            };

            // Act
            var result = await fileDiffManager.CompareAsync();

            // Assert
            Assert.IsNotNull(result, "The comparison result of the files is null!");
            Assert.AreEqual(ComparisonResult.Equals.ToString(), result.DiffResultType, "The result type should be Equals!");
            Assert.IsNull(result.Diffs, "The list of differences should be null!");
        }
    }
}
