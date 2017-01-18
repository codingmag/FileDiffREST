//-----------------------------------------------------------------------
// <copyright file="ComparisonResult.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.DTO
{
    /// <summary>
    /// The enumeration which contains comparison results that the difference operation may return.
    /// </summary>
    public enum ComparisonResult
    {
        /// <summary>
        /// Indicates the case that both files are equal both in size and content.
        /// </summary>
        Equals,

        /// <summary>
        /// Indicates the case that sizes of the files are identical, however their content is not.
        /// </summary>
        ContentDoNotMatch,

        /// <summary>
        /// Indicates the case that sizes of the files do not match.
        /// </summary>
        SizeDoNotMatch
    }
}
