//-----------------------------------------------------------------------
// <copyright file="IFileDiffContext.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Data.Interfaces
{
    using System.Data.Entity;
    using Models;

    /// <summary>
    /// The interface for DB context.
    /// </summary>
    public interface IFileDiffContext
    {
        /// <summary>
        /// Gets the files for comparison.
        /// </summary>
        /// <value>
        /// The files for comparison.
        /// </value>
        DbSet<FileComparison> FilesForComparison { get; }
        
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
