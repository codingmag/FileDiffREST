//-----------------------------------------------------------------------
// <copyright file="FileComparison.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model class for file instance to be uploaded.
    /// </summary>
    public class FileComparison
    {
        /// <summary>
        /// Gets or sets the ID of the comparison.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets binary content of the left file.
        /// </summary>
        public byte[] Left { get; set; }

        /// <summary>
        /// Gets or sets binary content of the right file.
        /// </summary>
        public byte[] Right { get; set; }
    }
}
