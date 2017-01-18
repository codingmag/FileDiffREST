//-----------------------------------------------------------------------
// <copyright file="DTOToEntityMappingProfile.cs" company=".">
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
    /// Configures the AutoMapper mapping profile for DTO to entity mappings in service layer.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class DTOToEntityMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DTOToEntityMappingProfile"/> class.
        /// </summary>
        public DTOToEntityMappingProfile()
        {
            this.CreateMap<FileComparisonDTO, FileComparison>();
        }

        /// <summary>
        /// Gets the name of the profile.
        /// </summary>
        /// <value>
        /// The name of the profile.
        /// </value>
        public override string ProfileName => "DTOToEntityMappings";
    }
}
