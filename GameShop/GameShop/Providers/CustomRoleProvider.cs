using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using GameShop.Models;
using GameShop.DataAccess;

namespace GameShop.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            string[] role = new string[] { };
            using (GameShopContext db = new GameShopContext())
            {
                // Получаем пользователя
                Customer customer = db.Customers.FirstOrDefault(u => u.Login == username);
                if (customer != null)
                {
                    // получаем роль
                    Role userRole = db.Roles.Find(customer.RoleId);
                    if (userRole != null)
                        role = new string[] { userRole.Name };
                }
            }
            return role;
        }
        public override void CreateRole(string roleName)
        {
            Role newRole = new Role() { Name = roleName };
            GameShopContext db = new GameShopContext();
            db.Roles.Add(newRole);
            db.SaveChanges();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя
            using (GameShopContext db = new GameShopContext())
            {
                // Получаем пользователя
                Customer customer = db.Customers.FirstOrDefault(u => u.Email == username);
                if (customer != null)
                {
                    // получаем роль
                    Role userRole = db.Roles.Find(customer.RoleId);
                    //сравниваем
                    if (userRole != null && userRole.Name == roleName)
                        outputResult = true;
                }
            }
            return outputResult;
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}