// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Maroon.Shop.Api.Client.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.18.0")]
    #pragma warning disable CS1591
    public partial class BasketItemResponse : IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The basketId property</summary>
        public long? BasketId { get; set; }
        /// <summary>The basketItemId property</summary>
        public long? BasketItemId { get; set; }
        /// <summary>The productId property</summary>
        public long? ProductId { get; set; }
        /// <summary>The quantity property</summary>
        public int? Quantity { get; set; }
        /// <summary>The totalPrice property</summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>The unitPrice property</summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Maroon.Shop.Api.Client.Models.BasketItemResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Maroon.Shop.Api.Client.Models.BasketItemResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Maroon.Shop.Api.Client.Models.BasketItemResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "basketId", n => { BasketId = n.GetLongValue(); } },
                { "basketItemId", n => { BasketItemId = n.GetLongValue(); } },
                { "productId", n => { ProductId = n.GetLongValue(); } },
                { "quantity", n => { Quantity = n.GetIntValue(); } },
                { "totalPrice", n => { TotalPrice = n.GetDecimalValue(); } },
                { "unitPrice", n => { UnitPrice = n.GetDecimalValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteLongValue("basketId", BasketId);
            writer.WriteLongValue("basketItemId", BasketItemId);
            writer.WriteLongValue("productId", ProductId);
            writer.WriteIntValue("quantity", Quantity);
            writer.WriteDecimalValue("totalPrice", TotalPrice);
            writer.WriteDecimalValue("unitPrice", UnitPrice);
        }
    }
}
#pragma warning restore CS0618
