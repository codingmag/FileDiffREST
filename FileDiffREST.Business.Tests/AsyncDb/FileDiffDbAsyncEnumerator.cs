//-----------------------------------------------------------------------
// <copyright file="FileDiffDbAsyncEnumerator.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Tests.AsyncDb
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Enumerator for async DB conbtext.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <seealso cref="System.Data.Entity.Infrastructure.IDbAsyncEnumerator{T}" />
    class FileDiffDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        /// <summary>
        /// The inner
        /// </summary>
        private readonly IEnumerator<T> inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDiffDbAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        public FileDiffDbAsyncEnumerator(IEnumerator<T> inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.inner.Dispose();
        }

        /// <summary>
        /// Advances the enumerator to the next element in the sequence, returning the result asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the sequence.
        /// </returns>
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.inner.MoveNext());
        }

        /// <summary>
        /// Gets the current element in the iteration.
        /// </summary>
        public T Current
        {
            get { return this.inner.Current; }
        }

        /// <summary>
        /// Gets the current element in the iteration.
        /// </summary>
        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}
