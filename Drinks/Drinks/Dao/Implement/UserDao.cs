using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.Dao.Implement
{
    public class UserDao
    {
        private Models.UserEntities db;
        public UserDao() 
        {
            db = new Models.UserEntities();
        }

        public string GetUserId(string userName) 
        {
            AspNetUsers user = GetUser(userName);
            return user != null? user.Id : "";
        }

        public AspNetUsers GetUser(string userName) 
        {
            return GetUsers()
                   .Where(u => u.UserName == userName)
                   .Select(u => u)
                   .FirstOrDefault();
        }

        public List<AspNetUsers> GetUsers() 
        {
            return (from s in db.AspNetUsers select s).ToList();
        }
    }
}