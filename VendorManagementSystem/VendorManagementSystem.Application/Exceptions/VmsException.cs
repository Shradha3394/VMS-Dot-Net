using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Exceptions
{
    public class VmsException: Exception
    {
        public int ErrorCode { get; }
        public VmsException(int errorcode, string message): base(message)
        {
            ErrorCode = errorcode;
        }
        public VmsException(int errorcode, string message, Exception ex) : base(message, ex)
        {
            ErrorCode = errorcode;
        }
    }
}
