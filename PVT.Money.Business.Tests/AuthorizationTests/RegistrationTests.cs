using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PVT.Money.Business;
using PVT.Money.Business.Tests.FakeClasses;
using PVT.Money.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PVT.Money.Business.Authorization;
using PVT.Money.Business.Enums;

namespace PVT.Money.Business.Tests
{
    [TestFixture]
    public class RegistrationTests : BusinessTestMethods
    {
        [Test]
        public async Task RegistrationOK()
        {

            //Arrange
            string login = "Sergey";
            string password = "AAAaaa111";
            string email = "Sergey@gmail.com";

            IRegistration registration = new Registration(_userManager, _signInManager, _dbContext);
            IAuthentication authentication = new Authentication(_signInManager, _userManager);

            ApplicationRole role = new ApplicationRole() { Name = "User" };
            await _roleManager.CreateAsync(role);
            await _roleManager.UpdateAsync(role);

            //var prop = _roleManager.GetType().GetProperty("Store", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            //object store = prop.GetValue(_roleManager);
            //object dbcontext1 = store.GetType().GetProperty("Context").GetValue(store);

            ApplicationRole userRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            IEnumerable<ApplicationRole> roles = _dbContext.Roles;

            //Act
            RegistrationResult regResult = await registration.RegisterAsync(login, email, password);

            //Assert
            ApplicationUser resultUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == login);
            Assert.AreEqual(RegistrationResult.Success,regResult);
            Assert.NotNull(resultUser);
        }

        //[Test]
        //public async Task AuthorizationEmailOK()
        //{
        //    //Arrange
        //    string login = "Sergey2";
        //    string password = "AAAaaa1112";
        //    string email = "Sergey2@gmail.com";
        //    User userCheck = null;

        //    using (DatabaseContext dbContext = GetInMemoryContext())
        //    {
        //        UserRoleEntity role = new UserRoleEntity() { Role = "blabla" };

        //        //dbContext.OldRoles.Add(role);
        //        dbContext.SaveChanges();

        //        dbContext.OldUsers.Add(new UserEntity() { UserName = login, Email = email, Password = password, RoleID = role.ID });
        //        dbContext.SaveChanges();

        //        List<UserEntity> users = dbContext.OldUsers.Include(u => u.Role).ToList();

        //        //Act
        //        Authentication autentification = new Authentication(dbContext,null);
        //        userCheck = await autentification.CheckAuthentication(email, password);

        //        //Assert
        //        Assert.NotNull(userCheck);
        //    }
        //}

        //[Test]
        //public async Task AuthorizationFailed()
        //{
        //    //Arrange
        //    string login = "Sergey1";
        //    string password = "AAAaaa1111";
        //    string email = "Sergey1@gmail.com";
        //    User userCheck = null;

        //    using (DatabaseContext dbContext = GetInMemoryContext())
        //    {
        //        UserRoleEntity role = new UserRoleEntity() { Role = "blabla" };

        //        //dbContext.OldRoles.Add(role);
        //        dbContext.SaveChanges();

        //        dbContext.OldUsers.Add(new UserEntity() { UserName = login, Email = email, Password = password, RoleID = role.ID });
        //        dbContext.SaveChanges();

        //        List<UserEntity> users = dbContext.OldUsers.Include(u => u.Role).ToList();

        //        //Act
        //        Authentication autentification = new Authentication(dbContext,null);
        //        userCheck = await autentification.CheckAuthentication(login+"1", password);

        //        //Assert
        //        Assert.Null(userCheck);
        //    }            
        //}
    }
}
