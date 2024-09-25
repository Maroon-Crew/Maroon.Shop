// <auto-generated/>
#pragma warning disable CS0618
using Maroon.Shop.Api.Client.Models;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId
{
    /// <summary>
    /// Builds and executes requests for operations under \api\BasketItem\ByBasketId
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
    public partial class ByBasketIdRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ByBasketIdRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/BasketItem/ByBasketId?BasketId={BasketId}{&PageNumber*,PageSize*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ByBasketIdRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/BasketItem/ByBasketId?BasketId={BasketId}{&PageNumber*,PageSize*}", rawUrl)
        {
        }
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Models.BasketItemResponsePagedResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Maroon.Shop.Api.Client.Models.BasketItemResponsePagedResponse?> GetAsync(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder.ByBasketIdRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Maroon.Shop.Api.Client.Models.BasketItemResponsePagedResponse> GetAsync(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder.ByBasketIdRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            return await RequestAdapter.SendAsync<global::Maroon.Shop.Api.Client.Models.BasketItemResponsePagedResponse>(requestInfo, global::Maroon.Shop.Api.Client.Models.BasketItemResponsePagedResponse.CreateFromDiscriminatorValue, default, cancellationToken).ConfigureAwait(false);
        }
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder.ByBasketIdRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder.ByBasketIdRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json, text/plain;q=0.9");
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Maroon.Shop.Api.Client.Api.BasketItem.ByBasketId.ByBasketIdRequestBuilder(rawUrl, RequestAdapter);
        }
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
        #pragma warning disable CS1591
        public partial class ByBasketIdRequestBuilderGetQueryParameters 
        #pragma warning restore CS1591
        {
            public long? BasketId { get; set; }
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
        }
    }
}
#pragma warning restore CS0618
