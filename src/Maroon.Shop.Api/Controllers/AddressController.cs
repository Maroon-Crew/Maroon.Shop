using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Microsoft.AspNetCore.Mvc;

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
        private readonly AddressRepository _addressRepository;

        public AddressController(AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
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

            var addressResponse = _addressRepository.GetById(getAddressRequest);

            if (addressResponse == null)
            {
                // The Address could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                return Ok(addressResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Address/".
        /// Attempts to retrieve all Addresses.
        /// </summary>
        /// <param name="getAddressesRequest">A <see cref="GetAddressesRequest"/> representing the Addresses Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{AddressResponse}}"/> representing the Addreses found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<AddressResponse>> GetAddresses([FromQuery] GetAddressesRequest getAddressesRequest)
        {
            if (getAddressesRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = _addressRepository.GetAddresses(getAddressesRequest, routeName, Url);

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/Address/ByPostCode".
        /// Attempts to retrieve all Addresses that start with the given postCode.
        /// </summary>
        /// <param name="getAddressesByPostCodeRequest">A <see cref="GetAddressesByPostCodeRequest"/> representing the Addresses By PostCode Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{AddressResponse}}"/> representing the Addresses found.</returns>
        [HttpGet("ByPostCode")]
        public ActionResult<PagedResponse<AddressResponse>> GetAddressesByPostCode([FromQuery] GetAddressesByPostCodeRequest getAddressesByPostCodeRequest)
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

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = _addressRepository.GetAddressesByPostCode(getAddressesByPostCodeRequest, routeName, Url);

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

            // Map the Address entity to a new response object.
            var addressResponse = _addressRepository.CreateAddress(createAddressRequest);

            if (addressResponse == null)
            {
                return BadRequest();
            }

            // Return the created Address with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { AddressId = addressResponse.AddressId }, addressResponse);
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
            var existingAddress = _addressRepository.GetById(new GetAddressRequest { AddressId = addressId });
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

            _addressRepository.UpdateAddress(addressId, updateAddressRequest);

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}