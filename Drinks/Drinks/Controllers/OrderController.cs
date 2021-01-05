using AutoMapper;
using Drinks.Models;
using Drinks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Drinks.Filters;
using Drinks.App_Code;
using System.IO;
using System.Reflection;

namespace Drinks.Controllers
{
    [LoginAuthorizeFilter]
    public class OrderController : BaseController
    {
        private Service.Implement.StoreService storeService;
        private Service.Implement.OrderService orderService;
        private Service.Implement.UserService userService;
        public OrderController() 
        {
            storeService = new Service.Implement.StoreService();
            orderService = new Service.Implement.OrderService();
            userService = new Service.Implement.UserService();
        }
        // GET: Order
        public ActionResult Index()
        {
            var stores = storeService.GetStores();
            var result = Mapper.Map<List<StoresVM>>(stores);
            return View(result);
        }

        [HttpPost]
        public JsonResult Create(OrdersVM orderVM, int storeId) 
        {
            var order = Mapper.Map<Orders>(orderVM);
            var result = orderService.Create(order, storeId);

            return Json(result);
        }

        [HttpPost]
        public JsonResult Delete(int id) 
        {
            var result = orderService.Delete(id);

            return Json(result);
        }

        [HttpPost]
        public JsonResult Cancel(int id) 
        {
            orderService.DeleteDetails(id);
            return Json(new { });
        }

        public JsonResult List() 
        {
            var orders = orderService.GetOrders();
            var result = SetGridViewModel(Mapper.Map<List<OrdersVM>>(orders));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<OrdersVM> SetGridViewModel(List<OrdersVM> viewmodel) 
        {
            var users = userService.GetUsers()
                        .Select(u => new { u.Id, u.UserName })
                        .ToDictionary(u => u.Id, u => u.UserName);
            string userId = userService.GetUserId(System.Web.HttpContext.Current.User.Identity.Name);

            foreach (var o in viewmodel)
            {
                o.Creater = users.ContainsKey(o.CreateId) ? users[o.CreateId] : string.Empty;
                o.HasDetail = o.OrderDetails.Where(od => od.CreateId == userId).Count() > 0;
                foreach (var od in o.OrderDetails)
                {
                    od.Orderer = users.ContainsKey(od.CreateId) ? users[od.CreateId] : string.Empty;
                }
            }
            
            return viewmodel;
        }

        public JsonResult GetOrderByUserId(int id)
        {
            var order = orderService.GetOrderById(id);
            var result = Mapper.Map<OrdersVM>(order);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(OrdersVM orderVM)
        {    
            var order = Mapper.Map<Orders>(orderVM);
            var result = orderService.Save(order);

            return Json(result);
        }

        [HttpPost]
        public JsonResult ExportToCsv(int id, string filepath =  @"D:\Drinks.csv") 
        {
            using(DrinksEntities db = new DrinksEntities()) {
                var dbModel = (from s in db.Orders
                               where s.Id == id
                               select s).FirstOrDefault();
                

                using (UserEntities dbUser = new UserEntities())
                {
                    var users = (from s in dbUser.AspNetUsers
                                 select new { s.Id, s.UserName }).ToDictionary(s => s.Id, s => s.UserName);

                    foreach (var od in dbModel.OrderDetails)
                    {
                        od.Orderer = users.ContainsKey(od.CreateId) ? users[od.CreateId] : string.Empty;
                    }
                }

                var result = Mapper.Map<List<OrderDetailsExportVM>>(dbModel.OrderDetails);
                if (dbModel != null) {
                    CSVGenerator<OrderDetailsExportVM>(true, filepath, result);
                }
            }
            return Json(new { });
        }

        void CSVGenerator<T>(bool genColumn, string FilePath, List<T> data)
        {
            using (var file = new StreamWriter(FilePath))
            {
                Type t = typeof(T);
                PropertyInfo[] propInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                //是否要輸出屬性名稱
                if (genColumn)
                {
                    file.WriteLineAsync(string.Join(",", propInfos.Select(i => i.CustomAttributes.First().NamedArguments.First().TypedValue.Value)));
                }
                foreach (var item in data)
                {
                    file.WriteLineAsync(string.Join(",", propInfos.Select(i => i.GetValue(item))));
                }
            }
        }
    }
}