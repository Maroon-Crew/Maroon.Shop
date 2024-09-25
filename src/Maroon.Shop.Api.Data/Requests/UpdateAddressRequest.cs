using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for updating an existing Address.
    /// </summary>
    public class UpdateAddressRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Address to be updated.
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// Gets or Sets the name of the recipient. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Name Of Recipient is required.")]
        [StringLength(50, ErrorMessage = "Name Of Recipient cannot exceed 50 characters.")]
        public required string NameOfRecipient { get; set; }

        /// <summary>
        /// Gets or Sets the Line 1 of the Address. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Line 1 is required.")]
        [StringLength(50, ErrorMessage = "Line 1 cannot exceed 50 characters.")]
        public required string Line1 { get; set; }

        /// <summary>
        /// Gets or Sets the Line 2 of the Address.
        /// </summary>
        [StringLength(50, ErrorMessage = "Line 2 cannot exceed 50 characters.")]
        public string? Line2 { get; set; }

        /// <summary>
        /// Gets or Sets the Town of the Address.
        /// </summary>
        [StringLength(50, ErrorMessage = "Town cannot exceed 50 characters.")]
        public string? Town { get; set; }

        /// <summary>
        /// Gets or Sets the County of the Address.
        /// </summary>
        [StringLength(50, ErrorMessage = "County cannot exceed 50 characters.")]
        public string? County { get; set; }

        /// <summary>
        /// Gets or Sets the PostCode of the Address.
        /// </summary>
        [Required(ErrorMessage = "Post Code is required.")]
        [StringLength(50, ErrorMessage = "Post Code cannot exceed 50 characters.")]
        public required string PostCode { get; set; }

        /// <summary>
        /// Gets or Sets the Country of the Address.
        /// </summary>
        /// 
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string? Country { get; set; }
    }
}