using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.Service.Implement
{
    public class StoreService
    {
        private Dao.Implement.StoreDao storeDao;
        public StoreService() 
        {
            storeDao = new Dao.Implement.StoreDao();
        }

        public Stores CreateStore(Stores store) 
        {
            storeDao.Create(store);
            return store;
        }

        public List<Stores> GetStores() 
        {
            var result = storeDao.Get();
            return result;
        }

        public Stores UpdateStore(Stores store) 
        {
            storeDao.Update(store);
            return store;
        }

        public Stores DeleteStore(Stores store) 
        {
            storeDao.Delete(store);
            return store;
        }

    }
}