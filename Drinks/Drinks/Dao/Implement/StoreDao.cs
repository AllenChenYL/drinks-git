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

        public void Create(Stores store) 
        {
            db.Stores.Add(store);
            db.SaveChanges();
        }

        public List<Stores> Get() 
        {
            List<Stores> result = (from s in db.Stores select s).ToList();
            return result;
        }

        public void Update(Stores store) 
        {
            var dbModel = (from s in db.Stores where s.Id == store.Id select s).FirstOrDefault();

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

        public Stores Get(int id) 
        {
            return (from s in db.Stores
                    where s.Id == id
                    select s).FirstOrDefault();
        }

        public void Delete(Stores store) 
        {
            var dbModel = Get(store.Id);
            if (dbModel != null)
            {
                db.Stores.Remove(dbModel);
                db.SaveChanges();
            }
        }

    }
}