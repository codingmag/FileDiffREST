//-----------------------------------------------------------------------
// <copyright file="TestConstants.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Tests.Core
{
    /// <summary>
    /// Static class that contains constant definitions for unit tests.
    /// </summary>
    public static class TestConstants
    {
        /// <summary>
        /// Base64 content of file to be compared.
        /// </summary>
        public const string File1Base64Content = "AAAAAA==";

        /// <summary>
        /// Base64 content of file which is in identical size but having different content from the original one to be compared.
        /// </summary>
        public const string FileIdenticalSizeNonIdenticalContentBase64Content = "AQABAQ==";

        /// <summary>
        /// Base64 content of file which is in different size from the original one to be compared.
        /// </summary>
        public const string FileDifferentSizeBase64Content = "AAA=";
    }
}
