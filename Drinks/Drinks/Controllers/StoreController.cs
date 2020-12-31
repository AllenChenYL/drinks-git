using AutoMapper;
using Drinks.App_Code;
using Drinks.Filters;
using Drinks.Models;
using Drinks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Drinks.Controllers
{
    [LoginAuthorizeFilter]
    public class StoreController : BaseController
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(StoreVM model) 
        {
            Mapper.CreateMap<StoreVM, Store>();
            var result = Mapper.Map<Store>(model);

            using(Models.DrinksEntities db = new DrinksEntities())
            {
                db.Store.Add(result);
                db.SaveChanges();
            }

            return Json(result);
        }

        public JsonResult List() 
        {
            using (Models.DrinksEntities db = new DrinksEntities())
            {
                var model = (from s in db.Store select s).ToList();
                Mapper.CreateMap<Store, StoreVM>();
                var result = Mapper.Map<List<StoreVM>>(model);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(StoreVM model)
        {
            using (Models.DrinksEntities db = new DrinksEntities())
            {
                var dbModel = (from s in db.Store where s.Id == model.Id select s).FirstOrDefault();

                if (dbModel != null)
                {
                    Mapper.CreateMap<StoreVM, Store>();
                    var result = Mapper.Map<Store>(model);
                    dbModel.Name = result.Name;
                    dbModel.Phone = result.Phone;
                    dbModel.Address = result.Address;
                    dbModel.Note = result.Note;
                    dbModel.DefaultImageId = result.DefaultImageId;
                    db.SaveChanges();
                }
                return Json(new { });
            }
        }

        [HttpPost]
        public JsonResult Delete(StoreVM model)
        {
            using (Models.DrinksEntities db = new DrinksEntities())
            {
                var result = (from s in db.Store where s.Id == model.Id select s).FirstOrDefault();
                
                if(result != null)
                {
                    db.Store.Remove(result);
                    db.SaveChanges();
                }
                return Json(new { });
            }
        }

    }
}