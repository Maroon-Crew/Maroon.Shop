// <auto-generated/>
#pragma warning disable CS0618
using Maroon.Shop.Api.Client.Api.Basket.ByCustomerId;
using Maroon.Shop.Api.Client.Api.Basket.Create;
using Maroon.Shop.Api.Client.Api.Basket.Item;
using Maroon.Shop.Api.Client.Api.Basket.Update;
using Maroon.Shop.Api.Client.Models;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Maroon.Shop.Api.Client.Api.Basket
{
    /// <summary>
    /// Builds and executes requests for operations under \api\Basket
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
    public partial class BasketRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The ByCustomerId property</summary>
        public global::Maroon.Shop.Api.Client.Api.Basket.ByCustomerId.ByCustomerIdRequestBuilder ByCustomerId
        {
            get => new global::Maroon.Shop.Api.Client.Api.Basket.ByCustomerId.ByCustomerIdRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The Create property</summary>
        public global::Maroon.Shop.Api.Client.Api.Basket.Create.CreateRequestBuilder Create
        {
            get => new global::Maroon.Shop.Api.Client.Api.Basket.Create.CreateRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The Update property</summary>
        public global::Maroon.Shop.Api.Client.Api.Basket.Update.UpdateRequestBuilder Update
        {
            get => new global::Maroon.Shop.Api.Client.Api.Basket.Update.UpdateRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Maroon.Shop.Api.Client.api.Basket.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Api.Basket.Item.WithBasketItemRequestBuilder"/></returns>
        public global::Maroon.Shop.Api.Client.Api.Basket.Item.WithBasketItemRequestBuilder this[long position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("BasketId", position);
                return new global::Maroon.Shop.Api.Client.Api.Basket.Item.WithBasketItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>Gets an item from the Maroon.Shop.Api.Client.api.Basket.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Api.Basket.Item.WithBasketItemRequestBuilder"/></returns>
        [Obsolete("This indexer is deprecated and will be removed in the next major version. Use the one with the typed parameter instead.")]
        public global::Maroon.Shop.Api.Client.Api.Basket.Item.WithBasketItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                if (!string.IsNullOrWhiteSpace(position)) urlTplParams.Add("BasketId", position);
                return new global::Maroon.Shop.Api.Client.Api.Basket.Item.WithBasketItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public BasketRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/Basket{?PageNumber*,PageSize*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public BasketRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/Basket{?PageNumber*,PageSize*}", rawUrl)
        {
        }
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Models.BasketResponsePagedResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Maroon.Shop.Api.Client.Models.BasketResponsePagedResponse?> GetAsync(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder.BasketRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Maroon.Shop.Api.Client.Models.BasketResponsePagedResponse> GetAsync(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder.BasketRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            return await RequestAdapter.SendAsync<global::Maroon.Shop.Api.Client.Models.BasketResponsePagedResponse>(requestInfo, global::Maroon.Shop.Api.Client.Models.BasketResponsePagedResponse.CreateFromDiscriminatorValue, default, cancellationToken).ConfigureAwait(false);
        }
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder.BasketRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder.BasketRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder(rawUrl, RequestAdapter);
        }
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
        #pragma warning disable CS1591
        public partial class BasketRequestBuilderGetQueryParameters 
        #pragma warning restore CS1591
        {
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
        }
        /// <summary>
        /// Configuration for the request such as headers, query parameters, and middleware options.
        /// </summary>
        [Obsolete("This class is deprecated. Please use the generic RequestConfiguration class generated by the generator.")]
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
        public partial class BasketRequestBuilderGetRequestConfiguration : RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Basket.BasketRequestBuilder.BasketRequestBuilderGetQueryParameters>
        {
        }
    }
}
#pragma warning restore CS0618
