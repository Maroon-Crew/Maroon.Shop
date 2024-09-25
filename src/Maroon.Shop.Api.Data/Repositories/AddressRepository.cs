using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Data.Repositories
{
    public class AddressRepository
    {
        private readonly ShopContext _context;

        public AddressRepository(ShopContext context)
        {
            _context = context;
        }

        public AddressResponse? GetById(GetAddressRequest getAddressRequest)
        {
            // Query for an Address with the given addressId.
            var query = _context.Addresses.Where(address => address.AddressId == getAddressRequest.AddressId);

            if (!query.Any())
            {
                // The Address could not be found, return a 404 Not Found response.
                return null;
            }
            else
            {
                // Return the first matching Address and build the Response.
                var address = query.First();
                var addressResponse = new AddressResponse
                {
                    AddressId = address.AddressId,
                    NameOfRecipient = address.NameOfRecipient,
                    Line1 = address.Line1,
                    Line2 = address.Line2,
                    Town = address.Town,
                    County = address.County,
                    PostCode = address.PostCode,
                    Country = address.Country
                };

                return addressResponse;
            }
        }

        public PagedResponse<AddressResponse> GetAddresses(GetAddressesRequest getAddressesRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Products using pagination.
            var addresses = _context.Addresses
                .OrderBy(addresses => addresses.AddressId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getAddressesRequest.PageNumber - 1) * getAddressesRequest.PageSize)
                .Take(getAddressesRequest.PageSize)
                .Select(a => new AddressResponse
                {
                    AddressId = a.AddressId,
                    NameOfRecipient = a.NameOfRecipient,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    Town = a.Town,
                    County = a.County,
                    PostCode = a.PostCode,
                    Country = a.Country,
                })
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Addresses.Count();

            // Create the response.
            var response = new PagedResponse<AddressResponse>(
                 data: addresses,
                 pageNumber: getAddressesRequest.PageNumber,
                 pageSize: getAddressesRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName
             );

            return response;
        }

        public PagedResponse<AddressResponse> GetAddressesByPostCode(GetAddressesByPostCodeRequest getAddressesByPostCodeRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Addresses that Start with the given Post Code.
            var filteredAddresses = _context.Addresses
                .Where(address => address.PostCode.StartsWith(getAddressesByPostCodeRequest.PostCode))
                .Select(a => new AddressResponse
                {
                    AddressId = a.AddressId,
                    NameOfRecipient = a.NameOfRecipient,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    Town = a.Town,
                    County = a.County,
                    PostCode = a.PostCode,
                    Country = a.Country,
                });

            // Apply Pagination.
            var filteredAddressesPaginated = filteredAddresses
                .Skip((getAddressesByPostCodeRequest.PageNumber - 1) * getAddressesByPostCodeRequest.PageSize)
                .Take(getAddressesByPostCodeRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredAddresses.Count();

            // Create the response.
            var response = new PagedResponse<AddressResponse>(
                 data: filteredAddressesPaginated,
                 pageNumber: getAddressesByPostCodeRequest.PageNumber,
                 pageSize: getAddressesByPostCodeRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getAddressesByPostCodeRequest.PostCode } // Pass in the PostCode Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public AddressResponse CreateAddress(CreateAddressRequest createAddressRequest)
        {
            // Map the request object to a new Address entity.
            var address = new Address
            {
                NameOfRecipient = createAddressRequest.NameOfRecipient,
                Line1 = createAddressRequest.Line1,
                Line2 = createAddressRequest.Line2,
                Town = createAddressRequest.Town,
                County = createAddressRequest.County,
                PostCode = createAddressRequest.PostCode,
                Country = createAddressRequest.Country
            };

            // Add the new Address to the Database Context.
            _context.Addresses.Add(address);
            _context.SaveChanges();

            // Map the Address entity to a new response object.
            var addressResponse = new AddressResponse
            {
                AddressId = address.AddressId,
                NameOfRecipient = address.NameOfRecipient,
                Line1 = address.Line1,
                Line2 = address.Line2,
                Town = address.Town,
                County = address.County,
                PostCode = address.PostCode,
                Country = address.Country
            };

            return addressResponse;
        }

        public void UpdateAddress(long addressId, UpdateAddressRequest updateAddressRequest)
        {
            // Attempt to get the Address to be updated.
            var existingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == addressId);

            if (existingAddress == null)
            {
                return;
            }

            // Update the existing Address with the values from the provided Address.
            existingAddress.NameOfRecipient = updateAddressRequest.NameOfRecipient;
            existingAddress.Line1 = updateAddressRequest.Line1;
            existingAddress.Line2 = updateAddressRequest.Line2;
            existingAddress.Town = updateAddressRequest.Town;
            existingAddress.County = updateAddressRequest.County;
            existingAddress.PostCode = updateAddressRequest.PostCode;
            existingAddress.Country = updateAddressRequest.Country;

            // Save the changes to the database.
            _context.SaveChanges();
        }
    }
}