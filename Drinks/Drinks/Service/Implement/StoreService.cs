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

        public Store CreateStore(Store store) 
        {
            storeDao.Create(store);
            return store;
        }

        public List<Store> GetStores() 
        {
            var result = storeDao.Get();
            return result;
        }

        public Store UpdateStore(Store store) 
        {
            storeDao.Update(store);
            return store;
        }

        public Store DeleteStore(Store store) 
        {
            storeDao.Delete(store);
            return store;
        }

    }
}