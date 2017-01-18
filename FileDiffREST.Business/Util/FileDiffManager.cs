//-----------------------------------------------------------------------
// <copyright file="FileDiffManager.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Util
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTO;

    /// <summary>
    /// Utility class that compares two files as the "left" file and the "right" file.
    /// </summary>
    public class FileDiffManager
    {
        /// <summary>
        /// Field holding the binary data for the file on the left side of comparison.
        /// </summary>
        private byte[] leftFile;

        /// <summary>
        /// Field holding the binary data for the file on the right side of comparison.
        /// </summary>
        private byte[] rightFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDiffManager"/> class. 
        /// </summary>
        public FileDiffManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDiffManager"/> class.
        /// </summary>
        /// <param name="leftFile">File on the left side of the comparison.</param>
        /// <param name="rightFile">File on the right side of comparison.</param>
        public FileDiffManager(byte[] leftFile, byte[] rightFile)
        {
            this.leftFile = leftFile;
            this.rightFile = rightFile;
        }

        /// <summary>
        /// Gets or sets the binary data for the file on the left side of comparison.
        /// </summary>
        public byte[] LeftFile
        {
            get
            {
                return this.leftFile;
            }

            set
            {
                this.leftFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the binary data for the file on the right side of comparison.
        /// </summary>
        public byte[] RightFile
        {
            get
            {
                return this.rightFile;
            }

            set
            {
                this.rightFile = value;
            }
        }

        /// <summary>
        /// Compares left file with right file and returns a <see cref="DifferenceDTO" /> object.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> of <see cref="DifferenceDTO" /> representing the results obtained from comparison.
        /// </returns>
        public async Task<DiffResultDTO> CompareAsync()
        {
            if (this.leftFile == null || this.leftFile.Length == 0)
            {
                throw new ArgumentNullException(nameof(this.leftFile));
            }

            if (this.rightFile == null || this.rightFile.Length == 0)
            {
                throw new ArgumentNullException(nameof(this.rightFile));
            }

            // Return "SizeDoNotMatch" if the sizes of files are not identical. 
            if (this.leftFile.Length != this.rightFile.Length)
            {
                return new DiffResultDTO { DiffResultType = ComparisonResult.SizeDoNotMatch.ToString() };
            }

            var diffs = await Task.Run(() => this.GetDifferences());

            // Return "Equals" if both files are identical in both content and size.
            if (diffs == null || diffs.Count == 0)
            {
                return new DiffResultDTO { DiffResultType = ComparisonResult.Equals.ToString() };
            }

            // Return the list of differences (as starting offset and length) if files are identical in size,
            // but not in content.
            return new DiffResultDTO
            {
                DiffResultType = ComparisonResult.ContentDoNotMatch.ToString(),
                Diffs = diffs
            };
        }

        /// <summary>
        /// Gets the differences between left and right files and returns a list of <see cref="DifferenceDTO"/> objects. 
        /// </summary>
        /// <returns>
        /// A list of <see cref="DifferenceDTO"/> objects for the offset and length of differences detected.
        /// </returns>
        private List<DifferenceDTO> GetDifferences()
        {
            // Throw an exception if method call is moved to somewhere else that cause mismatch in file lengths
            if (this.leftFile.Length != this.rightFile.Length)
            {
                throw new ArgumentException("Length of the both files should be the same in order to get their differences.");
            }

            var diffs = new List<DifferenceDTO>();

            // Variable indicates the offset where the diff was detected.
            // It is set to -1 if no diff is detected in the current offset.
            var diffDetectedOffset = -1;

            for (int i = 0; i < this.leftFile.Length; i++)
            {
                if (this.leftFile[i] != this.rightFile[i])
                {
                    // If it has not already been identified before, mark the current offset as diff detected
                    // and wait until the diff block ends for adding it to the list of diffs.
                    if (diffDetectedOffset < 0)
                    {
                        diffDetectedOffset = i;
                    }

                    continue;
                }

                if (diffDetectedOffset > -1)
                {
                    // As current offset is the one after the diff, length calculation is made accordingly.
                    // e.g. Diff is detected starting in offset 0, ending in 3 and the current offset is 4 
                    // => offset = 0; length = 4
                    var diffLength = i - diffDetectedOffset;
                    diffs.Add(new DifferenceDTO { Length = diffLength, Offset = diffDetectedOffset });

                    // diffdetectedOffset is set back to -1 in order to identify next diffs
                    diffDetectedOffset = -1;
                }
            }

            // Do not miss the case where the diff continues until the end of the array
            if (diffDetectedOffset > -1)
            {
                var diffLength = this.leftFile.Length - diffDetectedOffset;
                diffs.Add(new DifferenceDTO { Length = diffLength, Offset = diffDetectedOffset });
            }

            return diffs;
        }
    }
}
