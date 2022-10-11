using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Contracts
{
    public interface IUserService
    {
        string GetUserName(string userId);

        (bool isRegistered, string error) Register(UserRegisterViewModel model);

        string Login(UserLoginViewModel model);
    }
}
