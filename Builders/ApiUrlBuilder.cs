using Microsoft.AspNetCore.Http;

namespace LibApp.Builders
{
    public class ApiUrlBuilder : IApiUrlBuilder
    {
        private string _hostUrl;

        public ApiUrlBuilder(IHttpContextAccessor contextAccessor)
        {
            _hostUrl = $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}";
        }

        public string BuildApiUrl(string endpointUrl)
        {
            return _hostUrl + endpointUrl;
        }
    }
}
