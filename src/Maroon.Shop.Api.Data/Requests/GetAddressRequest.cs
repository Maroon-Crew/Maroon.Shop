using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving an Address by it's identifier.
    /// </summary>
    public class GetAddressRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Address to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Address Id is required.")]
        public long AddressId { get; set; }
    }
}