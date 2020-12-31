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
        // GET: Order
        public ActionResult Index()
        {
            using (Models.DrinksEntities db = new Models.DrinksEntities())
            {
                var dbModel = (from s in db.Store select s).ToList();
                
                var result = Mapper.Map<List<StoreVM>>(dbModel);
                return View(result);
            }
        }

        [HttpPost]
        public JsonResult Create(OrderVM model) 
        {
            using (Models.DrinksEntities db = new DrinksEntities()) 
            {
                var dbStore = (from s in db.Store where s.Id == model.StoreId select s).FirstOrDefault();
                if (dbStore != null)
                {
                    var userId = HttpContext.User.Identity.GetUserId();
                    var order = new Order()
                    {
                        CreateDate = DateTime.Now,
                        StoreName = dbStore.Name,
                        StorePhone = dbStore.Phone,
                        StoreAddress = dbStore.Address,
                        DefaultImageId = dbStore.DefaultImageId,
                        EndDate = model.EndDate,
                        CreateId = userId,
                        Note = model.Note
                    };
                    db.Order.Add(order);
                    db.SaveChanges();
                }
            }
            return Json(new { });
        }

        [HttpPost]
        public JsonResult Delete(int id) 
        {
            using (DrinksEntities db = new DrinksEntities())
            {
                var dbModel = (from s in db.Order
                               where s.Id == id 
                               select s).FirstOrDefault();
                
                if (dbModel != null) {
                    if (dbModel.OrderDetail.Count > 0) {
                        db.OrderDetail.RemoveRange(db.OrderDetail);
                        db.SaveChanges();
                    }
                    db.Order.Remove(dbModel);
                    db.SaveChanges();
                }
            }
            return Json(new { });
        }

        [HttpPost]
        public JsonResult Cancel(int id) 
        {
            var userId = HttpContext.User.Identity.GetUserId();
            using (DrinksEntities db = new DrinksEntities())
            {
                var dbModel = (from s in db.OrderDetail
                               where s.OrderId == id && s.CreateId == userId 
                               select s).ToList();
                
                if (dbModel != null)
                {
                    db.OrderDetail.RemoveRange(dbModel);
                    db.SaveChanges();
                }
            }
            return Json(new { });
        }

        public JsonResult List() 
        {
            using(DrinksEntities db = new DrinksEntities())
            {
                DateTime overOneDay = DateTime.Now.AddDays(-1); //撈超過一天以前的飲料團
                var model = (from s in db.Order
                             where overOneDay < s.EndDate
                             select s).ToList().OrderBy(s => s.EndDate);

                var result = SetGridViewModel(Mapper.Map<List<OrderVM>>(model));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private List<OrderVM> SetGridViewModel(List<OrderVM> viewmodel) 
        {
            var userId = HttpContext.User.Identity.GetUserId();
            using(UserEntities db = new UserEntities())
            {
                var users = (from s in db.AspNetUsers 
                             select new{s.Id, s.UserName}).ToDictionary(s => s.Id, s => s.UserName);

                foreach (var o in viewmodel) {
                    o.Creater = users.ContainsKey(o.CreateId) ? users[o.CreateId] : string.Empty;
                    o.HasDetail = o.OrderDetail.Where(od => od.CreateId == userId).Count() > 0;
                    foreach (var od in o.OrderDetail) {
                        od.Orderer = users.ContainsKey(od.CreateId) ? users[od.CreateId] : string.Empty;
                    }
                }
            }
            return viewmodel;
        }

        public JsonResult GetDetailByUser(int id)
        {
            var userId = HttpContext.User.Identity.GetUserId();
            using (DrinksEntities db = new DrinksEntities())
            {
                var model = (from s in db.Order
                             where s.Id == id
                             select s).FirstOrDefault();
                model.OrderDetail = model.OrderDetail.Where(od => od.CreateId == userId).ToList();

                var result = Mapper.Map<OrderVM>(model);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Save(OrderVM viewmodel)
        {
            var model = Mapper.Map<Order>(viewmodel);
            var userId = HttpContext.User.Identity.GetUserId();

            foreach(var od in model.OrderDetail) {
                od.CreateId = userId;
            }

            using (DrinksEntities db = new DrinksEntities())
            {
                var dbModel = (from s in db.OrderDetail
                               where s.OrderId == model.Id && s.CreateId == userId
                               select s).ToList();
                
                if (dbModel.Count > 0)
                {
                    db.OrderDetail.RemoveRange(dbModel);
                    db.SaveChanges();
                }
                db.OrderDetail.AddRange(model.OrderDetail);
                db.SaveChanges();
            }
            return Json(new { });
        }

        [HttpPost]
        public JsonResult ExportToCsv(int id, string filepath =  @"D:\Drinks.csv") 
        {
            using(DrinksEntities db = new DrinksEntities()) {
                var dbModel = (from s in db.Order
                               where s.Id == id
                               select s).FirstOrDefault();
                

                using (UserEntities dbUser = new UserEntities())
                {
                    var users = (from s in dbUser.AspNetUsers
                                 select new { s.Id, s.UserName }).ToDictionary(s => s.Id, s => s.UserName);

                    foreach (var od in dbModel.OrderDetail)
                    {
                        od.Orderer = users.ContainsKey(od.CreateId) ? users[od.CreateId] : string.Empty;
                    }
                }

                var result = Mapper.Map<List<OrderDetailExportVM>>(dbModel.OrderDetail);
                if (dbModel != null) {
                    CSVGenerator<OrderDetailExportVM>(true, filepath, result);
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