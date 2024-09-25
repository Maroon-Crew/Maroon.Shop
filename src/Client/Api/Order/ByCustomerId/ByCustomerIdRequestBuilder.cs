// <auto-generated/>
#pragma warning disable CS0618
using Maroon.Shop.Api.Client.Api.Order.ByCustomerId.Item;
using Maroon.Shop.Api.Client.Models;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Maroon.Shop.Api.Client.Api.Order.ByCustomerId
{
    /// <summary>
    /// Builds and executes requests for operations under \api\Order\ByCustomerId
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
    public partial class ByCustomerIdRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Maroon.Shop.Api.Client.api.Order.ByCustomerId.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.Item.WithCustomerItemRequestBuilder"/></returns>
        public global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.Item.WithCustomerItemRequestBuilder this[long position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("CustomerId", position);
                return new global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.Item.WithCustomerItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>Gets an item from the Maroon.Shop.Api.Client.api.Order.ByCustomerId.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.Item.WithCustomerItemRequestBuilder"/></returns>
        [Obsolete("This indexer is deprecated and will be removed in the next major version. Use the one with the typed parameter instead.")]
        public global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.Item.WithCustomerItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                if (!string.IsNullOrWhiteSpace(position)) urlTplParams.Add("CustomerId", position);
                return new global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.Item.WithCustomerItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ByCustomerIdRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/Order/ByCustomerId?CustomerId={CustomerId}{&PageNumber*,PageSize*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ByCustomerIdRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/Order/ByCustomerId?CustomerId={CustomerId}{&PageNumber*,PageSize*}", rawUrl)
        {
        }
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Models.OrderResponsePagedResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Maroon.Shop.Api.Client.Models.OrderResponsePagedResponse?> GetAsync(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder.ByCustomerIdRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Maroon.Shop.Api.Client.Models.OrderResponsePagedResponse> GetAsync(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder.ByCustomerIdRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            return await RequestAdapter.SendAsync<global::Maroon.Shop.Api.Client.Models.OrderResponsePagedResponse>(requestInfo, global::Maroon.Shop.Api.Client.Models.OrderResponsePagedResponse.CreateFromDiscriminatorValue, default, cancellationToken).ConfigureAwait(false);
        }
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder.ByCustomerIdRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder.ByCustomerIdRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder(rawUrl, RequestAdapter);
        }
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
        #pragma warning disable CS1591
        public partial class ByCustomerIdRequestBuilderGetQueryParameters 
        #pragma warning restore CS1591
        {
            public long? CustomerId { get; set; }
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
        }
        /// <summary>
        /// Configuration for the request such as headers, query parameters, and middleware options.
        /// </summary>
        [Obsolete("This class is deprecated. Please use the generic RequestConfiguration class generated by the generator.")]
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
        public partial class ByCustomerIdRequestBuilderGetRequestConfiguration : RequestConfiguration<global::Maroon.Shop.Api.Client.Api.Order.ByCustomerId.ByCustomerIdRequestBuilder.ByCustomerIdRequestBuilderGetQueryParameters>
        {
        }
    }
}
#pragma warning restore CS0618
