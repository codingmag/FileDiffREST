//-----------------------------------------------------------------------
// <copyright file="RetrieveService.cs" company=".">
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
    using Data.Models;
    using DTO;
    using Interfaces;

    /// <summary>
    /// The business layer service which provides methods for file retrieving operations.
    /// </summary>
    public class RetrieveService : IRetrieveService
    {
        /// <summary>
        /// The readonly field which holds the <see cref="FileDiffContext"/> instance.
        /// </summary>
        private readonly FileDiffContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveService"/> class.
        /// </summary>
        /// <param name="fileDiffContext">The instance of the <see cref="FileDiffContext"/>.</param>
        public RetrieveService(FileDiffContext fileDiffContext)
        {
            this.context = fileDiffContext;
        }

        /// <summary>
        /// Gets the left and right files to be compared within the given comparison ID.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>A <see cref="Task"/> object representing the result of the async operation.</returns>
        public async Task<FileComparisonDTO> GetFilesAsync(int id)
        {
            var files = await this.context.FilesForComparison.Where(f => f.Id == id).FirstOrDefaultAsync();

            return files == null ? null : AutoMapper.Mapper.Map<FileComparison, FileComparisonDTO>(files);
        }

        /// <summary>
        /// Tests if the left file exists in comparison record.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>True if it exists, false if not.</returns>
        public async Task<bool> LeftFileExistsAsync(int id)
        {
            var files = await this.context.FilesForComparison.Where(f => f.Id == id).FirstOrDefaultAsync();

            return files?.Left != null;
        }

        /// <summary>
        /// Tests if the right file exists in comparison record.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <returns>True if it exists, false if not.</returns>
        public async Task<bool> RightFileExistsAsync(int id)
        {
            var files = await this.context.FilesForComparison.Where(f => f.Id == id).FirstOrDefaultAsync();

            return files?.Right != null;
        }
    }
}
