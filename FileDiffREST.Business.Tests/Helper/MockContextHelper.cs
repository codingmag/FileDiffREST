//-----------------------------------------------------------------------
// <copyright file="MockContextHelper.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Tests.Helper
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using AsyncDb;
    using Data.DAL;
    using Data.Models;
    using Moq;

    /// <summary>
    /// The helper class for setting up fake DbSet and DbContext
    /// </summary>
    public static class MockContextHelper
    {
        /// <summary>
        /// Sets up the mock DB context.
        /// </summary>
        /// <param name="mockSet">The mock DB set.</param>
        /// <returns>
        /// Mock <see cref="FileDiffContext" /> object.
        /// </returns>
        public static Mock<FileDiffContext> SetupMockContext(Mock<DbSet<FileComparison>> mockSet)
        {
            var mockContext = new Mock<FileDiffContext>();
            mockContext.Setup(c => c.FilesForComparison).Returns(mockSet.Object);

            return mockContext;
        }

        /// <summary>
        /// Sets up the mock DB set.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// Mock <see cref="FileDiffContext" /> object.
        /// </returns>
        public static Mock<DbSet<FileComparison>> SetupMockSet(IQueryable<FileComparison> data)
        {
            var mockSet = new Mock<DbSet<FileComparison>>();
            mockSet.As<IDbAsyncEnumerable<FileComparison>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new FileDiffDbAsyncEnumerator<FileComparison>(data.GetEnumerator()));
            mockSet.As<IQueryable<FileComparison>>()
                .Setup(m => m.Provider)
                .Returns(new FileDiffDbAsyncQueryProvider<FileComparison>(data.Provider));
            mockSet.As<IQueryable<FileComparison>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<FileComparison>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<FileComparison>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}
