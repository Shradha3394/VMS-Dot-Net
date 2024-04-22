using Microsoft.IdentityModel.Tokens;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;

namespace VendorManagementSystem.Application.Validator
{
    public static class LoginDTOValidator
    {
        /*public string Email { get; set; }
        public string Password { get; set; }*/

        public static ValidityCheckDTO validate(LoginDTO loginDto)
        {
            ValidityCheckDTO validity = new();

            if (loginDto.Email.IsNullOrEmpty())
            {
                validity.status = false;
                validity.AddError(nameof(loginDto.Email), "Email is null or Empty");

                if (!Helpers.isValidEmail(loginDto.Email))
                {
                    validity.status = false;
                    validity.AddError(nameof(loginDto.Email), "Email is Invalid");
                }
            }

            if(loginDto.Password.IsNullOrEmpty())
            {
                validity.status = false;
                validity.AddError(nameof(loginDto.Password), "Password is null or Empty");
            }

            return validity;
        }
    }
}
