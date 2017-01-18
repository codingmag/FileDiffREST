﻿//-----------------------------------------------------------------------
// <copyright file="FileDiffContext.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Data.DAL
{
    using System.Data.Entity;
    using Interfaces;
    using Models;

    /// <summary>
    /// The DB context for EF.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    /// <seealso cref="FileDiffREST.Data.Interfaces.IFileDiffContext" />
    public class FileDiffContext : DbContext, IFileDiffContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDiffContext"/> class.
        /// </summary>
        public FileDiffContext() : base("FileDiffConnection")
        {
        }

        /// <summary>
        /// Gets or sets the files for comparison.
        /// </summary>
        /// <value>
        /// The files for comparison.
        /// </value>
        public virtual DbSet<FileComparison> FilesForComparison { get; set; }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileComparison>().ToTable("FilesForComparison");
        }
    }
}
