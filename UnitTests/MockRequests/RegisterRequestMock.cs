using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels.Requests;

namespace UnitTests.MockRequests
{
    public class RegisterRequestMock
    {
        RegisterRequest registerRequest = new RegisterRequest()
        {
            Email = "test2@mail.com",
            Username = "test2",
            Password = "123",
            ConfirmPassword = "123",
            Roles = { "admin", "user" }
        };

    }
}
