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
        private Service.Implement.StoreService storeService;
        public StoreController() 
        {
            storeService = new Service.Implement.StoreService();
        }

        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(StoresVM storeVM) 
        {
            var store = Mapper.Map<Stores>(storeVM);
            var result = storeService.CreateStore(store);

            return Json(result);
        }

        public JsonResult List() 
        {
            var stores = storeService.GetStores();
            var result = Mapper.Map<List<StoresVM>>(stores);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(StoresVM storeVM)
        {
            var store = Mapper.Map<Stores>(storeVM);
            var result = storeService.UpdateStore(store);

            return Json(result);
        }

        [HttpPost]
        public JsonResult Delete(StoresVM storeVM)
        {
            var store = Mapper.Map<Stores>(storeVM);
            var result = storeService.DeleteStore(store);

            return Json(result);
        }

    }
}