//-----------------------------------------------------------------------
// <copyright file="DifferenceDTOComparer.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Tests.Core.Comparers
{
    using System;
    using System.Collections;
    using Business.DTO;

    /// <summary>
    /// Custom comparer for comparing instances of <see cref="DifferenceDTO"/> in tests.
    /// </summary>
    /// <seealso cref="System.Collections.IComparer" />
    public class DifferenceDTOComparer : IComparer
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public int Compare(object x, object y)
        {
            var leftDiffDTO = x as DifferenceDTO;
            var rightDiffDTO = y as DifferenceDTO;
            if (leftDiffDTO == null || rightDiffDTO == null)
            {
                throw new InvalidOperationException();
            }

            return Compare(leftDiffDTO, rightDiffDTO);
        }

        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public int Compare(DifferenceDTO x, DifferenceDTO y)
        {
            if (x.Offset == y.Offset && x.Length == y.Length)
            {
                return 0;
            }

            if (x.Offset < y.Offset)
            {
                return 1;
            }

            return -1;
        }
    }
}
