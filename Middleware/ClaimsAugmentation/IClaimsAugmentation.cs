using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.ClaimsAugmentation
{
    public interface IClaimsAugmentation
    {       
        Task InvokeAsync(HttpContext context);


    }
}
