//-----------------------------------------------------------------------
// <copyright file="DiffResultDTO.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO which contains information for the result of the diff operation.
    /// </summary>
    public class DiffResultDTO
    {
        /// <summary>
        /// Gets or sets the type of the difference after comparison.
        /// </summary>
        public string DiffResultType { get; set; }

        /// <summary>
        /// Gets or sets the list of differences.
        /// </summary>
        public List<DifferenceDTO> Diffs { get; set; }
    }
}
