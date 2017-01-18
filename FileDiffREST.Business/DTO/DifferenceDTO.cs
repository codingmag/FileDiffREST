//-----------------------------------------------------------------------
// <copyright file="DifferenceDTO.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.DTO
{
    /// <summary>
    /// The DTO for collecting the file difference information.
    /// </summary>
    public class DifferenceDTO
    {
        /// <summary>
        /// Gets or sets the offset where a difference exists.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the length of the difference starting from a certain offset.
        /// </summary>
        public int Length { get; set; }
    }
}
