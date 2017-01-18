//-----------------------------------------------------------------------
// <copyright file="FileDiffController.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using ActionResults;
    using Business.DTO;
    using Business.Interfaces;

    /// <summary>
    /// The controller for the REST endpoints that provide FileDiff operations.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class FileDiffController : ApiController
    {
        /// <summary>
        /// The difference service
        /// </summary>
        private readonly IDiffService _diffService;

        /// <summary>
        /// The retrieve service
        /// </summary>
        private readonly IRetrieveService _retrieveService;

        /// <summary>
        /// The upload service
        /// </summary>
        private readonly IUploadService _uploadService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDiffController"/> class.
        /// </summary>
        /// <param name="diffService">The difference service.</param>
        /// <param name="retrieveService">The retrieve service.</param>
        /// <param name="uploadService">The upload service.</param>
        public FileDiffController(IDiffService diffService, IRetrieveService retrieveService, IUploadService uploadService)
        {
            _diffService = diffService;
            _uploadService = uploadService;
            _retrieveService = retrieveService;
        }

        /// <summary>
        /// Gets the difference asynchronous.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        /// <returns></returns>
        [Route("v1/diff/{diffId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDiffAsync(int diffId)
        {
            if (!await _retrieveService.LeftFileExistsAsync(diffId) || !await _retrieveService.RightFileExistsAsync(diffId))
            {
                return NotFound();
            }
            
            var diffResult = await _diffService.GetDiffAsync(diffId);
            return Ok(diffResult);
        }

        /// <summary>
        /// Puts the left file asynchronous.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        /// <param name="fileContent">Content of the file.</param>
        /// <returns></returns>
        [Route("v1/diff/{diffId}/left")]
        [HttpPut]
        public async Task<IHttpActionResult> PutLeftFileAsync(int diffId, [FromBody] FileContentDTO fileContent)
        {
            if (string.IsNullOrEmpty(fileContent.Data))
            {
                return BadRequest();
            }

            await _uploadService.UploadLeftAsync(diffId, Convert.FromBase64String(fileContent.Data));
            
            return new CreatedFileActionResult(Request);
        }

        /// <summary>
        /// Puts the right file asynchronous.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        /// <param name="fileContent">Content of the file.</param>
        /// <returns></returns>
        [Route("v1/diff/{diffId}/right")]
        [HttpPut]
        public async Task<IHttpActionResult> PutRightFileAsync(int diffId, [FromBody] FileContentDTO fileContent)
        {
            if (string.IsNullOrEmpty(fileContent.Data))
            {
                return BadRequest();
            }

            await _uploadService.UploadRightAsync(diffId, Convert.FromBase64String(fileContent.Data));

            return new CreatedFileActionResult(Request);
        }
    }
}
