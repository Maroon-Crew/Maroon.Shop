using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Web.Models
{
    /// <summary>
    /// Represents a Customer.
    /// </summary>
    public class CustomerModel
    {
        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Customer.
        /// </summary>
        public required long CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'First Name' of the Customer.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Last Name' of the Customer.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Email Address' of the Customer.
        /// </summary>
        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(50, ErrorMessage = "Email address cannot exceed 50 characters.")]
        public required string EmailAddress { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Customer's Billing Address.
        /// </summary>
        public required long BillingAddressId { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Customer's Shipping Address.
        /// </summary>
        public required long DefaultShippingAddressId { get; set; }

        /// <summary>
        /// Gets or Sets an <see cref="AddressModel"/> representing the Customer's Billing Address.
        /// </summary>
        public AddressModel? BillingAddress { get; set; }

        /// <summary>
        /// Gets or Sets an <see cref="AddressModel"/> representing the Customer's Shipping Address.
        /// </summary>
        public AddressModel? ShippingAddress { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="bool"/> indicating if the Customer's Shipping Address is the same as the Customer's Billing Address.
        /// </summary>
        public bool IsShippingAddressTheSameAsBillingAddress { get; set; }
    }
}