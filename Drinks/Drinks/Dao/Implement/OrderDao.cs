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

        public void Create(Order order) 
        {
            db.Order.Add(order);
            db.SaveChanges();
        }

        public void CreateDetails(List<OrderDetail> orderDetail)
        {
            db.OrderDetail.AddRange(orderDetail);
            db.SaveChanges();
        }

        public void Delete(Order order) 
        {
            db.Order.Remove(order);
            db.SaveChanges();
        }

        public void DeleteDetails(List<OrderDetail> orderDetail) 
        {
            db.OrderDetail.RemoveRange(orderDetail);
            db.SaveChanges();
        }

        public Order Get(int id) 
        {
            return Get().Where(o => o.Id == id).Select(o => o).FirstOrDefault();
        }

        public List<Order> Get() 
        {
            return (from s in db.Order select s).ToList();
        }

    }
}