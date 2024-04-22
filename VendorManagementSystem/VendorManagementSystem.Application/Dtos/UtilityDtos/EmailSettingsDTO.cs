using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class EmailSettingsDTO
    {
        public string From { get; set; }
        public string FromName { get; set; }
        public string AuthEmail { get; set; }
        public string AuthKey { get; set; }
        public string SmtpClient { get; set; }
        public int Port { get; set; }
    }
}
