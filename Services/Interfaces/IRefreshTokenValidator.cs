using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Requests;
using Models;
using Models.Responses;

namespace Services.Interfaces
{
    public interface IRefreshTokenValidator
    {
        bool Validate(string refreshToken);
    }
}
