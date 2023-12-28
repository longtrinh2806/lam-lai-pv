using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.ViewModel
{
    public class ResponseModel
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
