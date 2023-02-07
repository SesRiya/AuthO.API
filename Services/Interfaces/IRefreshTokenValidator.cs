using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels.Requests;
using WebModels;
using WebModels.Responses;

namespace Services.Interfaces
{
    public interface IRefreshTokenValidator
    {
        bool Validate(string refreshToken);


    }
}
