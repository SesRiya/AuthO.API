using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Responses
{
    public record ErrorResponse
    {
        public IEnumerable<string> ErrorMessages { get; set; }

        public ErrorResponse(string errorMessage) : this(new List<string>() { errorMessage })
        {
        }
        public ErrorResponse(IEnumerable<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
