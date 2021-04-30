using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    public class ResponseBase
    {
        public bool IsError => Errors != null && Errors.Count > 0;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
