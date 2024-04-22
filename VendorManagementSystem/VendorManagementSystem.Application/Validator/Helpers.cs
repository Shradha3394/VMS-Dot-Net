using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Validator
{
    internal static class Helpers
    {
        public static bool IsNull<T>(T value)
        {
            return value == null;
        }

        public static bool isValidEmail(string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
    }
}
