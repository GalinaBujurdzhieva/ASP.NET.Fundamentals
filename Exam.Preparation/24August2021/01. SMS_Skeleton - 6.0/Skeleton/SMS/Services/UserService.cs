using SMS.Contracts;
using SMS.Data.Models;
using SMS.Data.Repo;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{

    public class UserService : IUserService
    {
        private readonly IRepository repository;
        private readonly IValidationService validationService;

        public UserService(IRepository _repository, IValidationService _validationService)
        {
            repository = _repository;
            validationService = _validationService;
        }

        public string GetUserName(string userId)
        {
            return repository.All<User>().FirstOrDefault(u => u.Id == userId)?.Username;
        }

        public string Login(UserLoginViewModel model)
        {
            var user = repository.All<User>()
                .FirstOrDefault(x => x.Username == model.Username && x.Password == CalculateHash(model.Password));

            return user?.Id;
        }

        public (bool isRegistered, string error) Register(UserRegisterViewModel model)
        {
            bool isRegistered = false;
            string error = string.Empty;
            
            var (isValid, listOfErrors) = validationService.ValidateModel(model);
            
            if (!isValid)
            {
                return (false, listOfErrors);
            }
            Cart cart = new Cart();
            User user = new User()
            {
                Username = model.Username,
                Email = model.Email,
                Password = CalculateHash(model.Password),
                Cart = cart,
                CartId = cart.Id
            };

            try
            {
                repository.Add(user);
                repository.SaveChanges();
                isRegistered = true;
            }
            catch (Exception)
            {
                error = "Could not save user in Database.";
            }
            return (isRegistered, error);
        }

        private string CalculateHash(string password)
        {
            byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);
            
            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(passwordInBytes));
            }
        }
    }
}
