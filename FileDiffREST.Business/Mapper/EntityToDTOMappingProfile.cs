//-----------------------------------------------------------------------
// <copyright file="EntityToDTOMappingProfile.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Mapper
{
    using AutoMapper;
    using Data.Models;
    using DTO;

    /// <summary>
    /// Configures the AutoMapper mapping profile for entity to DTO mappings in service layer.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class EntityToDTOMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityToDTOMappingProfile"/> class.
        /// </summary>
        public EntityToDTOMappingProfile()
        {
            this.CreateMap<FileComparison, FileComparisonDTO>();
        }

        /// <summary>
        /// Gets the name of the profile.
        /// </summary>
        /// <value>
        /// The name of the profile.
        /// </value>
        public override string ProfileName => "EntityToDTOMappings";
    }
}
