using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Simple
{
    public class ReturnObject
    {
        public ReturnObject()
        {
            Success = true;
        }
        public ReturnObject(string message)
        {
            Success = false;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }
}
