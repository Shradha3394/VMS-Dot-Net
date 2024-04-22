using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.IServices;
using Microsoft.IdentityModel.Tokens;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.API.Utilities;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult LoggedIn() {
            var jwtToken = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var response = _userService.ValidateToken(jwtToken!);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if(!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _userService.Login(loginDTO);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("create-user")]
        [Authorize(Roles = "superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateUser([FromBody] CreateUserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }

            var jwtToken = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var response = _userService.CreateUser(userDto, jwtToken!);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("get-users")]
        [Authorize(Roles = "superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(  StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllUsers()
        {
            var response = _userService.GetAllUsers();

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("update-password/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult SetUserPassword([FromBody] UpdatePasswordDTO updatePasswordDTO, string token)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            if (token.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApplicationResponseDTO<object>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InvalidInputFields,
                        Message = new List<string> { $"{nameof(token)} is Empty or null" },
                    }
                });
            }
            var response = _userService.SetUserPassword(updatePasswordDTO, token);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("validate-user/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ValidateNewUserToken(string token)
        {
            if (token.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApplicationResponseDTO<object>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InvalidInputFields,
                        Message = new List<string> { $"{nameof(token)} is Empty or null" },
                    }
                });
            }
            var response = _userService.ValidateToken(token);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("forget-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult SendForgetPasswordEmail([FromBody] ForgotPasswordDTO forgetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _userService.SendForgetPasswordEmail(forgetPasswordDTO.Email, forgetPasswordDTO.RedirectUrl);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        // Delete this
        [HttpPost]
        [Route("create-superadmin")]
        public ActionResult CreateSuperUser([FromBody] SuperAdminDTO superuserDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _userService.CreateSuperAdmin(superuserDto);
            return Ok(response);
        }
    }
}
