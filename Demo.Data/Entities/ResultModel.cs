using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Entities
{
    public class ResultModel
    {
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
        public object ResponseFailed { get; set; }
    }
    public class ResultMessage
    {
        public bool IsSuccessStatus { get; set; }
        public object Response { get; set; }
    }
}
