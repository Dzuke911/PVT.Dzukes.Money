using PVT.Money.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Authorization
{
    public interface IRegistration
    {
        Task<RegistrationResult> RegisterAsync(string login, string email, string password);
        Task<RegistrationResult> RegisterAsync(string login, string email);
        Task<bool> EmailAlreadyExistsAsync(string email);
        Task<bool> LoginAlreadyExistsAsync(string login);
    }
}
