using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FileDiffREST.Web.ActionResults
{
    public class CreatedFileActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;

        public CreatedFileActionResult(HttpRequestMessage request)
        {
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.Created);
            return Task.FromResult(response);
        }
    }
}