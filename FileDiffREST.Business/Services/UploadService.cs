//-----------------------------------------------------------------------
// <copyright file="UploadService.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Services
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.DAL;
    using Data.Models;
    using DTO;
    using Interfaces;

    /// <summary>
    /// The business layer service which provides methods for file upload operations.
    /// </summary>
    public class UploadService : IUploadService
    {
        /// <summary>
        /// The readonly field that holds <see cref="FileDiffContext"/> instance.
        /// </summary>
        private readonly FileDiffContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadService"/> class.
        /// </summary>
        /// <param name="fileDiffContext">The instance of the <see cref="FileDiffContext"/>.</param>
        public UploadService(FileDiffContext fileDiffContext)
        {
            this.context = fileDiffContext;
        }

        /// <summary>
        /// Uploads the file which is in the left side of the comparison.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <param name="content">Binary content of the file.</param>
        /// <returns>
        /// A <see cref="Task" /> object for the result of the async task.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">content</exception>
        public async Task<FileComparisonDTO> UploadLeftAsync(int id, byte[] content)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var files = await this.context.FilesForComparison.Where(f => f.Id == id).FirstOrDefaultAsync();

            if (files != null)
            {
                files.Left = content;
            }
            else
            {
                files = new FileComparison
                {
                    Left = content,
                    Id = id
                };
                this.context.FilesForComparison.Add(files);
            }

            await this.context.SaveChangesAsync();

            return AutoMapper.Mapper.Map<FileComparison, FileComparisonDTO>(files);
        }

        /// <summary>
        /// Uploads the file which is in the right side of the comparison.
        /// </summary>
        /// <param name="id">The comparison ID.</param>
        /// <param name="content">Binary content of the file.</param>
        /// <returns>
        /// A <see cref="Task" /> object for the result of the async task.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">content</exception>
        public async Task<FileComparisonDTO> UploadRightAsync(int id, byte[] content)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var files = await this.context.FilesForComparison.Where(f => f.Id == id).FirstOrDefaultAsync();

            if (files != null)
            {
                files.Right = content;
            }
            else
            {
                files = new FileComparison
                {
                    Right = content,
                    Id = id
                };
                this.context.FilesForComparison.Add(files);
            }

            await this.context.SaveChangesAsync();

            return AutoMapper.Mapper.Map<FileComparison, FileComparisonDTO>(files);
        }
    }
}
