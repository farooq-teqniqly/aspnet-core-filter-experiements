using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ApiHttpHandler : DelegatingHandler
    {
        private readonly string token;

        public ApiHttpHandler(string token)
        {
            this.token = token;
            this.InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;

            if (authHeader == null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
            }

            var accept = request.Headers.Accept;

            if (accept == null || accept.All(m => m.MediaType != "application/json"))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
