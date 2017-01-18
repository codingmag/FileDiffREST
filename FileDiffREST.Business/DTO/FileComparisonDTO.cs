//-----------------------------------------------------------------------
// <copyright file="FileComparisonDTO.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.DTO
{
    /// <summary>
    /// The DTO which contains the files to be compared.
    /// </summary>
    public class FileComparisonDTO
    {
        /// <summary>
        /// Gets or sets the ID of the comparison.
        /// </summary>
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
