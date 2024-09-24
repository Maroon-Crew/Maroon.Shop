namespace Maroon.Shop.Web.Models
{
    /// <summary>
    /// Represents an Address.
    /// </summary>
    public class AddressModel
    {
        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Address.
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// Gets or Sets a<see cref="string"/> representing the 'Name of recipient' of the Address.
        /// </summary>
        public string? NameOfRecipient { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Line 1' of the Address.
        /// </summary>
        public string? Line1 { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Line 2' of the Address.
        /// </summary>
        public string? Line2 { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Town' of the Address.
        /// </summary>
        public string? Town { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'County' of the Address.
        /// </summary>
        public string? County { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Post Code' of the Address.
        /// </summary>
        public string? PostCode { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Country' of the Address.
        /// </summary>
        public string? Country { get; set; }
    }
}