namespace Maroon.Shop.Api.Client
{
    using Microsoft.Kiota.Abstractions.Authentication;
    using Microsoft.Kiota.Http.HttpClientLibrary;

    public class MaroonClientFactory
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly HttpClient _httpClient;

        public MaroonClientFactory(HttpClient httpClient)
        {
            _authenticationProvider = new AnonymousAuthenticationProvider();
            _httpClient = httpClient;
        }

        public MaroonClient GetClient()
        {
            return new MaroonClient(new HttpClientRequestAdapter(_authenticationProvider, httpClient: _httpClient));
        }
    }
}
