using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.Dao.Implement
{
    public class OrderDao
    {
        private Models.DrinksEntities db;
        public OrderDao()
        {
            db = new Models.DrinksEntities();
        }

        public void Create(Orders order) 
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public void CreateDetails(List<OrderDetails> orderDetail)
        {
            db.OrderDetails.AddRange(orderDetail);
            db.SaveChanges();
        }

        public void Delete(Orders order) 
        {
            db.Orders.Remove(order);
            db.SaveChanges();
        }

        public void DeleteDetails(List<OrderDetails> orderDetail) 
        {
            db.OrderDetails.RemoveRange(orderDetail);
            db.SaveChanges();
        }

        public Orders Get(int id) 
        {
            return Get().Where(o => o.Id == id).Select(o => o).FirstOrDefault();
        }

        public List<Orders> Get() 
        {
            return (from s in db.Orders select s).ToList();
        }

    }
}