using Microsoft.AspNetCore.Http;
using PVT.Money.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Management
{
    public interface IMoneyImageParser
    {
        ImageCheckResult CheckImage(IFormFile file, long maxBytes);
        Task SaveUserImage(IFormFile file, string userName);
        Task<byte[]> GetUserImage(string userName, bool isSmall);
    }
}
