//-----------------------------------------------------------------------
// <copyright file="AutoMapperServiceLayerConfiguration.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Business.Mapper
{
    /// <summary>
    /// Configures AutoMapper for mappings between DTOs and entities in service layer.
    /// </summary>
    public class AutoMapperServiceLayerConfiguration
    {
        /// <summary>
        /// Configures this mapper instance.
        /// </summary>
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(x =>
            {
                x.AddProfile<DTOToEntityMappingProfile>();
                x.AddProfile<EntityToDTOMappingProfile>();
            });
        }
    }
}
