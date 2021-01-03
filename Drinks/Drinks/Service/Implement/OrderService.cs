using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.Service.Implement
{
    public class OrderService
    {
        private Dao.Implement.UserDao userDao;
        private Dao.Implement.StoreDao storeDao;
        private Dao.Implement.OrderDao orderDao;
        private string userName;
        public OrderService() {
            userDao = new Dao.Implement.UserDao();
            storeDao = new Dao.Implement.StoreDao();
            orderDao = new Dao.Implement.OrderDao();
            userName = System.Web.HttpContext.Current.User.Identity.Name;
        }

        public Order Create(Order order, int storeId)
        {
            string userId = userDao.GetUserId(userName);
            
            var store = storeDao.Get(storeId);
            if (store != null)
            {
                var newOrder = new Order()
                {
                    CreateDate = DateTime.Now,
                    StoreName = store.Name,
                    StorePhone = store.Phone,
                    StoreAddress = store.Address,
                    DefaultImageId = store.DefaultImageId,
                    EndDate = order.EndDate,
                    CreateId = userId,
                    Note = order.Note
                };
                orderDao.Create(newOrder);
            }
            
            return order;
        }

        public Order Delete(int id) 
        {
            var order = orderDao.Get(id);

            if (order != null)
            {
                if (order.OrderDetail.Count > 0)
                {
                    DeleteDetails(order.OrderDetail.ToList());
                }
                orderDao.Delete(order);
            }

            return order;
        }

        private void DeleteDetails(List<OrderDetail> orderDetail) 
        {
            orderDao.DeleteDetails(orderDetail);
        }

        public void DeleteDetails(int id) 
        {
            string userId = userDao.GetUserId(userName);

            var order = orderDao.Get(id);
            var orderDetails = order.OrderDetail.Where(od => od.CreateId == userId).Select(od => od).ToList();
            orderDao.DeleteDetails(orderDetails);
        }

        public List<Order> GetOrders() 
        {
            DateTime overOneDay = DateTime.Now.AddDays(-1); //撈超過一天以前的飲料團
            return orderDao.Get()
                           .Where(o => overOneDay < o.EndDate)
                           .OrderBy(o => o.EndDate)
                           .ToList();
        }

        public Order GetOrderById(int id)
        {
            string userId = userDao.GetUserId(userName);
            var order = GetOrders().Where(o => o.Id == id).Select(o => o).FirstOrDefault();
            order.OrderDetail = order.OrderDetail.Where(od => od.CreateId == userId).Select(od => od).ToList();
            return order;
        }

        public Order Save(Order order) 
        {
            string userId = userDao.GetUserId(userName);
            foreach (var od in order.OrderDetail)
            {
                od.CreateId = userId;
            }

            var dbOrderDetail = orderDao.Get(order.Id).OrderDetail
                                                      .Where(od => od.CreateId == userId)
                                                      .Select(od => od)
                                                      .ToList();
            if (dbOrderDetail.Count > 0)
            {
                orderDao.DeleteDetails(dbOrderDetail);
            }
            orderDao.CreateDetails(order.OrderDetail.ToList());

            return order;
        }

    }
}