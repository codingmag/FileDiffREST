//-----------------------------------------------------------------------
// <copyright file="IDiffService.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Interfaces
{
    using System.Threading.Tasks;
    using DTO;

    /// <summary>
    /// The interface for the service to provide file difference operations.
    /// </summary>
    public interface IDiffService
    {
        /// <summary>
        /// Gets the diff of left and right files which are stored with the given ID.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>A <see cref="Task"/> object that includes <see cref="DiffResultDTO"/> for the list of differences.</returns>
        Task<DiffResultDTO> GetDiffAsync(int id);
    }
}
