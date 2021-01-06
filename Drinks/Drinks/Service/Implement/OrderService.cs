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

        public Orders Create(Orders order, int storeId)
        {
            string userId = userDao.GetUserId(userName);
            
            var store = storeDao.Get(storeId);
            if (store != null)
            {
                var newOrder = new Orders()
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

        public Orders Delete(int id) 
        {
            var order = orderDao.Get(id);

            if (order != null)
            {
                if (order.OrderDetails.Count > 0)
                {
                    DeleteDetails(order.OrderDetails.ToList());
                }
                orderDao.Delete(order);
            }

            return order;
        }

        private void DeleteDetails(List<OrderDetails> orderDetail) 
        {
            orderDao.DeleteDetails(orderDetail);
        }

        public void DeleteDetails(int id) 
        {
            string userId = userDao.GetUserId(userName);

            var order = orderDao.Get(id);
            var orderDetails = order.OrderDetails.Where(od => od.CreateId == userId).Select(od => od).ToList();
            orderDao.DeleteDetails(orderDetails);
        }

        public List<Orders> GetOrders() 
        {
            DateTime overOneDay = DateTime.Now.AddDays(-1); //撈超過一天以前的飲料團
            return orderDao.Get()
                           .Where(o => overOneDay < o.EndDate)
                           .OrderBy(o => o.EndDate)
                           .ToList();
        }

        public Orders GetOrderById(int id)
        {
            return GetOrders().Where(o => o.Id == id).Select(o => o).FirstOrDefault();
        }

        public Orders GetOrderByUserId(int id, string userId) 
        {
            var order = GetOrderById(id);
            order.OrderDetails = order.OrderDetails.Where(od => od.CreateId == userId).Select(od => od).ToList();
            return order;
        }

        public Orders Save(Orders order) 
        {
            string userId = userDao.GetUserId(userName);
            foreach (var od in order.OrderDetails)
            {
                od.CreateId = userId;
            }

            var dbOrderDetail = orderDao.Get(order.Id).OrderDetails
                                                      .Where(od => od.CreateId == userId)
                                                      .Select(od => od)
                                                      .ToList();
            if (dbOrderDetail.Count > 0)
            {
                orderDao.DeleteDetails(dbOrderDetail);
            }
            orderDao.CreateDetails(order.OrderDetails.ToList());

            return order;
        }

    }
}