using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.Service.Implement
{
    public class UserService
    {
        private Dao.Implement.UserDao userDao;
        public UserService() 
        {
            userDao = new Dao.Implement.UserDao();
        }

        public List<AspNetUsers> GetUsers() 
        {
            return userDao.GetUsers();
        }

        public string GetUserId(string userName) 
        {
            return userDao.GetUserId(userName);
        }
    }
}