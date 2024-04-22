using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using Azure;


namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/vendor")]
    public class VendorController : ControllerBase
    {

        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }


        [HttpPost]
        [Route("create-vendor")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public ActionResult CreateVendor([FromBody] CreateVendorDTO vendorDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var jwtToken = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var response = _vendorService.CreateVendor(vendorDTO, jwtToken);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);

        }


        [HttpGet]
        [Route("get-vendors")]
        [Authorize]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllVendors([FromQuery] int page, [FromQuery] int size) {
            var response = _vendorService.GetAllVendors(page, size);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error),response);
        }


        [HttpGet]
        [Route("get-vendor/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public ActionResult GetVendorById(int id)
        {
            var response = _vendorService.GetVendorById(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }



        [HttpPatch]
        [Route("update-vendor/{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateVendors(int id,[FromBody] CreateVendorDTO vendorDTO) {
            if(!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var jwtToken = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var response = _vendorService.UpdateVendor(id, vendorDTO, jwtToken);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpPost]
        [Route("update-vendor-status/{id}")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public ActionResult DeactivateVendor(int id)
        {
            var response = _vendorService.ToogleVendorStatus(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("get-form-creation-data")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public ActionResult GetVendorFormCreationData()
        {
            var response = _vendorService.GetFormData();
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
    }
}




    