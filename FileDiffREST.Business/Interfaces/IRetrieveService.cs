//-----------------------------------------------------------------------
// <copyright file="IRetrieveService.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Interfaces
{
    using System.Threading.Tasks;
    using DTO;
    
    /// <summary>
    /// The interface for the service to provide file retrieving operations.
    /// </summary>
    public interface IRetrieveService
    {
        /// <summary>
        /// Gets the left and right files to be compared within the given comparison ID.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>A <see cref="Task"/> object representing the result of the async operation.</returns>
        Task<FileComparisonDTO> GetFilesAsync(int id);

        /// <summary>
        /// Tests if the left file exists in comparison record.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>True if it exists, false if not.</returns>
        Task<bool> LeftFileExistsAsync(int id);

        /// <summary>
        /// Tests if the right file exists in comparison record.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>True if it exists, false if not.</returns>
        Task<bool> RightFileExistsAsync(int id);
    }
}
