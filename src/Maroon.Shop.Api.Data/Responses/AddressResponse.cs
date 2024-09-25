namespace Maroon.Shop.Api.Data.Responses
{
    /// <summary>
    /// Represents a response model for an Address.
    /// </summary>
    public class AddressResponse
    {
        /// <summary>
        /// Gets or Sets the unique identifier for the Address.
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// Gets or Sets the name of the recipient. This field is required.
        /// </summary>
        public required string NameOfRecipient { get; set; }

        /// <summary>
        /// Gets or Sets the Line 1 of the Address. This field is required.
        /// </summary>
        public required string Line1 { get; set; }

        /// <summary>
        /// Gets or Sets the Line 2 of the Address.
        /// </summary>
        public string? Line2 { get; set; }

        /// <summary>
        /// Gets or Sets the Town of the Address.
        /// </summary>
        public string? Town { get; set; }

        /// <summary>
        /// Gets or Sets the County of the Address.
        /// </summary>
        public string? County { get; set; }

        /// <summary>
        /// Gets or Sets the PostCode of the Address.
        /// </summary>
        public required string PostCode { get; set; }

        /// <summary>
        /// Gets or Sets the Country of the Address.
        /// </summary>
        public string? Country { get; set; }
    }
}