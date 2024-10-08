﻿namespace Maroon.Shop.Api.Data.Responses
{
    /// <summary>
    /// Represents a response model for a Product.
    /// </summary>
    public class ProductResponse
    {
        /// <summary>
        /// Gets or Sets the unique identifier for the Product.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Gets or Sets the name of the Product. This field is required.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or Sets the description of the Product. This field is required.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or Sets the notes of the Product. This field is required.
        /// </summary>
        public string? PleaseNote { get; set; }

        /// <summary>
        /// Gets or Sets the URL-friendly version of the Product name. This field is required.
        /// </summary>
        public required string UrlFriendlyName { get; set; }

        /// <summary>
        /// Gets or Sets the Image URL of the Product. This field is required.
        /// </summary>
        public required string ImageUrl { get; set; }

        // <summary>
        /// Gets or Sets the price of the Product.
        /// </summary>
        public decimal Price { get; set; }
    }
}