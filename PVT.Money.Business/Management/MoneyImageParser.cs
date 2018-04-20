using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PVT.Money.Business.Enums;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageMagick;
using PVT.Money.Business.Extensions;

namespace PVT.Money.Business.Management
{
    class MoneyImageParser : IMoneyImageParser
    {
        protected readonly DatabaseContext _DBContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoneyImageParser(DatabaseContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _DBContext = dbContext;
            _userManager = userManager;
        }

        public ImageCheckResult CheckImage(IFormFile file, long maxBytes)
        {
            if (file == null)
                return ImageCheckResult.NoImage;

            if (file.ContentType != "image/jpeg")
                return ImageCheckResult.IsNotJpeg;

            if (file.Length > maxBytes)
                return ImageCheckResult.MaxSizeError;

            return ImageCheckResult.Success;
        }

        public async Task SaveUserImage(IFormFile file, string userName)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            UserInfoEntity userInfo = await _DBContext.UsersInfo.Include(ui => ui.User).FirstOrDefaultAsync(ui => ui.User == user);

            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                byte[] buffer = ms.ToArray();

                using (MagickImage image = new MagickImage(buffer))
                {
                    MagickGeometry size = new MagickGeometry(100);

                    image.Resize(size);
                    image.Strip();
                    image.Quality = 100;
                    buffer = image.ToByteArray();
                }
                userInfo.Photo = buffer;
            }

            _DBContext.UsersInfo.Update(userInfo);
            await _DBContext.SaveChangesAsync();
        }

        public async Task<byte[]> GetUserImage(string userName, bool isSmall)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            UserInfoEntity userInfo = await _DBContext.UsersInfo.Include(ui => ui.User).FirstOrDefaultAsync(ui => ui.User == user);

            
            byte[] result = userInfo.Photo;

            if (isSmall)
            {
                MagickImage image;
                if (result != null)
                {                    
                    using (image = new MagickImage(result))
                    {
                        image.Resize(42, 42);
                        image.Strip();
                        image.Quality = 100;

                        result = image.ToByteArray();
                    }
                }
                else
                {
                    using (image = new MagickImage("wwwroot/images/DefaultAva.jpg"))
                    {
                        //image.Resize(42, 42);
                        //image.Strip();
                        //image.Quality = 100;
                        result = image.ToByteArray();
                    }
                }                
            }

            return result;
        }
    }
}
