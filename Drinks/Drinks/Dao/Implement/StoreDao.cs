using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.Dao.Implement
{
    public class StoreDao
    {
        private Models.DrinksEntities db;
        public StoreDao() 
        {
            db = new DrinksEntities();
        }

        public void Create(Store store) 
        {
            db.Store.Add(store);
            db.SaveChanges();
        }

        public List<Store> Get() 
        {
            List<Store> result = (from s in db.Store select s).ToList();
            return result;
        }

        public void Update(Store store) 
        {
            var dbModel = (from s in db.Store where s.Id == store.Id select s).FirstOrDefault();

            if (dbModel != null)
            {
                dbModel.Name = store.Name;
                dbModel.Phone = store.Phone;
                dbModel.Address = store.Address;
                dbModel.Note = store.Note;
                dbModel.DefaultImageId = store.DefaultImageId;
                db.SaveChanges();
            }
        }

        public Store Get(int id) 
        {
            return (from s in db.Store
                    where s.Id == id
                    select s).FirstOrDefault();
        }

        public void Delete(Store store) 
        {
            var dbModel = Get(store.Id);
            if (dbModel != null)
            {
                db.Store.Remove(dbModel);
                db.SaveChanges();
            }
        }

    }
}