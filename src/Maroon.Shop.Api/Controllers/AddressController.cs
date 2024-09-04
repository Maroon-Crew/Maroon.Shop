using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Address Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Address".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Address Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public AddressController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Address/{addressId}".
        /// Attempts to retrieve an Address for the given Address Id.
        /// </summary>
        /// <param name="getAddressRequest">A <see cref="GetAddressRequest"/> representing the Address Requested.</param>
        /// <returns>An <see cref="ActionResult{AddressResponse}"/> representing the Address found.</returns>
        [HttpGet]
        [Route("{AddressId}")]
        public ActionResult<AddressResponse> GetById([FromRoute] GetAddressRequest getAddressRequest)
        {
            if (getAddressRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Query for an Address with the given addressId.
            var query = _context.Addresses.Where(address => address.AddressId == getAddressRequest.AddressId);

            if (!query.Any())
            {
                // The Address could not be found, return a 404 Not Found response.
                return NotFound();
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

                return Ok(addressResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Address/".
        /// Attempts to retrieve all Addresses.
        /// </summary>
        /// <param name="getAddressesRequest">A <see cref="GetAddressesRequest"/> representing the Addresses Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Address}}"/> representing the Addreses found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<Address>> GetAddresses([FromQuery] GetAddressesRequest getAddressesRequest)
        {
            if (getAddressesRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            // Retrieve all Products using pagination.
            var addresses = _context.Addresses
                .OrderBy(addresses => addresses.AddressId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getAddressesRequest.PageNumber - 1) * getAddressesRequest.PageSize)
                .Take(getAddressesRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Addresses.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<Address>(
                 data: addresses,
                 pageNumber: getAddressesRequest.PageNumber,
                 pageSize: getAddressesRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/Address/ByPostCode".
        /// Attempts to retrieve all Addresses that start with the given postCode.
        /// </summary>
        /// <param name="getAddressesByPostCodeRequest">A <see cref="GetAddressesByPostCodeRequest"/> representing the Addresses By PostCode Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Address}}"/> representing the Addresses found.</returns>
        [HttpGet("ByPostCode")]
        public ActionResult<PagedResponse<Address>> GetAddressesByPostCode([FromQuery] GetAddressesByPostCodeRequest getAddressesByPostCodeRequest)
        {
            if (getAddressesByPostCodeRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Retrieve all Addresses that Start with the given Post Code.
            var filteredAddresses = _context.Addresses
                .Where(address => address.PostCode.StartsWith(getAddressesByPostCodeRequest.PostCode));
            
            // Apply Pagination.
            var filteredAddressesPaginated = filteredAddresses
                .Skip((getAddressesByPostCodeRequest.PageNumber - 1) * getAddressesByPostCodeRequest.PageSize)
                .Take(getAddressesByPostCodeRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredAddresses.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<Address>(
                 data: filteredAddressesPaginated,
                 pageNumber: getAddressesByPostCodeRequest.PageNumber,
                 pageSize: getAddressesByPostCodeRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getAddressesByPostCodeRequest.PostCode } // Pass in the PostCode Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles POST requests to "api/Address/Create".
        /// Attempts to create a new Address.
        /// </summary>
        /// <param name="createAddressRequest">A <see cref="CreateAddressRequest"/> representing the new Address to be created.</param>
        /// <returns>An <see cref="ActionResult{AddressResponse}"/> representing the created Product.</returns>
        [HttpPost("Create")]
        public ActionResult<AddressResponse> CreateAddress([FromBody] CreateAddressRequest createAddressRequest)
        {
            if (createAddressRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

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

            // Return the created Address with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { AddressId = address.AddressId }, addressResponse);
        }

        /// <summary>
        /// Handles PUT requests to "api/Address/Update".
        /// Attempts to update an existing Address.
        /// </summary>
        /// <param name="addressId">A <see cref="long"/> representing the Id of the Address to update.</param>
        /// <param name="updateAddressRequest">An <see cref="UpdateAddressRequest"/> representing the updated Address data.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("Update")]
        public IActionResult UpdateAddress(long addressId, [FromBody] UpdateAddressRequest updateAddressRequest)
        {
            if (updateAddressRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            if (updateAddressRequest.AddressId != addressId)
            {
                // The addressId does not match the Address. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Address Id mismatch.");
            }

            // Attempt to get the Address to be updated.
            var existingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == addressId);
            if (existingAddress == null)
            {
                // The Address to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return NotFound();
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
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

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}