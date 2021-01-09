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
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

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
            return View();
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
            string userId = userService.GetUserId(System.Web.HttpContext.Current.User.Identity.Name);
            var order = orderService.GetOrderByUserId(id, userId);
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

        public void ExportToExcel(int id) 
        {
            var order = orderService.GetOrderById(id);
            var users = userService.GetUsers().ToDictionary(s => s.Id, s => s.UserName);

            foreach (var od in order.OrderDetails)
            {
                od.Orderer = users.ContainsKey(od.CreateId) ? users[od.CreateId] : string.Empty;
            }

            var result = Mapper.Map<List<OrderDetailsExportVM>>(order.OrderDetails);
            var fileBytes = ConvertToExcel(result);

            Response.Expires = 0;
            Response.ExpiresAbsolute = DateTime.UtcNow.AddYears(-1);
            Response.Clear(); 
            Response.ClearHeaders(); 
            Response.Buffer = false;
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", "attachment;" + String.Format("filename={0:yyyyMMdd_HH時mm分}", DateTime.Now) + ".xlsx");
            try
            {
                Response.BinaryWrite(fileBytes);
            }
            catch
            {
                throw new FileNotFoundException("file not found");
            }
            Response.End();
        }

        private byte[] ConvertToExcel(List<OrderDetailsExportVM> od) 
        {
            XSSFWorkbook xssfworkbook = new XSSFWorkbook(); 
            ISheet sheet = xssfworkbook.CreateSheet("訂購人名單");

            sheet.CreateRow(0).CreateCell(0).SetCellValue("訂購人");
            sheet.GetRow(0).CreateCell(1).SetCellValue("飲料名稱");
            sheet.GetRow(0).CreateCell(2).SetCellValue("容量");
            sheet.GetRow(0).CreateCell(3).SetCellValue("甜度");
            sheet.GetRow(0).CreateCell(4).SetCellValue("冰量");
            sheet.GetRow(0).CreateCell(5).SetCellValue("價格");
            sheet.GetRow(0).CreateCell(6).SetCellValue("數量");
            sheet.GetRow(0).CreateCell(7).SetCellValue("備註");

            int rowIndex = 1;
            string orderer = string.Empty;
            int totalByOrderer = 0;
            for (int row = 0; row < od.Count(); row++)
            {
                sheet.CreateRow(rowIndex).CreateCell(0).SetCellValue(orderer == od[row].Orderer ? string.Empty : od[row].Orderer);
                sheet.GetRow(rowIndex).CreateCell(1).SetCellValue(od[row].Name);
                sheet.GetRow(rowIndex).CreateCell(2).SetCellValue(od[row].Size);
                sheet.GetRow(rowIndex).CreateCell(3).SetCellValue(od[row].SugarLevel);
                sheet.GetRow(rowIndex).CreateCell(4).SetCellValue(od[row].IceLevel);
                sheet.GetRow(rowIndex).CreateCell(5).SetCellValue(od[row].Price);
                sheet.GetRow(rowIndex).CreateCell(6).SetCellValue(od[row].Quantity);
                sheet.GetRow(rowIndex).CreateCell(7).SetCellValue(od[row].Memo);
                totalByOrderer += od[row].Price * od[row].Quantity;
                if (rowIndex > 1 && orderer != od[row].Orderer)
                {
                    sheet.GetRow(rowIndex - 1).CreateCell(8).SetCellValue(String.Format("{0}元", totalByOrderer));
                    totalByOrderer = 0;
                }
                rowIndex++;
                orderer = od[row].Orderer;
            }
            sheet.GetRow(rowIndex - 1).CreateCell(8).SetCellValue(String.Format("{0}元", totalByOrderer));

            var ms = new MemoryStream();
            xssfworkbook.Write(ms);
            byte[] bytesInStream = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return bytesInStream;
        }
        
    }
}