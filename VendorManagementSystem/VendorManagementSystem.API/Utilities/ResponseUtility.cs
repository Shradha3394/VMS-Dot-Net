using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VendorManagementSystem.API.Utilities
{
    public static class ResponseUtility
    {
        public static int GetStatusCode(Error? Error)
        {
            if (Error != null)
            {
                if (Error.Code == (int)ErrorCodes.DatabaseError || Error.Code == (int)ErrorCodes.InternalError)
                {
                    return StatusCodes.Status500InternalServerError;
                }
                else if (Error.Code == (int)ErrorCodes.NotFound)
                {
                    return StatusCodes.Status404NotFound;
                }
                return StatusCodes.Status400BadRequest;
            }
            else
            {
                return StatusCodes.Status200OK;
            }
        }


        public static ApplicationResponseDTO<object> ModelError(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorResponse = new ApplicationResponseDTO<object>
            {
                Error = new Error
                {
                    Code = (int)ErrorCodes.InvalidInputFields,
                    Message = errors,
                }
            };
            return errorResponse;
        }
    }
}
