//-----------------------------------------------------------------------
// <copyright file="MockContextHelper.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Tests.AsyncDb
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Enumerable for async DB context operations.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <seealso cref="System.Linq.EnumerableQuery{T}" />
    /// <seealso cref="System.Data.Entity.Infrastructure.IDbAsyncEnumerable{T}" />
    /// <seealso cref="System.Linq.IQueryable{T}" />
    internal class FileDiffDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDiffDbAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="enumerable">A collection to associate with the new instance.</param>
        public FileDiffDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDiffDbAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="expression">An expression tree to associate with the new instance.</param>
        public FileDiffDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        /// <summary>
        /// Gets an enumerator that can be used to asynchronously enumerate the sequence.
        /// </summary>
        /// <returns>
        /// Enumerator for asynchronous enumeration over the sequence.
        /// </returns>
        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new FileDiffDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        /// <summary>
        /// Gets an enumerator that can be used to asynchronously enumerate the sequence.
        /// </summary>
        /// <returns>
        /// Enumerator for asynchronous enumeration over the sequence.
        /// </returns>
        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        IQueryProvider IQueryable.Provider
        {
            get { return new FileDiffDbAsyncQueryProvider<T>(this); }
        }
    }
}
