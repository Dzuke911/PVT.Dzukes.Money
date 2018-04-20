using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PVT.Money.Business.Admin;
using PVT.Money.Business.Tests.FakeClasses;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Tests.AdminTests
{
    [TestFixture]
    public class MoneyRoleManagerTests : BusinessTestMethods
    {
//        private readonly UserManager<ApplicationUser> _userManager;

//        public MoneyRoleManagerTests()
//        {
//            //_userManager = 
//        }

//        [Test]
//        public void AdminFunctionalityChangeRoleOK()
//        {
//            //Arrange
//            const string login = "Sergey2";
//            const string password = "AAAaaa1112";
//            const string email = "Sergey2@gmail.com";
//            const string firstRole = "admin";
//            const string secondRole = "user";
//            UserEntity result;

//            using (DatabaseContext dbContext = GetInMemoryContext())
//            {
//                UserRoleEntity role1 = new UserRoleEntity() { Role = firstRole };
//                UserRoleEntity role2 = new UserRoleEntity() { Role = secondRole };

//                //dbContext.OldRoles.Add(role1);
//                //dbContext.OldRoles.Add(role2);
//                dbContext.SaveChanges();

////                UserEntity user = new UserEntity() { UserName = login, Email = email, Password = password, RoleID = role1.ID };

//                //dbContext.OldUsers.Add(user);
//                dbContext.SaveChanges();

//                //Act
//                //MoneyRoleManager admin = new MoneyRoleManager(dbContext,null,null,null);
//                //admin.ChangeUserRole(login, secondRole);

//                //Assert
//                //result = dbContext.OldUsers.Include(u => u.Role).FirstOrDefault(u => u.UserName == login);
//            }
//            //Assert.AreEqual(secondRole, result.Role.Role);
//        }

//        [Test]
//        public void AdminFunctionalityAddPermissionOK()
//        {
//            //Arrange
//            const string roleName = "newRole";
//            const string permissionName = "CreateRole";
//            //UserRoleEntity resultRole;

//            using (DatabaseContext context = GetInMemoryContext())
//            {
//                //context.OldRoles.Add(new UserRoleEntity() { Role = roleName });
//                context.Permissions.Add(new PermissionEntity() { Permission = permissionName });
//                context.SaveChanges();

//                //Act
//                //AdminFunctionality admin = new AdminFunctionality(context,null,null,null);
//                //admin.AddPermissionToRole(roleName, permissionName);

//                //Assert
//                //resultRole = context.OldRoles.Include(r => r.RolePermissions).FirstOrDefault(r => r.Role == roleName);
//                //foreach (var rp in resultRole.RolePermissions)
//                //{
//                //    context.Entry(rp).Reference(i => i.Permission).Load();
//                //}

//                //Assert.IsTrue(resultRole.RolePermissions.Any(ptr => ptr.Permission.Permission == permissionName));
//            }
//        }

//        [Test]
//        public void AdminFunctionalityRemovePermissionOK()
//        {
//            //Arrange
//            const string roleName = "VerynewRole";
//            const string permissionName = "DeleteRole";
//            //UserRoleEntity resultRole;

//            using (DatabaseContext context = GetInMemoryContext())
//            {
//                //context.OldRoles.Add(new UserRoleEntity() { Role = roleName });
//                context.Permissions.Add(new PermissionEntity() { Permission = permissionName });
//                context.SaveChanges();

//                //AdminFunctionality admin = new AdminFunctionality(context,null,null,null);
//                //admin.AddPermissionToRole(roleName, permissionName);

//                ////Act
//                //admin.RemovePermissionFromRole(roleName, permissionName);

//                //Assert
//                //resultRole = context.OldRoles.Include(r => r.RolePermissions).FirstOrDefault(r => r.Role == roleName);
//                //foreach (var rp in resultRole.RolePermissions)
//                //{
//                //    context.Entry(rp).Reference(i => i.Permission).Load();
//                //}

//                //Assert.IsTrue(!resultRole.RolePermissions.Any(ptr => ptr.Permission.Permission == permissionName));
//            }
//        }

//        [Test]
//        public async Task AdminFunctionalityCreateRole()
//        {
//            //Arrange
//            const string roleName = "JustNewRole";
//            UserRoleEntity resultRole = null;

//            using (DatabaseContext context = GetInMemoryContext())
//            {
//                //MoneyRoleManager admin = new MoneyRoleManager(context,null,null,null);

//                ////Act
//                //bool result = await admin.CreateRole(roleName);

//                //Assert
//                //resultRole = context.OldRoles.FirstOrDefault(r => r.Role == roleName);

//                Assert.NotNull(resultRole);
//                //Assert.True(result);
//            }
//        }
    }
}
