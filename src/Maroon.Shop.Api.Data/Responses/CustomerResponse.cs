namespace Maroon.Shop.Api.Data.Responses
{
    public class CustomerResponse
    {
        public long CustomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public long BillingAddressId { get; set; }
        public long DefaultShippingAddressId { get; set; }
    }
}