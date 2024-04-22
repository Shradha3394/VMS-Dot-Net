using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VendorManagementSystem.Application.Utilities
{
    public static class EmailUtility
    {
        public static string GetInvitationBody(string toName, string fromName, string link)
        {
            return @$"<p>Hello {toName},</p>
                        <p>You have been invited to join Ex Squared Vendor Management System by {fromName}</p>
                        <p>Please click on the following link to set your password and get started:</p>
                        <a href={link}>Generate New Password</a>
                        <p>Best Regards,</p>
                        <p>VMS team</p>";
        }
        public static string ForgetPasswordBody(string toName, string link)
        {
            return @$"<p>Hello {toName},</p
                        <p>Please click on the following link to reset your password and get started:</p>
                        <a href={link}>Generate New Password</a>
                        <p>Best Regards,</p>
                        <p>VMS team</p>";
        }
    }
}
