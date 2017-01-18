//-----------------------------------------------------------------------
// <copyright file="IUploadService.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Interfaces
{
    using System.Threading.Tasks;
    using DTO;

    /// <summary>
    /// The interface for the service to provide file uploading operations.
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Uploads the file which is in the left side of the comparison.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <param name="content">Binary content of the file.</param>
        /// <returns>A <see cref="Task"/> object for the result of the async task.</returns>
        Task<FileComparisonDTO> UploadLeftAsync(int id, byte[] content);

        /// <summary>
        /// Uploads the file which is in the right side of the comparison.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <param name="content">Binary content of the file.</param>
        /// <returns>A <see cref="Task"/> object for the result of the async task.</returns>
        Task<FileComparisonDTO> UploadRightAsync(int id, byte[] content);
    }
}
