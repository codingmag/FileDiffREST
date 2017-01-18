//-----------------------------------------------------------------------
// <copyright file="DiffService.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Services
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.DAL;
    using DTO;
    using Interfaces;
    using Util;

    /// <summary>
    /// The business layer service which provides methods for file difference operations.
    /// </summary>
    public class DiffService : IDiffService
    {
        /// <summary>
        /// The readonly field which holds the <see cref="FileDiffContext"/> instance.
        /// </summary>
        private readonly FileDiffContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffService"/> class.
        /// </summary>
        /// <param name="fileDiffContext">The instance of the <see cref="FileDiffContext"/>.</param>
        public DiffService(FileDiffContext fileDiffContext)
        {
            this.context = fileDiffContext;
        }

        /// <summary>
        /// Gets the diff of left and right files which are stored with the given ID.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>A <see cref="Task"/> object that includes <see cref="DiffResultDTO"/> for the list of differences.</returns>
        public async Task<DiffResultDTO> GetDiffAsync(int id)
        {
            var comparisonFiles = await this.context.FilesForComparison.Where(f => f.Id == id).FirstOrDefaultAsync();

            var diffManager = new FileDiffManager(comparisonFiles.Left, comparisonFiles.Right);
            var diffs = await diffManager.CompareAsync();

            return diffs;
        }
    }
}
