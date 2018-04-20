using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;

namespace PVT.Money.Data.Tests
{
    [TestFixture]
    [Category("IntegrationTests")]
    public class DatabaseTests
    {
        public DatabaseTests()
        {
            DatabaseFacade.InitDatabase("Server = (localdb)\\MSSQLLocalDB; Database = MoneyTestDatabase; Integrated Security = true;");
        }

        private DatabaseContext getDBContext()
        {
            DbContextOptionsBuilder<DatabaseContext> contextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            contextOptionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = MoneyTestDatabase; Integrated Security = true;");
            DbContextOptions<DatabaseContext> options = contextOptionsBuilder.Options;
            return new DatabaseContext(options);
        }

        [Test]
        public void UsersTableExists()
        {
            using (var context = getDBContext())
            {
                //Arrange
                

                //Act
                //var users = context.OldUsers.ToArray();

                //Assert
                //Assert.IsNotNull(users);
            }
        }

        [Test]
        public void PermissionsTableExists()
        {
            using (var context = getDBContext())
            {
                //Arrange

                //Act
                var permissions = context.Permissions.ToArray();

                //Assert
                Assert.IsNotNull(permissions);
            }
        }

        [Test]
        public void RolesTableExists()
        {
            using (var context = getDBContext())
            {
                //Arrange

                //Act
                //var roles = context.OldRoles.ToArray();

                //Assert
                //Assert.IsNotNull(roles);
            }
        }

        [Test]
        public void UsersInfoTableExists()
        {
            using (var context = getDBContext())
            {
                //Arrange

                //Act
                var roles = context.UsersInfo.ToArray();

                //Assert
                Assert.IsNotNull(roles);
            }
        }

        [Test]
        public void AddUser()
        {
            const string roleName = "111";
            const string str = "222";

            using (var context = getDBContext())
            {
                //Arrange
                context.Database.ExecuteSqlCommand("DELETE FROM Users");
                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");

                UserRoleEntity userRole = new UserRoleEntity() { Role = roleName };
                //context.OldRoles.Add(userRole);
                context.SaveChanges();

                //Act
//                UserEntity userEntity = new UserEntity() { UserName = str, Email = str, Password = str, RoleID = userRole.ID };
                //context.OldUsers.Add(userEntity);
                context.SaveChanges();

                //UserEntity newUser = context.OldUsers.Include(u => u.Role).SingleOrDefault(u => u.UserName == userEntity.UserName);

                //Assert
                //Assert.AreEqual(userEntity.UserName, newUser.UserName);
               // Assert.AreEqual(userEntity.Password, newUser.Password);
                //Assert.AreEqual(userEntity.Email, newUser.Email);
                //Assert.AreEqual(userEntity.RoleID, newUser.Role.ID);

                context.Database.ExecuteSqlCommand("DELETE FROM Users");
                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");                
            }
        }

        [Test]
        public void AddRole()
        {
            const string roleName = "111";

            using (var context = getDBContext())
            {
                //Arrange
                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");

                //Act
                UserRoleEntity userRole = new UserRoleEntity() { Role = roleName };
                //context.OldRoles.Add(userRole);
                context.SaveChanges();

                //UserRoleEntity newRole = context.OldRoles.SingleOrDefault(r => r.ID == userRole.ID);

                //Assert
                //Assert.AreEqual(userRole.Role, newRole.Role);

                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");
            }
        }

        [Test]
        public void AddPermission()
        {
            const string permissionName = "111";

            using (var context = getDBContext())
            {
                //Arrange
                context.Database.ExecuteSqlCommand("DELETE FROM Permissions");

                //Act
                PermissionEntity permission = new PermissionEntity() { Permission = permissionName };
                context.Permissions.Add(permission);
                context.SaveChanges();

                PermissionEntity newPermission = context.Permissions.SingleOrDefault(p => p.ID == permission.ID);

                //Assert
                Assert.AreEqual(permission.Permission, newPermission.Permission);

                context.Database.ExecuteSqlCommand("DELETE FROM Permissions");
            }
        }

        [Test]
        public void AddPermissionsToRoleRelations()
        {
            const string roleName = "111";
            const string permissionName = "222";

            using (var context = getDBContext())
            {
                //Arrange
                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");
                context.Database.ExecuteSqlCommand("DELETE FROM Permissions");

                PermissionEntity permission = new PermissionEntity() { Permission = permissionName };
                context.Permissions.Add(permission);
                UserRoleEntity role = new UserRoleEntity() { Role = roleName };
                //context.OldRoles.Add(role);

                context.SaveChanges();

                //Act
                //role.RolePermissions = new List<PermissionToRoleEntity>();
                //role.RolePermissions.Add(new PermissionToRoleEntity() { Permission = permission });
                //context.OldRoles.Update(role);
                context.SaveChanges();

                //UserRoleEntity newRole = context.OldRoles.Include(r => r.RolePermissions).SingleOrDefault(r => r.ID == role.ID);
                PermissionEntity newPermission = context.Permissions.Include(p => p.PermissionRoles).SingleOrDefault(p => p.ID == permission.ID);

                //PermissionEntity rolePermission = newRole.RolePermissions.SingleOrDefault(p => p.Permission.ID == permission.ID).Permission;
         //       UserRoleEntity permissionRole = newPermission.PermissionRoles.SingleOrDefault(p => p.Role.ID == role.ID).Role;

                //Assert
                //Assert.AreEqual(permissionName, rolePermission.Permission);
                //Assert.AreEqual(roleName, permissionRole.Role);

                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");
                context.Database.ExecuteSqlCommand("DELETE FROM Permissions");
            }
        }

        [Test]
        public void AddUserInfo()
        {
            const string roleName = "111";
            const string str = "222";
            const string fName = "fghfghfgh";
            const string lName = "dfgdfg";
            const string address = "asdasd";
            const string phone = "5465464";
            const string gender = "male";
            DateTime date = new DateTime(1985, 7, 20);

            using (var context = getDBContext())
            {
                //Arrange
                context.Database.ExecuteSqlCommand("DELETE FROM Users");
                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");
                context.Database.ExecuteSqlCommand("DELETE FROM UsersInfo");

                UserRoleEntity userRole = new UserRoleEntity() { Role = roleName };
                //context.OldRoles.Add(userRole);
                context.SaveChanges();
                
//                UserEntity userEntity = new UserEntity() { UserName = str, Email = str, Password = str, RoleID = userRole.ID };
                //context.OldUsers.Add(userEntity);
                context.SaveChanges();

                //Act
                UserInfoEntity uInfo = new UserInfoEntity()
                {
                    FirstName = fName,
                    LastName = lName,
                    Address = address,
                    Phone = phone,
                    Gender = gender,
                    BirthDate = date,
                    //User = context.OldUsers.Include(u => u.Role).SingleOrDefault(u => u.UserName == str)
                };

                context.UsersInfo.Add(uInfo);
                context.SaveChanges();

                UserInfoEntity resultInfo = context.UsersInfo.Include(ui => ui.User).FirstOrDefault(ui => ui.User.UserName == str);

                //Assert
                Assert.AreEqual(address, resultInfo.Address);
                Assert.AreEqual(date, resultInfo.BirthDate);
                Assert.AreEqual(fName, resultInfo.FirstName);
                Assert.AreEqual(gender, resultInfo.Gender);
                Assert.AreEqual(lName, resultInfo.LastName);
                Assert.AreEqual(phone, resultInfo.Phone);

                context.Database.ExecuteSqlCommand("DELETE FROM Users");
                context.Database.ExecuteSqlCommand("DELETE FROM UserRoles");
                context.Database.ExecuteSqlCommand("DELETE FROM UsersInfo");
            }
        }
    }
}
