﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class EmailDetailsDTO
    {
        //string fromName, string toName, string toAddress, string link
        public string ToAddress { get; set; }
        public string ToName { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
