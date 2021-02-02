using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Adnc.Infr.Consul.Consumer
{
    public class SimpleAuthHeaderHandler : DelegatingHandler
    {
        //private readonly IHttpContextAccessor authTokenStore;

        //public SimpleAuthHeaderHandler(IHttpContextAccessor authTokenStore, HttpMessageHandler innerHandler = null)
        //   : base(innerHandler ?? new HttpClientHandler())
        //{
        //    this.authTokenStore = authTokenStore ?? throw new ArgumentNullException(nameof(authTokenStore));
        //}

        private readonly Func<Task<string>> _token;
        public Guid ContextId { get; private set; }

        public SimpleAuthHeaderHandler(Func<Task<string>> token = null)
        {
           _token = token;
            this.ContextId = Guid.NewGuid();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var token = authTokenStore.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            //potentially refresh token here if it has expired etc.
            //if(!string.IsNullOrEmpty(token))
            //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //    //request.Headers.Add("X-Tenant-Id", tenantProvider.GetTenantId());

            //return await base.SendAsync(request, cancellationToken);

            var auth = request.Headers.Authorization;
            if (auth != null && _token != null)
            {
                var tokenTxt = await _token();
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
            }
            var responseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return responseMessage;
        }
    }
}
